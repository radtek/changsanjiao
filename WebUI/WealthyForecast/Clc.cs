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

            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "空气清洁度指数";
            int period = 0;
            bool isTodb = false;//先不入库,正式用改成true
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00"));
            if (DateTime.Now.Hour > 12)
            {
                period = 24;
                forecastTime =
                DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 20:00:00"));// 12点之后就算下午时次
                DataTable dt1 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                period = 48;
                DataTable dt2 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                period = 72;
                DataTable dt3 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
            }
            else
            {
                period = 0;
                DataTable dt1 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                period = 24;
                DataTable dt2 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
                period = 48;
                DataTable dt3 = sg.CalClPollutionGrade(aqi, wind, forecastTime, period, site, disease, isTodb);
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
        public void CalViewAir(string site, string forecastTimes, double rain, double temp, double wind, double cldf, double hze, int aqi, string items)
        {
            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "综合观景指数";
            int period = 0;
            bool isTodb = false;//先不入库,正式用改成true
            int e1 = 0;
            int e2 = 0;
            int e3 = 0;
            DateTime forecastTime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 08:00:00"));
            if (DateTime.Now.Hour <= 12)
            {
                period = 0;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt1 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);

                //=======================
                period = 24;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt2 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);

                //=======================
                period = 48;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt3 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);
            }
            else
            {
                forecastTime =
                               DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 20:00:00"));// 12点之后就算下午时次
                period = 24;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt1 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);

                //=======================
                period = 48;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt2 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);

                //=======================
                period = 72;
                e1 = CalTQ(rain, temp, wind, cldf, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e2 = CalQualityAir(aqi, items, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                e3 = CalHaze(hze, site, forecastTime.ToString("yyyy-MM-dd HH:mm:ss"), period);
                DataTable dt3 = sg.CalPoGroupGrade(e1, e3, e2, forecastTime, period, site, disease, isTodb);
            }
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
        public int CalTQ(double rain, double temp, double wind, double cldf, string site, string forecastTimes, int period)
        {
            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "天气分指数";
            bool isTodb = false;//先不入库,正式用改成true
            DataTable dt1 = sg.CalMetGrade(rain, temp, wind, cldf, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
            int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);
            return grade;

        }

        /// <summary>
        /// 霾指数
        /// </summary>
        /// <param name="rain"></param>
        /// <param name="temp"></param>
        /// <param name="wind"></param>
        /// <param name="cldf"></param>
        /// <param name="site"></param>
        /// <param name="forecastTimes"></param>
        public int CalHaze(double hze, string site, string forecastTimes, int period)
        {
            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "霾指数";
            bool isTodb = false;//先不入库,正式用改成true
            DataTable dt1 = sg.CalHazeGrade(hze, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
            int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);
            return grade;
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
        public int CalQualityAir(int aqi, string items, string site, string forecastTimes, int period)
        {
            Database db = new Database();
            SceneryGrade sg = new SceneryGrade(db);
            string disease = "空气质量分指数";
            bool isTodb = false;//先不入库,正式用改成true
            DataTable dt1 = sg.CalPollutionGrade(aqi, items, DateTime.Parse(forecastTimes), period, site, disease, isTodb);
            int grade = 0; int.TryParse(dt1.Rows[0]["Grade"].ToString(), out grade);
            return grade;
        }
    }
}
