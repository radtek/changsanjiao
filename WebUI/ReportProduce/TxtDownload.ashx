<%@ WebHandler Language="C#" Class="TxtDownload" %>

using System;
using System.Web;

public class TxtDownload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.QueryString["ProductPath"] != null)
        {
            string siteFileUrl = System.Configuration.ConfigurationManager.AppSettings["AQISiteReportURL"].ToString();
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + "AQI\\SiteReport\\" + context.Request.QueryString["ProductPath"].Trim('\'');
            string path = siteFileUrl + context.Request.QueryString["ProductPath"].Trim('\'');
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Exists)
            {
                context.Response.Clear();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + context.Server.UrlEncode(fi.Name));
                context.Response.ContentType = "application/x-download";
                context.Response.Filter.Close();
                context.Response.WriteFile(fi.FullName);
                context.Response.End();
            }
            else
            {
                context.Response.Status = "404 File Not Found";
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "File Not Found";
                context.Response.Write("File Not Found");
                context.Response.End();
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}