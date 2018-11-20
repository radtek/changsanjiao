<%@ WebHandler Language="C#" Class="FileExist" %>

using System;
using System.Web;

public class FileExist : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}