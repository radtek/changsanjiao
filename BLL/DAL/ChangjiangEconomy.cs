using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace MMShareBLL.DAL
{
    public class ChangjiangEconomy
    {
        private Database m_Database;
        public ChangjiangEconomy()
        {
            m_Database = new Database();           
        }

        public string GetChangjiangThreeImg(string forecastDate)
        {
            StringBuilder sb = new StringBuilder();
            //获取图片是获取前一天的文件夹内
            string strForecastDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00.000");
            
            string strSQL = "SELECT TOP(3) Period, ('Product/'+Folder + '/' + Name) AS DM FROM T_ModelTest where ForecastDate='" + strForecastDate + "' AND Layers='35' ORDER BY Period ASC";
            DataTable dt = m_Database.GetDataTable(strSQL);



            //作者：张伟锋    日期：2016-05-30
            //当没有这个时间的数据的时候，自动填充最新的日期，以便于前台能够展现最新的时间
            if (dt.Rows.Count == 0)
            {
                //xuehui 0925
                string strForecastDate2 = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd 20:00:00.000");
                strSQL = "SELECT TOP(3) Period, ('Product/'+Folder + '/' + Name) AS DM FROM T_ModelTest where ForecastDate='" + strForecastDate2 + "' AND Layers='35' ORDER BY Period ASC";
                dt = m_Database.GetDataTable(strSQL);

                for (int i = 1; i < 4; i++)
                {
                    DataRow newRow = dt.NewRow();
                    newRow[0] = string.Format("{0:000}", 24 * i);
                    dt.Rows.Add(newRow);
                }
            }

            if (dt.Rows.Count > 0)
            {
                for(int i =0;i<dt.Rows.Count;i++)
                {
                    sb.Append("\"" + GetShowDate(DateTime.Parse(strForecastDate),i+1)+ "\":\"" + dt.Rows[i]["DM"].ToString() + "\",");
                }
                sb.Remove(sb.Length-1,1);
                return "{"+sb.ToString()+"}";
            }
            return "";
        }

        /// <summary>
        /// 根据当前的起报时间，返回显示的标签
        /// </summary>
        /// <param name="forecastDate"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private string GetShowDate(DateTime forecastDate, int i)
        {
            
            string showDate = string.Format("{0}日20时-{1}日20时",forecastDate.AddDays(i).Day,forecastDate.AddDays(i+1).Day);

            return showDate;
        }



        public string QueryChangjiangAQIData(string interval)
        {
            DateTime dtNow = DateTime.Now.Date;

            string strMaxForeDate = "";
            string strMaxDateSQL = "select MAX(ForecastDate) from dbo.T_ForecastSite";

            DataTable dtMax = m_Database.GetDataTable(strMaxDateSQL);
            if (dtMax.Rows.Count > 0)
            {
                strMaxForeDate = DateTime.Parse(dtMax.Rows[0][0].ToString()).ToString("yyyy-MM-dd 20:00:00.000");
            }
            string forecastDateTime = dtNow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            string strSQL = "select * from dbo.T_ForecastSite where ForecastDate='" + forecastDateTime + "' and Module='WRFCMAQ' and Interval='" + interval + "' and durationID=7";//xuehui
            StringBuilder sb = new StringBuilder("{");
            string json = "";
            DataTable dTable = m_Database.GetDataTable(strSQL);
            if (dTable.Rows.Count > 0)
            {
                json = GetAQIJson(dTable);
            }
            else {
                forecastDateTime = dtNow.AddDays(-2).ToString("yyyy-MM-dd 20:00:00");
                strSQL = "select * from dbo.T_ForecastSite where ForecastDate='" + forecastDateTime + "' and Module='WRFCMAQ' and Interval='" + interval + "' and durationID=7";//xuehui
                dTable = m_Database.GetDataTable(strSQL);// xuehui 0925
                try
                {
                    json = GetAQIJson(dTable);
                }
                catch { }

            }
            return json;
        }

        public string QueryChangjiangAQIDataHistory(string interval)
        {
            DateTime dtNow = DateTime.Now.Date;
            string strMaxForeDate = "";
            string strMaxDateSQL = "select MAX(ForecastDate) from dbo.T_ForecastSite_CJEco";

            DataTable dtMax = m_Database.GetDataTable(strMaxDateSQL);
            if (dtMax.Rows.Count > 0)
            {
                strMaxForeDate = DateTime.Parse(dtMax.Rows[0][0].ToString()).ToString("yyyy-MM-dd 20:00:00.000");
            }
            //string forecastDateTime = dtNow.AddDays(-2).ToString("yyyy-MM-dd 20:00:00");
            string strSQL = "select * from dbo.T_ForecastSite_CJEco where ForecastDate='" + strMaxForeDate + "' and Module='WRFCMAQ' and Interval='" + interval + "'";
            StringBuilder sb = new StringBuilder("{");
            string json = "";
            DataTable dTable = m_Database.GetDataTable(strSQL);
            if (dTable.Rows.Count > 0)
            {
                json = GetAQIJson(dTable);
            }
            return json;
        }

        public string SaveChangjiangData(string data,string interval)
        {
            string strForecastDateTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00.000");
            string strLST = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00.000")).AddHours(Convert.ToInt32(interval)).ToString("yyyy-MM-dd 20:00:00.000");
           
            string duratonId = "7";
            StringBuilder sb = new StringBuilder("INSERT INTO T_ForecastSite_CJEco (LST, ForecastDate, Interval,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Quality,Parameter)");
            if (data != "")
            {
                data = System.Text.RegularExpressions.Regex.Replace(data, @"\s+", "");
                data = System.Text.RegularExpressions.Regex.Replace(data, @"\n+", "");
                string[] cells = data.Split('&');

                string strAQIItemId = "";
                string strAQI = "";

                for (int i = 0; i < cells.Length; i++)
                {
                    string[] strSingleArea = cells[i].Split('_');
                    switch (strSingleArea[1])
                    {
                        case "PM2.5":
                            strAQIItemId = "1";
                            break;
                        case "PM10":
                            strAQIItemId = "2";
                            break;
                        case "NO2":
                            strAQIItemId = "3";
                            break;
                        case "O3_8h":
                            strAQIItemId = "5";
                            break;
                        case "O3":
                            strAQIItemId = "4";
                            break;
                        case "CO":
                            strAQIItemId = "6";
                            break;
                        case "SO2":
                            strAQIItemId = "7";
                            break;
                        default:
                            strAQIItemId = "0";
                            break;
                    }
                    strAQI = strSingleArea[2];
                    if (i == 0)
                    {
                        sb.Append(string.Format(" SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'", strLST, strForecastDateTime, interval, "", strSingleArea[0], duratonId, strAQIItemId, "0", strAQI, "WRFCMAQ","",""));
                    }
                    else
                    {
                        sb.Append(string.Format(" UNION ALL SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'", strLST, strForecastDateTime, interval, "", strSingleArea[0], duratonId, strAQIItemId, "0", strAQI, "WRFCMAQ", "", ""));
                    }
                }
                m_Database.Execute("delete from T_ForecastSite_CJEco where ForecastDate='" + strForecastDateTime + "' and Interval='" + interval+"'");
                m_Database.Execute(sb.ToString());
                return "success";
            }
            return "fail";
        }

        public string SaveChangjiangDataAndPic(string data, string interval, string imgUrl)
        {
            string strForecastDateTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00.000");
            string strLST = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:00:00.000")).AddHours(Convert.ToInt32(interval)).ToString("yyyy-MM-dd 20:00:00.000");

            string duratonId = "7";
            StringBuilder sb = new StringBuilder("INSERT INTO T_ForecastSite_CJEco (LST, ForecastDate, Interval,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Quality,Parameter)");
            if (data != "")
            {
                data = System.Text.RegularExpressions.Regex.Replace(data, @"\s+", "");
                data = System.Text.RegularExpressions.Regex.Replace(data, @"\n+", "");
                string[] cells = data.Split('&');

                string strAQIItemId = "";
                string strAQI = "";

                for (int i = 0; i < cells.Length; i++)
                {
                    string[] strSingleArea = cells[i].Split('_');
                    switch (strSingleArea[1])
                    {
                        case "PM2.5":
                            strAQIItemId = "1";
                            break;
                        case "PM10":
                            strAQIItemId = "2";
                            break;
                        case "NO2":
                            strAQIItemId = "3";
                            break;
                        case "O3-8h":
                            strAQIItemId = "5";
                            break;
                        case "O3":
                            strAQIItemId = "4";
                            break;
                        case "CO":
                            strAQIItemId = "6";
                            break;
                        case "SO2":
                            strAQIItemId = "7";
                            break;
                        default:
                            strAQIItemId = "0";
                            break;
                    }
                    strAQI = strSingleArea[2];
                    if (i == 0)
                    {
                        sb.Append(string.Format(" SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'", strLST, strForecastDateTime, interval, "", strSingleArea[0], duratonId, strAQIItemId, "0", strAQI, "WRFCMAQ", "", ""));
                    }
                    else
                    {
                        sb.Append(string.Format(" UNION ALL SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'", strLST, strForecastDateTime, interval, "", strSingleArea[0], duratonId, strAQIItemId, "0", strAQI, "WRFCMAQ", "", ""));
                    }
                }
                m_Database.Execute("delete from T_ForecastSite_CJEco where ForecastDate='" + strForecastDateTime + "' and Interval='" + interval + "'");
                m_Database.Execute(sb.ToString());
                string strExportImg = DealData(data, imgUrl);
                return "success" + "&" + strExportImg;
            }
            return "fail";
        }

        private string GetAQIJson(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            string strItem = "";
            //存储污染等级的json序列
            StringBuilder strPolLevelJson = new StringBuilder("{");
            //存储首要污染物的json序列
            StringBuilder strFirstPolJson = new StringBuilder("{");
            //存储AQIjson序列
            StringBuilder strAQIJson = new StringBuilder("{");
            //存储AQI的序列
            StringBuilder strHazeJson = new StringBuilder("{");
            //存储污染等级颜色json序列
            StringBuilder strColorJson = new StringBuilder("{");
            foreach (DataRow row in dataTable.Rows)
            {
                string strAreaID = row["Site"].ToString();
                string strItemID = row["ITEMID"].ToString();
                string strAQI = row["AQI"].ToString();
                string strAQILevel = CalculateAQLLevel(strAQI);
                //污染等级对应数字
                string strAQILevelNO = CalculateAQLLevelNo(strAQI).ToString();
                switch (strItemID)
                {
                    case "7":
                        strItem = "SO2";
                        break;
                    case "6":
                        strItem = "CO";
                        break;
                    case "3":
                        strItem = "NO2";
                        break;
                    case "2":
                        strItem = "PM10";
                        break;
                    case "5":
                        strItem = "O3-8h";
                        break;
                    case "4":
                        strItem = "O3";
                        break;
                    case "1":
                        strItem = "PM2.5";
                        break;
                    default:
                        strItem = "PM2.5";
                        break;
                }
                strPolLevelJson.Append("\"" + strAreaID + "_Level\":\"" + strAQILevel + "\",");
                strFirstPolJson.Append("\"" + strAreaID + "_Item\":\"" + strItem + "\",");
                strAQIJson.Append("\"" + strAreaID + "_AQI\":\"" + strAQI + "\",");
                strColorJson.Append("\"" + strAreaID + "\":\"" + strAQILevelNO + "\",");
                //sb.Append("'" + strAreaID + "_Item':'" + strItem + "','" + strAreaID + "_AQI':'" + strAQI + "','" + strAreaID + "_Level':'" + strAQILevel + "','" + strAreaID + "_ColorNo':'" + strAQILevelNO + "',");                
            }
            //去掉多余的“,”
            //if (sb.Length > 1)
            //{
            //    sb.Remove(sb.Length - 1, 1);                
            //}
            if (strPolLevelJson.Length > 1)
            {
                strPolLevelJson.Remove(strPolLevelJson.Length - 1, 1);
                strPolLevelJson.Append("}");
            }
            if (strFirstPolJson.Length > 1)
            {
                strFirstPolJson.Remove(strFirstPolJson.Length - 1, 1);
                strFirstPolJson.Append("}");
            }
            if (strAQIJson.Length > 1)
            {
                strAQIJson.Remove(strAQIJson.Length - 1, 1);
                strAQIJson.Append("}");
            }
            if (strColorJson.Length > 1)
            {
                strColorJson.Remove(strColorJson.Length - 1, 1);
                strColorJson.Append("}");
            }
            sb.Append("{");
            sb.Append("\"PolLevel\":" + strPolLevelJson.ToString() + ",");
            sb.Append("\"FirstPol\":" + strFirstPolJson.ToString() + ",");
            sb.Append("\"AQI\":" + strAQIJson.ToString() + ",");
            sb.Append("\"LevelColor\":" + strColorJson.ToString() + "}");
            return sb.ToString();
        }

        //计算AQI等级（优，良，轻度，重度，严重）
        public string CalculateAQLLevel(string aqiValue)
        {
            string strAQLLevel = "";
            if (aqiValue != null)
            {
                int intAQI = Convert.ToInt32(aqiValue);
                if (intAQI > 0 && intAQI <= 50)
                {
                    strAQLLevel = "优";
                }
                else if (intAQI > 50 && intAQI <= 100)
                {
                    strAQLLevel = "良";
                }
                else if (intAQI > 100 && intAQI <= 150)
                {
                    strAQLLevel = "轻度污染";
                }
                else if (intAQI > 150 && intAQI <= 200)
                {
                    strAQLLevel = "中度污染";
                }
                else if (intAQI > 200 && intAQI <= 300)
                {
                    strAQLLevel = "重度污染";
                }
                else if (intAQI > 300)
                {
                    strAQLLevel = "严重污染";
                }
            }
            return strAQLLevel;
        }

        //计算AQI等级对应的编码
        public int CalculateAQLLevelNo(string aqiValue)
        {
            int intAQLLevel = 0;
            if (aqiValue != null)
            {
                int intAQI = Convert.ToInt32(aqiValue);
                if (intAQI > 0 && intAQI <= 50)
                {
                    intAQLLevel = 1;
                }
                else if (intAQI > 50 && intAQI <= 100)
                {
                    intAQLLevel = 2;
                }
                else if (intAQI > 100 && intAQI <= 150)
                {
                    intAQLLevel = 3;
                }
                else if (intAQI > 150 && intAQI <= 200)
                {
                    intAQLLevel = 4;
                }
                else if (intAQI > 200 && intAQI <= 300)
                {
                    intAQLLevel = 5;
                }
                else if (intAQI > 300)
                {
                    intAQLLevel = 6;
                }
            }
            return intAQLLevel;
        }


        public string DealData(string data, string imgUrl)
        {
            try
            {
                if (imgUrl != "")
                {
                    string strCjImgBase = ConfigurationManager.AppSettings["ChangjiangImgBasePath"];
                    string strImgName = imgUrl.Substring(imgUrl.LastIndexOf('/') + 1);
                    string strChangjiangImgBase = @"F:\EMFCDatabase\ChangjiangImg\";
                    if (!Directory.Exists(strChangjiangImgBase))
                    {
                        Directory.CreateDirectory(strChangjiangImgBase);
                    }
                    string exportImageName = @"F:\EMFCDatabase\ChangjiangImg\" + strImgName;
                    string basemap = strCjImgBase + imgUrl.Substring(imgUrl.IndexOf("ModuleTest"));
                    Image imgPart = Image.FromFile(basemap);

                    Bitmap bitmapSource = new Bitmap(imgPart.Width, imgPart.Height);
                    Graphics resultGraphics;    //用来绘图的实例   
                    resultGraphics = Graphics.FromImage(bitmapSource);
                    resultGraphics.DrawImage(imgPart, 0, 0, imgPart.Width, imgPart.Height);
                    int sourceX = 0;
                    int sourceY = 0;

                    if (data != "")
                    {
                        data = System.Text.RegularExpressions.Regex.Replace(data, @"\s+", "");
                        data = System.Text.RegularExpressions.Regex.Replace(data, @"\n+", "");
                        string[] cells = data.Split('&');
                        string strAQIItemId = "";
                        string strAQI = "";

                        for (int i = 0; i < cells.Length; i++)
                        {
                            string[] strSingleArea = cells[i].Split('_');
                            switch (strSingleArea[1])
                            {
                                case "PM2.5":
                                    strAQIItemId = "6";
                                    break;
                                case "PM10":
                                    strAQIItemId = "3";
                                    break;
                                case "NO2":
                                    strAQIItemId = "2";
                                    break;
                                case "O3-8h":
                                    strAQIItemId = "5";
                                    break;
                                case "O3":
                                    strAQIItemId = "4";
                                    break;
                                case "CO":
                                    strAQIItemId = "4";
                                    break;
                                case "SO2":
                                    strAQIItemId = "1";
                                    break;
                                default:
                                    strAQIItemId = "0";
                                    break;
                            }
                            strAQI = strSingleArea[2];
                            switch (strSingleArea[0])
                            {
                                case "58367A":
                                    sourceX = 988;
                                    sourceY = 333;
                                    break;
                                case "58468A":
                                    sourceX = 984;
                                    sourceY = 378;
                                    break;
                                case "58457A":
                                    sourceX = 949;
                                    sourceY = 374;
                                    break;
                                case "58259A":
                                    sourceX = 979;
                                    sourceY = 297;

                                    break;
                                case "58349A":
                                    sourceX = 957;
                                    sourceY = 332;
                                    break;
                                case "58245A":
                                    sourceX = 917;
                                    sourceY = 289;
                                    break;
                                //南京
                                case "58238A":
                                    sourceX = 917;
                                    sourceY = 289;
                                    break;
                                case "58334A":
                                    sourceX = 882;
                                    sourceY = 331;
                                    break;
                                case "58321A":
                                    sourceX = 846;
                                    sourceY = 321;
                                    break;
                                case "58424A":
                                    sourceX = 840;
                                    sourceY = 368;
                                    break;
                                case "58502A":
                                    sourceX = 802;
                                    sourceY = 403;
                                    break;
                                case "57494A":
                                    sourceX = 734;
                                    sourceY = 363;
                                    break;
                                case "57461A":
                                    sourceX = 633;
                                    sourceY = 354;
                                    break;
                                case "57516A":
                                    sourceX = 455;
                                    sourceY = 377;
                                    break;
                                case "56491A":
                                    sourceX = 380;
                                    sourceY = 401;
                                    break;
                                case "56289A":
                                    sourceX = 366;
                                    sourceY = 321;
                                    break;
                            }
                            //if (strSingleArea[0] == "58334A")
                            //{
                            //    sourceX += 20;
                            //}
                            //if (strSingleArea[0] == "58367A")
                            //{
                            //    sourceX += 45;
                            //}
                            //if (strSingleArea[0] == "58468A")
                            //{
                            //    sourceX += 20;
                            //}
                            string site=strSingleArea[0];
                            if (site == "58321A")
                            {
                                sourceX = sourceX - 60;
                                sourceY = sourceY + 20;
                            }
                            else if (site == "58424A")
                            {
                                sourceX = sourceX - 12;
                                sourceY = sourceY + 26;
                            }
                            else if (site == "58334A")
                            {
                                sourceX = sourceX - 22;
                                sourceY = sourceY + 26;
                            }
                            else if (site == "58457A")
                            {
                                sourceX = sourceX - 30;
                                sourceY = sourceY + 26;
                            }
                            else if (site == "58468A")
                            {
                                sourceX = sourceX + 13;
                                sourceY = sourceY + 16;
                            }
                            else if (site == "58245A")
                            {
                                sourceX = sourceX - 24;
                                sourceY = sourceY -40;
                            }
                            else if (site == "58259A")
                            {
                                sourceY = sourceY + 27;
                                sourceY = sourceY - 9;
                            }
                            else if (site == "58367A")
                            {
                                sourceX = sourceX + 30;
                                sourceY = sourceY + 13;
                            }
                            else
                            {
                                sourceX = sourceX - 19;
                                sourceY = sourceY + 26;
                            }

                            SetTextElement(strAQI + "/" + strSingleArea[1], sourceX, sourceY, 14, bitmapSource, "宋体", Color.Black);
                        }
                    }
                    bitmapSource.Save(exportImageName, ImageFormat.Png);
                    imgPart.Dispose();
                    return exportImageName;
                }
                return "";
            }
            catch
            {
                return "";
            }

        }
        public Dictionary<string, string> GetAreapngs(Dictionary<string, string> AreaData)
        {
            Dictionary<string, string> AreaPath = new System.Collections.Generic.Dictionary<string, string>();
            string m_Basepath = "";
            foreach (var dic in AreaData)
            {
                string valie = dic.Value;
                string area = dic.Key;
                string datapath = string.Format("{0}/{1}/{2}/{3}", m_Basepath, "maps", area, valie.ToString() + ".png");
                AreaPath.Add(area, datapath);
            }
            return AreaPath;
        }

        /// <summary>
        /// 设置文本要素
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <param name="bmp"></param>
        private static void SetTextElement(string s, int x, int y, int size, Image bmp, string texttype, Color color)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Font f = new Font(texttype, size, FontStyle.Bold))
                {
                    using (Brush b = new SolidBrush(Color.Black))
                    {
                        string addText = s;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        g.DrawString(addText, f, b, x, y);
                    }
                }
            }

        }
    }
}
