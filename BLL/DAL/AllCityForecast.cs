using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using MySql.Data.MySqlClient;
using ChinaAQI;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MMShareBLL.DAL
{
    public class AllCityForecast
    {
        private Database m_DatabaseMySQL;
        private Database m_Database;
        public AllCityForecast()
        {
            m_DatabaseMySQL = new Database("MySQL");
            m_Database = new Database();
        }
        //public string GetForecast(string forecastDate)
        //{
        //    string[] siteID = { "50745", "50873", "54453", "54337", "54324", "54237", "54497", "54347", "54094" ,"50850","50853","54827"};
        //    forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
        //    //给预报数据赋值
        //    string strSQL = " select * from forecast_day_csj where forecastDate='" + forecastDate + "' and citycode in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827');";
        //    DataTable dt = m_Database.GetDataTable(strSQL);

        //    strSQL = " select Site,LST,DATEDIFF(day,forecastDate,LST) as type, ITEMID, Value, AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and Interval in(4,28,52)  and durationID=7 " +
        //        "  union select Site,LST,DATEDIFF(day,forecastDate,LST) as type,'0' as ITEMID, MAX(Value), max(AQI) as AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and durationID=7  and Interval in(4,28,52) group by Site,LST,DATEDIFF(day,forecastDate,LST)";
        //    DataTable dm = m_Database.GetDataTable(strSQL);
        //    StringBuilder sb = new StringBuilder("{");
        //    //给参考模式数据赋值
        //    foreach (DataRow dr in dm.Rows)
        //    {
        //        //AQI的值
        //        if (dr["ITEMID"].ToString() == "0")
        //            sb.Append(string.Format("'PH{0}{1}AQI':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["AQI"].ToString()));
        //        else
        //        {
        //            sb.Append(string.Format("'PH{0}{1}{3}':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["Value"].ToString() == "" ? "" : Math.Round(double.Parse(dr["Value"].ToString()), 1).ToString(), dr["ITEMID"].ToString()));
        //        }
        //    }
        //    AQIExtention aqiExt;
        //    int AQI = 0;
        //    string returnDestribute = "";
        //    string[] desrtibute;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        //颜色等级一类的特殊处理
        //        if (dr["AQI"].ToString() != "0" && !string.IsNullOrEmpty(dr["AQI"].ToString()) )
        //        {
        //            sb.Append(string.Format("'H{0}{1}1':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM25"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}2':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM10"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}3':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["NO2"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}5':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["O3"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}6':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["CO"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}7':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["SO2"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}primeplu':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["primeplu"].ToString()));
        //            sb.Append(string.Format("'H{0}{1}AQI':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["AQI"].ToString()));
        //            AQI = int.Parse(dr["AQI"].ToString());
        //            aqiExt = new AQIExtention(AQI);
        //            returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true);
        //            desrtibute = returnDestribute.Split('，');
        //            if (desrtibute[1].Trim() == "优")
        //                sb.Append(string.Format("'H{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), "-"));
        //            else
        //                sb.Append(string.Format("'H{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[0]));
        //            sb.Append(string.Format("'H{0}{1}text':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[1]));
        //            sb.Append(string.Format("'H{0}{1}color':'{2}',", dr[0].ToString(), dr[2].ToString(), aqiExt.Color));
        //        }
        //    }
        //    //去掉多余的“,”
        //    if (sb.Length > 1)
        //    {
        //        sb.Remove(sb.Length - 1, 1);
        //    }
        //    sb.Append("}");
        //    return sb.ToString();

        //}

        public string GetForecast(string forecastDate)
        {
            string[] siteID = { "50745", "50873", "54094", "50850", "50853", "54827","50953" };
            forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
            //给预报数据赋值
            string strSQL = " select * from forecast_day_csj where forecastDate='" + forecastDate + "' and citycode in ('50745','50873','54094','50850','50853','54827','50953');";
            DataTable dt = m_Database.GetDataTable(strSQL);

          //  strSQL = " select Site,LST,DATEDIFF(day,forecastDate,LST) as type, ITEMID, Value, AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827','50953') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and Interval in(4,28,52)  and durationID=7 " +
              //  "  union select Site,LST,DATEDIFF(day,forecastDate,LST) as type,'0' as ITEMID, MAX(Value), max(AQI) as AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827','50953') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and durationID=7  and Interval in(4,28,52) group by Site,LST,DATEDIFF(day,forecastDate,LST)";
            
            
            //薛辉修改  2016-12-14 
            strSQL = " (select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type, ITEMID, Value, AQI from T_ForecastSite "+
                   " where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and " +
                   " Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   "  and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and Interval in(28,52)  and durationID=7 "+
                   "  union"+
                   "  select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type, ITEMID, Value, AQI from T_ForecastSite "+
                   " where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and " +
                   " Site in ('50745','50873','54094','50850','50853','54827','50953')" +
                   "  and Module='WRFCMAQ' and ITEMID in (1,2,3,5,6,7) and Interval =76 and durationID=7 ) "+
                   "  union  "+
                   " select * from ("+
                   "  select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type,'0' as ITEMID, "+
                   "  MAX(Value) as Value, max(AQI) as AQI from T_ForecastSite where"+
                   "   forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "'" +
                   "    and Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   " and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and durationID=7 "+
                   "     and Interval in(28,52) group by Site,LST,DATEDIFF(day,forecastDate,LST)"+
                   "     union"+
                   "      select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type,'0' as ITEMID, "+
                   "  MAX(Value) as Value, max(AQI) as AQI from T_ForecastSite where"+
                   "   forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "'" +
                   "    and Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   " and Module='WRFCMAQ' and ITEMID in (1,2,3,5,6,7) and durationID=7 "+
                   "     and Interval = 76  group by Site,LST,DATEDIFF(day,forecastDate,LST)) x";

            
            
            
            
            Database m_Databaseii = new Database();
            DataTable dm = m_Databaseii.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder("{");
            //给参考模式数据赋值
            foreach (DataRow dr in dm.Rows)
            {
                //AQI的值
                if (dr["ITEMID"].ToString() == "0")
                    sb.Append(string.Format("'PH{0}{1}AQI':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["AQI"].ToString()));
                else
                {
                    sb.Append(string.Format("'PH{0}{1}{3}':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["Value"].ToString() == "" ? "" : Math.Round(double.Parse(dr["Value"].ToString()), 1).ToString(), dr["ITEMID"].ToString()));
                }
            }
            AQIExtention aqiExt;
            int AQI = 0;
            string returnDestribute = "";
            string[] desrtibute;
            foreach (DataRow dr in dt.Rows)
            {
                //颜色等级一类的特殊处理
                if (dr["AQI"].ToString() != "0" && !string.IsNullOrEmpty(dr["AQI"].ToString()))
                {
                    sb.Append(string.Format("'H{0}{1}1':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM25"].ToString()));
                    sb.Append(string.Format("'H{0}{1}2':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM10"].ToString()));
                    sb.Append(string.Format("'H{0}{1}3':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["NO2"].ToString()));
                    sb.Append(string.Format("'H{0}{1}5':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["O3"].ToString()));
                    sb.Append(string.Format("'H{0}{1}6':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["CO"].ToString()));
                    sb.Append(string.Format("'H{0}{1}7':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["SO2"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}primeplu':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["primeplu"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}AQI':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["AQI"].ToString()));
                    AQI = int.Parse(dr["AQI"].ToString());
                    aqiExt = new AQIExtention(AQI);
                    returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true);
                    desrtibute = returnDestribute.Split('，');
                    if (desrtibute[1].Trim() == "优")
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[0]));
                    else
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[0]));
                    sb.Append(string.Format("'XH{0}{1}text':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[1]));
                    sb.Append(string.Format("'XH{0}{1}color':'{2}',", dr[0].ToString(), dr[2].ToString(), aqiExt.Color));
                    sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["ELE_overproof"].ToString()));

                }
                else {
                    sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["ELE_overproof"].ToString()));
                }
            }
            //去掉多余的“,”
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();

        }

        //读取各产品发布状态表
        public string ReadStateTable()
        {
            StringBuilder sb = new StringBuilder("{");
            //string strStateJson = "";
            string strReadSQL = "SELECT ModuleType,State From T_State WHERE datediff(day,ReTime,getdate())=0 and Type='3' ";//新增一个Type='3'科技服务预报
            DataTable dtState = m_Database.GetDataTable(strReadSQL);
            if (dtState.Rows.Count > 0)
            {
                for (int i = 0; i < dtState.Rows.Count; i++)
                {
                    sb.Append("\"" + dtState.Rows[i]["ModuleType"].ToString() + "\":\"" + dtState.Rows[i]["State"].ToString().Trim(' ') + "\",");
                }
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("}");
                }
                return sb.ToString();
            }
            return "";
        }

        public void InsertIntoStateTable(string functionName, string reTime, string deadLine, string state, string type)
        {
            string strStateNum = "0";
            if (state == "")
            {
                state = "0";
            }

            //查询当天是否有记录
            string strQueryExistSQL = "select ModuleType, ReTime from T_State where ModuleType='" + functionName + "' AND ReTime='" + reTime + "'";
            DataTable dtQueryExist = m_Database.GetDataTable(strQueryExistSQL);
            deadLine = "";
            string strInsertStateSQL = "INSERT INTO T_State(ModuleType,ReTime,DeadLine,State,Type) VALUES('" + functionName + "','" + reTime + "','" + deadLine + "','" + state + "','" + type + "')";
            //当天未插入状态记录
            if (dtQueryExist.Rows.Count == 0)
            {
                strInsertStateSQL = "INSERT INTO T_State(ModuleType,ReTime,DeadLine,State,Type) VALUES('" + functionName + "','" + reTime + "','" + deadLine + "','" + state + "','" + type + "')";
            }
            //当天已插入记录
            else
            {
                strInsertStateSQL = "delete from T_State where ModuleType='" + functionName + "' AND ReTime='" + reTime + "' " + "INSERT INTO T_State(ModuleType,ReTime,DeadLine,State,Type) VALUES('" + functionName + "','" + reTime + "','" + deadLine + "','" + state + "','" + type + "')";
            }
            m_Database.Execute(strInsertStateSQL);
        }

        public string GetForecastHistory()
        {
            DateTime dts = DateTime.Now.AddDays(-1);
            string forecastDate = dts.ToString("yyyy-MM-dd HH:mm:ss");
            string[] siteID = { "50745","50873","54094","50850","50853","54827","50953"};//新添加三个城市  2016-10-30  
            forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");

            string forecastDate1 = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string forecastDate2 = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd 00:00:00");

            string strSQL = " select * from forecast_day_csj where forecastDate='" + forecastDate + "' and citycode in ('50745','50873','54094','50850','50853','54827','50953') ";
            DataTable dt = m_Database.GetDataTable(strSQL);



           // strSQL = " select Site,LST,DATEDIFF(day,forecastDate,LST) as type, ITEMID, Value, AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(0).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827','50953') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and Interval in(4,28,52)  and durationID=7 " +
              //  "  union select Site,LST,DATEDIFF(day,forecastDate,LST) as type,'0' as ITEMID, MAX(Value), max(AQI) as AQI from T_ForecastSite where forecastDate='" + DateTime.Parse(forecastDate).AddDays(0).ToString("yyyy-MM-dd 20:00:00") + "' and Site in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827','50953') and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and durationID=7  and Interval in(4,28,52) group by Site,LST,DATEDIFF(day,forecastDate,LST)";


            //薛辉修改  2016-12-14 
            strSQL = " (select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type, ITEMID, Value, AQI from T_ForecastSite " +
                   " where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and " +
                   " Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   "  and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and Interval in(28,52)  and durationID=7 " +
                   "  union" +
                   "  select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type, ITEMID, Value, AQI from T_ForecastSite " +
                   " where forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "' and " +
                   " Site in ('50745','50873','54094','50850','50853','54827','50953')" +
                   "  and Module='WRFCMAQ' and ITEMID in (1,2,3,5,6,7) and Interval =76 and durationID=7 ) " +
                   "  union  " +
                   " select * from (" +
                   "  select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type,'0' as ITEMID, " +
                   "  MAX(Value) as Value, max(AQI) as AQI from T_ForecastSite where" +
                   "   forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "'" +
                   "    and Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   " and Module='CUACE' and ITEMID in (1,2,3,5,6,7) and durationID=7 " +
                   "     and Interval in(28,52) group by Site,LST,DATEDIFF(day,forecastDate,LST)" +
                   "     union" +
                   "      select Site,LST,DATEDIFF(day,forecastDate,LST)-1 as type,'0' as ITEMID, " +
                   "  MAX(Value) as Value, max(AQI) as AQI from T_ForecastSite where" +
                   "   forecastDate='" + DateTime.Parse(forecastDate).AddDays(-1).ToString("yyyy-MM-dd 20:00:00") + "'" +
                   "    and Site in ('50745','50873','54094','50850','50853','54827','50953') " +
                   " and Module='WRFCMAQ' and ITEMID in (1,2,3,5,6,7) and durationID=7 " +
                   "     and Interval = 76  group by Site,LST,DATEDIFF(day,forecastDate,LST)) x";
            
            
            Database m_Databaseii = new Database();
            DataTable dm = m_Databaseii.GetDataTable(strSQL);


            StringBuilder sb = new StringBuilder("{");
            foreach (DataRow dr in dm.Rows)
            {
                if (dr["ITEMID"].ToString() == "0")
                    sb.Append(string.Format("'PH{0}{1}AQI':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["AQI"].ToString()));
                else
                {
                    sb.Append(string.Format("'PH{0}{1}{3}':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["Value"].ToString() == "" ? "" : Math.Round(double.Parse(dr["Value"].ToString()), 1).ToString(), dr["ITEMID"].ToString()));
                }
            }
            AQIExtention aqiExt;
            int AQI = 0;
            string returnDestribute = "";
            string[] desrtibute;
            string ELE = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["AQI"].ToString() != "0" && !string.IsNullOrEmpty(dr["AQI"].ToString()))
                {
                    int type = int.Parse(dr[2].ToString()) - 1;
                    if (type == 0) {
                        ELE = dr["ELE_overproof"].ToString();
                        continue;
                    }
                    sb.Append(string.Format("'H{0}{1}1':'{2}',", dr[0].ToString(), type, dr["PM25"].ToString()));
                    sb.Append(string.Format("'H{0}{1}2':'{2}',", dr[0].ToString(), type, dr["PM10"].ToString()));
                    sb.Append(string.Format("'H{0}{1}3':'{2}',", dr[0].ToString(), type, dr["NO2"].ToString()));
                    sb.Append(string.Format("'H{0}{1}5':'{2}',", dr[0].ToString(), type, dr["O3"].ToString()));
                    sb.Append(string.Format("'H{0}{1}6':'{2}',", dr[0].ToString(), type, dr["CO"].ToString()));
                    sb.Append(string.Format("'H{0}{1}7':'{2}',", dr[0].ToString(), type, dr["SO2"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}primeplu':'{2}',", dr[0].ToString(), type, dr["primeplu"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}AQI':'{2}',", dr[0].ToString(), type, dr["AQI"].ToString()));
                    AQI = int.Parse(dr["AQI"].ToString());
                    aqiExt = new AQIExtention(AQI);
                    returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true);
                    desrtibute = returnDestribute.Split('，');
                    if (desrtibute[1].Trim() == "优")
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), type, desrtibute[0]));
                    else
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), type, desrtibute[0]));
                    sb.Append(string.Format("'XH{0}{1}text':'{2}',", dr[0].ToString(), type, desrtibute[1]));
                    sb.Append(string.Format("'XH{0}{1}color':'{2}',", dr[0].ToString(), type, aqiExt.Color));
                    sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", dr[0].ToString(), type, ELE));
                }
            }

            string types = "3";
            foreach (string str in siteID)
            {
                sb.Append(string.Format("'H{0}{1}1':'{2}',", str, types, ""));
                sb.Append(string.Format("'H{0}{1}2':'{2}',", str, types, ""));
                sb.Append(string.Format("'H{0}{1}3':'{2}',", str, types, ""));
                sb.Append(string.Format("'H{0}{1}5':'{2}',", str, types, ""));
                sb.Append(string.Format("'H{0}{1}6':'{2}',", str, types, ""));
                sb.Append(string.Format("'H{0}{1}7':'{2}',", str, types, ""));
                sb.Append(string.Format("'XH{0}{1}primeplu':'{2}',", str, types, ""));
                sb.Append(string.Format("'XH{0}{1}AQI':'{2}',", str, types, ""));
                AQI = 0;
                aqiExt = new AQIExtention(AQI);
                returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true);
                desrtibute = returnDestribute.Split('，');
                if (desrtibute[1].Trim() == "优")
                    sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", str, types, "-"));
                else
                    sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", str, types, ""));
                sb.Append(string.Format("'XH{0}{1}text':'{2}',", str, types, ""));
                sb.Append(string.Format("'XH{0}{1}color':'{2}',", str, types, ""));
                sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", str, types, ""));
            }
            //去掉多余的“,”
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();

        }
        public string PublishForecast(string postJson, string forecastDate)
        {
            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = SaveContentII(postJson, forecastDate);
            //入自己的库
            string strSQL = "DELETE from forecast_day_csj WHERE  forecastDate='" + startDate.ToString("yyyy-MM-dd 00:00:00") + "'  and citycode in ('50745','50873','54453','54324','54237','54497','54347','54094','50850','50853','54827','50953')";
            m_Database.Execute(strSQL);
            m_Database.BulkCopy(dt);

            //MySQL数据库的批量数据处理
            strSQL = "DELETE from forecast_day_csj WHERE  forecastDate='" + startDate.ToString("yyyy-MM-dd") + "'  and citycode in ('50745','50873','54453','54324','54237','54497','54347','54094','50850','50853','54827','50953')";
            m_DatabaseMySQL.ExecuteMySQL(strSQL);

            int count = BulkInsert(dt);
            if (count > 0)
            {
                InsertIntoStateTable("KJFW", startDate.ToString("yyyy-MM-dd HH:00:00"), startDate.ToString("yyyy-MM-dd HH:00:00"),"3", "3");
                return "发布成功！";
            }
            else
                return "发布失败";
        }
        public DataTable SaveContent(string postJson, string forecastDate)
        {
            string[] parts = postJson.Split(';');
            int[] itemCode = { 1, 2, 6, 5, 7, 3 };
            string[] itemPatameter = { "PM25", "PM10", "CO", "O3", "SO2", "NO2" };
            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = new DataTable("forecast_day_csj");
            dt.Columns.Add("citycode", typeof(string));
            dt.Columns.Add("showDate", typeof(DateTime));
            dt.Columns.Add("type", typeof(int));

            dt.Columns.Add("PM25", typeof(int));
            dt.Columns.Add("PM10", typeof(int));
            dt.Columns.Add("CO", typeof(int));
            dt.Columns.Add("O3", typeof(int));
            dt.Columns.Add("SO2", typeof(int));
            dt.Columns.Add("NO2", typeof(int));
            dt.Columns.Add("AQI", typeof(int));

            dt.Columns.Add("level", typeof(int));
            dt.Columns.Add("primeplu", typeof(string));
            dt.Columns.Add("ELE_overproof", typeof(string));
            dt.Columns.Add("forecastDate", typeof(DateTime));
            string[] keyValue;
            string[] ItemValue;
            int type = 0;
            DateTime lst;
            int maxAQI = 0;
            int AQI = 0;
            string parameter = ""; ;
            AQIExtention aqiExt;
            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    maxAQI = 0;
                    parameter = ""; ;

                    DataRow newRow = dt.NewRow();
                    keyValue = parts[i].Split(':');
                    //获取行号，并计算此行的预报日期
                    type = int.Parse(keyValue[0].Substring(keyValue[0].Length - 1, 1));
                    newRow[0] = keyValue[0].Substring(1, keyValue[0].Length - 2);
                    lst = startDate.AddDays(type);
                    newRow[1] = lst.ToString("yyyy-MM-dd");
                    newRow[2] = type;
                    ItemValue = keyValue[1].Split(',');
                    for (int j = 0; j < ItemValue.Length; j++)
                    {
                        AQI = 0;
                        if (ItemValue[j] == "")
                            newRow[3 + j] = DBNull.Value;
                        else
                        {
                            newRow[3 + j] = ItemValue[j];
                            AQI = ToAQI(ItemValue[j], itemCode[j].ToString());

                        }
                        if (AQI > maxAQI)
                        {
                            maxAQI = AQI;
                            parameter = itemPatameter[j].ToString();

                        }
                        else if (AQI == maxAQI && AQI != 0)
                            parameter = parameter + "," + itemPatameter[j].ToString();
                    }
                    if (maxAQI > 0)
                    {
                        aqiExt = new AQIExtention(maxAQI);
                        newRow[9] = maxAQI;
                        newRow[10] = aqiExt.IntGrade;
                    }
                    else
                    {
                        newRow[9] = DBNull.Value;
                        newRow[10] = DBNull.Value;
                    }

                    if (DBNull.Value == newRow[9] ||
                        int.Parse(newRow[9].ToString()) <= 50)
                    {
                        newRow[11] = "";
                    }
                    else
                    {
                        newRow[11] = parameter;
                    }

                    newRow[12] = DBNull.Value;
                    newRow[13] = startDate.ToString("yyyy-MM-dd");
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public DataTable SaveContentII(string postJson, string forecastDate)
        {
            string[] parts = postJson.Split(';');
            int[] itemCode = { 1, 2, 6, 5, 7, 3 };
            string[] itemPatameter = { "PM25", "PM10", "CO", "O3", "SO2", "NO2" };
            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = new DataTable("forecast_day_csj");
            dt.Columns.Add("citycode", typeof(string));
            dt.Columns.Add("showDate", typeof(DateTime));
            dt.Columns.Add("type", typeof(int));

            dt.Columns.Add("PM25", typeof(int));
            dt.Columns.Add("PM10", typeof(int));
            dt.Columns.Add("CO", typeof(int));
            dt.Columns.Add("O3", typeof(int));
            dt.Columns.Add("SO2", typeof(int));
            dt.Columns.Add("NO2", typeof(int));
            dt.Columns.Add("AQI", typeof(int));

            dt.Columns.Add("level", typeof(int));
            dt.Columns.Add("primeplu", typeof(string));
            dt.Columns.Add("ELE_overproof", typeof(string));
            dt.Columns.Add("forecastDate", typeof(DateTime));
            string[] keyValue;
            string[] ItemValue;
            int type = 0;
            DateTime lst;
            int maxAQI = 0;
            int AQI = 0;
            string parameter = ""; ;
            AQIExtention aqiExt;
            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    maxAQI = 0;
                    parameter = ""; ;

                    DataRow newRow = dt.NewRow();
                    keyValue = parts[i].Split(':');
                    //获取行号，并计算此行的预报日期
                    type = int.Parse(keyValue[0].Substring(keyValue[0].Length - 1, 1));
                    newRow[0] = (keyValue[0].Substring(1, keyValue[0].Length - 2)).Replace("H","");
                    lst = startDate.AddDays(type);
                    newRow[1] = lst.ToString("yyyy-MM-dd");
                    newRow[2] = type;
                    ItemValue = keyValue[1].Split(',');
                    newRow[3] = DBNull.Value;
                    newRow[4] = DBNull.Value;
                    newRow[5] = DBNull.Value;
                    newRow[6] = DBNull.Value;
                    newRow[7] = DBNull.Value;
                    newRow[8] = DBNull.Value;
                    try
                    {
                        int aqi = int.Parse(keyValue[1].Split(',')[0]);
                        maxAQI = aqi;
                        aqiExt = new AQIExtention(maxAQI);
                        newRow[9] = maxAQI;
                        newRow[10] = aqiExt.IntGrade;
                    }
                    catch {
                        newRow[9] = DBNull.Value;
                        newRow[10] = DBNull.Value;
                    }
                    
                    //if (DBNull.Value == newRow[9] ||
                    //    int.Parse(newRow[9].ToString()) <= 50)  薛辉修改 20161121
                    //{
                    //    newRow[11] = "";
                    //}
                    //else
                    //{
                        try
                        {
                            Regex reg = new Regex(@"(?<=\[)[^\[\]]+(?=\])");
                            MatchCollection mc = reg.Matches(keyValue[1]);
                            newRow[11] = mc[0].Value;
                        }
                        catch { newRow[11] = ""; }
                   // }
                    try
                    {
                        newRow[12] = keyValue[1].Split(',')[keyValue[1].Split(',').Length-1];
                        if (keyValue[1].EndsWith("]")) {
                            newRow[12] = "";
                        }
                    }
                    catch {newRow[12] = DBNull.Value; }
                    newRow[13] = startDate.ToString("yyyy-MM-dd");
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public string SaveEdits(string postJson, string forecastDate)
        {

            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = SaveContentII(postJson, forecastDate);
            try
            {
                string strSQL = "DELETE from forecast_day_csj WHERE  forecastDate='" + startDate.ToString("yyyy-MM-dd 00:00:00") + "'  and citycode in ('50745','50873','54453','54337','54324','54237','54497','54347','54094','50850','50853','54827','50953')";
                m_Database.Execute(strSQL);
                //MySQL数据库的批量数据处理
                bool count = m_Database.BulkCopy(dt);
                if (count)
                {
                    InsertIntoStateTable("KJFW", startDate.ToString("yyyy-MM-dd HH:00:00"), startDate.ToString("yyyy-MM-dd HH:00:00"), "2", "3");
                    return "保存成功！";
                }
                else
                    return "保存失败";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public int BulkInsert(DataTable dt)
        {
            if (string.IsNullOrEmpty(dt.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            if (dt.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(dt);
            File.WriteAllText(tmpPath, csv);
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["MySQL"];
            string connectionString = settings.ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = dt.TableName,
                    };

                    insertCount = bulk.Load();
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }
        ///将DataTable转换为标准的CSV  
        /// </summary>  
        /// <param name="table">数据表</param>  
        /// <returns>返回标准的CSV</returns>  
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }


            return sb.ToString();
        }
        public string AQIDescribe(string content)
        {
            try
            {
                string returnStr = "";
                string[] parts = content.Split(',');
                int maxAQI = 0;
                int tempAQI = 0;
                double tempValue = 0;
                string itemID = "";
                string tempItemID = "";
                string id = "";
                for (int i = 0; i < parts.Length; i++)
                {
                    string[] durationValue = parts[i].Split(':');
                    if (durationValue.Length > 1)
                    {
                        if (durationValue[1] != "")
                        {
                            tempValue = Math.Round(float.Parse(durationValue[1]), 1);
                            tempItemID = durationValue[0].Substring(durationValue[0].Length - 1, 1);
                            tempAQI = ToAQI(tempValue.ToString(), tempItemID);
                            if (tempAQI > maxAQI)
                            {
                                maxAQI = tempAQI;
                                id = durationValue[0].Substring(0, durationValue[0].Length - 1);
                                itemID = tempItemID;

                            }

                        }
                    }

                }
                if (maxAQI != 0)
                {
                    returnStr = changeAQIValue(maxAQI, id, itemID);
                }
                return returnStr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string AQIDescribeII(string content)
        {
            try
            {
                string returnStr = "";
                string[] parts = content.Split(',');
                int maxAQI = 0;
                int tempAQI = 0;
                double tempValue = 0;
                string itemID = "";
                string tempItemID = "";
                string id = "";
                for (int i = 0; i < parts.Length; i++)
                {
                    string[] durationValue = parts[i].Split(':');
                    if (durationValue.Length > 1)
                    {
                        if (durationValue[1] != "")
                        {
                            tempValue = Math.Round(float.Parse(durationValue[1]), 1);
                            tempItemID = durationValue[0].Substring(durationValue[0].Length - 1, 1);
                            tempAQI = int.Parse(tempValue.ToString());
                            if (tempAQI > maxAQI)
                            {
                                maxAQI = tempAQI;
                                id = durationValue[0].Substring(0, durationValue[0].Length - 1);
                                //itemID = tempItemID;

                            }

                        }
                    }

                }
                if (maxAQI != 0)
                {
                    returnStr = changeAQIValue(maxAQI, id.Replace("AQ",""), itemID);
                }
                return returnStr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string returnPatamer(string itemID)
        {
            string papameter = "";
            switch (itemID)
            {
                case "1":
                    papameter = "PM25";
                    break;
                case "2":
                    papameter = "PM10";
                    break;
                case "3":
                    papameter = "NO2";
                    break;
                case "5":
                    papameter = "O3";
                    break;
                case "7":
                    papameter = "SO2";
                    break;
                case "6":
                    papameter = "CO";
                    break;

            }
            return papameter;
        }
        public string changeAQIValue(int value, string id, string itemID)
        {

            AQIExtention aqiExt = new AQIExtention(value);
            string returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, "", true);
            string[] desrtibute = returnDestribute.Split('，');
            StringBuilder sb = new StringBuilder("{");
            sb.Append(string.Format("{0}{1}:'{2}',", id, "color", aqiExt.Color));
            sb.Append(string.Format("{0}{1}:'{2}',", id, "AQI", value));
            if (desrtibute[1].Trim() == "优")
            {
                //sb.Append(string.Format("{0}{1}:'{2}',", id, "primeplu", ""));
            }
            else
            {
                if (value <= 50)
                {
                   // sb.Append(string.Format("{0}{1}:'{2}',", id, "primeplu", ""));//薛辉 2016-09-10
                }
                else
                {
                    sb.Append(string.Format("{0}{1}:'{2}',", id, "primeplu", ""));//2016-11-09这里不处理了。客户自己选
                }
            }
            sb.Append(string.Format("{0}{1}:'{2}',", id, "text", desrtibute[1]));
            sb.Append(string.Format("{0}{1}:'{2}'", id, "kqzl", desrtibute[0]));
            sb.Append("}");
            return sb.ToString();

        }
        //日AQI
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
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 11, 0);
                    break;
                case "4":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
                    break;
                case "5":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
                    break;
                case "6":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 6, 11, 180);
                    break;
                case "7":
                    AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 1, 11, 180);
                    break;
            }

            return AQIValue;
        }
        private string ParseAQIForAirQuality(int aqi, string firstParameter, bool showDescription)
        {
            string description = "首要污染物";
            if (showDescription == false)
                description = "";
            description = "，" + description + firstParameter;


            int fAQI = aqi - 15;
            int tAQI = aqi + 15;

            if (fAQI <= 0)
                fAQI = 1;


            AQIExtention fAqiExt = new AQIExtention(fAQI);
            AQIExtention tAqiExt = new AQIExtention(tAQI);
            string strGrade = fAqiExt.Quality;

            if (strGrade != tAqiExt.Quality)
            {
                strGrade = string.Format("{0}到{1}", strGrade, tAqiExt.Quality);
            }
            else if (strGrade == "优")//优的情况下，不显示首页污染物
                description = "";
            if (strGrade == "轻度污染到中度污染")
                strGrade = "轻度-中度";
            if (strGrade == "中度污染到重度污染")
                strGrade = "中度-重度";
            if (strGrade == "重度污染到严重污染")
                strGrade = "重度-严重";
            if (strGrade == "良到轻度污染")
                strGrade = "良到-轻度";

            string aqiSM = string.Format("{0}-{1}，{2}", fAQI, tAQI, strGrade);

            return aqiSM;
        }


        public string GetSHForecast(string forecastDate)
        {
            string[] siteID = { "31000" };
            forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
            //给预报数据赋值
            string strSQL = " select * from forecast_day_sh where forecastDate='" + forecastDate + "' and citycode in ('31000');";
            DataTable dt = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder("{");
            //给参考模式数据赋值
            //foreach (DataRow dr in dm.Rows)
            //{
            //    try
            //    {
            //        //AQI的值
            //        if (dr["ITEMID"].ToString() == "0")
            //            sb.Append(string.Format("'PH{0}{1}AQI':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["AQI"].ToString()));
            //        else
            //        {
            //            sb.Append(string.Format("'PH{0}{1}{3}':'{2}',", dr["Site"].ToString(), dr["type"].ToString(), dr["Value"].ToString() == "" ? "" : Math.Round(double.Parse(dr["Value"].ToString()), 1).ToString(), dr["ITEMID"].ToString()));
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            AQIExtention aqiExt;
            int AQI = 0;
            string returnDestribute = "";
            string[] desrtibute;
            foreach (DataRow dr in dt.Rows)
            {
                //颜色等级一类的特殊处理
                if (dr["AQI"].ToString() != "0" && !string.IsNullOrEmpty(dr["AQI"].ToString()))
                {
                    sb.Append(string.Format("'H{0}{1}1':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM25"].ToString()));
                    sb.Append(string.Format("'H{0}{1}2':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["PM10"].ToString()));
                    sb.Append(string.Format("'H{0}{1}3':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["NO2"].ToString()));
                    sb.Append(string.Format("'H{0}{1}5':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["O3"].ToString()));
                    sb.Append(string.Format("'H{0}{1}6':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["CO"].ToString()));
                    sb.Append(string.Format("'H{0}{1}7':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["SO2"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}primeplu':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["primeplu"].ToString()));
                    sb.Append(string.Format("'XH{0}{1}AQI':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["AQI"].ToString()));
                    AQI = int.Parse(dr["AQI"].ToString());
                    aqiExt = new AQIExtention(AQI);
                    returnDestribute = ParseAQIForAirQuality(aqiExt.AQI, aqiExt.FirstPItemNoByGrade, true);
                    desrtibute = returnDestribute.Split('，');
                    if (desrtibute[1].Trim() == "优")
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[0]));
                    else
                        sb.Append(string.Format("'XH{0}{1}kqzl':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[0]));
                    sb.Append(string.Format("'XH{0}{1}text':'{2}',", dr[0].ToString(), dr[2].ToString(), desrtibute[1]));
                    sb.Append(string.Format("'XH{0}{1}color':'{2}',", dr[0].ToString(), dr[2].ToString(), aqiExt.Color));
                    sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["ELE_overproof"].ToString()));

                }
                else
                {
                    sb.Append(string.Format("'XH{0}{1}tqxs':'{2}',", dr[0].ToString(), dr[2].ToString(), dr["ELE_overproof"].ToString()));
                }
            }
            //去掉多余的“,”
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();

        }

        public DataTable SaveSHContent(string postJson, string forecastDate)
        {
            string[] parts = postJson.Split(';');
            int[] itemCode = { 1, 2, 6, 5, 7, 3 };
            string[] itemPatameter = { "PM25", "PM10", "CO", "O3", "SO2", "NO2" };
            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = new DataTable("forecast_day_sh");
            dt.Columns.Add("citycode", typeof(string));
            dt.Columns.Add("showDate", typeof(DateTime));
            dt.Columns.Add("type", typeof(int));

            dt.Columns.Add("PM25", typeof(int));
            dt.Columns.Add("PM10", typeof(int));
            dt.Columns.Add("CO", typeof(int));
            dt.Columns.Add("O3", typeof(int));
            dt.Columns.Add("SO2", typeof(int));
            dt.Columns.Add("NO2", typeof(int));
            dt.Columns.Add("AQI", typeof(int));

            dt.Columns.Add("level", typeof(int));
            dt.Columns.Add("primeplu", typeof(string));
            dt.Columns.Add("ELE_overproof", typeof(string));
            dt.Columns.Add("forecastDate", typeof(DateTime));
            string[] keyValue;
            string[] ItemValue;
            int type = 0;
            DateTime lst;
            int maxAQI = 0;
            AQIExtention aqiExt;
            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    maxAQI = 0;

                    DataRow newRow = dt.NewRow();
                    keyValue = parts[i].Split(':');
                    //获取行号，并计算此行的预报日期
                    type = int.Parse(keyValue[0].Replace("XH31000", ""));
                    newRow[0] = keyValue[0].Substring(2, 5);
                    lst = startDate.AddDays(type);
                    newRow[1] = lst.ToString("yyyy-MM-dd");
                    newRow[2] = type;
                    ItemValue = keyValue[1].Split(',');
                    newRow[3] = DBNull.Value;
                    newRow[4] = DBNull.Value;
                    newRow[5] = DBNull.Value;
                    newRow[6] = DBNull.Value;
                    newRow[7] = DBNull.Value;
                    newRow[8] = DBNull.Value;
                    try
                    {
                        int aqi = int.Parse(keyValue[1].Split(',')[0]);
                        maxAQI = aqi;
                        aqiExt = new AQIExtention(maxAQI);
                        newRow[9] = maxAQI;
                        newRow[10] = aqiExt.IntGrade;
                    }
                    catch
                    {
                        newRow[9] = DBNull.Value;
                        newRow[10] = DBNull.Value;
                    }
                    try
                    {
                        Regex reg = new Regex(@"(?<=\[)[^\[\]]+(?=\])");
                        MatchCollection mc = reg.Matches(keyValue[1]);
                        newRow[11] = mc[0].Value;
                    }
                    catch { newRow[11] = ""; }
                    try
                    {
                        newRow[12] = keyValue[1].Split(',')[keyValue[1].Split(',').Length - 1];
                        if (keyValue[1].EndsWith("]"))
                        {
                            newRow[12] = "";
                        }
                    }
                    catch { newRow[12] = DBNull.Value; }
                    newRow[13] = startDate.ToString("yyyy-MM-dd");
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public string SaveSHEdits(string postJson, string forecastDate)
        {

            DateTime startDate = DateTime.Parse(forecastDate);
            DataTable dt = SaveSHContent(postJson, forecastDate);
            try
            {
                string strSQL = "DELETE from forecast_day_sh WHERE  forecastDate='" + startDate.ToString("yyyy-MM-dd 00:00:00") + "'  and citycode in ('31000')";
                m_Database.Execute(strSQL);
                bool count = m_Database.BulkCopy(dt);
                if (count) return "保存成功！";
                else
                    return "保存失败";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
