using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using Readearth.Data;

public partial class Comforecast_cityForeChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static List<string> GetDatas()
    {
        string strSql = "SELECT MONTH(SHOWDATE),DAY(SHOWDATE),year(SHOWDATE),AQI,PRIMEPLU AS PLU FROM FORECAST_DAY_SH WHERE FORECASTDATE IN (SELECT MAX(FORECASTDATE) FROM FORECAST_DAY_SH)";
        Database db = new Database();
        List<string> strReturn = new List<string>();
        DataTable dt = db.GetDataTable(strSql);
        foreach (DataRow dr in dt.Rows)
        {
            strReturn.Add(dr[0].ToString() + "-" + dr[1].ToString() + "&" + dr[2].ToString() + ":" + dr[3].ToString() + ":" + dr[4].ToString());
        }
        return strReturn;
    }
}