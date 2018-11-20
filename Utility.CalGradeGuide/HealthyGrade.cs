using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using ChinaAirUtility;
using Readearth.Data;
using Readearth.DB;

namespace Utility.GradeGuide
{
   public class HealthyGrade
    {
        private Database m_Database;
        private List<string> m_Type;    //要素对应表名
        private List<int> PERIOD;    //要素对应表名
        private DataTable StationTable;             //站点表
        private DataTable dtCalData_HealthyGrade;//指引表
        private DataTable ZSTable;//中暑数据表
        private string m_Month;
        private double T24Grade,T48Grade;
        enum Itemid
        {
            PM25 = 1,
            PM10 = 2,
            NO2 = 3,
            O3 = 4,
            O38H = 5,
           
        };
        public HealthyGrade(Database database)
        {
            m_Database = database;

            StationTable = m_Database.GetDataTable("(select station_co FROM sta_reg_set where province ='上海市')");
            dtCalData_HealthyGrade = DTOperation.CreateVoidDT(m_Database, "T_HealthyGrade");
            m_Type = new List<string>();
            m_Type.Add("中暑");
            m_Type.Add("儿童感冒");
            m_Type.Add("成人感冒");
            m_Type.Add("老人感冒");
            m_Type.Add("儿童哮喘");
            m_Type.Add("COPD");
            
            m_Month = DateTime.Now.Month.ToString().PadLeft(2, '0');

            PERIOD = new List<int>();
            PERIOD.Add(24);
            PERIOD.Add(48);
            PERIOD.Add(72);
        }
        //处理预报等级数据
        public void InsertData(DateTime forecasttime, string module, bool isDo)
        {
            try
            {
//                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
//  FROM [T_HealthyGrade] where  PERIOD!=0 and    ForecastDate ='{0}'  order by  ForecastDate desc", forecasttime)) == forecasttime.ToString())
//                    return;
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
  FROM [T_ForecastWeather1] where   Module='{1}' and  ForecastDate='{0}' order by  ForecastDate desc", forecasttime, module)) != forecasttime.ToString())
                    return;
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
  FROM [T_ForecastSite] where   Module='{0}' and  ForecastDate='{1}' order by   [ForecastDate] desc", module, forecasttime)) != forecasttime.ToString())
                    return;

                m_Month = forecasttime.AddHours(4).Month.ToString().PadLeft(2, '0');
                ZSTable = m_Database.GetDataTable(string.Format(@"( SELECT TGrade,ForecastDate,[station]
  FROM T_HealthyGrade WHERE    [PERIOD]=24 and [ForecastDate]>='{0}' and  ForecastDate<='{2}' AND  datepart(hh,[ForecastDate]) ={1} and  Type='中暑'  ) ", forecasttime.AddDays(-4), forecasttime.Hour, forecasttime));
                foreach (string disease in m_Type)
                {
                    foreach (int period in PERIOD)
                    {
                        DateTime LST = Convert.ToDateTime(forecasttime.Date.ToString("yyyy-MM-dd 20:00:00")).AddHours(period - 24);
                        DateTime fromtime = LST;
                        if (disease.Contains("感冒"))
                            fromtime = LST.AddDays(-4);
                        m_Month = LST.Month.ToString().PadLeft(2, '0');
                        //模式污染物数据
                        string sql = string.Format(@"  SELECT site,value,ITEMID 
  FROM [T_ForecastSite] WHERE    [durationID]=7 and [LST]='{0}'   and  Module='{2}'  AND  ForecastDate ='{1}'  ", LST, forecasttime, module);
                        DataTable PollutionTable = m_Database.GetDataTable(sql);

                        //气象数据
                        sql = string.Format(@"SELECT [Site] ,[ITEMID]  ,avg([Value]) as Value  FROM [T_ForecastWeather1] WHERE    [durationID]=7  and datepart(HH,[LST])=20 and datepart(HH,ForecastDate) ={2} and  Module='{3}' and [LST] between '{0}' and '{1}'    group by  [Site]   ,[ITEMID],Module", fromtime, LST, forecasttime.Hour, module);
                        DataTable WeatherTable = m_Database.GetDataTable(sql);
                        foreach (DataRow Dr in StationTable.Rows)
                        {
                            string site = Dr[0].ToString();
                            try
                            {
                                DataRow[] pollutiondrs = PollutionTable.Select("Site='" + site + "' ");
                                DataRow[] weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 8);
                                double rh = Convert.ToDouble(weatherdrs[0]["Value"]);
                                weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 9);
                                double temp = Convert.ToDouble(weatherdrs[0]["Value"]);
                                weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 10);
                                double maxtemp = Convert.ToDouble(weatherdrs[0]["Value"]);
                                double pm10 = 0, pm25 = 0, o3 = 0, o38h = 0, no2 = 0;
                                foreach (int pid in Enum.GetValues(typeof(Itemid)))
                                {
                                    Itemid item = (Itemid)pid;
                                    string var = item.ToString();
                                    try
                                    {
                                        pollutiondrs = PollutionTable.Select("Site='" + site + "' " + " AND  ITEMID=" + pid);
                                        if (var == "PM25")
                                            pm25 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                        if (var == "O3")
                                            o3 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                        if (var == "PM10")
                                            pm10 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                        if (var == "NO2")
                                            no2 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                        if (var == "O38H")
                                            o38h = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                    }
                                    catch { }

                                }
                                Dictionary<string, string> LastGrade = new Dictionary<string, string>();
                                if (disease == "中暑")
                                {
                                    DataRow[] zsdrs = ZSTable.Select("station='" + site + "'");
                                    foreach (DataRow dr in zsdrs)
                                    {
                                        LastGrade.Add(dr[1].ToString(), dr[0].ToString());
                                    }
                                    if (period >= 48)
                                        LastGrade.Add(forecasttime.AddDays(0).ToString("yyyy-MM-dd HH:00:00"), T24Grade.ToString());
                                    if (period >= 72)
                                        LastGrade.Add(forecasttime.AddDays(1).ToString("yyyy-MM-dd HH:00:00"), T48Grade.ToString());
                                }
                                double grade = GetGrade(disease, rh, temp, pm10, o3, pm25, maxtemp, o38h, no2, period, forecasttime, LastGrade);
                                if (period == 24)
                                    T24Grade = grade;
                                if (period == 48)
                                    T48Grade = grade;
                                DataRow newdr = dtCalData_HealthyGrade.NewRow();
                                newdr[0] = Convert.ToInt32(grade);
                                newdr[1] = forecasttime;
                                newdr[2] = period;
                                newdr[3] = site;
                                newdr[4] = disease;
                                newdr[5] = m_Month;
                                newdr[6] = Convert.ToInt32(grade);
                                dtCalData_HealthyGrade.Rows.Add(newdr);

                            }
                            catch { }
                        }
                    }
                }
                m_Database.Execute("DELETE T_HealthyGrade WHERE  ForecastDate='" + forecasttime + "' and PERIOD!=0 ");
                DTOperation.InsertToDB(m_Database, dtCalData_HealthyGrade, "T_HealthyGrade");
                int rowscount = dtCalData_HealthyGrade.Rows.Count;
                dtCalData_HealthyGrade.Clear();
                dtCalData_HealthyGrade.AcceptChanges();
                if (isDo)
                {
                    InsertAscData(forecasttime, module);
                }
            }
            catch { }
        }
        //处理订正等级数据
        public void InsertTData(DateTime forecasttime, string module)
        {
            try
            {
           
                ZSTable = m_Database.GetDataTable(string.Format(@"( SELECT TGrade,ForecastDate,[station]
  FROM T_HealthyGrade WHERE    [PERIOD]=24 and [ForecastDate]>='{0}' and  ForecastDate<='{2}' AND  datepart(hh,[ForecastDate]) ={1} and  Type='中暑'  ) ", forecasttime.AddDays(-4), forecasttime.Hour, forecasttime));

                foreach (string disease in m_Type)
                {
                    foreach (int period in PERIOD)
                    {
                        bool isPO = false;
                        if (period == 72 && forecasttime.Hour == 8)
                            continue;
                        DateTime LST = Convert.ToDateTime(forecasttime.Date.ToString("yyyy-MM-dd 20:00:00")).AddHours(period - 24);
                        m_Month = LST.Month.ToString().PadLeft(2, '0');
                        //污染物
                        DataTable PollutionTable = m_Database.GetDataTable(string.Format(@"(SELECT [Site]  ,[ITEMID],Value
  FROM [T_ForecastGroup] WHERE    [durationID]=7 and [ForecastDate]='{0}'  and   [Module]='GENERAL'  and PERIOD={1} and Value is not null) ", forecasttime.Date, period));

                        DateTime fromtime = forecasttime;
                        if (disease.Contains("感冒"))
                            fromtime = forecasttime.AddDays(-4);

                        //气象数据
                        DataTable WeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT [Site] ,[ITEMID]  ,avg([Value]) as Value, Module FROM [T_ForecastWeather1] WHERE    [durationID]=7  and datepart(HH,[LST])=20 and datepart(HH,ForecastDate) ={2} and  Module='{3}' AND  ITEMID  IN (10,11) and [LST] between '{0}' and '{1}'    group by  [Site],[ITEMID],Module) union (SELECT [Site] ,[ITEMID]  ,avg([Value]) as Value, Module FROM [T_ForecastWeather1] WHERE    [durationID]=7  and datepart(HH,[LST])=20 and    Module = 'WRF' and [LST] between '{0}' and '{1}'    group by  [Site]   ,[ITEMID],Module)", fromtime, LST, forecasttime.Hour, module));
                       
                        if (PollutionTable.Rows.Count == 0)
                        {
                            //模式污染物数据
                            for (DateTime dt = forecasttime; dt >= forecasttime.AddDays(-4); dt = dt.AddHours(-12))
                            {
                                string sql = string.Format(@"  SELECT  site ,value,ITEMID 
  FROM [T_ForecastSite] WHERE    [durationID]=7 and [LST]='{0}'   and  Module = 'WRF'  AND  ForecastDate = '{1}'  ", LST, dt, module);
                                PollutionTable = m_Database.GetDataTable(sql);
                                if (PollutionTable.Rows.Count != 0)
                                {
                                    isPO = true;
                                    break;
                                }

                            }
                        }
                        foreach (DataRow Dr in StationTable.Rows)
                        {
                            string site = Dr[0].ToString();
                            try
                            {
                                double grade = 0;
                                Dictionary<string, string> LastGrade = new Dictionary<string, string>();
                                if (disease == "中暑")
                                {
                                    DataRow[] zsdrs = ZSTable.Select("station='" + site + "'");
                                    foreach (DataRow dr in zsdrs)
                                    {
                                        LastGrade.Add(dr[1].ToString(), dr[0].ToString());
                                    }
                                    if (period >= 48)
                                        LastGrade.Add(forecasttime.AddDays(0).ToString("yyyy-MM-dd HH:00:00"), T24Grade.ToString());
                                    if (period >= 72)
                                        LastGrade.Add(forecasttime.AddDays(1).ToString("yyyy-MM-dd HH:00:00"), T48Grade.ToString());
                                    grade = GetGrade(disease, 0, 0, 0, 0, 0, 0, 0, 0, period, forecasttime, LastGrade);
                                    if (period == 24)
                                        T24Grade = grade;
                                    if (period == 48)
                                        T48Grade = grade;
                                }
                                else
                                {
                                    DataRow[] weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 8);
                                    double rh = Convert.ToDouble(weatherdrs[0]["Value"]);
                                    weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 9);
                                    double temp = Convert.ToDouble(weatherdrs[0]["Value"]);
                                    weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + 10);
                                    double maxtemp = Convert.ToDouble(weatherdrs[0]["Value"]);
                                    double pm10 = 0, pm25 = 0, o3 = 0, o38h = 0, no2 = 0;
                                    foreach (int pid in Enum.GetValues(typeof(Itemid)))
                                    {
                                        Itemid item = (Itemid)pid;
                                        string var = item.ToString();
                                        DataRow[] pollutiondrs = PollutionTable.Select("Site='58637' " + " AND  ITEMID=" + pid);
                                        if (isPO)
                                            pollutiondrs = PollutionTable.Select("Site='" + site + "'" + " AND  ITEMID=" + pid);
                                        if (pollutiondrs.Length > 0)
                                            try
                                            {
                                                if (var == "PM25")
                                                    pm25 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                                if (var == "O3")
                                                    o3 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                                if (var == "PM10")
                                                    pm10 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                                if (var == "NO2")
                                                    no2 = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                                if (var == "O38H")
                                                    o38h = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                            }
                                            catch { }

                                    }
                                    grade = GetGrade(disease, rh, temp, pm10, o3, pm25, maxtemp, o38h, no2, period, forecasttime, LastGrade);
                                }

                                DataRow newdr = dtCalData_HealthyGrade.NewRow();
                                newdr[0] = Convert.ToInt32(grade);
                                newdr[1] = forecasttime;
                                newdr[2] = period;
                                newdr[3] = site;
                                newdr[4] = disease;
                                newdr[5] = m_Month;
                                newdr[6] = Convert.ToInt32(grade);
                                newdr[7] = "Up";
                                dtCalData_HealthyGrade.Rows.Add(newdr);

                            }
                            catch { }
                        }
                    }
                }
                m_Database.Execute("DELETE T_HealthyGrade WHERE  ForecastDate='" + forecasttime + "' and PERIOD!=0 ");
                DTOperation.InsertToDB(m_Database, dtCalData_HealthyGrade, "T_HealthyGrade");
                int rowscount = dtCalData_HealthyGrade.Rows.Count;
                dtCalData_HealthyGrade.Clear();
                dtCalData_HealthyGrade.AcceptChanges();
            }
            catch { };
        }
        //处理实况等级数据
        public void InsertAaData(DateTime forecasttime)
        {
            try
            {
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
                  FROM [T_HealthyGrade] where  PERIOD=0 and    ForecastDate='{0}' ", forecasttime)) == forecasttime.ToString())
                    return;
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 Datetime
  FROM tbDust_24h  where   Datetime='{0}' ", forecasttime)) != forecasttime.ToString())
                    return;
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 time_point
  FROM T_ChinaRTAirData  where   time_point='{0}' ", forecasttime)) != forecasttime.ToString())
                    return;
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 collect_time
  FROM T_Cimiss_Day  where   collect_time='{0}' ", forecasttime)) != forecasttime.ToString())
                    return;


                Dictionary<string, string> shstation = new Dictionary<string, string>();
                shstation.Add("58367", "徐家汇");
                //shstation.Add("58361", "闵行");
                //shstation.Add("58365", "嘉定");
                shstation.Add("58362", "宝山");
                shstation.Add("58462", "松江");
                shstation.Add("58460", "金山");
                shstation.Add("58461", "青浦");
                shstation.Add("58370", "浦东");
                //shstation.Add("58463", "奉贤");
                shstation.Add("58366", "崇明");
                string selectsql = string.Format(@"(SELECT  '58367' as [station_code]
      ,[time_point]  ,avg([co_24h]) as co  ,avg([no2_24h]) as no2  ,avg([o3_24h]) as o3  ,avg([o3_8h_24h]) as o38h  ,avg([pm10_24h])as pm10 ,avg([pm2_5_24h]) as pm25
      ,avg([so2_24h]) as so2  FROM [EMFCShare].[dbo].[T_ChinaRTAirData]  where  [time_point]='{0}' and [station_code] in (SELECT [station_code]   FROM [T_AirStationInfo] where  [station_code] in ('1141A','1142A','1143A','1144A','1145A','1147A')) group by  [time_point])union (SELECT  '58370' as [station_code]
      ,[time_point]  ,avg([co_24h]) as co  ,avg([no2_24h]) as no2  ,avg([o3_24h]) as o3  ,avg([o3_8h_24h]) as o38h  ,avg([pm10_24h])as pm10 ,avg([pm2_5_24h]) as pm25
      ,avg([so2_24h]) as so2  FROM [EMFCShare].[dbo].[T_ChinaRTAirData]  where  [time_point]='{0}' and [station_code] in (SELECT [station_code]   FROM [T_AirStationInfo] where  [station_code] in ('1148A','1149A','1150A')) group by  [time_point])union (SELECT  '58461' as [station_code]
      ,[time_point]  ,avg([co_24h]) as co  ,avg([no2_24h]) as no2  ,avg([o3_24h]) as o3  ,avg([o3_8h_24h]) as o38h  ,avg([pm10_24h])as pm10 ,avg([pm2_5_24h]) as pm25
      ,avg([so2_24h]) as so2  FROM [EMFCShare].[dbo].[T_ChinaRTAirData]  where  [time_point]='{0}' and [station_code] ='1146A' group by  [time_point]) union (SELECT  [StationID] as [station_code]
      ,[Datetime]as [time_point]
      ,avg([CO]) as co
      ,avg([NO2]*1000.0) as no2
      ,avg([O3]*1000.0) as o3
      ,avg([O38h]*1000.0) as o38h
      ,avg([PM10]*1000.0)as pm10
      ,avg([PM2_5]*1000.0) as pm25
      ,avg([SO2]*1000.0) as so2
  FROM [EMFCShare].[dbo].[tbDust_24h]  where  [Datetime]='{0}' and [StationID] in ('58362','58366','58460') group by  [Datetime],[StationID]) union (SELECT  '58462' as [station_code] ,[Datetime]as [time_point] ,avg([CO]) as co ,avg([NO2]*1000.0) as no2 ,avg([O3]*1000.0) as o3  ,avg([O38h]*1000.0) as o38h
      ,avg([PM10]*1000.0)as pm10
      ,avg([PM2_5]*1000.0) as pm25
      ,avg([SO2]*1000.0) as so2
  FROM [EMFCShare].[dbo].[tbDust_24h]  where  [Datetime]='{0}' and [StationID] in ('99115') group by  [Datetime])", forecasttime);

                string insertsql = "delete T_24ShAQI where time_point = '" + forecasttime + "';insert into T_24ShAQI select * from  (" + selectsql + ") as a ";
                m_Database.Execute(insertsql);

                DataTable PollutionTable = m_Database.GetDataTable(selectsql);

                ZSTable = m_Database.GetDataTable(string.Format(@"(SELECT TGrade,ForecastDate,[station]
  FROM T_HealthyGrade WHERE    [PERIOD]=0 and [ForecastDate]>='{0}' and  ForecastDate<='{1}' and  Type='中暑' ) ", forecasttime.AddDays(-4), forecasttime));
                foreach (string disease in m_Type)
                {
                    DateTime fromtime = forecasttime;
                    if (disease.Contains("感冒"))
                        fromtime = forecasttime.AddDays(-4);
                    DataTable WeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT  [station]  ,avg(temperature)  as Temp,avg([maxtemperature]) as [MaxTemp]  ,avg([relativehumidity]) as [RH]  FROM T_Cimiss_Day WHERE   [collect_time] between '{0}' and '{1}'    group by  [station]) ", fromtime, forecasttime));
                    foreach (string site in shstation.Keys)
                    {
                        try
                        {
                            int period = 0;
                            DataRow[] pollutiondrs = PollutionTable.Select("station_code='" + site + "'");
                            DataRow[] weatherdrs = WeatherTable.Select("station='" + site + "'");
                            double rh = Convert.ToDouble(weatherdrs[0][3]);
                            double temp = Convert.ToDouble(weatherdrs[0][1]);
                            double maxtemp = Convert.ToDouble(weatherdrs[0][2]);
                            double pm10 = 0, pm25 = 0, o3 = 0, o38h = 0, no2 = 0;
                            pm25 = Convert.ToDouble(pollutiondrs[0]["pm25"].ToString());
                            o3 = Convert.ToDouble(pollutiondrs[0]["o3"].ToString());
                            pm10 = Convert.ToDouble(pollutiondrs[0]["pm10"].ToString());
                            no2 = Convert.ToDouble(pollutiondrs[0]["no2"].ToString());
                            o38h = Convert.ToDouble(pollutiondrs[0]["o38h"].ToString());
                            Dictionary<string, string> LastGrade = new Dictionary<string, string>();
                            if (disease == "中暑")
                            {
                                DataRow[] zsdrs = ZSTable.Select("station='" + site + "'");
                                foreach (DataRow dr in zsdrs)
                                {
                                    LastGrade.Add(dr[1].ToString(), dr[0].ToString());
                                }
                                if (period >= 48)
                                    LastGrade.Add(forecasttime.AddDays(1).ToString("yyyy-MM-dd HH:00:00"), T24Grade.ToString());
                                if (period >= 72)
                                    LastGrade.Add(forecasttime.AddDays(2).ToString("yyyy-MM-dd HH:00:00"), T48Grade.ToString());
                            }
                            double grade = GetGrade(disease, rh, temp, pm10, o3, pm25, maxtemp, o38h, no2, period, forecasttime, LastGrade);
                            if (period == 24)
                                T24Grade = grade;
                            if (period == 48)
                                T48Grade = grade;
                            DataRow newdr = dtCalData_HealthyGrade.NewRow();
                            newdr[0] = Convert.ToInt32(grade);
                            newdr[1] = forecasttime;
                            newdr[2] = period;
                            newdr[3] = site;
                            newdr[4] = disease;
                            newdr[5] = m_Month;
                            newdr[6] = Convert.ToInt32(grade);
                            dtCalData_HealthyGrade.Rows.Add(newdr);
                        }
                        catch { }
                    }

                }
                m_Database.Execute("DELETE T_HealthyGrade WHERE  ForecastDate='" + forecasttime + "' and PERIOD=0 ");
                DTOperation.InsertToDB(m_Database, dtCalData_HealthyGrade, "T_HealthyGrade");
                dtCalData_HealthyGrade.Clear();
                dtCalData_HealthyGrade.AcceptChanges();
            }
            catch { }
        
        }

        // 处理格点数据
        public void InsertAscData(DateTime forecasttime, string module)
        {
            int plperiod = 0;
            if (forecasttime.Hour == 8)
                plperiod = 12;
            int ROW = 400;
            int COL = 360;
            double m_X = -1047392.10507507;
            double m_Y = 2581464.302125650;
            double[, ,] ZSArray = new double[5, 400, 360];
            Dictionary<string, string> ZSFileInfos = new Dictionary<string, string>();
            //float[, ,] PM10 = new float[3, 400, 360], PM25 = new float[3, 400, 360], O3 = new float[3, 400, 360], O38H = new float[3, 400, 360], NO2 = new float[3, 400, 360], RH = new float[3, 400, 360], Temp = new float[3, 400, 360], MaxTemp = new float[3, 400, 360];
            string basepath = "X:/WRF/";
            //获取基础数据
            DateTime fromtime = forecasttime.AddDays(-4);
            DateTime totime = forecasttime;
            int index = -1;
            for (; fromtime < totime; fromtime = fromtime.AddDays(1))
            {
                string filename = string.Format("{0}/{1}/{2}/{3}", basepath, fromtime.ToString("yyyy"), fromtime.ToString("yyyyMMdd"), module + "_ZS_" + (24 + plperiod).ToString().PadLeft(3, '0') + "_07.asc");
                index++;
                if (!File.Exists(filename))
                    continue;
                StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("GB2312"));
                String line = sr.ReadLine();
                line = sr.ReadLine();
                int row = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    int col = 0;
                    string[] values = line.ToString().Split(new char[] { ' ' });
                    if (values.Length != COL)
                        continue;
                    foreach (string s in values)
                    {
                        if (s == "")
                            continue;
                        ZSArray[index, row, col] = Convert.ToSingle(s);

                        col++;
                    }
                    row++;
                }


            }
            foreach (string disease in m_Type)
            {
                foreach (int period in PERIOD)
                {
                    try
                    {
                        //if (period == 72)
                        //    continue;
                        Dictionary<string, string> FileInfos = new Dictionary<string, string>();
                        Dictionary<string, float[,]> BaseDatas = new Dictionary<string, float[,]>();

                        FileInfos.Add("RH", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_Humidity_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("MaxTemp", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_MaxTemperature_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("Temp", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_Temperature_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));

                        FileInfos.Add("PM25", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_PM25D_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("PM10", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_PM10D_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("O3", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_O3D_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("O38H", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_O38HD_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        FileInfos.Add("NO2", string.Format("{0}/{1}/{2}/{3}", basepath, forecasttime.ToString("yyyy"), forecasttime.ToString("yyyyMMdd"), module + "_NO2D_" + (period - 24 + plperiod).ToString().PadLeft(3, '0') + "_7.asc"));
                        index = 0;
                        if (disease .Contains( "感冒"))
                        {
                            fromtime = forecasttime.AddDays(-4);
                            for (; fromtime < totime; fromtime = fromtime.AddDays(1))
                            {

                                FileInfos.Add("RH&" + index, string.Format("{0}/{1}/{2}/{3}", basepath, fromtime.ToString("yyyy"), fromtime.ToString("yyyyMMdd"), module + "_Humidity_" + (period - 24).ToString().PadLeft(3, '0') + "_7.asc"));
                                FileInfos.Add("MaxTemp&" + index, string.Format("{0}/{1}/{2}/{3}", basepath, fromtime.ToString("yyyy"), fromtime.ToString("yyyyMMdd"), module + "_MaxTemperature_" + (period - 24).ToString().PadLeft(3, '0') + "_7.asc"));
                                FileInfos.Add("Temp&" + index, string.Format("{0}/{1}/{2}/{3}", basepath, fromtime.ToString("yyyy"), fromtime.ToString("yyyyMMdd"), module + "_Temperature_" + (period - 24).ToString().PadLeft(3, '0') + "_7.asc"));
                                index++;

                            }
                        }


                        foreach (string key in FileInfos.Keys)
                        {
                            string filename = FileInfos[key];
                            if (!File.Exists(filename))
                                continue;
                            StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("GB2312"));
                            String line = sr.ReadLine();
                            line = sr.ReadLine();
                            int row = 0;
                            int mark = 0;
                            if (BaseDatas.Keys.Contains(key.Split('&')[0]))
                                mark = 1;
                            else
                                BaseDatas.Add(key, new float[ROW, COL]);
                            while ((line = sr.ReadLine()) != null)
                            {
                                int col = 0;
                                string[] values = line.ToString().Split(new char[] { ' ' });
                                if (values.Length != COL + 1)
                                    continue;
                                foreach (string s in values)
                                {
                                    if (s == "")
                                        continue;
                                    if (mark == 1)
                                        BaseDatas[(key.Split('&')[0])][row, col] = Convert.ToSingle((BaseDatas[(key.Split('&')[0])][row, col] + Convert.ToSingle(s)) / 2.0);
                                    else
                                        BaseDatas[key][row, col] = Convert.ToSingle(s);

                                    col++;
                                }
                                row++;
                            }
                        }

                        //处理等级数据
                        double pm10 = 0, pm25 = 0, o3 = 0, o38h = 0, no2 = 0, rh, temp, maxtemp;
                        double[,] grades = new double[ROW, COL];
                        if (BaseDatas.Keys.Count == 0)
                            continue;
                        for (int i = 0; i < ROW; i++)
                        {
                            for (int j = 0; j < COL; j++)
                            {
                                pm10 = BaseDatas["PM10"][i, j];
                                pm25 = BaseDatas["PM25"][i, j];
                                o3 = BaseDatas["O3"][i, j];
                                o38h = BaseDatas["O38H"][i, j];
                                no2 = BaseDatas["NO2"][i, j];
                                rh = BaseDatas["RH"][i, j];
                                temp = BaseDatas["Temp"][i, j];
                                maxtemp = BaseDatas["MaxTemp"][i, j];
                                Dictionary<string, string> LastGrade = new Dictionary<string, string>();
                                if (disease == "中暑")
                                {
                                    for (int p = 0; p < ZSArray.GetLength(0); p++)
                                    {
                                        LastGrade.Add(forecasttime.AddDays(ZSArray.GetLength(0) - p).ToString(), ZSArray[p, i, j].ToString());
                                    }
                                    if (period >= 48)
                                        LastGrade.Add(forecasttime.AddDays(1).ToString("yyyy-MM-dd HH:00:00"), T24Grade.ToString());
                                    if (period >= 72)
                                        LastGrade.Add(forecasttime.AddDays(2).ToString("yyyy-MM-dd HH:00:00"), T48Grade.ToString());
                                }
                                double grade = GetGrade(disease, rh, temp, pm10, o3, pm25, maxtemp, o38h, no2, period, forecasttime, LastGrade);
                                if (period == 24)
                                    T24Grade = grade;
                                if (period == 48)
                                    T48Grade = grade;
                                grades[i, j] = grade;
                            }

                        }

                        StringBuilder sb = new StringBuilder("ncols" + "      " + COL.ToString() + Environment.NewLine);
                        sb.AppendLine("nrows" + "      " + ROW.ToString());
                        sb.AppendLine("xllcorner" + "      " + m_X);
                        sb.AppendLine("yllcorner" + "      " + m_Y);
                        sb.AppendLine("cellsize" + "      " + "6000");
                        sb.AppendLine("NODATA_value" + "      " + "-9999");
                        for (int rowindex = ROW - 1; rowindex > -1; rowindex--)
                        {
                            string strvalue = "";
                            for (int colindex = 0; colindex < COL; colindex++)
                            {
                                strvalue = strvalue + Convert.ToInt32(grades[rowindex, colindex]) + " ";

                            }
                            sb.AppendLine(strvalue);
                        }
                        string filepathname = basepath + forecasttime.ToString("yyyy") + "//" + forecasttime.ToString("yyyyMMdd") + "//" + module + "_" + GetdiseaseCode(disease) + "_" + (period + plperiod).ToString().PadLeft(3, '0') + "_07.asc";
                        StreamWriter sw = new StreamWriter(filepathname);
                        sw.Write(sb.ToString());
                        sw.Flush();
                        sb.Length = 0;
                        sw.Close();
                        FileInfo outFileInfo = new FileInfo(filepathname);
                        filepathname = filepathname.Replace(outFileInfo.Extension, ".prj");
                        WritePrj(filepathname);

                    }
                    catch { }
                }


            }
        }
        public string GetdiseaseCode(string disease)
        { 
           
        string code =disease;
        switch (disease)
            {
                case "中暑":
                    code = "ZS";
                    break;
                case "儿童哮喘":
                    code = "XC";
                    break;
                case "成人感冒":
                    code = "ColdT";
                    break;
                case "老人感冒":
                    code = "ColdO";
                    break;
                case "儿童感冒":
                    code = "ColdK";
                    break;


            
            
            }
        return code;
        
        }
        public void WritePrj(string prjName)
        {

            StreamWriter sr = new StreamWriter(prjName, false, Encoding.Default);
            sr.Write(@" Projection    LAMBERT
                      Datum         WGS84
                Spheroid      WGS84
             Units         METERS
       Zunits        NO
Xshift        0.0 
Yshift        0.0
Parameters  
  32  0  0.0 /* 1st standard parallel
  32  0  0.0 /* 2nd standard parallel
 118  0  0.0 /* central meridian
   0  0  0.0 /* latitude of projection's origin
0.0 /* false easting (meters)
0.0 /* false northing (meters)
");
            sr.Close();
        }
          //switch (key)
          //                      {
          //                          case "PM25":
          //                              PM25[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "PM10":
          //                              PM10[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "O3":
          //                              O3[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "O38H":
          //                              PM25[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "NO2":
          //                              NO2[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "RH":
          //                              RH[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "Temp":
          //                              Temp[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;
          //                          case "MaxTemp":
          //                              MaxTemp[((period - 24) / 24), row, col] = Convert.ToSingle(s);
          //                              break;

          //                      }
        public double GetGrade(string disease, double rh, double temp, double pm10, double o3, double pm25, double maxtemp, double o38h, double no2, int period, DateTime forecasttime, Dictionary<string, string> lastgrade)
        {
            double grade = 0;
           
            Dictionary<string, string> LastGradeOr = new Dictionary<string, string>();
            switch (disease)
            {
                case "儿童感冒":
                case "成人感冒":
                case "老人感冒":

                    double rhgrade = GetCograde(rh, "RH", disease)*1.0;
                    double tempgrade = GetCograde(temp, "T", disease) * 1.0;
                    if (disease == "成人感冒")
                        grade = (4.04 * tempgrade + rhgrade) / 5.04;
                    else
                        grade = (4.3 * tempgrade + rhgrade) / 5.3;
                    break;
                case "儿童哮喘":
                    int tempg = GetXcpdgrade(temp, "T");
                    int pm10g = GetXcpdgrade(pm10, "PM10");
                    int pm25g = GetXcpdgrade(pm25, "PM25");
                    int o3g = GetXcpdgrade(o3, "O3");
                    int maxg = pm10g;
                    if (pm25g > pm10g)
                        maxg = pm25g;
                    if (temp <= 13)
                        grade = (3 * tempg + maxg) / 4.0;
                    else
                        grade = (3 * tempg + maxg + o3g) / 5.0;
                    grade = Convert.ToInt32(grade);
                    if (pm10g >= 4 || pm25g >= 4)
                    {
                        if (grade <= 2)
                            grade = grade + 2;
                        else if (grade <= 4 && grade >= 3)
                            if (pm10g == 5 || pm25g == 5)
                                grade = grade + 1;
                    }
                    if (pm10 > 350 || pm25 > 150)
                        grade = 5;
                    if (grade > 5)
                        grade = 5;
                        //grade = (3 * tempg + pm10g + o3g) / 5.0;
                        //if (tempg == 1)
                        //    if (pm10g >= 4 && o3g >= 4)
                        //        grade = 3;
                        //if (tempg == 5)
                        //    grade = (3 * tempg + pm10g) / 4.0;
                        //if (DateTime.Now.Month >= 2 && DateTime.Now.Month <= 7)
                        //    grade = grade * 0.8;
                        //if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 12)
                        //    grade = grade * 1.3;
                        break;
                case "COPD":
                    List<double> list = new List<double> { GetCopdgrade(pm25, "PM25"), GetCopdgrade(pm10, "PM10"), GetCopdgrade(o38h, "O38H"), GetCopdgrade(no2, "NO2") };
                    var v = list.Max(p => p);
                    int tgrade = GetCopdgrade(temp, "T");
                    grade = (5.0 * tgrade + v * 3) / 8.0;
                    if (list[0] >= 4 || list[1] >= 4)
                    {
                        if (grade <= 2)
                            grade = grade + 2;
                        else if (grade <= 4 && grade >= 3)
                            if (list[0] >= 5 || list[1] >= 5)
                                grade = grade + 1;
                    }
                   if (list[2] >= 5 && grade < 4)
                    {
                        grade = grade + 1;
                    }
                    if (pm10 > 350 || pm25 > 150)
                        grade = 5;
                    if (grade >= 5)
                        grade = 5;
                    break;
                case "中暑":
                    double Trgrade = GetTgrade(1.8 * maxtemp - 0.55 * (1 - rh / 100.0) * (1.8 * maxtemp - 26) + 32, maxtemp);
                    int constday = 1;
                    LastGradeOr = lastgrade.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                   
                    foreach (string key in LastGradeOr.Keys)
                    {
                        int tgradetemp =Convert.ToInt32( LastGradeOr[key]);
                        if (Trgrade == tgradetemp)
                            constday++;
                        else
                            break;
                    }
                    grade = GetZSgrade(Trgrade, constday);
                    break;
            }
            return grade;
        }
        //中暑等级
        public int GetZSgrade(double tgrade, int constday)
        {
            int grade = 1;
            if (tgrade == 1 && constday == 1)
                grade = 1;
            else if (tgrade == 1 && constday == 2)
                grade = 2;
            else if (tgrade == 1 && constday == 3)
                grade = 2;
            else if (tgrade == 1 && constday >= 4)
                grade = 3;
            if (tgrade == 2 && constday == 1)
                grade = 2;
            else if (tgrade == 2 && constday == 2)
                grade = 2;
            else if (tgrade == 2 && constday == 3)
                grade = 3;
            else if (tgrade == 2 && constday >= 4)
                grade = 4;
            if (tgrade == 3 && constday == 1)
                grade = 4;
            else if (tgrade == 3 && constday == 2)
                grade = 4;
            else if (tgrade == 3 && constday == 3)
                grade = 4;
            else if (tgrade == 3 && constday >= 4)
                grade = 5;
            if (tgrade == 4 && constday == 1)
                grade = 4;
            else if (tgrade == 4 && constday == 2)
                grade = 4;
            else if (tgrade == 4 && constday == 3)
                grade = 5;
            else if (tgrade == 4 && constday >= 4)
                grade = 5;
            return grade;
        }
        //炎热等级中暑
        public int GetTgrade(double tgrade,double maxtemp)
        {
            int grade = 0;
            if (tgrade < 92 && maxtemp > 34 && maxtemp <= 35)
                grade = 1;
            else if (tgrade >= 92 && maxtemp > 33 && maxtemp <= 34)
                grade = 1;
            else if (tgrade <87 && maxtemp > 35 && maxtemp <= 37)
                grade = 2;
            else if (tgrade >= 87 && tgrade < 92 && maxtemp > 35 && maxtemp <= 36)
                grade = 2;
            else if (tgrade < 0.96 && tgrade >=92  && maxtemp > 34 && maxtemp <= 36)
                grade = 2;
            else if (tgrade >= 96 && maxtemp > 34 && maxtemp <= 35)
                grade = 2;
            else if (tgrade < 87 && maxtemp >37 && maxtemp <=39)
                grade = 3;
            else if (tgrade >= 87 && tgrade < 96 && maxtemp >36 && maxtemp <=39)
                grade = 3;
            else if (tgrade >= 96  && maxtemp >35 && maxtemp <=38)
                grade = 3;
            else if (tgrade < 96 && maxtemp > 39)
                grade = 4;
            else if (tgrade >= 96 && maxtemp > 38)
                grade = 4;
            return grade;
        }
        //感冒
        public int GetCograde(double value,string item,string type)
        {
            if (type == "成人感冒")
                type = "AT";
            else
                type = "KO";
            int grade = 0;
            if (item == "T" && type == "AT")
            {
                if (value > 22 && value <= 27)
                    grade = 1;
                else if(value>16&&value<=22)
                    grade = 2;
                else if (value > 27 && value <= 30)
                    grade = 2;
                else if (value >10 && value <= 16)
                    grade = 3;
                else if (value > 30)
                    grade = 3;
                else if (value > 5 && value <= 10)
                    grade = 4;
                else if (value <= 5)
                    grade = 5;
            }
            if (item == "RH" && type == "KO")
            {
                if (value > 80 )
                    grade = 1;
                else if (value > 65 && value <= 80)
                    grade = 2;
                else if (value > 50 && value <= 65)
                    grade = 3;
                else if (value > 35 && value <= 50)
                    grade = 4;
                else if (value <= 35)
                    grade = 5;
            }
            if (item == "T" && type == "KO")
            {
                if (value > 22 && value <= 27)
                    grade = 1;
                else if (value > 16 && value <= 27)
                    grade = 2;
                else if (value > 27 && value <= 31)
                    grade = 2;
                else if (value > 12 && value <= 16)
                    grade = 3;
                else if (value > 31)
                    grade = 3;
                else if (value > 6 && value <= 12)
                    grade = 4;
                else if (value <= 6)
                    grade = 5;
            }
            if (item == "RH" && type == "AT")
            {
                if (value > 80)
                    grade = 1;
                else if (value > 65 && value <= 80)
                    grade = 2;
                else if (value > 50 && value <= 65)
                    grade = 3;
                else if (value > 35 && value <= 50)
                    grade = 4;
                else if (value <= 35)
                    grade = 5;
            }
            return grade;
        }
        //COPD
        public int GetCopdgrade(double value, string item)
        {
            int grade = 0;
            if (item == "T" )
            {
                if (value > 26)
                    grade = 1;
                else if (value > 19 && value <= 26)
                    grade = 2;
                else if (value > 12 && value <= 19)
                    grade = 3;
                else if (value > 5 && value <= 12)
                    grade = 4;
                else if (value <= 7)
                    grade = 5;
            }
            if (item == "PM25" )
            {
                if (value > 115)
                    grade = 5;
                else if (value > 75 && value <= 115)
                    grade = 4;
                else if (value > 55 && value <= 75)
                    grade = 3;
                else if (value > 35 && value <= 55)
                    grade = 2;
                else if (value <= 35)
                    grade = 1;
            }
            if (item == "PM10" )
            {
                if (value > 200 )
                    grade = 5;
                else if (value > 150 && value <= 200)
                    grade = 4;
                else if (value > 100 && value <= 150)
                    grade = 3;
                else if (value > 50 && value <= 100)
                    grade = 2;
                else if (value <= 50)
                    grade = 1;
            }
            if (item == "NO2" )
            {
                if (value > 180)
                    grade = 5;
                else if (value > 80 && value <= 180)
                    grade = 4;
                else if (value > 40 && value <= 80)
                    grade = 3;
                else if (value > 20 && value <= 40)
                    grade = 2;
                else if (value <= 20)
                    grade = 1;
            }
            if (item == "O38H")
            {
                if (value > 215)
                    grade = 5;
                else if (value > 160 && value <= 215)
                    grade = 4;
                else if (value > 100 && value <= 160)
                    grade = 3;
                else if (value > 50 && value <= 100)
                    grade = 2;
                else if (value <= 50)
                    grade = 1;
            }
            return grade;
        }
        //哮喘
        public int GetXcpdgrade(double value, string item)
        {
            int grade = 0;
            if (item == "T")
            {
                if (value > 27)
                    grade = 1;
                else if (value > 22 && value <= 27)
                    grade = 2;
                else if (value > 13 && value <= 22)
                    grade = 3;
                else if (value > 8 && value <= 13)
                    grade = 4;
                else if (value <= 8)
                    grade = 5;
            }
          
            if (item == "PM10")
            {
                if (value > 250)
                    grade = 5;
                else if (value > 150 && value <= 250)
                    grade = 4;
                else if (value > 95 && value <= 150)
                    grade = 3;
                else if (value > 50 && value <= 95)
                    grade = 2;
                else if (value <= 50)
                    grade = 1;
            }
           
            if (item == "O3")
            {
                if (value > 215)
                    grade = 5;
                else if (value > 160 && value <= 215)
                    grade = 4;
                else if (value > 100 && value <= 160)
                    grade = 3;
                else if (value > 50 && value <= 100)
                    grade = 2;
                else if (value <= 50)
                    grade = 1;
            }
            if (item == "PM25")
            {
                if (value > 115)
                    grade = 5;
                else if (value > 75 && value <= 115)
                    grade = 4;
                else if (value > 55 && value <= 75)
                    grade = 3;
                else if (value > 35 && value <= 55)
                    grade = 2;
                else if (value <= 35)
                    grade = 1;
            }
            return grade;
        }
    }
}
