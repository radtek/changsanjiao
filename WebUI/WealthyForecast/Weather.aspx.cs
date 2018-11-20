using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Web.Services;
using System.Data;
using System.Text;
using WindowsFormsApplication11;

public partial class WealthyForecast_Wealthy : System.Web.UI.Page
{
    public static Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
         m_Database = new Database("DBCONFIGII");
    }

    [WebMethod]
    public static DataTable getWeather()
    {
        List<DataTable> myData=new List<DataTable>();
        String strQuery="SELECT WeaPhenomena FROM T_WeatherPhenomena";
        return m_Database.GetDataTable(strQuery);


    }

    //王斌  2017.5.2
    [WebMethod]
    public static void save(String con,String forecast,String site,String pollutantA,String pollutantM, String aqi,String wind)
    {
        string[] str = con.Split(',');
        string[] strAqi = aqi.Split(',');
        String sqlDel = "DELETE FROM T_WeatherCor WHERE Site='" + site + "' AND ForcasteDate='" + forecast + "'";
        m_Database.Execute(sqlDel);

        String sqlInsert = @"INSERT INTO T_WeatherCor(SiteID,Site,ForcasteDate,MaxTemperature,MinTemperature,MaxTemperatureM,MinTemperatureA,ReaHumidity, 
                     ReaHumidityN,ReaHumidityM,ReaHumidityA,MeanPressure,MeanSpeed,WeaPhenomenaM,
                     WeaPhenomenaN,HazeM,HazeA,SixWindM,SixWindA,rainM,rainA,temperatureM,temperatureA,speedM,speedA,cloudM,cloudA,UVRankM,UVRankA,O3_1HM,O3_1HA,
                     O3_8HM,O3_8HA,PM25M,PM25A,PM10M,PM10A,pollutantA,AQIA,pollutantM,AQIM,wind) values
                    ((SELECT station_co FROM Sta_reg_set WHERE station_name='" + site + "'),'" + site + "','" + forecast + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "','" + str[4] + "','" + str[5] + "','" + str[6] + "','" + str[7] + "','" + str[8] + "','" + str[9] + "','" + str[10] + "','" + str[11] + "','" + str[12] + "','" + str[13] + "','" + str[14] + "','" + str[15] + "','" + str[16] + "','" + str[17] + "','" + str[18] + "','" + str[19] + "','" + str[20] + "','" + str[21] + "','" + str[22] + "','" + str[23] + "','" + str[24] + "','" + str[25] + "','" + str[26] + "','" + str[27] + "','" + str[28] + "','" + str[29] + "','" + str[30] + "','" + str[31] + "','" + str[32] + "','" + str[33] + "','" + pollutantA + "','" + strAqi[1] + "','" + pollutantM + "','" + strAqi[0] + "','" +wind+ "') ";

        if (site == "全部")
        {
            sqlInsert = @"INSERT INTO T_WeatherCor(SiteID,Site,ForcasteDate,MaxTemperature,MinTemperature,MaxTemperatureM,MinTemperatureA,ReaHumidity, 
                     ReaHumidityN,ReaHumidityM,ReaHumidityA,MeanPressure,MeanSpeed,WeaPhenomenaM,
                     WeaPhenomenaN,HazeM,HazeA,SixWindM,SixWindA,rainM,rainA,temperatureM,temperatureA,speedM,speedA,cloudM,cloudA,UVRankM,UVRankA,O3_1HM,O3_1HA,
                     O3_8HM,O3_8HA,PM25M,PM25A,PM10M,PM10A,pollutantA,AQIA,pollutantM,AQIM,wind) values
                    ('10000A','" + site + "','" + forecast + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "','" + str[4] + "','" + str[5] + "','" + str[6] + "','" + str[7] + "','" + str[8] + "','" + str[9] + "','" + str[10] + "','" + str[11] + "','" + str[12] + "','" + str[13] + "','" + str[14] + "','" + str[15] + "','" + str[16] + "','" + str[17] + "','" + str[18] + "','" + str[19] + "','" + str[20] + "','" + str[21] + "','" + str[22] + "','" + str[23] + "','" + str[24] + "','" + str[25] + "','" + str[26] + "','" + str[27] + "','" + str[28] + "','" + str[29] + "','" + str[30] + "','" + str[31] + "','" + str[32] + "','" + str[33] + "','" + pollutantA + "','" + strAqi[1] + "','" + pollutantM + "','" + strAqi[0] + "','"+wind+"') ";
        }

        m_Database.Execute(sqlInsert);
    
    }
    
    [WebMethod]
    public static string refresh(String dates,String site)
    {
        String str="";
        String subStr = "";
        List<DataTable> myData = new List<DataTable>();
        String sqlQuery = @"SELECT TOP 1 MaxTemperature,MinTemperature,MaxTemperatureM,MinTemperatureA,ReaHumidity, 
                     ReaHumidityN,ReaHumidityM,ReaHumidityA,MeanPressure,MeanSpeed,WeaPhenomenaM,
                     WeaPhenomenaN,HazeM,HazeA,SixWindM,SixWindA,rainM,rainA,temperatureM,temperatureA,speedM,speedA,cloudM,cloudA,UVRankM,UVRankA,O3_1HM,O3_1HA,
                     O3_8HM,O3_8HA,PM25M,PM25A,PM10M,PM10A,pollutantM,pollutantA,AQIM,AQIA,wind FROM T_WeatherCor WHERE site='" + site + "' AND ForcasteDate='" + dates + "' ORDER BY ID DESC ";
        DataTable dt = m_Database.GetDataTable(sqlQuery);
        if (dt == null || dt.Rows.Count == 0) {
            return str;
        }
         foreach (DataRow dr in dt.Rows) {
             dr["pollutantM"] = dr["pollutantM"].ToString().TrimStart(',');
             for (int i = 0; i < dt.Columns.Count; i++) {
                 str = str + dr[i].ToString() + "_";
             }
    
         }

         subStr = str.Substring(0, str.Length - 1);
         return subStr;
    }


    [WebMethod]
    public static string refreshII(string dates, string site)
    {
        string subStr = ""; //return subStr;
        m_Database = new Database();
        Utility.GradeGuide.SceneryGuide sg = new Utility.GradeGuide.SceneryGuide(m_Database);
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd 08:00:00");
        string dateTimeII = DateTime.Now.ToString("yyyy-MM-dd 08:00:00");
        int period = 0;
        int index = 1;

        //天气现象
        //取数据库预报的最大数据
        if (DateTime.Now.Hour > 12)
        {
            dateTimeII = DateTime.Now.ToString("yyyy-MM-dd 20:00:00");
        }
        string sql_query = "select  *,DATEADD(HOUR,PERIOD,ForecastDate) as' LST'  from T_ScenAirForecast where Type='综合观景指数' " +
                           " and ForecastDate='" + dateTimeII + "' and station='" + GetSiteID(site)
                             + "' and Convert(varchar(10),DATEADD(HOUR,PERIOD,ForecastDate),120)='" +
                             DateTime.Parse(dates).ToString("yyyy-MM-dd") + "' ";
        m_Database = new Database();
        DataTable dt = m_Database.GetDataTable(sql_query);
        string guid1 = "";
        if (dt != null && dt.Rows.Count > 0)
            guid1 = dt.Rows[0]["GuideLines1"].ToString();
        string tqxx = TQXX(guid1);

        TimeSpan dtime = DateTime.Parse(DateTime.Parse(dates).ToString("yyyy-MM-dd 00:00:00")) -
                             DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        period = int.Parse(dtime.TotalHours.ToString());
        if (period == 0)
            index = 1;
        else if (period == 24)
            index = 2;
        else if (period == 48)
            index = 3;
        else if (period == 72)
            index = 4;

        if (DateTime.Now.Hour > 12)
        {
            period = period - 24;
            dateTime = DateTime.Now.ToString("yyyy-MM-dd 20:00:00");
            index--;
        }
        //Utility.GradeGuide.BaseDatas bd = sg.GetBaseData(DateTime.Parse(dateTime), period,  GetSiteID(site));
        Utility.GradeGuide.SBaseDatas bd = sg.GetBaseData(DateTime.Parse(dateTime), period, GetSiteID(site));

        string mdj = "";
        switch (bd.hazegrade.ToString())
        {
            case "1": mdj = "无霾"; break;
            case "2": mdj = "轻微"; break;
            case "3": mdj = "轻度"; break;
            case "4": mdj = "中度"; break;
            case "5": mdj = "重度"; break;
            case "6": mdj = "严重"; break;
        }
        subStr = "MDJ" + index.ToString() + ":" + mdj + "*";
        subStr += "JYL" + index.ToString() + ":" + bd.rain.ToString() + "*";
        subStr += "PJWD" + index.ToString() + ":" + bd.temp.ToString() + "*";
        subStr += "PJFS" + index.ToString() + ":" + bd.wind.ToString() + "*";
        subStr += "YL" + index.ToString() + ":" + bd.cldf.ToString() + "*";
        subStr += "AQI" + index.ToString() + ":" + bd.aqi.ToString() + "*";
        if (index == 1)
            subStr += "pollutantM" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*";
        else if (index == 2)
            subStr += "pollutantM2" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*";
        else if (index == 3)
            subStr += "pollutantM3" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*";

        subStr += "WeaPhenomenaM:" + tqxx + "*";
        subStr += "WeaPhenomenaN:" + tqxx + "*";
        return subStr;
    }


    /// <summary>
    /// 获取天气现象
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    private  static string TQXX(string guid)
    {
        string tq = "";
        if (guid.IndexOf("弱降水") >= 0)
        {
            tq = "弱降水";
        }
        else if (guid.IndexOf("有降水") >= 0)
        {
            tq = "有降水";
        }
        else if (guid.IndexOf("有明显降水") >= 0)
        {
            tq = "有明显降水";
        }
        else if (guid.IndexOf("晴") >= 0)
        {
            tq = "晴";
        }
        else if (guid.IndexOf("少云") >= 0)
        {
            tq = "少云";
        }
        else if (guid.IndexOf("多云") >= 0)
        {
            tq = "多云";
        }
        else if (guid.IndexOf("阴天") >= 0)
        {
            tq = "阴天";
        }
        return tq;
    }

   [WebMethod]
    public static string refreshIII(string dates, string site)
    {
       //取数据库预报的最大数据
        string sql_query = "select  *,DATEADD(HOUR,PERIOD,ForecastDate) as' LST'  from T_ScenAirForecast where Type='综合观景指数' "+
                           " and ForecastDate=(select top 1 max(ForecastDate ) " +
                            "from T_ScenAirForecast where Type='综合观景指数' ) and station='" + GetSiteID(site) 
                             + "' and Convert(varchar(10),DATEADD(HOUR,PERIOD,ForecastDate),120)='" +
                             DateTime.Parse(dates).ToString("yyyy-MM-dd") + "' ";
        m_Database = new Database();
        DataTable dt=m_Database.GetDataTable(sql_query);
        string maxForecastDate = "";
        string guid1 = "";
        string guid2 = "";
        if (dt != null && dt.Rows.Count > 0)
        {
            maxForecastDate = dt.Rows[0]["ForecastDate"].ToString();
            guid1 = dt.Rows[0]["GuideLines1"].ToString();
            guid2 = dt.Rows[0]["GuideLines2"].ToString();

        }
        else {
            return "";
        }

       //天气现象
        string tqxx = TQXX(guid1);

        //获取数据
        string subStr = "";
        Utility.GradeGuide.SceneryGuide sg = new Utility.GradeGuide.SceneryGuide(m_Database);
        string dateTime = maxForecastDate;
        int period = int.Parse(dt.Rows[0]["period"].ToString());
        int periodII = int.Parse(dt.Rows[0]["period"].ToString());
        int index = 1;
        if (maxForecastDate.IndexOf("20:00:00") >= 0 || DateTime.Now.Hour>12)
            period = period - 24;

        if (maxForecastDate.IndexOf("20:00:00") >= 0)
            periodII = periodII - 24;

        if (period == 0)
            index = 1;
        else if (period == 24)
            index = 2;
        else if (period == 48)
            index = 3;


        Utility.GradeGuide.SBaseDatas bd = sg.GetBaseData(DateTime.Parse(dateTime), periodII, GetSiteID(site));

        string mdj = "";
        switch (bd.hazegrade.ToString()) {
            case "1": mdj = "无霾"; break;
            case "2": mdj = "轻微"; break;
            case "3": mdj = "轻度"; break;
            case "4": mdj = "中度"; break;
            case "5": mdj = "重度"; break;
            case "6": mdj = "严重"; break;
        }
        subStr = "MDJ"   + index.ToString() + ":" + mdj + "*";
        subStr += "JYL"  + index.ToString() + ":" + bd.rain.ToString() + "*";
        subStr += "PJWD" + index.ToString() + ":" + bd.temp.ToString() + "*";
        subStr += "PJFS" + index.ToString() + ":" + bd.wind.ToString() + "*";
        subStr += "YL"   + index.ToString() + ":" + bd.cldf.ToString() + "*";
        subStr += "AQI"  + index.ToString() + ":" + bd.aqi.ToString()  + "*";
        if (index == 1)
            subStr += "pollutantM" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*";
        else if (index == 2)
            subStr += "pollutantM2" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*"; 
        else if (index == 3)
            subStr += "pollutantM3" + ":" + bd.items.ToString().Replace("PM25", "PM2.5").Replace("O38h", "O3").Replace("&", ",") + "*";

        subStr += "WeaPhenomenaM:" + tqxx + "*";
        subStr += "WeaPhenomenaN:" + tqxx + "*";
        
        return subStr;
    }

    [WebMethod]
    public static DataTable getSite()
    {
        String sql = "SELECT station_name FROM  Sta_reg_set WHERE Flag='1002'";
        //添加一个全部
         DataTable dt=m_Database.GetDataTable(sql);
         if (dt != null)
         {
             DataRow newRow = dt.NewRow();
             newRow["station_name"] = "全部";
             dt.Rows.InsertAt(newRow, 0);
         }
         return dt;
    }

    private static string GetSiteID(string name) {

        string siteID = "";
        switch (name)
        {
            case "上海野生动物园": siteID = "10001A"; break;
            case "上海世纪公园": siteID = "10002A"; break;
            case "上海鲜花港": siteID = "10003A"; break;
            case "锦江乐园": siteID = "10004A"; break;
            case "上海金罗店美兰湖景区": siteID = "10005A"; break;
            case "上海马陆葡萄艺术村": siteID = "10012A"; break;//xuehui 03-14
            case "上海欢乐谷": siteID = "10006A"; break;
            case "上海辰山植物园": siteID = "10007A"; break;
            case "枫泾古镇": siteID = "10008A"; break;
            case "金山城市沙滩景区": siteID = "10009A"; break;
            case "廊下生态园": siteID = "10010A"; break;
            case "朱家角古镇": siteID = "10011A"; break;
            case "上海市青少年校外活动营地—东方绿舟": siteID = "10013A"; break;
            case "上海市青少年校外活动营地-东方绿舟": siteID = "10013A"; break;
            case "碧海金沙景区": siteID = "10014A"; break;
            case "上海海湾国家森林公园": siteID = "10015A"; break;
            case "东平国家森林公园": siteID = "10016A"; break;
            case "上海明珠湖·西沙湿地景区": siteID = "10017A"; break;
            case "迪士尼度假区": siteID = "10018A"; break;
            case "全部": siteID = "10004A"; break;
        }
        return siteID;
    }

    [WebMethod]
    public static void Calc(string aqi, string wind, string site, string forecastTimes, string rain, string temp, string cldf, string hze, string items, string windspeed, string tqxxb, string tqxxy)
    {

        string[] sites = { "上海野生动物园", "上海世纪公园", "上海鲜花港", "锦江乐园", "上海金罗店美兰湖景区", 
                           "上海马陆葡萄艺术村", "上海欢乐谷", "上海辰山植物园", "枫泾古镇", 
                           "金山城市沙滩景区","廊下生态园","朱家角古镇","上海市青少年校外活动营地—东方绿舟",
                           "碧海金沙景区","上海海湾国家森林公园","东平国家森林公园","上海明珠湖·西沙湿地景区" ,"迪士尼度假区"};

        string tqxx = tqxxb;//天气现象
        //王斌  2017.5.9
        TimeSpan dtime = DateTime.Parse(DateTime.Parse(forecastTimes).ToString("yyyy-MM-dd 00:00:00")) -
                      DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        string nowTime = DateTime.Now.ToString("yyyy-MM-dd 08:00:00");
        int periods = int.Parse(dtime.TotalHours.ToString());
        
        if (DateTime.Now.Hour > 12)
        {
            periods = periods + 0;
            nowTime = DateTime.Now.ToString("yyyy-MM-dd 20:00:00");
        }
        if (tqxxb != tqxxy)
        {
            tqxx = tqxxb + "转" + tqxxy;
        }
        if (site == "全部")
        {
            foreach (string st in sites)
            {
                double hze1 = 0d;
                string siteID = "";
                switch (hze)
                {
                    case "无霾": hze1 = 1d; break;
                    case "轻微": hze1 = 2d; break;
                    case "轻度": hze1 = 3d; break;
                    case "中度": hze1 = 4d; break;
                    case "重度": hze1 = 5d; break;
                    case "严重": hze1 = 6d; break;// xuehui 0927
                }
                
                siteID = GetSiteID(st);
                Clc Calculate = new Clc();
                items = items.TrimStart(',');
                try
                {
                   // Calculate.CalClearAir(int.Parse(aqi), double.Parse(wind), siteID, forecastTimes);
                }
                catch { }
                try
                {
                    Calculate.CalViewAir(siteID, forecastTimes, double.Parse(rain), double.Parse(temp), wind, double.Parse(cldf), hze1, int.Parse(aqi), items, windspeed, tqxx);
                    //王斌  2017.5.9
                    string del = "DELETE FROM T_ScenAirIndex WHERE STATION= '" + siteID + "' AND ForecastDate='" + nowTime + "' AND PERIOD='"+periods+"' AND TYPE='综合观景指数'";
                    m_Database.Execute(del);
                }
                catch { }
            }
        }
        else
        {
            double hze1 = 0d;
            string siteID = "";
            switch (hze)
            {
                case "无霾": hze1 = 1d; break;
                case "轻微": hze1 = 2d; break;
                case "轻度": hze1 = 3d; break;
                case "中度": hze1 = 4d; break;
                case "重度": hze1 = 5d; break;
                case "严重": hze1 = 6d; break;
            }

            siteID = GetSiteID(site);
            Clc Calculate = new Clc();
            items = items.TrimStart(',');
            //Calculate.CalClearAir(int.Parse(aqi), double.Parse(wind), siteID, forecastTimes);
            try {
                Calculate.CalViewAir(siteID, forecastTimes, double.Parse(rain), double.Parse(temp), wind, double.Parse(cldf), hze1, int.Parse(aqi), items, windspeed, tqxx);
                //王斌  2017.5.9
                string del = "DELETE FROM T_ScenAirIndex WHERE STATION= '" + siteID + "' AND ForecastDate='" + nowTime + "' AND PERIOD='" + periods + "' AND TYPE='综合观景指数'";
                //string del = "DELETE FROM T_ScenAirIndex WHERE STATION= '" + siteID + "' AND ForecastDate='" + forecastTimes + "'AND TYPE=" + "综合观景指数" + "'";
                m_Database.Execute(del);
            }catch(Exception e){}
            
        }
    }

    /*
    /// <summary>
    /// DataTable to json
    /// </summary>
    /// <param name="jsonName">返回json的名称</param>
    /// <param name="dt">转换成json的表</param>
    /// <returns></returns>
    private string DataTableToJson(string jsonName, System.Data.DataTable dt)
    {
        StringBuilder Json = new StringBuilder();
        Json.Append("{\"" + jsonName + "\":[");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Json.Append("[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                    if (j < dt.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("]");
                if (i < dt.Rows.Count - 1)
                {
                    Json.Append(",");
                }
            }
        }
        Json.Append("]}");
        return Json.ToString();
    }
    */
    

    //王斌  2017.5.2
    [WebMethod]
    public static DataTable getWind() {
        List<DataTable> myData = new List<DataTable>();
        string sql = "SELECT wind FROM T_WeatherPhenomena WHERE wind IS NOT NULL";
        return m_Database.GetDataTable(sql);
    }
}