using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Readearth.Data;

public partial class HealthyWeather_HandleCancel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    public static string AgreeRequest(string id,string userId,string type)
    {
        Database db = new Database("DBCONFIGII");
        string strSql = "UPDATE T_PubUser SET CAN" + type + "=0," + type + "_PUBTIME=''," + type + "_PUBLVL='' WHERE USERID=" + userId;
        try
        {
            db.Execute(strSql).ToString();
            return Handled(id,db);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [WebMethod]
    public static string Reject(string id)
    {
        Database db = new Database("DBCONFIGII");
        try
        {
            return Handled(id, db);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private static string Handled(string id, Database db)
    {
        string strSql = "UPDATE T_CancelRequest SET HANDLED=1 WHERE ID=" + id;
        return db.Execute(strSql).ToString();
    
    }

}