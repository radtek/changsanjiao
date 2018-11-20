using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

using AQIQuery.aQuery;

namespace MMShareBLL.DAL
{
   public  class DayForecast
    {
        private Database m_Database;
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DayForecast()
        {
            m_Database = new Database();
        }
        public string TransStaionData(string fromDate)
        {
            string time = DateTime.Parse(fromDate).ToString("yyyy/MM/dd");
            int[] siteArray = { 255, 257, 254, 256, 258, 0, 888888 };//{ 83, 84, 85, 86, 87,0};//select * from dbo.Site
            int[] parameterArray = { 21, 22, 2, 6, 145, 7, 92, 93, 94, 95, 106, 105, 76, 61, 62, 38 };//
            int[] CparameterArray = { 309, 307, 310, 308, 301, 302, 888888, 888888, 888888, 888888, 888888, 888888, 88888, 88888, 888888, 88888 };//


            string sql="SELECT  SiteID, ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value "+
                        "FROM SEMC_DMS.DBO.Data WHERE siteID IN ( 255, 257, 254, 256, 258) AND parameterID IN " +
                            "(21, 22, 2, 6, 145, 7) and LST='{0}' "+
                                "and DurationID=11  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 group by parameterID,siteID,LST ";
            string sql2 = "SELECT  SiteID, ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value " +
                       "FROM SEMC_DMS.DBO.Data WHERE siteID IN ( 255, 257, 254, 256, 258) AND parameterID IN " +
                           "( 92, 93, 94, 95, 106, 105, 76, 61, 62, 38) and LST='{0}' " +
                               "and DurationID=10  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 group by parameterID,siteID,LST ";


            string strSQL = string.Format(sql, time);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);//常规污染物
            strSQL = string.Format(sql2, time); 
            DataTable dtSiteDataII = m_Database.GetDataTable(strSQL);//其他污染物

            string siteString = "102";// SiteGroup.GetSiteIDbyGroupIDsString("101");
            DateTime dtimeStart = DateTime.Parse(time+" 00:00:00");
            DateTime dtimeEnd = DateTime.Parse(time + " 00:00:00");
            DataTable dtChinaData = Data.GroupDailyAQI(dtimeStart, dtimeEnd, siteString, "309,307,310,308,301,302");
            StringBuilder sb = new StringBuilder("{");
            string filter = "";
           // if (dtSiteData.Rows.Count > 0)
            //{
                for (int i = 0; i < siteArray.Length; i++)
                {
                    for (int j = 0; j < parameterArray.Length; j++)
                    {
                        filter = string.Format("SiteID = '{0}' AND ParameterID = {1}", siteArray[i], parameterArray[j]);
                        if (i <= 4)
                        {
                            if (j > 5)
                            {
                                DataRow[] rows = dtSiteDataII.Select(filter);
                                #region
                                if (rows.Length > 0)
                                {
                                    if (rows[0][1].ToString() == "92" ||
                                       rows[0][1].ToString() == "93" ||
                                       rows[0][1].ToString() == "94" ||
                                       rows[0][1].ToString() == "95" ||
                                       rows[0][1].ToString() == "106" ||
                                       rows[0][1].ToString() == "105" ||
                                       rows[0][1].ToString() == "61" ||
                                       rows[0][1].ToString() == "76" ||
                                       rows[0][1].ToString() == "62")
                                    {
                                        // 黄色的除以1000，保留三位小数
                                        sb.Append(string.Format("H{0}{1}:'{2}',", rows[0][0], rows[0][1], Math.Round(double.Parse(rows[0][3].ToString()) , 3).ToString()));
                                    }
                                    else if (rows[0][1].ToString() == "38")
                                    {
                                        //绿色的除以1000，保留0位小数
                                        sb.Append(string.Format("H{0}{1}:'{2}',", rows[0][0], rows[0][1], Math.Round(double.Parse(rows[0][3].ToString()) , 0).ToString()));
                                    }
                                    //else if (rows[0][1].ToString() == "6")
                                    //{
                                    //    sb.Append(string.Format("H{0}{1}:'{2}',", rows[0][0], rows[0][1], Math.Round(double.Parse(rows[0][3].ToString()) / 1000, 3).ToString()));
                                    //}
                                    //else
                                    //{
                                    //    sb.Append(string.Format("H{0}{1}:'{2}',", rows[0][0], rows[0][1], rows[0][3]));
                                    //}

                                }
                                else
                                {
                                    if (siteArray[i] == 254 || siteArray[i] == 256 || siteArray[i] == 258 || siteArray[i] == 257)
                                    {
                                        if (parameterArray[j] == 61 || parameterArray[j] == 62)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 254 || siteArray[i] == 258) && parameterArray[j] == 7)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 256 || siteArray[i] == 258) && parameterArray[j] == 38)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 257) && parameterArray[j] == 7)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else
                                        {
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                        }

                                    }
                                    else
                                        sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                }
                                #endregion
                            }
                            else
                            {
                                DataRow[] rowsII = dtSiteData.Select(filter);
                                #region
                                if (rowsII.Length > 0)
                                {
                                    if (j != 3)
                                        sb.Append(string.Format("H{0}{1}:'{2}',", rowsII[0][0], rowsII[0][1], Math.Round(double.Parse(rowsII[0][3].ToString()) * 1000, 1).ToString()));
                                    else
                                        sb.Append(string.Format("H{0}{1}:'{2}',", rowsII[0][0], rowsII[0][1], Math.Round(double.Parse(rowsII[0][3].ToString()), 3).ToString()));
                                }
                                else
                                {
                                    #region
                                    if (siteArray[i] == 254 || siteArray[i] == 256 || siteArray[i] == 258 || siteArray[i] == 257)
                                    {
                                        if (parameterArray[j] == 61 || parameterArray[j] == 62)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 254 || siteArray[i] == 258) && parameterArray[j] == 7)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 256 || siteArray[i] == 258) && parameterArray[j] == 38)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else if ((siteArray[i] == 257) && parameterArray[j] == 7)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "-"));
                                        else
                                        {
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                        }

                                    }
                                    else
                                        sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        if (siteArray[i] == 0)
                        {
                            if (j <= 5)
                            {
                                filter = string.Format(" ParameterID = {0}", parameterArray[j]);
                                DataRow[] rowsAVG = dtSiteData.Select(filter);
                                Double avgValue = 0d;
                                try
                                {
                                    foreach (DataRow row in rowsAVG)
                                    {
                                        avgValue += double.Parse(row["Value"].ToString());
                                    }
                                    if (avgValue == 0d) { sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null")); }
                                    else
                                    {
                                        if (j != 3)
                                            sb.Append(string.Format("H{0}{1}:'{2}',",  siteArray[i], parameterArray[j], Math.Round((avgValue/rowsAVG.Length) * 1000, 1).ToString()));
                                        else
                                           sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], Math.Round((avgValue/rowsAVG.Length), 3).ToString()));
                                    }
                                }
                                catch {
                                    sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                }
                               
                            }
                            else {
                                filter = string.Format("ParameterID = {0}", parameterArray[j]);
                                DataRow[] rowsAVG = dtSiteDataII.Select(filter);
                                Double avgValue = 0d;
                                try
                                {
                                    foreach (DataRow row in rowsAVG)
                                    {
                                        avgValue += double.Parse(row["Value"].ToString());
                                    }
                                    if (avgValue == 0d) { sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null")); }
                                    else
                                    {
                                        if(j==15)
                                            sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], Math.Round((avgValue / rowsAVG.Length), 0).ToString()));
                                        else
                                        sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], Math.Round((avgValue / rowsAVG.Length), 3).ToString()));
                                    }
                                }
                                catch {
                                    sb.Append(string.Format("H{0}{1}:'{2}',", siteArray[i], parameterArray[j], "Null"));
                                }
                                
                            }
                        }
                        //*****************
                        if (siteArray[i] == 888888)
                        {
                            string sqlFilter = " AQIItemID=" + CparameterArray[j] + "";
                            DataRow[] rowss = dtChinaData.Select(sqlFilter);
                            if (rowss != null && rowss.Length > 0)
                            {
                                // 求平均
                                //Double avgValue = 0d;
                                //foreach (DataRow row in rowss)
                                //{
                                //    avgValue += double.Parse(row["Value"].ToString());
                                //}
                                double values = double.Parse(rowss[0]["Value"].ToString());
                                if (CparameterArray[j] != 308)
                                {
                                    values = values * 1000;//mg转成ug/m3 
                                    values = Math.Round(values, 1);
                                }
                                else
                                {
                                    values = Math.Round(values, 3);
                                }

                                sb.Append(string.Format("H{0}{1}:'{2}',", "888888", parameterArray[j], values.ToString()));
                            }
                            else
                            {
                                if (CparameterArray[j] == 309 ||
                                    CparameterArray[j] == 307 ||
                                    CparameterArray[j] == 310 ||
                                    CparameterArray[j] == 308 ||
                                    CparameterArray[j] == 301 ||
                                    CparameterArray[j] == 302)
                                {
                                    sb.Append(string.Format("H{0}{1}:'{2}',", "888888", parameterArray[j], "Null"));
                                }
                                else
                                {
                                    sb.Append(string.Format("H{0}{1}:'{2}',", "888888", parameterArray[j], "-"));
                                }
                            }
                        }
                    }

                }
                    //去掉多余的“,”
                    if (sb.Length > 1)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
           // }
            if (sb.Length > 1)
            {
                sb.Append("}");
            }
            else
                sb.Length = 0;

            return sb.ToString();
        }
        public string CreateDayProduct(string fromDate)
        {
            try
            {
                //string time = DateTime.Parse(fromDate).ToString("yyyy/MM/dd");
                //string strSQL = string.Format("SELECT  SiteID, ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value FROM SEMC_DMS.DBO.Data WHERE siteID IN (254,255,256,257,258) AND parameterID IN (2, 6, 7, 21, 22, 145, 92, 93, 94,95, 106, 105, 76, 61, 62, 38) and LST='{0}' and DurationID=10  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 group by parameterID,siteID,LST", time);
                //DataTable dt = m_Database.GetDataTable(strSQL);
                //dt.TableName = "TransData";
                //strSQL = string.Format("DELETE SEMC_DMS.DBO.TransData WHERE LST= '{0}'", time);
                //m_Database.Execute(strSQL);//删除已有记录
                //Database m_DatabaseDMS = new Database("SEMCDMS");
                //m_DatabaseDMS.BulkCopy(dt);
                //strSQL = string.Format(" insert into  SEMC_DMS.DBO.TransData select * from (SELECT 0 as SiteID,* FROM( select ParameterID,LST,avg(Value) as Value from SEMC_DMS.DBO.TransData  WHERE LST='{0}' group by  ParameterID,LST ) as t ) as m", time);
                //dt = m_Database.GetDataTable(strSQL);
                //m_DatabaseDMS.BulkCopy(dt);

                return "{success:true}";
            }
            catch (Exception ex)
            {
                m_Log.Error("CreateDayProduct", ex);
                return ex.ToString();
            }

        }
    }
}
