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
using Utility.GradeGuide;
using MMShareBLL.DAL;

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
        String strQuery = "SELECT DM,MC FROM Weather_Phenomena";
        return m_Database.GetDataTable(strQuery);
    }

    //保存的数据也要入eledata表，否则重新计算没作用
    [WebMethod]
    public static string save(string name,string val,string LST,string site,string code)
    {
        string tip = "";
        DateTime dNow = DateTime.Now;
        string[] fName = name.Split(',');
        string[] fValue = val.Split(',');
        string[] fCode = code.Split(',');
        string hour = " 06:00:00";
        if (dNow.Hour >= 12)
        {
            hour = " 16:00:00";
        }
        string forecastTime = dNow.ToString("yyyy-MM-dd") + hour;
        LST = DateTime.Parse(LST).ToString("yyyy-MM-dd") + hour;
        //先删掉
        string sql_del = "delete from T_Weather_Cor where station='"+site+"' and lst='"+LST+"' and forecastDate='"+forecastTime+"'";
        m_Database.Execute(sql_del);
        for (int i = 0; i < fValue.Length; i++)
        {
            try {
                string sql_insert = "insert into T_Weather_Cor (station,code,name,value,forecastDate,lst) values ('"+site+"','"+fCode[i]+"','"+fName[i]+"','"+fValue[i]+"','"+forecastTime+"','"+LST+"')";
                m_Database.Execute(sql_insert);
                tip += "success";
            }catch(Exception e){
                tip += "error";
            }
        }
        #region    数据保存到eledata表中做处理   ////////////////////////////////////////////////
        string lst = DateTime.Parse(LST).ToString("yyyy-MM-dd");
        string forecastDate = dNow.ToString("yyyy-MM-dd");
        TimeSpan day = DateTime.Parse(LST) - DateTime.Parse(forecastDate);
        string period = "24";
        if (day.TotalDays == 1)
        {
            period = "48";
        }
        else if (day.TotalDays == 2)
        {
            period = "72";
        }
        SaveToEledata(fValue, fCode, period, site, day, forecastDate, lst);
        #endregion                    ////////////////////////////////////////////////////////////////////
        return tip;
    }

    //删除指数订正的保存数据
    public static void delIndexSTable(string LST,string site,string forecastTime) {
        string del = "delete from T_Weather_Save where forecastDate=(select max(forecastDate) from T_Weather_Save) and lst='" + LST + "' and station='" + site + "'";
        m_Database.Execute(del);
    }
    
    //订正
    [WebMethod]
    public static string Corrent(string name, string code, string val, string LST, string site)
    {
        IndexCalculate indexCal = new IndexCalculate(m_Database);
        LiveIndex liveIndex = new LiveIndex();
        string period = "";
        DateTime dNow = DateTime.Now;
        string hour = " 06:00:00";
        if (dNow.Hour >= 12)
        {
            hour = " 16:00:00";
        }
        string[] _code = code.Split(',');
        string[] _val = val.Split(',');    //不需要.TrimEnd(',')
        //把数据保存到保存表中
        save(name, val, LST, site,code);

        string lst = DateTime.Parse(LST).ToString("yyyy-MM-dd");
        string forecastDate = dNow.ToString("yyyy-MM-dd");
        string[] station = { site };
        TimeSpan day = DateTime.Parse(LST) - DateTime.Parse(forecastDate);
        if (day.TotalDays == 0)
        {
            period = "24";
        }
        else if (day.TotalDays == 1)
        {
            period = "48";
        }
        else if (day.TotalDays == 2)
        {
            period = "72";
        }
        try
        {
            SaveToEledata(_val, _code, period, site, day, forecastDate, lst);
            DateTime foreD = DateTime.Parse(dNow.ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime L = DateTime.Parse(lst + " 00:00:00");
            DataTable dtWeather_IndexResult = indexCal.Index_Calcu(foreD, L, station);
            ///把先前指数订正保存的数据删掉，要不然没效果
            DelWeaSaveData(site, lst, forecastDate,hour);
            //delIndexSTable(lst+hour, site, forecastDate+hour);    //把先前指数订正保存的数据删掉，要不然没效果
            return "success";
        }
        catch (Exception e) {
            return "error";
        }
    }

    //删除订正后保存的数据
    public static void DelWeaSaveData(string station, string lst, string forecastDate,string hour)
    {
        m_Database = new Database("DBCONFIG");
        IndexCalculate indexCal = new IndexCalculate(m_Database);

        string del_Wea_Save = "delete from T_Weather_Save where lst = '" + lst + " "+hour+"' and forecastdate = '" + forecastDate + " "+hour+"'" +
                                " and station='" + station + "'";
        m_Database.Execute(del_Wea_Save);
    }

    public static void SaveToEledata(string[] value, string[] code, string period, string site, TimeSpan day, string forecastDate, string lst)
    {
        try
        {
            DateTime dNow = DateTime.Now;
            string hour = dNow.Hour.ToString() + ":00:00";
            //插入早晚臭氧预报的值固定为夏季为100，冬季为140-150
            int month = dNow.Month;
            int v = 140;
            if (month < 6 || month > 8)
            {
                v = 100;
            }
            string[] codeO3_8 = { "w40", "w54" };
            for (int j = 0; j < codeO3_8.Length; j++)
            {
                string insertO3_8 = "insert into Weather_EleData(lst,ForecastDate,interval,PERIOD,Site,ITEMID,Value,Module)" +
                " values('" + lst + " 00:00:00','" + forecastDate + " " + hour + "','" + day.TotalHours.ToString() + "','" + period + "','" + site + "','" + codeO3_8[j] + "','" + v + "','manual')";
                m_Database.Execute(insertO3_8);
            }

            for (int i = 0; i < value.Length; i++)
            {
                //修改要素订正表中的数据Weather_Eledata
                //判断若Weather_EleData表中有这个要素则更新要素，若没有则插入这个要素信息
                string sql_update_Eledata = "update Weather_EleData set Value='" + value[i] + "',Module ='manual',forecastDate='" + forecastDate + " " + hour + "'";
                string comWhere = " where itemID='" + code[i] + "' and site='" + site + "' and lst between '" + lst + " 00:00:00' and '" + lst + " 23:00:00'";
                string selMaxForecastDateWhere = "" + comWhere + " and forecastDate between '" + forecastDate + " 00:00:00' and '" + forecastDate + " 23:00:00'";
                string where = "" + comWhere + " and ForecastDate =(select max(forecastDate) from Weather_EleData " + comWhere + ") ";
                string sql_query = "select * from Weather_EleData" + where;
                DataTable dt_check = m_Database.GetDataTable(sql_query);
                if (dt_check.Rows.Count > 0)
                {
                    m_Database.Execute(sql_update_Eledata + where);
                }
                else
                {
                    //插入
                    string interval = day.TotalHours.ToString();
                    string insert_value = " values('" + lst + " 00:00:00','" + forecastDate + " " + hour + "','" + interval + "','" + period + "','" + site + "','" + code[i] + "','" + value[i] + "','manual')";
                    string sql_insert = "insert into Weather_EleData(lst,ForecastDate,interval,PERIOD,Site,ITEMID,Value,Module)";
                    m_Database.Execute(sql_insert + insert_value);
                }

            }
        }catch(Exception e){
        
        }
    }

    [WebMethod]
    public static DataTable Refresh(string dates,string site,string day)
    {
        try
        {
            IndexCalculate indexCal = new IndexCalculate(m_Database);
            DateTime dNow = DateTime.Now;
            string hour = " 06:00:00";
            if (dNow.Hour >= 12)
            {
                hour = " 16:00:00";
            }
            DataTable result = new DataTable();   //存放最终经过处理的数据，是返回前台的数据
            string forecastDate = dNow.ToString("yyyy-MM-dd");
            string LST = DateTime.Parse(dates).ToString("yyyy-MM-dd");
            string timeWhere = "lst between '" + LST + " 00:00:00' and '" + LST + " 23:59:59'";
            string timeWhereS = "lst between '" + LST + " 00:00:00' and '" + DateTime.Parse(LST).AddDays(2).ToString("yyyy-MM-dd") + " 23:00:00'";   //获取保存表Cor中三天的预报数据
            //DataTable dt = m_Database.GetDataTable(sql);
            //DataTable dtEleData1 = indexCal.GetEleData1(DateTime.Parse(LST),site);
            DataTable dt_EleData3 = indexCal.GetEleData3(DateTime.Parse(forecastDate), site);   //获取接口中的数据  3天
            DataTable dt_WeaCor = GetWeaCorData(timeWhereS, forecastDate, hour, site);    //获取各个要素在保存表中的数据  T_weather_Cor
            result = MerRefreshData(dt_EleData3, dt_WeaCor, forecastDate);
            return result;
        }catch(Exception e){
            return null;
        }
    }

    public static DataTable MerRefreshData(DataTable dt_EleData,DataTable dt_WeaCor,string forecastDate) {
        dt_EleData.Columns.Remove("PERIOD");
        dt_EleData.Columns.Remove("Site");
        dt_EleData.Columns.Remove("Module");
        DataTable dt = dt_WeaCor.Clone();
        int i = 0, k = 0;
        List<string> LST = new List<string>();   //该发布日期所对应的三天预报日期
        while(i <= 2){
            LST.Add(DateTime.Parse(forecastDate).AddDays(i).ToString("yyyy-MM-dd"));
            i++;
        }
        //遍历获取三天数据
        foreach (string lst in LST) {
            string com = "lst >= '" + lst + " 00:00:00' and lst<='" + lst + " 23:59:00'";
            DataRow [] dr = dt_WeaCor.Select(com);
            if (dr.Length > 0)
            {
                //取保存的数据
                dt = GetDataRowData(dr, dt);
            }
            else { 
                //取接口数据
                dr = dt_EleData.Select("lst >= '"+LST[k]+" 00:00:00' and lst <= '"+LST[k]+" 23:00:00'");
                dt = GetDataRowData(dr,dt);
            }
            k++;
        }
        return dt;
    }

    //把datarow中的数据赋值给一个DataTable
    public static DataTable GetDataRowData(DataRow [] dr,DataTable dt) {
        foreach (DataRow row in dr) {
            dt.Rows.Add(row.ItemArray);
        }
        return dt;
    }

    //获取保存表中的数据weather_cor
    public static DataTable GetWeaCorData(string timeWhere,string forecastDate,string hour,string site)
    {
        string sql = "select lst, forecastDate,code,name,value from T_Weather_Cor where forecastDate =(select MAX(forecastDate) from T_Weather_Cor where "+timeWhere+" and station='"+site+"') and " + timeWhere + " and station='"+site+"'";
        //string sql = "select lst, forecastDate,code,name,value from T_Weather_Cor where forecastDate =(select MAX(forecastDate) from T_Weather_Cor where " + timeWhere + ") and " + timeWhere + "";
        DataTable dt = m_Database.GetDataTable(sql);
        return dt;
    }
    
    //只获取接口数据
    /*  注释掉，这个是后台接口分时段获取数据的，订正后在该时段只获取订正的数据，除非时段变化
     [WebMethod]
    public static DataTable GetEleInterface(string site)
    {
        IndexCalculate indexCal = new IndexCalculate(m_Database);
        string forecastDate = DateTime.Now.ToString("yyyy-MM-dd");
        DataTable dt_EleData3 = new DataTable();
        dt_EleData3 = indexCal.GetEleData3(DateTime.Parse(forecastDate), site);   //获取接口中的数据  3天
        dt_EleData3.Columns["LST"].ColumnName = "lst";
        dt_EleData3.Columns["ITEMID"].ColumnName = "code";
        dt_EleData3.Columns["Value"].ColumnName = "value";
        return dt_EleData3;
    }*/
    [WebMethod]   
    //直接在数据库中获取要素的最新值
    public static DataTable GetEleInterface(string site,string ele) {
        DateTime dNow = DateTime.Now;
        string[] element = ele.Split(',');
        string forecastdate = dNow.ToString("yyyy-MM-dd");
        string sqlQuery = "";
        foreach (string str in element) {
            //获取某个站点3天的预报数据
            for (int i = 0; i < 3; i++)
            {
                string lst = dNow.AddDays(i).ToString("yyyy-MM-dd");
                sqlQuery += "select itemid as code,value,lst from Weather_EleData where LST between '" + lst + " 00:00:00.000' and '" + lst + " 23:00:00.000' and " +
                    "Site='" + site + "' and ITEMID='" + str + "' and ForecastDate=(select MAX(ForecastDate) from Weather_EleData where LST between '" + lst + " 00:00:00.000' and '" + lst + " 23:00:00.000' and Site='" + site + "' and ITEMID='"+str+"') union all ";
            }
        }
        char[] c = "union all ".ToCharArray();
        sqlQuery = sqlQuery.TrimEnd(c);
        DataTable dt = m_Database.GetDataTable(sqlQuery);
        return dt;
    }
    //王斌  2017.5.2
    [WebMethod]
    public static DataTable getWind() {
        List<DataTable> myData = new List<DataTable>();
        string sql = "SELECT wind FROM T_WeatherPhenomena WHERE wind IS NOT NULL";
        return m_Database.GetDataTable(sql);
    }
}