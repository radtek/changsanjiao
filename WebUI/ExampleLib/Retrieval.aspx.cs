using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Data;

public partial class Example_Retrieval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    [WebMethod]
    public static string QueryHour(string date,string field)
    {
        string strSql = "SELECT STUFF((SELECT ',['+STR(DATEDIFF(SS, '1970-1-1', TIME_POINT),10,0)+'000,' +  CAST("+field+" AS VARCHAR(7)) +']' FROM T_CityAQI WHERE AREA='上海市' AND (TIME_POINT BETWEEN '"
            + DateTime.Parse(date).AddDays(-2).ToString("yyyy-MM-dd") + " 00:00:00' AND '" + DateTime.Parse(date).ToString("yyyy-MM-dd") + " 23:59:59' ) ORDER BY TIME_POINT FOR XML PATH('')),1,1,'')";
        Database db = new Database();
        return "[{\"name\": \"" + field + "\",\"data\":[" + db.GetFirstValue(strSql) + "]}]";
    }

    [WebMethod]
    public static DataSet GetImg(string dateClick, string dateQuery)
    {
        string strSql = "SELECT FOLDER,NAME FROM T_HistoryWeatherPic WHERE FORECASTDATE='" + dateClick
            + "';SELECT FOLDER,NAME FROM T_HistoryWeatherPic WHERE FORECASTDATE='" + dateQuery + " 20:00:00'";
        Database db = new Database();
        return db.GetDataset(strSql);
    }

}