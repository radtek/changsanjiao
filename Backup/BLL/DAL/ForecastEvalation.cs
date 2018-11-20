using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Readearth.Data;
using System.Collections;
using Readearth.Data.Entity;
using ChinaAQI;
using System.Configuration;

namespace MMShareBLL.DAL
{
    public class ForecastEvaluation
    {
        DataSetForcast.EvalutionDTDataTable evalutionDt = new DataSetForcast.EvalutionDTDataTable();
        DataSetForcast.ForecastDataTable forecastDt = new DataSetForcast.ForecastDataTable();//预报和实测的分析数据
        DataSetForcast.EvalutionDTDataTable evaDt = new DataSetForcast.EvalutionDTDataTable();
        DataSetForcast.FODataTable fODt = new DataSetForcast.FODataTable();
        private Database m_Database;
        private int m_BackDays;


        public ForecastEvaluation() {
            m_Database = new Database();
            m_BackDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
        }

        /// <summary>
        /// 查询考核数据
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="count"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public string GetEvaluationString(string fromDate, string toDate)
        {
            //获取考核数据，按照时间段
            string strSQL = "SELECT * FROM T_Evaluation WHERE LST between '{0}' and '{1}' order by Lst";
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            strSQL = string.Format(strSQL, startDate, endDate);
            DataTable db = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder();
            string str = "<table id='{0}' width='100%' border='0' cellpadding='0' cellspacing='0' class='tablekuang'>";
            sb.AppendLine(string.Format(str, "table1"));
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th class='tabletitle'>起报时间</th>");
            sb.AppendLine("<th class='tabletitle' >夜间</th>");
            sb.AppendLine("<th class='tabletitle' >上午</th>");
            sb.AppendLine("<th class='tabletitle' >下午</th>");
            sb.AppendLine("<th class='tabletitle' >日平均</th>");
            sb.AppendLine("<th class='tabletitle' >总计得分</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("{0}");
            sb.AppendLine("</tbody>");
            sb.Append("</table>");

            StringBuilder htmlSbd = new StringBuilder();
            if (db != null && db.Rows.Count > 0)
            {
                evaDt.Clear();
                #region 考核数据处理
                foreach (DataRow row in db.Rows)
                {
                    string LST = row["LST"].ToString().Trim();
                    DataRow[] rows = evaDt.Select("LST = '" + LST + "' ");
                    if (rows == null || rows.Length <= 0)
                    {
                        DataRow newRow = evaDt.NewRow();
                        newRow["LST"] = row["LST"].ToString().Trim();
                        evaDt.Rows.Add(newRow);
                        rows = evaDt.Select("LST = '" + LST + "' ");
                    }
                    if (rows != null && rows.Length > 0)
                    {
                        if (row["duration"].ToString() == "夜晚")
                            rows[0]["Night"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "上午")
                            rows[0]["AM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "下午")
                            rows[0]["PM"] = row["Score"].ToString().Trim();

                        if (row["duration"].ToString() == "全天")
                        {
                            rows[0]["DayAVG"] = row["Score"].ToString().Trim();
                            rows[0]["SocreCount"] = row["ScoreCount"].ToString().Trim();
                        }
                    }
                }
                #endregion ss

                string tr = "<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>";
                float totleScore = 0f;
                foreach (DataRow row in evaDt.Rows)
                {

                    htmlSbd.Append(string.Format(tr, Guid.NewGuid().ToString()));
                    string  scount = "0";
                    string night = row["Night"].ToString();
                    string AM = row["AM"].ToString();
                    string PM = row["PM"].ToString();
                    string DayAVG = row["DayAVG"].ToString();
                    scount = Proces(night, AM, PM, DayAVG);
                    foreach (DataColumn dc in evaDt.Columns)
                    {
                        string td = "<td class='tablerow' id='{1}'>{0}</td>";
                        string value = row[dc.ColumnName].ToString();
                        #region 
                        if (string.IsNullOrEmpty(value))
                        {
                            value = "/";
                        }
                        if (dc.ColumnName == "SocreCount")
                        {

                            float v = 0f;
                            float.TryParse(row[dc.ColumnName].ToString(), out v);
                            totleScore += (v);
                            value=scount;
                        }
                        if (dc.ColumnName == "LST")
                        {
                            value = DateTime.Parse(value).ToString("yyyy年MM月dd日");
                        }
                        htmlSbd.Append(string.Format(td, value, Guid.NewGuid().ToString()));
                        #endregion
                    }
                    htmlSbd.Append("</tr>");
                }
            }

            string htmlstr = string.Format(sb.ToString(),
                             htmlSbd.ToString());
            return htmlstr;
        }

        /// <summary>
        /// 计算总积分
        /// </summary>
        /// <param name="night"></param>
        /// <param name="am"></param>
        /// <param name="pm"></param>
        /// <param name="qt"></param>
        /// <returns></returns>
        private string Proces(string night ,string am, string pm, string qt) {
            //判断是否到了第二个时间段
            float yw = 0f;
            try
            {
                float.TryParse(night, out yw);
            }
            catch { }
            float sw = 0f;
            try
            {
                float.TryParse(am, out sw);
            }
            catch { }
            float xw = 0f;
            try
            {
                float.TryParse(pm, out xw);
            }
            catch { }
            float rpj = 0f;
            try
            {
                float.TryParse(qt, out rpj);//日平均
            }
            catch { }
            string Fcount = (yw * 0.3 +
                             sw * 0.3 +
                             xw * 0.3 +
                             rpj * 0.1).ToString();

            float fc = 0f;
            float.TryParse(Fcount, out fc);
            if (fc <= 0f)
                fc = 0f;
            else if (fc > 100f)
                fc = 100f;

            return fc.ToString();
        }


        /// <summary>
        /// 重载筛选
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="period"></param>
        /// <param name="forecasPeriod"></param>
        /// <param name="dataType"></param>
        /// <param name="dataModule"></param>
        /// <returns></returns>
        public void GetEvaluationDataTables(string fromDate, string toDate, string period, 
                                            string forecasPeriod, string dataType, string dataModule)
        {
            DataSet ds = StrSQLString(fromDate, toDate, period, forecasPeriod, dataType, dataModule);
            int count = 0;
            string[] durationID;
            if (forecasPeriod != "")
            {
                durationID = forecasPeriod.Split(',');
                count = durationID.Length;
            }
            TableEvaluationString(ds, count, fromDate, toDate);
        }


        public DataSet StrSQLString(string fromDate, string toDate, string period, 
                                    string forecasPeriod, string dataType, string dataModule)
        {
            string str;
            int num2;
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endTime = DateTime.Parse(toDate).AddDays(1);
            int length = 0;
            string str3 = string.Concat(new object[] { "LST BETWEEN  '", startDate, "' AND '", endTime, "' AND PERIOD =", period });
            string str2 = string.Concat(new object[] { "LST BETWEEN  '", startDate, "' AND '", endTime, "'" });
            if (forecasPeriod != "")
            {
                string[] strArray = forecasPeriod.Split(new char[] { ',' });
                length = strArray.Length;
                str = "(";
                for (num2 = 0; num2 < strArray.Length; num2++)
                {
                    str = str + strArray[num2] + ",";
                }
                str = str.Substring(0, str.Length - 1) + ")";
                str2 = str2 + " AND durationID IN" + str;
                str3 = str3 + " AND durationID IN" + str;
            }
            if (dataModule != "")
            {
                string[] strArray2 = dataModule.Split(new char[] { ',' });
                str = "(";
                for (num2 = 0; num2 < strArray2.Length; num2++)
                {
                    str = str + "'" + strArray2[num2] + "',";
                }
                str = str.Substring(0, str.Length - 1) + ")";
                str3 = str3 + " AND Module  IN" + str;
            }
            else
            {
                str3 = str3 + " AND Module  =''";
            }
            str = "select T.*, B.MC,B.indexD from (SELECT LST, ForecastDate, PERIOD, Module, durationID, "+
                   "ITEMID, Value, AQI,Parameter FROM T_ForecastGroup WHERE " + str3 + ") T INNER JOIN  "+
                   "(SELECT DM,MC,indexD FROM D_DurationTest)  B on  T.durationID=B.DM ORDER BY T.LST,B.indexD";
            DataTable dataTable = this.m_Database.GetDataTable(str);
            DataTable dm = new DataTable();
            if (dataType != "")
            {
                str = "SELECT LST,durationID, ITEMID, Value, AQI,Parameter,Quality  FROM T_ObsDataGroup Where " + str2 + "ORDER BY ITEMID";
                dm = this.m_Database.GetDataTable(str);
            }
            DataSet set = this.tableUnion(dataTable, dm, startDate, endTime, forecasPeriod, period);
            DataTable table = this.tableAQI(dataTable, dm, startDate, endTime, forecasPeriod, period);
            set.Tables.Add(table);
            return set;
        }
        
        public DataSet tableUnion(DataTable dt, DataTable dm, DateTime startDate, DateTime endTime, string durationID, string period)
        {
            DataSet set = new DataSet();
            try
            {
                DataTable table = new DataTable("T_ForecastSearch");
                table.Columns.Add("LST", typeof(string));
                table.Columns.Add("sectionStr", typeof(string));
                table.Columns.Add("timeSpan", typeof(string));
                table.Columns.Add("PERIOD", typeof(string));
                table.Columns.Add("shiCeValue", typeof(double));
                table.Columns.Add("comValue", typeof(double));
                table.Columns.Add("CMAQValue", typeof(double));
                table.Columns.Add("WRFValue", typeof(double));
                table.Columns.Add("shiCeAQI", typeof(int));
                table.Columns.Add("comAQI", typeof(int));
                table.Columns.Add("CMAQAQI", typeof(int));
                table.Columns.Add("WRFAQI", typeof(int));
                table.Columns.Add("Quality", typeof(string));
                table.Columns.Add("Parameter", typeof(string));
                string filterExpression = "";
                string queryString = "SELECT DM,MC,Description FROM D_DurationTest";
                DataTable dataTable = this.m_Database.GetDataTable(queryString);
                for (int i = 1; i <= 5; i++)
                {
                    DataTable table3 = table.Clone();
                    table3.TableName = string.Format("table{0}", i);
                    string[] strArray = durationID.Split(new char[] { ',' });
                    TimeSpan span = (TimeSpan) (endTime - startDate);
                    int days = span.Days;
                    for (int j = 0; j < Math.Abs(days); j++)
                    {
                        if (durationID != "")
                        {
                            #region init
                            for (int k = 0; k < strArray.Length; k++)
                            {
                                DataRow row;
                                int num5;
                                DataRow row2;
                                string[] durationSpan;
                                string str3;
                                string str4;
                                DataRow[] rowArray2;
                                DataRow[] rowArray3;
                                double num6;
                                filterExpression = string.Format("ITEMID={0} AND LST>='{1}' AND LST<'{2}' AND durationID={3}", new object[] { i, startDate.AddDays((double) j).ToString(), startDate.AddDays((double) (j + 1)).ToString(), int.Parse(strArray[k].ToString()) });
                                DataRow[] rowArray = dt.Select(filterExpression);
                                if (rowArray.Length > 0)
                                {
                                    row = rowArray[0];
                                    num5 = int.Parse(strArray[k].ToString());
                                    row2 = table3.NewRow();
                                    row2[0] = DateTime.Parse(row[0].ToString()).ToLongDateString();
                                    row2[3] = row[2].ToString() + "小时";
                                    durationSpan = this.GetDurationSpan(dataTable, num5);
                                    str3 = string.Format("{0}:00", int.Parse(durationSpan[0])) + "-" + string.Format("{0}:00", int.Parse(durationSpan[1]));
                                    row2[2] = str3;
                                    str4 = string.Format("DM={0}", num5);
                                    rowArray2 = dataTable.Select(str4);
                                    row2[1] = rowArray2[0][2].ToString();
                                    foreach (DataRow row3 in rowArray)
                                    {
                                        string str6 = row3[3].ToString();
                                        if (str6 != null)
                                        {
                                            if (!(str6 == "CMAQ"))
                                            {
                                                if (str6 == "Manual")
                                                {
                                                    goto Label_04BF;
                                                }
                                                if (str6 == "WRF")
                                                {
                                                    goto Label_0574;
                                                }
                                            }
                                            else
                                            {
                                                if (row3[6].ToString() == "")
                                                {
                                                    row2[6] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    row2[6] = Math.Round(double.Parse(row3[6].ToString()), 1);
                                                }
                                                if (row3[7].ToString() == "")
                                                {
                                                    row2[10] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    row2[10] = int.Parse(row3[7].ToString());
                                                }
                                            }
                                        }
                                        continue;
                                    Label_04BF:
                                        if (row3[6].ToString() == "")
                                        {
                                            row2[5] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[5] = Math.Round(double.Parse(row3[6].ToString()), 1);
                                        }
                                        if (row3[7].ToString() == "")
                                        {
                                            row2[9] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[9] = int.Parse(row3[7].ToString());
                                        }
                                        continue;
                                    Label_0574:
                                        if (row3[6].ToString() == "")
                                        {
                                            row2[7] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[7] = Math.Round(double.Parse(row3[6].ToString()), 1);
                                        }
                                        if (row3[7].ToString() == "")
                                        {
                                            row2[11] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[11] = int.Parse(row3[7].ToString());
                                        }
                                    }
                                    if (dm.Rows.Count > 0)
                                    {
                                        rowArray3 = dm.Select(filterExpression);
                                        if (rowArray3.Length > 0)
                                        {
                                            num6 = 0.0;
                                            double.TryParse(rowArray3[0][3].ToString(), out num6);
                                            row2[4] = Math.Round(num6, 1);
                                            double.TryParse(rowArray3[0][4].ToString(), out num6);
                                            row2[8] = num6.ToString();
                                            row2["Quality"] = rowArray3[0]["Quality"].ToString();
                                            row2["Parameter"] = rowArray3[0]["Parameter"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        row2[4] = DBNull.Value;
                                        row2[8] = DBNull.Value;
                                        row2["Quality"] = "";
                                        row2["Parameter"] = "";
                                    }
                                    table3.Rows.Add(row2);
                                }
                                else if (dm.Rows.Count > 0)
                                {
                                    rowArray3 = dm.Select(filterExpression);
                                    if (rowArray3.Length > 0)
                                    {
                                        row = rowArray3[0];
                                        num5 = int.Parse(strArray[k].ToString());
                                        row2 = table3.NewRow();
                                        row2[0] = DateTime.Parse(row[0].ToString()).ToLongDateString();
                                        row2[3] = period + "小时";
                                        durationSpan = this.GetDurationSpan(dataTable, num5);
                                        str3 = string.Format("{0}:00", int.Parse(durationSpan[0])) + "-" + string.Format("{0}:00", int.Parse(durationSpan[1]));
                                        row2[2] = str3;
                                        str4 = string.Format("DM={0}", num5);
                                        rowArray2 = dataTable.Select(str4);
                                        row2[1] = rowArray2[0][2].ToString();
                                        num6 = double.Parse(rowArray3[0][3].ToString());
                                        row2[4] = Math.Round(num6, 1);
                                        row2[8] = int.Parse(rowArray3[0][4].ToString());
                                        row2["Quality"] = rowArray3[0]["Quality"].ToString();
                                        row2["Parameter"] = rowArray3[0]["Parameter"].ToString();
                                        table3.Rows.Add(row2);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    set.Tables.Add(table3);
                }
            }
            catch (Exception)
            {
            }
            return set;
        
        }

        public DataTable tableAQI(DataTable dt, DataTable dm, DateTime startDate, DateTime endTime, string duration, string period)
        {
            DataTable table = new DataTable("T_ForecastAQI");
            table.Columns.Add("LST", typeof(string));
            table.Columns.Add("sectionStr", typeof(string));
            table.Columns.Add("timeSpan", typeof(string));
            table.Columns.Add("PERIOD", typeof(string));
            table.Columns.Add("shiceAQI", typeof(int));
            table.Columns.Add("shiceParameter", typeof(string));
            table.Columns.Add("comAQI", typeof(int));
            table.Columns.Add("comParameter", typeof(string));
            table.Columns.Add("CMAQAQI", typeof(int));
            table.Columns.Add("CMAQParameter", typeof(string));
            table.Columns.Add("WRFAQI", typeof(int));
            table.Columns.Add("WRFParameter", typeof(string));
            table.Columns.Add("Quality", typeof(string));
            table.Columns.Add("Parameter", typeof(string));
            string filterExpression = "";
            string queryString = "SELECT DM,MC,Description FROM D_DurationTest";
            DataTable dataTable = this.m_Database.GetDataTable(queryString);
            DataTable table3 = table.Clone();
            table3.TableName = string.Format("table{0}", 0);
            string[] strArray = duration.Split(new char[] { ',' });
            for (int i = 0; i < Math.Abs((int)(endTime.Day - startDate.Day)); i++)
            {
                if (duration != "")
                {
                    for (int j = 0; j < strArray.Length; j++)
                    {
                        DataRow row;
                        int num3;
                        DataRow row2;
                        string[] durationSpan;
                        string str3;
                        string str4;
                        DataRow[] rowArray2;
                        DataRow[] rowArray3;
                        float num4;
                        filterExpression = string.Format("ITEMID={0} AND LST>='{1}' AND LST<'{2}' AND durationID={3}", 
                                                          new object[] { 0, startDate.AddDays((double)i).ToString(), 
                                                              startDate.AddDays((double)(i + 1)).ToString(),
                                                                     int.Parse(strArray[j].ToString()) });
                        DataRow[] rowArray = dt.Select(filterExpression);
                        if (rowArray.Length > 0)
                        {
                            #region 
                            row = rowArray[0];
                            num3 = int.Parse(strArray[j].ToString());
                            row2 = table3.NewRow();
                            row2[0] = DateTime.Parse(row[0].ToString()).ToLongDateString();
                            row2[3] = row[2].ToString() + "小时";
                            durationSpan = this.GetDurationSpan(dataTable, num3);
                            str3 = string.Format("{0}:00", int.Parse(durationSpan[0])) + "-" + string.Format("{0}:00", int.Parse(durationSpan[1]));
                            row2[2] = str3;
                            str4 = string.Format("DM={0}", num3);
                            rowArray2 = dataTable.Select(str4);
                            row2[1] = rowArray2[0][2].ToString();
                            foreach (DataRow row3 in rowArray)
                            {
                                string str8 = row3[3].ToString();
                                if (str8 != null)
                                {
                                    if (!(str8 == "CMAQ"))
                                    {
                                        if (str8 == "Manual")
                                        {
                                            goto Label_047F;
                                        }
                                        if (str8 == "WRF")
                                        {
                                            goto Label_051A;
                                        }
                                    }
                                    else
                                    {
                                        if (row3[7].ToString() == "")
                                        {
                                            row2[8] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[8] = int.Parse(row3[7].ToString());
                                        }
                                        if (row3[8].ToString() == "")
                                        {
                                            row2[9] = DBNull.Value;
                                        }
                                        else
                                        {
                                            row2[9] = row3[8].ToString();
                                        }
                                    }
                                }
                                continue;
                            Label_047F:
                                if (row3[7].ToString() == "")
                                {
                                    row2[6] = DBNull.Value;
                                }
                                else
                                {
                                    row2[6] = int.Parse(row3[7].ToString());
                                }
                                if (row3[8].ToString() == "")
                                {
                                    row2[7] = DBNull.Value;
                                }
                                else
                                {
                                    row2[7] = row3[8].ToString();
                                }
                                continue;
                            Label_051A:
                                if (row3[7].ToString() == "")
                                {
                                    row2[10] = DBNull.Value;
                                }
                                else
                                {
                                    row2[10] = int.Parse(row3[7].ToString());
                                }
                                if (row3[8].ToString() == "")
                                {
                                    row2[11] = DBNull.Value;
                                }
                                else
                                {
                                    row2[11] = row3[8].ToString();
                                }
                            }
                            if (dm.Rows.Count > 0)
                            {
                                rowArray3 = dm.Select(filterExpression);
                                if (rowArray3.Length > 0)
                                {
                                    string str6 = rowArray3[0][5].ToString();
                                    row2[5] = str6;
                                    num4 = 0f;
                                    float.TryParse(rowArray3[0][4].ToString(), out num4);
                                    row2[4] = num4.ToString();
                                    row2["Quality"] = rowArray3[0]["Quality"].ToString();
                                    row2["Parameter"] = rowArray3[0]["Parameter"].ToString();
                                }
                            }
                            else
                            {
                                row2[4] = DBNull.Value;
                                row2[5] = DBNull.Value;
                                row2["Quality"] = "";
                                row2["Parameter"] = "";
                            }
                            table3.Rows.Add(row2);
                            #endregion
                        }
                        else if (dm.Rows.Count > 0)
                        {
                            rowArray3 = dm.Select(filterExpression);
                            if (rowArray3.Length > 0)
                            {
                                row = rowArray3[0];
                                num3 = int.Parse(strArray[j].ToString());
                                row2 = table3.NewRow();
                                row2[0] = DateTime.Parse(row[0].ToString()).ToLongDateString();
                                row2[3] = period + "小时";
                                durationSpan = this.GetDurationSpan(dataTable, num3);
                                str3 = string.Format("{0}:00", int.Parse(durationSpan[0])) + "-" +
                                       string.Format("{0}:00", int.Parse(durationSpan[1]));
                                row2[2] = str3;
                                str4 = string.Format("DM={0}", num3);
                                rowArray2 = dataTable.Select(str4);
                                row2[1] = rowArray2[0][2].ToString();
                                string str7 = row[5].ToString();
                                row2[5] = str7;
                                num4 = 0f;
                                float.TryParse(row[4].ToString(), out num4);
                                row2[4] = num4.ToString();
                                row2["Quality"] = rowArray3[0]["Quality"].ToString();
                                row2["Parameter"] = rowArray3[0]["Parameter"].ToString();
                                table3.Rows.Add(row2);
                            }
                        }
                    }
                }
            }
            return table3;

        }

        public void TableEvaluationString(DataSet temp, int count, string fromDate, string toDate)
        {
            try
            {
                ProcessDataForecast(temp, count, fromDate, toDate);
                ProcessForecastData(fromDate);
                GetEvalautionDT(fromDate, toDate);
                ProcessEvaluationData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessForecastData(string fromDate) {

            if (forecastDt != null && forecastDt.Rows.Count > 0) {

                string lstTime = "";
                foreach (DataRow row in forecastDt.Rows) {
                    string rq = row["RQ"].ToString();
                    if (rq == "") {
                        rq = lstTime;
                        row["RQ"] = lstTime;
                    }
                    lstTime = rq;
                }

                string lastDate = "";
                string lastSDMC = "";
                int rowIndex=0;
                int posIndex = 0;
                DataRow addRow = forecastDt.NewRow();
                DataRow addRowII = forecastDt.NewRow();
                DataRow addRowIII = forecastDt.NewRow();
                foreach (DataRow row in forecastDt.Rows) {
                    string rq = row["rq"].ToString();
                    if (rq.IndexOf('年') >= 0) {
                        lastDate = rq;
                    }
                    if (string.IsNullOrEmpty(rq)) {
                        rq = lastDate;
                    }
                    string sdmc = row["sdmc"].ToString();
                    if (sdmc == "夜晚") {
                       //取得前一个日期的夜晚信息
                        string frontDate=DateTime.Parse(rq).AddDays(-1).ToString("yyyy年M月d日");
                        DataRow[] rows=forecastDt.Select("RQ='" + frontDate + "' and SDMC='"+sdmc+"'");
                        if (rows != null && rows.Length > 0)
                        {
                            foreach (DataColumn dc in forecastDt.Columns)
                            {
                                addRowIII[dc] = row[dc];//重新赋值
                            }
                            foreach (DataColumn dc in forecastDt.Columns)
                            {

                                if (addRowII["RQ"].ToString() =="" &&
                                    addRowII["SDMC"].ToString() == "")
                                {
                                    if (dc.ColumnName != "RQ")
                                        row[dc] = rows[0][dc];//重新赋值
                                }
                                else
                                {
                                    if (dc.ColumnName != "RQ")
                                        row[dc] = addRowII[dc];//重新赋值
                                }
                            }
                        }
                        foreach (DataColumn dc in forecastDt.Columns)
                        {
                            addRowII[dc] = addRowIII[dc];//重新赋值
                        }

                    }
                  
                    //处理没有夜晚的情况
                    if(sdmc=="全天"){
                        if (lastSDMC != "夜晚")
                        {
                            string frontDate = DateTime.Parse(rq).AddDays(-1).ToString("yyyy年M月d日");
                            DataRow[] rows = forecastDt.Select("RQ='" + frontDate + "' and SDMC='夜晚'");
                            if (rows != null && rows.Length > 0) {
                                  foreach (DataColumn dc in forecastDt.Columns)
                                  {
                                      if (dc.ColumnName == "RQ")
                                      {
                                          addRow[dc] = DateTime.Parse(rq).ToString("yyyy年M月d日");
                                      }
                                      else {
                                          addRow[dc] = addRowII[dc];//重新赋值
                                      }
                                  }
                                  posIndex = rowIndex;
                            }
                        }
                    }
                    lastSDMC = sdmc;
                    rowIndex++;
                }
                if (addRow != null ) {
                    if (addRow["YBSX"].ToString() != "")
                    {
                        forecastDt.Rows.InsertAt(addRow, rowIndex-1);
                    }
                }
                //删除前面几行数据
                if (forecastDt != null && forecastDt.Rows.Count > 0) {
                        for(int i=0;i<4;i++)
                        {
                            forecastDt.Rows.RemoveAt(0);
                        }
                }

                lstTime = "";
                foreach (DataRow row in forecastDt.Rows)
                {
                    string rq = row["RQ"].ToString();
                    if (rq == lstTime) {
                        row["RQ"] = "";
                    }
                    lstTime = rq;
                }

                foreach (DataRow row in forecastDt.Rows)
                {
                    string rq = row["RQ"].ToString();
                    if(rq!=""){
                        row["RQ"] = DateTime.Parse(rq).AddDays(-1).ToString("yyyy年M月d日");
                    }

                }

            }
        }

        private void ProcessDataForecast(DataSet temp, int count, string fromDate, string toDate)
        {
            #region 数据处理
            //forecastDt.Clear();
            //int z = 0;
            //int w = 0;
            //int h = 0;
            //int t = 2;
            //for (int i = 0; i < temp.Tables.Count; i++)
            //{
            //    DataTable dt = temp.Tables[i];
            //    string tableName = dt.TableName.ToString();
            //    int items = int.Parse(tableName.Substring(5, 1));
            //    int m = 0;
            //    int k = 0;
            //    int x = 0;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        #region 判断要添加的数据列集合只有首次查询才整合
            //        if (i == 0)
            //        {
            //            DataRow forecastRow = forecastDt.NewRow();
            //            forecastDt.Rows.Add(forecastRow);
            //        }
            //        k++;
            //        if (z == 0)
            //            x = 0;
            //        else
            //            x = 4;

            //        if (i == temp.Tables.Count - 1)
            //        {
            //            t = 0;
            //        }

            //        for (int j = x; j < dt.Columns.Count - t; j++)
            //        {
            //            if (j == 0)
            //            {
            //                if (m == 0 || m % count == 0)
            //                {
            //                    //添加日期
            //                    forecastDt.Rows[m]["RQ"] = dr[j].ToString();
            //                }
            //            }
            //            else
            //            {
            //                #region 处理对应的预报和实测数据
            //                string value = "";
            //                if ((items == 0 && (j == 4 || j == 6 || j == 8 || j == 10)) || ((j == 9 || j == 8 || j == 10 || j == 11) && items != 0))
            //                {
            //                    if (dr[j].ToString() != "")
            //                        value = dr[j].ToString();
            //                    else
            //                        value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();

            //                }
            //                else
            //                    value = dr[j].ToString() == "" ? "/ " : dr[j].ToString();


            //                forecastDt.Rows[m][w + 1] = value;
            //                w++;
            //                h = w;
            //                #endregion
            //            }
            //        }
            //        w = z;
            //        m++;
            //        #endregion
            //    }
            //    z = h;
            //    w = h;
            //}
            #endregion

            #region 数据处理 new 
            forecastDt.Clear();
            foreach (DataTable dt in temp.Tables)
            {
                //添加表头信息
                int rowindex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = forecastDt.NewRow();
                    string LST = row["LST"].ToString().Trim();
                    int columnindex = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dt.TableName != "table0")
                        {
                            if (columnindex < 12)
                            {
                                if (dt.TableName == "table1")
                                {
                                    string oldLST = "x";
                                    if (rowindex > 0 && (columnindex==0))
                                    {
                                        oldLST =
                                               forecastDt.Rows[rowindex - 1]["RQ"].ToString();
                                    }
                                    if (row["sectionStr"].ToString() == "上午") {
                                        newRow[columnindex] = row[dc].ToString();
                                    }
                                    if (LST != oldLST && (oldLST!=""))
                                    { 
                                        newRow[columnindex] = row[dc].ToString();
                                    }
                                }
                                else { 
                                     //填充其他列
                                    if (columnindex > 3)  //从索引4开始赋值
                                    {
                                        int c=(columnindex-4)+12;
                                        c = c + (int.Parse(dt.TableName.Replace("table","").ToString()) - 2)*8;
                                        forecastDt.Rows[rowindex][c] = row[dc].ToString();
                                    }
                                }
                            }
                        }
                        else if (dt.TableName == "table0")
                        {
                            if (columnindex < 14)
                            {
                                if (columnindex > 3)  //从索引4开始赋值
                                {
                                    int c = (columnindex - 4) + 44;
                                    forecastDt.Rows[rowindex][c] = row[dc].ToString();
                                }
                            }
                        }
                        columnindex++;
                    }
                    if (dt.TableName == "table1")
                    {
                        forecastDt.Rows.Add(newRow);
                    }
                    rowindex++;
                }
            }
            #endregion
        }

        /// <summary>
        /// 根据规则算法得到每个时段考核的评分
        /// </summary>
        /// <returns></returns>
        private void GetEvalautionDT(string fromDate, string toDate)
        {
            //查询各实况各指标是否都是优等级 要考虑臭氧的情况
            bool isAllGood = true;
            bool O31h_bool = false;//031h是否加入考核
            bool O31h_bool2 = false;//O31h是否为0加入考核
            DataTable dt = forecastDt;
            Hashtable result_hst = new Hashtable();
            if (dt != null && dt.Rows.Count > 0)
            {
                string lstDate = "";//日期
                int rowindex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string sdmc = row["SDMC"].ToString();//时段名称
                    string rq = row["RQ"].ToString();//日期
                    string quality = row["Quality"].ToString().Trim();
                    string O31hValue = row["O31x1"].ToString();//臭氧1小时AQI值
                    string firstPollutant = row["AQIy"].ToString();//首要污染物
                    string O31hForecast = row["O31y"].ToString();//臭氧1小时预报值
                    ArrayList lst = new ArrayList();
                    #region 查询PM25 、PM10 、NO2、031h、038h实况数据
                    string PM25 = row["PM25x1"].ToString();
                    string PM10 = row["PM10x1"].ToString();
                    string N02 = row["NO2x1"].ToString();
                    string O31h = row["O31x1"].ToString();
                    string O38h = row["O38x1"].ToString();

                    int PM25i = 0;
                    int.TryParse(PM25, out PM25i);
                    PM25i = GetAQILevel(PM25i);
                    lst.Add(PM25i);

                    int PM10i = 0;
                    int.TryParse(PM10, out PM10i);
                    PM10i = GetAQILevel(PM10i);
                    lst.Add(PM10i);

                    int N02i = 0;
                    int.TryParse(N02, out N02i);
                    N02i = GetAQILevel(N02i);
                    lst.Add(N02i);

                    int O31hi = 0;
                    int.TryParse(O31h, out O31hi);
                    O31hi = GetAQILevel(O31hi);
                    lst.Add(O31hi);

                    int O38hi = 0;
                    int.TryParse(O38h, out O38hi);
                    O38hi = GetAQILevel(O38hi);
                    lst.Add(O38hi);

                    //查询各指标是否都为优等级,考虑臭氧情况
                    if (PM25i != 0)
                        isAllGood = false;
                    else if (PM10i != 0)
                        isAllGood = false;
                    else if (N02i != 0)
                        isAllGood = false;
                    #endregion

                    string rqs = rq;
                    if (rq.Trim() == "")
                    {
                        rqs = lstDate;
                    }

                    #region 判断臭氧1小时情况在上午的时候，要不要参与考核
                    if (sdmc == "上午")
                    {
                        DateTime nowTime = DateTime.Parse(rqs);
                        DateTime beginTime = DateTime.Parse(nowTime.Year + "-03" + "-16");
                        DateTime endTime = DateTime.Parse(nowTime.Year + "-11" + "-15");
                        //臭氧1小时是否在夏半年(3月16日至11月15日)
                        if (nowTime >= beginTime && nowTime <= endTime)
                        {
                            #region
                            float result = 0f;
                            float.TryParse(O31hValue, out result);
                            //还要判断预报员有没有填报臭氧信息(这里只判断臭氧1小时)
                            if (string.IsNullOrEmpty(O31hForecast) && O31hForecast == "/")
                            {
                                //未填报分为两种情况
                                //1、若未填报且该时段臭氧实况未达到考核标准（第2条），则臭氧不计入考核；
                                if (result <= 100f || firstPollutant.IndexOf("O3-1h") < 0)
                                {
                                    O31h_bool2 = true;
                                    isAllGood = true;
                                }
                                else
                                {
                                    //2、若未填报且该时段臭氧实况达到考核标准，则该项指数以预报值为“0”记录和考核
                                    isAllGood = false;
                                    O31h_bool = true;//臭氧1小时加入考核
                                    O31h_bool2 = true;
                                }
                            }
                            else
                            {
                                //判断臭氧1小时的情况大于100并且成为首要污染物
                                if (result > 100f && firstPollutant.IndexOf("O3-1h") >= 0)
                                {
                                    O31h_bool = true;//臭氧1小时加入考核

                                    if (O31hi != 0)//臭氧1小时的指标是否为优秀
                                        isAllGood = false;
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    string fx = ProcessData(row, isAllGood, lst, O31h_bool, O31h_bool2, rqs);
                    //f 总分= f夜间 × 0.3 + f上午 × 0.3 + f下午 × 0.3 + f次日 × 0.1

                    if (rqs != lstDate && lstDate != "")//日平均
                    {
                        //判断是否到了第二个时间段
                        float yw = 0f;
                        try
                        {
                            float.TryParse(result_hst["夜晚"].ToString(), out yw);
                        }
                        catch { }
                        float sw = 0f;
                        try
                        {
                            float.TryParse(result_hst["上午"].ToString(), out sw);
                        }
                        catch { }
                        float xw = 0f;
                        try
                        {
                            float.TryParse(result_hst["下午"].ToString(), out xw);
                        }
                        catch { }
                        float rpj = 0f;
                        try
                        {
                            float.TryParse(result_hst["全天"].ToString(), out rpj);//日平均
                        }
                        catch { }
                        string Fcount = (yw * 0.3 +
                                         sw * 0.3 +
                                         xw * 0.3 +
                                         rpj * 0.1).ToString();

                        float fc = 0f;
                        float.TryParse(Fcount, out fc);
                        if (fc <= 0f)
                            fc = 0f;
                        else if (fc > 100f) 
                            fc = 100f;

                        dt.Rows[rowindex - 1]["RQ"] = fc.ToString();//在末尾第一列添加考核的总分
                        result_hst.Clear();//归置
                    }
                    lstDate = rqs;
                    float fxs = 0f;
                    float.TryParse(fx, out fxs);
                    if (fxs <= 0f)
                        fxs = 0f;
                    else if (fxs > 100f)
                        fxs = 100f;

                    result_hst[sdmc] = fxs;
                    row["SDQJ"] = fxs;//分段考核分数存到对应列，方便下次取出

                    rowindex++;
                }
            }
        }


        /// <summary>
        /// 处理考核完的数据 
        /// </summary>
        private void ProcessEvaluationData()
        {
            //将考核的数据存入数据库
            string existsSQL = @"SELECT ID FROM T_Evaluation WHERE LST='{0}' and duration='{1}'";
            string updateSQL = @"UPDATE T_Evaluation SET Score='{0}' , ScoreCount='{1}' where LST = '{2}' and duration='{3}'";
            string insertSQL = @"INSERT INTO T_Evaluation VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            string lastTime = "";

            foreach (DataRow row in forecastDt.Rows)
            {
                string rq = row["RQ"].ToString();

                string sdmc = row["SDMC"].ToString();
                if (string.IsNullOrEmpty(rq))
                    rq = lastTime;

                string score = Math.Round(decimal.Parse(row["SDQJ"].ToString()), 0).ToString();//保留2
                string scoreCount = "0";
                if (rq.IndexOf('年') < 0 && rq != "")
                {
                    scoreCount = Math.Round(decimal.Parse(rq), 0).ToString();
                    rq = lastTime;
                }

                string dateTime = DateTime.Parse(rq).ToString("yyyy-MM-dd");
                string existsSQLs = string.Format(existsSQL, dateTime, sdmc);
                string updateSQLs = string.Format(updateSQL, score, scoreCount, dateTime, sdmc);
                string insertSQLs = string.Format(insertSQL, dateTime, dateTime, "24", sdmc, "", score, scoreCount);//取当前登录用户
                m_Database.Execute(existsSQLs, updateSQLs, insertSQLs);
                lastTime = rq;
            }
        }


        public int GetAQILevel(int AQI)
        {
            int level = 0;
            if (0 <= AQI && AQI <= 50)
            {
                level = 0;
            }
            else if (51 <= AQI && AQI <= 100)
            {
                level = 1;
            }
            else if (101 <= AQI && AQI <= 150)
            {
                level = 2;
            }
            else if (151 <= AQI && AQI <= 200)
            {
                level = 3;
            }
            else if (201 <= AQI && AQI <= 300)
            {
                level = 4;
            }
            else if (AQI > 300)
            {
                level = 5;
            }
            return level;
        }

        /// <summary>
        /// 根据Duration字典表，返回相应ID的时间区间
        /// </summary>
        /// <param name="dcDuration"></param>
        /// <param name="durationID"></param>
        /// <returns></returns>
        private string[] GetDurationSpan(DataTable dcDuration, int durationID)
        {
            string filter = string.Format("DM ={0}", durationID);
            DataRow[] rows = dcDuration.Select(filter);
            string mcValue = rows[0][1].ToString();
            return mcValue.Split('-');

        }

        /// <summary>
        /// 处理五大考核情况
        /// </summary>
        /// <param name="row"></param>
        /// <param name="flag"></param>
        private string ProcessData(DataRow row, bool flag, ArrayList lst, bool O31h_bool, bool O31h_bool2, string rqs)
        {
            string F = "0";

            #region f1  首要污染物正确分
            // --  ②如果预报首要污染物和实况完全相同，得100分；
            string f1 = "0";
            string aqiy = row["AQIy"].ToString().Trim();//实测首要污染物
            string aqiz = row["AQIk"].ToString().Trim();//预报首要污染物
            //可能有多个污染物
            int sucessCount = 0;//匹配总数
            int faildCount = 0;//没有匹配总数
            foreach (string str in aqiz.Split(' '))
            {
                if (aqiy.IndexOf(str.Trim()) >= 0)
                {
                    sucessCount++;
                }
                else
                {
                    faildCount++;
                }
            }

            //④如果预报出现2个或以上首要污染物，
            //实况首要污染物为其中一项，
            //得分为f1=100 × 1 / N预报
            //（N预报为预报污染物个数）；
            if (aqiz.Split(' ').Length > 1 &&
                sucessCount > 0)
            {
                f1 = (100 * 1 / aqiz.Split(' ').Length).ToString();
            }
            //⑤如果实况出现2个或以上首要污染物，
            //预报为其中一项，得100分；
            else if (aqiy.Split(';').Length > 1 &&
                sucessCount > 0)
            {
                f1 = "100";
            }
            //②如果预报首要污染物和实况完全相同，得100分；
            else if (aqiz.Split(' ').Length == sucessCount)
            {
                f1 = "100";
            }
            else if (aqiz.Split(' ').Length == faildCount)
            {
                //③如果预报首要污染物和实况完全不同，得0分；
                f1 = "0";
            }
            #endregion
            //------------------------
            #region f2（级别准确分）
            //判断实况的基本级别（也可以说是获得实况的基本级别) ,注意 级别是往下走的。
            //统计频率②该时间段内出现频次最多的相邻两个级别（或一个级别）。
            string f2 = "0";
            int[] sort = { 0, 0, 0, 0, 0, 0 };
            #region init
            Hashtable hst_sort = new Hashtable();
            hst_sort.Add("优", 0);
            hst_sort.Add("良", 0);
            hst_sort.Add("轻度污染", 0);
            hst_sort.Add("中度污染", 0);
            hst_sort.Add("重度污染", 0);
            hst_sort.Add("严重污染", 0);
            #endregion
            foreach (int str in lst)
            {
                switch (str)
                {
                    case 0://优 0~50
                        sort[0] = (++sort[0]);
                        hst_sort["优"] = sort[0];
                        break;
                    case 1://良 51~100
                        sort[1] = (++sort[1]);
                        hst_sort["良"] = sort[1];
                        break;
                    case 2://轻度污染 101~150
                        sort[2] = (++sort[2]);
                        hst_sort["轻度污染"] = sort[2];
                        break;
                    case 3://中度污染 151~200
                        sort[3] = (++sort[3]);
                        hst_sort["中度污染"] = sort[3];
                        break;
                    case 4://重度污染 201~300
                        sort[4] = (++sort[4]);
                        hst_sort["重度污染"] = sort[4];
                        break;
                    default:
                        //严重污染 > 300
                        sort[5] = (++sort[5]);
                        hst_sort["严重污染"] = sort[5];
                        break;
                }
            }
            //排序
            string skValue = ""; //实况基本级别
            HashComparerByValue hashCmp = new HashComparerByValue(System.Windows.Forms.SortOrder.Descending);
            DictionaryEntry[] dirs = SortHashtable(hst_sort, hashCmp);
            for (int i = 0; i < 2; i++)
            {
                //取前两个或一个
                if (skValue.IndexOf(dirs[i].Key.ToString()) < 0 &&
                   int.Parse(dirs[i].Value.ToString()) > 0)
                {
                    skValue = skValue + dirs[i].Key.ToString() + "到";
                }
            }
            if (!string.IsNullOrEmpty(skValue))
            {
                //开始计算实况基本级别规则
                //查询预报的基本级别
                string queryString = "select * from  tb_AirForecast  where foreDate='{0}' ";
                queryString = string.Format(queryString, rqs);
                DataTable dt = m_Database.GetDataTable(queryString);
                string ybValue = "";
                string memo = "";//发布描述
                if (dt != null && dt.Rows.Count > 0)
                {

                    memo = dt.Rows[0]["Detail"].ToString().Trim();

                    if (row["SDMC"].ToString() == "夜晚")
                        ybValue = dt.Rows[0]["Grade1"].ToString().Trim();
                    if (row["SDMC"].ToString() == "上午")
                        ybValue = dt.Rows[0]["Grade2"].ToString().Trim();
                    if (row["SDMC"].ToString() == "下午")
                        ybValue = dt.Rows[0]["Grade3"].ToString().Trim();
                    //日平均没有数据，估计要改表字段，加一条数据进去
                }
                //如果实况的基本级别完全被预报的基本级别覆盖，则判定为完全准确，得100分；
                string[] sks = skValue.Split(new string[] { "到" }, StringSplitOptions.RemoveEmptyEntries);
                int sksCount = 0;
                for (int i = 0; i < sks.Length; i++)
                {
                    if (ybValue.IndexOf(sks[i].ToString()) >= 0)
                    {
                        sksCount++;
                    }
                }
                if (sksCount == sks.Length)
                {
                    //完全覆盖  100分
                    f2 = "100";
                }
                //如果实况的基本级别部分被预报的基本级别覆盖 得50分；
                else if (sksCount > 0)
                {
                    f2 = "50";
                }
                //如果实况的基本级别完全没有被预报的基本级别覆盖 得0分；
                else if (sksCount == 0)
                {
                    f2 = "0";
                }
                //========判断实况是否跨两级以上
                if (sks.Length > 1)
                {
                    if (ProcessLevel(sks[0], sks[1]))
                    {
                        //提取预报描述里的变化趋势
                        sks[0] = sks[0].Replace("污染", "");
                        sks[1] = sks[1].Replace("污染", "");

                        //如果实况需要变化趋势描述而预报没有，在基本级别得分的基础上扣20分；
                        if (string.IsNullOrEmpty(memo))
                        {
                            f2 = (float.Parse(f2) - 20f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) >= 0 &&
                               memo.IndexOf(sks[1]) >= 0)
                        {
                            //如果实况需要变化趋势描述，且其最高/最低级别与预报描述相符，在基本级别得分的基础上加20分；
                            f2 = (float.Parse(f2) + 20f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) >= 0 ||
                               memo.IndexOf(sks[1]) >= 0)
                        {
                            //如果实况需要变化趋势描述，且其最高/最低级别与预报描述差一级，在基本级别得分的基础上扣10分；
                            f2 = (float.Parse(f2) - 10f).ToString();
                        }
                        else if (memo.IndexOf(sks[0]) < 0 &&
                               memo.IndexOf(sks[1]) < 0)
                        {
                            //如果实况需要变化趋势描述，且其最高/最低级别与预报描述差两级（含）以上，在基本级别得分的基础上扣20分。
                            f2 = (float.Parse(f2) - 20f).ToString();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(memo))
                            //如果实况不需要变化趋势描述而预报有，在基本级别得分的基础上扣10分；
                            f2 = (float.Parse(f2) - 10f).ToString();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(memo))
                        //如果实况不需要变化趋势描述而预报有，在基本级别得分的基础上扣10分；
                        f2 = (float.Parse(f2) - 10f).ToString();

                }
                //========④级别准确性得分最高为100分，最低为0分，得分高于100分/低于0分则以100分/0分计算。
                if (float.Parse(f2) > 100)
                    f2 = "100";
                else if (float.Parse(f2) < 0)
                    f2 = "0";
            }

            #endregion
            //------------------------
            #region f3（首要污染物iAQI精度)
            string f3 = "0";
            //获取首要污染物
            //row["AQIy"].ToString();
            float f3x = ProcessF3("AQIz", "AQIx", row);
            f3 = f3x.ToString();
            #endregion
            //------------------------
            #region f4其他污染物IAQI精度 排除首要污染物
            string f4 = "0";
            int count = 0;

            //要判断臭氧1h的情况 PM25
            float f4x = 0f;
            if (row["AQIy"].ToString().IndexOf("PM2.5") < 0)
            {
                f4x = ProcessF3("PM25y1", "PM25x1", row);
                count++;
            }
            //PM10
            float f4y = 0f;
            if (row["AQIy"].ToString().IndexOf("PM10") < 0)
            {
                f4y = ProcessF3("PM10y1", "PM10x1", row);
                count++;
            }
            //NO2
            float f4z = 0f;
            if (row["AQIy"].ToString().IndexOf("NO2") < 0)
            {
                f4z = ProcessF3("NO2y1", "NO2x1", row);
                count++;
            }
            //031h  --考虑O31h情况 
            float f4k = 0f;
            if (O31h_bool)
            {
                if (!O31h_bool2)
                {
                    if (row["AQIy"].ToString().IndexOf("O3-1h") < 0)
                    {
                        f4k = ProcessF3("O31y1", "O31x1", row);
                        count++;
                    }
                }
            }
            //O38h
            float f4q = 0f;
            if (row["AQIy"].ToString().IndexOf("O3-8h") < 0)
            {
                f4q = ProcessF3("O38y1", "O38x1", row);
            }
            //处理平均
            f4 = ((f3x + f4y + f4z + f4k + f4q) / count).ToString();
            #endregion
            //------------------------
            #region f0（污染预报附加分项）
            string f0 = "0";
            string sk = row["Parameter"].ToString().Trim();//实测
            string yb = row["AQIz"].ToString().Trim();//预报
            #region  CONVERT
            try
            {
                switch (GetAQILevel(int.Parse(yb))) { 
                    case 0:
                        yb = "优";
                        break;
                    case 1:
                        yb = "良";
                        break;
                    case 2:
                        yb = "轻度污染";
                        break;
                    case 3:
                        yb = "中度污染";
                        break;
                    case 4:
                        yb = "重度污染";
                        break;
                    case 5:
                        yb = "严重污染";
                        break;
                }
            }
            catch { }
            #endregion
            //当实况出现轻度及以上污染时，进行AQI附加分（f0）评定
            if (sk.IndexOf('优') < 0 &&
                sk.IndexOf('良') < 0 &&
                sk.IndexOf('/') > 0)
            {
                f0 = ProcessFO(sk, yb);
            }
            #endregion
            //------------------------
            #region 计算数据
            if (flag)
            {
                //全优等级  
                //F = 0.3 × f1 + 0.7× f4 + f0    
                F = (0.3f * float.Parse(f2) +
                    (0.7f * (float.Parse(f3) + float.Parse(f4))) +
                   float.Parse(f0)).ToString();//f1 = f2  f4 = f3
            }
            else
            {
                //非优等级 
                //F = 0.1 × f1 +0.2 × f2+ 0.7 × f3 + f0 
                F = (0.1f * float.Parse(f1) +
                     0.2f * float.Parse(f2) +
                     0.3f * float.Parse(f3) +
                     0.4f * float.Parse(f4) +
                     float.Parse(f0)).ToString();//f1 = f2  f4 = f3
            }
            #endregion
            return F;
        }

        /// 处理F0情况
        /// </summary>
        /// <param name="sk">实况数据</param>
        /// <param name="yb">预报数据</param>
        private string ProcessFO(string sk, string yb)
        {
            string F0 = "0";
            if (fODt.Rows.Count <= 0)
            {
                #region 添加数据进去
                DataRow row0 = fODt.NewRow();
                row0["名称"] = "优良";
                row0["优良"] = "0";
                row0["轻度"] = "0";
                row0["中度"] = "-2";
                row0["重度"] = "-4";
                row0["严重"] = "-8";
                fODt.Rows.Add(row0);
                DataRow row1 = fODt.NewRow();
                row1["名称"] = "轻度";
                row1["优良"] = "0";
                row1["轻度"] = "2";
                row1["中度"] = "0";
                row1["重度"] = "-2";
                row1["严重"] = "-4";
                fODt.Rows.Add(row1);
                DataRow row2 = fODt.NewRow();
                row2["名称"] = "中度";
                row2["优良"] = "-2";
                row2["轻度"] = "0";
                row2["中度"] = "4";
                row2["重度"] = "0";
                row2["严重"] = "-2";
                fODt.Rows.Add(row2);
                DataRow row3 = fODt.NewRow();
                row3["名称"] = "重度";
                row3["优良"] = "-4";
                row3["轻度"] = "-2";
                row3["中度"] = "0";
                row3["重度"] = "8";
                row3["严重"] = "1";
                fODt.Rows.Add(row3);
                DataRow row4 = fODt.NewRow();
                row4["名称"] = "严重";
                row4["优良"] = "-8";
                row4["轻度"] = "-4";
                row4["中度"] = "-2";
                row4["重度"] = "1";
                row4["严重"] = "10";
                fODt.Rows.Add(row4);
                #endregion
            }
            DataRow[] rows = fODt.Select("[名称] like '%" + sk + "%'");
            if (rows != null && rows.Length > 0)
            {
                if (yb.IndexOf('优') >= 0 ||
                    yb.IndexOf('良') >= 0)
                {
                    F0 = rows[0][0].ToString();
                }
                else
                {
                    yb = yb.Replace("污染", "");
                    F0 = rows[0][yb].ToString();
                }
            }
            return F0;
        }

        /// 处理F3情况
        /// </summary>
        /// <param name="x">预报值</param>
        /// <param name="y">实测值</param>
        private float ProcessF3(string x, string y, DataRow row)
        {
            string f3iPM25 = row[x].ToString().Trim();//预报值
            string f3iPM25s = row[y].ToString().Trim();//实测值
            float f3iPM25V1 = 0f;
            float.TryParse(f3iPM25, out f3iPM25V1);
            float f3iPM25V2 = 0f;
            float.TryParse(f3iPM25s, out f3iPM25V2);

            float mx = f3iPM25V2 > 50f ? f3iPM25V2 : 50;//max(实况,50)

            float z = (1 - (Math.Abs(f3iPM25V1 - f3iPM25V2) / mx));
            string f3i = ((z > 0 ? z : 0) * 100).ToString();////取max(xx,0)
            float f3iV = 0f; //------------------------------------------
            float.TryParse(f3i, out f3iV);
            return f3iV;
        }

        /// 哈希表的排序
        /// </summary>
        /// <param name="hasTbl"></param>
        /// <param name="cmp"></param>
        /// <returns></returns>
        private DictionaryEntry[] SortHashtable(Hashtable hasTbl, IComparer cmp)
        {
            DictionaryEntry[] dic = new DictionaryEntry[hasTbl.Count];
            hasTbl.CopyTo(dic, 0);
            Array.Sort(dic, cmp);
            return dic;
        }

        private bool ProcessLevel(string l, string r)
        {
            Hashtable hs = new Hashtable();
            hs.Add("优", "1");
            hs.Add("良", "2");
            hs.Add("轻度污染", "3");
            hs.Add("中度污染", "4");
            hs.Add("重度污染", "5");
            hs.Add("严重污染", "6");

            int x = int.Parse(hs[l].ToString());
            int y = int.Parse(hs[r].ToString());
            int z = x - y;
            if (z < 0)
                z = y - x;

            if (z >= 2) //跨度大于2
                return true;
            else
                return false;
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataTable GetEvaluationExport(string fromDate, string toDate)
        {
            //获取考核数据，按照时间段
            string strSQL = "SELECT * FROM T_Evaluation WHERE LST between '{0}' and '{1}' order by Lst";
            DateTime startDate = DateTime.Parse(fromDate);
            DateTime endDate = DateTime.Parse(toDate);
            strSQL = string.Format(strSQL, startDate, endDate);
            DataTable db = m_Database.GetDataTable(strSQL);
            StringBuilder htmlSbd = new StringBuilder();
            evaDt.Clear();
            #region 考核数据处理
            foreach (DataRow row in db.Rows)
            {
                string LST = row["LST"].ToString().Trim();
                DataRow[] rows = evaDt.Select("LST = '" + LST + "' ");
                if (rows == null || rows.Length <= 0)
                {
                    DataRow newRow = evaDt.NewRow();
                    newRow["LST"] = row["LST"].ToString().Trim();
                    evaDt.Rows.Add(newRow);
                    rows = evaDt.Select("LST = '" + LST + "' ");
                }
                if (rows != null && rows.Length > 0)
                {
                    if (row["duration"].ToString() == "夜晚")
                        rows[0]["Night"] = row["Score"].ToString().Trim();

                    if (row["duration"].ToString() == "上午")
                        rows[0]["AM"] = row["Score"].ToString().Trim();

                    if (row["duration"].ToString() == "下午")
                        rows[0]["PM"] = row["Score"].ToString().Trim();

                    if (row["duration"].ToString() == "全天")
                    {
                        rows[0]["DayAVG"] = row["Score"].ToString().Trim();
                        rows[0]["SocreCount"] = row["ScoreCount"].ToString().Trim();
                    }
                }
            }
            #endregion 
            return evaDt;
        }

    }
}
