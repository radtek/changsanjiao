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
        string userInfo = userManager.LoginByIP(clientIP);
        string method= Request["Method"];
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
