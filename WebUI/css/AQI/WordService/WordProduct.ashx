<%@ WebHandler Language="C#" Class="WordProduct" %>

using System;
using System.Web;
using System.IO;

public class WordProduct : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //ID为文档的主键，如果ID不为空，则更新数据，否则新建一条记录
        string ID = context.Request.Params["ID"];
        //根据功能确定临时文件夹的名称
        string strFunctionName = context.Request["FunctionFile"];
        //文件名
        string strFileName = context.Request["FileName"];
        if (context.Request.Files.Count > 0)
        {
            string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            HttpPostedFile upPhoto = context.Request.Files[0];
            int upPhotoLength = upPhoto.ContentLength;
            byte[] PhotoArray = new Byte[upPhotoLength];
            Stream PhotoStream = upPhoto.InputStream;
            PhotoStream.Read(PhotoArray, 0, upPhotoLength); //这些编码是把文件转换成二进制的文件
            //string Newfilename = Super.Wdxt.Kpgl.Common.Utils.NewName("") + "_" + Super.Wdxt.Kpgl.Common.getUserBasicInfo.UserId() + ".doc";
            //string strNewFileName = strBase + "AQI/WordProduct/TempWord/" + strFunctionName + "/" + strFileName;
            string strNewFileName = strBase + "AQI/WordProduct/TempWord/" + strFunctionName + "/" + strFileName + ".doc";
           // string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"].ToString();
            if (!File.Exists(strNewFileName))
            {
                FileStream fs = new System.IO.FileStream(strNewFileName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
                fs.Write(PhotoArray, 0, PhotoArray.Length);
                fs.Close();
            }
            string state = context.Request.Params["state"];
            //bool flag = Super.Wdxt.Kpgl.BLL.AssessdocumentsBLL.Edit(ID, strNewFileName, state);
            //if (flag)
            //{
            //    context.Response.Write("succeed");
            //}
            //else
            //{
            //    context.Response.Write("failed");
            //}
            context.Response.End();

            //-------------------------------------------
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}