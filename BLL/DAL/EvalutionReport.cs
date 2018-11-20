using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    public class EvalutionReport
    {
        private Database m_Database;
        public DataTable dIAQITable;
        public EvalutionReport()
        {
            m_Database = new Database();
        }
        public string CalculateIcsAndSts(string forecastDate)
        {
            string str = "";
            DateTime time = DateTime.Parse(forecastDate);
            TimeSpan span = (TimeSpan)(DateTime.Parse(forecastDate).AddMonths(1) - time);
            int days = span.Days;
            string str2 = DateTime.Parse(forecastDate).ToString("yyyy-MM-01 00:00:00");
            string str3 = DateTime.Parse(forecastDate).Date.AddMonths(1).AddDays(-1.0).ToString("yyyy-MM-dd 23:59:59");
            DateTime.Parse(forecastDate);
            string str4 = string.Format("select time_point,AQI from T_24hAQI where area='上海市' and time_point BETWEEN '{0}' and '{1}' and CONVERT(varchar(2),time_point, 114 )='20' ORDER by time_point", str2, str3);
            DataTable dataTable = this.m_Database.GetDataTable(str4);
            string str5 = string.Format("select ForecastDate,AQI from T_ForecastGroup where ForecastDate BETWEEN '{0}' and '{1}' and Period='24' and durationID='7' and (ITEMID='6' or ITEMID='0') and Module='WRF'", DateTime.Parse(forecastDate).AddDays(-1.0).ToString("yyyy-MM-01 00:00:00"), DateTime.Parse(forecastDate).Date.AddMonths(1).AddDays(-2.0).ToString("yyyy-MM-dd 23:59:59"));
            DataTable table2 = this.m_Database.GetDataTable(str5);
            string str6 = "";
            string str7 = "";
            double num = 0.0;
            double num2 = 0.0;
            int num3 = 0;
            int num4 = 0;
            double num5 = 0.0;
            double num6 = 0.0;
            try
            {
                if (dataTable.Rows.Count <= 0)
                {
                    return str;
                }
                double num7 = 0.0;
                double num8 = 0.0;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    str6 = dataTable.Rows[i]["AQI"].ToString();
                    string str8 = DateTime.Parse(dataTable.Rows[i]["time_point"].ToString()).ToString("yyyy-MM-dd 00:00:00.000");
                    for (int j = 0; j < table2.Rows.Count; j++)
                    {
                        if (DateTime.Parse(table2.Rows[j]["ForecastDate"].ToString()).AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss.000") == str8)
                        {
                            str7 = table2.Rows[j]["AQI"].ToString();
                            num3 = 0;
                            num4 = 0;
                            if (((str6 != "") && (str6 != null)) && ((str7 != "") && (str7 != null)))
                            {
                                num3 = Convert.ToInt32(str6);
                                num4 = Convert.ToInt32(str7);
                                if ((num4 > 150) && (num3 > 150))
                                {
                                    num = 1.0;
                                    num2 = 0.0;
                                }
                                else if ((num4 > 150) && (num3 < 150))
                                {
                                    num = 0.0;
                                    num2 = 1.0;
                                }
                                else if ((num4 < 150) && (num3 < 150))
                                {
                                    num = 0.0;
                                    num2 = 0.0;
                                }
                                else if ((num4 < 150) && (num3 > 150))
                                {
                                    num = 0.0;
                                    num2 = 0.0;
                                }
                                num5 = num;
                                num6 = num2;
                                num7 = num;
                                num8 = num5 - num6;
                                string str10 = str;
                                str = str10 + "\"" + DateTime.Parse(dataTable.Rows[i]["time_point"].ToString()).ToString("yyyy年MM月dd日") + "\":\"" + num7.ToString() + "&" + num8.ToString() + "\",";
                            }
                            else
                            {
                                str = str + "\"" + DateTime.Parse(dataTable.Rows[i]["time_point"].ToString()).ToString("yyyy年MM月dd日") + "\":\"-&-\",";
                            }
                        }
                    }
                }
                return ("{" + str.Trim(new char[] { ',' }) + "}");
            }
            catch
            {
            }
            return str;
        }
        
        /// <summary>
        /// 返回雾霾评分表 2017.7.10 by 孙明宇
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public string HazeScoreForReport(string dateTime)
        {
            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).Day;
            string strSQL = string.Format("SELECT  LST,Score05, Score17, RtAllDay as RtAllDay17,RTDays as RtAllDay05, NoneDays05, NoneDays17, FailDay05, FailDay17,RTDays-NoneDays05 as  CorrectDays05,RtAllDay-NoneDays17 as CorrectDays17 FROM T_HazeMoth WHERE LST='{0}'", dtFrom);//2017.7.10 by 孙明宇
            //string strSQL = string.Format("SELECT  LST,Score05, Score17, RtAllDay as RtAllDay05,RTDays as RtAllDay17, NoneDays05, NoneDays17, FailDay05, FailDay17, CorrectDays05, CorrectDays17 FROM T_HazeMoth WHERE LST='{0}'", dtFrom);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            string[] name = { "05时（08-20）", "17时（20-20）" };
            string[] nameStr = { "05", "17" };
            string[] titleName = { "实况", "准确预报", "空报", "漏报", "预报评分" };
            string[] titleStr = { "RtAllDay", "CorrectDays", "NoneDays", "FailDay", "Score" };
           
            StringBuilder sm = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                sm = new StringBuilder();
                for (int j = 0; j < titleName.Length; j++)
                {
                    if (hazeTable.Rows.Count > 0)
                    {
                        if (titleName[j] == "实况")
                        {
                            sb.Append("\"PO_noHaze" + nameStr[i] + "Fact" + "\":\"" + (totalDays - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString() + "\",");
                            sb.Append("\"PO_Haze" + nameStr[i] + "Fact" + "\":\"" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "\",");
                            sm.Append("\"PO_noHaze" + nameStr[i] + "Correct" + "\":\"" + (totalDays - int.Parse(hazeTable.Rows[0]["NoneDays" + nameStr[i]].ToString()) - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString() + "\",");
                            sm.Append("\"PO_Haze" + nameStr[i] + "Correct" + "\":\"" + (int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString()) - int.Parse(hazeTable.Rows[0]["FailDay" + nameStr[i]].ToString())).ToString() + "\",");
                        }
                        else if (titleName[j] == "准确预报")
                        {
                            sb.Append(sm.ToString());
                        }
                        else if (titleName[j] == "空报")
                        {
                            sb.Append("\"PO_noHaze" + nameStr[i] + "Null" + "\":\"" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "\",");
                            sb.Append("\"PO_Haze" + nameStr[i] + "Null" + "\":\"-\",");
                        }
                        else if (titleName[j] == "漏报")
                        {
                            sb.Append("\"PO_noHaze" + nameStr[i] + "Miss" + "\":\"-\",");
                            sb.Append("\"PO_Haze" + nameStr[i] + "Miss" + "\":\"" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "\",");
                        }
                        else if (titleName[j] == "预报评分")
                        {
                            sb.Append("\"PO_Haze" + nameStr[i] + "Score" + "\":\"" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "\",");
                        }

                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            //sb.Append("}");
            return sb.ToString();
        }
        
        /// <summary>
        /// 返回国家局评分表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ReturnChinaTableForReport(string dateTime)
        {
            try
            {
                string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
                string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                string[] modle = { "RT", "Manual", "WRF" };
                string[] modleName = { "实况", "主观预报", "WRF-chem" };
                string strSQL = string.Format("SELECT  Module,COUNT(Quality) as count,D_Parameter.MC from  T_ChinaEvaluation as a join  D_Parameter  on a.Quality=D_Parameter.MC Where LST between '{0}' and '{1}' group by Module,Quality,MC;", fromTime, toTime);
                strSQL = strSQL + string.Format("select Module, count(Parameter) as count,D_Item.MC from  T_ChinaEvaluation as a join  D_Item  on a.Parameter=D_Item.MC Where LST between '{0}' and '{1}' group by Module,Parameter,MC;", fromTime, toTime);
                strSQL = strSQL + string.Format("SELECT Module,avg(f1) as f1,avg(f2) as f2,avg(f3) as f3,avg( S) as s FROM  T_ChinaEvaluation where Module<>'RT' and LST between '{0}' and '{1}'  group by Module order by Module desc;", fromTime, toTime);
                DataSet dSet = m_Database.GetDataset(strSQL);
                DataTable dt;
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < dSet.Tables.Count; j++)
                {

                    //24小时空气质量预报质量表
                    dt = dSet.Tables[j];
                    if (j == 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //PO_GoodFact
                                sb.Append("\"PO_");
                                switch (dt.Rows[i]["MC"].ToString().Trim(' '))
                                {
                                    case "优":
                                        sb.Append("Good");
                                        break;
                                    case "良":
                                        sb.Append("Fine");
                                        break;
                                    case "轻度污染":
                                        sb.Append("Light");
                                        break;
                                    case "中度污染":
                                        sb.Append("Medium");
                                        break;
                                    case "重度污染":
                                        sb.Append("Heavy");
                                        break;

                                }
                                switch (dt.Rows[i]["Module"].ToString().Trim(' '))
                                {
                                    case "Manual":
                                        sb.Append("Sub");
                                        break;
                                    case "RT":
                                        sb.Append("Fact");
                                        break;
                                    case "WRF":
                                        sb.Append("WRF");
                                        break;
                                }
                                sb.Append("\":\"");
                                sb.Append(dt.Rows[i]["count"].ToString());
                                sb.Append("\",");
                            }
                        }
                    }

                    //首要污染物预报情况
                    if (j == 1)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //PO_GoodFact
                                sb.Append("\"PO_");
                                switch (dt.Rows[i]["MC"].ToString().Trim(' '))
                                {
                                    case "PM2.5":
                                        sb.Append("PM25");
                                        break;
                                    case "PM10":
                                        sb.Append("PM10");
                                        break;
                                    case "NO2":
                                        sb.Append("NO2");
                                        break;
                                    case "O3":
                                        sb.Append("O3");
                                        break;
                                }
                                switch (dt.Rows[i]["Module"].ToString().Trim(' '))
                                {
                                    case "Manual":
                                        sb.Append("Sub");
                                        break;
                                    case "RT":
                                        sb.Append("Fact");
                                        break;
                                    case "WRF":
                                        sb.Append("WRF");
                                        break;
                                }
                                sb.Append("\":\"");
                                sb.Append(dt.Rows[i]["count"].ToString());
                                sb.Append("\",");
                            }
                        }
                    }

                    //AQI主客观预报准确率及综合评分
                    if (j == 2)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //PO_GoodFact
                                //if (dt.Rows[i]["Module"].ToString() == "WRF")
                                //{
                                //    sb.Append("\"PO_MainWRF\":\"");
                                //    sb.Append(dt.Rows[i]["f1"].ToString() + "\",");
                                //    sb.Append("\"PO_CorrectWRF\":\"");
                                //    sb.Append(dt.Rows[i]["f2"].ToString() + "\",");
                                //    sb.Append("\"PO_ErrorWRF\":\"");
                                //    sb.Append(dt.Rows[i]["f3"].ToString() + "\",");
                                //    sb.Append("\"PO_MultiWRF\":\"");
                                //    sb.Append(dt.Rows[i]["s"].ToString() + "\",");
                                //}
                                //if (dt.Rows[i]["Module"].ToString() == "Manual")
                                //{
                                //    sb.Append("\"PO_MainSub\":\"");
                                //    sb.Append(dt.Rows[i]["f1"].ToString() + "\",");
                                //    sb.Append("\"PO_CorrectSub\":\"");
                                //    sb.Append(dt.Rows[i]["f2"].ToString() + "\",");
                                //    sb.Append("\"PO_ErrorSub\":\"");
                                //    sb.Append(dt.Rows[i]["f3"].ToString() + "\",");
                                //    sb.Append("\"PO_MultiSub\":\"");
                                //    sb.Append(dt.Rows[i]["s"].ToString() + "\",");
                                //}

                                if (dt.Rows[i]["Module"].ToString() == "WRF")
                                {
                                    sb.Append("\"PO_MainWRF\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f1"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_CorrectWRF\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f2"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_ErrorWRF\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f3"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_MultiWRF\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["s"].ToString()).ToString("0.0") + "\",");
                                }
                                if (dt.Rows[i]["Module"].ToString() == "Manual")
                                {
                                    sb.Append("\"PO_MainSub\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f1"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_CorrectSub\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f2"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_ErrorSub\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["f3"].ToString()).ToString("0.0") + "\",");
                                    sb.Append("\"PO_MultiSub\":\"");
                                    sb.Append(Convert.ToDouble(dt.Rows[i]["s"].ToString()).ToString("0.0") + "\",");
                                }

                            }
                        }
                    }

                }
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                return sb.ToString();
            }
            catch { }
            return "";
        }
        
        /// <summary>
        /// 返回分段评分表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ReturnDurationTableForReport(string dateTime)
        {
            try
            {
                string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");

                string[] modle = { "RT", "Manual", "ManualSubmit", "ManualCenter", "WRF" };
                string[] modleName = { "实况", "气象部门", "环保部门", "两家合作", "WRF-chem" };
                string strSQL = string.Format("SELECT DurationID, Module,COUNT(Quality) as count,D_Parameter.MC from  T_DurationEvaluation as a join  D_Parameter  on a.Quality=D_Parameter.MC Where LST between '{0}' and '{1}'  group by Module,DurationID, Quality,MC;", fromTime, toTime);
                strSQL = strSQL + string.Format("select DurationID, Module,count(Parameter) as count,D_Item.MC from  T_DurationEvaluation as a join  D_Item  on a.Parameter=D_Item.MC Where LST between '{0}' and '{1}'  group by Module,DurationID,Parameter,MC;", fromTime, toTime);
                strSQL = strSQL + string.Format("SELECT DurationID, ITEMID,D_Item.MC, Module, AVG(Score) AS Score FROM T_IAQIEvaluation join D_Item  on T_IAQIEvaluation.ITEMID=D_Item.DM   Where LST between '{0}' and '{1}' GROUP BY DurationID, ITEMID, Module,MC ;", fromTime, toTime);

                DataSet dSet = m_Database.GetDataset(strSQL);
                DataTable tempTable = dSet.Tables[2].DefaultView.ToTable(true, "ITEMID");
                strSQL = string.Format("SELECT AVG(F) as F,Module,DurationID FROM T_DurationEvaluation where module<>'RT' and LST between '{0}' and '{1}' group by Module,DurationID ", fromTime, toTime);
                DataTable AllScore = m_Database.GetDataTable(strSQL);
                string[][] para = new string[2][];
                para[0] = new string[6] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
                if (tempTable.Rows.Count > 3)
                    para[1] = new string[4] { "PM2.5", "PM10", "NO2", "O3" };
                else
                    para[1] = new string[3] { "PM2.5", "PM10", "NO2" };
                string[] duration = { "6", "2", "3" };
                string[] durationName = { "夜间", "上午", "下午" };
                string filter = "";
                DataRow[] dataRows;
                DataTable dt;
                int flag = 0;
                StringBuilder sb = new StringBuilder();
                StringBuilder sbReturn = new StringBuilder("{");
                for (int j = 0; j < dSet.Tables.Count; j++)
                {
                    dt = dSet.Tables[j];

                    //空气质量等级预报情况(2 上午 3 下午  6 夜间)
                    if (j == 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sb.Append("\"PO_");
                                switch (dt.Rows[i]["DurationID"].ToString())
                                {
                                    case "6":
                                        sb.Append("night_");
                                        break;
                                    case "2":
                                        sb.Append("am_");
                                        break;
                                    case "3":
                                        sb.Append("pm_");
                                        break;
                                }
                                switch (dt.Rows[i]["MC"].ToString().Trim(' '))
                                {
                                    case "优":
                                        sb.Append("Good");
                                        break;
                                    case "良":
                                        sb.Append("Fine");
                                        break;
                                    case "轻度污染":
                                        sb.Append("Light");
                                        break;
                                    case "中度污染":
                                        sb.Append("Medium");
                                        break;
                                    case "重度污染":
                                        sb.Append("Heavy");
                                        break;
                                }
                                switch (dt.Rows[i]["Module"].ToString().Trim(' '))
                                {
                                    case "ManualCenter":
                                        sb.Append("Co");
                                        break;
                                    case "Manual":
                                        sb.Append("MA");
                                        break;
                                    case "ManualSubmit":
                                        sb.Append("EP");
                                        break;
                                    case "RT":
                                        sb.Append("Fact");
                                        break;
                                    case "WRF":
                                        sb.Append("WRF");
                                        break;
                                }
                                sb.Append("\":\"");
                                sb.Append(dt.Rows[i]["count"].ToString());
                                sb.Append("\",");
                            }
                        }
                    }

                    //首要污染物预报情况(2 上午 3 下午  6 夜间)
                    if (j == 1)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sb.Append("\"PO_");
                                switch (dt.Rows[i]["DurationID"].ToString().Trim(' '))
                                {
                                    case "6":
                                        sb.Append("night_");
                                        break;
                                    case "2":
                                        sb.Append("am_");
                                        break;
                                    case "3":
                                        sb.Append("pm_");
                                        break;
                                }
                                switch (dt.Rows[i]["MC"].ToString().Trim(' '))
                                {
                                    case "PM10":
                                        sb.Append("PM10");
                                        break;
                                    case "PM2.5":
                                        sb.Append("PM25");
                                        break;
                                    case "NO2":
                                        sb.Append("NO2");
                                        break;
                                    //case "O3":
                                    //    sb.Append("O3");
                                    //    break;

                                }
                                switch (dt.Rows[i]["Module"].ToString().Trim(' '))
                                {
                                    case "ManualCenter":
                                        sb.Append("Co");
                                        break;
                                    case "Manual":
                                        sb.Append("MA");
                                        break;
                                    case "ManualSubmit":
                                        sb.Append("EP");
                                        break;
                                    case "RT":
                                        sb.Append("Fact");
                                        break;
                                    case "WRF":
                                        sb.Append("WRF");
                                        break;
                                }
                                sb.Append("\":\"");
                                sb.Append(dt.Rows[i]["count"].ToString());
                                sb.Append("\",");
                            }
                        }
                    }

                    //IAQI主客观预报准确率及综合评分
                    if (j == 2)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int m = 0; m < duration.Length; m++)
                            {
                                for (int i = 1; i < modleName.Length; i++)
                                {

                                    //if (i == 1)
                                    //    sb.Append("<td class='tableRow'  rowspan='4'>" + durationName[m] + "</td>");
                                    //sb.Append("<td class='tableRow' >" + modleName[i] + "</td>");

                                    //for (int k = 0; k < para[j].Length; k++)
                                    //{
                                    //    filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                                    //    dataRows = dt.Select(filter);
                                    //    if (dataRows.Length > 0)
                                    //        sb.Append("<td class='tableRowChild'>" + dataRows[0][2] + "</td>");
                                    //    else
                                    //        sb.Append("<td class='tableRowChild'>0</td>");

                                    //}


                                    for (int k = 0; k < para[1].Length; k++)
                                    {
                                        filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", modle[i], para[1][k], duration[m]);
                                        dataRows = dt.Select(filter);
                                        //if (dataRows.Length > 0)
                                        //    sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(dataRows[0][4].ToString()), 1) + "</td>");
                                        //else
                                        //    sb.Append("<td class='tableRowChild'>0</td>");
                                        if (dataRows.Length > 0)
                                        {
                                            sb.Append("\"PO_IAQI_");
                                            switch (duration[m])
                                            {
                                                case "6":
                                                    sb.Append("night_");
                                                    break;
                                                case "2":
                                                    sb.Append("am_");
                                                    break;
                                                case "3":
                                                    sb.Append("pm_");
                                                    break;
                                            }
                                            switch (para[1][k])
                                            {
                                                case "PM10":
                                                    sb.Append("PM10");
                                                    break;
                                                case "PM2.5":
                                                    sb.Append("PM25");
                                                    break;
                                                case "NO2":
                                                    sb.Append("NO2");
                                                    break;
                                                //case "O3":
                                                //    sb.Append("O3");
                                                //    break;
                                            }
                                            switch (modle[i])
                                            {
                                                case "ManualCenter":
                                                    sb.Append("Co");
                                                    break;
                                                case "Manual":
                                                    sb.Append("MA");
                                                    break;
                                                case "ManualSubmit":
                                                    sb.Append("EP");
                                                    break;
                                                case "RT":
                                                    sb.Append("Fact");
                                                    break;
                                                case "WRF":
                                                    sb.Append("WRF");
                                                    break;
                                            }
                                            sb.Append("\":\"");
                                            sb.Append(Math.Round(double.Parse(dataRows[0][4].ToString()), 1));
                                            sb.Append("\",");
                                        }
                                    }
                                    filter = string.Format("Module='{0}'  and DurationID='{1}'", modle[i], duration[m]);
                                    dataRows = AllScore.Select(filter);
                                    //PO_IAQI_night_MultiEP
                                    if (dataRows.Length > 0)
                                    {
                                        sb.Append("\"PO_IAQI_");
                                        switch (duration[m])
                                        {
                                            case "6":
                                                sb.Append("night_");
                                                break;
                                            case "2":
                                                sb.Append("am_");
                                                break;
                                            case "3":
                                                sb.Append("pm_");
                                                break;
                                        }
                                        sb.Append("Multi");
                                        switch (modle[i])
                                        {
                                            case "ManualCenter":
                                                sb.Append("Co");
                                                break;
                                            case "Manual":
                                                sb.Append("MA");
                                                break;
                                            case "ManualSubmit":
                                                sb.Append("EP");
                                                break;
                                            case "RT":
                                                sb.Append("Fact");
                                                break;
                                            case "WRF":
                                                sb.Append("WRF");
                                                break;
                                        }

                                        sb.Append("\":\"");
                                        sb.Append(Math.Round(double.Parse(dataRows[0][0].ToString()), 1));
                                        sb.Append("\",");
                                    }
                                    //else
                                    //    sb.Append("<td class='tableRowChild'>0</td>");
                                    //sb.Append("</tr>");
                                }
                            }
                        }
                    }
                }


                //strSQL = string.Format("SELECT LST, DurationID, Module, Quality, Grade FROM T_DurationEvaluation Where LST between '{0}' and '{1}';", fromTime, toTime);
                //strSQL = strSQL + string.Format("SELECT LST, DurationID, Module, Quality, Grade,Parameter FROM T_DurationEvaluation Where LST between '{0}' and '{1}';", fromTime, toTime);
                //DataSet dtSet = m_Database.GetDataset(strSQL);
                //for (int j = 0; j < dtSet.Tables.Count; j++)
                //{
                //    dt = dtSet.Tables[j];
                //    //sb = new StringBuilder();
                //    //sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
                //    //sb.Append("<tr>");
                //    //sb.Append("<td class='tabletitleChild'></td>");
                //    //sb.Append("<td class='tabletitle'></td>");
                //    //for (int k = 0; k < para[j].Length; k++)
                //    //{
                //    //    sb.Append("<td class='tabletitleChild'>" + para[j][k] + "</td>");
                //    //}
                //    //sb.Append("</tr>");
                //    int totalDay = 0;
                //    double day = 0;
                //    for (int m = 0; m < duration.Length; m++)
                //    {
                //        for (int i = 1; i < modleName.Length; i++)
                //        {
                //            //sb.Append("<tr>");
                //            //if (i == 1)
                //            //    sb.Append("<td class='tableRow'  rowspan='4'>" + durationName[m] + "</td>");
                //            //sb.Append("<td class='tableRow' >" + modleName[i] + "</td>");

                //            for (int k = 0; k < para[j].Length; k++)
                //            {
                //                day = 0;
                //                //if (j == 0)
                //                //    filter = string.Format("Module='{0}' and Quality='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                //                //else
                //                //    filter = string.Format("Module='{0}' and Parameter='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                //                if (j == 0)
                //                {
                //                    filter = string.Format("Module='{0}' and Quality='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                //                    dataRows = dt.Select(filter);
                //                    totalDay = dataRows.Length;

                //                    if (dataRows.Length > 0)
                //                    {
                //                        foreach (DataRow dayRows in dataRows)
                //                        {

                //                            //IAQI主客观预报准确率及综合评分
                //                            if (j == 0)
                //                            {
                //                                sb.Append("\"PO_IAQI_");
                //                                filter = string.Format("Module='RT' and Quality='{1}' and DurationID='{2}' and LST='{3}'", modle[i], para[j][k], duration[m], DateTime.Parse(dayRows[0].ToString()).ToString("yyyy-MM-dd HH:00:00"));

                //                                day = day + double.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                //                                switch (duration[m])
                //                                {
                //                                    case "6":
                //                                        sb.Append("night_");
                //                                        break;
                //                                    case "2":
                //                                        sb.Append("am_");
                //                                        break;
                //                                    case "3":
                //                                        sb.Append("pm_");
                //                                        break;
                //                                }
                //                                switch (para[1][k])
                //                                {
                //                                    case "PM10":
                //                                        sb.Append("PM10");
                //                                        break;
                //                                    case "PM2.5":
                //                                        sb.Append("PM25");
                //                                        break;
                //                                    case "NO2":
                //                                        sb.Append("NO2");
                //                                        break;
                //                                    //case "O3":
                //                                    //    sb.Append("O3");
                //                                    //    break;
                //                                }
                //                                switch (modle[i])
                //                                {
                //                                    case "ManualCenter":
                //                                        sb.Append("Co");
                //                                        break;
                //                                    case "Manual":
                //                                        sb.Append("MA");
                //                                        break;
                //                                    case "ManualSubmit":
                //                                        sb.Append("EP");
                //                                        break;
                //                                    case "RT":
                //                                        sb.Append("Fact");
                //                                        break;
                //                                    case "WRF":
                //                                        sb.Append("WRF");
                //                                        break;
                //                                }
                //                                sb.Append("\":\"");
                //                                sb.Append(Math.Round(day * 100 / totalDay, 1));
                //                                sb.Append("\",");
                //                            }
                //                            //else
                //                            //    filter = string.Format("Module='RT' and Parameter='{1}' and DurationID='{2}' and LST='{3}'", modle[i], para[j][k], duration[m], DateTime.Parse(dayRows[0].ToString()).ToString("yyyy-MM-dd HH:00:00"));
                //                            //day = day + double.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                //                        }
                //                        //sb.Append("<td class='tableRowChild'>" + Math.Round(day * 100 / totalDay, 1) + "</td>");
                //                    }
                //                    //else
                //                    //{
                //                    //    sb.Append("\":\"");
                //                    //    sb.Append(0);
                //                    //    sb.Append("\",");
                //                    //}
                //                }

                //            }
                //        }
                //    }
                //}
                if (sb.Length > 1)
                {
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                    //sb.Append("}");
                return sb.ToString();
            }
            catch { }
            return "";
        }
        
        /// <summary>
        /// 紫外线评分
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string UVScore(string dateTime)
        {
            try
            {
                string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<5  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<5  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<5 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);
                DataSet hazeTable = m_Database.GetDataset(strSQL);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
                sb.Append("<tr>");
                string[] name = { "实况", "预报（16时）", "预报（10时）", "评分" };
                sb.Append("<td class='tabletitleHaze' ></td>");
                sb.Append("<td class='tabletitleHaze'>1级</td>");
                sb.Append("<td class='tabletitleHaze'>2级</td>");
                sb.Append("<td class='tabletitleHaze'>3级</td>");
                sb.Append("<td class='tabletitleHaze'>4级</td>");
                sb.Append("<td class='tabletitleHaze'>5级</td>");
                sb.Append("</tr>");
                DataTable dt = new DataTable();
                dt = hazeTable.Tables[3];
                int totalDays = 0;
                string score = "";
                string filter = "";
                int tempCOunt = 0;
                int count = 0;
                DataRow[] rows;
                if (dt.Rows.Count > 0)
                {
                    totalDays = int.Parse(dt.Rows[0][0].ToString());
                    score = Math.Round(double.Parse(dt.Rows[0][1].ToString() == "" ? "0" : dt.Rows[0][1].ToString()), 1).ToString();
                }
                for (int i = 0; i < hazeTable.Tables.Count; i++)
                {
                    dt = hazeTable.Tables[i];
                    sb.Append("<tr>");
                    count = 0;
                    sb.Append("<td class='tableRowHaze' >" + name[i] + "</td>");
                    if (i != hazeTable.Tables.Count - 1)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            if (j != 5)
                            {
                                filter = "count='" + j + "'";
                                rows = dt.Select(filter);
                                if (rows.Length > 0)
                                {
                                    count = count + int.Parse(rows[0][0].ToString());
                                    sb.Append("<td class='tableRowHaze' >" + rows[0][0].ToString() + "</td>");
                                }
                                else
                                    sb.Append("<td class='tableRowHaze' >0</td>");
                            }
                            else
                            {
                                tempCOunt = totalDays - count;
                                sb.Append("<td class='tableRowHaze' >" + tempCOunt + "</td>");
                            }

                        }
                    }
                    else
                    {
                        sb.Append("<td class='tableRowHaze' colspan='5'>" + score + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                return sb.ToString();
            }
            catch { }
            return "";
        }
        
        /// <summary>
        /// 返回紫外线评分表备份
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string UVScoreForReportCopy(string dateTime)
        {
            try
            {
                string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                //string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<5  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<5  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<5 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);

                string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<6  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<6  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<6 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);

                DataSet hazeTable = m_Database.GetDataset(strSQL);
                StringBuilder sb = new StringBuilder();
                string[] name = { "实况", "预报（16时）", "预报（10时）", "评分" };

                DataTable dt = new DataTable();
                dt = hazeTable.Tables[3];
                int totalDays = 0;
                string score = "";
                string filter = "";
                int tempCOunt = 0;
                int count = 0;
                DataRow[] rows;
                if (dt.Rows.Count > 0)
                {
                    totalDays = int.Parse(dt.Rows[0][0].ToString());
                    score = Math.Round(double.Parse(dt.Rows[0][1].ToString() == "" ? "0" : dt.Rows[0][1].ToString()), 1).ToString();
                }
                for (int i = 0; i < hazeTable.Tables.Count; i++)
                {
                    dt = hazeTable.Tables[i];
                    count = 0;
                    if (i != hazeTable.Tables.Count - 1)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            //if (j != 5)
                            //{
                            //    filter = "count='" + j + "'";
                            //    rows = dt.Select(filter);
                            //    if (rows.Length > 0)
                            //    {
                            //        count = count + int.Parse(rows[0][0].ToString());
                            //        sb.Append("<td class='tableRowHaze' >" + rows[0][0].ToString() + "</td>");
                            //    }
                            //    else
                            //        sb.Append("<td class='tableRowHaze' >0</td>");
                            //}
                            //else
                            //{
                            //    tempCOunt = totalDays - count;
                            //    sb.Append("<td class='tableRowHaze' >" + tempCOunt + "</td>");
                            //}
                            if (j != 5)
                            {
                                filter = "count='" + j + "'";
                                rows = dt.Select(filter);
                                if (j == 1)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                    }

                                    //else
                                    //    sb.Append("<td class='tableRowHaze' >0</td>");
                                }
                                else if (j == 2)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv2Fact\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv2Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv2Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }

                                    //else
                                    //    sb.Append("<td class='tableRowHaze' >0</td>");
                                }
                                else if (j == 3)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv3Fact\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv3Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv3Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }

                                    //else
                                    //    sb.Append("<td class='tableRowHaze' >0</td>");
                                }
                                else if (j == 4)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv4Fact\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv4Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv4Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                            }
                            else if (j == 5)
                            {
                                tempCOunt = totalDays - count;
                                if (i == 0)
                                {
                                    sb.Append("\"PO_uv5Fact\":\"" + tempCOunt + "\",");
                                }
                                else if (i == 1)
                                {
                                    sb.Append("\"PO_uv5Forecast16\":\"" + tempCOunt + "\",");
                                }
                                else if (i == 2)
                                {
                                    sb.Append("\"PO_uv5Forecast10\":\"" + tempCOunt + "\",");
                                }
                            }
                        }
                    }
                    else
                    {
                        sb.Append("\"PO_uvForecastScore\":\"" + score + "\"");

                    }
                }
                return sb.ToString();
            }
            catch { }
            return "";
        }
        
        /// <summary>
        /// 返回紫外线评分表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string UVScoreForReport(string dateTime)
        {
            try
            {
                string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
                //string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<5  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<5  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<5 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
                //strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);

                string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<6  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<6  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<6 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
                strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", dtFrom, dtTo);

                DataSet hazeTable = m_Database.GetDataset(strSQL);
                StringBuilder sb = new StringBuilder();
                string[] name = { "实况", "预报（16时）", "预报（10时）", "评分" };

                DataTable dt = new DataTable();
                dt = hazeTable.Tables[3];
                int totalDays = 0;
                string score = "";
                string filter = "";
                //int tempCOunt = 0;
                int count = 0;
                DataRow[] rows;
                if (dt.Rows.Count > 0)
                {
                    totalDays = int.Parse(dt.Rows[0][0].ToString());
                    score = Math.Round(double.Parse(dt.Rows[0][1].ToString() == "" ? "0" : dt.Rows[0][1].ToString()), 1).ToString();
                }
                for (int i = 0; i < hazeTable.Tables.Count; i++)
                {
                    dt = hazeTable.Tables[i];
                    count = 0;
                    if (i != hazeTable.Tables.Count - 1)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            //if (j != 5)
                            //{
                            filter = "DM='" + j + "'";
                            rows = dt.Select(filter);
                                if (j == 1)//2017.7.10 by 孙明宇
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv1Fact\":\"" + rows[0][0].ToString() + "\",");
                                            sb.Append("\"PO_uv1FactText\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv1Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv1Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                                
                                else if (j == 2)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv2Fact\":\"" + rows[0][0].ToString() + "\",");
                                            sb.Append("\"PO_uv2FactText\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv2Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv2Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                                else if (j == 3)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv3Fact\":\"" + rows[0][0].ToString() + "\",");
                                            sb.Append("\"PO_uv3FactText\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv3Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv3Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                                else if (j == 4)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv4Fact\":\"" + rows[0][0].ToString() + "\",");
                                            sb.Append("\"PO_uv4FactText\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv4Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv4Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                                else if (j == 5)
                                {
                                    if (rows.Length > 0)
                                    {
                                        count = count + int.Parse(rows[0][0].ToString());
                                        if (i == 0)
                                        {
                                            sb.Append("\"PO_uv5Fact\":\"" + rows[0][0].ToString() + "\",");
                                            sb.Append("\"PO_uv5FactText\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 1)
                                        {
                                            sb.Append("\"PO_uv5Forecast16\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                        else if (i == 2)
                                        {
                                            sb.Append("\"PO_uv5Forecast10\":\"" + rows[0][0].ToString() + "\",");
                                        }
                                    }
                                }
                            //}
                            //else if (j == 5)
                            //{
                            //    tempCOunt = totalDays - count;
                            //    if (i == 0)
                            //    {
                            //        sb.Append("\"PO_uv5Fact\":\"" + tempCOunt + "\",");
                            //    }
                            //    else if (i == 1)
                            //    {
                            //        sb.Append("\"PO_uv5Forecast16\":\"" + tempCOunt + "\",");
                            //    }
                            //    else if (i == 2)
                            //    {
                            //        sb.Append("\"PO_uv5Forecast10\":\"" + tempCOunt + "\",");
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        sb.Append("\"PO_uvForecastScore\":\"" + score + "\",");
                        sb.Append("\"PO_Evaluation\":\"" + score + "\"");

                    }
                }
                return sb.ToString();
            }
            catch { }
            return "";
        }
        /// <summary>
        /// 自动生成霾报告 2017.7.10 by 孙明宇
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public string HazeIntroduce(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string month = DateTime.Parse(dateTime).ToString("M月");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT LST,RtAllDay,RtDay FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' AND (RtAllDay=1);", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            DataRow[] Rows = dt.Select();
            int upMonth=0;
            int midMonth=0;
            int downMonth=0;
            TimeSpan shangxun=new TimeSpan(10, 0, 0, 0);
            TimeSpan xiaxun=new TimeSpan(20,0,0,0);
            foreach (DataRow row in Rows)
            {

                if (Convert.ToInt32(row[2]) > 0)
                {
                    if (Convert.ToDateTime(row[0]) - DateTime.Parse(dateTime) < shangxun)
                    {
                        upMonth++;
                    }
                    else if (Convert.ToDateTime(row[0]) - DateTime.Parse(dateTime) >= shangxun
                        && Convert.ToDateTime(row[0]) - DateTime.Parse(dateTime) < xiaxun)
                    {
                        midMonth++;
                    }
                    else if (Convert.ToDateTime(row[0]) - DateTime.Parse(dateTime) >= xiaxun)
                    {
                        downMonth++;
                    }
                }
            }
            int sum=upMonth+midMonth+downMonth;
            StringBuilder sb = new StringBuilder();
            sb.Append("\"PO_SHWeatherReport\":\"").Append(month).Append("份，上海出现").Append(sum.ToString()).Append("个霾日，");
            if (sum > 0)
            {
                sb.Append("其中");
                if (upMonth > 0)
                { 
                    sb.Append("上旬").Append(upMonth.ToString()).Append("天，"); 
                }
                if (midMonth > 0)
                { 
                    sb.Append("中旬").Append(midMonth.ToString()).Append("天，"); 
                }
                if (downMonth > 0)
                { 
                    sb.Append("下旬").Append(downMonth.ToString()).Append("天，"); 
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("(霾日的统计时效为20-20时）。预报质量见表1。\"");
            
            return sb.ToString();
        }
        /// <summary>
        /// 自动生成紫外线报告 2017.7.10 by 孙明宇
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string UVIntroduce(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string month = DateTime.Parse(dateTime).ToString("M月");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("select RTGrade,COUNT(RTGrade) as RT FROM T_UVEvaluate WHERE LST BETWEEN '{0}' and '{1}' group by RTGrade ORDER BY RTGrade;", fromTime, toTime);
            strSQL = strSQL + string.Format("SELECT count( *) as count,avg(ScoreUV) FROM T_UVEvaluate  WHERE LST BETWEEN '{0}' and '{1}'  ;", fromTime, toTime);
            DataSet UVData = m_Database.GetDataset(strSQL);
            DataTable dt =UVData.Tables[0];
            DataTable sdt=UVData.Tables[1];
            DataRow[] rows = dt.Select();
            StringBuilder sb = new StringBuilder();
            sb.Append("\"PO_SHUVReport\":\"").Append(month).Append("紫外线预报评分").Append(Math.Round(double.Parse(sdt.Rows[0][1].ToString() == "" ? "0" : sdt.Rows[0][1].ToString()), 1).ToString()).Append("。其中");
            foreach (DataRow row in rows)
            {
                sb.Append(row[0].ToString()).Append("级出现").Append(row[1].ToString()).Append("天，");
            }
            sb.Append("未出现");
            for (int i = 1; i < 6; i++)
            {
                DataRow[] exist = dt.Select("RTGrade='" + i.ToString() + "'");
                if (exist.Length == 0)
                {
                    sb.Append(i.ToString()).Append("级和");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1).Append("。预报质量见表8。\"");
            }
            return sb.ToString();

            
        }

        //环境气象预报质量评定通报获取多个评分表格数据 2017.7.10 by 孙明宇
        public string GetEnvReportAllContent(string dateTime)
        {
            string strHazeJson = HazeScoreForReport(dateTime);
            if (strHazeJson != "")
            {
                strHazeJson += ",";
            }
            string strChinaEvaJson = ReturnChinaTableForReport(dateTime);
            if (strChinaEvaJson != "")
            {
                strChinaEvaJson += ",";
            }
            string strDuraJson = ReturnDurationTableForReport(dateTime);
            if (strDuraJson != "")
            {
                strDuraJson += ",";
            }
            string strUVJson = UVScoreForReport(dateTime);
            if (strUVJson != "")
            {
                strUVJson += ",";
            }
            string strHazeIntroJson = HazeIntroduce(dateTime);
            if (strHazeIntroJson != "")
            {
                strHazeIntroJson += ",";
            }
            string strUVIntroJson = UVIntroduce(dateTime);
            return "{" + strHazeJson + strChinaEvaJson  + strDuraJson +strUVJson+strHazeIntroJson+strUVIntroJson+ "}";
        }
    }
}
