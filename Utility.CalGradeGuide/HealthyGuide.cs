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
    public class HealthyGuide
    {
        private Database m_Database;
        private List<string> m_Type;    //要素对应表名
        private List<int> PERIOD;    //要素对应表名
        private DataTable StationTable;             //站点表
        private DataTable dtCalData_HealthyForecast;//指引表
        private string m_Month;
        private double m_Temp;
        /// <summary>
        /// 枚举污染物编号
        /// </summary>
        enum Itemid
        {
            PM25 = 1,
            PM10 = 2,
            NO2 = 3,
            O3 = 4,
            O38H = 5,
            CO = 6,
            SO2 = 7,
        };
        /// <summary>
        /// 枚举污染物编号
        /// </summary>
        enum Weatherid
        {
            温度 = 9,
            高温 = 10,
            低温 = 11,
            湿度 = 8,
            风速 = 12,
        };
        public HealthyGuide(Database database)
        {
            m_Database = database;

            StationTable = m_Database.GetDataTable("select station_co FROM sta_reg_set where province ='上海市'");
            dtCalData_HealthyForecast = DTOperation.CreateVoidDT(m_Database, "T_HealthyForecast");
            m_Type = new List<string>();
            m_Type.Add("中暑");
            m_Type.Add("儿童感冒");
            m_Type.Add("成人感冒");
            m_Type.Add("老人感冒");
            m_Type.Add("儿童哮喘");
            m_Type.Add("COPD");
            m_Month = DateTime.Now.Month.ToString().PadLeft(2,'0');

            PERIOD = new List<int>();
            PERIOD.Add(24);
            PERIOD.Add(48);
            PERIOD.Add(72);
        }
        //指引数据
        public void InsertData(DateTime forecasttime, string module)
        {
            try
            {
            //模式预报数据
            if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
  FROM [T_ForecastWeather1] where     [durationid]=7 and [ForecastDate]='{1}' and DATEPART(HH,[LST])=20 and  Module='{0}' order by   [ForecastDate] desc", module, forecasttime)) != forecasttime.ToString())
                return;
            if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
  FROM [T_ForecastSite] where  [durationID]=7 and [ForecastDate]='{1}'  and DATEPART(HH,[LST])=20  and  Module='{0}'  order by   [ForecastDate] desc", module, forecasttime, forecasttime.Hour)) != forecasttime.ToString())
                return;
            //中暑等级
            DataTable ZSTable = m_Database.GetDataTable(string.Format(@"(SELECT TGrade,ForecastDate,[station],PERIOD
  FROM T_HealthyGrade WHERE     [ForecastDate]='{0}' and  Type='中暑' and  Period<>0 ) ", forecasttime));
            foreach (int period in PERIOD)
            {
                if (period == 72 && forecasttime.Hour == 8)
                    continue;
                DateTime LST = Convert.ToDateTime(forecasttime.Date.ToString("yyyy-MM-dd 20:00:00")).AddHours(period - 24);
                m_Month = LST.Month.ToString().PadLeft(2, '0');
                //模式污染物数据
                DataTable PollutionTable = m_Database.GetDataTable(string.Format(@"  SELECT site,value,ITEMID 
  FROM [T_ForecastSite] WHERE    [durationID]=7 and [LST]='{0}'   and  Module='{1}'  AND  ForecastDate ='{2}' ", LST, module, forecasttime));
                //前一天气象数据
                DataTable LastWeatherTable = m_Database.GetDataTable(string.Format(@"SELECT [Site]  ,[durationID]  ,[ITEMID],value
  FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'  and [LST]='{0}' and ForecastDate IN (SELECT MAX (ForecastDate) FROM T_ForecastWeather1 WHERE    [durationID]=7 and [LST]='{0}'   and  Module='{1}'  and  ForecastDate<='{2}' )  ", LST.AddDays(-1), module, forecasttime));
                //气象数据
                DataTable WeatherTable = m_Database.GetDataTable(string.Format(@"SELECT [Site]  ,[durationID]  ,[ITEMID],value
  FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'  and [LST]='{0}' and ForecastDate ='{2}' ", LST, module, forecasttime));

                foreach (DataRow Dr in StationTable.Rows)
                {
                    string site = Dr[0].ToString();

                    foreach (string disease in m_Type)
                    {
                        if (disease == "中暑")
                        {
                            try
                            {
                                DataRow[] zsdr = ZSTable.Select("station='" + site + "'  AND PERIOD=" + period);
                                int grade = Convert.ToInt32(zsdr[0][0].ToString());
                                string GuideLines1 = "";
                                switch (grade)
                                {
                                    case 1:
                                        GuideLines1 = "不易中暑,应注意饮食清淡,睡眠充足.";
                                        break;
                                    case 2:
                                        GuideLines1 = "气温较高，可能导致中暑，请注意防暑降温，尽量减少午后或气温较高时长时间在露天环境中活动。";
                                        break;
                                    case 3:
                                        GuideLines1 = "高温天气，较易发生中暑，请注意防暑降温，减少午后或气温较高时在日光下暴晒及在露天环境中活动。";
                                        break;
                                    case 4:
                                        GuideLines1 = "高温炎热，容易发生中暑，请注意采取防暑降温措施，尽量避免午后或高温时段在日光下暴晒及在露天环境中活动。";
                                        break;
                                    case 5:
                                        GuideLines1 = "极度酷热天气，极易发生中暑，请采取积极有效的防暑降温措施，避免在日光下暴晒，避免高温时段或高温环境中的户外活动。";
                                        break;

                                }
                                DataRow newdr = dtCalData_HealthyForecast.NewRow();
                                newdr[2] = forecasttime;
                                newdr[3] = period;
                                newdr[4] = GuideLines1;
                                newdr[6] = site;
                                newdr[7] = disease;
                                newdr[10] = m_Month;
                                dtCalData_HealthyForecast.Rows.Add(newdr);
                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                // 要素阈值代码计算
                                #region
                                string sqlpriority = string.Format("SELECT    [Item]  FROM     D_HealthyGuidelines where Month='{0}' and CHARINDEX(Type,'{1}') > 0 order by  [Top]   ", m_Month, disease);
                                DataTable dtpriority = m_Database.GetDataTable(sqlpriority);

                                Dictionary<string, string> standardcodes = new Dictionary<string, string>();
                                Dictionary<string, string> items = new Dictionary<string, string>();
                                Dictionary<string, double> values = new Dictionary<string, double>();
                                for (int i = 0; i < dtpriority.Rows.Count; i++)
                                {
                                    DataRow dr = dtpriority.Rows[i];
                                    string item = dr[0].ToString();
                                    if (item == "")
                                        continue;
                                    DataRow[] weatherdrs = WeatherTable.Select("Site='" + site + "'");

                                    double value = 0;
                                    switch (item)
                                    {
                                        case "PM2.5":
                                        case "PM10":
                                        case "O3":
                                        case "NO2":
                                        case "SO2":

                                            int I = (int)Enum.Parse(typeof(Itemid), (item == "PM2.5" ? "PM25" : item));
                                            DataRow[] pollutiondrs = PollutionTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                            if (pollutiondrs.Length == 0)
                                                continue;
                                            if (pollutiondrs[0]["Value"].ToString() == "")
                                                continue;
                                            value = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                            GetStandard(value, item, ref  standardcodes, ref  items, ref  values);

                                            break;
                                        case "温度":
                                        case "降温":
                                        case "高温":
                                        case "温差":
                                        case "湿度":
                                        case "风速":
                                            try
                                            {
                                                if (item == "降温")
                                                {
                                                    I = (int)Enum.Parse(typeof(Weatherid), "温度");
                                                    weatherdrs = WeatherTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                                    if (weatherdrs.Length == 0)
                                                        continue;
                                                    value = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                    I = (int)Enum.Parse(typeof(Weatherid), "温度");
                                                    weatherdrs = LastWeatherTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                                    double lastvalue = value;
                                                    try
                                                    {
                                                        lastvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                    }
                                                    catch { }
                                                    value = lastvalue - value;
                                                }
                                                else if (item == "温差")
                                                {
                                                    I = (int)Enum.Parse(typeof(Weatherid), "高温");
                                                    weatherdrs = WeatherTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                                    if (weatherdrs.Length == 0)
                                                        continue;
                                                    double maxvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                    I = (int)Enum.Parse(typeof(Weatherid), "低温");
                                                    weatherdrs = WeatherTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                                    if (weatherdrs.Length == 0)
                                                        continue;
                                                    double minvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                    value = maxvalue - minvalue;

                                                }
                                                else
                                                {
                                                    I = (int)Enum.Parse(typeof(Weatherid), item);
                                                    weatherdrs = WeatherTable.Select("Site='" + site + "'  AND ITEMID=" + I);
                                                    value = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                   
                                                }
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                            }
                                            catch { }
                                            break;
                                    }
                                }
                                #endregion
                                //时间类型代码计算
                                GetGuideLines(standardcodes, disease, items, site, period, forecasttime, "", m_Month);
                            }
                            catch { }
                        }
                    }
                }
            }
            m_Database.Execute("DELETE T_HealthyForecast WHERE  ForecastDate='" + forecasttime + "' and PERIOD!=0");
            DTOperation.InsertToDB(m_Database, dtCalData_HealthyForecast, "T_HealthyForecast");
            dtCalData_HealthyForecast.Clear();
            dtCalData_HealthyForecast.AcceptChanges();
             }
            catch { }
        }
        //订正指引数据
        public void InsertTData(DateTime forecasttime, string module)
        {
            try
            {
                DateTime cityftime = forecasttime;
                if (forecasttime.Hour == 20)
                    cityftime = cityftime.AddHours(12);
                else if (forecasttime.Hour == 8)
                    cityftime = cityftime.AddHours(4);
                //中暑等级
                DataTable ZSTable = m_Database.GetDataTable(string.Format(@"(SELECT TGrade,ForecastDate,[station],PERIOD
  FROM T_HealthyGrade WHERE     [ForecastDate]='{0}' and  Type='中暑' and  Period<>0 ) ", forecasttime));
            bool isPO = false;
            foreach (int period in PERIOD)
            {
                if (period == 72 && forecasttime.Hour == 8)
                    continue;
                DateTime LST = Convert.ToDateTime(forecasttime.Date.ToString("yyyy-MM-dd 20:00:00")).AddHours(period - 24);
                m_Month = LST.Month.ToString().PadLeft(2, '0');
               
                //气象数据前一天
                DataTable LastWeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT id ,[Site]  ,[ITEMID],value ,Module
  FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'   AND  ITEMID   IN (10,11) and [LST]='{0}' and  ForecastDate in (select max(ForecastDate)     FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'   AND  ITEMID   IN (10,11) and [LST]='{0}')   ) union (SELECT id ,[Site]   ,[ITEMID],value,Module FROM [T_ForecastWeather1] WHERE  [durationID]=7    and  Module = 'WRF'  and [LST]='{0}' and ForecastDate  in (select max(ForecastDate)     FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='WRF'   and [LST]='{0}')   )   ", LST.AddDays(-1), module, forecasttime));
                //污染物
                DataTable PollutionTable = m_Database.GetDataTable(string.Format(@"(SELECT [Site]  ,[ITEMID],Value
  FROM [T_ForecastGroup] WHERE    [durationID]=7 and [ForecastDate]='{0}'  and   [Module]='GENERAL'  and PERIOD={1} and  value is not null) ", forecasttime.Date, period));
                //气象数据
                DataTable WeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT id ,[Site]  ,[ITEMID],value,Module
  FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'   AND  ITEMID   IN (10,11) and [LST]='{0}' and ForecastDate ='{2}'   ) union (SELECT id ,[Site]   ,[ITEMID],value,Module FROM [T_ForecastWeather1] WHERE  [durationID]=7    and  Module = 'WRF'  and [LST]='{0}' and ForecastDate  in (select max(ForecastDate)     FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='WRF'   and [LST]='{0}')  )   ", LST, module, cityftime));
               
                    //dt = forecasttime;
                    ////气象数据
                    //for (dt = forecasttime; dt >= forecasttime.AddDays(-4); dt = dt.AddHours(-12))
                    //{
                    //    string sql = string.Format(@"(SELECT id ,[Site]   ,[ITEMID],value FROM [T_ForecastWeather1] WHERE  [durationID]=7    and  Module = 'WRF'  and [LST]='{0}' and ForecastDate ='{2}')   ", LST, module, dt);
                    //    WeatherTable = m_Database.GetDataTable(sql);
                    //    if (WeatherTable.Rows.Count != 0)
                    //        break;

                    //}
//                    WeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT id ,[Site]  ,[ITEMID],value,Module
//  FROM [T_ForecastWeather1] WHERE  [durationID]=7  and  Module='{1}'   AND  ITEMID   IN (10,11) and [LST]='{0}' and ForecastDate ='{2}'   ) union (SELECT id ,[Site]   ,[ITEMID],value,Module FROM [T_ForecastWeather1] WHERE  [durationID]=7    and  Module = 'WRF'  and [LST]='{0}' and ForecastDate ='{3}' )   ", LST, module, forecasttime.AddHours(12), dt));

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
                        foreach (string disease in m_Type)
                        {
                            try
                            {
                                if (disease == "中暑")
                                {
                                    try
                                    {
                                        DataRow[] zsdr = ZSTable.Select("station='" + site + "'  AND PERIOD=" + period);
                                        int grade = Convert.ToInt32(zsdr[0][0].ToString());
                                        string GuideLines1 = "";
                                        switch (grade)
                                        {
                                            case 1:
                                                GuideLines1 = "不易中暑,应注意饮食清淡,睡眠充足.";
                                                break;
                                            case 2:
                                                GuideLines1 = "气温较高，可能导致中暑，请注意防暑降温，尽量减少午后或气温较高时长时间在露天环境中活动。";
                                                break;
                                            case 3:
                                                GuideLines1 = "高温天气，较易发生中暑，请注意防暑降温，减少午后或气温较高时在日光下暴晒及在露天环境中活动。";
                                                break;
                                            case 4:
                                                GuideLines1 = "高温炎热，容易发生中暑，请注意采取防暑降温措施，尽量避免午后或高温时段在日光下暴晒及在露天环境中活动。";
                                                break;
                                            case 5:
                                                GuideLines1 = "极度酷热天气，极易发生中暑，请采取积极有效的防暑降温措施，避免在日光下暴晒，避免高温时段或高温环境中的户外活动。";
                                                break;

                                        }
                                        DataRow newdr = dtCalData_HealthyForecast.NewRow();
                                        newdr[2] = forecasttime;
                                        newdr[3] = period;
                                        newdr[4] = GuideLines1;
                                        newdr[6] = site;
                                        newdr[7] = disease;
                                        //newdr[8] = values["1"];
                                        //newdr[9] = values["2"];
                                        newdr[10] = m_Month;
                                        newdr[11] = "Up";
                                        dtCalData_HealthyForecast.Rows.Add(newdr);
                                    }
                                    catch { }

                                }
                                else
                                {

                                    // 要素阈值代码计算
                                    #region
                                    string sqlpriority = string.Format("SELECT    [Item]  FROM         D_HealthyGuidelines where Month='{0}' and CHARINDEX(Type,'{1}') > 0 order by  [Top]   ", m_Month, disease);
                                    DataTable dtpriority = m_Database.GetDataTable(sqlpriority);
                                    Dictionary<string, string> standardcodes = new Dictionary<string, string>();
                                    Dictionary<string, string> items = new Dictionary<string, string>();
                                    Dictionary<string, double> values = new Dictionary<string, double>();
                                    for (int i = 0; i < dtpriority.Rows.Count; i++)
                                    {
                                        DataRow dr = dtpriority.Rows[i];
                                        string item = dr[0].ToString();
                                        if (item == "")
                                            continue;
                                        DataRow[] weatherdrs = WeatherTable.Select("Site='" + site + "'");
                                        try
                                        {
                                            double value = 0;
                                            switch (item)
                                            {
                                                case "PM2.5":
                                                case "PM10":
                                                case "O3":
                                                case "NO2":
                                                case "SO2":
                                                    int I = (int)Enum.Parse(typeof(Itemid), (item == "PM2.5" ? "PM25" : item));
                                                    DataRow[] pollutiondrs = PollutionTable.Select("Site='58637' " + " AND  ITEMID=" + I);
                                                    if (isPO)
                                                        pollutiondrs = PollutionTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                    if (pollutiondrs.Length == 0)
                                                        continue;
                                                    if (pollutiondrs[0]["Value"].ToString() == "")
                                                        continue;
                                                    value = Convert.ToDouble(pollutiondrs[0]["Value"].ToString());
                                                    GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                    break;
                                                case "温度":
                                                case "降温":
                                                case "高温":
                                                case "温差":
                                                case "湿度":
                                                case "风速":
                                                    if (item == "降温")
                                                    {
                                                        I = (int)Enum.Parse(typeof(Weatherid), "温度");
                                                        weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                        if (weatherdrs.Length == 0)
                                                            continue;
                                                        value = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                        I = (int)Enum.Parse(typeof(Weatherid), "温度");
                                                        weatherdrs = LastWeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                        double lastvalue = value;
                                                        try
                                                        {
                                                            lastvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                        }
                                                        catch { }
                                                        value = lastvalue - value;
                                                    }
                                                    else if (item == "温差")
                                                    {
                                                        I = (int)Enum.Parse(typeof(Weatherid), "高温");
                                                        weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                        if (weatherdrs.Length == 0)
                                                            continue;
                                                        double maxvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                        I = (int)Enum.Parse(typeof(Weatherid), "低温");
                                                        weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                        if (weatherdrs.Length == 0)
                                                            continue;
                                                        double minvalue = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                        value = maxvalue - minvalue;

                                                    }
                                                    else
                                                    {
                                                        I = (int)Enum.Parse(typeof(Weatherid), item);
                                                        weatherdrs = WeatherTable.Select("Site='" + site + "'" + " AND  ITEMID=" + I);
                                                        value = Convert.ToDouble(weatherdrs[0]["Value"].ToString());
                                                        if (item == "温度")
                                                            m_Temp = value;
                                                    }
                                                    GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                    break;
                                            }
                                        }
                                        catch { }
                                    }
                                    #endregion

                                    GetGuideLines(standardcodes, disease, items, site, period, forecasttime, "Up", m_Month);
                                }
                            }
                            catch { }
                        }

                    }
                    catch { }
                }
            }
            m_Database.Execute("DELETE T_HealthyForecast WHERE  ForecastDate='" + forecasttime + "' and PERIOD!=0");
            DTOperation.InsertToDB(m_Database, dtCalData_HealthyForecast, "T_HealthyForecast");
            dtCalData_HealthyForecast.Clear();
            dtCalData_HealthyForecast.AcceptChanges();
			 }
            catch { }
        }
        //实况指引数据
        public void InsertAaData(DateTime forecasttime)
        {
            try
            {
                if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
                  FROM T_HealthyForecast where  PERIOD=0 and    ForecastDate='{0}' ", forecasttime)) == forecasttime.ToString())
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

                //string insertsql = "delete T_24ShAQI where time_point = '" + forecasttime + "';insert into T_24ShAQI select * from  (" + selectsql + ") as a ";
                //m_Database.Execute(insertsql);

                DataTable PollutionTable = m_Database.GetDataTable(selectsql);

                //中暑等级
                DataTable ZSTable = m_Database.GetDataTable(string.Format(@"(SELECT TGrade,ForecastDate,[station],PERIOD
  FROM T_HealthyGrade WHERE     [ForecastDate]='{0}' and  Type='中暑' and  Period=0 ) ", forecasttime));

                int period = 0;

                DateTime LST = forecasttime;
                    m_Month = LST.Month.ToString().PadLeft(2, '0');
                  
                    //前一天气象数据
                    DataTable LastWeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT  [station]  ,temperature  as Temp,[maxtemperature] as [MaxTemp]  ,[relativehumidity] as [RH],mintemperature as MinTemp ,wind_2min_speed as Wind  FROM T_Cimiss_Day WHERE   [collect_time] ='{0}'    ) ", LST.AddDays(-1)));

                    //气象数据
                    DataTable WeatherTable = m_Database.GetDataTable(string.Format(@"(SELECT  [station]  ,temperature  as Temp,[maxtemperature] as [MaxTemp]  ,[relativehumidity] as [RH],mintemperature as MinTemp ,wind_2min_speed as Wind  FROM T_Cimiss_Day WHERE   [collect_time] ='{0}'    ) ", LST));

                    foreach (string site in shstation.Keys)
                    {
                        //string site = Dr[0].ToString();

                        foreach (string disease in m_Type)
                        {
                            if (disease == "中暑")
                            {
                                #region
                                try
                                {
                                    DataRow[] zsdr = ZSTable.Select("station='" + site + "'");
                                    int grade = Convert.ToInt32(zsdr[0][0].ToString());
                                    string GuideLines1 = "";
                                    switch (grade)
                                    {
                                        case 1:
                                            GuideLines1 = "不易中暑,应注意饮食清淡,睡眠充足.";
                                            break;
                                        case 2:
                                            GuideLines1 = "气温较高，可能导致中暑，请注意防暑降温，尽量减少午后或气温较高时长时间在露天环境中活动。";
                                            break;
                                        case 3:
                                            GuideLines1 = "高温天气，较易发生中暑，请注意防暑降温，减少午后或气温较高时在日光下暴晒及在露天环境中活动。";
                                            break;
                                        case 4:
                                            GuideLines1 = "高温炎热，容易发生中暑，请注意采取防暑降温措施，尽量避免午后或高温时段在日光下暴晒及在露天环境中活动。";
                                            break;
                                        case 5:
                                            GuideLines1 = "极度酷热天气，极易发生中暑，请采取积极有效的防暑降温措施，避免在日光下暴晒，避免高温时段或高温环境中的户外活动。";
                                            break;

                                    }
                                    DataRow newdr = dtCalData_HealthyForecast.NewRow();
                                    newdr[2] = forecasttime;
                                    newdr[3] = period;
                                    newdr[4] = GuideLines1;
                                    newdr[6] = site;
                                    newdr[7] = disease;
                                    newdr[10] = m_Month;
                                    dtCalData_HealthyForecast.Rows.Add(newdr);
                                }
                                catch { }
                                #endregion
                            }
                            else
                            {
                                try
                                {
                                    // 要素阈值代码计算
                                    #region
                                    string sqlpriority = string.Format("SELECT    [Item]  FROM     D_HealthyGuidelines where Month='{0}' and CHARINDEX(Type,'{1}') > 0 order by  [Top]   ", m_Month, disease);
                                    DataTable dtpriority = m_Database.GetDataTable(sqlpriority);

                                    Dictionary<string, string> standardcodes = new Dictionary<string, string>();
                                    Dictionary<string, string> items = new Dictionary<string, string>();
                                    Dictionary<string, double> values = new Dictionary<string, double>();
                                    DataRow[] pollutiondrs = PollutionTable.Select("station_code='" + site + "'");
                                    DataRow[] weatherdrs = WeatherTable.Select("station='" + site + "'");
                                    double rh = Convert.ToDouble(weatherdrs[0][3]);
                                    double temp = Convert.ToDouble(weatherdrs[0][1]);
                                    double maxtemp = Convert.ToDouble(weatherdrs[0][2]);
                                    double pm10 = 0, pm25 = 0, o3 = 0, o38h = 0, no2 = 0, so2 = 0;
                                    pm25 = Convert.ToDouble(pollutiondrs[0]["pm25"].ToString());
                                    o3 = Convert.ToDouble(pollutiondrs[0]["o3"].ToString());
                                    pm10 = Convert.ToDouble(pollutiondrs[0]["pm10"].ToString());
                                    no2 = Convert.ToDouble(pollutiondrs[0]["no2"].ToString());
                                    o38h = Convert.ToDouble(pollutiondrs[0]["o38h"].ToString());
                                    so2 = Convert.ToDouble(pollutiondrs[0]["so2"].ToString());
                                    for (int i = 0; i < dtpriority.Rows.Count; i++)
                                    {
                                        DataRow dr = dtpriority.Rows[i];
                                        string item = dr[0].ToString();
                                        if (item == "")
                                            continue;
                                       
                                        
                                        double value = 0;
                                        switch (item)
                                        {
                                            case "PM2.5":
                                                value = pm25;
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                break;
                                            case "PM10":
                                                value = pm10;
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                break;
                                            case "O3":
                                                value = o3;
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                break;
                                            case "NO2":
                                                value = no2;
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                break;
                                            case "SO2":
                                                value = so2;
                                                GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                break;

                                            case "温度":
                                            case "降温":
                                            case "高温":
                                            case "温差":
                                            case "湿度":
                                            case "风速":
                                                try
                                                {
                                                    weatherdrs = WeatherTable.Select("station='" + site + "'");
                                                    if (weatherdrs.Length == 0)
                                                        continue;
                                                    if (item == "降温")
                                                    {
                                                        value = Convert.ToDouble(weatherdrs[0]["Temp"]);
                                                        weatherdrs = LastWeatherTable.Select("station='" + site + "'");
                                                        double lastvalue = value;
                                                        try
                                                        {
                                                            lastvalue = Convert.ToDouble(weatherdrs[0]["Temp"]);
                                                        }
                                                        catch { }
                                                        value = lastvalue - value;
                                                    }
                                                    else if (item == "温差")
                                                    {
                                                        double maxvalue = Convert.ToDouble(weatherdrs[0]["MaxTemp"].ToString());
                                                        double minvalue = Convert.ToDouble(weatherdrs[0]["MinTemp"].ToString());
                                                        value = maxvalue - minvalue;

                                                    }
                                                    else if (item == "温度")
                                                    {
                                                        value  = Convert.ToDouble(weatherdrs[0]["Temp"].ToString());
                                                        
                                                    }
                                                    else if (item == "高温")
                                                    {
                                                        value = Convert.ToDouble(weatherdrs[0]["MaxTemp"].ToString());
                                                    }
                                                    else if (item == "湿度")
                                                    {
                                                        value = Convert.ToDouble(weatherdrs[0]["RH"].ToString());
                                                    }
                                                    else if (item == "风速")
                                                    {
                                                        value = Convert.ToDouble(weatherdrs[0]["Wind"].ToString());
                                                    }
                                                    GetStandard(value, item, ref  standardcodes, ref  items, ref  values);
                                                }
                                                catch { }
                                                break;
                                        }
                                    }
                                    #endregion
                                    //时间类型代码计算
                                    GetGuideLines(standardcodes, disease, items, site, period, forecasttime, "", m_Month);
                                }
                                catch { }
                            }
                        }
                    }
                
                m_Database.Execute("DELETE T_HealthyForecast WHERE  ForecastDate='" + forecasttime + "' and  Period=0");
                DTOperation.InsertToDB(m_Database, dtCalData_HealthyForecast, "T_HealthyForecast");
                dtCalData_HealthyForecast.Clear();
                dtCalData_HealthyForecast.AcceptChanges();
            }
            catch { }
        }
        //获取符合条件的代码
        private void GetStandard(double value, string item, ref Dictionary<string, string> standardcodes ,ref  Dictionary<string, string> items, ref Dictionary<string, double> values)
        {
            DataTable standard = m_Database.GetDataTable(string.Format(@"SELECT  DM, MC, DP, DvF, DvT, Code
FROM         D_HealthyStandard where Code='{0}' ", item));
            string weadm = "";
            foreach (DataRow standarddr in standard.Rows)
            {
                string strdvf = standarddr["DvF"].ToString();
                double dvf = strdvf.Contains("]") ? Convert.ToDouble(strdvf.Substring(0, strdvf.Length - 1)) : Convert.ToDouble(strdvf);
                string strdvt = standarddr["DvT"].ToString();
                double dvt = strdvt.Contains("]") ? Convert.ToDouble(strdvt.Substring(0, strdvt.Length - 1)) : Convert.ToDouble(strdvt);
                if (strdvf.Contains("]") && strdvt.Contains("]"))
                {
                    if (value >= dvf && value <= dvt)
                        {
                        weadm = standarddr["DM"].ToString();
                        items.Add((items.Count + 1).ToString(), item);
                        standardcodes.Add((standardcodes.Count + 1).ToString(), weadm);
                        values.Add((values.Count + 1).ToString(), value);
                       
                    }
                }
                else if (strdvt.Contains("]"))
                {
                    if (value > dvf && value <= dvt)
                    {
                        weadm = standarddr["DM"].ToString();
                        items.Add((items.Count + 1).ToString(), item);
                        standardcodes.Add((standardcodes.Count + 1).ToString(), weadm);
                        values.Add((values.Count + 1).ToString(), value);

                        
                    }
                }
                else if (strdvf.Contains("]"))
                {
                    if (value >= dvf && value < dvt)
                    {
                        weadm = standarddr["DM"].ToString();
                        items.Add((items.Count + 1).ToString(), item);
                        standardcodes.Add((standardcodes.Count + 1).ToString(), weadm);
                        values.Add((values.Count + 1).ToString(), value);

                      
                    }
                }
                else
                {
                    if (value > dvf && value < dvt)
                    {
                        weadm = standarddr["DM"].ToString();
                        items.Add((items.Count + 1).ToString(), item);
                        standardcodes.Add((standardcodes.Count + 1).ToString(), weadm);
                        values.Add((values.Count + 1).ToString(), value);
                    }
                }
            }
           
        }
        private void GetGuideLines(Dictionary<string, string> standardcodes, string disease, Dictionary<string, string> items, string site, int period, DateTime forecasttime, string T,string month)
        {
            //时间类型代码计算
            string[] poitem = new string[] { "PM2.5", "PM10", "O3", "SO2", "NO2" };
            string[] weaitem = new string[] { "温度", "降温", "高温", "温差", "湿度", "风速" };
            Dictionary<string, string> PoGuideLines = new Dictionary<string, string>();
            Dictionary<string, string> WeaGuideLines = new Dictionary<string, string>();
            Dictionary<string, string> PoCode = new Dictionary<string, string>();
            Dictionary<string, string> WeaCode = new Dictionary<string, string>();
            foreach (string key in standardcodes.Keys)
            {
                DataTable timetypetable = m_Database.GetDataTable(string.Format("select  dm FROM [D_HealthyMonths] where  Code='{0}' and  CHARINDEX('{1}',[MC])>0", items[key], month));
                foreach (DataRow dr in timetypetable.Rows)
                {
                    //时间类型代码计算
                    string timetype = dr["dm"].ToString();
                    string tempguideLines = m_Database.GetFirstValue(string.Format("SELECT  top 1 GuideLines1+GuideLines2 FROM  T_HealthyGuidelines where  Standard ='{0}' and Months='{1}' and   Type='{2}'", standardcodes[key], timetype, disease));

                    if (tempguideLines != "")
                    {
                        if (poitem.Contains(items[key]))
                        {
                            PoGuideLines.Add(items[key], tempguideLines);
                            PoCode.Add(items[key], timetype + "&" + standardcodes[key]);
                        }
                        if (WeaGuideLines.Keys.Count(p => p.Contains("温")) == 0 && weaitem.Contains(items[key]))
                        {
                            WeaGuideLines.Add(items[key], tempguideLines);
                            WeaCode.Add(items[key], timetype + "&" + standardcodes[key]);
                        }

                    }
                }
            }
            string item1 = "";
            string item2 = "";
            string GuideLines1 = "";
            string GuideLines2 = "";
            string standard1 = "";
            string standard2 = "";
            if (PoGuideLines.Count > 0)
            {
                item1 = PoGuideLines.Keys.ElementAt(0);
                GuideLines1 = PoGuideLines.Values.ElementAt(0);
                if (WeaGuideLines.Count > 0)
                {
                    item2 = WeaGuideLines.Keys.ElementAt(0);
                    GuideLines2 = WeaGuideLines.Values.ElementAt(0);
                }
            }
            else if (WeaGuideLines.Count > 0)
            {
                item1 = WeaGuideLines.Keys.ElementAt(0);
                GuideLines1 = WeaGuideLines.Values.ElementAt(0);
                standard1 = WeaCode.Keys.ElementAt(0);
                if (WeaGuideLines.Count > 1)
                {
                    item2 = WeaGuideLines.Keys.ElementAt(1);
                    GuideLines2 = WeaGuideLines.Values.ElementAt(1);
                    standard2 = WeaCode.Keys.ElementAt(1);
                }
               string addlines= ExtraGuideLines(WeaCode, disease);
               GuideLines2 += addlines;
            }
            DataRow newdr = dtCalData_HealthyForecast.NewRow();
            newdr[0] = item1;
            newdr[1] = item2;
            newdr[2] = forecasttime;
            newdr[3] = period;
            newdr[4] = GuideLines1;
            newdr[5] = GuideLines2;
            newdr[6] = site;
            newdr[7] = disease;
            newdr[10] = m_Month;
            newdr[11] = T;
            dtCalData_HealthyForecast.Rows.Add(newdr);
        
        }

        private string ExtraGuideLines(Dictionary<string, string> weacode,string disease)
        {
            string addguidelines = "";
            if (weacode.Keys.Contains("温度") && weacode.Count <= 2)
            {
                if (weacode.Count == 1 || weacode.Keys.Contains("湿度"))
                {
                    bool isdo = false;
                    //                    12<温度≤16	12月，1月-2月
                    //16<温度≤19	3月-5月
                    //19<温度≤22	1月-12月
                    int month = Convert.ToInt32(m_Month);
                    if ((m_Temp > 12 && m_Temp <= 16) && !(month>=3 && month <=11))
                        isdo = true;
                    if ((m_Temp > 16 && m_Temp <= 19) && (month >=3&& month <= 5))
                        isdo = true;
                    if (m_Temp > 19 && m_Temp <= 22)
                        isdo = true;
                    if (isdo)
                        switch (disease)
                        {
                            case "儿童感冒":
                                addguidelines = "可适当进行体育锻炼。";
                                break;
                            case "成人感冒":
                                addguidelines = "可适当锻炼。";
                                break;
                            case "老人感冒":
                                addguidelines = "可适当进行轻体力活动和锻炼。";
                                break;
                            case "儿童哮喘":
                                addguidelines = "可适当锻炼。";
                                break;
                            case "COPD":
                                addguidelines = "可适当进行轻体力活动。";
                                break;
                        }
                }
            }
            return addguidelines;
        
        }
    }
}
