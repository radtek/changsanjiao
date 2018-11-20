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
    public class SceneryGrade
    {
        private Database m_Database;
        private List<string> m_Type;    //要素对应表名
        private List<int> PERIOD;
        private DataTable StationTable;             //站点表
        private DataTable dtCalData_T_ScenAirForecast;//指引表
        enum Itemid
        {
            PM25 = 1,
            PM10 = 2,
            NO2 = 3,
            O3 = 4,
            O38h = 5,
            CO = 6,
            SO2 = 7

        };
        public SceneryGrade(Database database)
        {
            m_Database = database;

            StationTable = m_Database.GetDataTable("(select station_co FROM sta_reg_set where flag =1002)");
            dtCalData_T_ScenAirForecast = DTOperation.CreateVoidDT(m_Database, "T_ScenAirForecast");
            m_Type = new List<string>();
            m_Type.Add("霾指数");
            m_Type.Add("天气分指数");
            m_Type.Add("空气质量分指数");
            m_Type.Add("综合观景指数");
            m_Type.Add("空气清洁度指数");

            PERIOD = new List<int>();
            PERIOD.Add(0);
            PERIOD.Add(24);
            PERIOD.Add(48);
        }
        //指引数据
        public void InsertData(DateTime forecasttime)
        {
            try
            {

                //            if (m_Database.GetFirstValue(string.Format(@"SELECT top 1 ForecastDate
                //  FROM T_ScenAirForecast where  ForecastDate ='{0}'  order by   [ForecastDate] desc", forecasttime)) == forecasttime.ToString())
                //                return;


                foreach (DataRow Dr in StationTable.Rows)
                {
                    string site = Dr[0].ToString();
                    try
                    {
                        foreach (int period in PERIOD)
                        {
                            BaseDatas basedata = GetBaseData(forecasttime, period, site);
                            foreach (string type in m_Type)
                            {
                                try
                                {
                                    if (type == "霾指数")
                                    {
                                        //霾数据
                                        
                                        int hazegrade = basedata.hazegrade;
                                        DataTable result = CalHazeGrade(hazegrade, forecasttime, period, site, type, false);
                                        foreach (DataRow row in result.Rows)
                                        {
                                            dtCalData_T_ScenAirForecast.ImportRow(row);
                                        }

                                    }
                                    else if (type == "天气分指数")
                                    {
                                        //气象数据

                                        double wind = basedata.wind;

                                        double temp = basedata.temp;

                                        double rain = basedata.rain;

                                        double cldf = basedata.cldf;

                                        DataTable result = CalMetGrade(rain, temp, wind, cldf, forecasttime, period, site, type, false);
                                        foreach (DataRow row in result.Rows)
                                        {
                                            dtCalData_T_ScenAirForecast.ImportRow(row);
                                        }
                                    }
                                    else if (type == "空气质量分指数" || type == "空气清洁度指数")
                                    {
                                        int aqi = basedata.aqi;
                                        string items = basedata.items;
                                   
                                        if (type == "空气质量分指数")
                                        {
                                            DataTable result = CalPollutionGrade(aqi, items, forecasttime, period, site, type, false);
                                            foreach (DataRow row in result.Rows)
                                            {
                                                dtCalData_T_ScenAirForecast.ImportRow(row);
                                            }
                                        }
                                        else if (type == "空气清洁度指数")
                                        {

                                            double wind = basedata.wind;

                                            DataTable result = CalClPollutionGrade(aqi, wind, forecasttime, period, site, type, false);
                                            foreach (DataRow row in result.Rows)
                                            {
                                                dtCalData_T_ScenAirForecast.ImportRow(row);
                                            }
                                        }
                                    }
                                    else if (type == "综合观景指数")
                                    {
                                        DataRow[] gradedrs = dtCalData_T_ScenAirForecast.Select(string.Format("PERIOD={0} and  station='{1}' and Type='{2}' ", period, site, "天气分指数"));
                                        //天气分指数
                                        int qgrade = Convert.ToInt32(gradedrs[0]["Grade"]);

                                        gradedrs = dtCalData_T_ScenAirForecast.Select(string.Format("PERIOD={0} and  station='{1}' and Type='{2}' ", period, site, "霾指数"));
                                        //霾分指数
                                        int hgrade = Convert.ToInt32(gradedrs[0]["Grade"]);

                                        gradedrs = gradedrs = dtCalData_T_ScenAirForecast.Select(string.Format("PERIOD={0} and  station='{1}' and Type='{2}' ", period, site, "空气质量分指数"));
                                        //环境分指数
                                        int pgrade = Convert.ToInt32(gradedrs[0]["Grade"]);
                                        DataTable result = CalPoGroupGrade(qgrade, hgrade, pgrade, forecasttime, period, site, type, false);
                                        foreach (DataRow row in result.Rows)
                                        {
                                            dtCalData_T_ScenAirForecast.ImportRow(row);
                                        }
                                    }
                                }
                                catch { }
                            }
                        }

                    }
                    catch { }
                }
                m_Database.Execute("DELETE T_ScenAirForecast WHERE  ForecastDate='" + forecasttime + "'");
                DTOperation.InsertToDB(m_Database, dtCalData_T_ScenAirForecast, "T_ScenAirForecast");
                dtCalData_T_ScenAirForecast.Clear();
                dtCalData_T_ScenAirForecast.AcceptChanges();
            }
            catch { }
        }
        /// <summary>
        /// 观景指数
        /// </summary>
        /// <param name="qgrade">天气分指数</param>
        /// <param name="hgrade">霾分指数</param>
        /// <param name="pgrade">空气质量分指数</param>
        /// <param name="forecasttime">时间</param>
        /// <param name="period">时效</param>
        /// <param name="site">站点号</param>
        /// <param name="disease">类型</param>
        /// <param name="istodb">是否入库</param>
        public  DataTable CalPoGroupGrade(int qgrade, int hgrade, int pgrade, DateTime forecasttime, int period, string site, string disease, bool istodb)
        {
            int Grade = 0;
            if (qgrade == 1)
            {
                if (hgrade == 1 && pgrade == 1)
                    Grade = 1;
                else if (hgrade == 1 && pgrade <= 3)
                    Grade = 2;
                else if (hgrade == 2 && pgrade <= 3)
                    Grade = 2;
                else if (hgrade == 3 && pgrade == 1)
                    Grade = 2;
                else if (hgrade == 3 && pgrade >= 2)
                    Grade = 3;
            }
            else if (qgrade == 2)
            {
                if (hgrade == 1 && pgrade == 1)
                    Grade = 1;
                else if (hgrade == 1 && pgrade == 2)
                    Grade = 2;
                else if (hgrade == 1 && pgrade == 3)
                    Grade = 3;
                else if (hgrade == 2 && pgrade <= 2)
                    Grade = 2;
                else if (hgrade == 2 && pgrade == 3)
                    Grade = 3;
                else if (hgrade == 3 && pgrade == 1)
                    Grade = 2;
                else if (hgrade == 3 && pgrade >= 2)
                    Grade = 3;
            }
            else if (qgrade == 3)
            {
                if (hgrade == 1 && pgrade == 1)
                    Grade = 2;
                else if (hgrade == 1 && pgrade >= 2)
                    Grade = 3;
                else if (hgrade == 2 && pgrade == 1)
                    Grade = 2;
                else if (hgrade == 2 && pgrade >= 2)
                    Grade = 2;
                else if (hgrade == 3 && pgrade <= 2)
                    Grade = 3;
                else if (hgrade == 3 && pgrade == 3)
                    Grade = 4;

            }
            if (qgrade == 4 || hgrade == 4 || pgrade == 4)
                Grade = 4;
            return GetGuidlines(forecasttime, period, site, disease, new List<string>(), new List<string>(), Grade, istodb);

        }

        /// <summary>
        /// 计算 空气质量 分指数
        /// </summary>
        /// <param name="aqi"></param>
        /// <param name="items">首要污染物</param>
        /// <param name="forecasttime">时间</param>
        /// <param name="period">时效</param>
        /// <param name="site">站点号</param>
        /// <param name="disease">类型</param>
        /// <param name="istodb">是否入库</param>
        public DataTable CalPollutionGrade(int aqi, string items, DateTime forecasttime, int period, string site, string disease, bool istodb)
        {
            int primarycode = 0;
            if (aqi > 100)
            {
                if (items.Contains("PM2") && items.Contains("O3"))
                    primarycode = 2;
                else if (items.Contains("PM2"))
                    primarycode = 1;
                else
                    primarycode = 0;
            }
            else if (aqi > 50)
                primarycode = 4;
            else
                primarycode = 3;

            List<string> srandards1 = new List<string>();
            List<string> srandards2 = new List<string>();
            int grade = 0;

            srandards1.Add(GetStandard(aqi, "AQI").Split('&')[0]);
            srandards2.Add(GetStandard(primarycode, "首要污染物").Split('&')[0]);

            return GetGuidlines(forecasttime, period, site, disease, srandards1, srandards2, grade, istodb);
        }
        /// <summary>
        /// 计算 空气清洁度指数
        /// </summary>
        /// <param name="aqi"></param>
        /// <param name="wind">风速</param>
        /// <param name="forecasttime">时间</param>
        /// <param name="period">时效</param>
        /// <param name="site">站点号</param>
        /// <param name="disease">类型</param>
        /// <param name="istodb">是否入库</param>
        public DataTable CalClPollutionGrade(int aqi, double wind, DateTime forecasttime, int period, string site, string disease, bool istodb)
        {

            List<string> srandards1 = new List<string>();
            List<string> srandards2 = new List<string>();
            int grade = 0;
            srandards1.Add(GetStandard(wind, "风速").Split('&')[0]);
            srandards2.Add(GetStandard(aqi, "AQI").Split('&')[0]);

            return GetGuidlines(forecasttime, period, site, disease, srandards1, srandards2, grade, istodb);
        }
        /// <summary>
        /// 计算天气分指数需求
        /// </summary>
        /// <param name="rain">雨</param>
        /// <param name="temp">温度</param>
        /// <param name="wind">风</param>
        /// <param name="cldf">云量</param>
        /// <param name="forecasttime"></param>
        /// <param name="period"></param>
        /// <param name="site"></param>
        /// <param name="disease"></param>
        public DataTable CalMetGrade(double rain, double temp, double wind, double cldf, DateTime forecasttime, int period, string site, string disease, bool istodb)
        {

            int m = CalM(rain, temp, wind);
            List<string> srandards1 = new List<string>();
            List<string> srandards2 = new List<string>();
            int grade = 0;
            srandards1.Add(GetStandard(m, "M").Split('&')[0]);
            srandards2.Add(GetStandard(wind, "风速").Split('&')[0]);
            srandards2.Add(GetStandard(temp, "温度").Split('&')[0]);
            srandards2.Add(GetStandard(rain, "降水").Split('&')[0]);
            srandards2.Add(GetStandard(cldf, "云").Split('&')[0]);

            return GetGuidlines(forecasttime, period, site, disease, srandards1, srandards2, grade, istodb);

        }
        /// <summary>
        /// 计算 霾分指数
        /// </summary>
        /// <param name="hazegrade">霾等级</param>
        /// <param name="forecasttime"></param>
        /// <param name="period"></param>
        /// <param name="site"></param>
        /// <param name="disease"></param>
        public  DataTable CalHazeGrade(double hazegrade, DateTime forecasttime, int period, string site, string disease, bool istodb)
        {
            DataTable result = DTOperation.CreateVoidDT(m_Database, "T_ScenAirForecast");
            //string dm = GetStandard(hazegrade, "霾等级").Split('&')[0];

            List<string> srandards1 = new List<string>();
            List<string> srandards2 = new List<string>();
            int grade = 0;
            srandards1.Add(GetStandard(hazegrade, "霾等级").Split('&')[0]);
            srandards2.Add("00");

            return GetGuidlines(forecasttime, period, site, disease, srandards1, srandards2, grade, istodb);

        }
        //获取符合条件的代码
        public string GetStandard(double value, string item)
        {
            string sqlp = string.Format("SELECT * from [D_ScenAirStandard] where  Code ='{0}'", item);
            DataTable standards = m_Database.GetDataTable(sqlp);
            string podm = "", pomc = "";

            foreach (DataRow tempdr in standards.Rows)
            {
                string strdvf = tempdr["DvF"].ToString();
                double dvf = strdvf.Contains("]") ? Convert.ToDouble(strdvf.Substring(0, strdvf.Length - 1)) : Convert.ToDouble(strdvf);
                string strdvt = tempdr["DvT"].ToString();
                double dvt = strdvt.Contains("]") ? Convert.ToDouble(strdvt.Substring(0, strdvt.Length - 1)) : Convert.ToDouble(strdvt);
                if (strdvf.Contains("]") && strdvt.Contains("]"))
                {
                    if (value >= dvf && value <= dvt)
                    {
                        podm = tempdr["DM"].ToString();
                        pomc = tempdr["MC"].ToString();
                    }
                }
                else if (strdvt.Contains("]"))
                {
                    if (value > dvf && value <= dvt)
                    {
                        podm = tempdr["DM"].ToString();
                        pomc = tempdr["MC"].ToString();
                    }
                }
                else if (strdvf.Contains("]"))
                {
                    if (value >= dvf && value < dvt)
                    {
                        podm = tempdr["DM"].ToString();
                        pomc = tempdr["MC"].ToString();
                    }
                }
                else
                    if (value > dvf && value < dvt)
                    {
                        podm = tempdr["DM"].ToString();
                        pomc = tempdr["MC"].ToString();
                    }

            }
            return podm + "&" + pomc;
        }
        //计算空气质量指数M值 
        private int CalM(double rain, double temp, double wind)
        {
            int m = 0;
            int coderain = Convert.ToInt32(GetStandard(rain, "降水").Split('&')[1]);
            int codetemp = Convert.ToInt32(GetStandard(temp, "温度").Split('&')[1]);
            int codewind = Convert.ToInt32(GetStandard(wind, "风速").Split('&')[1]);
            m = coderain * codetemp * codewind;
            return m;
        }

        /// 计算服务标语
        private DataTable GetGuidlines(DateTime forecasttime, int period, string site, string disease, List<string> srandards1, List<string> srandards2, int grade, bool istodb)
        {
            DataTable result = DTOperation.CreateVoidDT(m_Database, "T_ScenAirForecast");
            string GuideLines1 = "";
            string GuideLines2 = "";
            foreach (string item1 in srandards1)
            {
                foreach (string item2 in srandards2)
                {
                    string GuideLines = m_Database.GetFirstValue(string.Format("SELECT  GuideLines1+'&'+ GuideLines2 +'&'+ Grade  FROM   T_ScenAirGuidelines where Standard1='{0}' and  Standard2='{1}' ", item1, item2));
                    if (GuideLines != "")
                    {
                        GuideLines1 += GuideLines.Split('&')[0];
                        GuideLines2 += GuideLines.Split('&')[1];
                        int tempgrade = Convert.ToInt32(GuideLines.Split('&')[2]);
                        if (tempgrade > grade)
                            grade = tempgrade;
                        break;
                    }
                }
            }
            DataRow newdr = result.NewRow();
            newdr[0] = forecasttime;
            newdr[1] = forecasttime.Hour == 8 ? period : period + 24;
            newdr[2] = GuideLines1;
            newdr[3] = GuideLines2;
            newdr[4] = site;
            newdr[5] = disease;
            newdr[6] = grade;
            result.Rows.Add(newdr);
            if (istodb)
            {
                if (result.Rows.Count > 0)
                {

                    m_Database.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", forecasttime, period, site, disease));
                    DTOperation.InsertToDB(m_Database, result, "T_ScenAirForecast");
                }
            }
            return result;
        }
        /// <summary>
        /// 获取基础数据
        /// </summary>
        /// <param name="forecasttime">时间</param>
        /// <param name="period">时效</param>
        /// <param name="site">站点</param>
        /// <returns></returns>
        public  BaseDatas GetBaseData(DateTime forecasttime, int period, string site)
        {
            BaseDatas basedata = new BaseDatas();
            basedata.forecasttime = forecasttime;
            basedata.period = period;
            basedata.site = site;
            //霾数据
            DataTable HazeTable = m_Database.GetDataTable(string.Format(@"SELECT Haze FROM [T_Haze] where  LST='{0}' AND  ReTime in  (select  MAX(ReTime) from [T_Haze] where  LST='{0}' and ReTime<='{1}' )", forecasttime.Date.AddHours(period), forecasttime));
            if (HazeTable.Rows.Count > 0)
            {
                int hazegrade = Convert.ToInt32(HazeTable.Rows[0]["Haze"].ToString());
                basedata.hazegrade = hazegrade;
            }

            //气象数据
            string sql = string.Format(@" SELECT Value,[ITEMID] 
  FROM [T_ForecastWeather1] WHERE  [durationid]=7 and [LST]='{0}' and  Module='ZGrid' and  Site='{1}'  and  ForecastDate IN (SELECT MAX (ForecastDate) FROM T_ForecastWeather1 WHERE    [durationID]=7 and [LST]='{0}'   and  Module='ZGrid' and  Site='{1}' )   ", forecasttime.AddHours(period), site);
            DataTable WeatherTable = m_Database.GetDataTable(sql);

            DataRow[] weatherdrs = WeatherTable.Select("ITEMID=" + 12);
            double wind = Convert.ToDouble(weatherdrs[0]["Value"]);
            basedata.wind = wind;
            weatherdrs = WeatherTable.Select(" ITEMID=" + 9);
            double temp = Convert.ToDouble(weatherdrs[0]["Value"]);
            basedata.temp = temp;
            weatherdrs = WeatherTable.Select("ITEMID=" + 15);
            double rain = Convert.ToDouble(weatherdrs[0]["Value"]);
            basedata.rain = rain;
            weatherdrs = WeatherTable.Select("ITEMID=" + 13);
            double cldf = Convert.ToDouble(weatherdrs[0]["Value"]);
            basedata.cldf = cldf;

            int aqi = 0;
            string items = "";
            //会商气象数据
            DataTable RPollutionTable = new DataTable();
            if (forecasttime.Hour == 8 && period == 0)
            {
                sql = string.Format(@"SELECT AQI,Parameter,durationID
  FROM [T_ForecastGroup] where  [Module]='GENERAL'  and   [durationID] in  (2,3,6) AND  [ITEMID] =0 AND  [LST]='{0}'   AND ForecastDate='{1}'  ORDER BY AQI desc  ", forecasttime.Date.AddHours(period), forecasttime.AddHours(-24).Date);
                RPollutionTable = m_Database.GetDataTable(sql);
            }
            else if (forecasttime.Hour == 20)
            {
                if (period == 0 || period == 24)
                    RPollutionTable = m_Database.GetDataTable(string.Format(@"  SELECT AQI,Parameter,durationID
  FROM [T_ForecastGroup]where  [Module]='GENERAL'  and   [durationID] =6 AND  [ITEMID] =0    AND  [LST]='{2}' AND ForecastDate='{1}'  union SELECT AQI,Parameter,durationID
  FROM [T_ForecastGroup]where  [Module]='GENERAL'  and   [durationID] =2 AND  [ITEMID] =0 AND  [LST]='{0}'   AND ForecastDate='{1}'  union SELECT AQI,Parameter,durationID
  FROM [T_ForecastGroup]where  [Module]='GENERAL'  and   [durationID] =3 AND  [ITEMID] =0 AND  [LST]='{0}'   AND ForecastDate='{1}'  ORDER BY AQI DESC  ", forecasttime.Date.AddHours(period + 24), forecasttime.Date, forecasttime.Date.AddHours(period)));
            }

            if (RPollutionTable.Rows.Count == 3)
            {
                aqi = Convert.ToInt32(RPollutionTable.Rows[0]["AQI"]);

                // items = (RPollutionTable.Rows[0][1]).ToString();
                if (RPollutionTable.Select("AQI=" + aqi).Length > 0)
                {
                    foreach (DataRow dr in RPollutionTable.Select("AQI=" + aqi))
                    {
                        items = items + "&" + dr["Parameter"];
                    }
                }
                basedata.aqi = aqi;
                basedata.items = items.Remove(0,1);
            }
            else
            {
                //模式污染物数据
                DataTable PollutionTable = m_Database.GetDataTable(string.Format(@"  SELECT AQI,ITEMID 
  FROM [EMFCShare].[dbo].[T_ForecastSite] WHERE    [durationID]=7 and [LST]='{0}'   and  Module='WRF' and  Site='{1}' AND  ForecastDate ='{2}'   ORDER BY AQI DESC", forecasttime.AddHours(period), site, forecasttime));

                if (PollutionTable.Rows.Count == 0)
                {
                    //模式污染物数据
                    for (DateTime dt = forecasttime; dt >= forecasttime.AddDays(-4); dt = dt.AddHours(-12))
                    {
                        sql = string.Format(@" SELECT AQI,ITEMID 
  FROM [EMFCShare].[dbo].[T_ForecastSite] WHERE    [durationID]=7 and [LST]='{0}'   and  Module='WRF' and  Site='{1}' AND  ForecastDate ='{2}'   ORDER BY AQI DESC  ", forecasttime.AddHours(period), site, dt);
                        PollutionTable = m_Database.GetDataTable(sql);
                        if (PollutionTable.Rows.Count != 0)
                            break;
                    }
                }
                aqi = Convert.ToInt32(PollutionTable.Rows[0][0]);

                //items ="";
                if (PollutionTable.Select("AQI=" + aqi).Length > 0)
                {
                    foreach (DataRow dr in PollutionTable.Select("AQI=" + aqi))
                    {
                        items = items + "&" + ((Itemid)Convert.ToInt32(dr[1])).ToString();
                    }
                }
                basedata.aqi = aqi;
                basedata.items = items.Remove(0,1);
            }
            return basedata;
        }
    }

    public class BaseDatas
    {
        public int hazegrade { get; set; }
        public double wind { get; set; }
        public double temp { get; set; }
        public double rain { get; set; }
        public double cldf { get; set; }
        public int aqi { get; set; }
        public string items { get; set; }
        public string site { get; set; }
        public int period { get; set; }
        public DateTime forecasttime { get; set; }
        public BaseDatas()
        {
            
        }

    }
}
