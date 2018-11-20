using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Readearth.Data;
using ChinaAQI;
using System.IO;

namespace MMShareBLL.DAL
{
    public class EvalutionCaculateJX
    {
        private Database m_Database;
        public DataTable dIAQITable;
        public EvalutionCaculateJX()
        {
            //dIAQITable = new DataTable("T_IAQIEvaluation");
            //dIAQITable.Columns.Add("LST", typeof(DateTime));
            //dIAQITable.Columns.Add("DurationID", typeof(int));
            //dIAQITable.Columns.Add("ITEMID", typeof(int));
            //dIAQITable.Columns.Add("Module", typeof(string));

            //dIAQITable.Columns.Add("Score", typeof(float));
            //dIAQITable.Columns.Add("userID", typeof(string));
            m_Database = new Database();
        }
        public string ReturnJXScore(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string[] modle = { "Manual", "WRF" };
            string[] modleName = { "主观预报", "WRF-chem" };
            string strSQL = string.Format("SELECT  LST,Module, f1, f2, f3, S from  T_JXChinaEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
            string filter = "";
            int[] day = new int[2];
            DataRow[] dataRows;
            string[] className = { "tableRow", "tableRowBottom" };
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitle'>日期</td>");
            sb.Append("<td class='tabletitle'></td>");
            sb.Append("<td class='tabletitle'>首要污染物</br>正确性评分(f1)</td>");
            sb.Append("<td class='tabletitle'>AQI预报级别</br>正确性评分（f2）</td>");
            sb.Append("<td class='tabletitle'>AQI预报数值</br>误差评分（f3）</td>");
            sb.Append("<td class='tabletitle'>空气质量预报</br>精确度评分（S）</td>");
            sb.Append("</tr>");
            for (int i = 0; i < modle.Length; i++)
            {
                filter = string.Format("Module='{0}'", modle[i]);
                dataRows = dt.Select(filter);
                day[i] = dataRows.Length;
            }

            foreach (DataRow rows in timeTable.Rows)
            {
                for (int i = 0; i < modle.Length; i++)
                {
                    sb.Append("<tr>");
                    if (i == 0)
                        sb.Append("<td class='tableRowBottom' rowspan='2'>" + DateTime.Parse(rows[0].ToString()).ToString("MM月dd日") + "</td>");
                    sb.Append("<td class='" + className[i] + "'>" + modleName[i] + "</td>");
                    filter = string.Format("Module='{0}' and LST='{1}'", modle[i], rows[0].ToString());
                    dataRows = dt.Select(filter);
                    if (dataRows.Length > 0)
                    {
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][2] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][3] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][4] + "</td>");
                        sb.Append("<td class='" + className[i] + "'>" + dataRows[0][5] + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                        sb.Append("<td class='" + className[i] + "'></td>");
                    }
                    sb.Append("</tr>");
                }
            }
            int k = 0;
            string tempStr = "";
            for (int i = 0; i < day.Length; i++)
            {
                if (day[i] - totalDays < 0)
                {

                    sb.Append("<tr>");
                    if (k == 0)
                    {
                        if (modleName[i] == "WRF-chem")
                            tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                        else
                            tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                    }
                    else
                        tempStr = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                    k++;
                    sb.Append("<td class='tableRowOther' colspan='6'>" + tempStr + "</td>");
                    sb.Append("</tr>");
                }
            }

            sb.Append("</table>");
            return sb.ToString();
        }

        /// <summary>
        /// 国家局评分
        /// </summary>
        /// <param name="DateMoth"></param>
        public void InsertEvaluationTable_JX(string DateMoth)
        {
            //try
            //{
                DataTable dt = new DataTable("T_JXChinaEvaluation");
                dt.Columns.Add("LST", typeof(DateTime));
                dt.Columns.Add("AQI", typeof(int));
                dt.Columns.Add("Module", typeof(string));
                dt.Columns.Add("Quality", typeof(string));
                dt.Columns.Add("Grade", typeof(int));
                dt.Columns.Add("Parameter", typeof(string));
                dt.Columns.Add("userID", typeof(string));
                //将国家局主观预报存入数据库
                DataTable dm = new DataTable("T_JXChinaValue");
                dm.Columns.Add("LST", typeof(DateTime));
                dm.Columns.Add("PM25", typeof(float));
                dm.Columns.Add("PM10", typeof(float));
                dm.Columns.Add("NO2", typeof(float));
                dm.Columns.Add("O3", typeof(float));

                dm.Columns.Add("AQI", typeof(int));
                dm.Columns.Add("Quality", typeof(string));
                dm.Columns.Add("Grade", typeof(int));
                dm.Columns.Add("Parameter", typeof(string));
                if (DateMoth == "")
                {
                    DateMoth = DateTime.Now.ToString("yyyy年MM月");
                }
                
                DateTime time = DateTime.Parse(DateMoth);
                string fromTime = time.Date.ToString("yyyy-MM-dd 00:00:00");
                string toTime = time.AddMonths(1).Date.AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
                //实况数据
                //string strSQL = string.Format("select a.time_point,a.AQI,a.primary_pollutant from ( SELECT time_point,AQI, primary_pollutant,datename(hour, time_point) as hourNme FROM T_24hAQI  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市') a where  a.hourNme=20 ORDER BY time_point;", fromTime, toTime);
                string strSQL = string.Format("select a.time_point,a.AQI,a.primary_pollutant from ( SELECT time_point,AQI, primary_pollutant,datename(hour, time_point) as hourNme FROM T_24hAQI  Where time_point BETWEEN '{0}' and '{1}' and  area='南昌市') a where  a.hourNme=20 ORDER BY time_point;", fromTime, toTime);
                DataTable RTDataTable = m_Database.GetDataTable(strSQL);
                //模式数据
                fromTime = DateTime.Parse(fromTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                toTime = DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
                //strSQL = string.Format("select LST,ITEMID,Value,D_Item.MC, Module from (SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and Interval=24 and durationID=7 group by LST,ITEMID,Module) a left join D_Item on D_Item.DM=a.ITEMID order by LST,ITEMID ;", fromTime, toTime);
                strSQL = string.Format("select LST,ITEMID,Value,D_Item.MC, Module from (SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE province='江西') and Interval=24 and durationID=7 group by LST,ITEMID,Module) a left join D_Item on D_Item.DM=a.ITEMID order by LST,ITEMID ;", fromTime, toTime);
                DataTable WRfTable = m_Database.GetDataTable(strSQL);
                DataTable dTime = WRfTable.DefaultView.ToTable(true, "LST");
                string filter = "";
                DataRow[] newRow;
                int AQIValue = 0;
                int maxAQI = 0;
                string itemID = "";
                foreach (DataRow rows in dTime.Rows)
                {
                    DataRow EvaluationRow = dt.NewRow();
                    EvaluationRow[0] = rows[0];
                    filter = string.Format("LST='{0}'", rows[0]);
                    newRow = WRfTable.Select(filter);
                    if (newRow.Length > 0)
                    {
                        foreach (DataRow rowData in newRow)
                        {
                            AQIValue = ToAQI(rowData[2].ToString(), rowData[1].ToString());
                            if (AQIValue > maxAQI)
                            {
                                maxAQI = AQIValue;
                                itemID = rowData[1].ToString();
                            }
                        }
                        EvaluationRow[1] = maxAQI;
                        EvaluationRow[2] = "WRF";
                        AQIExtention aqiExt = new AQIExtention(maxAQI, int.Parse(itemID));
                        EvaluationRow[3] = aqiExt.Quality;
                        EvaluationRow[4] = aqiExt.IntGrade;
                        EvaluationRow[5] = aqiExt.FirstPItemNoByGrade;
                        EvaluationRow[6] = DBNull.Value;
                    }
                    dt.Rows.Add(EvaluationRow);
                }

                //主观数据
                string Foretime = "";
                string userName = "";
                DateTime dtNow = DateTime.Parse(DateMoth);
                int intDayCount = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
                DataTable dtSingleDay;
                string AQI = "";
                string parameter = "";
                string strFilter = "";
                DataRow[] dataRows;
                DataTable dtAQI_24;
                for (int i = 0; i < intDayCount; i++)
                {
                    Foretime = dtNow.AddDays(i).ToString("yyyy-MM-dd 20:00:00.000");
                    DataRow EvaluRow = dt.NewRow();
                    EvaluRow[0] = Foretime;
                    DataRow chinaRow = dm.NewRow();
                    chinaRow[0] = Foretime;

                    strSQL = "SELECT * FROM dbo.T_ForecastSite WHERE ForecastDate='" + dtNow.AddDays(i).ToString("yyyy-MM-dd 20:00:00.000") + "' AND Interval='24' AND ITEMID in('7','3','2','6','4','1') AND Site='58606' AND durationID='10' order by Interval asc";
                    dtSingleDay = m_Database.GetDataTable(strSQL);
                    if (dtSingleDay.Rows.Count > 0)
                    {
                        strFilter = "ITEMID='1'";
                        dataRows = dtSingleDay.Select(strFilter);
                        if (dataRows.Length > 0)
                        {
                            chinaRow[1] = Math.Round(double.Parse(dataRows[0]["Value"].ToString()) / 100, 1);
                        }
                        strFilter = "ITEMID='2'";
                        dataRows = dtSingleDay.Select(strFilter);
                        if (dataRows.Length > 0)
                        {
                            chinaRow[2] = Math.Round(double.Parse(dataRows[0]["Value"].ToString()) / 100, 1);
                        }
                        strFilter = "ITEMID='3'";
                        dataRows = dtSingleDay.Select(strFilter);
                        if (dataRows.Length > 0)
                        {
                            chinaRow[3] = Math.Round(double.Parse(dataRows[0]["Value"].ToString()) / 100, 1);
                        }
                        strFilter = "ITEMID='4'";
                        dataRows = dtSingleDay.Select(strFilter);
                        if (dataRows.Length > 0)
                        {
                            chinaRow[4] = Math.Round(double.Parse(dataRows[0]["Value"].ToString()) / 100, 1);
                        }

                        dtAQI_24 = GetReportTextAQIValueAndItemIDTableNew(dtNow.AddDays(i+1).ToString("yyyy-MM-dd 20:00:00.000"), dtNow.AddDays(i).ToString("yyyy-MM-dd 20:00:00.000"), "58606");
                        if (dtAQI_24.Rows.Count > 0)
                        {
                            AQI = dtAQI_24.Rows[0]["AQI"].ToString();
                            chinaRow[5] = dtAQI_24.Rows[0]["AQI"].ToString();
                            parameter = FirstItem(dtAQI_24.Rows[0]["ITEMID"].ToString());
                            EvaluRow[1] = dtAQI_24.Rows[0]["AQI"].ToString(); ;
                            EvaluRow[2] = "Manual";
                            AQIExtention aqiExt = new AQIExtention(int.Parse(AQI));
                            EvaluRow[3] = aqiExt.Quality;
                            chinaRow[6] = aqiExt.Quality;
                            EvaluRow[4] = aqiExt.IntGrade;
                            chinaRow[7] = aqiExt.IntGrade;
                            chinaRow[8] = returnParamerID(parameter);
                            EvaluRow[5] = returnParamerID(parameter);
                            EvaluRow[6] = userName;
                            //dt.Rows.Add(EvaluRow);
                            //dm.Rows.Add(chinaRow);
                        }
                        dt.Rows.Add(EvaluRow);
                        dm.Rows.Add(chinaRow);                      
                    }                   
                }
                fromTime = time.Date.ToString("yyyy-MM-dd 00:00:00");
                toTime = time.AddMonths(1).Date.AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
                strSQL = string.Format("DELETE T_JXChinaValue WHERE LST between  '{0}' AND '{1}'", DateTime.Parse(fromTime).Date.ToString("yyyy-MM-dd 00:00:00"), DateTime.Parse(fromTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                m_Database.Execute(strSQL);//删除已有记录
                bool k = m_Database.BulkCopy(dm);
                chinaProces(RTDataTable, dt, DateMoth);
            //}
            //catch(Exception e) {
            //    string s = e.Message;
            //}
        }

        public void chinaProces(DataTable RTTable, DataTable WRFTble, string month)
        {
            DataTable dt = new DataTable("T_JXChinaEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Quality", typeof(string));
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("f1", typeof(float));
            dt.Columns.Add("f2", typeof(float));
            dt.Columns.Add("f3", typeof(float));
            dt.Columns.Add("S", typeof(float));
            dt.Columns.Add("userID", typeof(string));
            string Lst = "";
            string filter = "";
            string[] module = { "WRF", "Manual" };
            foreach (DataRow rows in RTTable.Rows)
            {
                DataRow newRow1 = dt.NewRow();
                Lst = rows[0].ToString();
                int AQI = int.Parse(rows[1].ToString());
                AQIExtention aqiExt = new AQIExtention(AQI);
                int index = 0;
                int spanAQI = 0;
                double f1 = 0.0;
                string parameter = returnParemeter(rows[2].ToString());
                for (int i = 0; i < module.Length; i++)
                {
                    DataRow newRow2 = dt.NewRow();
                    filter = string.Format("LST='{0}' and  Module='{1}'", Lst, module[i]);
                    DataRow[] WrfRows = WRFTble.Select(filter);
                    if (WrfRows.Length > 0)
                    {
                        newRow2[0] = DateTime.Parse(Lst).ToString("yyyy-MM-dd 00:00:00");
                        newRow2[1] = WrfRows[0][1];
                        newRow2[2] = WrfRows[0][2];
                        newRow2[3] = WrfRows[0][3];
                        newRow2[4] = WrfRows[0][5];

                        if (parameter.IndexOf(WrfRows[0][5].ToString()) >= 0)
                            f1 = 100;
                        else
                            f1 = 0;
                        newRow2[5] = f1;
                        index = aqiExt.IntGrade - int.Parse(WrfRows[0][4].ToString());
                        newRow2[6] = f2Score(index);
                        spanAQI = Math.Abs(AQI - int.Parse(WrfRows[0][1].ToString()));
                        newRow2[7] = f3Score(spanAQI);
                        newRow2[8] = 0.2 * f1 + 0.5 * f2Score(index) + 0.3 * f3Score(spanAQI);
                        newRow2[9] = WrfRows[0][6];
                        dt.Rows.Add(newRow2);
                    }
                }
                newRow1[0] = DateTime.Parse(Lst).ToString("yyyy-MM-dd 00:00:00");
                newRow1[1] = rows[1];
                newRow1[2] = "RT";

                newRow1[3] = aqiExt.Quality;
                newRow1[4] = parameter;
                newRow1[5] = DBNull.Value;
                newRow1[6] = DBNull.Value;
                newRow1[7] = DBNull.Value;
                newRow1[8] = DBNull.Value;
                newRow1[9] = DBNull.Value;
                dt.Rows.Add(newRow1);
            }
            string strSQL = string.Format("DELETE T_JXChinaEvaluation WHERE LST between  '{0}' AND '{1}'", DateTime.Parse(month).Date.ToString("yyyy-MM-dd 00:00:00"), DateTime.Parse(month).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            m_Database.Execute(strSQL);//删除已有记录
            bool k = m_Database.BulkCopy(dt);
        }

        public string returnParemeter(string Paremeter)
        {
            Paremeter = Paremeter.Replace("PM25", "PM2.5");
            Paremeter = Paremeter.Replace("Qzone8", "O3");
            Paremeter = Paremeter.Replace("Qzone1", "O3");
            return Paremeter;
        }

        public int ToAQI(string value, string itemID)
        {
            int AQIValue = 0;
            double inputValue = double.Parse(value) / 1000;
            switch (itemID)
            {
                case "1":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
                    break;
                case "2":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 7, 11, 180);
                    break;
                case "3":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 10, 0);
                    break;
                case "4":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
                    break;
                case "5":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
                    break;

            }

            return AQIValue;
        }

        public double f2Score(int index)
        {
            double score = 0.0;
            if (index == 0)
                score = 100;
            else if (Math.Abs(index) == 1)
                score = 50;
            else if (Math.Abs(index) == 2)
                score = 25;
            else
                score = 0;
            return score;
        }
        public double f3Score(int spanValue)
        {
            double score = 0.0;
            if (score <= 25)
                score = 100;
            else if (score <= 50)
                score = 80;
            else if (score <= 100)
                score = 60;
            else if (score <= 150)
                score = 30;
            else
                score = 0;
            return score;
        }

        public string returnParamerID(string itemId)
        {
            string id = "";
            switch (itemId)
            {
                case "6":
                    id = "PM2.5";
                    break;
                case "3":
                    id = "PM10";
                    break;
                case "2":
                    id = "NO2";
                    break;
                case "5":
                    id = "O3";
                    break;
                default:
                    id = " ";
                    break;
            }
            return id;
        }

        public string GetShanghaiReportContent()
        {
            //DateTime dtNow = DateTime.Now.Date;
            //dtNow = dtNow.AddDays(1);
            ////if (forecastDate != "")
            ////    dtNow = DateTime.Parse(forecastDate);
            //string forecastDateTime = dtNow.ToString("yyyy-MM-dd 20:00:00");


            List<string> sitrList = new List<string>();
            sitrList.Add("58367");
            string strTotalContent = "";
            if (sitrList.Count < 10)
            {
                strTotalContent = "00" + sitrList.Count.ToString() + "\n";
            }
            else if (sitrList.Count >= 10 && sitrList.Count < 100)
            {
                strTotalContent = "0" + sitrList.Count.ToString() + "\n";
            }
            else
            {
                strTotalContent = sitrList.Count.ToString() + "\n";
            }
            string strSingleSiteText = "";
            string strMaxForeDate = "";
            string strMaxDateSQL = "select MAX(ForecastDate) from dbo.T_ForecastSite";

            DataTable dtMax = m_Database.GetDataTable(strMaxDateSQL);
            if (dtMax.Rows.Count > 0)
            {
                strMaxForeDate = dtMax.Rows[0][0].ToString();
            }
            for (int i = 0; i < sitrList.Count; i++)
            {

                //strSingleSiteText = GetAQIAreaReportText(sitrList[i], strMaxForeDate);
                strSingleSiteText = GetAQIAreaReportTextNew(sitrList[i], strMaxForeDate);

                if (i < sitrList.Count - 1)
                {
                    strTotalContent += strSingleSiteText + "\n";
                }
                else
                {
                    strTotalContent += strSingleSiteText + "=" + "\r\nNNNN";
                }
            }
            return strTotalContent;
        }

        public string GetAQIAreaReportTextNew(string siteID, string maxDate)
        {
            string strContent = "";
            DateTime dtNow = DateTime.Now.Date;
            //dtNow = dtNow.AddDays(1);
            string forecastDateTime = dtNow.ToString("yyyy-MM-dd 20:00:00");
            //int intMark = DateTime.Now.Day - Convert.ToDateTime(maxDate).Day;            
            TimeSpan timeSpan = Convert.ToDateTime(DateTime.Now.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(maxDate).ToShortDateString());
            int intMark = timeSpan.Days;
            string strSQL = "";
            //strSQL = "SELECT * FROM dbo.T_ForecastSite WHERE ForecastDate='" + forecastDateTime + "' from dbo.T_ForecastSite) AND Interval in ('";
            strSQL = "SELECT * FROM dbo.T_ForecastSite WHERE ForecastDate='" + maxDate + "' AND Interval in ('";
            strSQL += (24 * (Math.Abs(intMark)) + 3).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 6).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 9).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 12).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 15).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 18).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 21).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 24).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 30).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 36).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 42).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 48).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 54).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 60).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 66).ToString() + "','";
            strSQL += (24 * (Math.Abs(intMark)) + 72).ToString() + "')";
            //strSQL += " AND ITEMID in('1','2','3','4','5','6') AND Site='" + siteID + "' AND durationID='10' order by Interval asc";
            strSQL += " AND ITEMID in('7','3','2','6','4','1') AND Site='" + siteID + "' AND durationID='10' order by Interval asc";


            //string strSQL ="SELECT * FROM dbo.T_AreaResult where Site='58606'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            string strAQI = "0";
            string strAQIItem = "0";

            DataTable dtAQI = GetReportTextAQIValueAndItemIDTableNew(forecastDateTime, maxDate, siteID);
            if (dtAQI.Rows.Count > 0)
            {
                strAQI = dtAQI.Rows[0]["AQI"].ToString();
                strAQIItem = dtAQI.Rows[0]["ITEMID"].ToString();
            }

            //查找对应站点的经纬度
            string strCordSQL = "SELECT * FROM dbo.sta_reg_set WHERE station_co='" + siteID + "'";
            DataTable dtXY = m_Database.GetDataTable(strCordSQL);
            //纬度
            string strX = "";
            //经度
            string strY = "";
            if (dtXY.Rows.Count > 0)
            {
                double dblX = Convert.ToDouble(dtXY.Rows[0]["x"].ToString());
                double dblY = Convert.ToDouble(dtXY.Rows[0]["y"].ToString());
                Math.Round(dblX, 2, MidpointRounding.AwayFromZero);
                strX = (Math.Round(dblX, 2, MidpointRounding.AwayFromZero) * 100).ToString();
                strY = (Math.Round(dblY, 2, MidpointRounding.AwayFromZero) * 100).ToString();

            }
            //根据站点编号，日期和24小时AQI预报值，生成预报文本
            //strContent = GetReportText(dt, siteID, strAQI, strAQIItem, strX, strY);
            return strContent;
        }

        public DataTable GetReportTextAQIValueAndItemIDTableNew(string forecastDateTime, string maxdate, string siteID)
        {
            if (siteID != "" && forecastDateTime != "" && maxdate != "")
            {
                string strAQISQL = "select m.* from  ( select Max(AQI) AS AQI,Site From(select Site,LST,ITEMID,AQI from T_ForecastSite  WHERE  Site ='" + siteID + "' AND durationID=7 AND ForecastDate='" + maxdate + "' AND LST='" + forecastDateTime + "' and ITEMID <>5) result GROUP BY result.Site  ) t , ( select Site,LST,ITEMID,AQI ,[ForecastDate]from T_ForecastSite  WHERE  Site ='" + siteID + "' AND durationID=7  AND ForecastDate='" + maxdate + "' AND LST='" + forecastDateTime + "' and ITEMID <>5) m where t.AQI=m.AQI and t.Site=m.Site";
                return m_Database.GetDataTable(strAQISQL);
            }
            return null;
        }
        public string Valuejisuan(double denValue)
        {
            string strValuReturn;
            strValuReturn = (denValue * 100).ToString();
            switch (strValuReturn.Length)
            {
                case 0:
                    strValuReturn = "000000" + strValuReturn;
                    break;
                case 1:
                    strValuReturn = "00000" + strValuReturn;
                    break;
                case 2:
                    strValuReturn = "0000" + strValuReturn;
                    break;
                case 3:
                    strValuReturn = "000" + strValuReturn;
                    break;
                case 4:
                    strValuReturn = "00" + strValuReturn;
                    break;
                case 5:
                    strValuReturn = "0" + strValuReturn;
                    break;
                case 6:
                    strValuReturn = "" + strValuReturn;
                    break;

            }
            return strValuReturn;
        }

        public string FirstItem(string firstItem)
        {
            string strValuReturn = null;
            int intITEMID = Convert.ToInt32(firstItem);
            string strUseItem = "";
            switch (firstItem)
            {
                case "1":
                    strUseItem = "6";
                    break;
                case "2":
                    strUseItem = "3";
                    break;
                case "3":
                    strUseItem = "2";
                    break;
                case "4":
                    strUseItem = "5";
                    break;
                case "6":
                    strUseItem = "4";
                    break;
                case "7":
                    strUseItem = "1";
                    break;
            }
            strValuReturn = strUseItem ;
            return strValuReturn;
        }
    }
}
