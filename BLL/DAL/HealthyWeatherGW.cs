
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    public class HealthyWeatherGW
    {
        public string GetChartElements(string fromDate, string toDate, string eName, string type, string period, string duration)
        {
            string strReturn = "", strReturns = "";
            Database m_Databases = new Database("DBCONFIGGW");
            string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd 00:00:00");
            string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd 23:59:59");
            if ((int)(DateTime.Now - DateTime.Parse(toDate)).TotalDays == 0) {
                to = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string field = "DustMassCon";
            string tableName = "tbDustS";
            string valueScope = "DustMassCon<=500";
            string sites = "";
            string[] SitesArray = { "58367", "58362", "58460", "58366", "58370",
                                 "58363", "99114", "99116", "99115", "99119", "99118", "99110", "99989" ,"58365","58463"};
            foreach (string str in SitesArray)
            {
                sites += str + " ";
            }
            #region
            if (eName == "PM25")
            {
                valueScope += "and DeviceType = 2";
            }
            else if (eName == "PM10")
            {
                valueScope += "and DeviceType = 3";
            }
            else if (eName == "PM1")
            {
                valueScope += "and DeviceType = 4";
            }
            else if (eName == "O3")
            {
                field = "o3";
                tableName = "tbO3";
                valueScope = "o3>=0 and o3<=250 ";
            }
            else if (eName == "NO2") {
                field = "NO2";
                tableName = "tbNOx";
                valueScope = " NO2>=0 and NO2<=300";
            }
            #endregion
            string sql = "EXEC dbo.[EleChart] @startTime = '" + from + "'," +
                  "@sites = '" + sites + "', " +
                  " @endTime = '" + to + "'," +
                  "@table = '" + tableName + "'," +
                 " @field = '" + field + "'," +
                  "@valueScope = '" + valueScope + "'";
            DataSet ds = m_Databases.GetDataset(sql);
            DataTable dtRes = new DataTable();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                string site = SitesArray[i];
                dtRes = ds.Tables[i];
                if (site == "58363" && eName == "PM25")
                {  //东滩pm25取另一张表
                    dtRes = GettbDust(from, to);
                }
                string x = "", y = "";
                foreach (DataRow row in dtRes.Rows)
                {
                    x = x + "|" + row[0].ToString();
                    y = y + "|" + (row[1].ToString() == "" ? "null" : row[1].ToString());
                }
                strReturn = strReturn + ",'0':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                strReturns += "{" + strReturn.TrimStart(',') + "}&";
            }
            strReturns = strReturns.TrimEnd('&');
            return strReturns;
        }

        public DataTable GettbDust(string form,string to) {
            Database m_Databases = new Database("DBCONFIGGW");
            string sql = "SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(16),[DateTIME], 120)) AS [END],  PM2_5 as 'value',[DateTime],stationID as 'Site' from tbDust "+
                  "where(DateTime between '"+form+"' and '"+to+"') AND PM2_5<= 500"+
                  "AND stationID ='58363' ORDER BY datetime asc; ";
            DataTable dt = m_Databases.GetDataTable(sql);
            return dt;
        }
    }
}


