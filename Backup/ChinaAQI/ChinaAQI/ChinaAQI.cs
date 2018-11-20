using System;
using System.Collections.Generic;
using System.Text;

namespace Lucas.AQI2012
{
    public class ConvertAQI
    {
        //**This section may not need all of the following objects.
        private static IConvertAQI converter = new AQI2012();

        public static int ConvertToAQI(object value, int ParameterID, int DurationID, int AggregateID)
        {
           return converter.ConvertToAQI(value, ParameterID, DurationID, AggregateID);
        }

        public static double ConvertToAQIUnrounded(object value, int ParameterID, int DurationID, int AggregateID)
        {
            return converter.ConvertToAQIUnrounded(value, ParameterID, DurationID, AggregateID);
        }

        public static void UseConversion(string converterQualifiedName)
        {
            Type converterType = Type.GetType(converterQualifiedName, true);
            converter = (IConvertAQI)Activator.CreateInstance(converterType);
        }

        public static IConvertAQI Converter
        {
            get { return converter; }
            set { converter = value; }
        }
    }

    //**This section SHOULD NOT need to be updated.
    public interface IConvertAQI
    {
        int ConvertToAQI(object value, int ParameterID, int DurationID, int AggregateID);
        double ConvertToAQIUnrounded(object value, int ParameterID, int DurationID, int AggregateID);
    }

    public class AQI2012 : IConvertAQI
    {
        //**This section MAY need to be updated if the parameter list is not correct 
        public enum Parameter
        {
            OZONE = 8,
            PM10 = 7,
            NO2 = 22,
            CO = 6,
            SO2 = 1,
            PM25 = 24
        }

        //**This section MAY need to be updated if the Duration list is not correct 
        public enum Duration
        {
            HOURLY = 10,
            DAILY = 11,
            NOON_TO_NOON = 15,
            //AVERAGE_8HR = 110,
            AVERAGE_8HR = 16,
            SURROGATE = 150
        }

        //**This section MAY need to be updated if the Aggregate list is not correct 
        public enum Aggregate
        {
            NONE = 0,            
            AVERAGE_1HR = 10,
            AVERAGE_8HR = 110,
            //AVERAGE_8HR = 16,
            AVERAGE_24HR = 180
        }

        //**This section MAY need to be updated if the EffectiveDuration list is not correct 
        private enum EffectiveDuration
        {
            HOURLY,
            EIGHT_HOUR,
            DAILY
        }

        //**This section SHOULD NOT need to be changed, if the number of API categories is the 
        //  same as the number of AQI categories (I believe it is). 
        public readonly int MISSING_VALUE = -999;
        public readonly int BAD_VALUE = -980;
        private readonly int NUM_CATEGORIES = 8;

        //**This section WILL need to be updated to reflect the correct pollutant names and cutpoints.  
        //  The first row of each pair shows the minimum concentration in each AQI cateogry and the second
        //  row of the pair shows the corresponding maximum concentration in each AQI category.  
        private readonly double[] empty = { -999.0, -999.0, -999.0, -999.0, -999.0, -999.0, -999.0, -999.0 };

        private readonly double[] rSO2Lo24Hr = { 0.0, 0.050, 0.150,0.475, 0.800, 1.600, 2.100, 2.620 };
        private readonly double[] rSO2Hi24Hr = { 0.050, 0.150,0.475, 0.800, 1.600, 2.100, 2.620, 999.0 };

        private readonly double[] rSO2Lo01Hr = { 0.0, 0.150, 0.500, 0.650, 0.800, -999.0, -999.0, -999.0 };
        private readonly double[] rSO2Hi01Hr = { 0.150, 0.500, 0.650, 0.800, -999.0, -999.0, -999.0, -999.0 };

        private readonly double[] rNO2Lo24Hr = { 0.0,0.04, 0.080, 0.180, 0.280, 0.565, 0.750, 0.940 };
        private readonly double[] rNO2Hi24Hr = { 0.040,0.080, 0.180, 0.280, 0.565, 0.750, 0.940, 999.0 };

        private readonly double[] rNO2Lo01Hr = { 0.0, 0.100, 0.200, 0.700, 1.200, 2.340, 3.090, 3.840 };
        private readonly double[] rNO2Hi01Hr = { 0.100, 0.200, 0.700, 1.200, 2.340, 3.090, 3.840, 999.0 };

        private readonly double[] rPM10Lo24Hr = { 0.0, 0.05, 0.15, 0.250, 0.35, 0.42, 0.50, 0.60 };
        private readonly double[] rPM10Hi24Hr = { 0.050, 0.150,0.250, 0.350, 0.420, 0.500, 0.600, 999.0 };

        private readonly double[] rPM25Lo24Hr = { 0.0, 0.035, 0.075, 0.115, 0.150, 0.250, 0.350, 0.500 };
        private readonly double[] rPM25Hi24Hr = { 0.035, 0.075, 0.115, 0.150, 0.250, 0.350, 0.500, 999.0 };


        private readonly double[] rOZONELo01Hr = { 0.0, 0.160, 0.200,0.300, 0.400, 0.800, 1.000, 1.200 };
        private readonly double[] rOZONEHi01Hr = { 0.160, 0.200, 0.300, 0.400, 0.800, 1.000, 1.200, 999.0 };

        private readonly double[] rOZONELo08Hr = { 0.0, 0.100, 0.160, 0.215, 0.265, 0.800, -999.0, -999.0 };
        private readonly double[] rOZONEHi08Hr = { 0.100, 0.160, 0.215, 0.265, 0.800, -999.0, -999.0, -999.0 };

        private readonly double[] rCOLo01Hr = { 0.0, 5.000, 10.000, 35.00, 60.000, 90.000, 120.000, 150.000 };
        private readonly double[] rCOHi01Hr = { 5.000, 10.000, 35.00, 60.000, 90.000, 120.000, 150.000, 999.0 };

        private readonly double[] rCOLo24Hr = { 0.0, 2.000, 4.000, 14.000, 24.000, 36.000, 48.000, 60.000 };
        private readonly double[] rCOHi24Hr = { 2.000, 4.000, 14.000, 24.000, 36.000, 48.000, 60.000, 999.0 };


        private readonly double[] rAPILo = { 0.0, 50.0, 100.0, 150.0, 200.0, 300.0, 400.0, 500.0 };
        private readonly double[] rAPIHi = { 50.0, 100.0, 150.0, 200.0, 300.0, 400, 500.0, 500.0 };


        ////Lucas add
        ////**This section SHOULD NOT need to be updated.
        //public int ConvertToAQI(object value, int ItemID)
        //{
        //    if (value == null) return MISSING_VALUE;
        //    if (value == DBNull.Value) return MISSING_VALUE;
        //    Parameter param = (Parameter)Enum.ToObject(typeof(Parameter), paramID);
        //    Duration duration = (Duration)Enum.ToObject(typeof(Duration), durationID);
        //    Aggregate aggregate = (Aggregate)Enum.ToObject(typeof(Aggregate), aggregateID);
        //    return ConvertToAQI(Convert.ToDouble(value), param, duration, aggregate);
        //}
        ////Lucas add
        ////**This section SHOULD NOT need to be updated.
        //public int ConvertToAQI(object value, string Parameter, EffectiveDuration StandardType)
        //{

        //    if (value == null) return MISSING_VALUE;
        //    if (value == DBNull.Value) return MISSING_VALUE;
        //    Parameter param = (Parameter)Enum.ToObject(typeof(Parameter), paramID);
        //    Duration duration = (Duration)Enum.ToObject(typeof(Duration), durationID);
        //    Aggregate aggregate = (Aggregate)Enum.ToObject(typeof(Aggregate), aggregateID);
        //    return ConvertToAQI(Convert.ToDouble(value), param, duration, aggregate);
        //}



        //**This section SHOULD NOT need to be updated.
        public int ConvertToAQI(object value, int paramID, int durationID, int aggregateID)
        {
            if (value == null) return MISSING_VALUE;
            if (value == DBNull.Value) return MISSING_VALUE;
            Parameter param = (Parameter)Enum.ToObject(typeof(Parameter), paramID);
            Duration duration = (Duration)Enum.ToObject(typeof(Duration), durationID);
            Aggregate aggregate = (Aggregate)Enum.ToObject(typeof(Aggregate), aggregateID);
            return ConvertToAQI(Convert.ToDouble(value), param, duration, aggregate);
        }

        //**This section SHOULD NOT need to be updated.
        public double ConvertToAQIUnrounded(object value, int paramID, int durationID, int aggregateID)
        {
            if (value == null)
                return MISSING_VALUE;
            if (value == DBNull.Value)
                return MISSING_VALUE;
            Parameter param = (Parameter)Enum.ToObject(typeof(Parameter), paramID);
            Duration duration = (Duration)Enum.ToObject(typeof(Duration), durationID);
            Aggregate aggregate = (Aggregate)Enum.ToObject(typeof(Aggregate), aggregateID);
            return ConvertToAQIUnrounded(Convert.ToDouble(value), param, duration, aggregate);
        }

        //**This section SHOULD NOT need to be updated.
        public int ConvertToAQI(double value, Parameter param, Duration duration, Aggregate aggregate)
        {
            value = Math.Round(value, 3);//Lucas
            double aqi = ConvertToAQIUnrounded(value, param, duration, aggregate);
            // And round to get an integer value
            return (int)Math.Ceiling(aqi);//Lucas
            // return (int)Math.Round(aqi);//Lucas
        }

        //**This section SHOULD NOT need to be updated.
        public double ConvertToAQIUnrounded(double value, Parameter param, Duration duration, Aggregate aggregate)
        {
            if (value == BAD_VALUE || value == MISSING_VALUE)
            {
                return (int)value;
            }
            double[] maxLo;
            double[] maxHi;
            EffectiveDuration dur = GetEffectiveDuration(param, duration, aggregate);
            GetCutpoints(param, dur, out maxLo, out maxHi);
            if (maxLo == empty || maxHi == empty)
            {
                return MISSING_VALUE;
            }
            return GetAQIUnrounded(value, maxLo, maxHi);
        }

        //**This section WILL need to be updated to match the EffectiveDuration and Aggregate lists set 
        //  above.  This section links the EffectiveDuration list to the Aggregate list.
        private EffectiveDuration GetEffectiveDuration(Parameter param, Duration duration, Aggregate aggregate)
        {
            switch (duration)
            {
                case Duration.DAILY:
                    return EffectiveDuration.DAILY;
                case Duration.NOON_TO_NOON:
                    return EffectiveDuration.DAILY;

                case Duration.HOURLY:
                    switch (aggregate)
                    {
                        case Aggregate.NONE:
                            return EffectiveDuration.HOURLY;
                        case Aggregate.AVERAGE_1HR:
                            return EffectiveDuration.HOURLY;
                        case Aggregate.AVERAGE_8HR:
                            return EffectiveDuration.EIGHT_HOUR;
                        case Aggregate.AVERAGE_24HR:
                            return EffectiveDuration.DAILY;
                    }
                    break;

                case Duration.AVERAGE_8HR:
                    return EffectiveDuration.EIGHT_HOUR;

                case Duration.SURROGATE:
                    switch (param)
                    {
                        case Parameter.OZONE:
                            return EffectiveDuration.EIGHT_HOUR;
                        /*
                        case Parameter.PM25:
                            return EffectiveDuration.DAILY;
                         */
                    }
                    break;
            }
            throw new Exception("Unexpected DurationID/AggregateID combination");
        }

        //** This section WILL need to be updated to match Parameter and 
        //   EffectiveDuration lists updated above.  This section sets the parameter
        //   cutpoints.
        private void GetCutpoints(Parameter param, EffectiveDuration dur,
                out double[] maxLo, out double[] maxHi)
        {
            maxLo = empty;
            maxHi = empty;
            switch (dur)
            {
                case EffectiveDuration.HOURLY:
                    switch (param)
                    {
                        case Parameter.OZONE:
                            maxLo = rOZONELo01Hr;
                            maxHi = rOZONEHi01Hr;
                            break;
                        case Parameter.CO:
                            maxLo = rCOLo01Hr;
                            maxHi = rCOHi01Hr;
                            break;
                        case Parameter.SO2:
                            maxLo = rSO2Lo01Hr;
                            maxHi = rSO2Hi01Hr;
                            break;
                        case Parameter.NO2:
                            maxLo = rNO2Lo01Hr;
                            maxHi = rNO2Hi01Hr;
                            break;

                    }
                    break;
                
                case EffectiveDuration.EIGHT_HOUR:
                    switch (param)
                    {
                        case Parameter.OZONE:
                            maxLo = rOZONELo08Hr;
                            maxHi = rOZONEHi08Hr;
                            break;
                
                    }
                    break;
                case EffectiveDuration.DAILY:
                    switch (param)
                    {
                        
                        case Parameter.PM25:
                            maxLo = rPM25Lo24Hr;
                            maxHi = rPM25Hi24Hr;
                            break;                         
                        case Parameter.PM10:
                            maxLo = rPM10Lo24Hr;
                            maxHi = rPM10Hi24Hr;
                            break;
                        case Parameter.SO2:
                            maxLo = rSO2Lo24Hr;
                            maxHi = rSO2Hi24Hr;
                            break;
                        case Parameter.NO2:
                            maxLo = rNO2Lo24Hr;
                            maxHi = rNO2Hi24Hr;
                            break;
                        case Parameter.CO:
                            maxLo = rCOLo24Hr;
                            maxHi = rCOHi24Hr;
                            break;

                    }
                    break;
            }
            return;
        }

        //**This section may not be needed.  It is assigning the data to an AQI/API cateogry
        //  rather than calculating a specific integer value. 
        private int GetCategory(double value, double[] maxLo, double[] maxHi)
        {
            int category = MISSING_VALUE;
            for (int i = 0; i < NUM_CATEGORIES; i++)
            {
                if ((value >= maxLo[i]) && (value <= maxHi[i]))//Lucas
                {
                    category = i + 1;
                    break;
                }
            }
            return category;
        }
        //**This section SHOULD NOT need updating.  This is needed to round the calculated 
        //  AQI/API to an integer.
        private int GetAQI(double value, double[] maxLo, double[] maxHi)
        {
            value = Math.Round(value, 3);//Add by Lucas 
            double aqi = GetAQIUnrounded(value, maxLo, maxHi);
            // And round to get an integer value
            return (int)Math.Ceiling(aqi);//return (int)Math.Round(aqi);
        }
        //**This section needs to be updated for special 24-hr average rounding requirment for China API calculations.
        private double GetAQIUnrounded(double value, double[] maxLo, double[] maxHi)
        {
            //updated by Lucas
            int idx = GetCategory(value, maxLo, maxHi) - 1;
            if (idx >= maxLo.Length - 1)
            {
                return 500;
            }
            if (idx < 0 || maxLo[idx] == MISSING_VALUE || maxHi[idx] == MISSING_VALUE)
            {
                return MISSING_VALUE;
            }
            // Actual AQI formula is here
            double aqi = (((rAPIHi[idx] - rAPILo[idx]) / (maxHi[idx] - maxLo[idx]))
                * (value - maxLo[idx]) + rAPILo[idx]);
            return aqi;
            //}
        }
    }
    //
    public class AQILevel
    {

        public enum LevelBreak : int
        {
            Level_0 = 0,
            Level_1 = 50,
            Level_2 = 100,
            Level_3 = 150,
            Level_4 = 200,
            Level_5 = 300,
            Level_6 = 500
        }
        public enum LevelNum : int
        {
            Level_1 = 1,
            Level_2 = 2,
            Level_3 = 3,
            Level_4 = 4,
            Level_5 = 5,
            Level_6 = 6
        }
        private readonly static string[] LevelName = { "一级","二级","三级","四级","五级","六级" };
        private readonly static string[] LevelQuality = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染" };
        private readonly static string[] LevelColor = { "绿色", "黄色", "橙色", "红色", "紫色", "褐红色" };


        public static int GetAQILevel(int AQI)
        {
            int level=0;
            if(AQI>(int)LevelBreak.Level_0 &&AQI<=(int)LevelBreak.Level_1)
            {
                level = 1;
            }
            else if (AQI > (int)LevelBreak.Level_1 && AQI <= (int)LevelBreak.Level_2)
            {
                level = 2;
            }
            else if (AQI > (int)LevelBreak.Level_2 && AQI <= (int)LevelBreak.Level_3)
            {
                level = 3;
            }
            else if (AQI > (int)LevelBreak.Level_3 && AQI <= (int)LevelBreak.Level_4)
            {
                level = 4;
            }
            else if (AQI > (int)LevelBreak.Level_4 && AQI <= (int)LevelBreak.Level_5)
            {
                level = 5;
            }
            else if (AQI > (int)LevelBreak.Level_5 && AQI <= (int)LevelBreak.Level_6)
            {
                level = 6;
            }

            return level;
        }

        public static int GetAQILevel(int AQI, out string levelName)
        {
            int level = GetAQILevel(AQI);
            //int level = GetAQILevel(AQI);
            if (level > 0)
            {
                levelName = LevelName[level - 1];
            }
            else
            {
                levelName = "无效";
            }
           
            return level;
        }
        public static int GetAQILevel(int AQI, out string levelName, out string levelQuality)
        {
            
            int level = GetAQILevel(AQI);
            if (level > 0)
            {
                levelName = LevelName[level - 1];
                levelQuality = LevelQuality[level - 1];
            }
            else
            {
                levelName = "无效";
                levelQuality = "无效";
            }
            return level;
        }
        public static int GetAQILevel(int AQI, out string levelName, out string levelQuality, out string levelColor)
        {
            int level = GetAQILevel(AQI);
            if (level > 0)
            {
                levelName = LevelName[level - 1];
                levelQuality = LevelQuality[level - 1];
                levelColor = LevelColor[level - 1];
            }
            else
            {
                levelName = "无效";
                levelQuality = "无效";
                levelColor = "无效";
            }

            return level;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pm10"></param>
        /// <param name="pm25"></param>
        /// <param name="so2"></param>
        /// <param name="no2"></param>
        /// <param name="co"></param>
        /// <param name="o3_1hr"></param>
        /// <param name="o3_8hr"></param>
        /// <param name="primaryPollutions">可以是多个，以逗号隔开</param>
        /// <returns></returns>
        public static int GetAQIfromIAQI(int pm10,int pm25,int so2, int no2, int co, int o3_1hr, int o3_8hr, out string primaryPollutions)
        {
            int aqi = 0;
            primaryPollutions = "";
            if (pm10 >= aqi)
            {
                aqi = pm10;
            }
            if (pm25 >= aqi)
            {
                aqi = pm25;
            }
            if (so2 >= aqi)
            {
                aqi = so2;
            }
            if (no2 >= aqi)
            {
                aqi = no2;
            }
            if (co >= aqi)
            {
                aqi = co;
            }
            if (o3_1hr >= aqi)
            {
                aqi = o3_1hr;
            }
            if (o3_8hr >= aqi)
            {
                aqi = o3_8hr;
            }
            if (aqi <= 0)
            {
                primaryPollutions = "null";
            }
            else if (aqi <= 50)
            {
                primaryPollutions = "/";
            }
            else
            {
                if (pm10 == aqi)
                {
                    primaryPollutions = primaryPollutions + ",PM10";
                }
                if (pm25 == aqi)
                {
                    primaryPollutions = primaryPollutions + ",PM2.5";
                }
                if (so2 == aqi)
                {
                    primaryPollutions = primaryPollutions + ",SO2";
                }
                if (no2 == aqi)
                {
                    primaryPollutions = primaryPollutions + ",NO2";
                }
                if (co == aqi)
                {
                    primaryPollutions = primaryPollutions + ",CO";
                }
                if (o3_1hr >= aqi)
                {
                    primaryPollutions = primaryPollutions + ",O3_1hr";
                }
                if (o3_8hr >= aqi)
                {
                    primaryPollutions = primaryPollutions + ",O3_8hr";
                }
                primaryPollutions = primaryPollutions.Substring(1, primaryPollutions.Length - 1);
            }
            return aqi;
        }
    }
}

