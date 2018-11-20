using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

public partial class AQI_PageOfficePreview_FutureTenDaysPreview : System.Web.UI.Page
{
    string filePath = "";
    string wordTempContent = "";
    string strProductName = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        strProductName = Request["ProductName"]; 
        filePath = Request["filePath"];
        string strWordPartJson = ConfigurationManager.AppSettings["WordPartJsonFile"] + strProductName;
        string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        string strDate = DateTime.Now.ToString("yyyy-MM-dd");
        string fileName = strProductName+"_" + strDate + ".txt";
        string strSavePath = strBase + strWordPartJson+"\\" + fileName;
        if (File.Exists(strSavePath))
        {
            StreamReader reader = new StreamReader(strSavePath);
            wordTempContent = reader.ReadToEnd();
            reader.Close();
        }
    }
    protected void PageOfficeCtrl1_Load(object sender, EventArgs e)
    {
        PageOffice.WordWriter.WordDocument wordDoc = new PageOffice.WordWriter.WordDocument();
        if (wordTempContent != "")
        {
            wordTempContent = wordTempContent.Replace("[{", "");
            wordTempContent = wordTempContent.Replace("}]", "");
            wordTempContent = wordTempContent.Replace("},{", "&");
            wordTempContent = wordTempContent.Replace("\"", "");
            string[] wordParts = wordTempContent.Split('&');
            string strBookmarkName = "";
            string strMarkValue = "";
           
            if (wordParts.Length > 0)
            {
                for (int i = 0; i < wordParts.Length; i++)
                {
                    if (wordParts[i].Split(':')[0] == "PO_issueNum")
                    {
                        //strIssueNum = wordParts[i].Split(':')[1];
                    }
                    strBookmarkName = wordParts[i].Split(':')[0];
                    strMarkValue = wordParts[i].Split(':')[1];
                    PageOffice.WordWriter.DataRegion dataRegion = wordDoc.OpenDataRegion(strBookmarkName);
                    strMarkValue = wordParts[i].Split(':')[1];
                    dataRegion.Value = strMarkValue;
                }
            }
        }
        PageOfficeCtrl1.SetWriter(wordDoc);// 注意不要忘记此代码，如果缺少此句代码，不会赋值成功。
        string strWordModelPath = ConfigurationManager.AppSettings["WordModelFilePath"];
        PageOfficeCtrl1.Caption = "文档预览";
        PageOfficeCtrl1.AddCustomToolButton("保存", "Save()", 1);
        // 设置PageOffice组件服务页面
        PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
        // 设置保存文件
        PageOfficeCtrl1.SaveFilePage = "../../AQI/WordProduct/SaveFile.aspx?ProductName=" + strProductName + "&Year=" + DateTime.Now.Year.ToString();
        PageOfficeCtrl1.WebOpen(Request.ApplicationPath +"/"+ strWordModelPath + filePath, PageOffice.OpenModeType.docNormalEdit, "T");
    }
}