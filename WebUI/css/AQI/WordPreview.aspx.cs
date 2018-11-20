using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AQI_WordPreview : System.Web.UI.Page
{
    string filePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        filePath = Request["filePath"];
        //Literal_Info.Text = "filePath的值是：" + filePath;
    }
    protected void PageOfficeCtrl1_Load(object sender, EventArgs e)
    {
        // 设置PageOffice组件服务页面
        PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
        // 设置保存文件页面
        PageOfficeCtrl1.SaveFilePage = "SaveFile.aspx";
        //PageOfficeCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
        // 打开文档
        PageOfficeCtrl1.WebOpen("../AQI/WordModel/" + filePath, PageOffice.OpenModeType.docNormalEdit, "Tom");
    }
}