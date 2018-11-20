using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Readearth.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebService : System.Web.Services.WebService
{
    private int  minHour;
    private  string stationLimit;
    private Database m_Database;
    public WebService()
    {
        m_Database = new Database();
        minHour =int.Parse(ConfigurationManager.AppSettings["minHour"].ToString());
        stationLimit = ConfigurationManager.AppSettings["stationLimit"].ToString();
    }
    [WebMethod]
    public DataTable limitDataTable(string end)
    {
        end = DateTime.Parse(end).ToString("yyyy/MM/dd HH:00:00");
        string from = DateTime.Parse(end).AddHours(-minHour).ToString("yyyy/MM/dd HH:00:00");
        string strSQL = string.Format("SELECT LST,ITEMID,ItemType,SITEID,DurationID,ParameterID,AggregateID,Value,AQI FROM SEMC_DMC.DBO.LT_RT_SiteData  WHERE LST BETWEEN '{0}' AND '{1}' AND SITEID IN({2})", from, end, stationLimit);
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);
        return dtSiteData;
    }

}

