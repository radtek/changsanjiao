<%@ WebHandler Language="C#" Class="TempSave" %>

using System;
using System.Web;
using MMShareBLL.DAL;
using System.Configuration;

public class TempSave : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string strReportTxt=context.Request["content"];
        string strFileName=context.Request["fileName"];
        string strFtpIP = ConfigurationManager.AppSettings["ftpIP"];
        string strFtpUser = ConfigurationManager.AppSettings["ftpUser"];
        string strFtpPwd = ConfigurationManager.AppSettings["ftpPwd"];
        AQIForecast aqiForecast = new AQIForecast();
        aqiForecast.UpLoadTxtFtp(strReportTxt, strFtpIP, strFtpUser, strFtpPwd, "20151202", strFileName);

        string str = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}