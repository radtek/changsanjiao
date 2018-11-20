using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Readearth.Data;
using Utility.GradeGuide;
using System.Data;

namespace WindowsFormsApplication11
{
    public class Clc
    {
        /// <summary>
        /// 重新算空气清洁度
        /// </summary>
        /// <param name="aqi"></param>
        /// <param name="wind"></param>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        public void CalClearAir(int aqi, double wind, string site, string forecastTimes)
        {

            TimeSpan dtime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 00:00:00")) -
                          DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            int periods = int.Parse(dtime.TotalHours.ToString());
            int index = 1;
            if (periods == 0)
                index = 1;
            else if (periods == 24)
                index = 2;
            else if (periods == 48)
                index = 3;
            else if (periods == 72)
                index = 4;
            if (DateTime.Now.Hour > 12)
                index--;

            forecastTimes = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//修改03-23

            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "空气清洁度指数";
            int period = 0;
            bool isTodb = true;//先不入库,正式用改成true
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00"));
            if (DateTime.Now.Hour > 12)
            {
                forecastTime =DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 20:00:00"));// 12点之后就算下午时次

                if (index == 1)
                {
                    period = 24 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 24, site, disease }));
                    DataTable dt1 = sg.CalClPollutionGrade(aqi, wind, forecastTime, (period + 24), site, disease, isTodb);
                }

                if (index == 2)
                {
                    period = 48 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 48, site, disease }));
                    DataTable dt2 = sg.CalClPollutionGrade(aqi, wind, forecastTime, (period + 24), site, disease, isTodb);
                }

                if (index == 3)
                {
                    period = 72 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 72, site, disease }));
                    DataTable dt3 = sg.CalClPollutionGrade(aqi, wind, forecastTime, (period+24), site, disease, isTodb);
                }

            }
            else
            {
                if (index == 1)
                {
                    period = 0;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 0, site, disease }));
                    DataTable dt1 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                }

                if (index == 2)
                {
                    period = 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 24, site, disease }));
                    DataTable dt2 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                }

                if (index == 3)
                {
                    period = 48;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 48, site, disease }));
                    DataTable dt3 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                }
            }
        }

        /// <summary>
        /// 计算综合观景指数
        /// </summary>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        /// <param name="rain"></param>
        /// <param name="temp"></param>
        /// <param name="wind"></param>
        /// <param name="cldf"></param>
        /// <param name="hze"></param>
        /// <param name="aqi"></param>
        /// <param name="items"></param>
        public void CalViewAir(string site, string forecastTimes, double rain, double temp, string wind, double cldf, double hze, int aqi, string items,string windspeed,string tqxx)
        {
            TimeSpan dtime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 00:00:00")) -
                      DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            int periods = int.Parse(dtime.TotalHours.ToString());
            int index = 1;
            if (periods == 0)
                index = 1;
            else if (periods == 24)
                index = 2;
            else if (periods == 48)
                index = 3;
            else if (periods == 72)
                index = 4;
            if (DateTime.Now.Hour > 12)
                index--;


            double winds=Speed(wind.ToString());
            #region new Code xuehui 0830 
            forecastTimes = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Database db = new Database();
            SceneryGuide sg = new SceneryGuide(db);
            int period = 0;
            string disease = "综合观景指数";
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00"));
            if (DateTime.Now.Hour <= 12)
            {
                if (index == 1)
                {
                    period = 0;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 0, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                           forecastTime,
                                           period, site, GetClassName(site), false);
                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 0, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
                if (index == 2)
                {
                    period = 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 24, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                               forecastTime,
                                               period, site, GetClassName(site), false);
                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 24, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
                if (index == 3)
                {
                    period = 48;

                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 48, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                         forecastTime,
                                         period, site, GetClassName(site), false);
                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 48, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
            }
            else
            {
                forecastTime =
                               DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 20:00:00"));// 12点之后就算下午时次

                if (index == 1)
                {
                    period = 24 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 24, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                       forecastTime,
                                       period, site, GetClassName(site), false);

                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 24, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
                if (index == 2)
                {
                    period = 48 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 48, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                        forecastTime,
                                        period, site, GetClassName(site), false);
                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 48, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
                if (index == 3)
                {
                    period = 72 - 24;
                    db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTime, 72, site, disease }));
                    DataTable result = sg.CalPoGroupGrade(rain, temp, winds, cldf, aqi, int.Parse(hze.ToString()),
                                      forecastTime,
                                      period, site, GetClassName(site), false);
                    db.Execute(string.Format("INSERT INTO T_ScenAirForecast Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','" + result.Rows[0]["Grade"].ToString() + "') ", new object[] { forecastTime, 72, result.Rows[0]["GuideLines1"].ToString(), result.Rows[0]["GuideLines2"].ToString(), site, disease, result.Rows[0]["Grade"].ToString() }));
                }
            }

            #endregion 

        }

        private string GetClassName(string site) {
            string className = "";
            switch (site) {
                case "10001A": className = "一般类"; break;
                case "10002A": className = "一般类"; break;
                case "10003A": className = "花"; break;
                case "10004A": className = "游乐场类"; break;
                case "10005A": className = "一般类"; break;
                case "10006A": className = "游乐场类"; break;
                case "10007A": className = "森林类"; break;
                case "10008A": className = "古镇类"; break;
                case "10009A": className = "海"; break;
                case "10010A": className = "果"; break;
                case "10011A": className = "古镇类"; break;
                case "10012A": className = "果"; break;
                case "10013A": className = "一般类"; break;
                case "10014A": className = "海"; break;
                case "10015A": className = "森林类"; break;
                case "10016A": className = "森林类"; break;
                case "10017A": className = "森林类"; break;
                case "10018A": className = "游乐场类"; break;
            }
            return className;
        }

        private double Speed(string windGrade)
        {
            double wind = 0.0d;
            if (windGrade=="1-2级")
            {
                wind = 1.5;
            }
            else if (windGrade=="2-3级")
            {
                wind=3.3;
            }
            else if (windGrade == "3-4级")
            {
                wind = 5.4;
            }
            else if (windGrade == "4-5级")
            {
                wind = 7.9;
            }
            else if (windGrade == "5-6级")
            {
                wind = 10.7;
            }
            else if (windGrade == "6-7级")
            {
                wind = 13.8;
            }
            else if (windGrade == "6-7级")
            {
                wind = 13.8;
            }
            else if (windGrade == "7-8级")
            {
                wind = 17.1;
            }
            else if (windGrade == "8-9级")
            {
                wind = 20.7;
            }
            else if (windGrade == "9-10级")
            {
                wind = 24.3;
            }
            else
            {
                wind = 25;
            }

            return wind;
        }

        /// <summary>
        /// 天气分指数
        /// </summary>
        /// <param name="rain"></param>
        /// <param name="temp"></param>
        /// <param name="wind"></param>
        /// <param name="cldf"></param>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        //public int CalTQ(double rain, double temp, double wind, double cldf, string site, string forecastTimes, int period,string windSpeed,string tqxx)
        //{
        //    Database db = new Database();
        //    SceneryGuide sg = new SceneryGuide(db);
        //    string disease = "天气分指数";
        //    bool isTodb = false;//先不入库,正式用改成true
          
        //    if (DateTime.Now.Hour > 12)
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        //    else
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes,  period, site, disease }));

        //    wind=Speed(windSpeed);
          
        //    DataTable dt1 = null;// sg.CalMetGrade(rain, temp, wind, cldf, DateTime.Parse(forecastTimes), period, site, disease, isTodb);//薛辉 05-16 因为接口改动，我这边也要调整
        //    if (DateTime.Now.Hour > 12)
        //    {
        //       dt1= sg.CalMetGrade(rain, temp, wind, cldf, DateTime.Parse(forecastTimes), (period+24), site, disease, isTodb);//薛辉 05-16 因为接口改动，我这边也要调整
        //    }
        //    else {
        //       dt1= sg.CalMetGrade(rain, temp, wind, cldf, DateTime.Parse(forecastTimes), period, site, disease, isTodb);//薛辉 05-16 因为接口改动，我这边也要调整
        //    }
        //    int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);

        //    //覆盖原有详细提示 温度 风速  降水 天气现象
        //    string wd = "平均温度(" + temp + "℃);";  
        //    string fs = "风速(" + windSpeed + ");";
        //    string jyl = "降雨量(" + rain + "毫米);";
        //    string tq = "天空状况(" + tqxx + ")";
        //    string guideLine2 = wd + fs + jyl + tq;

        //    //if (DateTime.Now.Hour > 12)
        //      //  db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2 + "' WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        //  //  else
        //       // db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2+ "'  WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, period, site, disease }));

            
        //    return grade;

        //}

        /// <summary>
        /// 霾指数
        /// </summary>
        /// <param name="rain"></param>
        /// <param name="temp"></param>
        /// <param name="wind"></param>
        /// <param name="cldf"></param>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        //public int CalHaze(double hze, string site, string forecastTimes, int period)
        //{
        //    Database db = new Database();
        //    SceneryGuide sg = new SceneryGuide(db);
        //    string disease = "霾指数";
        //    bool isTodb = false;//先不入库,正式用改成true

        //    if (DateTime.Now.Hour > 12)
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        //    else
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, period, site, disease }));

        //    DataTable dt1 = null;// sg.CalHazeGrade(hze, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
        //    if (DateTime.Now.Hour > 12)
        //    {
        //        dt1=sg.CalHazeGrade(hze, DateTime.Parse(forecastTimes), (period+24), site, disease, isTodb);
        //    }
        //    else {
        //        dt1=sg.CalHazeGrade(hze, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
        //    }

        //    int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);

        //    //覆盖原有详细提示 霾
        //    string m = "无霾";
        //    if (hze != 1d) {
        //        m = "有霾";
        //        switch (hze.ToString())
        //        {
        //            case "2": m = m + "(能见度3-5km)";  break;
        //            case "3": m = m + "(能见度1-3km)"; break;
        //            case "4": m = m + "(能见度不足1km)"; break;
        //        }
        //    }

        //    string guideLine2 = m;

        //   // if (DateTime.Now.Hour > 12)
        //      //  db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2 + "' WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        // //   else
        //       // db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2 + "'  WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, period, site, disease }));


            
        //    return grade;
        //}



        public string GetLevelName(int aqi) {
            string levName = "";
            if (aqi >= 0 && aqi <= 50) {
                levName = "优";
            }
            else  if (aqi >= 51 && aqi <= 100)
            {
                levName = "良";
            }
            else if (aqi >= 101 && aqi <= 150)
            {
                levName = "轻度污染";
            }
            else if (aqi >= 151 && aqi <= 200)
            {
                levName = "中度污染";
            }
            else if (aqi >= 201 && aqi <= 300)
            {
                levName = "重度污染";
            }
            else if (aqi >= 301)
            {
                levName = "严重污染";
            }
            return levName;
           
        }
        public string GetLevelNameII(int aqi)
        {
            string levName = "";
            if (aqi >= 0 && aqi <= 50)
            {
                levName = "0~50";
            }
            else if (aqi >= 51 && aqi <= 100)
            {
                levName = "51~100";
            }
            else if (aqi >= 101 && aqi <= 150)
            {
                levName = "101~150";
            }
            else if (aqi >= 151 && aqi <= 200)
            {
                levName = "151~200";
            }
            else if (aqi >= 201 && aqi <= 300)
            {
                levName = "201~300";
            }
            else if (aqi >= 301)
            {
                levName = ">300";
            }
            return levName;

        }

        /// <summary>
        /// 空气质量分指数
        /// </summary>
        /// <param name="rain"></param>
        /// <param name="temp"></param>
        /// <param name="wind"></param>
        /// <param name="cldf"></param>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        //public int CalQualityAir(int aqi, string items, string site, string forecastTimes, int period)
        //{
        //    Database db = new Database();
        //    SceneryGuide sg = new SceneryGuide(db);
        //    string disease = "空气质量分指数";
        //    bool isTodb = false;//先不入库,正式用改成true

        //    if (DateTime.Now.Hour > 12)
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        //    else
        //        db.Execute(string.Format("DELETE T_ScenAirForecast WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, period, site, disease }));

        //    DataTable dt1 = null;// sg.CalPollutionGrade(aqi, items, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
        //    if (DateTime.Now.Hour > 12)
        //    {
        //        dt1=sg.CalPollutionGrade(aqi, items, DateTime.Parse(forecastTimes), (period+24), site, disease, isTodb);
        //    }
        //    else {
        //        dt1=sg.CalPollutionGrade(aqi, items, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
        //    }
        //    int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);

        //    //覆盖原有详细提示 AQI  王斌 2017.5.8
        //    string aqis = "AQI指数：" + aqi + ",等级:" + GetLevelName(aqi) + "(" + GetLevelNameII(aqi) + ")" + ";首要污染物:" + items;
        //    string guideLine2 = aqis;

        //   // if (DateTime.Now.Hour > 12)
        //        //db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2 + "' WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, (period + 24), site, disease }));
        //   // else
        //        //db.Execute(string.Format("UPDATE T_ScenAirForecast SET  GuideLines2='" + guideLine2 + "'  WHERE  ForecastDate='{0}' and PERIOD={1} and  station='{2}' and Type='{3}' ", new object[] { forecastTimes, period, site, disease }));

        //    return grade;
        //}
    }
}
