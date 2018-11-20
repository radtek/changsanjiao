using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Text;
using Aspose.Cells;
using System.Web.Services;
using System.Text.RegularExpressions;


public partial class EvaluateHtml_PersonRating : System.Web.UI.Page
{
    private static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database();
        //H00.Value = DateTime.Now.AddMonths(-1).Date.ToString("yyyy年MM月");
    }
    [WebMethod]
    public static string BtnQuery(string date) {
        date = DateTime.Parse(date).ToString("yyyy-MM-01");
        string sql = "select [forecaster],[bc],[cnAQIsum],[cnAQIaverge],[totalEnvir],[totalPM25],[totalO31h],"+
            "[totalO38h],[totalPM10],[totalNO2],[totalHAZE],[totalUV],[singleEnvir],[singlePM25],[singleO31h],[singleO38h],"+
            "[singlePM10],[singleNO2],[singleHAZE],[singleUV],[periodNight],[periodMorning],[periodAfternoon],"+
            "[dayScore],[envirSumScore],[personScore] from [T_Evaluate] where time='"+date+"'";
        DataTable dt = m_Database.GetDataTable(sql);
        //        {
        //            "row1": [{ "forecaster": "xxx" }, { "bc": 3 }, { "cnAQIsum": 234 }
        //        }
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
            {
                sb.Append("\"row" + rowCount + "\":[");
                for (int colCount = 0; colCount < dt.Columns.Count; colCount++)
                {
                    string colName = dt.Columns[colCount].ColumnName;
                    string val = dt.Rows[rowCount][colCount].ToString();
                    string p = @"^(-?\d+)(\.\d+)?$";
                    Regex rgx = new Regex(p);
                    if (colCount != 1)
                    {
                        if (rgx.IsMatch(val))
                        {
                            val = double.Parse(val).ToString("f2");
                        }
                    }
                   
                    if (colName.IndexOf("PM") > -1 || colName.IndexOf("O3") > -1 || colName.IndexOf("NO") > -1)
                    {
                        sb.Append("{\"" + colName + "\":\"" + val + "\"},");
                    }
                    else
                    {
                        sb.Append("{\"" + colName + "\":\"" + val + "\"},");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
            }
            sb.Remove(sb.Length - 1, 1);
        }
        else {
            sb.Append("\"row0\":[{\"val\":\"没有数据\"}]");
        }
        sb.Append("}");
        return sb.ToString();
    }

    [WebMethod]
    public static string PersonDown(string date)
    {
        try{
            PersonDurationDown(date); //个人分时段加工
            DayDown(date);
        }
        catch { return "个人分时段数据加工失败，请检查相关数据是否正确！！"; }

        Database m_Database = new Database();
        int processCount = 0;
        DateTime staTimeMonth = DateTime.Parse(date);
        string sql_users = "select * from [D_UserCode]";
        string sql_worksum = "select * from T_EvaluateBasicData where convert(varchar(7),[date],120)='{0}' and number='{1}'";
        string sql_data = "select * from T_PersonScore where LST='{0}' and UserName='{1}'";
        string sql_DayDurScore = "select sum(F) as 'F' from T_DayDurScore where convert(varchar(7),LST,120)='{0}' and UserID='{1}' and DurationID='{2}'";
        string sql_DayDurScoreItems = "select sum({3}) as 'F' from T_DayDurScore where convert(varchar(7),LST,120)='{0}' and UserID='{1}' and DurationID='{2}'";
        string sql_DayDurScoreItems_times = "select lst from T_DayDurScore where convert(varchar(7),LST,120)='{0}' and UserID='{1}' and DurationID='{2}'";//获取班次对应的时间
        string sql_shAQI = "select * from [dbo].[T_24hAQI] where Area='上海市' " +
                           " and substring(convert(varchar(20),time_point,120),12,2)='00' and time_point='{0}' ";
        DataTable dt_users = m_Database.GetDataTable(sql_users);
        string reTime = staTimeMonth.ToString("yyyy-MM-01 00:00:00");
        string reTimeMonth = staTimeMonth.ToString("yyyy-MM");
        foreach (DataRow row_user in dt_users.Rows)
        {
            try
            {
                //预报员
                string forecaster = row_user["userName"].ToString();
                string forecasterNumber = row_user["userCode"].ToString();
                //班次
                DataTable dt_work = m_Database.GetDataTable(string.Format(sql_worksum, reTimeMonth, forecasterNumber));
                string worknum = dt_work.Rows.Count.ToString();
                // 国家局AQI总分
                DataTable dt_person = m_Database.GetDataTable(string.Format(sql_data, reTime, forecaster));
                float ChinaAQISum = 0f;
                try { ChinaAQISum = float.Parse(dt_person.Rows[0]["SumChinaScore"].ToString()); }
                catch { }
                //国家局AQI平均分
                float ChinaAQIAvg = 0f;
                try { ChinaAQIAvg = float.Parse(dt_person.Rows[0]["AvgChinaScore"].ToString()); }
                catch { }
                //霾平均分
                float hazeAvg = 0f;
                try { hazeAvg = float.Parse(dt_person.Rows[0]["HazeScore"].ToString()); }
                catch { }
                //霾总分
                float hazeSum = 0f;
                try { hazeSum = hazeAvg * dt_work.Rows.Count; }
                catch { }
                //UV平均分
                float uvAvg = 0f;
                try { uvAvg = float.Parse(dt_person.Rows[0]["UVScore"].ToString()); }
                catch { }
                //UV总分
                float uvSum = 0f;
                try { uvSum = uvAvg * dt_work.Rows.Count; }
                catch { }
                //分时段夜间  取个人分时段新表
                DataTable dt_durations = m_Database.GetDataTable(string.Format(sql_DayDurScore, reTimeMonth, forecaster, "6"));
                float nightSum = 0f;
                try { nightSum = float.Parse(dt_durations.Rows[0]["F"].ToString()); }
                catch { }
                //分时段上午
                dt_durations = m_Database.GetDataTable(string.Format(sql_DayDurScore, reTimeMonth, forecaster, "2"));
                float noonSum = 0f;
                try { noonSum = float.Parse(dt_durations.Rows[0]["F"].ToString()); }
                catch { }
                //分时段下午
                dt_durations = m_Database.GetDataTable(string.Format(sql_DayDurScore, reTimeMonth, forecaster, "3"));
                float afterSum = 0f;
                try { afterSum = float.Parse(dt_durations.Rows[0]["F"].ToString()); }
                catch { }

                //总分 pm25
                float pm25Sum = GETDUration("f1", dt_durations, sql_DayDurScoreItems,
                    sql_DayDurScoreItems_times, reTimeMonth, forecaster, sql_shAQI, "PM25");
                //平均分 pm25
                float pm25Avg = (pm25Sum / dt_work.Rows.Count);

                //总分 pm10
                float pm10Sum = GETDUration("f_PM10", dt_durations, sql_DayDurScoreItems,
                    sql_DayDurScoreItems_times, reTimeMonth, forecaster, sql_shAQI, "PM10");
                //平均分 pm10
                float pm10Avg = (pm10Sum / dt_work.Rows.Count);

                //总分 NO2
                float no2Sum = GETDUration("f_NO2", dt_durations, sql_DayDurScoreItems,
                    sql_DayDurScoreItems_times, reTimeMonth, forecaster, sql_shAQI, "NO2");
                //平均分 NO2
                float no2Avg = (no2Sum / dt_work.Rows.Count);

                //总分 O3 1小时
                float o3Sum = GETDUration("f_O3", dt_durations, sql_DayDurScoreItems,
                    sql_DayDurScoreItems_times, reTimeMonth, forecaster, sql_shAQI, "Ozone1");
                //平均分 O3 1小时
                float o3Avg = (o3Sum / dt_work.Rows.Count);

                //总分 O3 8小时
                float o38Sum = GETDUration("f_pm25", dt_durations, sql_DayDurScoreItems,
                    sql_DayDurScoreItems_times, reTimeMonth, forecaster, sql_shAQI, "Ozone8");
                //平均分 O3 8小时
                float o38Avg = (o38Sum / dt_work.Rows.Count);

                //总分环境 夜间 sum(F) +  上午  sum(F)  + 下午 sum(F)
                float EnvSum = 0f;
                try { EnvSum = nightSum + noonSum + afterSum; }
                catch { }
                //总分环境平均分
                float EnvAvg = 0f;
                try { EnvAvg = (EnvSum / dt_work.Rows.Count); }
                catch { }
                //环境总分 =0.1*EW9+0.3*DY9+0.3*DO9+0.3*DF9
                float EnvSUMS = 0f;
                try {
                    string sql_env = "select sum(FF) as 'FF' from [T_DayScore] where convert(varchar(7),LST,120)='" + reTimeMonth + "' and userID='" + forecaster + "'";
                    DataTable dt=m_Database.GetDataTable(sql_env);
                    EnvSUMS = float.Parse(dt.Rows[0]["FF"].ToString());
                }
                catch { }
                //日评分
                float DaySum = 0f;
                try
                {
                    string sql_env = "select sum(F) as 'F' from [T_DayScore] where convert(varchar(7),LST,120)='" + reTimeMonth + "' and userID='" + forecaster + "'";
                    DataTable dt = m_Database.GetDataTable(sql_env);
                    DaySum = float.Parse(dt.Rows[0]["F"].ToString());
                }
                catch { }
                //个人成绩   =总分环境*0.6+霾总分*0.4+UV总分+扣分+科室工作加分+带班加分+国家局AQI总分
                double PersonScore = 0f;
                try
                {
                    float JobScore = 0f;
                    try
                    {
                        JobScore = float.Parse(dt_person.Rows[0]["JobScore"].ToString());
                    }
                    catch { }
                    //取科室加分JobScore
                    float DeductScore = 0f;
                    try
                    {
                        DeductScore = float.Parse(dt_person.Rows[0]["DeductScore"].ToString());
                    }
                    catch { }
                    //取扣分 DeductScore
                    float AddScore = 0f;
                    try
                    {
                        AddScore = float.Parse(dt_person.Rows[0]["AddScore"].ToString());
                    }
                    catch { }
                    //取带班加分 AddScore
                    PersonScore = (EnvSum * 0.6 + hazeSum * 0.4 + uvSum + JobScore + DeductScore + AddScore + ChinaAQISum);
                }
                catch { }

                //删除数据
                string sql_del = "delete from [T_Evaluate] where time = '{0}' and forecaster='{1}' ";
                m_Database.Execute(string.Format(sql_del, reTime, forecaster));
                //插入数据
                #region
                string sql_insert = "INSERT INTO [dbo].[T_Evaluate] " +
                                 "   ([time] " +
                                 "  ,[forecaster] " +
                                 "  ,[bc] ,[cnAQIsum]  ,[cnAQIaverge] " +
                                 "  ,[totalEnvir]  ,[totalPM25] " +
                                 "  ,[totalO31h] ,[totalO38h] " +
                                 "  ,[totalPM10]    ,[totalNO2] " +
                                 "  ,[totalHAZE] ,[totalUV] " +
                                 "  ,[singleEnvir]  ,[singlePM25] " +
                                 "  ,[singleO31h] ,[singleO38h] " +
                                 "  ,[singlePM10] ,[singleNO2] " +
                                 "  ,[singleHAZE] ,[singleUV] " +
                                 "  ,[periodNight] ,[periodMorning] " +
                                 "  ,[periodAfternoon] ,[dayScore] " +
                                 "  ,[envirSumScore] ,[personScore])" +
                                 "VALUES  ('{0}' " +
                                  "     ,'{1}'  ,'{2}' ,'{3}' " +
                                  "     ,'{4}' ,'{5}' " +
                                  "     ,'{6}'  ,'{7}' " +
                                  "     ,'{8}' ,'{9}'  ,'{10}'  ,'{11}' " +
                                  "     ,'{12}'  ,'{13}' ,'{14}' " +
                                  "     ,'{15}' ,'{16}' " +
                                  "     ,'{17}' ,'{18}' " +
                                  "     ,'{19}' ,'{20}' " +
                                  "     ,'{21}' ,'{22}' " +
                                  "     ,'{23}' ,'{24}' " +
                                  "     ,'{25}'  ,'{26}')";
                #endregion
                m_Database.Execute(string.Format(sql_insert, reTime, forecaster,
                                                 worknum, ChinaAQISum, ChinaAQIAvg,
                                                 EnvSum, pm25Sum, o3Sum, o38Sum, pm10Sum,
                                                 no2Sum, hazeSum, uvSum, EnvAvg,
                                                 pm25Avg, o3Avg, o38Avg, pm10Avg, no2Avg,
                                                 hazeAvg, uvAvg, nightSum, noonSum, afterSum,
                                                 DaySum, EnvSUMS, PersonScore));
                //更新数据
                string sql_update = "update T_PersonScore set WorkCount='{2}',SumSEMCScore='{3}',AVGSEMCScore='{4}', " +
                                    "PM25Score='{5}',PM10Score='{6}',O31hScore='{7}',O38hScore='{8}',NO2Score='{9}',HazeScore='{10}',UVScore='{11}' " +
                                    "    where LST='{0}' and UserName='{1}' ";
                m_Database.Execute(string.Format(sql_update, reTime, forecaster, worknum, EnvSum, EnvAvg,
                                                             pm25Avg, o3Avg, o38Avg, pm10Avg, no2Avg, hazeAvg, uvAvg));

                processCount++;
            }
            catch {  }
        }

        if (processCount > 0)
            return "处理成功！";
        else
            return "处理失败，请检查相关数据是否正确！";
        
    }

    public static float GETDUration(string f, DataTable dt_durations, string sql_DayDurScoreItems,
          string sql_DayDurScoreItems_times, string reTimeMonth, string forecaster, string sql_shAQI, string itemName)
    {
        Database m_Database = new Database();
        float result = 0f;
        //sum(上午级别 f1)  +  sum(下午级别 f1) +  sum(pm2.5实况等级)  这里为什么是加实况的等级
        float sumSW = 0f;
        DataTable dt_level = m_Database.GetDataTable(string.Format(sql_DayDurScoreItems, reTimeMonth, forecaster, "2", f));
        try { sumSW = float.Parse(dt_level.Rows[0]["F"].ToString()); }
        catch { }
        float sumXW = 0f;
        dt_level = m_Database.GetDataTable(string.Format(sql_DayDurScoreItems, reTimeMonth, forecaster, "3", f));
        try { sumXW = float.Parse(dt_level.Rows[0]["F"].ToString()); }
        catch { }
        float sumSK = 0f;//实况
        dt_level = m_Database.GetDataTable(string.Format(sql_DayDurScoreItems_times, reTimeMonth, forecaster, "3"));
        foreach (DataRow row_level in dt_level.Rows)
        {
            DataTable dt = m_Database.GetDataTable(string.Format(sql_shAQI, row_level["lst"].ToString()));
            try { sumSK += AQItoGrade(dt.Rows[0][("AQI_" + itemName)].ToString()); }
            catch { }
        }
        //总分  
        try { result = ((sumSW + sumXW + sumSK) / 3); }
        catch { } return result;

    }

    public static int AQItoGrade(string aqi)
    {
        int grade = 0;
        if (int.Parse(aqi) <= 50)
        {
            grade = 1;
        }
        else if (int.Parse(aqi) <= 100)
        {
            grade = 2;
        }
        else if (int.Parse(aqi) <= 150)
        {
            grade = 3;
        }
        else if (int.Parse(aqi) <= 200)
        {
            grade = 4;
        }
        else if (int.Parse(aqi) <= 300)
        {
            grade = 5;
        }
        else
        {
            grade = 6;
        }
        return grade;
    }


    private static void PersonDurationDown(string date)
    {
        Database m_Database = new Database();
        DateTime staTime = DateTime.Parse(date);
        DateTime endTime = staTime.AddMonths(1).AddDays(-1);
        //实况数据
        string sql_RT = "select * from T_shiTable where Module='RT' and LST='{0}' and DurationID='{1}' ";
        //预报数据
        string sql_Forecast = "select x.*,y.userName from T_EvaluateBasicData x  left join  [D_UserCode] y on x.number=y.userCode where  x.date='{0}'";
        //现有数据
        string sql_data = "select * from T_DurationEvaluation  where Module='Manual' and DurationID='{0}' and LST='{1}'";
        //三个时段
        string[] durations = { "6", "2", "3" };
        //string[] durationsName = { "a", "mn", "an" };
  
        for (DateTime beginTime = staTime; beginTime <= endTime; beginTime = beginTime.AddDays(1))
        {
            //计算三个时段  只加工4个污染物的精度评分
            foreach (string duration in durations)
            {
                try
                {
                    string ReTime = beginTime.ToString("yyyy-MM-dd 00:00:00");
                    string ReTime2 = beginTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    //预报数据
                    DataTable dt_forecast = m_Database.GetDataTable(string.Format(sql_Forecast, ReTime));
                    //如果没有预报数据则不进行计算
                    if (dt_forecast == null || dt_forecast.Rows.Count <= 0)
                        continue;

                    string forecaster = dt_forecast.Rows[0]["userName"].ToString();
                    //现有数据
                    DataTable dt_data = m_Database.GetDataTable(string.Format(sql_data, duration, ReTime));

                    //实况数据
                    DataTable dt_rt = m_Database.GetDataTable(string.Format(sql_RT, ReTime2, duration));
                    if (duration == "6") //夜间属于前一天所以不需要加一天
                        dt_rt = m_Database.GetDataTable(string.Format(sql_RT, ReTime, duration));

                    //计算f0  取现有的数据
                    float f0 = 0f;
                    try { f0 = float.Parse(dt_data.Rows[0]["f0"].ToString()); }
                    catch { }
                    //计算f1
                    float f1 = 0f;
                    try { f1 = float.Parse(dt_data.Rows[0]["f2"].ToString()); }
                    catch { }  //这个给的excel表格 f1=f2
                    //计算f2
                    float f2 = 0f;
                    try { f2 = float.Parse(dt_data.Rows[0]["f1"].ToString()); }
                    catch { }  //这个给的excel表格 f2=f1
                    //计算f3
                    float f3 = 0f;
                    try { f3 = float.Parse(dt_data.Rows[0]["f3"].ToString()); }
                    catch { }
                    //计算f4
                    float f4 = 0f;
                    try { f4 = float.Parse(dt_data.Rows[0]["f4"].ToString()); }
                    catch { }

                    //计算精度f_pm2.5 
                    //=IF(昨日夜间实况<51,MAX(1-ABS(昨日夜间实况-昨日夜间预报)/MAX(昨日夜间实况,50),0)*100,
                    //MAX(1-ABS(昨日夜间实况-昨日夜间预报)/MAX(昨日夜间实况,昨日夜间预报),0)*100) 
                    float f_pm25 = 0f;
                    try
                    {
                        #region
                        //实况值  AQI  预报 AQI 
                        float rt_data = 0f;
                        try { rt_data = float.Parse(dt_rt.Select("ItemID='1'")[0]["AQI"].ToString()); }
                        catch { }
                        float fore_data = 0f;
                        try
                        {
                            string x = "an";
                            if (duration == "2")
                                x = "m";
                            if (duration == "3")
                                x = "a";

                            fore_data = float.Parse(dt_forecast.Rows[0][(x + "_pm25")].ToString());

                        }
                        catch { }
                        #endregion
                        //应用公式 
                        if (rt_data < 51)
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, 50);
                            f_pm25 = Math.Max(y, 0) * 100;
                        }
                        else
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, fore_data);
                            f_pm25 = Math.Max(y, 0) * 100;
                        }
                    }
                    catch { }
                    //计算f_pm10  重新计算
                    float f_pm10 = 0f;
                    try
                    {
                        #region
                        //实况值  AQI  预报 AQI 
                        float rt_data = 0f;
                        try { rt_data = float.Parse(dt_rt.Select("ItemID='2'")[0]["AQI"].ToString()); }
                        catch { }
                        float fore_data = 0f;
                        try
                        {
                            string x = "an";
                            if (duration == "2")
                                x = "m";
                            if (duration == "3")
                                x = "a";

                            fore_data = float.Parse(dt_forecast.Rows[0][(x + "_pm10")].ToString());

                        }
                        catch { }
                        #endregion
                        //应用公式 
                        if (rt_data < 51)
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, 50);
                            f_pm10 = Math.Max(y, 0) * 100;
                        }
                        else
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, fore_data);
                            f_pm10 = Math.Max(y, 0) * 100;
                        }
                    }
                    catch { }
                    //计算f_NO2  重新计算
                    float f_no2 = 0f;
                    try
                    {
                        #region
                        //实况值  AQI  预报 AQI 
                        float rt_data = 0f;
                        try { rt_data = float.Parse(dt_rt.Select("ItemID='3'")[0]["AQI"].ToString()); }
                        catch { }
                        float fore_data = 0f;
                        try
                        {
                            string x = "an";
                            if (duration == "2")
                                x = "m";
                            if (duration == "3")
                                x = "a";

                            fore_data = float.Parse(dt_forecast.Rows[0][(x + "_NO2")].ToString());

                        }
                        catch { }
                        #endregion
                        //应用公式 
                        if (rt_data < 51)
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, 50);
                            f_no2 = Math.Max(y, 0) * 100;
                        }
                        else
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, fore_data);
                            f_no2 = Math.Max(y, 0) * 100;
                        }
                    }
                    catch { }
                    //计算f_03(031小时)  重新计算    
                    float f_03 = 0f;
                    try
                    {
                        #region
                        //实况值  AQI  预报 AQI 
                        float rt_data = 0f;
                        try { rt_data = float.Parse(dt_rt.Select("ItemID='4'")[0]["AQI"].ToString()); }
                        catch { }
                        float fore_data = 0f;
                        try
                        {
                            string x = "an";
                            if (duration == "2")
                                x = "m";
                            if (duration == "3")
                                x = "a";

                            fore_data = float.Parse(dt_forecast.Rows[0][(x + "_O31")].ToString());

                        }
                        catch { }
                        #endregion
                        //应用公式 
                        if (rt_data < 51)
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, 50);
                            f_03 = Math.Max(y, 0) * 100;
                        }
                        else
                        {
                            float y = 1 - Math.Abs(rt_data - fore_data) / Math.Max(rt_data, fore_data);
                            f_03 = Math.Max(y, 0) * 100;
                        }
                    }
                    catch { }
                    //计算F 这个要重新计算下
                    double F = 0f;
                    try
                    {
                        //=IF(MAX(BL7:BP7)=1,,)+f0
                        string sql = "select max(Grade) as 'Grade' from T_shiTable where Module='RT'" +
                           "and DurationID='" + duration + "'  and LST='" + ReTime2 + "' and ITEMID IN (1,2,3)";
                        if (duration == "6")
                        {
                            sql = "select max(Grade) as 'Grade' from T_shiTable where Module='RT'" +
                           "and DurationID='" + duration + "'  and LST='" + ReTime + "' and ITEMID IN (1,2,3)";
                        }
                        if (duration == "3")
                        {
                            //下午要考虑03情况
                            sql = "select max(Grade) as 'Grade' from T_shiTable where Module='RT'" +
                             "and DurationID='" + duration + "'  and LST='" + ReTime2 + "' and ITEMID IN (1,2,3,4)";
                        }
                        DataTable dt = m_Database.GetDataTable(sql);
                        if (float.Parse(dt.Rows[0]["Grade"].ToString()) > 0)
                            F = ((0.3 * f1 + 0.7 * (f_pm25 + f_pm10 + f_no2)) / 3) + f0;
                        else
                            F = (0.1 * f2 + 0.2 * f1 + 0.3 * f3 + 0.4 * f4) + f0;
                    }
                    catch { }

                    //入库
                    string del_sql = "delete from T_DayDurScore where LST='{0}' and DurationID='{1}'";
                    del_sql = string.Format(del_sql, ReTime, duration);
                    m_Database.Execute(del_sql);

                    string insert_sql = "insert into  T_DayDurScore values('{0}','{1}','{2}','{3}'," +
                                                                          "'{4}','{5}','{6}','{7}'," +
                                                                          "'{8}','{9}','{10}','{11}','{12}')";
                    del_sql = string.Format(insert_sql, ReTime, forecaster,
                        duration, f_pm25, f_pm10, f_no2, f_03, f0, f1, f2, f3, f4, F);
                    m_Database.Execute(del_sql);
                }
                catch { }
            }
        }
        // }

    }

    //计算个人日评分  
    private static void DayDown(string date)
    {

        DateTime staTime = DateTime.Parse(date);
        DateTime endTime = staTime.AddMonths(1).AddDays(0);

        for (DateTime beginTime = staTime; beginTime <= endTime; beginTime = beginTime.AddDays(1))
        {

            Database m_Database = new Database();
            DateTime staTimeMonth = DateTime.Parse(beginTime.ToString("yyyy-MM-dd 00:00:00"));
            //日数据只算前一天的所以要减去一天
            //实况
            string sql_rt = "select * from V_24hAQI  where area='上海市' and Time_point='{0}'";
            //预报24小时数据
            string sql_for = "select * from [dbo].[T_ChinaValue] where Module='Manual' and LST='{0}' order by lst asc";

            string reTime = staTimeMonth.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            string LST = staTimeMonth.ToString("yyyy-MM-dd 00:00:00");
            DataTable dt_rt = m_Database.GetDataTable(string.Format(sql_rt, LST));
            DataTable dt_fore = m_Database.GetDataTable(string.Format(sql_for, reTime));

            if (dt_fore == null || dt_fore.Rows.Count <= 0)
                continue;

            if (dt_rt == null || dt_rt.Rows.Count <= 0)
                continue;

            float f_pm25 = 0f;
            try
            {
                //=IF(CJ5<51,MAX(1-ABS(CJ5-CQ5)/MAX(CJ5,50),0)*100,
                //     MAX(1-ABS(CJ5-CQ5)/MAX(CJ5,CQ5),0)*100)
                float pm25_rt = float.Parse(dt_rt.Rows[0]["AQI_PM25"].ToString());
                float pm25_for = float.Parse(dt_fore.Rows[0]["PM25"].ToString());
                if (pm25_rt < 51)
                {
                    f_pm25 = Math.Max(1 - Math.Abs(pm25_rt - pm25_for) / Math.Max(pm25_rt, 50), 0) * 100;
                }
                else
                {
                    f_pm25 = Math.Max(1 - Math.Abs(pm25_rt - pm25_for) / Math.Max(pm25_rt, pm25_for), 0) * 100;
                }
            }
            catch { }
            float f_038 = 0f;
            try
            {
                float O3_rt = float.Parse(dt_rt.Rows[0]["AQI_Ozone1"].ToString());
                float O3_for = float.Parse(dt_fore.Rows[0]["O3"].ToString());
                if (O3_rt < 51)
                {
                    f_038 = Math.Max(1 - Math.Abs(O3_rt - O3_for) / Math.Max(O3_rt, 50), 0) * 100;
                }
                else
                {
                    f_038 = Math.Max(1 - Math.Abs(O3_rt - O3_for) / Math.Max(O3_rt, O3_for), 0) * 100;
                }
            }
            catch { }

            float f_pm10 = 0f;
            try
            {
                float pm10_rt = float.Parse(dt_rt.Rows[0]["AQI_PM10"].ToString());
                float pm10_for = float.Parse(dt_fore.Rows[0]["PM10"].ToString());
                if (pm10_rt < 51)
                {
                    f_pm10 = Math.Max(1 - Math.Abs(pm10_rt - pm10_for) / Math.Max(pm10_rt, 50), 0) * 100;
                }
                else
                {
                    f_pm10 = Math.Max(1 - Math.Abs(pm10_rt - pm10_for) / Math.Max(pm10_rt, pm10_for), 0) * 100;
                }
            }
            catch { }

            float f_no2 = 0f;
            try
            {
                float NO2_rt = float.Parse(dt_rt.Rows[0]["AQI_NO2"].ToString());
                float NO2_for = float.Parse(dt_fore.Rows[0]["NO2"].ToString());
                if (NO2_rt < 51)
                {
                    f_no2 = Math.Max(1 - Math.Abs(NO2_rt - NO2_for) / Math.Max(NO2_rt, 50), 0) * 100;
                }
                else
                {
                    f_no2 = Math.Max(1 - Math.Abs(NO2_rt - NO2_for) / Math.Max(NO2_rt, NO2_for), 0) * 100;
                }
            }
            catch { }


            float f1 = 0f;
            try
            {
                //IF(ABS(MAX(DZ5:EF5)-MAX(EG5:EM5))=0,100,IF(ABS(MAX(DZ5:EF5)-MAX(EG5:EM5))=1,50,0))
                int grade_rt = AQItoGrade(dt_rt.Rows[0]["AQI"].ToString());
                int grade_for = int.Parse(dt_fore.Rows[0]["Grade"].ToString());
                if (Math.Abs(grade_rt - grade_for) == 0)
                    f1 = 100;
                else if (Math.Abs(grade_rt - grade_for) == 1)
                    f1 = 50;
            }
            catch { }

            float f2 = 0f;
            try
            {

            }
            catch { }

            float f3 = 0f;
            try
            {
                // =IF(MAX(CJ7:CP7)=CJ7,EP7,IF(MAX(CJ7:CP7)=CL7,EQ7,IF(MAX(CJ7:CP7)=CN7,ER7,IF(MAX(CJ7:CP7)=CP7,ES7))))
                string Primary_pollutant = dt_rt.Rows[0]["Primary_pollutant"].ToString();
                if (Primary_pollutant.IndexOf("PM25") >= 0)
                    f3 = f_pm25;
                if (Primary_pollutant.IndexOf("Qzone") >= 0)
                    f3 = f_pm10;
                if (Primary_pollutant.IndexOf("PM10") >= 0)
                    f3 = f_038;
                if (Primary_pollutant == "NO2")
                    f3 = f_no2;
            }
            catch { }

            float f4 = 0f;
            try
            {
                // EQ: 038小时  ES :  NO2  ER: PM10  EP PM25
                //=IF(MAX(CJ7:CP7)=CJ7,(EQ7+ES7+ER7)/3,IF(MAX(CJ7:CP7)=CL7,(EP7+ES7+ER7)/3,IF(MAX(CJ7:CP7)=CN7,(EP7+EQ7+ES7)/3,IF(MAX(CJ7:CP7)=CP7,(EP7+EQ7+ER7)/3))))
                string Primary_pollutant = dt_rt.Rows[0]["Primary_pollutant"].ToString();
                if (Primary_pollutant.IndexOf("PM25") >= 0)
                    f4 = (f_038 + f_no2 + f_pm10) / 3;
                if (Primary_pollutant.IndexOf("Qzone") >= 0)
                    f4 = (f_pm25 + f_no2 + f_pm10) / 3;
                if (Primary_pollutant.IndexOf("PM10") >= 0)
                    f4 = (f_pm25 + f_038 + f_no2) / 3;
                if (Primary_pollutant == "NO2")
                    f4 = (f_pm25 + f_038 + f_pm10) / 3;
            }
            catch { }

            float f0 = 0f;
            try
            {

            }
            catch { }

            //日平均分
            double F = 0f;
            try
            {
                //=IF(MAX(DZ17:EF17)=1,0.3*EN17+0.7*(EP17+EQ17+ER17+ES17)/4,0.1*EO17+0.2*EN17+0.3*ET17+0.4*EU17)+EV17
                int grade_rt = AQItoGrade(dt_rt.Rows[0]["AQI"].ToString());
                if (grade_rt == 1)
                    F = 0.3 * f1 + 0.7 * (f_pm25 + f_038 + f_pm10 + f_no2) / 4;
                else
                    F = 0.1 * f2 + 0.2 * f1 + 0.3 * f3 + 0.4 * f4;

                F = F + f0;
            }
            catch { }
            //日总分
            double FF = 0f;
            string forecaster = "";
            try
            {
                //=0.1*EW10+0.3*DY10+0.3*DO10+0.3*DF10
                string sql = "select * from T_DayDurScore where LST='{0}'";
                DataTable dt = m_Database.GetDataTable(string.Format(sql, reTime));
                forecaster = dt.Rows[0]["userID"].ToString();
                float fxw = float.Parse(dt.Select("durationID='3'")[0]["F"].ToString());
                float fsw = float.Parse(dt.Select("durationID='2'")[0]["F"].ToString());
                float fyj = float.Parse(dt.Select("durationID='6'")[0]["F"].ToString());
                FF = 0.1 * F + 0.3 * fxw + 0.3 * fsw + 0.3 * fyj;
            }
            catch { }

            //操作数据库
            //删除数据
            if (forecaster == "")  //没有加工出来就不要操作下面的代码了
                continue;

            string sql_del = "delete from [T_DayScore] where lst = '{0}' and userID='{1}' ";
            m_Database.Execute(string.Format(sql_del, reTime, forecaster));
            //插入数据
            #region
            string insert_sql = "insert into  T_DayScore values('{0}','{1}','{2}','{3}'," +
                                                                             "'{4}','{5}','{6}','{7}'," +
                                                                             "'{8}','{9}','{10}','{11}','{12}')";
            m_Database.Execute(string.Format(insert_sql, reTime, forecaster, f_pm25, f_pm10, f_no2, f_038, f0, f1, f2, f3, f4, F, FF));
            #endregion
        }
    }

}