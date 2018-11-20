using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

using AQIQuery.aQuery;

namespace MMShareBLL.DAL
{
   public  class WeekReport
    {
        private Database m_Database;
        DataSetForcast.WeekReportDTDataTable weekDT = new DataSetForcast.WeekReportDTDataTable();
        DataSetForcast.CollectionRateDataTable crDT = new DataSetForcast.CollectionRateDataTable();
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public WeekReport()
        {
            m_Database = new Database();
        }

        /// <summary>
        /// 返回json字符
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="fromDateEnd"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public string WeekReportData(string fromDate,string fromDateEnd,string site)
        {
            string beginTime = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = DateTime.Parse(fromDateEnd).ToString("yyyy-MM-dd")+" 23:59:59";
            string siteID = "";
            if (site == "QP")
                siteID = "271";

            if (site == "CM")
                siteID = "251";

            if (site == "NJ")
                siteID = "272";

            if (site == "PD")
                siteID = "228";

            int[] parameterArray = { 53, 54, 51, 55, 52, 47, 48, 5, 56, 57, 31, 32, 33, 34, 35, 36, 37 };

            string sql = " Select   ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value  from SEMC_DMS.DBO.Data  " +
                           " where SiteID={0} "+
                           " and ParameterID in (53,54,51,55,52,47,48,5,56,57,31,32,33,34,35,36,37) and  "+
                           " (CONVERT(VARCHAR(19),LST,120) between '{1}' and '{2}') "+
                           "  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 and DurationID=10 "+
                           "  group by ParameterID,CONVERT(VARCHAR(10),LST,111) " +
                           "  order by ParameterID,LST desc ";

            string strSQL = string.Format(sql, siteID, beginTime,endTime);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");
            string filter = "";
            DateTime beingDate = DateTime.Parse(beginTime);
            DateTime endDate = DateTime.Parse(endTime);
            for (DateTime dtime = beingDate; dtime <= endDate; dtime = dtime.AddDays(1))
            {
                for (int i = 1; i <= parameterArray.Length; i++) {
                    string para= parameterArray[i-1].ToString();
                    filter = string.Format("ParameterID = '{0}' AND LST = '{1}'", para, dtime.ToString("yyyy/MM/dd"));
                    if (dtSiteData != null) {
                        DataRow[] rows=dtSiteData.Select(filter);
                        if (rows != null && rows.Length > 0)
                        {
                            if (para == "31" ||
                               para == "32" ||
                               para == "33" ||
                               para == "34" ||
                               para == "35" ||
                               para == "36" ||
                               para == "37")
                                sb.Append(string.Format("{0}{1}:'{2}',", (i), dtime.ToString("MMdd"),
                                           Math.Round(double.Parse(rows[0]["value"].ToString()), 0).ToString()));
                            else
                                sb.Append(string.Format("{0}{1}:'{2}',", (i), dtime.ToString("MMdd"),
                                           Math.Round(double.Parse(rows[0]["value"].ToString()), 3).ToString()));
                   
                        }
                        else {
                            sb.Append(string.Format("{0}{1}:'{2}',", (i), dtime.ToString("MMdd"), "0"));
                        }
                    }
                }
            }

            //去掉多余的“,”
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            
            if (sb.Length > 1)
                sb.Append("}");
            else
                sb.Length = 0;

            return sb.ToString();
        }


       /// <summary>
       /// 返回DataTable对象
       /// </summary>
       /// <param name="fromDate"></param>
       /// <param name="fromDateEnd"></param>
       /// <param name="site"></param>
       /// <returns></returns>
        public  DataTable WeekReportDataTable(string fromDate, string fromDateEnd, string site)
        {
            string beginTime = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = DateTime.Parse(fromDateEnd).ToString("yyyy-MM-dd") + " 23:59:59";
            string siteID = "";
            if (site == "QP")
                siteID = "271";

            if (site == "CM")
                siteID = "251";

            if (site == "NJ")
                siteID = "272";

            if (site == "PD")
                siteID = "228";

            int[] parameterArray = { 53, 54, 51, 55, 52, 47, 48, 5, 56, 57, 31, 32, 33, 34, 35, 36, 37 };

            string sql = " Select   ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value  from SEMC_DMS.DBO.Data  " +
                           " where SiteID={0} " +
                           " and ParameterID in (53,54,51,55,52,47,48,5,56,57,31,32,33,34,35,36,37) and  " +
                           " (CONVERT(VARCHAR(19),LST,120) between '{1}' and '{2}') " +
                           "  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 and DurationID=10 " +
                           "  group by ParameterID,CONVERT(VARCHAR(10),LST,111) " +
                           "  order by ParameterID,LST desc ";

            string strSQL = string.Format(sql, siteID, beginTime, endTime);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");
            string filter = "";
            DateTime beingDate = DateTime.Parse(beginTime);
            DateTime endDate = DateTime.Parse(endTime);
            for (DateTime dtime = beingDate; dtime <= endDate; dtime = dtime.AddDays(1))
            {
                for (int i = 0; i < parameterArray.Length; i++)
                {
                    string para = parameterArray[i].ToString();
                    filter = string.Format("ParameterID = '{0}' AND LST = '{1}'", para, dtime.ToString("yyyy/MM/dd"));
                    if (dtSiteData != null)
                    {
                        DataRow[] rows = dtSiteData.Select(filter);
                        string value = "0"; 
                        DataRow row = weekDT.NewRow();
                        row["DateT"] = dtime.ToString("MMdd");
                        row["Para"] = para;

                        if (rows != null && rows.Length > 0)
                        {
                            if (para == "31" ||
                                para == "32" ||
                                para == "33" ||
                                para == "34" ||
                                para == "35" ||
                                para == "36" ||
                                para == "37")
                                value = Math.Round(double.Parse(rows[0]["value"].ToString()), 0).ToString();
                            else
                                value = Math.Round(double.Parse(rows[0]["value"].ToString()), 3).ToString();
                        }

                        row["Value"] = value;
                        weekDT.Rows.Add(row);
                    }
                }
            }

            return weekDT;
        }


       /// <summary>
       /// 数据采集率
       /// </summary>
       /// <param name="fromDate"></param>
       /// <param name="fromDateEnd"></param>
       /// <param name="site"></param>
       /// <returns></returns>
        public DataTable CollectionRate(string fromDate, string fromDateEnd, string site) {

            string beginTime = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = DateTime.Parse(fromDateEnd).ToString("yyyy-MM-dd") + " 23:59:59";
            string siteID = "";
            if (site == "QP")
                siteID = "271";

            if (site == "CM")
                siteID = "251";

            if (site == "NJ")
                siteID = "272";

            if (site == "PD")
                siteID = "228";

            int[] parameterArray = { 53, 54, 51, 55, 52, 47, 48, 5, 56, 57, 31, 32, 33, 34, 35, 36, 37 };
            int dayCount = 0;
            for (DateTime dtime = DateTime.Parse(beginTime); 
                   dtime <= DateTime.Parse(endTime); 
                   dtime = dtime.AddDays(1))
                dayCount++;

            TimeSpan ts = DateTime.Parse(endTime) - DateTime.Parse(beginTime);
            string sql = " Select   ParameterID,"+
                         "   count(ParameterID)*100/(" + dayCount + "*24) as 'CollRate' from SEMC_DMS.DBO.Data  " +
                         "           where SiteID={0} "+
                         "           and ParameterID in (53,54,51,55,52,47,48,5,56,57,31,32,33,34,35,36,37) and  "+
                         "           (CONVERT(VARCHAR(19),LST,120) between '{1}' and '{2}') "+
                         "            and DurationID=10 "+
                         "            group by ParameterID   ";

            string strSQL = string.Format(sql, siteID, beginTime, endTime);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            return dtSiteData;
        }


        public string CollectionRateString(string fromDate, string fromDateEnd, string site)
        {
            StringBuilder strBuilder = new StringBuilder();
            string beginTime = DateTime.Parse(fromDate).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = DateTime.Parse(fromDateEnd).ToString("yyyy-MM-dd") + " 23:59:59";
            string siteID = "";
            if (site == "QP")
                siteID = "271";

            if (site == "CM")
                siteID = "251";

            if (site == "NJ")
                siteID = "272";

            if (site == "PD")
                siteID = "228";

            int[] parameterArray = { 53, 54, 51, 55, 52, 47, 48, 5, 56, 57, 31, 32, 33, 34, 35, 36, 37 };
            int dayCount = 0;
            for (DateTime dtime = DateTime.Parse(beginTime);
                   dtime <= DateTime.Parse(endTime);
                   dtime = dtime.AddDays(1))
                dayCount++;

            string sql = " Select   ParameterID," +
                         "  count(ParameterID)*100/(" + dayCount + "*24) as 'CollRate' from SEMC_DMS.DBO.Data  " +
                         "           where SiteID={0} " +
                         "           and ParameterID in (53,54,51,55,52,47,48,5,56,57,31,32,33,34,35,36,37) and  " +
                         "           (CONVERT(VARCHAR(19),LST,120) between '{1}' and '{2}') " +
                         "            and DurationID=10 " +
                         "            group by ParameterID   ";

            string strSQL = string.Format(sql, siteID, beginTime, endTime);
            DataTable dtSiteData = m_Database.GetDataTable(strSQL);
            foreach (DataRow row in dtSiteData.Rows) {
                strBuilder .Append(row["ParameterID"].ToString() +":" +
                              row["CollRate"].ToString()+";");
            }
            return strBuilder.ToString();
        }

    }
}
