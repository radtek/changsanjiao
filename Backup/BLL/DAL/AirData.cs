using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Readearth.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Readearth.Data.Entity;
namespace MMShareBLL.DAL
{

    public class AirData
    {
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Database m_Database;
        private Database m_DatabaseS;
        public AirData()
        {
            m_Database = new Database();
            m_DatabaseS = new Database("SEMCDMC");
        }
        public string newDataTime(string dataTableName,string type)
        {
            string strSQL = " SELECT MAX(ForecastDate) AS ForecastDate FROM "+dataTableName+" WHERE TYPE='"+type+"'";
            string maxTime = m_Database.GetFirstValue(strSQL);
            string josnData = "{\"maxTime\":\"" + maxTime + "\"}";
            return josnData;

        }
        public string imageData(string dataTableName, string type)//case when Period=null then '000' else Period end as Period
        {
            string strSQL = "SELECT TableName FROM T_Entity WHERE EntityName='" + dataTableName + "'";
            string tableName = m_Database.GetFirstValue(strSQL);
            Entity entity = new Entity(m_Database, dataTableName);
            string strWhere = "";
            if (entity.Condition != "")
            {
                strWhere = entity.Condition;
            }
            if (type == "")
            {
                if (strWhere != "")
                    strSQL = "SELECT TOP(6)Name,CONVERT(VARCHAR(19),DATEADD(hh,CONVERT(int,(case when  Period  is NULL then '000' else Period end)), ForecastDate),120)  AS ForecastDate1,Folder,Period FROM  " + tableName + " WHERE  " + strWhere + " ORDER BY ForecastDate DESC, Period DESC";
                else
                    strSQL = "SELECT TOP(6)Name,CONVERT(VARCHAR(19),DATEADD(hh,CONVERT(int,(case when  Period  is NULL then '000' else Period end)), ForecastDate),120)  AS ForecastDate1,Folder,Period FROM  " + tableName + "  ORDER BY ForecastDate DESC, Period DESC";
            }
            else
            {
                if (strWhere != "")
                    strWhere = " AND " + strWhere;
                strSQL = "SELECT TOP(6)Name,CONVERT(VARCHAR(19),DATEADD(hh,CONVERT(int,(case when  Period  is NULL then '000' else Period end)), ForecastDate),120) AS ForecastDate1,Folder,Period FROM " + tableName + " WHERE TYPE='" + type + "' " + strWhere + "  ORDER BY ForecastDate DESC, Period DESC";
            }
            DataTable dt = m_Database.GetDataTable(strSQL);
            string josnData = JsonConvert.SerializeObject(dt, new DataTableConverter());

            return josnData;
        }
        public string tableData(string startTime, string endTime,string  cityType)
        {
            string strSQL = "";
            string josnData = "";
            if (cityType == "0")//全国、长三角、华东
            {
                strSQL = string.Format("SELECT CONVERT(VARCHAR(19),a.timepoint,120) AS timepoint,A.area, value, aqi FROM (select * from  SEMC_DMC.DBO.CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint BETWEEN '{0}' AND '{1}'))as A left join SEMC_DMC.DBO.China_RT_city_station on a.area=SEMC_DMC.DBO.China_RT_city_station.area order by  a.timepoint", startTime, endTime);
                DataTable dt= m_DatabaseS.GetDataTable(strSQL);
                josnData = JsonConvert.SerializeObject(dt, new DataTableConverter());
            }
            else//上海
            {

                strSQL = string.Format("SELECT  CONVERT(VARCHAR(19),LST,120) AS LST1, Name,CONVERT(decimal(10, 1), VALUE * 1000) AS VALUE  FROM V_lutao_PM25_xy WHERE  LST BETWEEN '{0}' AND '{1}' order by LST", startTime, endTime);
                DataTable dt= m_Database.GetDataTable(strSQL);
                DataTable dtable = new DataTable("T_SHPM2.5");
                dtable.Columns.Add("LST", typeof(string));
                dtable.Columns.Add("Name", typeof(string));

                dtable.Columns.Add("Value", typeof(double));
                dtable.Columns.Add("AQI", typeof(int));
                foreach (DataRow rows in dt.Rows)
                {
                    DataRow newRow = dtable.NewRow();
                    newRow[0] = rows["LST1"];
                    newRow[1] = rows["Name"];
                    newRow[2] = rows["VALUE"];
                    string value = rows["VALUE"].ToString();
                    double inputValue = double.Parse(value) / 1000;
                    int  AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
                    newRow[3] = AQIValue;
                    dtable.Rows.Add(newRow);
                }
                josnData = JsonConvert.SerializeObject(dtable, new DataTableConverter());
            }
            return josnData;
        }
    }
}
