using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Readearth.Data;
using System.Configuration;

namespace MMShareBLL.DAL
{
    class NewEvalutionCaculate
    {
        private Database m_Database;
        public DataTable dIAQITable;
        string strSQL;
        public NewEvalutionCaculate()
        {
            m_Database = new Database("DBCONFIGEMFC");
            strSQL = string.Empty;
        }

        public string Process(string dateTIme)
        {
            try
            {
                LiveAVGAQI(Convert.ToDateTime(dateTIme));
                ChinaProcess(Convert.ToDateTime(dateTIme));
                return "ok";
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        public void LiveAVGAQI(DateTime date)
        {
            strSQL = string.Format("select * from T_NewConLive where Date between '{0}' and '{1}'", date.ToString("yyyy-MM-01"), date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:"));
        }
        public string FormatUrl(string dateTime, string method)
        {
            return string.Format("{0}{1}", "http://219.233.250.38:8087/semcshare/PatrolHandler.do?provider=MMShareBLL.DAL.AirData&method=" + method + "&dateTime=", dateTime);
        }
        public string GetValues(string dateMoth, string method)
        {
            string formatUrl = FormatUrl(DateTime.Parse(dateMoth).Date.ToString("yyyy-MM-01"), method);
            //发送http请求，获得值
            var request = WebRequest.Create(formatUrl);
            request.Timeout = 1000;
            var stream = request.GetResponse().GetResponseStream();
            string value = string.Empty;
            using (var reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))//返回值包含中文字符，使用gb2312代码解析
            {
                value = reader.ReadToEnd();
            }
            return value;

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
        /// <summary>
        /// 返回原数据表格
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        public string ReturnAllData(string dateMonth)
        {
            DateTime dNow = DateTime.Now;
            string beginTime = Convert.ToDateTime(dateMonth).ToString("yyyy-MM-01 00:00:00");
            string endTime = Convert.ToDateTime(dateMonth).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            if (dateMonth == dNow.ToString("yyyy年MM月"))
            {  //2018-06-21  王斌  
                endTime = dNow.ToString("yyyy-MM-dd 23:59:59");
            }
            string strSQL = string.Empty;
            Database UVdb = new Database("UVDBCONFIG");
            try
            {
                //雾霾预报
                strSQL = string.Format("select * from T_24HazeNew where  ReTime between '{0}' and '{1}'", beginTime, endTime);
                DataTable hazeFore = m_Database.GetDataTable(strSQL);
                //雾霾实况
                strSQL = string.Format("select * from T_RTData where  LST between '{0}' and '{1}'", beginTime, Convert.ToDateTime(endTime).AddDays(1).ToString("yyyy-MM-dd 23:59:59"));
                DataTable hazeLive = m_Database.GetDataTable(strSQL);
                //紫外线预报+实况
                strSQL = string.Format("SELECT convert(varchar(10),DATEADD(day,0,[ReTime]),120)+' 00:00:00' as LST, convert(varchar(2),[ReTime], 114) as hour, UVAB,[Index], UserName  FROM T_TbUVSNew WHERE ReTime between '{0}' and '{1}' order by lst", beginTime, endTime);
                DataTable UVData = m_Database.GetDataTable(strSQL);
                //污染物浓度实况（用于计算国家局与分时段评分）
                DataTable itemLive = new DataTable();
                try
                {
                    string LiveStr = GetValues(dateMonth, "AirQualityMonthDataII");
                    itemLive = JsonToDataTable(LiveStr);
                }
                catch
                {
                }


                //国家局预报
                //strSQL = string.Format("select * from T_ChinaValueNew where  LST between '{0}' and '{1}' AND Module ='Manual'", beginTime, endTime);
                //DataTable CNFore = m_Database.GetDataTable(strSQL);
                dateMonth = DateTime.Parse(dateMonth).ToString("yyyy-MM");
                 strSQL = "select SUBSTRING(CONVERT(varchar(20),lst, 20),0,11) as lst,aqi,itemId from T_ChinaShiValueII where  SUBSTRING(CONVERT(varchar(100),lst, 20),0,8) = '" + dateMonth + "'";
                DataTable CNFore = m_Database.GetDataTable(strSQL);

                //分时段预报
                strSQL = string.Format("select * from T_ForecastGroupNew where  durationID in (4,1,2,3) AND ForecastDate BETWEEN'{0}' AND '{1}' AND PERIOD =24 AND Module ='WRF'", beginTime, endTime);
                DataTable durFore = m_Database.GetDataTable(strSQL);
                //分时段环保
                strSQL = string.Format("select * from T_ForecastGroupNew where  durationID in (4,1,2,3) AND ForecastDate BETWEEN'{0}' AND '{1}' AND PERIOD =24 AND Module ='ENV'", beginTime, endTime);
                DataTable durForeEnv = m_Database.GetDataTable(strSQL);
                //24小时预报
                strSQL = string.Format("SELECT * FROM T_ForecastGroupNew where durationID=7 AND ForecastDate BETWEEN'{0}' AND '{1}' AND PERIOD =24 AND Module ='WRF'", beginTime, endTime);
                DataTable Fore_24h = m_Database.GetDataTable(strSQL);
                //48小时预报
                strSQL = string.Format("SELECT * FROM T_ForecastGroupNew where durationID=7 AND ForecastDate BETWEEN'{0}' AND '{1}' AND PERIOD =48 AND Module ='WRF'", beginTime, endTime);
                DataTable Fore_48h = m_Database.GetDataTable(strSQL);
                //环保局
                strSQL = string.Format("SELECT * FROM T_24hAQI  Where time_point BETWEEN '{0}' and '{1}' and  area='上海市' and datename(hour, time_point)=0 ORDER BY time_point;", beginTime, endTime);
                DataTable EPAFore = m_Database.GetDataTable(strSQL);

                StringBuilder sb = new StringBuilder();

                #region 表头
                sb.Append("<table   width='100%' border='0' id='forecastTable' cellpadding='0' cellspacing='0' word-wrap: break-word; word-break: break-all;>");

                sb.Append("<tr>");
                sb.Append("<td class='tabletitlePerson2' rowspan='3'><div class='cell'>日期</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='3'><div class='cell'>工号</div></td>");
                sb.Append("<td class='tabletitlePerson2' colspan='2'>霾实况</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='2'>UV实况</td>");

                sb.Append("<td class='tabletitlePerson2' colspan='14'>浓度实况</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='5'>国家局实况</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='8'>20-20实况</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                //sb.Append("<td class='tabletitlePersonOther'  rowspan='2'>05时预报</td>");
                //sb.Append("<td class='tabletitlePersonOther'  rowspan='2'>17时预报</td>");
                sb.Append("<td class='tabletitlePerson2'  rowspan='2'><div class='cell'>05时</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>17时</div></td>");

                //sb.Append("<td class='tabletitlePersonOther' rowspan='2'>紫外线实况值</td>");
                //sb.Append("<td class='tabletitlePersonOther1' rowspan='2'>紫外线实况</td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>10时</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'>16时</td>");


                sb.Append("<td class='tabletitlePerson2' colspan='4'>夜间</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='5'>上午</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='5'>下午</td>");

                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>O3-1h</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2' rowspan='2'><div class='cell'>NO2</div></td>");

                sb.Append("<td class='tabletitlePerson2' colspan='4'>20-20平均浓度</td>");
                sb.Append("<td class='tabletitlePerson2' colspan='4'>20-20AQI</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>NO2</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-1h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>NO2</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-1h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>NO2</div></td>");

                sb.Append("<td class='tabletitlePerson2'><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2'><div class='cell'>NO2</div></td>");

                sb.Append("<td class='tabletitlePerson2'><div class='cell'>PM2.5</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>O3-8h</div></td>");
                sb.Append("<td class='tabletitlePerson2' ><div class='cell'>PM10</div></td>");
                sb.Append("<td class='tabletitlePerson2'><div class='cell'>NO2</div></td>");
                sb.Append("</tr>");
                #endregion
                if (Fore_24h.Rows.Count > 0)
                {
                    for (DateTime time = Convert.ToDateTime(beginTime); time < Convert.ToDateTime(endTime); time = time.AddDays(1))
                    {
                        sb.Append("<tr>");
                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + time.ToString("MM月dd日") + "</div></td>");
                        DataRow[] ID = Fore_24h.Select("ForecastDate='" + time + "'");
                        if (ID.Length > 0)
                            //sb.Append("<td class='tableRowPerson'>" + ToUserCode(ID[0]["UserName"].ToString().Trim(' ')) + "</td>");
                            sb.Append("<td class='tableRowPerson'>" + ID[0]["UserName"].ToString().Trim(' ') + "</div></td>");
                        else
                            sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                        string filter = string.Empty;
                        DataRow[] drs = null;
                        //雾霾
                        #region  霾
                        string RTHaze = "无霾";
                        filter = "ReTime>='" + time.ToString("yyyy-MM-dd 00:00:00") + "' and ReTime<='" + time.ToString("yyyy-MM-dd 23:59:59") + "'";
                        // drs = hazeFore.Select(filter, "LST");
                        // if (drs.Length == 2)
                        // {
                        //     sb.Append("<td class='tableRowPerson'>" + returnHazeStyle(drs[0]["Haze"].ToString()) + "</td>");
                        //     sb.Append("<td class='tableRowPerson'>" + returnHazeStyle(drs[1]["Haze"].ToString()) + "</td>");


                        //     for (int i = 0; i < drs.Length; i++)
                        //     {

                        //         if (Convert.ToDateTime(drs[i]["ReTime"]).Hour == 5)
                        //             filter = "LST='" + time + "'";
                        //         else
                        //             filter = "LST='" + time.AddDays(1) + "'";
                        //         DataRow[] dr = hazeLive.Select(filter);
                        //         if (dr.Length > 0)
                        //         {
                        //             if (Convert.ToInt32(dr[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr[0]["NoneDays17"]) >= 2)
                        //                 RTHaze = "有霾";
                        //             else
                        //                 RTHaze = "无霾";
                        //             sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //         }
                        //         else
                        //             sb.Append("<td class='tableRowPerson'></td>");
                        //     }
                        // }
                        // else if (drs.Length == 1)
                        // {
                        //     if (Convert.ToDateTime(drs[0]["ReTime"]).Hour == 5)
                        //     {
                        //         sb.Append("<td class='tableRowPerson'>" + returnHazeStyle(drs[0]["Haze"].ToString()) + "</td>");
                        //         sb.Append("<td class='tableRowPerson'></td>");
                        //         filter = "LST='" + time + "'";
                        //         DataRow[] dr = hazeLive.Select(filter);
                        //         if (dr.Length > 0)
                        //         {
                        //             if (Convert.ToInt32(dr[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr[0]["NoneDays17"]) >= 2)
                        //                 RTHaze = "有霾";
                        //             else
                        //                 RTHaze = "无霾";
                        //             sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //         }
                        //         else
                        //             sb.Append("<td class='tableRowPerson'></td>");
                        //         filter = "LST='" + time.AddDays(1) + "'";
                        //         DataRow[] dr1 = hazeLive.Select(filter);
                        //         if (dr1.Length > 0)
                        //         {
                        //             if (Convert.ToInt32(dr1[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr1[0]["NoneDays17"]) >= 2)
                        //                 RTHaze = "有霾";
                        //             else
                        //                 RTHaze = "无霾";
                        //             sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //         }
                        //         else
                        //             sb.Append("<td class='tableRowPerson'></td>");
                        //     }
                        //     else
                        //     {
                        //         sb.Append("<td class='tableRowPerson'></td>");
                        //         sb.Append("<td class='tableRowPerson'>" + returnHazeStyle(drs[0]["Haze"].ToString()) + "</td>");
                        //         filter = "LST='" + time + "'";
                        //         DataRow[] dr1 = hazeLive.Select(filter);
                        //         if (dr1.Length > 0)
                        //         {
                        //             if (Convert.ToInt32(dr1[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr1[0]["NoneDays17"]) >= 2)
                        //                 RTHaze = "有霾";
                        //             else
                        //                 RTHaze = "无霾";
                        //             sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //         }
                        //         else
                        //             sb.Append("<td class='tableRowPerson'></td>");
                        //         filter = "LST='" + time.AddDays(1) + "'";
                        //         DataRow[] dr = hazeLive.Select(filter);
                        //         if (dr.Length > 0)
                        //         {
                        //             if (Convert.ToInt32(dr[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr[0]["NoneDays17"]) >= 2)
                        //                 RTHaze = "有霾";
                        //             else
                        //                 RTHaze = "无霾";
                        //             sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //         }
                        //         else
                        //             sb.Append("<td class='tableRowPerson'></td>");

                        //     }
                        // }
                        // else
                        // {
                        //     sb.Append("<td class='tableRowPerson'></td>");
                        //     sb.Append("<td class='tableRowPerson'></td>");
                        //     filter = "LST='" + time + "'";
                        //     DataRow[] dr = hazeLive.Select(filter);
                        //     if (dr.Length > 0)
                        //     {
                        //         if (Convert.ToInt32(dr[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr[0]["NoneDays17"]) >= 2)
                        //             RTHaze = "有霾";
                        //         else
                        //             RTHaze = "无霾";

                        //         sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //     }
                        //     else
                        //         sb.Append("<td class='tableRowPerson'></td>");
                        //     filter = "LST='" + time.AddDays(1) + "'";
                        //     DataRow[] dr1 = hazeLive.Select(filter);
                        //     if (dr1.Length > 0)
                        //     {
                        //         if (Convert.ToInt32(dr1[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr1[0]["NoneDays17"]) >= 2)
                        //             RTHaze = "有霾";
                        //         else
                        //             RTHaze = "无霾";
                        //         sb.Append("<td class='tableRowPerson'>" + RTHaze + "</td>");
                        //     }
                        //     else
                        //         sb.Append("<td class='tableRowPerson'></td>");
                        // }


                        //sb.Append("<td class='tableRowPerson'></td>");
                        //sb.Append("<td class='tableRowPerson'></td>");
                        filter = "LST='" + time + "'";
                        DataRow[] dr = hazeLive.Select(filter);
                        if (dr.Length > 0)
                        {
                            if (Convert.ToInt32(dr[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr[0]["NoneDays17"]) >= 2)
                                RTHaze = "有霾";
                            else
                                RTHaze = "无霾";

                            sb.Append("<td class='tableRowPerson'><div class='cell'>" + RTHaze + "</div></td>");
                        }
                        else
                            sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                        filter = "LST='" + time.AddDays(1) + "'";
                        DataRow[] dr1 = hazeLive.Select(filter);
                        if (dr1.Length > 0)
                        {
                            if (Convert.ToInt32(dr1[0]["NoneDays05"]) >= 2 || Convert.ToInt32(dr1[0]["NoneDays17"]) >= 2)
                                RTHaze = "有霾";
                            else
                                RTHaze = "无霾";
                            sb.Append("<td class='tableRowPerson'><div class='cell'>" + RTHaze + "</div></td>");
                        }
                        else
                            sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");


                        #endregion
                        //紫外线
                        #region  紫外线
                        if (UVData.Rows.Count > 0)
                        {
                            filter = "LST>='" + time.ToString("yyyy-MM-dd 00:00:00") + "' and LST<='" + time.ToString("yyyy-MM-dd 23:59:59") + "'";
                            drs = UVData.Select(filter);
                            strSQL = string.Format("SELECT  avg(UVS_AB) FROM dbo.tbUVS where DateTime>'{0} 10:00:00' and DateTime<='{0} 14:00:00' and StationID='10000'", time.ToString("yyyy-MM-dd"));
                            string UVRT = UVdb.GetFirstValue(strSQL);
                            //紫外线实况
                            if (drs.Length > 0 && UVRT != "")
                            {
                                sb.Append("<td class='tableRowPerson'><div class='cell'>" + Math.Round(Convert.ToDouble(UVRT)).ToString() + "</div></td>");
                                sb.Append("<td class='tableRowPerson'><div class='cell'>" + ReturnUVGrade(Convert.ToDouble(UVRT)) + "</div></td>");
                                //sb.Append("<td class='tableRowPerson'>" + drs[0]["UVAB"] + "</td>");
                                //sb.Append("<td class='tableRowPerson'>" + ReturnUVGrade(Convert.ToDouble(drs[0]["UVAB"])) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                                sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                            ////紫外线预报
                            //drs = UVData.Select(filter, "hour desc");
                            //if (drs.Length > 0)
                            //{
                            //    sb.Append("<td class='tableRowPerson'>" + returnUVIndex(drs[0]["Index"].ToString()) + "</td>");
                            //    sb.Append("<td class='tableRowPerson'>" + returnUVIndex(drs[1]["Index"].ToString()) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td class='tableRowPerson'></td>");
                            //    sb.Append("<td class='tableRowPerson'></td>");
                            //}
                        }
                        else
                        {
                            for (int iii = 0; iii < 2; iii++)
                            {
                                sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                        }
                        #endregion
                        //#region 环保局
                        ////环保局
                        //if (EPAFore.Rows.Count > 0)
                        //{
                        //    filter = "time_point='" + time + "'";
                        //    drs = EPAFore.Select(filter);
                        //    if (drs.Length > 0)
                        //    {
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_PM25"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_Ozone1"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_Ozone8"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_CO"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_PM10"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_SO2"] + "</td>");
                        //        sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI_NO2"] + "</td>");
                        //    }
                        //    else
                        //    {
                        //        for (int iii = 0; iii < 7; iii++)
                        //        {
                        //            sb.Append("<td class='tableRowPerson'></td>");
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    for (int iii = 0; iii < 7; iii++)
                        //    {
                        //        sb.Append("<td class='tableRowPerson'></td>");
                        //    }
                        //}
                        //#endregion 环保局结束
                        //#region  24小时
                        ////24小时
                        //int[] item = { 1, 4, 5, 2, 3 };
                        //if (Fore_24h.Rows.Count > 0)
                        //{

                        //    for (int j = 0; j < 5; j++)
                        //    {
                        //        filter = "ForecastDate='" + time + "' and ITEMID=" + item[j];
                        //        drs = Fore_24h.Select(filter);
                        //        if (drs.Length > 0)
                        //            sb.Append("<td class='tableRowPerson'>" + drs[0]["Value"] + "</td>");
                        //        else
                        //            sb.Append("<td class='tableRowPerson'></td>");
                        //    }
                        //    for (int k = 0; k < 5; k++)
                        //    {
                        //        filter = "ForecastDate='" + time + "' and ITEMID=" + item[k];
                        //        drs = Fore_24h.Select(filter);
                        //        if (drs.Length > 0)
                        //            sb.Append("<td class='tableRowPerson'>" + drs[0]["AQI"] + "</td>");
                        //        else
                        //            sb.Append("<td class='tableRowPerson'></td>");
                        //    }
                        //}
                        //else
                        //{
                        //    for (int iii = 0; iii < 10; iii++)
                        //    {
                        //        sb.Append("<td class='tableRowPerson'></td>");
                        //    }
                        //}
                        //#endregion 24小时结束
                        //Get48Hour(Fore_48h, sb, time);
                        //#region  分时段浓度
                        ////分时段浓度
                        //if (durFore.Rows.Count > 0)
                        //{
                        //    int[] duration = { 4, 1, 2, 3 };
                        //    int[] itemID = { 1, 4, 5, 2, 3 };
                        //    for (int jjj = 0; jjj < duration.Length; jjj++)
                        //    {
                        //        for (int kkk = 0; kkk < itemID.Length; kkk++)
                        //        {
                        //            if ((duration[jjj] == 4 || duration[jjj] == 1) && itemID[kkk] == 4)
                        //                continue;
                        //            filter = "ForecastDate='" + time + "' and durationID=" + duration[jjj] + " and ITEMID=" + itemID[kkk];
                        //            drs = durFore.Select(filter);
                        //            if (drs.Length > 0)
                        //                sb.Append("<td class='tableRowPerson'>" + drs[0]["Value"] + "</td>");
                        //            else
                        //                sb.Append("<td class='tableRowPerson'></td>");
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    for (int iii = 0; iii < 18; iii++)
                        //    {
                        //        sb.Append("<td class='tableRowPerson'></td>");
                        //    }
                        //}
                        //#endregion 分时段浓度
                        //GetPeriodENV(durForeEnv, sb, time);
                        int[] item = { 1, 4, 5, 2, 3 };//将上面的拿过来zhz
                        #region 浓度实况
                        //浓度实况
                        if (itemLive.Rows.Count > 0)
                        {
                            int[] duration = { 6, 2, 3 };
                            for (int l = 0; l < duration.Length; l++)
                            {
                                for (int m = 0; m < item.Length; m++)
                                {
                                    if (duration[l] == 6 && item[m] == 4)
                                        continue;
                                    filter = "LST='" + time.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") + "'and DurationID='" + duration[l] + "' and ITEMID='" + item[m] + "'";
                                    drs = itemLive.Select(filter);
                                    if (drs.Length > 0)
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + drs[0]["Value"] + "</div></td>");
                                    else
                                        sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                                }
                            }
                        }
                        else
                        {
                            for (int iii = 0; iii < 14; iii++)
                            {
                                sb.Append("<td class='tableRowPerson'></td>");
                            }
                        }
                        #endregion 浓度实况结束
                        #region 国家局
                        //国家局
                        string[] itemId = { "1", "4", "5", "2", "3" };//"1","4","5","2","3"
                        if (CNFore.Rows.Count > 0)
                        {
                                string lst = time.ToString("yyyy-MM-dd");

                                for (int i = 0; i < itemId.Length; i++)
                                {
                                    filter = "ITEMID='" + itemId[i] + "' and lst='" + lst + "'";
                                    drs = CNFore.Select(filter);
                                    if (drs.Length > 0)
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + drs[0]["aqi"] + "</div></td>");
                                    else
                                        sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                                }

                            

                            //int days = DateTime.DaysInMonth(DateTime.Parse(dateMonth).Year, DateTime.Parse(dateMonth).Month);
                            //for (int day = 1; day <= days; day++)
                            //{
                            //    string lst = DateTime.Parse(dateMonth + "-" + day).ToString("yyyy-MM-dd");
                               
                            //    for (int i = 0; i < itemId.Length; i++)
                            //    {
                            //         filter = "ITEMID='" + itemId[i] + "' and lst='" + lst + "'";
                            //         drs = CNFore.Select(filter);
                            //         if (drs.Length > 0)
                            //             sb.Append("<td class='tableRowPerson'>" + drs[0]["aqi"] + "</td>");
                            //         else
                            //             sb.Append("<td class='tableRowPerson'></td>");
                            //    }
                               
                            //}



                            //filter = "LST='" + time + "'";
                            //drs = CNFore.Select(filter);
                            ////double[] con = { Convert.ToDouble(drs[0]["PM25"]), 0, Convert.ToDouble(drs[0]["O3"]), Convert.ToDouble(drs[0]["PM10"]), Convert.ToDouble(drs[0]["NO2"]) };
                            ////int ii = con.ToList().IndexOf(con.Max());
                            //string[] CNAQI = { "", "", "", "", "" };
                            //if (drs.Length > 0)
                            //{
                            //    switch (drs[0]["Parameter"].ToString())
                            //    {
                            //        case "PM2.5":
                            //            CNAQI[0] = drs[0]["AQI"].ToString();
                            //            break;
                            //        case "O3":
                            //            CNAQI[2] = drs[0]["AQI"].ToString();
                            //            break;
                            //        case "PM10":
                            //            CNAQI[3] = drs[0]["AQI"].ToString();
                            //            break;
                            //        case "NO2":
                            //            CNAQI[4] = drs[0]["AQI"].ToString();
                            //            break;
                            //        case "":
                            //            int PM25AQI = ToAQI(drs[0]["PM25"].ToString(), "1");
                            //            int PM10AQI = ToAQI(drs[0]["PM10"].ToString(), "2");
                            //            int NO2AQI = ToAQI(drs[0]["NO2"].ToString(), "3");
                            //            int O3AQI = ToAQI(drs[0]["O3"].ToString(), "5");
                            //            int[] ItemAQI = { PM25AQI, 0, O3AQI, PM10AQI, NO2AQI };
                            //            int index = 0;
                            //            for (int ii = 0; ii < 5; ii++)
                            //            {
                            //                if (ItemAQI[ii] == ItemAQI.Max())
                            //                {
                            //                    index = ii;
                            //                }
                            //            }
                            //            CNAQI[index] = drs[0]["AQI"].ToString();
                            //            break;
                            //    }
                            //}
                            //for (int n = 0; n < CNAQI.Length; n++)
                            //{
                            //    sb.Append("<td class='tableRowPerson'>" + CNAQI[n] + "</td>");
                            //}
                        }
                        else
                        {
                            for (int iii = 0; iii < 5; iii++)
                            {
                                sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                        }
                        #endregion 国家局结束
                        #region 20-20平均实况浓度
                        //20-20平均实况浓度
                        if (itemLive.Rows.Count > 0)
                        {
                            int[] duration = { 6, 2, 3 };
                            for (int o = 0; o < item.Length; o++)
                            {
                                if (item[o] == 4)
                                    continue;
                                filter = "LST='" + time.ToString("yyyy-MM-dd 00:00:00") + "'and DurationID in ('6','2','3') and ITEMID='" + item[o] + "'";
                                drs = itemLive.Select(filter, "DurationID");

                                if (drs.Length == 3)
                                {
                                    double[] con = { Convert.ToDouble(drs[2]["Value"]), Convert.ToDouble(drs[0]["Value"]), Convert.ToDouble(drs[1]["Value"]) };

                                    if (item[o] != 5)
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + Math.Round((con[0] * 10 + con[1] * 6 + con[2] * 8) / 24, 3) + "</div></td>");
                                    else
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + Math.Round(con.Max()) + "</div></td>");
                                }
                                else
                                    sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                            for (int p = 0; p < item.Length; p++)
                            {
                                if (item[p] == 4)
                                    continue;
                                filter = "LST='" + time.ToString("yyyy-MM-dd 00:00:00") + "'and DurationID in ('6','2','3') and ITEMID='" + item[p] + "'";
                                drs = itemLive.Select(filter, "DurationID");


                                if (drs.Length == 3)
                                {
                                    double[] con = { Convert.ToDouble(drs[2]["Value"]), Convert.ToDouble(drs[0]["Value"]), Convert.ToDouble(drs[1]["Value"]) };
                                    if (item[p] != 5)
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + ReturnAQI((con[0] * 10 + con[1] * 6 + con[2] * 8) / 24, item[p]) + "</div></td>");
                                    else
                                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + ReturnAQI(con.Max(), item[p]) + "</div></td>");
                                }
                                else
                                    sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                        }
                        else
                        {
                            for (int iii = 0; iii < 8; iii++)
                            {
                                sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                            }
                        }
                        #endregion
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    return sb.ToString();
                }
                else
                {
                    return "加载失败，原因为：缺少班次数据";
                }
            }
            catch (Exception ex)
            {
                return "加载失败，原因为：" + ex.ToString();
            }

        }

        private static void GetPeriodENV(DataTable durFore, StringBuilder sb, DateTime time)
        {
            if (durFore.Rows.Count > 0)
            {
                int[] duration = { 4, 1, 2, 3 };
                int[] itemID = { 1, 4, 5, 2, 3 };
                for (int jjj = 0; jjj < duration.Length; jjj++)
                {
                    for (int kkk = 0; kkk < itemID.Length; kkk++)
                    {
                        if ((duration[jjj] == 4 || duration[jjj] == 1) && itemID[kkk] == 4)
                            continue;
                        string filter = "ForecastDate='" + time + "' and durationID=" + duration[jjj] + " and ITEMID=" + itemID[kkk];
                        DataRow[] drs = durFore.Select(filter);
                        if (drs.Length > 0)
                            sb.Append("<td class='tableRowPerson'><div class='cell'>" + drs[0]["Value"] + "</div></td>");
                        else
                            sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                    }
                }
            }
            else
            {
                for (int iii = 0; iii < 18; iii++)
                {
                    sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                }
            }
        }

        private static void Get48Hour(DataTable Fore_48h, StringBuilder sb, DateTime time)
        {
            int[] item = { 1, 4, 5, 2, 3 };
            if (Fore_48h.Rows.Count > 0)
            {
                string filter = "";
                DataRow[] drs = null;
                for (int j = 0; j < 5; j++)
                {
                    filter = "ForecastDate='" + time + "' and ITEMID=" + item[j];
                    drs = Fore_48h.Select(filter);
                    if (drs != null && drs.Length > 0)
                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + drs[0]["Value"] + "</div></td>");
                    else
                        sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                }
                for (int k = 0; k < 5; k++)
                {
                    filter = "ForecastDate='" + time + "' and ITEMID=" + item[k];
                    drs = Fore_48h.Select(filter);
                    if (drs != null && drs.Length > 0)
                        sb.Append("<td class='tableRowPerson'><div class='cell'>" + drs[0]["AQI"] + "</div></td>");
                    else
                        sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                }
            }
            else
            {
                for (int iii = 0; iii < 10; iii++)
                {
                    sb.Append("<td class='tableRowPerson'><div class='cell'></div></td>");
                }
            }
        }

        /// <summary>
        /// 返回AQI值
        /// </summary>
        /// <param name="con"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int ReturnAQI(double con, int itemID)
        {
            int aqi = 0;
            if (itemID == 1)
                aqi = ReturnPM25AQI(con);
            else if (itemID == 2)
                aqi = ReturnPM10AQI(con);
            else if (itemID == 3)
                aqi = ReturnNO2AQI(con);
            else if (itemID == 5)
                aqi = ReturnO38AQI(con);
            return aqi;
        }

        /// <summary>
        /// 返回PM2.5AQI
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public int ReturnPM25AQI(double con)
        {
            int AQI = 0;
            if (con <= 35)
                AQI = Convert.ToInt32(con * 50 / 35);
            else if (con > 35 && con <= 75)
                AQI = Convert.ToInt32((con - 35) * 50 / 40 + 50);
            else if (con > 75 && con <= 115)
                AQI = Convert.ToInt32((con - 75) * 50 / 40 + 100);
            else if (con > 115 && con <= 150)
                AQI = Convert.ToInt32((con - 115) * 50 / 35 + 150);
            else if (con > 150 && con <= 350)
                AQI = Convert.ToInt32((con - 150) + 200);
            else if (con > 350 && con <= 500)
                AQI = Convert.ToInt32((con - 350) * 100 / 150 + 400);
            else
                AQI = 500;
            return AQI;
        }

        /// <summary>
        /// 返回O38AQI
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public int ReturnO38AQI(double con)
        {
            int AQI = 0;
            if (con <= 100)
                AQI = Convert.ToInt32(con * 50 / 100);
            else if (con > 100 && con <= 160)
                AQI = Convert.ToInt32((con - 100) * 50 / 60 + 50);
            else if (con > 75 && con <= 115)
                AQI = Convert.ToInt32((con - 160) * 50 / 55 + 100);
            else if (con > 115 && con <= 150)
                AQI = Convert.ToInt32((con - 215) * 50 / 50 + 150);
            else if (con > 150 && con <= 350)
                AQI = Convert.ToInt32((con - 265) * 100 / 535 + 200);
            else
                AQI = 500;
            return AQI;
        }

        /// <summary>
        /// 返回PM10AQI
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public int ReturnPM10AQI(double con)
        {
            int AQI = 0;
            if (con <= 50)
                AQI = Convert.ToInt32(con * 50 / 50);
            else if (con > 50 && con <= 150)
                AQI = Convert.ToInt32((con - 50) * 50 / 100 + 50);
            else if (con > 150 && con <= 250)
                AQI = Convert.ToInt32((con - 150) * 50 / 100 + 100);
            else if (con > 250 && con <= 350)
                AQI = Convert.ToInt32((con - 250) * 50 / 100 + 150);
            else if (con > 350 && con <= 420)
                AQI = Convert.ToInt32((con - 350) * 100 / 70 + 200);
            else if (con > 420 && con <= 500)
                AQI = Convert.ToInt32((con - 420) * 100 / 80 + 300);
            else if (con > 500 && con <= 600)
                AQI = Convert.ToInt32((con - 500) * 100 / 100 + 400);
            else
                AQI = 500;
            return AQI;
        }

        /// <summary>
        /// 返回NO2AQI
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public int ReturnNO2AQI(double con)
        {
            int AQI = 0;
            if (con <= 40)
                AQI = Convert.ToInt32(con * 50 / 40);
            else if (con > 40 && con <= 80)
                AQI = Convert.ToInt32((con - 40) * 50 / 40 + 50);
            else if (con > 80 && con <= 180)
                AQI = Convert.ToInt32((con - 80) * 50 / 100 + 100);
            else if (con > 180 && con <= 280)
                AQI = Convert.ToInt32((con - 180) * 50 / 100 + 150);
            else if (con > 280 && con <= 565)
                AQI = Convert.ToInt32((con - 280) * 100 / 285 + 200);
            else if (con > 565 && con <= 750)
                AQI = Convert.ToInt32((con - 565) * 100 / 185 + 300);
            else if (con > 750 && con <= 940)
                AQI = Convert.ToInt32((con - 750) * 100 / 190 + 400);
            else
                AQI = 500;
            return AQI;
        }

        /// <summary>
        /// 返回紫外线等级
        /// </summary>
        /// <param name="UV"></param>
        /// <returns></returns>
        public string ReturnUVGrade(double UV)
        {
            string Grade = "0";
            if (UV < 5)
                Grade = "1";
            else if (UV >= 5 && UV < 10)
                Grade = "2";
            else if (UV >= 10 && UV < 15)
                Grade = "3";
            else if (UV >= 15 && UV < 30)
                Grade = "4";
            else if (UV >= 30)
                Grade = "5";
            return Grade;
        }

        /// <summary>
        /// 返回紫外线系数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int returnUVIndex(string index)
        {
            int grade = 0;
            double IntValue = double.Parse(index);
            if (IntValue < 3)
                grade = 1;
            else if (IntValue < 5)
                grade = 2;
            else if (IntValue < 7)
                grade = 3;
            else if (IntValue < 10)
                grade = 4;
            else
                grade = 5;
            return grade;

        }

        /// <summary>
        /// 名称对应工号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ToUserCode(string name)
        {
            string num = string.Empty;
            switch (name)
            {
                case "毛卓成":
                    num = "159";
                    break;
                case "陈镭":
                    num = "160";
                    break;
                case "曹钰":
                    num = "188";
                    break;
                case "瞿元昊":
                    num = "190";
                    break;
                case "余钟奇":
                    num = "192";
                    break;
                case "周婉君":
                    num = "199";
                    break;
            }
            return num;
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
                    haze = "无霾";
                    break;
                case "2":
                    haze = "轻微霾";
                    break;
                case "3":
                    haze = "轻度霾";
                    break;
                case "4":
                    haze = "中度霾";
                    break;
                case "5":
                    haze = "重度霾";
                    break;
                case "6":
                    haze = "严重霾";
                    break;
            }
            return haze;
        }

        /// <summary>
        /// 返回国家局评分表格
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ReturnChinaScore(string dateTime)
        {
            string fromTime = DateTime.Parse(dateTime).ToString("yyyy-MM-01 00:00:00");
            int totalDays = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).Day;
            string toTime = DateTime.Parse(dateTime).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            string[] modle = { "Manual" };
            string[] modleName = { "主观预报" };
            string strSQL = string.Format("SELECT  LST,Module, f1, f2, f3, S from  T_NewChinaEvaluation  Where LST between '{0}' and '{1}' and Module<>'RT' order by LST;", fromTime, toTime);
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
            if (dt.Rows.Count > 0)
            {
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
            }

            sb.Append("</table>");
            return sb.ToString();
        }
        /// <summary>
        /// 计算国家局评分 2018-03-30 by 孙明宇
        /// </summary>
        /// <param name="LST"></param>
        public void ChinaProcess(DateTime LST)
        {
            LST = Convert.ToDateTime(LST.ToString("yyyy-MM-01"));
            try
            {
                strSQL = string.Format("select * from T_New24hChinaIAQI where LST between '{0}' and '{1}'", LST.ToString("yyyy-MM-01"), LST.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                DataTable CNData = m_Database.GetDataTable(strSQL);
                strSQL = string.Format("select PM25_IAQI,O31_IAQI,O38_IAQI,PM10_IAQI,NO2_IAQI from T_New24h WHERE LST between '{0}' and '{1}'", LST.ToString("yyyy-MM-01"), LST.AddMonths(1).AddDays(-2).ToString("yyyy-MM-dd 23:59:59"));
                DataTable foreData = m_Database.GetDataTable(strSQL);
                strSQL = "insert into T_NewChinaEvaluation values";

                if (CNData.Rows.Count > 0)
                {
                    for (int i = 0; i < CNData.Rows.Count; i++)
                    {
                        DateTime time = Convert.ToDateTime(CNData.Rows[i][0]);
                        int AQI = 0;
                        string parameter = null;
                        for (int j = 2; j < CNData.Columns.Count; j++)
                        {
                            if (CNData.Rows[i][j].ToString() != "")
                            {
                                AQI = Convert.ToInt32(CNData.Rows[i][j]);
                                parameter = ItemName(CNData.Columns[j].ColumnName.Replace("_IAQI", ""));
                            }
                        }
                        string jobNum = CNData.Rows[i][1].ToString();
                        string module = "Manual";
                        string quality = AQIGrade(AQI)[1];
                        int foreAQI = 0;
                        string foreParameter = null;
                        for (int k = 0; k < foreData.Columns.Count; k++)
                        {
                            if (Convert.ToInt32(foreData.Rows[i][k]) > foreAQI)
                            {
                                foreAQI = Convert.ToInt32(foreData.Rows[i][k]);
                                foreParameter = ItemName(foreData.Columns[k].ColumnName.Replace("_IAQI", ""));
                            }
                        }
                        //计算评分
                        int f1 = parameter == foreParameter ? 100 : 0;
                        int f2 = 0;
                        if (AQIGrade(AQI)[0] == AQIGrade(foreAQI)[0])
                            f2 = 100;
                        else if (Math.Abs(Convert.ToInt32(AQIGrade(AQI)[0]) - Convert.ToInt32(AQIGrade(foreAQI)[0])) == 1)
                            f2 = 50;
                        else if (Math.Abs(Convert.ToInt32(AQIGrade(AQI)[0]) - Convert.ToInt32(AQIGrade(foreAQI)[0])) == 2)
                            f2 = 25;
                        else
                            f2 = 0;
                        int f3 = 0;
                        if (Math.Abs(AQI - foreAQI) >= 0 && Math.Abs(AQI - foreAQI) <= 25)
                            f3 = 100;
                        else if (Math.Abs(AQI - foreAQI) >= 26 && Math.Abs(AQI - foreAQI) <= 50)
                            f3 = 80;
                        else if (Math.Abs(AQI - foreAQI) >= 51 && Math.Abs(AQI - foreAQI) <= 100)
                            f3 = 60;
                        else if (Math.Abs(AQI - foreAQI) >= 101 && Math.Abs(AQI - foreAQI) <= 150)
                            f3 = 30;
                        else if (Math.Abs(AQI - foreAQI) >= 151 && Math.Abs(AQI - foreAQI) <= 500)
                            f3 = 0;
                        int s = Convert.ToInt32(0.2 * f1 + 0.5 * f2 + 0.3 * f3);
                        strSQL += string.Format("('{0}',{1},'{2}','{3}','{4}',{5},{6},{7},{8},'{9}'),", time.ToString("yyyy-MM-dd"), AQI, module, quality, parameter, f1, f2, f3, s, jobNum);
                    }
                    strSQL = strSQL.TrimEnd(',');
                    string delete = string.Format("delete from T_NewChinaEvaluation where LST between '{0}' and'{1}'", LST.ToString("yyyy-MM-01"), LST.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
                    m_Database.Execute(delete);
                    m_Database.Execute(strSQL);
                }

            }
            catch (Exception EX)
            { }
        }

        /// <summary>
        /// 返回AQI等级 //2018-03-27 by 孙明宇
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string[] AQIGrade(int AQI)
        {
            string grade = "";
            string quality = "";

            if (AQI <= 50) { grade = "1"; quality = "优"; }
            else if (AQI > 50 && AQI <= 100) { grade = "2"; quality = "良"; }
            else if (AQI > 100 && AQI <= 150) { grade = "3"; quality = "轻度污染"; }
            else if (AQI > 150 && AQI <= 200) { grade = "4"; quality = "中度污染"; }
            else if (AQI > 200 && AQI <= 300) { grade = "5"; quality = "重度污染"; }
            else if (AQI > 300) { grade = "6"; quality = "严重污染"; }
            string[] AQIGrade = { grade, quality };
            return AQIGrade;
        }

        /// <summary>
        /// 返回污染物名
        /// </summary>
        /// <param name="oriName"></param>
        /// <returns></returns>
        public string ItemName(string oriName)
        {
            string itemName = string.Empty;
            switch (oriName)
            {
                case "PM25":
                    itemName = "PM2.5";
                    break;
                case "PM10":
                    itemName = "PM10";
                    break;
                case "O31":
                    itemName = "O3";
                    break;
                case "O38":
                    itemName = "O3";
                    break;
                case "NO2":
                    itemName = "NO2";
                    break;
                case "SO2":
                    itemName = "SO2";
                    break;
                case "CO":
                    itemName = "CO";
                    break;
            }
            return itemName;
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
    }
}
