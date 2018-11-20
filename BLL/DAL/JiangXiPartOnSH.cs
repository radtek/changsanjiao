using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using Readearth.Data.Entity;
using Readearth.Common;
using MMShareBLL.Model;
using MMShareBLL.DAL;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MMShareBLL.DAL
{   
    public class JiangXiPartOnSH
    {
        private Database m_Database;
        public JiangXiPartOnSH()
        {
            m_Database = new Database();            
        }

        public string ReturnJiangXiSiteDataCopy(string dateTime,string period)
        {
            if (period == "")
            {
                period = "024";
            }
            else if(period == "24")
            {
                period = "024";
            }
            else if (period == "48")
            {
                period = "048";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitle'>站点</td>");
            sb.Append("<td class='tabletitle'>SO2(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>NO2(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>PM10(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>CO(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>O3-8h_ave(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>PM2.5(μg/m3)</td>");
            sb.Append("<td class='tabletitle'>首要污染物</td>");
            sb.Append("<td class='tabletitle'>空气质量指数(AQI)</td>");
            sb.Append("</tr>");

            string strDTSiteSQL = "SELECT station_co,x,y,x_m, y_m,station_name, province, height, flag FROM sta_reg_set WHERE (province = '江西') AND (station_name <> '') order by CHARINDEX(RTRIM(CAST(station_co as NCHAR)),'58606,57786,57793,57796,57799,57993,58502,58527,58619,58627,58637,58524')";
            DataTable dtSite = m_Database.GetDataTable(strDTSiteSQL);
            List<string> sitesCnName = null;
            List<string> sitesID = null;
            if (dtSite.Rows.Count > 0)
            {
                sitesCnName = new List<string>();
                sitesID = new List<string>();
                for (int i = 0; i < dtSite.Rows.Count; i++)
                {
                    sitesCnName.Add(dtSite.Rows[i]["station_name"].ToString());
                    sitesID.Add(dtSite.Rows[i]["station_co"].ToString());
                }
            }
            string strFilter = "";
            DataRow[] dataRows;
            if (dateTime != "")
            {
                string strForeDateTime = DateTime.Parse(dateTime).ToString("yyyy-MM-dd 20:00:00.000");
                string strSQL = "select * from T_ForecastSite where Site in(SELECT  station_co FROM sta_reg_set WHERE (province = '江西') AND (station_name <> '')) and ForecastDate='" + strForeDateTime + "' and DURATIONID=07 and interval=" + period + " order by Site desc";
                DataTable dt = m_Database.GetDataTable(strSQL);

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < sitesCnName.Count; j++)
                    {
                        strFilter = "Site=" + sitesID[j];

                        DataRow[] dataRowsAll = dt.Select(strFilter);

                        if (dataRowsAll.Length > 0)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td class='" + "tableRow'>" + sitesCnName[j] + "</td>");
                            for (int m = 0; m < dataRowsAll.Length - 1; m++)
                            {
                                if (m == 0)
                                {
                                    strFilter = "Site=" + sitesID[j]+" and ITEMID='7'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j]+"_7" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                else if (m == 1)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='3'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_3" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                else if (m == 2)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='2'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_2" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                else if (m == 3)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='6'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_6" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                else if (m == 4)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='5'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_5" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                else if (m == 5)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='1'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_1" + "'>" + dataRows[0]["Value"] + "</td>");
                                    }
                                }
                                
                                //sb.Append("<td class='" + "tableRow'>" + dataRows[m]["Value"] + "</td>");
                            }
                            sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_First" + "'>" + "首要污染物" + "</td>");
                            sb.Append("<td class='" + "tableRow'" + " id='" + sitesID[j] + "_AQI" + "'>" + "AQI值" + "</td>");
                            sb.Append("</tr>");
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < sitesCnName.Count; j++)
                    {
                            sb.Append("<tr>");
                            sb.Append("<td class='" + "tableRow'>" + sitesCnName[j] + "</td>");
                            for (int m = 0; m < 7; m++)
                            {
                                sb.Append("<td class='" + "tableRow'>" + "--" + "</td>");
                            }
                            sb.Append("<td class='" + "tableRowBottom'>" + "--" + "</td>");
                            sb.Append("</tr>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < sitesCnName.Count; j++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='" + "tableRow'>" + sitesCnName[j] + "</td>");
                    for (int m = 0; m < 7; m++)
                    {
                        sb.Append("<td class='" + "tableRow'>" + "--" + "</td>");
                    }
                    sb.Append("<td class='" + "tableRowBottom'>" + "--" + "</td>");
                    sb.Append("</tr>");
                }
            }            
            sb.Append("</table>");
            return sb.ToString();
        }

        public string ReturnJiangXiSiteData(string dateTime, string period)
        {
            if (period == "")
            {
                period = "024";
            }
            else if (period == "24")
            {
                period = "024";
            }
            else if (period == "48")
            {
                period = "048";
            }

            StringBuilder sb = new StringBuilder();

            string strDTSiteSQL = "SELECT station_co,x,y,x_m, y_m,station_name, province, height, flag FROM sta_reg_set WHERE (province = '江西') AND (station_name <> '') order by CHARINDEX(RTRIM(CAST(station_co as NCHAR)),'58606,57786,57793,57796,57799,57993,58502,58527,58619,58627,58637,58524')";
            DataTable dtSite = m_Database.GetDataTable(strDTSiteSQL);
            List<string> sitesCnName = null;
            List<string> sitesID = null;
            if (dtSite.Rows.Count > 0)
            {
                sitesCnName = new List<string>();
                sitesID = new List<string>();
                for (int i = 0; i < dtSite.Rows.Count; i++)
                {
                    sitesCnName.Add(dtSite.Rows[i]["station_name"].ToString());
                    sitesID.Add(dtSite.Rows[i]["station_co"].ToString());
                }
            }
            string strFilter = "";
            DataRow[] dataRows;
            if (dateTime != "")
            {
                string strForeDateTime = DateTime.Parse(dateTime).ToString("yyyy-MM-dd 20:00:00.000");
                string strSQL = "select * from T_ForecastSite where Site in(SELECT  station_co FROM sta_reg_set WHERE (province = '江西') AND (station_name <> '')) and ForecastDate='" + strForeDateTime + "' and DURATIONID=07 and interval=" + period + " order by Site desc";
                DataTable dt = m_Database.GetDataTable(strSQL);

                if (dt.Rows.Count > 0)
                {
                    //每个站点的AQI和首要污染物
                    string strAQI = "";
                    string strAQIItemID = "";
                    string strAQIItem = "";
                    //首要污染物名称
                    string strItemName = "";
                    for (int j = 0; j < sitesCnName.Count; j++)
                    {
                        strFilter = "Site=" + sitesID[j];

                        DataRow[] dataRowsAll = dt.Select(strFilter);
                        if (dataRowsAll.Length > 0)
                        {
                            sb.Append("{\"" + sitesID[j]+"\":{");
                            for (int m = 0; m < dataRowsAll.Length - 1; m++)
                            {
                                if (m == 0)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='7'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_7" + "\":\"" + dataRows[0]["Value"]+"\",");
                                    }
                                }
                                else if (m == 1)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='3'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_3" + "\":\"" + dataRows[0]["Value"] + "\",");
                                    }
                                }
                                else if (m == 2)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='2'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_2" + "\":\"" + dataRows[0]["Value"] + "\",");
                                    }
                                }
                                else if (m == 3)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='6'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_6" + "\":\"" + dataRows[0]["Value"] + "\",");
                                    }
                                }
                                else if (m == 4)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='5'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_5" + "\":\"" + dataRows[0]["Value"] + "\",");
                                    }
                                }
                                else if (m == 5)
                                {
                                    strFilter = "Site=" + sitesID[j] + " and ITEMID='1'";
                                    dataRows = dt.Select(strFilter);
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"" + sitesID[j] + "_1" + "\":\"" + dataRows[0]["Value"] + "\",");
                                    }
                                }
                            }
                            DataTable dtAQI = GetReportTextAQIValueAndItemIDTableNew(strForeDateTime, period, sitesID[j]);
                            strAQI = "";
                            strAQIItemID = "";
                            if (dtAQI.Rows.Count > 0)
                            {
                                strAQI = dtAQI.Rows[0]["AQI"].ToString();
                                strAQI = AQIjisuan(Convert.ToInt32(strAQI));
                                strAQIItemID = dtAQI.Rows[0]["ITEMID"].ToString();
                            }
                            switch (strAQIItemID)
                            {
                                case "1":
                                    strAQIItem = "6";
                                    strItemName = "PM2.5";
                                    break;
                                case "2":
                                    strAQIItem = "3";
                                    strItemName = "PM10";
                                    break;
                                case "3":
                                    strAQIItem = "2";
                                    strItemName = "NO2";
                                    break;
                                case "4":
                                    strAQIItem = "5";
                                    strItemName = "O3";
                                    break;
                                case "5":
                                    strAQIItem = "5";
                                    strItemName = "O3";
                                    break;
                                case "6":
                                    strAQIItem = "4";
                                    strItemName = "CO";
                                    break;
                                case "7":
                                    strAQIItem = "1";
                                    strItemName = "SO2";
                                    break;
                                default:
                                    strAQIItem = "6";
                                    strItemName = "PM2.5";
                                    break;
                            }
                            sb.Append("\"" + sitesID[j] + "_First" + "\":\"" + strItemName + "\",");
                            sb.Append("\"" + sitesID[j] + "_AQI" + "\":\"" + Convert.ToInt32(strAQI) + "\"");
                            sb.Append("}},");
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    return "[" + sb.ToString() + "]";
                }                
            }
            return sb.ToString();
        }

        public DataTable GetReportTextAQIValueAndItemIDTableNew(string maxdate,string period, string siteID)
        {
            string forecastDateTime = DateTime.Parse(maxdate).AddHours(Convert.ToInt32(period)).ToString("yyyy-MM-dd 20:00:00.000");
            if (siteID != "" && period != "" && maxdate != "")
            {
                string strAQISQL = "select m.* from  ( select Max(AQI) AS AQI,Site From(select Site,LST,ITEMID,AQI from T_ForecastSite  WHERE  Site ='" + siteID + "' AND durationID=7 AND ForecastDate='" + maxdate + "' AND LST='" + forecastDateTime + "' and ITEMID <>5) result GROUP BY result.Site  ) t , ( select Site,LST,ITEMID,AQI ,[ForecastDate]from T_ForecastSite  WHERE  Site ='" + siteID + "' AND durationID=7  AND ForecastDate='" + maxdate + "' AND LST='" + forecastDateTime + "' and ITEMID <>5) m where t.AQI=m.AQI and t.Site=m.Site";
                return m_Database.GetDataTable(strAQISQL);
            }
            return null;
        }

        public string AQIjisuan(int AQI)
        {
            switch (AQI.ToString().Length)
            {
                case 1:
                    return "000" + AQI.ToString();
                case 2:
                    return "00" + AQI.ToString();
                case 3:
                    return "0" + AQI.ToString();
                default:
                    return "9999";
            }
        }

        public string ReadOfJsonCopy(string path, List<string> Stations)
        {
            //path = @"E:\\Z_SEVP_C_BABJ_20160411070216_P_MSP3_NMC_ENVAQFC_AIR_L88_CHN_201604112000_00000-07200.TXT";
            //Stations = new List<string> { "58606", "57786", "57793", "57796", "57799", "57993", "58502", "58527", "58619", "58627", "58637", "58524" };
            FileInfo sourceFile = new FileInfo(path);
            StreamReader sr = new StreamReader(sourceFile.FullName, Encoding.Default);
            string strLine = sr.ReadLine();
            string stationID;
            Dictionary<string, Dictionary<string, string>> strJson = new Dictionary<string, Dictionary<string, string>>();

            while (Stations.Count != 0)
            {
                int i = Stations.Count;
                strLine = sr.ReadLine();
                if (strLine != "NNNN" && strLine != null)
                {
                    stationID = Regex.Split(strLine, "\\s+")[0];
                    Stations.Remove(stationID);
                    if (i == Stations.Count)
                    {
                        for (int j = 0; j < 15; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add(stationID + "_7", redouble(Regex.Split(strLine, "\\s+")[1]));
                        strStation.Add(stationID + "_3", redouble(Regex.Split(strLine, "\\s+")[2]));
                        strStation.Add(stationID + "_2", redouble(Regex.Split(strLine, "\\s+")[3]));
                        strStation.Add(stationID + "_6", redouble(Regex.Split(strLine, "\\s+")[4]));
                        strStation.Add(stationID + "_5", redouble(Regex.Split(strLine, "\\s+")[5]));
                        strStation.Add(stationID + "_1", redouble(Regex.Split(strLine, "\\s+")[6]));
                        strStation.Add(stationID + "_First", Regex.Split(strLine, "\\s+")[9]);
                        strStation.Add(stationID + "_AQI", Regex.Split(strLine, "\\s+")[7]);
                        strJson.Add(stationID, strStation);
                        for (int j = 0; j < 8; j++)
                            strLine = sr.ReadLine();
                    }
                }
                else
                {
                    for (int j = 0; j < Stations.Count; j++)
                    {
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add("错误信息", "不存在该站点。");
                        strJson.Add(Stations[j].ToString(), strStation);
                    }
                    Stations.Clear();
                }
            }
            return JsonConvert.SerializeObject(strJson);
        }

        public string ReadOfJsonPast(string forecastDate,string period,string moduleName,string filePath)
        {
            string strLST = "";
            if(period=="")
            {
                period="024";
            }
            if (forecastDate == "")
            {
                forecastDate = DateTime.Now.ToString("yyyy-MM-dd 20:00:00.000");
                strLST = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 20:00:00.000");
            }
            if(forecastDate!="" && period!="")
            {
                forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 20:00:00.000");
                strLST = DateTime.Parse(forecastDate).AddHours(Convert.ToInt32(period)).ToString("yyyy-MM-dd 20:00:00.000");
            }
            //string path = @"E:\浦东项目\20160425\Z_SEVP_C_BABJ_20160411070216_P_MSP3_NMC_ENVAQFC_AIR_L88_CHN_201604112000_00000-07200.txt";
            //List<string> Stations = new List<string> { "58606", "57786", "57793", "57796", "57799", "57993", "58502", "58527", "58619", "58627", "58637", "58524" };
            List<string> Stations = new List<string> { "58606", "57786", "57793", "57796", "57799", "57993", "58502", "58527", "58619", "58627", "58637" };
            FileInfo sourceFile = new FileInfo(filePath);
            StreamReader sr = new StreamReader(sourceFile.FullName, Encoding.Default);
            string strLine = sr.ReadLine();
            string stationID;
            Dictionary<string, Dictionary<string, string>> strJson = new Dictionary<string, Dictionary<string, string>>();
            string strInsertSQL = "";
            int intSysleIndex = 0;
            string strAQIItemID = "";
            string strAQIItem = "";
            string strItemName = "";
            while (Stations.Count != 0)
            {
                if (intSysleIndex == 0)
                {
                    strInsertSQL = "INSERT INTO  T_ForecastSite (LST, ForecastDate, Interval,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Parameter)";
                    strInsertSQL += " SELECT ";
                }
                else
                {
                    strInsertSQL += " " + "UNION ALL SELECT ";
                }
                
                int i = Stations.Count;
                strLine = sr.ReadLine();
                if (strLine != "NNNN" && strLine != null)
                {
                    stationID = Regex.Split(strLine, "\\s+")[0];
                    Stations.Remove(stationID);
                    if (i == Stations.Count)
                    {
                        for (int j = 0; j < 15; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add(stationID + "_7", redouble(Regex.Split(strLine, "\\s+")[1]));
                        strStation.Add(stationID + "_3", redouble(Regex.Split(strLine, "\\s+")[2]));
                        strStation.Add(stationID + "_2", redouble(Regex.Split(strLine, "\\s+")[3]));
                        strStation.Add(stationID + "_6", redouble(Regex.Split(strLine, "\\s+")[4]));
                        strStation.Add(stationID + "_5", redouble(Regex.Split(strLine, "\\s+")[5]));
                        strStation.Add(stationID + "_1", redouble(Regex.Split(strLine, "\\s+")[6]));
                        switch (Regex.Split(strLine, "\\s+")[9])
                        {
                            case "1":
                                strAQIItem = "6";
                                strItemName = "PM2.5";
                                break;
                            case "2":
                                strAQIItem = "3";
                                strItemName = "PM10";
                                break;
                            case "3":
                                strAQIItem = "2";
                                strItemName = "NO2";
                                break;
                            case "4":
                                strAQIItem = "5";
                                strItemName = "O3";
                                break;
                            case "5":
                                strAQIItem = "5";
                                strItemName = "O3";
                                break;
                            case "6":
                                strAQIItem = "4";
                                strItemName = "CO";
                                break;
                            case "7":
                                strAQIItem = "1";
                                strItemName = "SO2";
                                break;
                            default:
                                strAQIItem = "6";
                                strItemName = "PM2.5";
                                break;

                        }
                        strStation.Add(stationID + "_First", strItemName);
                        strStation.Add(stationID + "_AQI", Convert.ToInt32(Regex.Split(strLine, "\\s+")[7]).ToString());
                        strJson.Add(stationID, strStation);

                        strInsertSQL += "'" + strLST + "','" + forecastDate + "','" + "7" + "','" + period + "','" + stationID + "','" + "7" + "','" + Regex.Split(strLine, "\\s+")[9].Substring(0, 1) + "','" + "0" + "','" + Convert.ToInt32(Regex.Split(strLine, "\\s+")[7]) + "" + "','" + moduleName + "','" + "" + "'";
                        intSysleIndex++;
                        for (int j = 0; j < 8; j++)
                            strLine = sr.ReadLine();
                    }
                }
                else
                {
                    for (int j = 0; j < Stations.Count; j++)
                    {
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add("错误信息", "不存在该站点。");
                        strJson.Add(Stations[j].ToString(), strStation);
                    }
                    Stations.Clear();
                }
            }

            try
            {
                if (strInsertSQL != "")
                {
                    m_Database.Execute(strInsertSQL);
                }
            }
            catch { }
            return JsonConvert.SerializeObject(strJson);
        }

        public string ReadOfJson(string forecastDate, string period, string moduleName, string filePath)
        {
            string strLST = "";
            if (period == "")
            {
                period = "024";
            }
            if (forecastDate == "")
            {
                forecastDate = DateTime.Now.ToString("yyyy-MM-dd 20:00:00.000");
                strLST = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 20:00:00.000");
            }
            if (forecastDate != "" && period != "")
            {
                forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 20:00:00.000");
                strLST = DateTime.Parse(forecastDate).AddHours(Convert.ToInt32(period)).ToString("yyyy-MM-dd 20:00:00.000");
            }
            //string path = @"E:\浦东项目\20160425\Z_SEVP_C_BABJ_20160411070216_P_MSP3_NMC_ENVAQFC_AIR_L88_CHN_201604112000_00000-07200.txt";
            //List<string> Stations = new List<string> { "58606", "57786", "57793", "57796", "57799", "57993", "58502", "58527", "58619", "58627", "58637", "58524" };
            List<string> Stations = new List<string> { "58606", "57786", "57793", "57796", "57799", "57993", "58502", "58527", "58619", "58627", "58637" };
            FileInfo sourceFile = new FileInfo(filePath);
            StreamReader sr = new StreamReader(sourceFile.FullName, Encoding.Default);
            string strLine = sr.ReadLine();
            string stationID;
            Dictionary<string, Dictionary<string, string>> strJson = new Dictionary<string, Dictionary<string, string>>();
            string strInsertSQL = "";
            int intSysleIndex = 0;
            string strAQIItemID = "";
            string strAQIItem = "";
            string strItemName = "";
            while (Stations.Count != 0)
            {
                if (intSysleIndex == 0)
                {
                    strInsertSQL = "INSERT INTO  T_ForecastSite (LST, ForecastDate, Interval,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Parameter)";
                    strInsertSQL += " SELECT ";
                }
                else
                {
                    strInsertSQL += " " + "UNION ALL SELECT ";
                }

                int i = Stations.Count;
                strLine = sr.ReadLine();
                if (strLine != "NNNN" && strLine != null)
                {
                    stationID = Regex.Split(strLine, "\\s+")[0];
                    Stations.Remove(stationID);
                    if (i == Stations.Count)
                    {
                        for (int j = 0; j < 15; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                            strLine = sr.ReadLine();
                        strLine = sr.ReadLine();
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add(stationID + "_7", redouble(Regex.Split(strLine, "\\s+")[1]));
                        strStation.Add(stationID + "_3", redouble(Regex.Split(strLine, "\\s+")[2]));
                        strStation.Add(stationID + "_2", redouble(Regex.Split(strLine, "\\s+")[3]));
                        strStation.Add(stationID + "_6", redouble(Regex.Split(strLine, "\\s+")[4]));
                        strStation.Add(stationID + "_5", redouble(Regex.Split(strLine, "\\s+")[5]));
                        strStation.Add(stationID + "_1", redouble(Regex.Split(strLine, "\\s+")[6]));
                        switch (Regex.Split(strLine, "\\s+")[9])
                        {
                            case "1":
                                strAQIItem = "6";
                                strItemName = "PM2.5";
                                break;
                            case "2":
                                strAQIItem = "3";
                                strItemName = "PM10";
                                break;
                            case "3":
                                strAQIItem = "2";
                                strItemName = "NO2";
                                break;
                            case "4":
                                strAQIItem = "5";
                                strItemName = "O3";
                                break;
                            case "5":
                                strAQIItem = "5";
                                strItemName = "O3";
                                break;
                            case "6":
                                strAQIItem = "4";
                                strItemName = "CO";
                                break;
                            case "7":
                                strAQIItem = "1";
                                strItemName = "SO2";
                                break;
                            default:
                                strAQIItem = "6";
                                strItemName = "PM2.5";
                                break;

                        }
                        strStation.Add(stationID + "_First", strItemName);
                        strStation.Add(stationID + "_AQI", Convert.ToInt32(Regex.Split(strLine, "\\s+")[7]).ToString());
                        strJson.Add(stationID, strStation);

                        strInsertSQL += "'" + strLST + "','" + forecastDate + "','" + "7" + "','" + period + "','" + stationID + "','" + "7" + "','" + Regex.Split(strLine, "\\s+")[9].Substring(0, 1) + "','" + "0" + "','" + Convert.ToInt32(Regex.Split(strLine, "\\s+")[7]) + "" + "','" + moduleName + "','" + "" + "'";
                        intSysleIndex++;
                        for (int j = 0; j < 8; j++)
                            strLine = sr.ReadLine();
                    }
                }
                else
                {
                    for (int j = 0; j < Stations.Count; j++)
                    {
                        Dictionary<string, string> strStation = new Dictionary<string, string>();
                        strStation.Add("错误信息", "不存在该站点。");
                        strJson.Add(Stations[j].ToString(), strStation);
                    }
                    Stations.Clear();
                }
            }

            try
            {
                if (strInsertSQL != "")
                {
                    m_Database.Execute(strInsertSQL);
                }
            }
            catch { }
            return JsonConvert.SerializeObject(strJson);
        }

        public string redouble(string reStart)
        {
            string reEnd = "";
            if (reStart != "999999")
                reEnd = Math.Round((double.Parse(reStart) / 100), 1).ToString(".0");
            else
                reEnd = "0.0";
            return reEnd;
        }

        //将江西格点预报界面上修改过的数据保存到T_ForecastSite表内
        public string SaveJiangXiAdjData(string dateTime, string period, string cells, string moduleName)
        {
            string forecastDate = "";
            string strLST = "";
            string strSQL="";
            if (dateTime != "" && period != "" && cells != "")
            {
                forecastDate = DateTime.Parse(dateTime).ToString("yyyy-MM-dd 20:00:00.000");
                strLST = DateTime.Parse(dateTime).AddHours(Convert.ToInt32(period)).ToString("yyyy-MM-dd 20:00:00.000");
                string [] cellsList = cells.Split('&');
                if (cellsList.Length > 0)
                {
                    strSQL += "INSERT INTO  T_ForecastSite (LST, ForecastDate, Interval,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Parameter)";
                    for (int i = 0; i < cellsList.Length; i++)
                    {
                        if (cellsList[i].Split(':')[0].Split('_')[1] != "AQI" && cellsList[i].Split(':')[0].Split('_')[1] != "First")
                        {
                            if (i == 0)
                            {
                                strSQL += " SELECT ";
                            }
                            else
                            {
                                strSQL += " " + "UNION ALL SELECT ";
                            }
                            strSQL += "'" + strLST + "','" + forecastDate + "','" + "7" + "','" + period + "','" + cellsList[i].Split(':')[0].Split('_')[0] + "','" + "7" + "','" + cellsList[i].Split(':')[0].Split('_')[1] + "','" + cellsList[i].Split(':')[1] + "','" + "999" + "" + "','" + moduleName + "','" + "" + "'";
                        }
                        
                    }
                    m_Database.Execute("delete from T_ForecastSite where ForecastDate='" + forecastDate + "' and LST='" + strLST + "' and Module='" + moduleName + "' and PERIOD='" + period + "'");
                    m_Database.Execute(strSQL);
                    return "success";
                }
            }
            return "fail";
        }

    }
}
