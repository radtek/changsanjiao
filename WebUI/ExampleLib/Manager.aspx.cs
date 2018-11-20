using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;
using System.Data;

public partial class ExampleLib_Manager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static List<DataTable> InitControl()
    {
        List<DataTable> result = new List<DataTable>();
        string strSql = "SELECT DM,MC FROM D_POLUTELEVEL";
        Database db = new Database();
        result.Add(db.GetDataTable(strSql));
        strSql = "SELECT DM,MC,PERIOD FROM D_SEASON";
        result.Add(db.GetDataTable(strSql));
        strSql = "SELECT DM,MC,SEASON FROM D_WeatherType";
        result.Add(db.GetDataTable(strSql));
        return result;
    }

    [WebMethod]
    public static DataTable GetImg(string date)
    {
        string strSql = "SELECT FOLDER,NAME FROM T_HistoryWeatherPic WHERE FORECASTDATE='" + date + "'";
        Database db = new Database();
        return db.GetDataTable(strSql);
    }

}