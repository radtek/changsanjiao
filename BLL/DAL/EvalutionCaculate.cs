using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ChinaAQI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
namespace MMShareBLL.DAL
{
    public class EvalutionCaculate
    {
        private Database m_Database;
        public DataTable dIAQITable;
        public EvalutionCaculate()
        {
            m_Database = new Database();
        }
        public int ToAQI(string value, string itemID)
        {
            int AQIValue = 0;
            double inputValue = double.Parse(value) / 1000;
            switch (itemID)
            {
                case "1":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue,24,11, 180);
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
        public int CalcuDayAQI(string value, string itemID)
        {
            int AQIValue = -1;
            string conce = Math.Round(double.Parse(value) / 1000.0, 3).ToString();
            switch (itemID)
            {
                // PM25,PM10,NO2,O31h,O38h,CO,SO2
                case "1": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 24, 11, 180); break;
                case "2": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 7, 11, 180); break;
                case "3": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 22, 11, 180); break;
                case "4": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 8, 10, 0); break;
                case "5": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 8, 16, 16); break;
                case "6": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 6, 11, 180); break;
                case "7": AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(conce, 1, 11, 180); break;
            }

            return AQIValue;
        }

        public string returnParemeter(string Paremeter)
        {
            Paremeter = Paremeter.Replace("PM25", "PM2.5");
            Paremeter = Paremeter.Replace("Qzone8", "O3");
            Paremeter = Paremeter.Replace("Qzone1", "O3");
            return Paremeter;


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
        public void chinaProces(DataTable RTTable, DataTable WRFTble, string month)
        {
            DataTable dt = new DataTable("T_ChinaEvaluation");
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
                        newRow2[9] = DBNull.Value;
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
            string strSQL = string.Format("DELETE T_ChinaEvaluation WHERE LST between  '{0}' AND '{1}'", DateTime.Parse(month).Date.ToString("yyyy-MM-dd 00:00:00"), DateTime.Parse(month).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            m_Database.Execute(strSQL);//删除已有记录
            bool k = m_Database.BulkCopy(dt);
        }
        /// <summary>
        /// 判断实况各指标均为优等级
        /// </summary>
        /// <param name="dSet"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool IFGood(DataTable dSet, string filter)
        {
            DataRow[] rows = dSet.Select(filter);
            int AQI = 0;
            bool isGood = true;
            for (int i = 1; i < 6; i++)
            {
                if (rows.Length > 0)
                {
                    AQI = int.Parse(rows[0][i].ToString());
                    if (AQI > 50)
                    {
                        isGood = false;
                        break;
                    }
                }

            }

            return isGood;
        }
        public bool DurationProces(DataSet dSet, DataTable WrfTable, DataTable ForecastTable, string month)
        {
            try
            {
                DataSet AllTable = new DataSet();
                AllTable.Tables.Add(WrfTable);
                AllTable.Tables.Add(ForecastTable);
                DataTable dt = new DataTable("T_DurationEvaluation");
                dt.Columns.Add("LST", typeof(DateTime));
                dt.Columns.Add("DurationID", typeof(int));
                dt.Columns.Add("AQI", typeof(int));

                dt.Columns.Add("Module", typeof(string));
                dt.Columns.Add("Quality", typeof(string));
                dt.Columns.Add("Grade", typeof(int));
                dt.Columns.Add("Parameter", typeof(string));
                dt.Columns.Add("f0", typeof(float));
                dt.Columns.Add("f1", typeof(float));
                dt.Columns.Add("f2", typeof(float));
                dt.Columns.Add("f3", typeof(float));
                dt.Columns.Add("f4", typeof(float));
                dt.Columns.Add("F", typeof(float));
                dt.Columns.Add("userID", typeof(string));
                DataTable RtTable = new DataTable();
                string Lst = "";
                string filter = "";
                int[] DurationArray = { 6, 2, 3 };
                AQIExtention aqiExt1;
                AQIExtention aqiExt;
                DataTable dm = new DataTable();
                double f0 = 0.0;
                double f1 = 0.0;
                double f2 = 0.0;
                double f3 = 0.0;
                double f4 = 0.0;
                double F = 0.0;
                int initGrade = 0;
                bool isGood = false;
                for (int i = 0; i < dSet.Tables.Count; i++)
                {
                    RtTable = dSet.Tables[i];
                    foreach (DataRow rows in RtTable.Rows)
                    {
                        DataRow newRow1 = dt.NewRow();
                        Lst = DateTime.Parse(rows[0].ToString()).ToString("yyyy-MM-dd 00:00:00");
                        filter = string.Format("time_point='{0}'", rows[0].ToString());
                        isGood = IFGood(RtTable, filter);
                        int AQI = int.Parse(rows[6].ToString());
                        aqiExt = new AQIExtention(AQI);
                        string parameter = returnParemeter(rows[7].ToString());
                        for (int j = 0; j < AllTable.Tables.Count; j++)
                        {
                            dm = AllTable.Tables[j];
                            filter = string.Format("LST='{0}' and  DurationID='{1}' and ITEMID<>0", Lst, DurationArray[i]);
                            DataRow[] OtherRows = dm.Select(filter);

                            filter = string.Format("LST='{0}' and  DurationID='{1}' and ITEMID=0", Lst, DurationArray[i]);
                            DataRow[] WrfRows = dm.Select(filter);
                            f4 = ProcessF4(rows, OtherRows, DurationArray[i], isGood, WrfRows);
                            if (WrfRows.Length > 0)
                            {
                                DataRow newRow2 = dt.NewRow();
                                newRow2[0] = Lst;
                                newRow2[1] = DurationArray[i];
                                newRow2[2] = WrfRows[0][1];
                                newRow2[3] = WrfRows[0][2];
                                if (j == 0)
                                {
                                    newRow2[4] = WrfRows[0][3];
                                    newRow2[5] = WrfRows[0][4];
                                    initGrade = int.Parse(WrfRows[0][4].ToString());
                                }
                                else
                                {
                                    aqiExt1 = new AQIExtention(int.Parse(WrfRows[0][1].ToString()));
                                    newRow2[4] = aqiExt1.Quality;
                                    newRow2[5] = aqiExt1.IntGrade;
                                    initGrade = aqiExt1.IntGrade;
                                }
                                newRow2[6] = WrfRows[0][5];
                                f0 = ProcessFO(aqiExt.IntGrade, initGrade);
                                if (!isGood)
                                {
                                    f1 = ProcessF1(parameter, WrfRows[0][5].ToString());
                                    f3 = ProcessF3(AQI, int.Parse(WrfRows[0][1].ToString()));
                                }
                                f2 = ProcessF2(aqiExt.IntGrade, initGrade);
                                F = ProcessF(f0, f1, f2, f3, f4, isGood);
                                newRow2[7] = f0;
                                newRow2[8] = f1;
                                newRow2[9] = f2;
                                newRow2[10] = f3;
                                newRow2[11] = f4;
                                newRow2[12] = F;
                                newRow2[13] = DBNull.Value;

                                dt.Rows.Add(newRow2);

                            }
                        }


                        newRow1[0] = Lst;
                        newRow1[1] = DurationArray[i];
                        newRow1[2] = rows[6];
                        newRow1[3] = "RT";

                        newRow1[4] = aqiExt.Quality;
                        newRow1[5] = aqiExt.IntGrade;
                        newRow1[6] = parameter;
                        newRow1[7] = DBNull.Value;
                        newRow1[8] = DBNull.Value;
                        newRow1[9] = DBNull.Value;
                        newRow1[10] = DBNull.Value;
                        newRow1[11] = DBNull.Value;
                        newRow1[12] = DBNull.Value;
                        newRow1[13] = DBNull.Value;
                        dt.Rows.Add(newRow1);


                    }

                }
                string strSQL = string.Format("DELETE T_DurationEvaluation WHERE LST between  '{0}' AND '{1}';DELETE T_IAQIEvaluation WHERE LST between  '{0}' AND '{1}';", DateTime.Parse(month).Date.ToString("yyyy-MM-dd 00:00:00"), DateTime.Parse(month).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                m_Database.Execute(strSQL);//删除已有记录
                bool k = m_Database.BulkCopy(dIAQITable);
                return m_Database.BulkCopy(dt);
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        /// 处理总分情况
        /// </summary>
        private double ProcessF(double f0, double f1, double f2, double f3, double f4, bool isGood)
        {
            double f = 0.0;
            if (isGood)
                f = Math.Round(0.3 * f2 + 0.7 * f4 + f0, 1);
            else
                f = Math.Round(0.1 * f1 + 0.2 * f2 + 0.3 * f3 + 0.4 * f4 + f0, 1);
            return f;
        }

        /// 处理F4情况
        /// </summary>
        /// <param name="sk">实况AQI</param>
        /// <param name="yb">预报AQI</param>
        private double ProcessF4(DataRow skAQI, DataRow[] rows, int duration, bool isGood, DataRow[] WrfRows)
        {
            DateTime time = DateTime.Parse(skAQI[0].ToString()).Date;
            DateTime startTime = DateTime.Parse(time.Year + "-03-16");
            DateTime toTime = DateTime.Parse(time.Year + "-11-15");
            double F4 = 0.0;
            int maxAQI = 0;
            int AQI = 0;
            int RTAQI = 0;
            List<double> List = new List<double>();
            int i = 0;
            int k = 0;
            int length = 5;
            string itemID = "";
            foreach (DataRow newRow in rows)
            {
                if (newRow[2].ToString() == "Manual")
                    itemID = newRow[6].ToString();
                else
                    itemID = newRow[7].ToString();
                if (newRow[1].ToString() == "" || newRow[1].ToString() == "null")
                    AQI = 0;
                else
                    AQI = int.Parse(newRow[1].ToString());

                if (itemID == "4" || itemID == "5")
                {
                    if (time < startTime || time > toTime)
                    {
                        length = 3;
                        continue;
                    }
                    else
                    {
                        if (duration == 3)
                        {
                            DataRow dtRow = dIAQITable.NewRow();
                            dtRow[0] = skAQI[0].ToString();
                            dtRow[1] = duration;

                            dtRow[2] = itemID;
                            dtRow[3] = newRow[2].ToString();

                            if (itemID == "4")
                                RTAQI = int.Parse(skAQI["O31"].ToString() == "" ? "0" : skAQI["O31"].ToString());
                            else if (itemID == "5")
                                RTAQI = int.Parse(skAQI["O38"].ToString() == "" ? "0" : skAQI["O38"].ToString());
                            dtRow[4] = ProcessF3(RTAQI, AQI);
                            List.Add(ProcessF3(RTAQI, AQI));
                            dtRow[5] = DBNull.Value;
                            dIAQITable.Rows.Add(dtRow);
                        }
                        else if (duration == 2 && itemID == "4")
                        {
                            if (WrfRows.Length > 0)
                            {
                                if (WrfRows[0][5].ToString().IndexOf("O3-1h") >= 0)
                                {
                                    DataRow dtRow = dIAQITable.NewRow();
                                    dtRow[0] = skAQI[0].ToString();
                                    dtRow[1] = duration;

                                    dtRow[2] = itemID;
                                    dtRow[3] = newRow[2].ToString();

                                    RTAQI = int.Parse(skAQI["O31"].ToString() == "" ? "0" : skAQI["O31"].ToString());
                                    dtRow[4] = ProcessF3(RTAQI, AQI);
                                    List.Add(ProcessF3(RTAQI, AQI));
                                    dtRow[5] = DBNull.Value;
                                    dIAQITable.Rows.Add(dtRow);

                                }
                            }
                        }
                        else
                            continue;
                    }
                }
                else
                {
                    DataRow dtRow = dIAQITable.NewRow();
                    dtRow[0] = skAQI[0].ToString();
                    dtRow[1] = duration;
                    dtRow[2] = itemID;
                    dtRow[3] = newRow[2].ToString();
                    if (itemID == "1")
                        RTAQI = int.Parse(skAQI["PM25"].ToString());
                    else if (itemID == "2")
                        RTAQI = int.Parse(skAQI["PM10"].ToString());
                    else if (itemID == "3")
                        RTAQI = int.Parse(skAQI["NO2"].ToString());
                    dtRow[4] = ProcessF3(RTAQI, AQI);
                    List.Add(ProcessF3(RTAQI, AQI));
                    dtRow[5] = DBNull.Value;
                    dIAQITable.Rows.Add(dtRow);
                }
                if (maxAQI < AQI)
                {
                    maxAQI = AQI;
                    k = i;
                }
                i++;
            }

            for (int m = 0; m < List.Count; m++)
            {
                if (isGood)
                    F4 = F4 + List[m];
                else
                {
                    if (m != k)
                        F4 = F4 + List[m];
                }
            }
            double score = 0.0;
            if (isGood)
                score = Math.Round(F4 / length, 1);
            else
                score = Math.Round(F4 / length, 1);
            return score;
        }
        /// 处理F3情况
        /// </summary>
        /// <param name="sk">实况AQI</param>
        /// <param name="yb">预报AQI</param>
        private double ProcessF3(int sk, int yb)
        {
            double F3 = 0.0;
            double abs = Math.Abs(sk - yb);
            int max = sk > 50 ? sk : 50;
            double midddleValue = 1 - abs / max;
            F3 = Math.Round((midddleValue > 0 ? midddleValue : 0) * 100, 1);
            return F3;
        }
        /// 处理F2情况
        /// </summary>
        /// <param name="sk">实况等级</param>
        /// <param name="yb">预报等级</param>
        private double ProcessF2(int sk, int yb)
        {
            double F2 = 0.0;
            int level = Math.Abs(sk - yb);
            if (sk == yb)
                F2 = 100;
            else if (level == 1)
                F2 = 50;
            else
                F2 = 0;
            return Math.Round(F2, 1);
        }
        /// 处理F1情况
        /// </summary>
        /// <param name="sk">实况首要污染物</param>
        /// <param name="yb">预报首要污染物</param>
        private double ProcessF1(string sk, string yb)
        {
            sk = sk.Trim();
            yb = yb.Trim();
            string[] skArray = sk.Split('&');
            string[] ybArray = yb.Split('&');
            double F1 = 0.0;
            if (sk == yb)
                F1 = 100;
            else if (skArray.Length > 1 && ybArray.Length == 1)
            {
                if (sk.IndexOf(yb) >= 0)
                    F1 = 100;
                else
                    F1 = 0;
            }
            else if (ybArray.Length > 1 && skArray.Length == 1)
            {
                if (yb.IndexOf(sk) >= 0)
                    F1 = 100 / ybArray.Length;
                else
                    F1 = 0;
            }
            return Math.Round(F1, 1);
        }
        /// 处理F0情况
        /// </summary>
        /// <param name="sk">实况数据</param>
        /// <param name="yb">预报数据</param>
        private double ProcessFO(int sk, int yb)
        {
            double F0 = 0.0;
            if (sk != 1)
                sk = sk - 1;
            if (yb != 1)
                yb = yb - 1;

            int level = sk - yb;
            if ((sk >= 2 || yb >= 2))
            {
                if (level == 0)
                {
                    if (sk == 2)
                        F0 = 2;
                    else if (sk == 3)
                        F0 = 4;
                    else if (sk == 4)
                        F0 = 8;
                    else if (sk == 5)
                        F0 = 10;
                }
                else
                {
                    if (Math.Abs(level) == 1)
                    {
                        if (sk < 5 && yb < 5)
                            F0 = 0;
                        else
                            F0 = 1;
                    }
                    else if (Math.Abs(level) == 2)
                    {
                        if (level == -2)
                            F0 = -1;
                        else
                            F0 = -2;
                    }
                    else if (Math.Abs(level) == 3)
                    {
                        if (level == -3)
                            F0 = -2;
                        else
                            F0 = -4;
                    }
                    else if (Math.Abs(level) == 4)
                    {
                        if (level == -4)
                            F0 = -4;
                        else
                            F0 = -8;
                    }

                }
            }

            return F0;
        }
        public void DurationEvaluate()
        {
            string DateMoth = "2015-12";
            DataTable dt = new DataTable("T_DurationEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));

            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Quality", typeof(string));
            dt.Columns.Add("Grade", typeof(int));
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("DurationID", typeof(int));
            dt.Columns.Add("ITEMID", typeof(int));
            DateTime time = DateTime.Parse(DateMoth);
            string fromTime = time.Date.ToString("yyyy-MM-dd 00:00:00");
            string toTime = time.AddMonths(1).Date.AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            //实况数据
            string[] durationTalbeName = { "T_NightAQI", "T_AMAQI", "T_PMAQI" };
            string strSQL = "";
            for (int i = 0; i < durationTalbeName.Length; i++)
            {
                strSQL = strSQL + string.Format(" SELECT time_point,AQI_PM25 as PM25,AQI_PM10 as PM10,AQI_NO2 as NO2, AQI_Ozone1 as O31, AQI_Ozone8 as  O38,  AQI, primary_pollutant FROM  " + durationTalbeName[i] + "  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市' ORDER BY time_point;", fromTime, toTime);
            }

            DataSet RTDataSet = m_Database.GetDataset(strSQL);
            //模式数据
            fromTime = DateTime.Parse(fromTime).AddDays(-2).ToString("yyyy-MM-dd 20:00:00");
            toTime = DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
            strSQL = string.Format("SELECT ForecastDate,LST,ITEMID,avg(Value),Module,durationID FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and PERIOD=48  and  durationID in (2,3,6) group by LST,ITEMID,Module,durationID,ForecastDate  order by LST,ForecastDate,ITEMID ;", fromTime, toTime);
            DataTable WRfTable = m_Database.GetDataTable(strSQL);
            DataTable dTime = WRfTable.DefaultView.ToTable(true, "ForecastDate");
            string filter = "";
            DataRow[] newRow;
            int AQIValue = 0;
            int maxAQI = 0;
            string itemID = "";
            int[] duration = { 6, 2, 3 };
            AQIExtention aqiExt;
            foreach (DataRow rows in dTime.Rows)
            {
                for (int i = 0; i < duration.Length; i++)
                {

                    DataRow EvaluationRow = dt.NewRow();
                    maxAQI = 0;

                    EvaluationRow[0] = DateTime.Parse(rows[0].ToString()).AddDays(2).ToString("yyyy-MM-dd 00:00:00");
                    filter = string.Format("durationID='{0}' and ForecastDate='{1}'", duration[i], rows[0]);

                    newRow = WRfTable.Select(filter);
                    if (newRow.Length > 0)
                    {
                        foreach (DataRow rowData in newRow)
                        {
                            DataRow EvaluationRow1 = dt.NewRow();
                            AQIValue = ToAQI(rowData[3].ToString(), rowData[2].ToString());
                            EvaluationRow1[0] = DateTime.Parse(rows[0].ToString()).AddDays(2).ToString("yyyy-MM-dd 00:00:00");
                            EvaluationRow1[1] = AQIValue;
                            EvaluationRow1[2] = "WRF";
                            aqiExt = new AQIExtention(maxAQI, int.Parse(rowData[2].ToString()));
                            EvaluationRow1[3] = aqiExt.Quality;
                            EvaluationRow1[4] = aqiExt.IntGrade;
                            EvaluationRow1[5] = DBNull.Value;
                            EvaluationRow1[6] = duration[i];
                            EvaluationRow1[7] = rowData[2].ToString();
                            dt.Rows.Add(EvaluationRow1);
                            if (AQIValue > maxAQI)
                            {
                                maxAQI = AQIValue;
                                itemID = rowData[2].ToString();
                                //para=
                            }
                        }
                        EvaluationRow[1] = maxAQI;
                        EvaluationRow[2] = "WRF";
                        aqiExt = new AQIExtention(maxAQI, int.Parse(itemID));
                        EvaluationRow[3] = aqiExt.Quality;
                        EvaluationRow[4] = aqiExt.IntGrade;
                        EvaluationRow[5] = aqiExt.FirstItem;
                        EvaluationRow[6] = duration[i];
                        EvaluationRow[7] = 0;


                    }
                    dt.Rows.Add(EvaluationRow);

                }
            }
            string forecastStr = returnMonthForecastData("2015-12-01");
            DataTable foreCastTable = JsonToDataTable(forecastStr);
            DurationProces(RTDataSet, dt, foreCastTable, "2015-12-01");
        }
        /// <summary>
        ///插入国家局评分表 
        /// </summary>
        public void InsertEvaluationTable()
        {
            string DateMoth = "2015-12";
            DataTable dt = new DataTable("T_ChinaEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Quality", typeof(string));
            dt.Columns.Add("Grade", typeof(int));
            dt.Columns.Add("Parameter", typeof(string));
            DateTime time = DateTime.Parse(DateMoth);
            string fromTime = time.Date.ToString("yyyy-MM-dd 00:00:00");
            string toTime = time.AddMonths(1).Date.AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
            //实况数据
            string strSQL = string.Format("select a.time_point,a.AQI,a.primary_pollutant from ( SELECT time_point,AQI, primary_pollutant,datename(hour, time_point) as hourNme FROM T_24hAQI  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市') a where  a.hourNme=20 ORDER BY time_point;", fromTime, toTime);
            DataTable RTDataTable = m_Database.GetDataTable(strSQL);
            //模式数据
            fromTime = DateTime.Parse(fromTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            toTime = DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
            strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 20:00:00', LST) as LST,ITEMID,avg(Value),durationID,Module FROM  T_ForecastSite where Interval=0 and PERIOD=24 and ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( '1141A','1142A','1143A','1144A','1145A','1147A','1148A','1149A','1150A')  and  durationID in (2,3,6) group by LST,ITEMID,Module,durationID,ForecastDate  order by LST,ForecastDate,ITEMID;", fromTime, toTime);//2017.7 by 孙明宇
            //strSQL = string.Format("SELECT LST,ITEMID,avg(Value),Module FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and Interval=24 and durationID=7 group by LST,ITEMID,Module  order by LST,ITEMID ;", fromTime, toTime);
            DataTable WRfTable = m_Database.GetDataTable(strSQL);
            DataTable dTime = WRfTable.DefaultView.ToTable(true, "LST");
            string filter = "";
            DataRow[] newRow;
            int AQIValue = 0;
            int maxAQI = 0;
            string itemID = "";
            string qualityValue = "";
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
                    EvaluationRow[5] = aqiExt.FirstItem;

                }
                dt.Rows.Add(EvaluationRow);

            }

            //主观数据
            string path = "~/" + DateMoth;
            string fileName = "";
            string Foretime = "";
            string[] info;
            path = System.Web.HttpContext.Current.Server.MapPath(path);
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                path = files[i];
                fileName = Path.GetFileName(path);
                info = fileName.Split('_');
                Foretime = info[4].Substring(0, 4) + "-" + info[4].Substring(4, 2) + "-" + info[4].Substring(6, 2) + " 20:00:00";
                DataRow EvaluationRow = dt.NewRow();
                EvaluationRow[0] = Foretime;
                string AQI = "";
                string parameter = "";
                StreamReader read = new StreamReader(path);
                string tempStr = "";
                string[] array = null;
                while ((tempStr = read.ReadLine()) != null)
                {
                    array = tempStr.Split(' ');
                    if (array[0] == "24")
                    {
                        AQI = double.Parse(array[7]).ToString();
                        qualityValue = array[8];
                        parameter = array[9].Substring(0, 1);
                    }
                }
                EvaluationRow[1] = AQI;
                EvaluationRow[2] = "Manual";
                AQIExtention aqiExt = new AQIExtention(int.Parse(AQI));
                EvaluationRow[3] = aqiExt.Quality;
                EvaluationRow[4] = aqiExt.IntGrade;
                EvaluationRow[5] = parameter;
                dt.Rows.Add(EvaluationRow);
            }
            chinaProces(RTDataTable, dt, DateMoth);
        }
        public string returnMonthForecastData(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).Date.AddDays(-1).ToString("yyyy-MM-dd 18:00:00");
            string toTime = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-2).ToString("yyyy-MM-dd 18:59:00");

            string strSQL = "SELECT CONVERT(VARCHAR(10),DATEADD(day,1,ForecastDate),120)+  ' 00:00:00' as LST,AQI,'Manual' as Manual,Value,DurationID,Parameter,ITEMID FROM T_ForecastGroup WHERE Module='SMCSubmit' and ForecastDate between  '" + fromTime + "' and '" + toTime + "' and durationID in (2,3,6)   and PERIOD=24 ORDER BY LST";
            DataTable dt = m_Database.GetDataTable(strSQL);

            string josnData = JsonConvert.SerializeObject(dt, new DataTableConverter());
            return josnData;
        }
        public string returnMonthForecastDataChart(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).Date.AddDays(-1).ToString("yyyy-MM-dd 18:00:00");
            string toTime = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-2).ToString("yyyy-MM-dd 18:59:00");

            string strSQL = "SELECT  CONVERT(VARCHAR(19),LST,120) as LST,AQI,'Manual' as Manual,Value,DurationID,Parameter,ITEMID FROM T_ForecastGroup WHERE Module='SMCSubmit' and ForecastDate between  '" + fromTime + "' and '" + toTime + "' and durationID in (2,3,6)   and PERIOD=24 ORDER BY LST";
            DataTable dt = m_Database.GetDataTable(strSQL);

            string josnData = JsonConvert.SerializeObject(dt, new DataTableConverter());
            return josnData;
        }
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }
        public string ReturnWRFYearTalbe(string dateTime, string module)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-01 23:59:59");
            string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST,  AVG(PM25), AVG(PM10), AVG(NO2), AVG(O31), AVG(O38),AVG(Score) FROM T_AirQualityEvaluate  WHERE LST between '{0}' and '{1}' and Module = '{2}' Group by CONVERT(VARCHAR(7),LST,120) ORDER BY LST ", fromTime, toTime, module);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleYear'>日期</td>");
            sb.Append("<td class='tabletitleYear'>PM<sub>2.5</sub></td>");
            sb.Append("<td class='tabletitleYear'>PM<sub>10</sub></td>");
            sb.Append("<td class='tabletitleYear'>NO<sub>2</sub></td>");
            sb.Append("<td class='tabletitleYear'>O<sub>3</sub>-1h</td>");
            sb.Append("<td class='tabletitleYear'>O<sub>3</sub>-8h</td>");
            sb.Append("<td class='tabletitleYear'>AQI</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in dt.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowYear'>" + DateTime.Parse(rows[0].ToString()).Month.ToString() + "月" + "</td>");
                for (int i = 1; i <= 6; i++)
                {
                    if (rows[i].ToString() != "")
                        sb.Append("<td class='tableRowYear'>" + Math.Round(double.Parse(rows[i].ToString()), 1) + "</td>");
                    else
                        sb.Append("<td class='tableRowYear'>-</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string ReturnDurationYearTalbe(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-01 23:59:59");
            string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, DurationID, ITEMID, Module, avg(Score) as  Score FROM  T_IAQIEvaluation WHERE LST between '{0}' and '{1}' and Module in ('WRF','Manual') Group by CONVERT(VARCHAR(7),LST,120), DurationID, ITEMID, Module ORDER BY LST ", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, DurationID,Module,avg(F) as  Score  FROM T_DurationEvaluation WHERE LST between '{0}' and '{1}' and Module in ('WRF','Manual') Group by CONVERT(VARCHAR(7),LST,120), DurationID,Module ORDER BY LST ", fromTime, toTime);
            DataTable dk = m_Database.GetDataTable(strSQL);
            DataTable dtTime = dt.DefaultView.ToTable(true, "LST");
            string filter = "";
            DataRow[] newRow;
            string[] durationName = { "夜间", "上午", "下午" };
            int[] duration = { 6, 2, 3 };
            string[] module = { "Manual", "WRF" };
            string[] moduleName = { "主观预报", "WRF-chem" };

            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleYear'></td>");
            sb.Append("<td class='tabletitleYear'></td>");
            sb.Append("<td class='tabletitleYear'></td>");
            sb.Append("<td class='tabletitleYear'>PM<sub>2.5</sub></td>");
            sb.Append("<td class='tabletitleYear'>PM<sub>10</sub></td>");
            sb.Append("<td class='tabletitleYear'>NO<sub>2</sub></td>");
            sb.Append("<td class='tabletitleYear'>O<sub>3</sub></td>");
            sb.Append("<td class='tabletitleYear'>综合评分</td>");
            sb.Append("</tr>");
            foreach (DataRow timeRows in dtTime.Rows)
            {
                for (int i = 0; i < duration.Length; i++)
                {
                    for (int j = 0; j < module.Length; j++)
                    {
                        sb.Append("<tr>");
                        if (j == 0)
                        {
                            if (i == 0)
                                sb.Append("<td class='tableRowYear'  rowspan='6'>" + DateTime.Parse(timeRows[0].ToString()).Month.ToString() + "月" + "</td>");
                            sb.Append("<td class='tableRowYear'  rowspan='2'>" + durationName[i] + "</td>");
                        }
                        sb.Append("<td class='tableRowYear' >" + moduleName[j] + "</td>");
                        for (int k = 1; k <= 4; k++)
                        {
                            filter = string.Format("LST='{0}' and Module='{1}' and DurationID='{2}' and ITEMID='{3}'", timeRows[0], module[j], duration[i], k);
                            newRow = dt.Select(filter);
                            if (newRow.Length > 0)
                                sb.Append("<td class='tableRowYear'>" + Math.Round(double.Parse(newRow[0][4].ToString()), 1) + "</td>");
                            else
                                sb.Append("<td class='tableRowYear'>-</td>");

                        }
                        filter = string.Format("LST='{0}' and Module='{1}' and DurationID='{2}' ", timeRows[0], module[j], duration[i]);
                        newRow = dk.Select(filter);
                        if (newRow.Length > 0)
                            sb.Append("<td class='tableRowYear'>" + Math.Round(double.Parse(newRow[0][3].ToString()), 1) + "</td>");
                        else
                            sb.Append("<td class='tableRowYear'>-</td>");
                        sb.Append("</tr>");
                    }

                }
            }
            sb.Append("</table>");
            return sb.ToString();


        }
        public string ReturnDurationTalbe(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");

            string[] modle = { "RT", "Manual", "ManualSubmit", "ManualCenter", "WRF" };
            string[] modleName = { "实况", "气象部门", "环保部门", "两家合作", "WRF-chem" };
            string strSQL = string.Format("SELECT DurationID,Module,Quality as MC,LST from  T_DurationEvaluation  Where LST between '{0}' and '{1}';select DurationID, Module,Parameter as MC,LST from  T_DurationEvaluation  Where LST between '{0}' and '{1}' ;", fromTime, toTime);
            DataSet gradeSet = m_Database.GetDataset(strSQL);
            string[][] para = new string[2][];
            para[0] = new string[6] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
            para[1] = new string[4] { "PM2.5", "PM10", "NO2", "O3" };
            string[] duration = { "6", "2", "3" };
            string[] durationName = { "夜间", "上午", "下午" };
            string filter = "";
            DataTable dt;
            DataRow[] dataRow;
            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder("{");
            int count = 0;
            int otherCount = 0;
            for (int j = 0; j < 2; j++)
            {
                sb = new StringBuilder();
                sm = new StringBuilder();
                sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
                dt = gradeSet.Tables[j];
                sb.Append("<tr>");
                sb.Append("<td class='tabletitleChild'></td>");
                sb.Append("<td class='tabletitle'></td>");
                for (int k = 0; k < para[j].Length; k++)
                {
                    sb.Append("<td class='tabletitleChild'>" + para[j][k] + "</td>");
                }
                sb.Append("</tr>");
                sm.Append(sb.ToString());
                for (int m = 0; m < duration.Length; m++)
                {
                    for (int i = 0; i < modleName.Length; i++)
                    {
                        sb.Append("<tr>");
                        if (i == 0)
                            sb.Append("<td class='tableRow'  rowspan='5'>" + durationName[m] + "</td>");
                        sb.Append("<td class='tableRow' >" + modleName[i] + "</td>");
                        for (int k = 0; k < para[j].Length; k++)
                        {
                            if (modle[i] == "RT")
                            {
                                filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", modle[i], para[j][k], duration[m]);
                                count = int.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                                sb.Append("<td class='tableRowChild'>" + count + "</td>");
                            }
                            else
                            {
                                if (j == 0)
                                    strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Quality='{1}' and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Quality='{1}' and LST between '{3}' and '{4}');", modle[i], para[j][k], duration[m], fromTime, toTime);
                                else
                                    strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Parameter like '%{1}%' and AQI>50 and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Parameter='{1}' and LST between '{3}' and '{4}');", modle[i], para[j][k], duration[m], fromTime, toTime);
                                otherCount = int.Parse(m_Database.GetFirstValue(strSQL));
                                sb.Append("<td class='tableRowChild'>" + otherCount + "</td>");
                            }


                        }
                        sb.Append("</tr>");
                        if (i > 0)
                        {
                            sm.Append("<tr>");
                            if (i == 1)
                                sm.Append("<td class='tableRow'  rowspan='4'>" + durationName[m] + "</td>");
                            sm.Append("<td class='tableRow' >" + modleName[i] + "</td>");
                            for (int k = 0; k < para[j].Length; k++)
                            {
                                filter = string.Format("Module='{0}' and MC='{1}' and DurationID='{2}'", "RT", para[j][k], duration[m]);
                                count = int.Parse(dt.Compute("COUNT(LST)", filter).ToString());
                                if (j == 0)
                                    strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Quality='{1}' and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Quality='{1}' and LST between '{3}' and '{4}');", modle[i], para[j][k], duration[m], fromTime, toTime);
                                else
                                    strSQL = string.Format("SELECT COUNT(*) from  T_DurationEvaluation Where  Module='{0}' and Parameter like '%{1}%' and AQI>50 and DurationID='{2}' and LST in (SELECT LST from  T_DurationEvaluation where Module='RT' and DurationID='{2}' and Parameter='{1}' and LST between '{3}' and '{4}');", modle[i], para[j][k], duration[m], fromTime, toTime);
                                otherCount = int.Parse(m_Database.GetFirstValue(strSQL));
                                if (count != 0)
                                    sm.Append("<td class='tableRowChild'>" + Math.Round((otherCount / double.Parse(count.ToString())) * 100, 1) + "</td>");
                                else
                                    sm.Append("<td class='tableRowChild'>0</td>");

                            }
                            sm.Append("</tr>");
                        }
                    }

                }
                sm.Append("</table>");
                sb.Append("</table>");
                sbReturn.AppendFormat("\"coutTable{0}\":\"{1}\",", j.ToString(), sb.ToString());
                sbReturn.AppendFormat("\"coutTable{0}\":\"{1}\",", (j + 2).ToString(), sm.ToString());
            }
            //IAQI主客观预报准确率及综合评分(准确率/单位：百分比）
            //每个分指数的平均值
            strSQL = string.Format("SELECT DurationID, ITEMID, Module, AVG(Score) AS Score FROM T_IAQIEvaluation join D_Item  on T_IAQIEvaluation.ITEMID=D_Item.DM   Where LST between '{0}' and '{1}' GROUP BY DurationID, ITEMID, Module;", fromTime, toTime);
            DataTable iAQIScore = m_Database.GetDataTable(strSQL);
            strSQL = string.Format("SELECT AVG(F) as F,Module,DurationID FROM T_DurationEvaluation where module<>'RT' and LST between '{0}' and '{1}' group by Module,DurationID ", fromTime, toTime);
            DataTable totalScore = m_Database.GetDataTable(strSQL);
            sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleChild'></td>");
            sb.Append("<td class='tabletitle'></td>");
            for (int k = 0; k < para[1].Length; k++)
            {
                sb.Append("<td class='tabletitleChild'>" + para[1][k] + "</td>");
            }
            sb.Append("<td class='tabletitleChild'>综合评分</td>");
            sb.Append("</tr>");
            for (int m = 0; m < duration.Length; m++)
            {
                for (int i = 1; i < modleName.Length; i++)
                {
                    sb.Append("<tr>");
                    if (i == 1)
                        sb.Append("<td class='tableRow'  rowspan='4'>" + durationName[m] + "</td>");
                    sb.Append("<td class='tableRow' >" + modleName[i] + "</td>");
                    for (int k = 0; k < para[1].Length; k++)
                    {
                        filter = string.Format("Module='{0}' and ITEMID='{1}' and DurationID='{2}'", modle[i], k + 1, duration[m]);
                        dataRow = iAQIScore.Select(filter);
                        if (dataRow.Length > 0)
                            sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(dataRow[0][3].ToString()), 1) + "</td>");
                        else
                            sb.Append("<td class='tableRowChild'>-</td>");
                    }
                    filter = string.Format("Module='{0}'  and DurationID='{1}'", modle[i], duration[m]);
                    dataRow = totalScore.Select(filter);
                    if (dataRow.Length > 0)
                        sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(dataRow[0][0].ToString()), 1) + "</td>");
                    else
                        sb.Append("<td class='tableRowChild'>-</td>");

                }
            }
            sbReturn.AppendFormat("\"coutTable4\":\"{0}\",", sb.ToString());
            if (sbReturn.Length > 1)
                sbReturn = sbReturn.Remove(sbReturn.Length - 1, 1);
            sbReturn.Append("}");
            return sbReturn.ToString();
        }
        public string ReturnDurationScore(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string[] modle = { "Manual", "ManualSubmit", "ManualCenter", "WRF" };
            string[] modleName = { "气象部门", "环保部门", "两家合作", "WRF-chem" };
            string[] durationID = { "6", "2", "3" };
            string[] className = { "tableRowChild", "tableRowChild", "tableRowChild", "tableRowChild2" };
            string strSQL = string.Format("SELECT  LST,DurationID,Module, f0, f1, f2, f3, f4, F from T_DurationEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
            string filter = "";
            DataRow[] dataRows;
            int[] day = new int[4];
            //int duration = 0;
            //int[] durationDay = new int[2];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < modle.Length; i++)
            {
                filter = string.Format("Module='{0}'", modle[i]);
                dataRows = dt.Select(filter);
                day[i] = dataRows.Length;
                //for (int j = 0; j < durationID.Length; j++)
                //{
                //    filter = string.Format("Module='{0}' and DurationID='{2}'", modle[i],durationID[j]);
                //    dataRows = dt.Select(filter);
                //    duration = duration + (totalDays - dataRows.Length);

                //}
                //durationDay[i] = duration;
            }
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleDurationScore'></td>");
            sb.Append("<td class='tabletitleDurationScore'></td>");
            sb.Append("<td class='tabletitleDurationScore' colspan='6'>夜间</td>");
            sb.Append("<td class='tabletitleDurationScore' colspan='6'>上午</td>");
            sb.Append("<td class='tabletitleDurationScore' colspan='6'>下午</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleScore'>日期</td>");
            sb.Append("<td class='tabletitleScore'></td>");
            for (int i = 0; i < durationID.Length; i++)
            {
                sb.Append("<td class='tabletitleScore'>污染</br>附加分</br>(f0)</td>");
                sb.Append("<td class='tabletitleScore'>首要污染</br>物正确</br>性评分</br>(f1)</td>");
                sb.Append("<td class='tabletitleScore'>级别准</br>确性评分</br>（f2）</td>");
                sb.Append("<td class='tabletitleScore'>首要污染</br>物iAQI精度</br>正确性评分</br>（f3）</td>");
                sb.Append("<td class='tabletitleScore'>其他污染物</br>iAQI精度</br>正确性评分</br>（f4）</td>");
                sb.Append("<td class='tabletitleScore1'>综合评分</br>（F）</td>");
            }
            sb.Append("</tr>");
            foreach (DataRow rows in timeTable.Rows)
            {
                for (int i = 0; i < modle.Length; i++)
                {
                    sb.Append("<tr>");
                    if (i == 0)
                        sb.Append("<td class='tableRowChild2' rowspan='4'>" + DateTime.Parse(rows[0].ToString()).ToString("MM月dd日") + "</td>");
                    sb.Append("<td class='" + className[i] + "'>" + modleName[i] + "</td>");
                    for (int j = 0; j < durationID.Length; j++)
                    {
                        filter = string.Format("Module='{0}' and LST='{1}' and DurationID='{2}'", modle[i], rows[0].ToString(), durationID[j]);
                        dataRows = dt.Select(filter);

                        if (dataRows.Length > 0)
                        {
                            sb.Append("<td class='" + className[i] + "'>" + dataRows[0][3] + "</td>");
                            sb.Append("<td class='" + className[i] + "'>" + dataRows[0][4] + "</td>");
                            sb.Append("<td class='" + className[i] + "'>" + dataRows[0][5] + "</td>");
                            sb.Append("<td class='" + className[i] + "'>" + dataRows[0][6] + "</td>");
                            sb.Append("<td class='" + className[i] + "'>" + dataRows[0][7] + "</td>");
                            if (i == 0)
                                sb.Append("<td class='tableRowChildright'>" + dataRows[0][8] + "</td>");
                            else
                                sb.Append("<td class='tableRowChild2right'>" + dataRows[0][8] + "</td>");
                        }
                        else
                        {
                            sb.Append("<td class='" + className[i] + "'></td>");
                            sb.Append("<td class='" + className[i] + "'></td>");
                            sb.Append("<td class='" + className[i] + "'></td>");
                            sb.Append("<td class='" + className[i] + "'></td>");
                            sb.Append("<td class='" + className[i] + "'></td>");
                            sb.Append("<td class='" + className[i] + "'></td>");
                        }
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
                    sb.Append("<td class='tableRowOther' colspan='20'>" + tempStr + "</td>");
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string returnHazeStyle(string grade)
        {
            string haze = "严重霾";
            switch (grade)
            {
                case "0":
                    haze = "无霾";
                    break;
                case "1":
                    haze = "轻微霾";
                    break;
                case "2":
                    haze = "轻度霾";
                    break;
                case "3":
                    haze = "中度霾";
                    break;
                case "4":
                    haze = "重度霾";
                    break;
                case "5":
                    haze = "严重霾";
                    break;
            }
            return haze;
        }
        public string UVScore(string dateTime)
        {

            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT count( RTGrade) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.RTGrade=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and RTGrade<6  group by RTGrade,D_UVGrade.DM ;", dtFrom, dtTo);
            strSQL = strSQL + string.Format("SELECT count( ForecastGrade16) as count ,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade16=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade16<6  group by ForecastGrade16,D_UVGrade.DM ;", dtFrom, dtTo);
            strSQL = strSQL + string.Format("SELECT count( ForecastGrade10) as count,D_UVGrade.DM FROM T_UVEvaluate inner join D_UVGrade on T_UVEvaluate.ForecastGrade10=D_UVGrade.DM WHERE LST BETWEEN '{0}' and '{1}' and ForecastGrade10<6 group by ForecastGrade10,D_UVGrade.DM ;", dtFrom, dtTo);
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
                        filter = "DM='" + j + "'";
                        rows = dt.Select(filter);
                        if (rows.Length > 0)
                        {
                            count = count + int.Parse(rows[0][0].ToString());
                            sb.Append("<td class='tableRowHaze' >" + rows[0][0].ToString() + "</td>");
                        }
                        else
                            sb.Append("<td class='tableRowHaze' >0</td>");


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
        public string UVDayScore(string dateTime)
        {
            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT LST,ForecastGrade16, ForecastGrade10, RTGrade, ForecastAccuracy16, ForecastAccuracy10, ScoreUV FROM T_UVEvaluate WHERE LST BETWEEN '{0}' and '{1}' ORDER by LST", dtFrom, dtTo);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleUV2' rowspan='2'>日期</td>");
            sb.Append("<td class='tabletitleUV2' colspan='3'>UV</td>");
            sb.Append("<td class='tabletitleUV' rowspan='2'>16时评分</td>");
            sb.Append("<td class='tabletitleUV' rowspan='2'>10时评分</td>");
            sb.Append("<td class='tabletitleUV2' rowspan='2'>评分</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleUVOther'>16时预报</td>");
            sb.Append("<td class='tabletitleUVOther'>10时预报</td>");
            sb.Append("<td class='tabletitleUVOther2'>实况</td>");
            sb.Append("</tr>");
            if (hazeTable.Rows.Count > 0)
            {
                foreach (DataRow rows in hazeTable.Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='tableRowUV' >" + DateTime.Parse(rows[0].ToString()).ToString("yyyy月MM日dd日") + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["ForecastGrade16"] + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["ForecastGrade10"] + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["RTGrade"] + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["ForecastAccuracy16"] + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["ForecastAccuracy10"] + "</td>");
                    sb.Append("<td class='tableRowUV' >" + rows["ScoreUV"] + "</td>");
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string HazeDaysScore(string dateTime)
        {
            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string dtTo = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT LST,Forecast05, Forecast17, ForecastGrade05, ForecastGrade17, RtAllDay, RtDay, Score05, Score17, NoneDays05, NoneDays17, FailDay05,FailDay17, CorrectDays05, CorrectDays17 FROM T_HazeEvaluate WHERE LST BETWEEN '{0}' and '{1}' ORDER by LST", dtFrom, dtTo);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleHazeDay' >日期</td>");
            sb.Append("<td class='tabletitleHazeDay'>05时霾预报</td>");
            sb.Append("<td class='tabletitleHazeDay'>17时霾预报</td>");
            sb.Append("<td class='tabletitleHazeDay'>预报05时</td>");
            sb.Append("<td class='tabletitleHazeDay'>预报17时</td>");
            sb.Append("<td class='tabletitleHazeDay'>夜+白实况</td>");
            sb.Append("<td class='tabletitleHazeDay'>白实况</td>");
            sb.Append("<td class='tabletitleHazeDay'>05时得分</td>");
            sb.Append("<td class='tabletitleHazeDay'>17时得分</td>");
            sb.Append("<td class='tabletitleHazeDay'>05时空报</td>");
            sb.Append("<td class='tabletitleHazeDay'>17时空报</td>");
            sb.Append("<td class='tabletitleHazeDay'>05时漏报</td>");
            sb.Append("<td class='tabletitleHazeDay'>17时漏报</td>");
            sb.Append("</tr>");
            if (hazeTable.Rows.Count > 0)
            {
                foreach (DataRow rows in hazeTable.Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='tableRow' >" + DateTime.Parse(rows[0].ToString()).ToString("yyyy月MM日dd日") + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["Forecast05"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["Forecast17"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["ForecastGrade05"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["ForecastGrade17"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["RtAllDay"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["RtDay"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["Score05"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["Score17"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["NoneDays05"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["NoneDays17"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["FailDay05"] + "</td>");
                    sb.Append("<td class='tableRow' >" + rows["FailDay17"] + "</td>");
                    sb.Append("</tr>");
                }
            }
            else
            {
                string filter = "";
                DataRow[] filterRow;
                StringBuilder sm05 = new StringBuilder();
                StringBuilder sm17 = new StringBuilder();
                string[] filterArray = { "05", "17" };
                strSQL = string.Format("SELECT   LST, convert(varchar(2),[ReTime],114) as ReTime, Haze, UserName FROM T_24Haze WHERE LST between '{0}' and '{1}' order by lst", dtFrom, dtTo);
                DataTable ForecastTable = m_Database.GetDataTable(strSQL);
                strSQL = string.Format("SELECT LST, NoneDays05, NoneDays17 FROM T_RTData WHERE LST between '{0}' and '{1}' order by lst", dtFrom, dtTo);
                DataTable RTTable = m_Database.GetDataTable(strSQL);
                DataTable timeDefault = ForecastTable.DefaultView.ToTable(true, "LST");
                foreach (DataRow rows in timeDefault.Rows)
                {
                    sm05 = new StringBuilder();
                    sm17 = new StringBuilder();
                    sb.Append("<tr>");
                    sb.Append("<td class='tableRow' >" + DateTime.Parse(rows[0].ToString()).ToString("yyyy月MM日dd日") + "</td>");
                    for (int i = 0; i < filterArray.Length; i++)
                    {
                        filter = string.Format("LST='{0}' and ReTime='{1}'", rows[0], filterArray[i]);
                        filterRow = ForecastTable.Select(filter);
                        if (filterRow.Length > 0)
                        {
                            sm17.Append("<td class='tableRow' >" + filterRow[0]["Haze"] + "</td>");
                            sm05.Append("<td class='tableRow' >" + returnHazeStyle(filterRow[0]["Haze"].ToString()) + "</td>");
                        }
                        else
                        {
                            sm17.Append("<td class='tableRow' >-</td>");
                            sm05.Append("<td class='tableRow' >-</td>");
                        }

                    }
                    sb.Append(sm05.ToString());
                    sb.Append(sm17.ToString());
                    filter = string.Format("LST='{0}'", rows[0]);
                    filterRow = RTTable.Select(filter);
                    if (filterRow.Length > 0)
                    {
                        sb.Append("<td class='tableRow' >" + filterRow[0]["NoneDays05"] + "</td>");
                        sb.Append("<td class='tableRow' >" + filterRow[0]["NoneDays17"] + "</td>");
                    }
                    else
                    {
                        sb.Append("<td class='tableRow' >-</td>");
                        sb.Append("<td class='tableRow' >-</td>");
                    }
                    for (int k = 0; k < 6; k++)
                    {
                        sb.Append("<td class='tableRow' >-</td>");
                    }
                }

            }
            sb.Append("</table>");
            return sb.ToString();
        }
        /// <summary>
        /// 雾霾评分 2017.8.15 by xuehui
        /// </summary>
        /// <param name="TimeDate"></param>
        /// <returns></returns>
        public string HazeEvaluate(string TimeDate)
        {
            string strMess = "评分失败";
            try
            {
                DataTable dt = new DataTable("T_HazeEvaluate");
                dt.Columns.Add("LST", typeof(DateTime));
                dt.Columns.Add("UserID", typeof(string));
                dt.Columns.Add("Forecast05", typeof(string));
                dt.Columns.Add("Forecast17", typeof(string));
                dt.Columns.Add("ForecastGrade05", typeof(int));
                dt.Columns.Add("ForecastGrade17", typeof(int));
                dt.Columns.Add("RtAllDay", typeof(int));
                dt.Columns.Add("RtDay", typeof(int));
                dt.Columns.Add("Score05", typeof(float));
                dt.Columns.Add("Score17", typeof(float));
                dt.Columns.Add("NoneDays05", typeof(int));
                dt.Columns.Add("NoneDays17", typeof(int));
                dt.Columns.Add("FailDay05", typeof(int));
                dt.Columns.Add("FailDay17", typeof(int));

                dt.Columns.Add("CorrectDays05", typeof(int));
                dt.Columns.Add("CorrectDays17", typeof(int));//Score
                dt.Columns.Add("Score", typeof(int));

                string dtFrom = DateTime.Parse(TimeDate).AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                string dtTo = DateTime.Parse(TimeDate).AddMonths(1).AddDays(0).ToString("yyyy-MM-dd 23:59:59");
                string strSQL = string.Format("SELECT   LST, convert(varchar(2),[ReTime],114) as ReTime, Haze-1 as Haze, UserName FROM T_24Haze WHERE LST between '{0}' and '{1}' order by lst", dtFrom, dtTo);
                DataTable ForecastTable = m_Database.GetDataTable(strSQL);
                strSQL = string.Format("SELECT LST, NoneDays05, NoneDays17 FROM T_RTData WHERE LST between '{0}' and '{1}' order by lst", dtFrom, dtTo);
                DataTable RTTable = m_Database.GetDataTable(strSQL);
                string filter = "";
                DataRow[] filterRow;
                int grade = 0;
                int gradeRt05 = 0;
                int gradeRt17 = 0;
                int gradeForecast05 = 0;
                int gradeForecast17 = 0;
                foreach (DataRow rows in RTTable.Rows)
                {
                    int rtDays = 0;
                    int rtAllDays = 0;
                    filter = string.Format("LST='{0}'", rows[0]);
                    filterRow = ForecastTable.Select(filter);
                    if (filterRow.Length > 0)
                    {
                        DataRow newRow = dt.NewRow();

                        if (Convert.ToInt32(rows[2]) >= 2) // 17时   两个或者两个以上才有霾
                        { rtAllDays++; }
                        if (Convert.ToInt32(rows[1]) >= 2) // 05时
                        { rtDays++; }
                        gradeRt05 = rtDays;//
                        gradeRt17 = rtAllDays;
                        newRow[6] = rtAllDays;
                        newRow[7] = rtDays;
                        newRow[0] = DateTime.Parse(rows[0].ToString()).AddDays(0).ToString("yyyy-MM-dd HH:mm:ss"); // 现在用自动发送05时霾预报 所以算班次应该用前一天17的班次
                        newRow[1] = filterRow[0][3].ToString();
                        foreach (DataRow dtRow in filterRow)
                        {
                            if (dtRow[2].ToString() != "0")
                                grade = 1;
                            else
                                grade = 0;
                            if (dtRow[1].ToString() == "05")
                            {
                                newRow[2] = returnHazeStyle(dtRow[2].ToString());
                                newRow[4] = grade;
                                gradeForecast05 = grade;
                                if (gradeForecast05 == rtDays)
                                {
                                    newRow[8] = 100.0;
                                    newRow[10] = 0;
                                    newRow[12] = 0;
                                    newRow[14] = 1;
                                }
                                else
                                {
                                    newRow[14] = 0;
                                    newRow[8] = 0;
                                    if (grade == 1 && Convert.ToInt32(rows[1]) < 2)
                                    {
                                        newRow[10] = 1;
                                        newRow[12] = 0;
                                    }
                                    else
                                    {
                                        newRow[10] = 0;
                                        newRow[12] = 1;
                                    }
                                }

                            }
                            else
                            {
                                newRow[3] = returnHazeStyle(dtRow[2].ToString());
                                newRow[5] = grade;
                                gradeForecast17 = grade;
                                if (gradeForecast17 == rtAllDays)
                                {
                                    newRow[9] = 100.0;
                                    newRow[11] = 0;
                                    newRow[13] = 0;
                                    newRow[15] = 1;
                                }
                                else
                                {
                                    newRow[15] = 0;
                                    newRow[9] = 0;
                                    if (grade == 1 && Convert.ToInt32(rows[2]) < 2)
                                    {
                                        newRow[11] = 1;
                                        newRow[13] = 0;
                                    }
                                    else
                                    {
                                        newRow[11] = 0;
                                        newRow[13] = 1;
                                    }
                                }
                            }
                        }
                        newRow[16] = returnScore(gradeRt05, gradeRt17, gradeForecast05, gradeForecast17);
                        dt.Rows.Add(newRow);


                    }

                }
                dtFrom = DateTime.Parse(TimeDate).AddDays(0).ToString("yyyy-MM-dd 00:00:00"); // xuehui 0815
                dtTo = DateTime.Parse(TimeDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"); // xuehui 0815
                strSQL = string.Format("DELETE T_HazeEvaluate WHERE LST between  '{0}' AND '{1}'", dtFrom, dtTo);
                m_Database.Execute(strSQL);//删除已有记录
                bool flag = m_Database.BulkCopy(dt);
                if (flag)
                {
                    double score05 = 0.0;
                    double score17 = 0.0;
                    int totalDay05 = 0;
                    int totalDay17 = 0;

                    strSQL = string.Format("SELECT convert(varchar(7),[LST],120) as LST,SUM(RtAllDay) as RtAllDay,SUM(RtDay) as RtDay,SUM(NoneDays05) as NoneDays05, SUM(NoneDays17) as NoneDays17, SUM(FailDay05) as FailDay05,SUM(FailDay17) as FailDay17,SUM(CorrectDays05) as CorrectDays05, SUM(CorrectDays17) as CorrectDays17 FROM T_HazeEvaluate WHERE LST between '{0}' and '{1}' group by convert(varchar(7),[LST],120)", dtFrom, dtTo);
                    DataTable dm = m_Database.GetDataTable(strSQL);
                    if (dm.Rows.Count > 0)
                    {
                        totalDay05 = int.Parse(dm.Rows[0]["NoneDays05"].ToString()) + int.Parse(dm.Rows[0]["FailDay05"].ToString()) + int.Parse(dm.Rows[0]["CorrectDays05"].ToString());
                        score05 = 100.0 * int.Parse(dm.Rows[0]["CorrectDays05"].ToString()) / totalDay05;
                        totalDay17 = int.Parse(dm.Rows[0]["NoneDays17"].ToString()) + int.Parse(dm.Rows[0]["FailDay17"].ToString()) + int.Parse(dm.Rows[0]["CorrectDays17"].ToString());
                        score17 = 100.0 * int.Parse(dm.Rows[0]["CorrectDays17"].ToString()) / totalDay05;
                        strSQL = string.Format("DELETE T_HazeMoth WHERE LST='{0}';INSERT into T_HazeMoth VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','9','10') ;", DateTime.Parse(dm.Rows[0]["LST"].ToString()).ToString("yyyy-MM-01 00:00:00"), score05, score17, dm.Rows[0]["RtAllDay"].ToString(), dm.Rows[0]["RtDay"].ToString(), dm.Rows[0]["NoneDays05"].ToString(), dm.Rows[0]["NoneDays17"].ToString(), dm.Rows[0]["FailDay05"].ToString(), dm.Rows[0]["FailDay17"].ToString(), dm.Rows[0]["CorrectDays05"].ToString(), dm.Rows[0]["CorrectDays17"].ToString());
                        int k = m_Database.Execute(strSQL);
                        strMess = "评分成功！";
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return strMess;
        }

        /// <summary>
        /// 求霾的总评分
        /// </summary>
        /// <param name="RtGrade05"></param>
        /// <param name="RtGrade17"></param>
        /// <param name="foreGrad05"></param>
        /// <param name="foreGrad17"></param>
        /// <returns></returns>
        public int returnScore(int RtGrade05, int RtGrade17, int foreGrad05, int foreGrad17)
        {
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
            if (RtGrade05 == foreGrad05)
                score1 = 10;
            else
            {
                if (RtGrade17 == 0)
                    score1 = 0;
                else
                    score1 = -8;
            }
            return score1 + score;

        }
        public string UVYearScore(string dateTime)
        {
            string dtTo = DateTime.Parse(dateTime).ToString("yyyy-12-01 23:00:00");
            string dtFrom = DateTime.Parse(dateTime).Date.AddYears(-1).ToString("yyyy-01-01 00:00:00");
            string strSQL = string.Format("SELECT CONVERT(VARCHAR(7),LST,120) as LST, AVG(ScoreUV) as  ScoreUV  FROM  T_UVEvaluate WHERE LST between '{0}' and '{1}' Group by CONVERT(VARCHAR(7),LST,120) ORDER BY LST DESC", dtFrom, dtTo);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleHazeYear'>日期</td>");
            sb.Append("<td class='tabletitleHazeYear'>紫外线</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in hazeTable.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowHazeYear'>" + DateTime.Parse(rows[0].ToString()).ToString("M月") + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + Math.Round(double.Parse(rows[1].ToString()), 1).ToString() + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string HazeYearScore(string dateTime)
        {
            string dtTo = DateTime.Parse(dateTime).ToString("yyyy-12-01 23:00:00");
            string dtFrom = DateTime.Parse(dateTime).Date.AddYears(-1).ToString("yyyy-01-01 00:00:00");
            string strSQL = string.Format("SELECT LST, Score05, Score17, RtAllDay, RTDays, NoneDays05, NoneDays17, FailDay05, FailDay17, CorrectDays05, CorrectDays17 FROM  T_HazeMoth WHERE LST between '{0}' and '{1}' ORDER BY LST DESC", dtFrom, dtTo);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleHazeYear' rowspan='2'>日期</td>");
            sb.Append("<td class='tabletitleHazeYear' colspan='2'>评分</td>");
            sb.Append("<td class='tabletitleHazeYear'>霾日</td>");
            sb.Append("<td class='tabletitleHazeYear' colspan='3'>05时</td>");
            sb.Append("<td class='tabletitleHazeYear' colspan='3'>17时</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitleHazeYearOther'>05时</td>");
            sb.Append("<td class='tabletitleHazeYearOther2'>17时</td>");
            sb.Append("<td class='tabletitleHazeDay'>实况</td>");
            sb.Append("<td class='tabletitleHazeYearOther'>报对</td>");
            sb.Append("<td class='tabletitleHazeYearOther'>空报</td>");
            sb.Append("<td class='tabletitleHazeYearOther3'>漏报</td>");
            sb.Append("<td class='tabletitleHazeYearOther'>报对</td>");
            sb.Append("<td class='tabletitleHazeYearOther'>空报</td>");
            sb.Append("<td class='tabletitleHazeYearOther2'>漏报</td>");
            sb.Append("</tr>");
            foreach (DataRow rows in hazeTable.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td class='tableRowHazeYear'>" + DateTime.Parse(rows[0].ToString()).ToString("M月") + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows[1] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows[2] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + (int.Parse(rows[3].ToString()) + int.Parse(rows[4].ToString())).ToString() + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["CorrectDays05"] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["NoneDays05"] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["FailDay05"] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["CorrectDays17"] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["NoneDays17"] + "</td>");
                sb.Append("<td class='tableRowHazeYear'>" + rows["FailDay17"] + "</td>");

                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        public string HazeScore(string dateTime)
        {
            string dtFrom = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).Date.AddMonths(1).AddDays(-1).Day;

            string strSQL = string.Format("SELECT  LST,Score05, Score17, RtAllDay as RtAllDay17,RTDays as RtAllDay05, NoneDays05, NoneDays17, FailDay05, FailDay17,RTDays-NoneDays05 as  CorrectDays05,RtAllDay-NoneDays17 as CorrectDays17 FROM T_HazeMoth WHERE LST='{0}'", dtFrom);
            DataTable hazeTable = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder();
            StringBuilder sm = new StringBuilder();
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
                sm = new StringBuilder();
                for (int j = 0; j < titleName.Length; j++)
                {
                    sb.Append("<tr>");
                    if (j == 0)
                        sb.Append("<td class='tableRowHaze' rowspan='5'>" + name[i] + "</td>");
                    sb.Append("<td class='tableRowHaze' >" + titleName[j] + "</td>");
                    if (hazeTable.Rows.Count > 0)
                    {
                        if (titleName[j] == "实况")
                        {
                            sb.Append("<td class='tableRowHazeLow' >" + (totalDays - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString() + "</td>");
                            sb.Append("<td class='tableRowHazeLow' >" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "</td>");
                            sm.Append("<td class='tableRowHazeLow' >" + (totalDays - int.Parse(hazeTable.Rows[0]["NoneDays" + nameStr[i]].ToString()) - int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString())).ToString() + "</td>");
                            sm.Append("<td class='tableRowHazeLow' >" + (int.Parse(hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString()) - int.Parse(hazeTable.Rows[0]["FailDay" + nameStr[i]].ToString())).ToString() + "</td>");
                        }
                        else if (titleName[j] == "准确预报")
                        {
                            sb.Append(sm.ToString());
                        }
                        else if (titleName[j] == "空报")
                        {
                            sb.Append("<td class='tableRowHazeLow' >" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "</td>");
                            sb.Append("<td class='tableRowHazeLow' >-</td>");
                        }
                        else if (titleName[j] == "漏报")
                        {
                            sb.Append("<td class='tableRowHazeLow' >-</td>");
                            sb.Append("<td class='tableRowHazeLow' >" + hazeTable.Rows[0][titleStr[j] + nameStr[i]].ToString() + "</td>");
                        }
                        else if (titleName[j] == "预报评分")
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
        /// <summary>
        /// 返回领班预报评分表 2017.7.11 by 孙明宇
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ReturnForemanScore(string dateTime)
        {
            InsertForemanScore(dateTime);//计算并插入领班评分表
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("SELECT  LST,ID,Pollution, F1, F2, F3, S from  T_ForemanScore  Where LST between '{0}' and '{1}' order by LST;", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
            //string filter = "";
            int[] day = new int[2];
            DataRow[] dataRows;
            string[] className = { "tableRow", "tableRowBottom" };
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitle'>日期</td>");
            sb.Append("<td class='tabletitle'>评分人ID</td>");

            sb.Append("<td class='tabletitle'>首要污染物</br>正确性评分(f1)</td>");
            sb.Append("<td class='tabletitle'>AQI预报级别</br>正确性评分（f2）</td>");
            sb.Append("<td class='tabletitle'>AQI预报数值</br>误差评分（f3）</td>");
            sb.Append("<td class='tabletitle'>空气质量预报</br>精确度评分（S）</td>");
            sb.Append("</tr>");//表头
            dataRows = dt.Select();
            if (dataRows.Length != 0)
            {
                foreach (DataRow row in dataRows)
                {

                    sb.Append("<tr>");
                    sb.Append("<td class=tableRow>" + DateTime.Parse(row[0].ToString()).ToString("MM月dd日") + "</td>");
                    sb.Append("<td class=tableRow>" + row[1] + "</td>");

                    sb.Append("<td class=tableRow>" + row[3] + "</td>");
                    sb.Append("<td class=tableRow>" + row[4] + "</td>");
                    sb.Append("<td class=tableRow>" + row[5] + "</td>");
                    sb.Append("<td class=tableRow>" + row[6] + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                return sb.ToString();
            }
            else
            {
                sb.Append("<tr><td>本月尚未进行领班预报</td></tr>");
                return sb.ToString();
            }
        }
        /// <summary>
        /// 计算并插入领班评分表 2017.7.13 by 孙明宇
        /// </summary>
        /// <param name="dateTime"></param>
        public void InsertForemanScore(string dateTime)
        {
            DataTable dt = new DataTable("T_ForemanScore");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Pollution", typeof(string));
            dt.Columns.Add("F1", typeof(int));
            dt.Columns.Add("F2", typeof(int));
            dt.Columns.Add("F3", typeof(int));
            dt.Columns.Add("S", typeof(double));
            int f1 = 0;
            int f2 = 0;
            int f3 = 0;
            double S = 0d;
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string dateMoth = DateTime.Parse(dateTime).ToString("yyyy-MM");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string strSQL = string.Format("select  ForeTime,forecaster,AQI,Pollution from [T_ForemanDaily] where ForeTime between '{0}' and '{1}' ORDER BY ForeTime;", fromTime, toTime);
            DataTable fdt = m_Database.GetDataTable(strSQL);//dt为领班预报表
            string strSQL1 = string.Format("SELECT DISTINCT T1.* FROM T_ChinaShiValue T1,(SELECT MAX(AQI) as 'MaxAQI',LST FROM T_ChinaShiValue group by LST  ) T2 where T1.LST=T2.LST and T1.AQI=T2.MaxAQI and T1.ITEMID<>0 AND T1.LST BETWEEN '{0}' AND '{1}' ORDER BY LST", fromTime, toTime);
            DataTable sdt = m_Database.GetDataTable(strSQL1);//sdt为实况表
            DataRow[] Frows = fdt.Select();
            //DataRow[] RTrows = sdt.Select();
            string plu = "";
            foreach (DataRow row in Frows)
            {
                try
                {
                    string filter = "";
                    filter = string.Format("LST='{0}'", row[0].ToString());
                    //F1评分
                    DataRow[] Rrows = sdt.Select(filter);
                    if (Rrows.Length == 0)
                    {
                        continue;
                    }
                    switch (Convert.ToInt32(Rrows[0][2]))
                    {
                        case 1:
                            plu = "PM2.5";
                            break;
                        case 2:
                            plu = "PM10";
                            break;
                        case 3:
                            plu = "NO2";
                            break;
                        case 4:
                            plu = "O3";
                            break;
                        case 5:
                            plu = "O3";
                            break;
                    }
                    string[] pollution = row[3].ToString().Split(',');
                    int rAQI = Convert.ToInt32(Rrows[0][1]);//2017.8.11 孙明宇
                    for (int i = 0; i < pollution.Length; i++)
                    {
                        if (pollution[i] == plu || rAQI <= 50) // 如果实况是优则F1为100
                        {
                            f1 = 100;
                            break;
                        }
                        else
                        {
                            f1 = 0;
                        }
                    }
                    //f2评分
                    int FGrade = 0;
                    if (Convert.ToInt32(row[2]) <= 50)
                        FGrade = 1;
                    else if (Convert.ToInt32(row[2]) <= 100 && Convert.ToInt32(row[2]) > 50)
                        FGrade = 2;
                    else if (Convert.ToInt32(row[2]) <= 150 && Convert.ToInt32(row[2]) > 100)
                        FGrade = 3;
                    else if (Convert.ToInt32(row[2]) <= 200 && Convert.ToInt32(row[2]) > 150)
                        FGrade = 4;
                    else if (Convert.ToInt32(row[2]) <= 300 && Convert.ToInt32(row[2]) > 200)
                        FGrade = 5;
                    else
                        FGrade = 6;
                    int level = Math.Abs(FGrade - Convert.ToInt32(Rrows[0][4]));
                    if (level == 0)
                        f2 = 100;
                    else if (level == 1)
                        f2 = 50;
                    else if (level == 2)
                        f2 = 25;
                    else
                        f2 = 0;

                    //f3评分
                    int error = 0;
                    error = Math.Abs(Convert.ToInt32(row[2]) - Convert.ToInt32(Rrows[0][1]));
                    if (error <= 25)
                        f3 = 100;
                    else if (error <= 50 && error > 25)
                        f3 = 80;
                    else if (error <= 100 && error > 50)
                        f3 = 60;
                    else if (error <= 150 && error > 100)
                        f3 = 30;
                    else
                        f3 = 0;
                    //S评分
                    S = 0.2 * f1 + 0.5 * f2 + 0.3 * f3;
                    DataRow dr = dt.NewRow();
                    DateTime LST = Convert.ToDateTime(row[0]).AddDays(-1); 
                    dr[0] = LST;
                    dr[1] = row[1];
                    dr[2] = plu;
                    dr[3] = f1;
                    dr[4] = f2;
                    dr[5] = f3;
                    dr[6] = S;
                    dt.Rows.Add(dr);
                }
                catch (Exception ex) { continue; }
            }
            string strSQL2 = string.Format("DELETE T_ForemanScore WHERE LST between  '{0}' AND '{1}'", fromTime, toTime);
            m_Database.Execute(strSQL2);//删除已有记录
            bool k = m_Database.BulkCopy(dt);
            dt.Clear();

        }
        public string ReturnChinaScore(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string[] modle = { "Manual", "WRF" };
            string[] modleName = { "主观预报", "WRF-chem" };
            string strSQL = string.Format("SELECT  LST,Module, f1, f2, f3, S from  T_ChinaEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            DataTable timeTable = dt.DefaultView.ToTable(true, "LST");
            string filter = "";
            int[] day = new int[2];
            DataRow[] dataRows;
            string[] className = { "tableRow", "tableRowBottom" };
            StringBuilder sb = new StringBuilder();
            sb.Append("<table  class='evaluate' width='100%' border='0' cellpadding='0' cellspacing='0'>");
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
                        sb.Append("<td class='tableRowBottom' rowspan='1'>" + DateTime.Parse(rows[0].ToString()).ToString("MM月dd日") + "</td>");
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

        public string ReturnChinaTalbe(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string[] modle = { "RT", "Manual", "WRF" };
            string[] modleName = { "实况", "主观预报", "WRF-chem" };
            string strSQL = string.Format("SELECT  Module,COUNT(Quality) as count,D_Parameter.MC from  T_ChinaEvaluation as a join  D_Parameter  on a.Quality=D_Parameter.MC Where LST between '{0}' and '{1}' group by Module,Quality,MC;", fromTime, toTime);
            strSQL = strSQL + string.Format("select Module, count(Parameter) as count,Parameter as MC  from  T_ChinaEvaluation  Where LST between '{0}' and '{1}' group by Module,Parameter;", fromTime, toTime);
            strSQL = strSQL + string.Format("SELECT Module,avg(f1) as f1,avg(f2) as f2,avg(f3) as f3,avg( S) as s FROM  T_ChinaEvaluation where Module<>'RT' and LST between '{0}' and '{1}'  group by Module order by Module;", fromTime, toTime);
            DataSet dSet = m_Database.GetDataset(strSQL);
            string[][] para = new string[2][];
            para[0] = new string[6] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
            para[1] = new string[4] { "PM2.5", "PM10", "NO2", "O3" };
            string filter = "";
            DataRow[] dataRows;
            int[] day = new int[3];
            DataTable dt;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder("{");
            StringBuilder strData = new StringBuilder();
            for (int j = 0; j < dSet.Tables.Count; j++)
            {
                sb = new StringBuilder();
                sb.Append("<table   width='100%' border='0' cellpadding='0' cellspacing='0'>");
                dt = dSet.Tables[j];
                if (j == dSet.Tables.Count - 1)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='tabletitle'></td>");
                    sb.Append("<td class='tabletitleChild'>首要污染物</br>正确性评分</td>");
                    sb.Append("<td class='tabletitleChild'>AQI预报级别</br>正确性评分</td>");
                    sb.Append("<td class='tabletitleChild'>AQI预报数值</br>误差评分</td>");
                    sb.Append("<td class='tabletitleChild'>综合评分</td>");
                    sb.Append("</tr>");
                    for (int m = 0; m < dt.Rows.Count; m++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class='tableRow'>" + modleName[m + 1] + "</td>");
                        for (int t = 1; t < dt.Columns.Count; t++)
                        {
                            sb.Append("<td class='tableRowChild'>" + Math.Round(double.Parse(dt.Rows[m][t].ToString()), 1) + "</td>");
                        }
                        sb.Append("</tr>");
                    }

                }
                else
                {

                    for (int i = -1; i < modleName.Length; i++)
                    {
                        sb.Append("<tr>");
                        if (i == -1)
                            sb.Append("<td class='tabletitle'></td>");
                        else
                            sb.Append("<td class='tableRow'>" + modleName[i] + "</td>");
                        for (int k = 0; k < para[j].Length; k++)
                        {
                            if (i == -1)
                                sb.Append("<td class='tabletitleChild'>" + para[j][k] + "</td>");
                            else
                            {
                                filter = string.Format("Module='{0}' and MC='{1}'", modle[i], para[j][k]);
                                dataRows = dt.Select(filter);
                                if (dataRows.Length > 0)
                                    sb.Append("<td class='tableRowChild'>" + dataRows[0][1] + "</td>");
                                else
                                    sb.Append("<td class='tableRowChild'>0</td>");
                            }
                        }
                        sb.Append("</tr>");

                    }
                    int t = 0;
                    string tempStr = "";
                    if (j == 0)
                    {
                        for (int i = 0; i < modle.Length; i++)
                        {
                            filter = string.Format("Module='{0}'", modle[i]);
                            dataRows = dt.Select(filter);
                            if (dt.Compute("SUM(count)", filter).ToString() == "")
                                day[i] = 0;
                            else
                                day[i] = int.Parse(dt.Compute("SUM(count)", filter).ToString());
                            if (day[i] - totalDays < 0)
                            {
                                strData.Append("<tr>");
                                sb.Append("<tr>");
                                if (t == 0)
                                {
                                    if (modleName[i] == "WRF-chem")
                                        tempStr = "注：WRF-chem模式预报数据缺" + (totalDays - day[i]).ToString() + "日";
                                    else
                                        tempStr = "注：" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                                }
                                else
                                    tempStr = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + modleName[i] + "数据缺" + (totalDays - day[i]).ToString() + "日";
                                t++;
                                strData.Append("<td class='tableRowOther' colspan='" + para[j].Length + 1 + "'>" + tempStr + "</td>");
                                sb.Append("<td class='tableRowOther' colspan='" + para[j].Length + 1 + "'>" + tempStr + "</td>");
                                sb.Append("</tr>");
                                strData.Append("</tr>");
                            }
                        }
                    }
                    else
                    {
                        sb.Append(strData.ToString());
                    }

                }
                sb.Append("</table>");
                sbReturn.AppendFormat("\"coutTable{0}\":\"{1}\",", j.ToString(), sb.ToString());
            }
            if (sbReturn.Length > 1)
                sbReturn = sbReturn.Remove(sbReturn.Length - 1, 1);
            sbReturn.Append("}");
            return sbReturn.ToString();



        }
        //public DataTable GetWrf(DataTable WRfTable)
        //{
        //    DataTable dt = new DataTable("T_DurationEvaluation");
        //    dt.Columns.Add("LST", typeof(DateTime));
        //    dt.Columns.Add("AQI", typeof(string));
        //    dt.Columns.Add("DurationID", typeof(int));
        //    dt.Columns.Add("ITEMID", typeof(int));
        //    DataTable dTime = WRfTable.DefaultView.ToTable(true, "LST");
        //    string filter = "";
        //    DataRow[] newRow;
        //    int AQIValue = 0;
        //    int[] duration = { 6, 2, 3 };
        //    foreach (DataRow rows in dTime.Rows)
        //    {
        //        for (int i = 0; i < duration.Length; i++)
        //        {
        //            DataRow EvaluationRow = dt.NewRow();

        //            filter = string.Format("durationID='{0}' and LST='{1}'", duration[i], rows[0]);
        //            newRow = WRfTable.Select(filter);
        //            if (newRow.Length > 0)
        //            {
        //                foreach (DataRow rowData in newRow)
        //                {
        //                    DataRow EvaluationRow1 = dt.NewRow();
        //                    AQIValue = int.Parse(rowData["AQI"].ToString());// ToAQI(rowData[2].ToString(), rowData[1].ToString());
        //                    EvaluationRow1[0] = rowData[0].ToString();
        //                    EvaluationRow1[1] = AQIValue;

        //                    EvaluationRow1[2] = duration[i];
        //                    EvaluationRow1[3] = rowData[1].ToString();
        //                    dt.Rows.Add(EvaluationRow1);
        //                }

        //            }

        //        }
        //    }
        //    return dt;
        //}


        public DataTable GetWrf(DataTable WRfTable)
        {
            DataTable dt = new DataTable("T_DurationEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("DurationID", typeof(int));
            dt.Columns.Add("ITEMID", typeof(int));
            DataTable dTime = WRfTable.DefaultView.ToTable(true, "ForecastDate");
            string filter = "";
            DataRow[] newRow;
            int AQIValue = 0;
            int[] duration = { 2, 3, 6 };
            foreach (DataRow rows in dTime.Rows)
            {
                for (int i = 0; i < duration.Length; i++)
                {
                    DataRow EvaluationRow = dt.NewRow();

                    filter = string.Format("durationID='{0}' and ForecastDate='{1}'", duration[i], rows[0]);
                    newRow = WRfTable.Select(filter);
                    if (newRow.Length > 0)
                    {
                        foreach (DataRow rowData in newRow)
                        {
                            DataRow EvaluationRow1 = dt.NewRow();
                            AQIValue = int.Parse(rowData[6].ToString());
                            
                            EvaluationRow1[0] = rowData[1].ToString();
                            EvaluationRow1[1] = AQIValue;

                            EvaluationRow1[2] = duration[i];
                            EvaluationRow1[3] = rowData[2].ToString();
                            dt.Rows.Add(EvaluationRow1);
                        }
                    }
                }
            }
            return dt;
        }
        public DataTable GetWrfII(DataTable WRfTable)
        {
            DataTable dt = new DataTable("T_DurationEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("DurationID", typeof(int));
            dt.Columns.Add("ITEMID", typeof(int));
            dt.Columns.Add("Module", typeof(string));
            DataTable dTime = WRfTable.DefaultView.ToTable(true, "LST");
            string filter = "";
            DataRow[] newRow;
            int AQIValue = 0;
            int[] duration = { 2, 3, 6 };
            foreach (DataRow rows in dTime.Rows)
            {
                for (int i = 0; i < duration.Length; i++)
                {
                    DataRow EvaluationRow = dt.NewRow();

                    filter = string.Format("durationID='{0}' and LST='{1}'", duration[i], rows[0]);
                    newRow = WRfTable.Select(filter);
                    if (newRow.Length > 0)
                    {
                        foreach (DataRow rowData in newRow)
                        {
                            DataRow EvaluationRow1 = dt.NewRow();
                            try
                            {
                                AQIValue = int.Parse(rowData["AQI"].ToString());
                            }
                            catch {
                                AQIValue = -1;
                            }
                            EvaluationRow1[0] = rowData["LST"].ToString();
                            EvaluationRow1[1] = AQIValue;

                            EvaluationRow1[2] = duration[i];
                            EvaluationRow1[3] = rowData["ITEMID"].ToString();
                            EvaluationRow1["Module"] = rowData["Module"].ToString();
                            dt.Rows.Add(EvaluationRow1);
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable GetForecast(DataTable FTable)
        {
            DataTable dt = new DataTable("T_DurationEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("DurationID", typeof(int));
            dt.Columns.Add("ITEMID", typeof(int));
            DataTable dTime = FTable.DefaultView.ToTable(true, "ForecastDate");
            string filter = "";
            DataRow[] newRow;
            int AQIValue = 0;
            int[] duration = { 2, 3 , 6 };
            foreach (DataRow rows in dTime.Rows)
            {
                for (int i = 0; i < duration.Length; i++)
                {
                    DataRow EvaluationRow = dt.NewRow();

                    filter = string.Format("durationID='{0}' and ForecastDate='{1}'", duration[i], rows[0]);
                    newRow = FTable.Select(filter);
                    if (newRow.Length > 0)
                    {
                        foreach (DataRow rowData in newRow)
                        {
                            DataRow EvaluationRow1 = dt.NewRow();
                            try
                            {
                                AQIValue = int.Parse(rowData[2].ToString());
                            }
                            catch { AQIValue = -1; }

                            EvaluationRow1[0] = rowData[1].ToString();//
                            EvaluationRow1[1] = AQIValue;

                            EvaluationRow1[2] = duration[i];
                            EvaluationRow1[3] = rowData[7].ToString();
                            dt.Rows.Add(EvaluationRow1);
                        }
                    }
                }
            }
            return dt;
        }

        //public string getLine(DataTable dt, int itemID, string index)
        //{
        //    string x = "";
        //    string y = "";
        //    string strReturn = "";
        //    string filter = "";
        //    if (itemID == 5 || itemID == 4)
        //        filter = string.Format("ITEMID='{0}' and durationID='3'", itemID);
        //    else
        //        filter = string.Format("ITEMID='{0}'", itemID);
        //    DataRow[] rows = dt.Select(filter);
        //    DateTime dtTime;
        //    if (rows.Length > 0)
        //    {
        //        foreach (DataRow dr in rows)
        //        {
        //            if (dr[1].ToString() != "" && dr[1].ToString() != "null")
        //            {
        //                if (itemID == 4 || itemID == 5)
        //                {
        //                    dtTime = DateTime.Parse(DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd 00:00:00"));
        //                }
        //                else
        //                {
        //                    dtTime = DateTime.Parse(dr[0].ToString());

        //                    //用小时来判断不准，我改成用durationID来判断
        //                    string durationID = dr["durationID"].ToString();
        //                    if (durationID == "3")
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 08:00:00"));
        //                    else if (durationID == "6")
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 16:00:00"));
        //                    else if (durationID == "2")
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 23:00:00"));
        //                }
        //                x = x + "|" + (dtTime - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
        //                y = y + "|" + dr[1].ToString();
        //            }

        //        }
        //        strReturn = ",'" + index + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
        //    }
        //    return strReturn;
        //}

        public string getLine(DataTable dt, int itemID, string index)
        {
            string x = "";
            string y = "";
            string strReturn = "";
            string filter = "";
            if (itemID == 5 || itemID == 4)
                filter = string.Format("ITEMID='{0}' and durationID='3'", itemID);
            else
                filter = string.Format("ITEMID='{0}'", itemID);
            DataRow[] rows = dt.Select(filter);
            DateTime dtTime;
            string dtimes = "";
            if (rows.Length > 0)
            {
                foreach (DataRow dr in rows)
                {
                    if (dr[1].ToString() != "" && dr[1].ToString() != "null")
                    {
                        try
                        {
                            if (itemID == 4 || itemID == 5)
                            {
                                dtimes = DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd 00:00:00");
                            }
                            else
                            {
                                dtTime = DateTime.Parse(DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                                if (dtTime.Hour == 6)
                                    dtimes = dtTime.ToString("yyyy-MM-dd 08:00:00");
                                else if (dtTime.Hour == 12)
                                    dtimes = dtTime.ToString("yyyy-MM-dd 16:00:00");
                                else
                                    dtimes = dtTime.ToString("yyyy-MM-dd 23:00:00");
                            }
                            x = x + "|" + (DateTime.Parse(dtimes) - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
                            y = y + "|" + dr[1].ToString();
                        }
                        catch (Exception ex) {
                            string str = ex.Message;
                        }
                    }

                }
                strReturn = ",'" + index + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            return strReturn;
        }
        //public string getLineModule(DataTable dt, int itemID, string index, string module)
        //{
        //    string x = "";
        //    string y = "";
        //    string strReturn = "";
        //    string filter = "";

        //    //if (itemID == 5 || itemID == 4)
        //    //    filter = string.Format("ITEMID='{0}' and durationID='3'", itemID);
        //    //else
        //    filter = string.Format("ITEMID='{0}' and Module='{1}'", itemID, module);
        //    DataRow[] rows = dt.Select(filter);
        //    DateTime dtTime;
        //    if (rows.Length > 0)
        //    {
        //        foreach (DataRow dr in rows)
        //        {
        //            if (dr[3].ToString() != "" && dr[3].ToString() != "null")
        //            {
        //                dtTime = DateTime.Parse(dr[0].ToString());
        //                if (itemID == 4 || itemID == 5)
        //                {
        //                    dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 00:00:00"));
        //                }
        //                else
        //                {
        //                    if (dtTime.Hour == 6)
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 08:00:00"));
        //                    else if (dtTime.Hour == 12)
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 16:00:00"));
        //                    else
        //                        dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 23:00:00"));
        //                }
        //                x = x + "|" + (dtTime - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
        //                y = y + "|" + dr[3].ToString();
        //            }

        //        }
        //        strReturn = ",'" + index + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
        //    }
        //    return strReturn;
        //}
        public string getLineModule(DataTable dt, int itemID, string index, string module)
        {
            string x = "";
            string y = "";
            string strReturn = "";
            string filter = "";

            if (itemID == 5 || itemID == 4)
                filter = string.Format("ITEMID='{0}' and Module='{1}'  and durationID='3'", itemID, module);
            else
                filter = string.Format("ITEMID='{0}' and Module='{1}'", itemID, module);
            DataRow[] rows = dt.Select(filter);
            DateTime dtTime;
            if (rows.Length > 0)
            {
                foreach (DataRow dr in rows)
                {

                    if (dr["AQI"].ToString() != "" && dr["AQI"].ToString() != "null")
                    {
                        dtTime = DateTime.Parse(dr[0].ToString());
                        if (itemID == 4 || itemID == 5)
                        {
                            dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 00:00:00"));
                        }
                        else
                        {
                            if (dtTime.Hour == 6)
                                dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 08:00:00"));
                            else if (dtTime.Hour == 12)
                                dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 16:00:00"));
                            else
                                dtTime = DateTime.Parse(dtTime.Date.ToString("yyyy-MM-dd 23:00:00"));
                        }
                        x = x + "|" + (dtTime - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
                        y = y + "|" + dr["AQI"].ToString();
                    }
                }
                strReturn = ",'" + index + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            return strReturn;
        }

        public string getLineShi(DataTable dt, int itemID)
        {
            string x = "";
            string y = "";
            string strReturn = "";
            string filter = "";
            DataRow[] rows;
            if (itemID == 5 || itemID == 4)
            {
                filter = string.Format("durationID='3'", itemID);
                rows = dt.Select(filter);
            }
            else
            {
                rows = dt.Select();
            }

            if (rows.Length > 0)
            {
                foreach (DataRow dr in rows)
                {
                    if (itemID == 5 || itemID == 4)
                        x = x + "|" + (DateTime.Parse(DateTime.Parse(dr[0].ToString()).ToString("yyyy-MM-dd 00:00:00")) - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
                    else
                        x = x + "|" + (DateTime.Parse(dr[0].ToString()) - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
                    y = y + "|" + dr[itemID].ToString();

                }
                strReturn = "'0':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            return strReturn;
        }
        public string GetValues(string url)
        {

            string formatUrl = url;
            //发送http请求，获得值
            var request = WebRequest.Create(formatUrl);
            var stream = request.GetResponse().GetResponseStream();
            string value = string.Empty;
            using (var reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))//返回值包含中文字符，使用gb2312代码解析
            {
                value = reader.ReadToEnd();
            }
            return value;
        }
        public string GetValues(string dateMoth, string method)
        {
            string formatUrl = FormatUrl(DateTime.Parse(dateMoth).Date.ToString("yyyy-MM-01"), method);
            //发送http请求，获得值
            var request = WebRequest.Create(formatUrl);
            var stream = request.GetResponse().GetResponseStream();
            string value = string.Empty;
            using (var reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))//返回值包含中文字符，使用gb2312代码解析
            {
                value = reader.ReadToEnd();
            }
            return value;

        }
        public string FormatUrl(string dateTime, string method)
        {
            return string.Format("{0}{1}", "http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=" + method + "&dateTime=", dateTime);
        }
        public string ChinaChart(string dateTime)
        {
            DataTable dt = new DataTable("T_ChinaEvaluation");
            dt.Columns.Add("LST", typeof(DateTime));

            dt.Columns.Add("AQI", typeof(int));
            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Quality", typeof(string));
            dt.Columns.Add("Grade", typeof(int));
            dt.Columns.Add("Parameter", typeof(string));
            dt.Columns.Add("userID", typeof(string));
            StringBuilder sb = new StringBuilder("{");
            string strReturn = "";
            string[] ItemArray = { "1", "PM25", "PM10", "NO2", "O3" };
            DateTime time = DateTime.Parse(dateTime);
            string fromTime = time.Date.ToString("yyyy-MM-dd 00:00:00");
            string toTime = time.AddMonths(1).Date.AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
            //实况数据
            //string strSQL = string.Format(" SELECT  DATEDIFF(S,'1970-01-01 20:00:00', DateAdd(dd,-1,time_point)) as LST, AQI_PM25 as PM25,AQI_PM10 as PM10,AQI_NO2 as NO2,AQI_Ozone1 as O3,AQI_Ozone8,AQI , primary_pollutant  FROM T_24hAQI  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市' and  datename(hour, time_point)=20 ORDER BY time_point;", fromTime, toTime);

            string strSQL = string.Format("  SELECT DATEDIFF(S,'1970-01-01 20:00:00', DateAdd(dd,1,lst)) as LST," +
                   " Max(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'PM25',"+
                   " Max(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'PM10',"+
                   " Max(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'NO2',    "+ 
                   " Max(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'O3',"+
                   " Max(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8',"+
                   " Max(CASE ITEMID WHEN 0 THEN AQI ELSE 0 END) AS 'primary_pollutant',"+
                   " max(AQI) as 'Pollution'"+
                   " FROM T_ChinaShiValueII where LST BETWEEN '{0}' and '{1}'" +
                   " GROUP BY LST order by LST asc", fromTime, toTime);

            DataTable RTDataTable = m_Database.GetDataTable(strSQL);
            //主观数据  (07-24 ToAQI这个算法是不是有问题)
            strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 00:00:00', LST) as LST, PM25, PM10, NO2, O3, AQI, Quality, Grade, Parameter FROM T_ChinaValue Where LST Between '{0}' and '{1}' and (Module = 'Manual') order by lst", fromTime, toTime);
            DataTable ForecastTable = m_Database.GetDataTable(strSQL);
            //模式数据
            fromTime = DateTime.Parse(fromTime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
            toTime = DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd 20:59:59");
          //  strSQL = string.Format("select DATEDIFF(S,'1970-01-01 20:00:00', LST) as LST,ITEMID,Value,D_Item.MC, Module from (SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and Interval=24 and durationID=7 group by LST,ITEMID,Module) a left join D_Item on D_Item.DM=a.ITEMID order by LST,ITEMID ;", fromTime, toTime);

            strSQL = string.Format(" select  DATEDIFF(S,'1970-01-01 20:00:00', LST) as LST,ITEMID,Value,D_Item.MC, Module " +
	               " FROM  T_ForecastGroup a  "+
	               " left join D_Item on D_Item.DM=a.ITEMID   "+
                   " where ForecastDate between '{0}' and '{1}' and  ITEMID <6 and  PERIOD=48 and " +
                   " durationID=7 and Module='WRFChem'  order by LST,ITEMID ;", fromTime, toTime);

            DataTable WRfTable = m_Database.GetDataTable(strSQL);
            for (int i = 1; i < ItemArray.Length; i++)
            {
                strReturn = "";
                strReturn = strReturn + (getLineChina(RTDataTable, ItemArray[i], "0", i.ToString()));
                strReturn = strReturn + (getLineChina(ForecastTable, ItemArray[i], "1", i.ToString()));
                strReturn = strReturn + (getLineChina(WRfTable, i.ToString(), "2", i.ToString()));
                if (strReturn != ",")
                    strReturn = "'" + (i - 1).ToString() + "':{" + strReturn + "},";
                sb.Append(strReturn);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
        public string getLineChina(DataTable dt, string itemID, string id, string flag)
        {
            string x = "";
            string y = "";
            string strReturn = "";
            string filter = "";
            DataRow[] rows;
            if (id != "2")
            {
                foreach (DataRow ChinaRows in dt.Rows)
                {
                    if (ChinaRows[itemID].ToString() != "")
                    {
                        x = x + "|" + ChinaRows[0].ToString();
                        if (id == "1")
                        {
                            y = y + "|" + ToAQI(ChinaRows[itemID].ToString(), flag);
                        }
                        else
                            y = y + "|" + ChinaRows[itemID];
                    }
                }
            }
            else
            {
                filter = string.Format("ITEMID='{0}'", itemID);
                rows = dt.Select(filter);

                if (rows.Length > 0)
                {
                    foreach (DataRow dr in rows)
                    {
                        if (dr[2].ToString() != "")
                        {
                            x = x + "|" + dr[0].ToString();
                            y = y + "|" + ToAQI(dr[2].ToString(), flag);
                        }

                    }
                }
            }
            if (id == "0")
                strReturn = "'" + id + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            else
                strReturn = ",'" + id + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            return strReturn;
        }

        public string IAQIChart(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-02 00:00:00");
            string fromTimeII = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddMonths(1).ToString("yyyy-MM-01 23:59:59");

            StringBuilder sb = new StringBuilder("{");
            string strReturn = "";

            string strSQL = "";// string.Format("SELECT  CONVERT(varchar(100), time_point, 23)+' 08:00:00' AS time_point,AQI_PM25 ,AQI_PM10 ,AQI_NO2 ,AQI_Ozone1,AQI_Ozone8,'3' as durationID  FROM T_AMAQI  Where time_point between '{0}' and '{1}' and area='上海市' union  SELECT  CONVERT(varchar(100), time_point, 23)+' 16:00:00' AS time_point ,AQI_PM25,AQI_PM10,AQI_NO2,AQI_Ozone1,AQI_Ozone8,'6' as durationID  FROM T_PMAQI  Where time_point between '{0}' and '{2}' and area='上海市' union SELECT  CONVERT(varchar(100), dateadd(day,-1,time_point), 23)+' 23:00:00' AS time_point,AQI_PM25,AQI_PM10,AQI_NO2,AQI_Ozone1,AQI_Ozone8,'2' as durationID  FROM T_NightAQI  Where time_point between '{0}' and '{1}' and area='上海市' Order by time_point;", fromTime, DateTime.Parse(toTime).ToString("yyyy-MM-dd 12:00:00"), toTime);

            strSQL = "SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 08:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                   "    MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                    "	MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                    "	MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                    "	MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'2' as durationID      " +
                   "    FROM T_shiTable" +
                   "    WHERE DurationID='2' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST " +
                   "    union" +
                   "    SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 16:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                   "                MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                        "	        MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                        "	        MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                        "	        MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'3' as durationID      " +
                   "    FROM T_shiTable" +
                  "     WHERE DurationID='3' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST " +
                  "      union" +
                  "     SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 23:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                  "                 MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                    "	            MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                    "	            MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                    "	            MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'6' as durationID      " +
                   "    FROM T_shiTable" +
                   "    WHERE DurationID='6' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST ";
            DataTable shiTalbe = m_Database.GetDataTable(string.Format(strSQL, fromTimeII,DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")));//实况分段  08-09 xuehui 


           // strSQL = string.Format("SELECT ForecastDate,LST,ITEMID,avg(Value) as Value,Module,durationID FROM  T_ForecastSite where   ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and PERIOD=24  and  durationID in (2,3,6) and ForecastDate between '{0}' and '{1}' group by LST,ITEMID,Module,durationID,ForecastDate  order by LST,ForecastDate,ITEMID ;", DateTime.Parse(dateTime).ToString("yyyy-MM-dd 00:00:00"), DateTime.Parse(dateTime).AddMonths(1).ToString("yyyy-MM-01 12:59:00"));
            //DataTable wrfTable = GetWrf(m_Database.GetDataTable(strSQL));

            //模式数据
            DateTime sTime = DateTime.Parse(dateTime);
            DateTime tTime =DateTime.Parse(dateTime).AddMonths(1);
            strSQL = "SELECT  Convert(DateTime,REPLACE(CONVERT(VARCHAR(10),DATEADD(day,-1,LST),111),N'/0','/')+' 20:00:00') as ForecastDate,Convert(DateTime,REPLACE(CONVERT(VARCHAR(10),DATEADD(day,-1,LST),111),N'/0','/')+' 0:00:00') as LST,ITEMID,Value,'WRF' " +
                             "as Module,DurationID,AQI FROM T_ForecastGroup WHERE  Module='WRFChem'  " +
                             "and PERIOD=48 and ForecastDate between  '" + sTime.AddDays(-1).ToString("yyyy-MM-dd") + " 20:00:00' and '" + tTime.AddDays(-2).ToString("yyyy-MM-dd") + " 23:00:00' and durationID in (2,3) and ItemID<6  " +
                              "  union  " +
                            " SELECT  Convert(DateTime,REPLACE(CONVERT(VARCHAR(10),DATEADD(day,1,LST),111),N'/0','/')+' 20:00:00') as ForecastDate,Convert(DateTime,REPLACE(CONVERT(VARCHAR(10),DATEADD(day,1,LST),111),N'/0','/')+' 0:00:00') as LST,ITEMID,Value,'WRF'  " +
                            " as Module,DurationID,AQI FROM T_ForecastGroup WHERE Module='WRFChem'  " +
                             "and PERIOD=48 and ForecastDate between  '" + sTime.AddDays(-2).ToString("yyyy-MM-dd") + " 20:00:00' and '" + tTime.AddDays(-3).ToString("yyyy-MM-dd") + " 23:00:00' " +
                             " and durationID =6  and ItemID<6 ORDER BY LST asc ";
            DataTable wrfTables = m_Database.GetDataTable(strSQL);
            foreach (DataRow row in wrfTables.Rows)
            {
                //数据处理
                string durationID = row["DurationID"].ToString();
                switch (durationID)
                {
                    case "6": row["LST"] = DateTime.Parse(row["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 20:00:00"); break;
                    case "2": row["LST"] = DateTime.Parse(row["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 06:00:00"); break;
                    case "3": row["LST"] = DateTime.Parse(row["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 12:00:00"); break;
                }
            }

            DataTable wrfTable = GetWrf(wrfTables);

            //string formatUrl = string.Format("{0}{1}",
            //    "http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=returnMonthForecastDataChart&dateTime=",
            //    DateTime.Parse(dateTime).ToString("yyyy-MM-01"));
            //string forecastStr = GetValues(formatUrl);
            //DataTable foreCastTable = JsonToDataTable(forecastStr);

            string strSQLII = "SELECT  DATEADD(day,-1,LST) as 'ForecastDate',DATEADD(day,-1,LST) as LST,AQI,'Manual' as Module,Value,DurationID,Parameter," +
                              "ITEMID,UserName FROM T_ForecastGroup WHERE Module='WRF' and ForecastDate between  '" + sTime + "' and '" + tTime.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")
                              + "' and durationID in (2,3)   and PERIOD=24 union SELECT LST as 'ForecastDate', LST,AQI,'Manual' as Manual,Value,DurationID,Parameter," +
                              "ITEMID,UserName FROM T_ForecastGroup WHERE Module='WRF' and ForecastDate between  '" + sTime + "' and '" + 
                              tTime.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' and durationID =6   and PERIOD=24 ORDER BY LST";
            DataTable foreCastTable = m_Database.GetDataTable(strSQLII);// 主观预报(气象局自己填写）
            foreach (DataRow row in foreCastTable.Rows)
            {
                //数据处理
                string durationID = row["DurationID"].ToString();
                switch (durationID)
                {
                    case "6": row["LST"] = DateTime.Parse(row["LST"].ToString()).ToString("yyyy-MM-dd 20:00:00"); break;
                    case "2": row["LST"] = DateTime.Parse(row["LST"].ToString()).ToString("yyyy-MM-dd 06:00:00"); break;
                    case "3": row["LST"] = DateTime.Parse(row["LST"].ToString()).ToString("yyyy-MM-dd 12:00:00"); break;
                }
            }
            foreCastTable = GetForecast(foreCastTable);

            for (int i = 1; i < 6; i++)
            {
                strReturn = "";
                strReturn = strReturn + (getLineShi(shiTalbe, i));
                strReturn = strReturn + (getLine(wrfTable, i, "1"));
                strReturn = strReturn + (getLine(foreCastTable, i, "2"));
                if (strReturn != ",")
                    strReturn = "'" + (i - 1).ToString() + "':{" + strReturn + "},";
                sb.Append(strReturn);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        public string CompareChart(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-02 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddMonths(1).ToString("yyyy-MM-01 23:59:59");
            string[] moduleArray = { "SMCSubmit", "ManualSubmit", "ManualCenter" };
            StringBuilder sb = new StringBuilder("{");
            string strReturn = "";

            string fromTimeII = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            //实况
            string strSQL = "SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 08:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                               "    MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                                "	MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                                "	MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                                "	MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'2' as durationID      " +
                               "    FROM T_shiTable" +
                               "    WHERE DurationID='2' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST " +
                               "    union" +
                               "    SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 16:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                               "                MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                                    "	        MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                                    "	        MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                                    "	        MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'3' as durationID      " +
                               "    FROM T_shiTable" +
                              "     WHERE DurationID='3' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST " +
                              "      union" +
                              "     SELECT CONVERT(varchar(100), DATEADD(day,0,LST), 23)+' 23:00:00' AS time_point, MAX(CASE ITEMID WHEN 1 THEN AQI ELSE 0 END) AS 'AQI_PM25'," +
                              "                 MAX(CASE ITEMID WHEN 2 THEN AQI ELSE 0 END) AS 'AQI_PM10'," +
                                "	            MAX(CASE ITEMID WHEN 3 THEN AQI ELSE 0 END) AS 'AQI_NO2'," +
                                "	            MAX(CASE ITEMID WHEN 4 THEN AQI ELSE 0 END) AS 'AQI_Ozone1'," +
                                "	            MAX(CASE ITEMID WHEN 5 THEN AQI ELSE 0 END) AS 'AQI_Ozone8' ,'6' as durationID      " +
                               "    FROM T_shiTable" +
                               "    WHERE DurationID='6' and LST BETWEEN '{0}'  and '{1}' GROUP BY LST ";
            DataTable shiTalbe = m_Database.GetDataTable(string.Format(strSQL, fromTimeII, DateTime.Parse(toTime).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss")));//实况分段  08-09 xuehui 
            //string strSQL = string.Format("SELECT  CONVERT(varchar(100), time_point, 23)+' 08:00:00' AS time_point,AQI_PM25 ,AQI_PM10 ,AQI_NO2 ,AQI_Ozone1,AQI_Ozone8,'3' as durationID  FROM T_AMAQI  Where time_point between '{0}' and '{1}' and area='上海市' union  SELECT  CONVERT(varchar(100), time_point, 23)+' 16:00:00' AS time_point ,AQI_PM25,AQI_PM10,AQI_NO2,AQI_Ozone1,AQI_Ozone8,'6' as durationID  FROM T_PMAQI  Where time_point between '{0}' and '{2}' and area='上海市' union SELECT  CONVERT(varchar(100), dateadd(day,-1,time_point), 23)+' 23:00:00' AS time_point,AQI_PM25,AQI_PM10,AQI_NO2,AQI_Ozone1,AQI_Ozone8,'2' as durationID  FROM T_NightAQI  Where time_point between '{0}' and '{1}' and area='上海市' Order by time_point;", fromTime, DateTime.Parse(toTime).ToString("yyyy-MM-dd 12:00:00"), toTime);
            //DataTable shiTalbe = m_Database.GetDataTable(strSQL);


            string formatUrl = string.Format("{0}{1}",
                "http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=returnDurationScorePudong&dateTime=",
                DateTime.Parse(dateTime).ToString("yyyy-MM-01"));

            string forecastStr = GetValues(formatUrl);
            DataTable wrfTable = JsonToDataTable(forecastStr);
            wrfTable = GetWrfII(wrfTable);
            foreach (DataRow dr in wrfTable.Rows)
            {
                string durationID = dr["DurationID"].ToString();
                switch (durationID)
                {
                    case "6": dr["LST"] = DateTime.Parse(dr["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 20:00:00"); break;
                    case "2": dr["LST"] = DateTime.Parse(dr["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 06:00:00"); break;
                    case "3": dr["LST"] = DateTime.Parse(dr["LST"].ToString()).AddDays(0).ToString("yyyy-MM-dd 12:00:00"); break;
                }
            }

            for (int i = 1; i < 6; i++)
            {
                strReturn = "";
                strReturn = strReturn + (getLineShi(shiTalbe, i));
                for (int j = 0; j < moduleArray.Length; j++)
                {
                    strReturn = strReturn + (getLineModule(wrfTable, i, (j + 1).ToString(), moduleArray[j]));
                }
                if (strReturn != ",")
                    strReturn = "'" + (i - 1).ToString() + "':{" + strReturn + "},";
                sb.Append(strReturn);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        public string QueryWRFDay(string fromDate, string period)
        {
            DateTime dtFrom = DateTime.Parse(fromDate).Date;
            DateTime dtTo = DateTime.Parse(fromDate).Date.AddMonths(1);
            string strSQL = "";
            if (period == "日")
                strSQL = string.Format("select a.LST,a.ITEMID,a.Value,D_Item.MC from(SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and LST between '{0}' and '{1}' and Interval=24 and durationID=7 group by LST,ITEMID,Module )a left join D_Item on a.ITEMID=D_Item.DM order by LST ", dtFrom.ToString("yyyy-MM-dd 00:00:00"), dtTo.ToString("yyyy-MM-dd 23:59:59"));
            else
                strSQL = string.Format("select a.LST,a.ITEMID,a.Value,D_Item.MC from(SELECT LST,ITEMID,avg(Value) as Value,Module FROM  T_ForecastSite where  ITEMID <6 and  Site in  ( select  station_co FROM sta_reg_set WHERE (flag <= 10) and flag<> 7) and LST between '{0}' and '{1}' and Interval=48 and durationID in (2,3,6) group by LST,ITEMID,Module )a left join D_Item on a.ITEMID=D_Item.DM order by LST ", dtFrom.ToString("yyyy-MM-dd 00:00:00"), dtTo.ToString("yyyy-MM-dd 23:59:59"));
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");
            StringBuilder sb = new StringBuilder();
            string parameter = "";
            int AQI = 0;
            int maxAQI = 0;
            string filter = "";
            sb.AppendLine("<table id='publicLogDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");
            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >日期</td>");
            sb.AppendLine("<td class='tabletitle'>首要污染物</td>");
            sb.AppendLine("<td class='tabletitle'>AQI</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-8h</td>");

            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                StringBuilder sm = new StringBuilder();
                rowIndex++;
                maxAQI = 0;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow' style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy年MM月dd日")));
                for (int i = 1; i < 6; i++)
                {
                    filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], i.ToString());
                    DataRow[] rows = dtSiteData.Select(filter);
                    for (int j = 0; j < rows.Length; j++)
                    {
                        AQI = CalcuDayAQI(rows[0][2].ToString(), i.ToString());
                        sm.AppendLine(string.Format("<td class='tablerow'>{0}</td>", AQI));
                        if (AQI > maxAQI)
                        {
                            parameter = rows[0][3].ToString() + " ";
                            maxAQI = AQI;

                        }
                        else if (AQI == maxAQI)
                            parameter = parameter + rows[0][3].ToString() + " ";


                    }
                }
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", parameter));
                sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", maxAQI));
                sb.AppendLine(sm.ToString());
                sb.AppendLine("</tr>");
            }


            sb.AppendLine("</table>");

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="siteIDs"></param>
        /// <returns></returns>
        public string Query(string fromDate, string toDate, string siteIDs, string curSiteID)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate);
            int[] itemOrder = { 8, 9, 3, 4, 2, 6, 7, 1, 5 };
            string formatUrl = string.Format("http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=AirQualityPuDong&fromDate={0}&toDate={1}&siteIDs={2}", fromDate, toDate, siteIDs);
            string forecastStr = GetValues(formatUrl);
            DataTable dtSiteData = JsonToDataTable(forecastStr);

            DataTable distinctLst = dtSiteData.DefaultView.ToTable(true, "LST");


            StringBuilder sb = new StringBuilder();
            StringBuilder tdValue = new StringBuilder();
            string className = string.Empty;
            sb.AppendLine("<table id='publicLogDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='tabletitle'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >日期</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>2.5</sub>-24h</td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub></td>");
            sb.AppendLine("<td class='tabletitle'>PM<sub>10</sub>-24h</td>");
            sb.AppendLine("<td class='tabletitle'>NO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-1h</td>");
            sb.AppendLine("<td class='tabletitle'>O<sub>3</sub>-8h</td>");
            sb.AppendLine("<td class='tabletitle'>SO<sub>2</sub></td>");
            sb.AppendLine("<td class='tabletitle'>CO</td>");
            sb.AppendLine("</tr>");
            int rowID = 0;
            AQIExtention aqiExt;
            string aqiColor;
            int rowIndex = 0;
            foreach (DataRow row in distinctLst.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  onmouseover='mouseOver(this)' onmouseout='mouseOut(this)' id='{0}'>", rowIndex));
                sb.AppendLine(string.Format("<td class='tablerow' style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9' >{0}</td>", DateTime.Parse(row[0].ToString()).ToString("yyyy-MM-dd HH:mm")));
                rowID = rowID + 1;
                for (int i = 0; i < itemOrder.Length; i++)
                {
                    string filter = string.Format("LST = '{0}' AND ITEMID = {1}", row[0], itemOrder[i]);
                    DataRow[] rows = dtSiteData.Select(filter);
                    tdValue.Length = 0;
                    for (int j = 0; j < rows.Length; j++)
                    {
                        if (string.Format("{0:000}", rows[j]["SITEID"]) != curSiteID)
                            className = "class='hidden'";
                        else
                            className = "class='show'";
                        if (rows[j]["AQI"].ToString() != "" && rows[j]["AQI"].ToString() != "null")
                        {
                            int AQIValue = int.Parse(rows[j]["AQI"].ToString());
                            aqiExt = new AQIExtention(AQIValue);
                            aqiColor = string.Format("class='{0}'", aqiExt.Color);
                            if (int.Parse(itemOrder[i].ToString()) == 5)
                            {
                                tdValue.AppendFormat("<div id='H{5}{6}{0:000}' {1}>{3}/<span {2}>{4}</span></div>", rows[j]["SITEID"], className, aqiColor, Math.Round(double.Parse(rows[j]["VALUE"].ToString()) / 1000, 1), rows[j]["AQI"], rowID, i);
                            }
                            else if (int.Parse(itemOrder[i].ToString()) == 8 || int.Parse(itemOrder[i].ToString()) == 3)
                            {
                                tdValue.AppendFormat("<div id='H{4}{5}{0:000}' {1}>{2}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"], rowID, i);
                            }
                            else
                            {
                                tdValue.AppendFormat("<div id='H{5}{6}{0:000}' {1}>{3}/<span {2}>{4}</span></div>", rows[j]["SITEID"], className, aqiColor, rows[j]["VALUE"], rows[j]["AQI"], rowID, i);
                            }
                        }
                        else
                        {
                            if (int.Parse(itemOrder[i].ToString()) == 8 || int.Parse(itemOrder[i].ToString()) == 3)
                            {
                                tdValue.AppendFormat("<div id='H{3}{4}{0:000}' {1}>{2}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"].ToString(), rowID, i);
                            }
                            else
                            {
                                tdValue.AppendFormat("<div id='H{4}{5}{0:000}' {1}>{2}/{3}</div>", rows[j]["SITEID"], className, rows[j]["VALUE"], rows[j]["AQI"], rowID, i);
                            }

                        }
                    }
                    sb.AppendLine(string.Format("<td class='tablerow'>{0}</td>", tdValue.ToString()));


                }
                sb.AppendLine("</tr>");
            }


            sb.AppendLine("</table>");

            return sb.ToString();
        }
        public string PersonYearData(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-01-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddYears(1).AddDays(-1).ToString("yyyy-MM-01 23:59:59");
            string strSQL = string.Format("SELECT CONVERT(VARCHAR(4),LST,120) as LST, UserName,SUM(WorkCount), SUM(SumSEMCScore), AVG(AVGSEMCScore), AVG(PM25Score), AVG(PM10Score), AVG(O31hScore), AVG(O38hScore), AVG(NO2Score), AVG(HazeScore), AVG(UVScore),"
                       + "AVG(PersonScore),RANK() OVER(ORDER BY AVG(PersonScore) DESC) as PersonScoreRank, SUM(SumChinaScore), AVG(SumChinaScore) ,RANK() OVER(ORDER BY AVG(AvgChinaScore) DESC) FROM T_PersonScore WHERE LST between  '{0}' AND '{1}' GROUP BY CONVERT(VARCHAR(4),LST,120),UserName order by PersonScoreRank", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTable1' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitlePerson' colspan='2'>预报员及班次</td>");
            sb.Append("<td class='tabletitlePerson' colspan='11'>常规预报评分</td>");
            sb.Append("<td class='tabletitlePerson' colspan='3'>上传国家局AQI评分</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitlePersonOther3' >预报员</td>");
            sb.Append("<td class='tabletitlePerson' >班次</td>");
            sb.Append("<td class='tabletitlePersonOther' >环境</br>总分</td>");
            sb.Append("<td class='tabletitlePersonOther' >环境</br>平均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >PM2.5平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >PM10平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >NO2平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >O3-1h平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >O3-8h平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >霾平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >UV平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >个人</br>总评分</td>");
            sb.Append("<td class='tabletitlePersonOther1' >排名</td>");
            sb.Append("<td class='tabletitlePersonOther' >总分</td>");
            sb.Append("<td class='tabletitlePersonOther' >平均分</td>");
            sb.Append("<td class='tabletitlePersonOther1' >排名</td>");
            sb.Append("</tr>");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("<tr>");
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        if (j != 1 && j != 2 && j != 13 && j != 16)
                        {
                            if (dt.Rows[i][j].ToString() != "")
                                sb.Append("<td class='tableRowPerson'>" + Math.Round(double.Parse(dt.Rows[i][j].ToString()), 1) + "</td>");
                            else
                                sb.Append("<td class='tableRowPerson'>" + dt.Rows[i][j] + "</td>");
                        }
                        else
                            sb.Append("<td class='tableRowPerson'>" + dt.Rows[i][j] + "</td>");
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        /// <summary>
        /// 月统计每个人的各项评分
        /// </summary>
        /// <param name="monthDate"></param>
        /// <returns></returns>
        public string PersonnalScoreData(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");

            string strSQL = string.Format("SELECT *,RANK() OVER(ORDER BY PersonScore DESC) as PersonScoreRank,RANK() OVER(ORDER BY AvgChinaScore DESC) AvgChinaScoreRank FROM T_PersonScore WHERE LST between  '{0}' AND '{1}' and userName<>'管理员' Order by PersonScore desc", fromTime, toTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table   width='100%' border='0' id='forecastTable' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitlePerson' colspan='2'>预报员及班次</td>");
            sb.Append("<td class='tabletitlePerson' colspan='11'>常规预报评分</td>");
            sb.Append("<td class='tabletitlePerson' colspan='3'>上传国家</br>局AQI评分</td>");
            sb.Append("<td class='tabletitlePerson' colspan='4'>附加分</td>");
            sb.Append("<td class='tabletitlePerson' colspan='2'>个例总结</td>");
            sb.Append("<td class='tabletitlePerson' rowspan='2' >漏登记次</td>");
            sb.Append("<td class='tabletitlePersonOther' rowspan='2' >总成绩</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td class='tabletitlePersonOther3' >预报员</td>");
            sb.Append("<td class='tabletitlePerson' >班次</td>");
            sb.Append("<td class='tabletitlePersonOther' >环境</br>总分</td>");
            sb.Append("<td class='tabletitlePersonOther' >环境</br>平均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >PM2.5平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >PM10平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >NO2平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >O3-1h平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >O3-8h平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >霾平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >UV平</br>均分</td>");
            sb.Append("<td class='tabletitlePersonOther' >个人</br>总评分</td>");
            sb.Append("<td class='tabletitlePersonOther1' >排名</td>");
            sb.Append("<td class='tabletitlePersonOther' >总分</td>");
            sb.Append("<td class='tabletitlePersonOther' >平均分</td>");
            sb.Append("<td class='tabletitlePersonOther1' >排名</td>");
            sb.Append("<td class='tabletitlePersonOther' >担任科</br>室工作</br>加分</td>");
            sb.Append("<td class='tabletitlePersonOther' >扣分</td>");
            sb.Append("<td class='tabletitlePersonOther' >带班</br>加分</td>");
            sb.Append("<td class='tabletitlePersonOther2'>扣分</br>原因</td>");
            sb.Append("<td class='tabletitlePersonOther' >应总结</br>个例数</td>");
            sb.Append("<td class='tabletitlePersonOther1' >完成个</br>例数</td>");
            //sb.Append("<td class='tabletitlePerson' >次</td>");
            sb.Append("</tr>");
            double totalScore = 0.0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("<tr >");
                    for (int j = 1; j < dt.Columns.Count - 2; j++)
                    {
                        if (j < 15)
                        {
                            sb.Append(string.Format("<td class='tableRowPerson' onclick=\"trMonthClick('{0}','{1}')\">{2}</td>", fromTime, dt.Rows[i]["UserName"], dt.Rows[i][j]));
                            if (j == 12)
                                sb.Append("<td class='tableRowPerson'>" + dt.Rows[i]["PersonScoreRank"] + "</td>");
                            if (j == 14)
                                sb.Append("<td class='tableRowPerson'>" + dt.Rows[i]["AvgChinaScoreRank"] + "</td>");
                        }
                        else
                        {
                            if (j == 18)
                                sb.Append("<td class='tableRowPerson'><textarea class='textarea' clos='50' rows='3' id='textarea" + i.ToString() + "_" + dt.Rows[i][1] + "'>" + dt.Rows[i][j] + "</textarea></td>");
                            else
                                sb.Append("<td class='tableRowPerson'><div  contentEditable='true' id='div" + i.ToString() + j.ToString() + "_" + dt.Rows[i][1] + "'>" + dt.Rows[i][j] + "</div></td>");
                        }


                    }
                    try
                    {
                        totalScore = double.Parse(dt.Rows[i]["SumSEMCScore"].ToString()) + double.Parse(dt.Rows[i]["HazeScore"].ToString() == "" ? "0" : dt.Rows[i]["HazeScore"].ToString()) + double.Parse(dt.Rows[i]["UVScore"].ToString()) + double.Parse(dt.Rows[i]["AvgChinaScore"].ToString()) + double.Parse(dt.Rows[i]["JobScore"].ToString() == "" ? "0" : dt.Rows[i]["JobScore"].ToString()) - double.Parse(dt.Rows[i]["DeductScore"].ToString() == "" ? "0" : dt.Rows[i]["DeductScore"].ToString()) + double.Parse(dt.Rows[i]["AddScore"].ToString() == "" ? "0" : dt.Rows[i]["AddScore"].ToString());
                    }
                    catch { }
                    sb.Append("<td class='tableRowPerson'>" + totalScore + "</td>");
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            return sb.ToString();

        }
        public string PersonnalResoreData(string dateTime, string content)
        {
            string monthTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string strSQL = "";
            int index = 0;
            string JobScore = "", DeductScore = "", AddScore = "", Reason = "", CountPerson = "", SumCount = "", ForgetCout = "";
            string[] splitContent = content.Split('|');
            string[] splitValue;
            string json = "";
            string name = "";
            string id = "";
            string value = "";
            string[] everyNameContent;
            string[] tempValue = new string[2];
            for (int i = 0; i < splitContent.Length; i++)
            {
                everyNameContent = splitContent[i].Split('*');
                json = everyNameContent[1];
                name = everyNameContent[0];
                splitValue = json.Split(',');
                for (int j = 0; j < splitValue.Length; j++)
                {
                    tempValue = splitValue[j].Split(':');
                    id = tempValue[0];
                    value = tempValue[1];
                    index = id.IndexOf("_");
                    if (id.Substring(index - 2, 2) == "15")
                        JobScore = value.Replace("<br>", "");
                    else if (id.Substring(index - 2, 2) == "16")
                        DeductScore = value.Replace("<br>", "");
                    else if (id.Substring(index - 2, 2) == "17")
                        AddScore = value.Replace("<br>", "");
                    else if (id.Substring(index - 1, 1) == i.ToString() && id.Substring(0, 8) == "textarea")
                        Reason = value.Replace("<br>", "");
                    else if (id.Substring(index - 2, 2) == "19")
                        CountPerson = value.Replace("<br>", "");
                    else if (id.Substring(index - 2, 2) == "20")
                        SumCount = value.Replace("<br>", "");
                    else if (id.Substring(index - 2, 2) == "21")
                        ForgetCout = value.Replace("<br>", "");
                }

                JobScore = JobScore.Replace("<BR>", "");
                DeductScore = DeductScore.Replace("<BR>", "");
                AddScore = AddScore.Replace("<BR>", "");

                strSQL = strSQL + string.Format("UPDATE T_PersonScore set JobScore='{0}',DeductScore='{1}', AddScore='{2}',Reason='{3}',CountPerson='{4}',SumCount='{5}',ForgetCout='{6}' WHERE UserName='{7}' and LST='{8}';", JobScore, DeductScore, AddScore, Reason, CountPerson, SumCount, ForgetCout, name, monthTime);

            }
            int count = m_Database.Execute(strSQL);
            if (count > 0)
                return "保存成功！";
            else
                return "保存失败";
        }
        public DataTable returnPersonScore(string dateTime)
        {

            double huanjingScore = 0.0;
            double hazeScore = 0.0;
            double UVScore = 0.0;
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            //分时段环境分数，包括每个时段的总分，值班次数
            string strSQL = "select SUM(F)/3 as sumF, COUNT(*)/3 as count, userID,SUM(F)/COUNT(*) as AvgScore from T_DurationEvaluation  where Module='Manual' and LST between '2015-12-01 00:00:00' and '2015-12-31 23:59:59' group by userID ;";
            //各个污染物分指数的总分，值班次数，其中臭氧只取下午时段，其他污染物是三个时段平均值
            strSQL = strSQL + "select SUM(Score)/COUNT(*) as score,COUNT(*)/3 as count,ITEMID,userID from T_IAQIEvaluation where Module='Manual' and LST between '2015-12-01 00:00:00' and '2015-12-31 12:59:59' group by ITEMID,userID;";

            //霾平均值
            strSQL = strSQL + string.Format("select AVG(Score) as score,COUNT(*) as count,UserID from T_HazeEvaluate  where  LST between '{0}' and '{1}' group by userID;", fromTime, toTime);
            //紫外线平均分
            strSQL = strSQL + string.Format("select AVG(Score) as score,COUNT(*) as count, UserID  from T_UVEvaluate  where  LST between '{0}' and '{1}' group by userID;", fromTime, toTime);
            //国家局总分，平均分以及值班次数
            strSQL = strSQL + "select AVG(S) as  avgS,SUM(S) as SumS,COUNT(*) as count,userID from T_ChinaEvaluation where Module='Manual'and LST between '2015-12-01 00:00:00' and '2015-12-31 23:59:59'  group by userID  ; ";
            DataSet dtSet = m_Database.GetDataset(strSQL);

            DataTable dt = new DataTable("T_PersonScore");
            dt.Columns.Add("LST", typeof(DateTime));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("WorkCount", typeof(int));

            dt.Columns.Add("SumSEMCScore", typeof(string));
            dt.Columns.Add("AVGSEMCScore", typeof(string));
            dt.Columns.Add("PM25Score", typeof(string));
            dt.Columns.Add("PM10Score", typeof(int));
            dt.Columns.Add("O31hScore", typeof(string));
            dt.Columns.Add("O38hScore", typeof(float));
            dt.Columns.Add("NO2Score", typeof(float));
            dt.Columns.Add("HazeScore", typeof(float));
            dt.Columns.Add("UVScore", typeof(float));
            dt.Columns.Add("PersonScore", typeof(float));
            dt.Columns.Add("SumChinaScore", typeof(float));
            dt.Columns.Add("AvgChinaScore", typeof(float));
            dt.Columns.Add("JobScore", typeof(string));
            dt.Columns.Add("DeductScore", typeof(float));
            dt.Columns.Add("AddScore", typeof(float));
            dt.Columns.Add("Reason", typeof(string));
            dt.Columns.Add("CountPerson", typeof(float));
            dt.Columns.Add("SumCount", typeof(float));
            dt.Columns.Add("ForgetCout", typeof(string));

            DataTable dPerson = dtSet.Tables[0].DefaultView.ToTable(true, "userID");
            string filter = "";
            DataRow[] filterRows;
            DataTable dm = new DataTable();
            foreach (DataRow rows in dPerson.Rows)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
                newRow[1] = rows[0];
                for (int i = 0; i < dtSet.Tables.Count; i++)
                {
                    filter = "userID='" + rows[0] + "'";
                    dm = dtSet.Tables[i];
                    filterRows = dm.Select(filter);
                    if (filterRows.Length > 0)
                    {
                        if (i == 0)
                        {
                            newRow[2] = filterRows[0][1];
                            newRow[3] = filterRows[0][0];
                            newRow[4] = filterRows[0][3];
                            huanjingScore = double.Parse(filterRows[0][3].ToString());
                        }
                        else if (i == 1)
                        {
                            for (int j = 1; j < 6; j++)
                            {
                                filter = "userID='" + rows[0] + "' and ITEMID='" + j + "'";
                                filterRows = dm.Select(filter);
                                if (filterRows.Length > 0)
                                {
                                    newRow[j + 4] = filterRows[0][0];
                                }
                                else
                                    newRow[j + 4] = DBNull.Value; ;
                            }
                        }
                        else if (i == 2)
                        {
                            newRow[10] = filterRows[0][0];
                            hazeScore = double.Parse(filterRows[0][0].ToString());
                        }
                        else if (i == 3)
                        {
                            newRow[11] = filterRows[0][0];
                            UVScore = double.Parse(filterRows[0][0].ToString());
                        }
                        else if (i == 4)
                        {
                            newRow[13] = filterRows[0][1];
                            newRow[14] = filterRows[0][0];
                        }

                        newRow[12] = 0.6 * huanjingScore + hazeScore + UVScore;
                        for (int k = 15; k < 22; k++)
                        {
                            newRow[k] = DBNull.Value;
                        }

                    }

                }
                dt.Rows.Add(newRow);

            }
            strSQL = string.Format("DELETE T_PersonScore WHERE LST between  '{0}' AND '{1}';", fromTime, toTime);
            m_Database.Execute(strSQL);//删除已有记录
            bool t = m_Database.BulkCopy(dt);
            return dt;
        }

        /// <summary>
        /// 导出雾霾实时数据模板
        /// </summary>
        /// <param name="datetime"></param>
        public string ExportHazeData(string TimeDate)
        {
            try
            {
                TimeDate = TimeDate.Replace("年", "-").Replace("月", "-");
                int year = Convert.ToInt16(TimeDate.Split('-')[0]);
                string month = TimeDate.Split('-')[1];
                int days = 0;
                if (month == "01" || month == "03" || month == "05" || month == "07" || month == "08" || month == "10" || month == "12") { days = 31; }
                else if (month == "04" || month == "06" || month == "09" || month == "11") { days = 30; }
                else if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) { days = 29; }
                else { days = 28; }
                string[] model = new string[40];
                model[0] = "日期        夜间站数      白天站数闵行    宝山    嘉定    崇明    徐家汇  南汇    浦东    金山    青浦    松江    奉贤    闵行    宝山    嘉定    崇明    徐家汇  南汇    浦东    金山    青浦    松江    奉贤";
                for (int i = 1; i < days + 1; i++)
                {
                    model[i] = TimeDate.Insert(TimeDate.Length, i.ToString().PadLeft(2, '0')) + "   0   0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0       0";
                }
                string path = System.Web.HttpContext.Current.Server.MapPath("~/") + "CJDATA\\Excel\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllLines(path + "model.txt", model, Encoding.Default);
                return "~/CJDATA/Excel/model.txt";
            }
            catch { return ""; }
        }

    }
}
