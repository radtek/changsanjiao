using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AQI_PageOfficePreview_UploadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        bool fileOK = false;
        string path = Server.MapPath("~/Temp/");
        if (FileUpload1.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
            String[] allowedExtensions = { ".gif", ".png", ".bmp", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
        }
        if (fileOK)
        {
            try
            {
                FileUpload1.SaveAs(path + FileUpload1.FileName);
                LabMessage1.Text = "文件上传成功.";
                LabMessage2.Text = "<b>原文件路径：</b>" + FileUpload1.PostedFile.FileName + "<br />" +
                              "<b>文件大小：</b>" + FileUpload1.PostedFile.ContentLength + "字节<br />" +
                              "<b>文件类型：</b>" + FileUpload1.PostedFile.ContentType + "<br />";
            }
            catch (Exception ex)
            {
                LabMessage1.Text = "文件上传不成功.";
            }
        }
        else
        {
            LabMessage1.Text = "只能够上传图片文件.";
        }
    }
}