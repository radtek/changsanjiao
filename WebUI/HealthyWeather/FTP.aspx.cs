using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Web.Services;
using System.Data;

public partial class HealthyWeather_FTP : System.Web.UI.Page
{
    public static Database m_database;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            m_database = new Database();
        }
    }
   
   

}