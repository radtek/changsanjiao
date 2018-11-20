using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

namespace MMShareBLL.DAL
{

    public class ScoreData
    {
        private Database m_Database;
        public DataTable dIAQITable;
        public ScoreData()
        {
            m_Database = new Database();
        }
        public string ForecasterQuery(string from, string to)
        {
            //分段预报数据
            string fromTime = DateTime.Parse(from).ToString("yyyy-MM-dd 00:00:00");
            string toTime = DateTime.Parse(to).ToString("yyyy-MM-dd 00:00:00");
            //string strSQL = "SELECT LST,AQI,'Manual' as Manual,Value,DurationID,Parameter,ITEMID FROM T_ForecastGroup WHERE Module='WRF' and ForecastDate between  '" + fromTime + "' and '" + toTime + "' and durationID in (2,3,6)   and PERIOD=24 ORDER BY LST";
            //DataTable foreCastTable = m_Database.GetDataTable(strSQL);
            //strSQL = "";
            ////实况数据
            //string[] durationTalbeName = { "T_PMAQI", "T_NightAQI", "T_AMAQI" };
            //for (int i = 0; i < durationTalbeName.Length; i++)
            //{
            //    strSQL = strSQL + string.Format(" SELECT time_point,AQI_PM25 as PM25,AQI_PM10 as PM10,AQI_NO2 as NO2, AQI_Ozone1 as O31, AQI_Ozone8 as  O38,  AQI, primary_pollutant FROM  " + durationTalbeName[i] + "  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市' ORDER BY time_point;", fromTime, toTime);
            //}

            //DataSet RTDataSet = m_Database.GetDataset(strSQL);
            ////模式数据
            //strSQL = string.Format("SELECT ForecastDate,LST,ITEMID,avg(Value),Module,durationID FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and PERIOD=48  and  durationID in (2,3,6) group by LST,ITEMID,Module,durationID,ForecastDate  order by LST,ForecastDate,ITEMID ;", fromTime, toTime);
            //DataTable WRfTable = m_Database.GetDataTable(strSQL);
            //分段分数
            string strSQL = string.Format("SELECT f0, f1, f2, f3, f4, F,DurationID, FROM T_DurationEvaluation where module='Manual' and LST between '{0}' and '{1}' ", fromTime, toTime);
            return "";
        }
        public string MonthWord(string dateTime)
        {
            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT count(*)  as count FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' and RtDay>0 ;", dtFrom, DateTime.Parse(dateTime).ToString("yyyy-MM-10 23:59:59"));
            strSQL = strSQL + string.Format("SELECT count(*) as count FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' and RtDay>0 ;", DateTime.Parse(dateTime).ToString("yyyy-MM-11 00:00:00"), DateTime.Parse(dateTime).ToString("yyyy-MM-20 23:59:59"));
            strSQL = strSQL + string.Format("SELECT count(*)  as count FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' and RtDay>0 ;", DateTime.Parse(dateTime).ToString("yyyy-MM-21 00:00:00"), dtTo);
            DataSet dtSet = m_Database.GetDataset(strSQL);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder("{");

            sm.Append(string.Format("'hazaData':'<div >{0}月份，上海出现{1}个霾日，其中上旬出现{2}天，中旬出现{3}天，下旬出现{4}天（霾日的统计时效为20-20时）。预报质量见表1。'"));
            string[] name = { "05时（08-20）", "17时（20-20）" };
            string[] nameStr = { "05", "17" };
            string[] titleName = { "实况", "准确预报", "空报", "漏报", "预报评分" };
            string[] titleStr = { "RtAllDay", "CorrectDays", "NoneDays", "FailDay", "Score" };
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleHaze' colspan='2'></td>");
            sb.Append("<td class='tabletitleHaze'>无霾</td>");
            sb.Append("<td class='tabletitleHaze'>有霾</td>");
            sb.Append("</tr>");
            for (int i = 0; i < name.Length; i++)
            {
                for (int j = 0; j < titleName.Length; j++)
                {
                    sb.Append("<tr>");
                    if (j == 0)
                        sb.Append("<td class='tableRowHaze' rowspan='5'>" + name[i] + "</td>");
                    sb.Append("<td class='tableRowHaze' >" + titleName[j] + "</td>");
                    if (hazeTable.Rows.Count > 0)
                    {
                        if (titleName[j] != "预报评分")
                        {
                            //sb.Append("<td class='tableRowHazeLow' >" + (totalDays - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString() + "</td>");
                            //sb.Append("<td class='tableRowHazeLow' >" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "</td>");
                        }
                        else
                        {
                            sb.Append("<td class='tableRowHazeLow'  colspan='2'>" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "</td>");
                        }

                    }
                    else
                    {
                        if (titleName[j] != "预报评分")
                        {
                            sb.Append("<td class='tableRowHazeLow' ></td>");
                            sb.Append("<td class='tableRowHazeLow' ></td>");
                        }
                        else
                            sb.Append("<td class='tableRowHazeLow' colspan='2'></td>");
                    }
                    sb.Append("</tr>");
                }
            }

            sb.Append("</table>");
            return sb.ToString();

        }
        public string trickMonthChart(string year)
        {
            string time = year.Replace("年", "") + "-01-01 00:00:00";
            string strSQL = string.Format("SELECT UserName,PersonScore,RANK() OVER(ORDER BY PersonScore DESC) as PersonScoreRank FROM T_PersonScoreYear WHERE LST ='{0}' order by PersonScoreRank", time);
            DataTable dt = m_Database.GetDataTable(strSQL);
            string x = ""; string y = "";
            string strReturn = "";
            foreach (DataRow dr in dt.Rows)
            {
                x = x + "|" + dr[0].ToString();
                y = y + "|" + dr[1].ToString();
            }
            strReturn = x.TrimStart('|') + "*" + y.TrimStart('|');

            return strReturn;
        }
        public string retrunChinaYearList(string time, string userName, string f1, string f2, string f3, string totalScore)
        {
            string fromTime = DateTime.Parse(time).ToString("yyyy-01-01 00:00:00");
            string toTime = DateTime.Parse(fromTime).AddYears(1).AddDays(-1).ToString("yyyy-12-31 23:59:59");
            string strSQL = string.Format("SELECT LST,f1,f2,f3,S FROM  T_ChinaEvaluation where Module ='Manual' and LST between '{0}' and '{1}' and userID='{2}' order by LST ", fromTime, toTime, userName);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='4' class='tdInfo'>" + DateTime.Parse(time).ToString("yyyy年") + ":" + userName + "</td>");
            sb.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleChild'>时间</td>");
            sb.Append("<td class='tabletitleChild'>首要污染物</br>正确性评分</td>");
            sb.Append("<td class='tabletitleChild'>AQI预报级别</br>正确性评分</td>");
            sb.Append("<td class='tabletitleChild'>AQI预报数值</br>误差评分</td>");
            sb.Append("<td class='tabletitleChild'>综合评分</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in dt.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowChild'>" + rows["LST"].ToString() + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f1"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f2"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f3"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["S"].ToString()), 1) + "</td>");
                sb.Append("</tr>");
            }
            return sb.ToString();

        }
        public string retrunChinaYear(string year)
        {
            string fromTime = year.Replace("年", "") + "-01-01 00:00:00";
            string toTime = DateTime.Parse(fromTime).AddYears(1).AddDays(-1).ToString("yyyy-12-31 23:59:59");
            string strSQL = string.Format("SELECT userID,count(*) as count,avg(f1) as f1,avg(f2) as f2,avg(f3) as f3,avg( S) as totalScore,RANK() OVER(ORDER BY avg( S) DESC) as Rank FROM  T_ChinaEvaluation where Module ='Manual' and LST between '{0}' and '{1}'  group by userID order by Rank ", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleChild'>预报员</td>");
            sb.Append("<td class='tabletitleChild'>班次</td>");
            sb.Append("<td class='tabletitleChild'>首要污染物</br>正确性评分</td>");
            sb.Append("<td class='tabletitleChild'>AQI预报级别</br>正确性评分</td>");
            sb.Append("<td class='tabletitleChild'>AQI预报数值</br>误差评分</td>");
            sb.Append("<td class='tabletitleChild'>综合评分</td>");
            sb.Append("<td class='tabletitleChild'>名次</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in dt.Rows)
            {
                sb.Append(string.Format("<tr onclick=\"chinaYearClick('{0}','{1}',this)\">", fromTime, rows["userID"].ToString()));
                sb.Append("<td class='tableRowChild'>" + rows["userID"].ToString() + "</td>");
                sb.Append("<td class='tableRowChild'>" + rows["count"].ToString() + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f1"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f2"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["f3"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(rows["totalScore"].ToString()), 1) + "</td>");
                sb.Append("<td class='tableRowChild'>" + rows["Rank"].ToString() + "</td>");

                sb.Append("</tr>");
            }
            return sb.ToString();
        }
        public string trickMonthData(string year)
        {
            string fromTime = year.Replace("年", "") + "-01-01 00:00:00";
            string strSQL = string.Format("SELECT UserName,AVGSEMCScore,HazeScore, UVScore,PersonScore,AvgChinaScore,RANK() OVER(ORDER BY PersonScore DESC) as PersonScoreRank,RANK() OVER(ORDER BY AvgChinaScore DESC) as AvgChinaScoreRank FROM T_PersonScoreYear WHERE LST ='{0}' order by PersonScoreRank", fromTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTable' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitlePerson' rowspan='2'>姓名</td>");
            sb.Append("<td class='tabletitlePerson' colspan='5'>常规预报评分</td>");
            sb.Append("<td class='tabletitlePerson' colspan='2'>国家局评分</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleUVOther'>分时段预报</br>准确率</td>");
            sb.Append("<td class='tabletitleUVOther'>霾预报准确率</td>");
            sb.Append("<td class='tabletitleUVOther'>紫外线预报准确率</td>");
            sb.Append("<td class='tabletitleUVOther'>个人总分</td>");
            sb.Append("<td class='tabletitleUVOther4'>名次</td>");
            sb.Append("<td class='tabletitleUVOther'>国家局评分</td>");
            sb.Append("<td class='tabletitleUVOther2'>名次</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in dt.Rows)
            {
                sb.Append(string.Format("<tr onclick=\"trYearClick('{0}','{1}',this)\">", fromTime, rows["UserName"]));

                sb.Append("<td class='tableRowPerson'>" + rows["UserName"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["AVGSEMCScore"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["HazeScore"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["UVScore"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["PersonScore"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["PersonScoreRank"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["AvgChinaScore"] + "</td>");
                sb.Append("<td class='tableRowPerson'>" + rows["AvgChinaScoreRank"] + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string retrunSingleScore(string time, string userName)
        {
            string fromTime = DateTime.Parse(time).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            // select LST,AVG(F) as Score,userID from( SELECT DATEADD(DAY,1,LST) as LST,DurationID,F,userID FROM  T_DurationEvaluation WHERE  Module='Manual'   and DurationID=6 union select LST,DurationID,F,userID FROM  T_DurationEvaluation WHERE  Module='Manual'   and DurationID in (2,3)) a GROUP BY LST,userID  order by LST
            string strSQL = string.Format("SELECT LST,AVG(F) as Score FROM T_DurationEvaluation WHERE  LST between '{0}' and '{1}' and  Module='Manual' and DurationID in (2,3,6) and userID='{2}'  group by LST; ", fromTime, toTime, userName);
            //string strSQL = string.Format("SELECT LST,AVG(F) as Score  FROM  T_DurationEvaluation WHERE  userID='{2}' and Module='Manual' GROUP BY LST  order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_HazeEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_UVEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,S  as Score FROM T_ChinaEvaluation WHERE  userID='{2}' and Module='Manual' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            DataSet dtSet = m_Database.GetDataset(strSQL);
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='5' class='tdInfo'>" + DateTime.Parse(time).ToString("yyyy年M月") + ":" + userName + "</td>");
            sb.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleFilter'>时间</td>");
            sb.Append("<td class='tabletitleFilter'>分时段预报</br>准确率</td>");
            sb.Append("<td class='tabletitleFilter'>霾预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>紫外线预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>国家局准确率</td>");
            sb.Append("<td class='tabletitleFilter'>总分</td>");
            sb.Append("</tr>");
            string filter = "";
            DataRow[] dataRows;
            double totalScore = 0.0;
            sb.Append("<tr>");
            sb.Append("<td colspan='6' >");
            sb.Append("<div class='fixDIv'>");
            sm.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            foreach (DataRow rows in dtSet.Tables[0].Rows)
            {
                totalScore = 0.0;
                sm.Append(string.Format("<tr onclick=\"trDayClick('{0}','{1}',this)\">", DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00:00"), userName));
                sm.Append("<td class='tableRowFilter'>" + DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00") + "</td>");
                sm.Append("<td class='tableRowFilter'>" + Math.Round(double.Parse(rows["Score"].ToString()), 1) + "</td>");
                totalScore = 0.6 * Math.Round(double.Parse(rows["Score"].ToString()), 1);
                filter = string.Format("LST='{0}'", rows["LST"].ToString());
                for (int i = 1; i < dtSet.Tables.Count; i++)
                {
                    dataRows = dtSet.Tables[i].Select(filter);
                    if (dataRows.Length > 0)
                    {
                        if (i != dtSet.Tables.Count - 1)
                            totalScore = totalScore + double.Parse(dataRows[0]["Score"].ToString());
                        sm.Append("<td class='tableRowFilter'>" + dataRows[0]["Score"] + "</td>");
                    }
                    else
                        sm.Append("<td class='tableRowFilter'>-</td>");
                }
                sm.Append("<td class='tableRowFilter'>" + Math.Round(totalScore, 1) + "</td>");
                sm.Append("</tr>");
            }
            sm.Append("</table>");
            sb.Append(sm.ToString());
            sb.Append("</div>");
            sb.Append("</td>");
            strSQL = string.Format("SELECT AVGSEMCScore,HazeScore, UVScore,AvgChinaScore,PersonScore FROM T_PersonScore WHERE LST ='{0}' and UserName='{1}' ", fromTime, userName);
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowFilterMonth'>当月平均</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["AVGSEMCScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["HazeScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["UVScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["AvgChinaScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["PersonScore"] + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string retrunSinglePerson(string time, string userName)
        {
            string fromTime = DateTime.Parse(time).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(time).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            // select LST,AVG(F) as Score,userID from( SELECT DATEADD(DAY,1,LST) as LST,DurationID,F,userID FROM  T_DurationEvaluation WHERE  Module='Manual'   and DurationID=6 union select LST,DurationID,F,userID FROM  T_DurationEvaluation WHERE  Module='Manual'   and DurationID in (2,3)) a GROUP BY LST,userID  order by LST
            string strSQL = string.Format("SELECT LST,AVG(F) as Score FROM T_DurationEvaluation WHERE  LST between '{0}' and '{1}' and  Module='Manual' and DurationID in (2,3,6) and userID='{2}'  group by LST; ", fromTime, toTime, userName);
            //string strSQL = string.Format("SELECT LST,AVG(F) as Score  FROM  T_DurationEvaluation WHERE  userID='{2}' and Module='Manual' GROUP BY LST  order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_HazeEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_UVEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,S  as Score FROM T_ChinaEvaluation WHERE  userID='{2}' and Module='Manual' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            DataSet dtSet = m_Database.GetDataset(strSQL);
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='5' class='tdInfo'>" + DateTime.Parse(time).ToString("yyyy年M月") + ":" + userName + "</td>");
            sb.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleFilter'>时间</td>");
            sb.Append("<td class='tabletitleFilter'>分时段预报</br>准确率</td>");
            sb.Append("<td class='tabletitleFilter'>霾预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>紫外线预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>国家局准确率</td>");
            sb.Append("<td class='tabletitleFilter'>总分</td>");
            sb.Append("</tr>");
            string filter = "";
            DataRow[] dataRows;
            double totalScore = 0.0;
            sb.Append("<tr>");
            sb.Append("<td colspan='6' >");
            sb.Append("<div class='fixDIv'>");
            sm.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            foreach (DataRow rows in dtSet.Tables[0].Rows)
            {
                totalScore = 0.0;
                sm.Append(string.Format("<tr onclick=\"trDayClick('{0}','{1}',this)\">", DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00:00"), userName));
                sm.Append("<td class='tableRowFilter'>" + DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00") + "</td>");
                sm.Append("<td class='tableRowFilter'>" + Math.Round(double.Parse(rows["Score"].ToString()), 1) + "</td>");
                totalScore = 0.6 * Math.Round(double.Parse(rows["Score"].ToString()), 1);
                filter = string.Format("LST='{0}'", rows["LST"].ToString());
                for (int i = 1; i < dtSet.Tables.Count; i++)
                {
                    dataRows = dtSet.Tables[i].Select(filter);
                    if (dataRows.Length > 0)
                    {
                        if (i != dtSet.Tables.Count - 1)
                            totalScore = totalScore + double.Parse(dataRows[0]["Score"].ToString());
                        sm.Append("<td class='tableRowFilter'>" + dataRows[0]["Score"] + "</td>");
                    }
                    else
                        sm.Append("<td class='tableRowFilter'>-</td>");
                }
                sm.Append("<td class='tableRowFilter'>" + Math.Round(totalScore, 1) + "</td>");
                sm.Append("</tr>");
            }
            sm.Append("</table>");
            sb.Append(sm.ToString());
            sb.Append("</div>");
            sb.Append("</td>");
            strSQL = string.Format("SELECT AVGSEMCScore,HazeScore, UVScore,AvgChinaScore,PersonScore FROM T_PersonScore WHERE LST ='{0}' and UserName='{1}' ", fromTime, userName);
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowFilterMonth'>当月平均</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["AVGSEMCScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["HazeScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["UVScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["AvgChinaScore"] + "</td>");
                sb.Append("<td class='tableRowFilterMonth'>" + dt.Rows[0]["PersonScore"] + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        public string retrunYearList(string time, string userName, string durarion, string haze, string Uv, string china, string totalScore)
        {
            string fromTime = time;
            string toTime = DateTime.Parse(time).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT LST,AVG(F) as Score FROM T_DurationEvaluation WHERE  LST between '{0}' and '{1}' and  Module='Manual' and DurationID in (2,3,6) and userID='{2}'  group by LST; ", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_HazeEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,Score FROM T_UVEvaluate WHERE  userID='{2}' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            strSQL = strSQL + string.Format("SELECT LST,S  as Score FROM T_ChinaEvaluation WHERE  userID='{2}' and Module='Manual' and LST between '{0}' and '{1}' order by LST;", fromTime, toTime, userName);
            DataSet dtSet = m_Database.GetDataset(strSQL);
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='5' class='tdInfo'>" + DateTime.Parse(time).ToString("yyyy年") + ":" + userName + "</td>");
            sb.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleFilter'>时间</td>");
            sb.Append("<td class='tabletitleFilter'>分时段预报</br>准确率</td>");
            sb.Append("<td class='tabletitleFilter'>霾预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>紫外线预报准确率</td>");
            sb.Append("<td class='tabletitleFilter'>国家局准确率</td>");
            sb.Append("<td class='tabletitleFilter'>总分</td>");
            sb.Append("</tr>");
            string filter = "";
            DataRow[] dataRows;
            double totaldayScore = 0.0;
            sb.Append("<tr>");
            sb.Append("<td colspan='6' >");
            sb.Append("<div class='fixDIv'>");
            sm.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            foreach (DataRow rows in dtSet.Tables[0].Rows)
            {
                totaldayScore = 0.0;
                sm.Append(string.Format("<tr onclick=\"trDayClick('{0}','{1}',this)\">", DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00:00"), userName));
                sm.Append("<td class='tableRowFilter'>" + DateTime.Parse(rows["LST"].ToString()).ToString("yyyy-MM-dd HH:00") + "</td>");
                sm.Append("<td class='tableRowFilter'>" + Math.Round(double.Parse(rows["Score"].ToString()), 1) + "</td>");
                totaldayScore = 0.6 * Math.Round(double.Parse(rows["Score"].ToString()), 1);
                filter = string.Format("LST='{0}'", rows["LST"].ToString());
                for (int i = 1; i < dtSet.Tables.Count; i++)
                {
                    dataRows = dtSet.Tables[i].Select(filter);
                    if (dataRows.Length > 0)
                    {
                        if (i != dtSet.Tables.Count - 1)
                            totalScore = totalScore + double.Parse(dataRows[0]["Score"].ToString());
                        sm.Append("<td class='tableRowFilter'>" + dataRows[0]["Score"] + "</td>");
                    }
                    else
                        sm.Append("<td class='tableRowFilter'>-</td>");
                }
                sm.Append("<td class='tableRowFilter'>" + Math.Round(totaldayScore, 1) + "</td>");
                sm.Append("</tr>");
            }
            sm.Append("</table>");
            sb.Append(sm.ToString());
            sb.Append("</div>");
            sb.Append("</td>");

            sb.Append("<tr>");
            sb.Append("<td class='tableRowFilterMonth'>当年平均</td>");
            sb.Append("<td class='tableRowFilterMonth'>" + durarion + "</td>");
            sb.Append("<td class='tableRowFilterMonth'>" + haze + "</td>");
            sb.Append("<td class='tableRowFilterMonth'>" + Uv + "</td>");
            sb.Append("<td class='tableRowFilterMonth'>" + china + "</td>");
            sb.Append("<td class='tableRowFilterMonth'>" + Math.Round(double.Parse(totalScore.ToString()), 1) + "</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            return sb.ToString();
        }
        public string retrunSingleEveryDay(string time, string userName, string durarion, string haze, string Uv, string china, string totalScore)
        {
            string fromTime = DateTime.Parse(time).ToString("yyyy-MM-dd HH:00:00");
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
            string strSQL = string.Format("SELECT DurationID, AQI, Module, f0, f2, f1, f3, f4, F FROM T_DurationEvaluation WHERE LST ='{0}'  and  Module in ('Manual','RT');", fromTime);
            strSQL = strSQL + string.Format("SELECT ForecastGrade05, ForecastGrade17, RtAllDay, RtDay FROM T_HazeEvaluate WHERE LST ='{0}' and UserID='{1}'; ", fromTime, userName);
            strSQL = strSQL + string.Format("SELECT ForecastGrade16, ForecastGrade10, RTGrade, Score FROM T_UVEvaluate WHERE LST ='{0}'; ", fromTime);
            strSQL = strSQL + string.Format("SELECT AQI, Module, f1, f2, f3, S FROM T_ChinaEvaluation WHERE LST ='{0}'  and  Module in ('Manual','RT');", fromTime);
            DataSet dtSet = m_Database.GetDataset(strSQL);

            sb.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='9' class='tdInfo'>单日成绩查询 " + DateTime.Parse(time).ToString("yyyy-MM-dd HH:00") + "</td>");
            sb.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleFilter2'>分时段预报</td>");
            sb.Append("<td class='tabletitleFilter3'>" + durarion + "</td>");
            sb.Append("<td class='tabletitleFilter2'>霾预报</td>");
            sb.Append("<td class='tabletitleFilter3'>" + haze + "</td>");
            sb.Append("<td class='tabletitleFilter2'>紫外线预报</td>");
            sb.Append("<td class='tabletitleFilter3'>" + Uv + "</td>");
            sb.Append("<td class='tabletitleFilter2'>国家局</td>");
            sb.Append("<td class='tabletitleFilter3'>" + china + "</td>");
            sb.Append("<td class='tabletitleFilter2'>总分</td>");
            sb.Append("<td class='tabletitleFilter2'>" + totalScore + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='10' class='tdInfo'></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='10' >");
            sb.Append("<div class='fixDIv'>");
            sb.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td  class='tabletitleFilterThird'></td>");
            sb.Append("<td  class='tabletitleFilterThird1'></td>");
            sb.Append("<td  class='tabletitleFilterThird2'></td>");
            sb.Append("<td  class='tabletitleFilterThird'>准确率</td>");
            sb.Append("<td  class='tabletitleFilterThird'>实况</td>");
            sb.Append("<td  class='tabletitleFilterThird'>预报</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div >");
            sb.Append("</td>");
            sb.Append("</tr>");


            string[] durationName = { "夜间", "上午", "下午" };
            int[] durationID = { 6, 2, 3 };
            string[] scoreItems = { "污染附加分(f0)", "首要污染物正确性评分(f1)", "级别准确性评分(f2)", "首要污染物iAQI精度正确性评分（f3）", "其他污染物iAQI精度正确性评分（f4）", "综合评分（F）" };
            sb.Append("<tr>");
            sb.Append("<td colspan='10' >");
            sb.Append("<div class='fixDIv'>");
            sm.Append("<table   width='100%' border='0'  cellpadding='0' cellspacing='0'>");
            string filter = ""; string filter1 = "";
            DataRow[] rows;
            DataRow[] shirows;
            for (int i = 0; i < durationName.Length; i++)
            {
                for (int j = 0; j < scoreItems.Length; j++)
                {
                    sm.Append("<tr>");
                    if (i == 0 && j == 0)
                        sm.Append("<td rowspan='" + durationName.Length * scoreItems.Length + "' class='tableRowFilterDay'>分段时间预报</td>");
                    if (j == 0)
                        sm.Append("<td rowspan='" + scoreItems.Length + "' class='tableRowFilterMonth1'>" + durationName[i] + "</td>");
                    sm.Append("<td  class='tableRowFilterMonth2'>" + scoreItems[j] + "</td>");
                    filter = string.Format("DurationID='{0}' and Module='Manual'", durationID[i]);
                    filter1 = string.Format("DurationID='{0}' and Module='RT'", durationID[i]);
                    rows = dtSet.Tables[0].Select(filter);
                    shirows = dtSet.Tables[0].Select(filter1);
                    if (rows.Length > 0)
                    {
                        sm.Append("<td  class='tableRowFilterDay'>" + rows[0][3 + j] + "</td>");
                        if (j == 0)
                        {
                            sm.Append("<td  class='tableRowFilterDay' rowspan='6'>" + shirows[0]["AQI"] + "</td>");
                            sm.Append("<td  class='tableRowFilterDay' rowspan='6'>" + rows[0]["AQI"] + "</td>");
                        }
                    }
                    else
                    {
                        sm.Append("<td  class='tableRowFilterDay'>-</td>");
                        if (j == 0)
                        {
                            sm.Append("<td  class='tableRowFilterDay' rowspan='6' >-</td>");
                            sm.Append("<td  class='tableRowFilterDay' rowspan='6'>-</td>");
                        }
                    }

                    sm.Append("</tr>");

                }
            }
            int[] hazeScore = new int[2];
            if (dtSet.Tables[1].Rows.Count > 0)
            {
                hazeScore = returnScore(int.Parse(dtSet.Tables[1].Rows[0]["RtAllDay"].ToString()), int.Parse(dtSet.Tables[1].Rows[0]["RtDay"].ToString()), int.Parse(dtSet.Tables[1].Rows[0]["ForecastGrade05"].ToString()), int.Parse(dtSet.Tables[1].Rows[0]["ForecastGrade17"].ToString()));
            }
            string[] hazeName = { "05时", "17时" };
            for (int i = 0; i < hazeScore.Length; i++)
            {
                sm.Append("<tr>");
                if (i == 0)
                    sm.Append("<td rowspan='2' class='tableRowFilterDay'>霾预报</td>");
                sm.Append("<td  class='tableRowFilterMonth1' colspan='2'>" + hazeName[i] + "</td>");
                sm.Append("<td  class='tableRowFilterMonth2'>" + hazeScore[i] + "</td>");
                if (dtSet.Tables[1].Rows.Count > 0)
                {
                    sm.Append("<td  class='tableRowFilterDay'>" + dtSet.Tables[1].Rows[0][2 + i] + "</td>");
                    sm.Append("<td  class='tableRowFilterDay'>" + dtSet.Tables[1].Rows[0][i] + "</td>");
                }
                else
                {
                    sm.Append("<td  class='tableRowFilterDay'>-</td>");
                    sm.Append("<td  class='tableRowFilterDay'>-</td>");
                }
                sm.Append("</tr>");
            }
            //紫外线
            string[] UVName = { "16时", "10时" };
            hazeScore = new int[2];
            if (dtSet.Tables[2].Rows.Count > 0)
            {
                hazeScore = returnUVScore(int.Parse(dtSet.Tables[2].Rows[0]["RTGrade"].ToString()), int.Parse(dtSet.Tables[2].Rows[0]["ForecastGrade16"].ToString()), int.Parse(dtSet.Tables[2].Rows[0]["ForecastGrade10"].ToString()));
            }
            for (int i = 0; i < hazeScore.Length; i++)
            {
                sm.Append("<tr>");
                if (i == 0)
                    sm.Append("<td rowspan='2' class='tableRowFilterDay'>紫外线预报</td>");
                sm.Append("<td  class='tableRowFilterMonth1' colspan='2'>" + hazeName[i] + "</td>");
                sm.Append("<td  class='tableRowFilterMonth2'>" + hazeScore[i] + "</td>");

                if (i == 0)
                {
                    if (dtSet.Tables[2].Rows.Count > 0)
                        sm.Append("<td  class='tableRowFilterDay' rowspan='2' >" + dtSet.Tables[2].Rows[0]["RTGrade"] + "</td>");
                    else
                        sm.Append("<td  class='tableRowFilterDay' rowspan='2' >-</td>");
                }
                if (dtSet.Tables[2].Rows.Count > 0)
                    sm.Append("<td  class='tableRowFilterDay'>" + dtSet.Tables[2].Rows[0][i] + "</td>");
                else
                    sm.Append("<td  class='tableRowFilterDay'>-</td>");
                sm.Append("</tr>");
            }
            string[] chinaName = { "首要污染物正确性评分", "AQI预报级别正确性评分", "AQI预报数值误差评分", "综合评分" };
            for (int i = 0; i < chinaName.Length; i++)
            {
                sm.Append("<tr>");
                if (i == 0)
                    sm.Append("<td rowspan='4' colspan='2' class='tableRowFilterDay'>国家局</td>");
                sm.Append("<td  class='tableRowFilterMonth1'>" + chinaName[i] + "</td>");
                filter = " Module='Manual'";
                filter1 = " Module='RT'";
                rows = dtSet.Tables[3].Select(filter);
                shirows = dtSet.Tables[3].Select(filter1);
                if (rows.Length > 0)
                {
                    sm.Append("<td  class='tableRowFilterDay'>" + rows[0][2 + i] + "</td>");
                    if (i == 0)
                    {
                        sm.Append("<td  class='tableRowFilterDay' rowspan='4'>" + shirows[0]["AQI"] + "</td>");
                        sm.Append("<td  class='tableRowFilterDay' rowspan='4'>" + rows[0]["AQI"] + "</td>");
                    }
                }
                else
                {
                    sm.Append("<td  class='tableRowFilterDay'>-</td>");
                    if (i == 0)
                    {
                        sm.Append("<td  class='tableRowFilterDay' rowspan='4'>-</td>");
                        sm.Append("<td  class='tableRowFilterDay' rowspan='4'>-</td>");
                    }
                }
                sm.Append("</tr>");
            }
            sm.Append("</table>");
            sb.Append(sm.ToString());
            sb.Append("</div>");
            sb.Append("</td>");
            return sb.ToString();
        }
        public int[] returnUVScore(int grade, int grade16, int grade10)
        {
            int[] UVScore = new int[2];
            int score = 0;
            if (grade == grade16)
                score = 6;
            else if (Math.Abs(grade16 - grade) == 1)
                score = 3;
            else
                score = 0;
            UVScore[0] = score;
            if (grade == grade10)
                score = 4;
            else if (Math.Abs(grade16 - grade) == 1)
                score = 2;
            else
                score = 0;
            UVScore[1] = score;
            return UVScore;
        }
        /// <summary>
        /// 求霾的总评分
        /// </summary>
        /// <param name="RtGrade05"></param>
        /// <param name="RtGrade17"></param>
        /// <param name="foreGrad05"></param>
        /// <param name="foreGrad17"></param>
        /// <returns></returns>
        public int[] returnScore(int RtGrade05, int RtGrade17, int foreGrad05, int foreGrad17)
        {
            int[] hazeScore = new int[2];
            int score = 0;
            int score1 = 0;
            if (RtGrade17 == foreGrad17)
                score = 20;
            else
            {
                if (RtGrade17 == 0)
                    score = 0;
                else
                    score = -8;
            }
            hazeScore[1] = score;
            if (RtGrade05 == foreGrad05)
                score1 = 10;
            else
            {
                if (RtGrade17 == 0)
                    score1 = 0;
                else
                    score1 = -8;
            }
            hazeScore[0] = score1;
            return hazeScore;

        }

        /// <summary>
        /// 返回分时段每日预报表格
        /// </summary>
        /// <param name="TimeDate"></param>
        /// <returns></returns>
        public string returnDayDurScore(string TimeDate)
        {
            try
            {
                //设置起止时间
                DateTime beginTime = DateTime.Parse(DateTime.Parse(TimeDate).ToString("yyyy-MM-dd"));
                DateTime endTime = beginTime.AddMonths(1).AddDays(-1);

                //从数据库中获取日评分
                string strSQL = string.Format("select * from T_DayDurScore where LST between '{0}' and '{1}'", beginTime, endTime);
                DataTable dayDurScore = m_Database.GetDataTable(strSQL);

                //生成HTML表格代码
                StringBuilder tableHTML = new StringBuilder();
                string[] durationID = { "6", "2", "3" };
                tableHTML.Append("<table width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
                tableHTML.Append("<tr>");
                tableHTML.Append("<td colspan='11' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>夜间</td>");
                tableHTML.Append("<td colspan='9' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>上午</td>");
                tableHTML.Append("<td colspan='10'style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>下午</td>");
                //tableHTML.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
                tableHTML.Append("</tr>");

                if (dayDurScore != null && dayDurScore.Rows.Count > 0)
                {
                    tableHTML.Append("<tr>");
                    for (int i = 0; i < durationID.Length; i++)
                    {
                        if (i == 0)
                        {
                            tableHTML.Append("<td class='tabletitleFilter'>时间</td>");
                            tableHTML.Append("<td class='tabletitleFilter'>预报员</td>");
                        }
                        tableHTML.Append("<td class='tabletitleFilter'>级别<br/>评分f1</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>首要污染物<br/>评分</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>精度评分<br/>PM2.5</td>");
                        if (i == 2)
                        {
                            tableHTML.Append("<td class='tabletitleFilter'>精度评分<br/>O3</td>");
                        }
                        tableHTML.Append("<td class='tabletitleFilter'>精度评分<br/>PM10</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>精度评分<br/>NO2</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>首要污染物<br/>精度评分</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>其它指标的<br/>IAQI精度评分</td>");
                        tableHTML.Append("<td class='tabletitleFilter'>污染物<br/>附加分</td>");
                        tableHTML.Append("<td class='tabletitleFilter3'>&nbsp总分&nbsp</td>");
                    }
                    tableHTML.Append("</tr>");

                    for (DateTime startTime = beginTime; startTime < endTime.AddDays(1); startTime = startTime.AddDays(1))
                    {
                        string filter = string.Empty;
                        tableHTML.Append("<tr>");
                        for (int j = 0; j < durationID.Length; j++)
                        {
                            filter = string.Format("LST='{0}' AND DurationID={1}", startTime.ToString(), durationID[j]);
                            DataRow[] drs = dayDurScore.Select(filter);
                            if (drs.Length > 0)
                            {
                                DataRow dr = drs[0];
                                if (durationID[j] == "6")
                                {
                                    tableHTML.Append("<td class='tableRowFilter'>" + Convert.ToDateTime(dr["LST"]).ToString("MM月dd日") + "</td>");
                                    tableHTML.Append("<td class='tableRowFilter'>" + dr["UserID"] + "</td>");

                                }
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f1"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f2"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f_PM25"])).ToString() + "</td>");
                                if (durationID[j] == "3")
                                {
                                    tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f_O3"])).ToString() + "</td>");
                                }
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f_PM10"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f_NO2"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f3"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f4"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["f0"])).ToString() + "</td>");
                                tableHTML.Append("<td class='tableRowFilter'>" + Math.Round(Convert.ToDouble(dr["F"])).ToString() + "</td>");
                            }
                        }
                        tableHTML.Append("</tr>");
                    }
                }
                else
                {
                    tableHTML.Append("<tr>");
                    tableHTML.Append("<td colspan='30' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>无数据</td>");
                    tableHTML.Append("</tr>");
                }
                tableHTML.Append("</table>");
                return tableHTML.ToString();
            }
            catch (Exception ex)
            { return ex.ToString(); }
        }

        /// <summary>
        /// 返回详细数据：分时段实况AQI和分时段个人预报 2018-3-12 by 孙明宇
        /// </summary>
        /// <param name="TimeDate"></param>
        /// <returns></returns>
        public string returnForeData(string TimeDate)
        {
            //设置起止时间
            DateTime beginTime = DateTime.Parse(DateTime.Parse(TimeDate).ToString("yyyy-MM-dd"));
            DateTime endTime = beginTime.AddMonths(1).AddDays(-1);
            //获取个人预报浓度
            string strSQL = string.Format("select * from T_EvaluateBasicData where date between '{0}' and '{1}'", beginTime, endTime);
            DataTable foreData = m_Database.GetDataTable(strSQL);
            //获取实况IAQI指数
            strSQL = string.Format("select * from T_shiTable where LST between '{0}' and '{1}'", beginTime, endTime);
            DataTable shiData = m_Database.GetDataTable(strSQL);
            //构建HTML字符串
            StringBuilder tableHTML = new StringBuilder();
            string[] durationID = { "6", "2", "3" };
            string filter = string.Empty;

            tableHTML.Append("<div id='closeBtn' class='closeBtn1' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div><div><table width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'><tr>");
            tableHTML.Append("<td colspan='15' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>AQI分时段（指数实况）</td>");
            tableHTML.Append("</tr>");
            tableHTML.Append("<tr>");
            tableHTML.Append("<td colspan='5' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>夜间</td>");
            tableHTML.Append("<td colspan='5' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>上午</td>");
            tableHTML.Append("<td colspan='5'style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>下午</td>");
            tableHTML.Append("<tr>");
            for (int ii = 0; ii < durationID.Length; ii++)
            {
                if (ii == 0)
                {
                    tableHTML.Append("<td class='tabletitleFilter4'>时间</td>");
                }
                tableHTML.Append("<td class='tabletitleFilter4'>PM2.5</td>");
                if (ii == 1 || ii == 2)
                { tableHTML.Append("<td class='tabletitleFilter4'>O3一小时</td>"); }
                tableHTML.Append("<td class='tabletitleFilter4'>O3八小时</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>PM10</td>");
                tableHTML.Append("<td class='tabletitleFilter5'>NO2</td>");
            }
            tableHTML.Append("</tr>");
            tableHTML.Append("</tr>");
            if (foreData != null && foreData.Rows.Count > 0)
            {

                //第一张表：分时段实况数据

                for (DateTime rtTime = beginTime; rtTime <= endTime; rtTime = rtTime.AddDays(1))
                {
                    string filters = string.Empty;
                    tableHTML.Append("<tr>");
                    for (int jj = 0; jj < durationID.Length; jj++)
                    {
                        filter = string.Format("LST='{0}' AND DurationID={1}", rtTime.ToString(), durationID[jj]);
                        DataRow[] drs = shiData.Select(filter, "ITEMID asc");
                        if (drs.Length > 0)
                        {
                            if (durationID[jj] == "6")
                            {
                                tableHTML.Append("<td class='tableRowFilterNew'>" + Convert.ToDateTime(drs[0]["LST"]).ToString("MM月dd日") + "</td>");
                            }
                            tableHTML.Append("<td class='tableRowFilterNew'>" + drs[1]["AQI"] + "</td>");
                            if (durationID[jj] == "2" || durationID[jj] == "3")
                            {
                                tableHTML.Append("<td class='tableRowFilterNew'>" + drs[4]["AQI"] + "</td>");
                            }
                            tableHTML.Append("<td class='tableRowFilterNew'>" + drs[5]["AQI"] + "</td>");
                            tableHTML.Append("<td class='tableRowFilterNew'>" + drs[2]["AQI"] + "</td>");
                            tableHTML.Append("<td class='tableRowFilterNew'>" + drs[3]["AQI"] + "</td>");
                        }
                    }
                    tableHTML.Append("</tr>");
                }
            }
            else
            {
                tableHTML.Append("<tr>");
                tableHTML.Append("<td colspan='15' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>无数据</td>");
                tableHTML.Append("</tr>");
            }
            tableHTML.Append("</table></div><br/><br/>");
           
            //第二张表：分时段个人预报
            tableHTML.Append("<div><table width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
            tableHTML.Append("<tr>");
            tableHTML.Append("<td colspan='16' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>个人预报（指数）</td>");
            tableHTML.Append("</tr>");
            tableHTML.Append("<tr>");
            tableHTML.Append("<td colspan='2' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'></td>");
            tableHTML.Append("<td colspan='4' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>夜间</td>");
            tableHTML.Append("<td colspan='5' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>上午</td>");
            tableHTML.Append("<td colspan='5'style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>下午</td>");
            //tableHTML.Append("<td  class='tdInfo'><div id='closeBtn' class='closeBtn' onmouseover=\"this.className='closeBtnover'\" onmouseout=\"this.className='closeBtn'\" onclick='fadeOut()'></div></td>");
            tableHTML.Append("</tr>");
            tableHTML.Append("<tr>");

            for (int i = 0; i < durationID.Length; i++)
            {
                if (i == 0)
                {
                    tableHTML.Append("<td class='tabletitleFilter4'>时间</td>");
                    tableHTML.Append("<td class='tabletitleFilter5'>预报员</td>");
                }
                tableHTML.Append("<td class='tabletitleFilter4'>PM2.5</td>");
                if (i == 1 || i == 2)
                { tableHTML.Append("<td class='tabletitleFilter4'>O3一小时</td>"); }
                tableHTML.Append("<td class='tabletitleFilter4'>O3八小时</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>PM10</td>");
                tableHTML.Append("<td class='tabletitleFilter5'>NO2</td>");
            }
            tableHTML.Append("</tr>");
            if (shiData != null && shiData.Rows.Count > 0)
            {
                for (DateTime startTime = beginTime; startTime <= endTime; startTime = startTime.AddDays(1))
                {
                    filter = string.Format("date='{0}'", startTime);
                    DataRow[] drs = foreData.Select(filter);
                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        tableHTML.Append("<tr>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Convert.ToDateTime(dr["date"]).ToString("MM月dd日") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToUserName(dr["number"].ToString()) + "</td>");
                        //以下半夜为夜间 2018-03-15 by 孙明宇
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["an_PM25"], "1") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["an_O38"], "5") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["an_PM10"], "2") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["an_NO2"], "3") + "</td>");
                        //tableHTML.Append("<td class='tableRowFilterNew'>" + toNight(ToAQI(dr["mn_PM25"], "1"), ToAQI(dr["an_PM25"], "1")) + "</td>");
                        //tableHTML.Append("<td class='tableRowFilterNew'>" + toNight(ToAQI(dr["mn_O38"], "5"), ToAQI(dr["an_O38"], "5")) + "</td>");
                        //tableHTML.Append("<td class='tableRowFilterNew'>" + toNight(ToAQI(dr["mn_PM10"], "2"), ToAQI(dr["an_PM10"], "2")) + "</td>");
                        //tableHTML.Append("<td class='tableRowFilterNew'>" + toNight(ToAQI(dr["mn_NO2"], "3"), ToAQI(dr["an_NO2"], "3")) + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["m_PM25"], "1") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["m_O31"], "4") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["m_O38"], "5") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["m_PM10"], "2") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["m_NO2"], "3") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["a_PM25"], "1") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["a_O31"], "4") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["a_O38"], "5") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["a_PM10"], "2") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + ToAQI(dr["a_NO2"], "3") + "</td>");
                        tableHTML.Append("</tr>");
                    }
                }
            }
            else
            {
                tableHTML.Append("<tr>");
                tableHTML.Append("<td colspan='16' style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>无数据</td>");
                tableHTML.Append("</tr>");
            }
            tableHTML.Append("</table></div>");

            return tableHTML.ToString();
        }

        /// <summary>
        /// 上半夜下半夜转化为夜间浓度 2018-3-12 by 孙明宇
        /// </summary>
        /// <param name="mn"></param>
        /// <param name="an"></param>
        /// <returns></returns>
        public string toNight(object mn, object an)
        {
            return Convert.ToInt16(0.4 * Convert.ToDouble(mn) + 0.6 * Convert.ToDouble(an)).ToString();
        }

        /// <summary>
        /// 污染物浓度转化为IAQI
        /// </summary>
        /// <param name="value"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int ToAQI(object value, string itemID)
        {
            int AQIValue = 0;
            if (value == null || value.ToString() == "") { return AQIValue; }
            double inputValue = Convert.ToDouble(value) / 1000;
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

        /// <summary>
        /// 工号对应姓名  2018-3-12 by 孙明宇
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string ToUserName(string number)
        {
            string name = string.Empty;
            switch (number)
            {
                case "159":
                    name = "毛卓成";
                    break;
                case "160":
                    name = "陈镭";
                    break;
                case "188":
                    name = "曹钰";
                    break;
                case "190":
                    name = "瞿元昊";
                    break;
                case "192":
                    name = "余钟奇";
                    break;
                case "199":
                    name = "周婉君";
                    break;
            }
            return name;
        }
        public string ReturnDayScore(string TimeDate)
        {
            try
            {
                //设置起止时间
                DateTime beginTime = DateTime.Parse(DateTime.Parse(TimeDate).ToString("yyyy-MM-dd"));
                DateTime endTime = beginTime.AddMonths(1).AddDays(-1);

                //从数据库中获取日评分
                string strSQL = string.Format("select * from T_DayScore where LST between '{0}' and '{1}' order by lst asc", beginTime, endTime);
                DataTable dayScore = m_Database.GetDataTable(strSQL);

                //生成HTML表格代码
                StringBuilder tableHTML = new StringBuilder();
                tableHTML.Append("<table width='100%' border='0' id='forecastTableMonth' cellpadding='0' cellspacing='0'>");
                tableHTML.Append("<tr>");
                tableHTML.Append("<td colspan='12'style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>日评分</td>");
                tableHTML.Append("<td rowspan='2'style='border:1px #ff000 solid;text-align:center;' class='tabletitleFilter5'>总评分</td>");
                tableHTML.Append("</tr>");
                tableHTML.Append("<tr>");
                tableHTML.Append("<td class='tabletitleFilter4'>时间</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>预报员</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>级别<br/>评分f1</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>首要污染物<br/>评分</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>精度评分<br/>PM2.5</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>精度评分<br/>O3-8小时</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>精度评分<br/>PM10</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>精度评分<br/>NO2</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>首要污染物<br/>精度评分</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>其它指标的<br/>IAQI精度评分</td>");
                tableHTML.Append("<td class='tabletitleFilter4'>污染物<br/>附加分</td>");
                tableHTML.Append("<td class='tabletitleFilter5'>日评分F</td>");
                tableHTML.Append("</tr>");
                if (dayScore != null && dayScore.Rows.Count > 0)
                {
                    for (int i = 0; i < dayScore.Rows.Count; i++)
                    {
                        DataRow dr = dayScore.Rows[i];
                        tableHTML.Append("<tr>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Convert.ToDateTime(dr["LST"]).ToString("MM月dd日") + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + dr["UserID"] + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f1"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f2"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f_PM25"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f_O3"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f_PM10"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f_NO2"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f3"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f4"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["f0"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["F"])).ToString() + "</td>");
                        tableHTML.Append("<td class='tableRowFilterNew'>" + Math.Round(Convert.ToDouble(dr["FF"])).ToString() + "</td>");
                        tableHTML.Append("</tr>");
                    }
                    tableHTML.Append("</table>");

                }
                else
                {
                    tableHTML.Append("<tr>");
                    tableHTML.Append("<td colspan='13'style='border:1px #ff000 solid;text-align:center;' class='tdInfo'>无数据</td>");
                    tableHTML.Append("</tr>");

                }
                return tableHTML.ToString();
            }
            catch (Exception ex) { return ex.ToString(); }
        }
    }
}
