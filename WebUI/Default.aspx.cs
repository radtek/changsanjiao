using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MMShareBLL.DAL;
using Readearth.Data;
using ChinaAQI;
public partial class Default : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        #region　ExtJS

        ExtHelper.Add(this.Header, this);

        #endregion

        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        string clientIP = HttpContext.Current.Request.UserHostAddress;
        UserManager userManager = new UserManager();
        Database m_Database=new Database();
        string userInfo = userManager.LoginByIP(clientIP);
        string method= Request["Method"];
        string strSQL = "SELECT  TOP (1)aqi,time_point FROM T_CityAQI WHERE (aqi is not null and area = '上海市') ORDER BY time_point DESC";
        DataTable dt = m_Database.GetDataTable(strSQL);
        string aqi = dt.Rows[0][0].ToString();
        AQIExtention aqiExt = new AQIExtention(int.Parse(aqi), 0);
        string aqiColor = string.Format("class='{0}'", aqiExt.Color);
        AQI.InnerHtml = string.Format("<span {0} title='{2}'>{1}</span>", aqiColor, aqi,DateTime.Parse(dt.Rows[0][1].ToString()).ToString("yyyy-MM-dd HH:00"));

       // loginResult.Value = "{Alias:'系统管理员',Local:'2013年11月02日',JB:1,LoginCount:'4697',UserName:'admin'}";       
        //当根据ip地址获取
        if (method == "loginOut")
            loginResult.Value = "";
        else
        {
            if (userInfo != "")
            {
                string[] userArray = userInfo.Split('|');
                string loginInfo = userManager.Login(userArray[0].ToString(), userArray[1].ToString(), clientIP);
                string[] loginInfomation = loginInfo.Split(new char[] { ',', ':' });
                HttpCookie newCookie = new HttpCookie("User");
                newCookie.Values.Add("name", userArray[0].ToString());
                newCookie.Values.Add("indexUser", loginInfomation[5]);
                newCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(newCookie);
                //Session["SYSTEMUSERID"] = userArray[0].ToString() + "," + loginInfomation[5];
                loginResult.Value = loginInfo;

            }
            else
            {
                loginResult.Value = "";
            }
        } 
    }
}
