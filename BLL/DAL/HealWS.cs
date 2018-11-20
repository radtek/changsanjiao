using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Readearth.Data;

namespace MMShareBLL.DAL
{
     class HealWS
    {
        Database m_database;
        public string GetCrows(string authCode)
        {
            string lastTime = "";  //最新发布的时间
            string period = "";   //时效
            m_database = new Database("DBCONFIG");
            //从T_ServiceInterfaceMag表中根据key值获取区域、以及产品
            DataTable result = new DataTable();
            DataTable dt_Service = new DataTable();
            DataTable dt_HealTime = new DataTable();
            DataTable dt_HealAuto = new DataTable();
            DataTable dt_HealWeather = new DataTable();
            dt_HealWeather.TableName = "tmp";
            result.TableName = "resultName";
            string sql_Service = "SELECT region,product FROM T_ServiceInterfaceMag WHERE SECRETKEY='" + authCode + "'";
            dt_Service = m_database.GetDataTable(sql_Service);
            if (dt_Service.Rows.Count > 0 && dt_Service != null)
            {
                string product = "";
                #region  处理产品类型
                foreach (DataRow dr in dt_Service.Rows)
                {
                    string str = "";
                    foreach (string temp in dr["product"].ToString().Split('_'))
                    {
                        switch (temp)
                        {
                            case "2": str += "儿童感冒_"; break;
                            case "3": str += "青年感冒_"; break;
                            case "4": str += "老人感冒_"; break;
                            case "5": str += "COPD_"; break;
                            case "6": str += "儿童哮喘_"; break;
                            case "7": str += "中暑_"; break;
                            case "8": str += "重污染_"; break;
                        }
                    }
                    product = str.TrimEnd('_');
                    dr["product"] = product;
                }
                #endregion  产品类型结束
                #region   处理区域
                string servivrRegion = "";
                foreach (DataRow dr in dt_Service.Rows)
                {
                    string[] str = dr["region"].ToString().Trim().Split(',');
                    foreach (string temp in str)
                    {
                        servivrRegion += "'" + temp + "',";
                    }
                    servivrRegion = servivrRegion.TrimEnd(',');
                }
                #endregion  区域处理结束


                //从[T_HealthyAuto]表中获取最新发布的时间，时效

                string sql_HealTime = "SELECT TOP 1 PERIOD,CREATETIME FROM [T_HealthyAuto] ORDER BY CREATETIME DESC,PERIOD DESC";
                dt_HealTime = m_database.GetDataTable(sql_HealTime);
                foreach (DataRow dr in dt_HealTime.Rows)
                {
                    lastTime = dr["CREATETIME"].ToString();
                    period = dr["PERIOD"].ToString();
                }

                //根据上面获取的最新时间、时效、是否一键发送来获取forecaster、region

                string sql_HealAuto = "SELECT forecaster,REGION FROM [T_HealthyAuto] WHERE CREATETIME='" + lastTime + "' AND PERIOD='" + period + "' AND TYPE='一键发送'";
                dt_HealAuto = m_database.GetDataTable(sql_HealAuto);
                #region 处理T_HealthyAuto中得到的region
                string[] region;
                string healRegion = "";
                foreach (DataRow dr in dt_HealAuto.Rows)
                {
                    region = dr["region"].ToString().Trim().Split('_');
                    foreach (string str in region)
                    {
                        healRegion += "'" + str + "',";
                    }
                    healRegion = healRegion.TrimEnd(',');
                }
                #endregion  region处理结束

                #region  处理forecaster
                string manager = "";
                foreach (DataRow dr in dt_HealAuto.Rows)
                {
                    manager = dr["forecaster"].ToString();
                }
                #endregion  forecaster处理结束

                //根据forecaster、最新时间、时效、station、type从t_healthyweather表中查询ID（自增）、type、时间、level、people、 Premunition

                #region   处理T_ServiceInterfaceMag表中获取的产品类型
                string[] type = product.Split('_');
                string healType = "";
                for (int i = 0; i < type.Length; i++)
                {
                    healType += "'" + type[i] + "',";
                }
                healType = healType.TrimEnd(',');
                #endregion  产品类型处理结束

                string time = (DateTime.Parse(lastTime)).ToString("yyyy-MM-dd 00:00:00");
                string sql_HealWeather = "SELECT RANK() OVER(ORDER BY ID) AS ID,station,TYPE AS Crow,LST AS Date,Level AS WarningLevel,Level AS WarningDesc,People AS Influ,premunition AS Wat_guide FROM t_healthyweather WHERE station IN (" + healRegion + ") AND forecastdate='" + time + "' AND TYPE IN (" + healType + ") AND PERIOD='" + period + "' AND FORECASTER='" + manager + "' order by Station ASC,type ASC,LST ASC";
                dt_HealWeather = m_database.GetDataTable(sql_HealWeather);
                #region  处理等级
                string level = "";
                foreach (DataRow dr in dt_HealWeather.Rows)
                {
                    level = GetWarnLevel(dr["WarningLevel"].ToString().Trim());
                    dr["WarningLevel"] = level;
                }
                #endregion  等级处理结束
                //根据T_ServiceInterfaceMag表中获得的区域再次筛选dt_HealWeather
                string condition = "station in (" + servivrRegion + ")";
                DataRow[] dr2 = dt_HealWeather.Select(condition);
                result = dt_HealWeather.Clone();
                for (int i = 0; i < dr2.Length; i++)
                {
                    result.NewRow();
                    result.Rows.Add(dr2[i].ItemArray);
                }
            }
            string str_xml=ConvertDataTableToXML(result);
            return FormatXml(str_xml);
        }
        
        public string GetWarnLevel(string Level)
        {
            string warnLevel = "";
            switch (Level)
            {
                case "低": warnLevel = "1级"; break;
                case "轻微": warnLevel = "2级"; break;
                case "中等": warnLevel = "3级"; break;
                case "较高": warnLevel = "4级"; break;
                case "高": warnLevel = "5级"; break;
                case "不易中暑": warnLevel = "1级"; break;
                case "可能中暑": warnLevel = "2级"; break;
                case "较易中暑": warnLevel = "3级"; break;
                case "容易中暑": warnLevel = "4级"; break;
                case "极易中暑": warnLevel = "5级"; break;
            }
            return warnLevel;
        }
        private string ConvertDataTableToXML(DataTable xmlDS)
        {
            xmlDS.TableName = "resultName1";
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.GetEncoding("GB2312"));
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                return Encoding.GetEncoding("GB2312").GetString(arr).Trim();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        } 

         private string FormatXml(string sUnformattedXml)  
        {  
         XmlDocument xd = new XmlDocument();  
         xd.LoadXml(sUnformattedXml);  
         StringBuilder sb = new StringBuilder();  
         StringWriter sw = new StringWriter(sb);  
         XmlTextWriter xtw = null;  
         try  
         {  
             xtw = new XmlTextWriter(sw);  
             xtw.Formatting = Formatting.Indented;  
             xtw.Indentation = 1;  
             xtw.IndentChar = '\t';  
             xd.WriteTo(xtw);  
         }  
         finally  
         {  
             if (xtw != null)  
                 xtw.Close();  
         }  
         return sb.ToString();  
     }  
    }
}
