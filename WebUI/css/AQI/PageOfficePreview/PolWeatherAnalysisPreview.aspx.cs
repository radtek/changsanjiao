using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PageOffice;
using System.IO;
using System.Drawing;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using MMShareBLL.DAL;

public partial class AQI_PageOfficePreview_PolWeatherAnalysisPreview : System.Web.UI.Page
{
    string filePath = "";
    string wordTempContent = "";
    string strProductName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        strProductName = Request["ProductName"]; 
        string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        string strWordPartJson = ConfigurationManager.AppSettings["WordPartJsonFile"] + strProductName;
        string strDate = DateTime.Now.ToString("yyyy-MM-dd");
        string fileName = strProductName+"_" + strDate + ".txt";
        string strSavePath = strBase + strWordPartJson + "\\" + fileName;
        filePath = Request["filePath"];
        string strYear = Request["PO_year"];
        if (File.Exists(strSavePath))
        {
            StreamReader reader = new StreamReader(strSavePath);
            wordTempContent = reader.ReadToEnd();            
        }        
    }
    protected void PageOfficeCtrl1_Load(object sender, EventArgs e)
    {
        PageOffice.WordWriter.WordDocument wordDoc = new PageOffice.WordWriter.WordDocument();
        string strImgProductBaseUrl = ConfigurationManager.AppSettings["ImgProductBaseURL"];
        string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (wordTempContent != "")
        {
            wordTempContent=wordTempContent.Replace("[{","");
            wordTempContent=wordTempContent.Replace("}]","");
            wordTempContent=wordTempContent.Replace("},{","&");
            wordTempContent = wordTempContent.Replace("\"", "");
             string[] wordParts = wordTempContent.Split('&');
             string strBookmarkName = "";
             string strMarkValue = "";
             if (wordParts.Length > 0)
             {
                 for(int i=0;i<wordParts.Length;i++)
                 {
                     strBookmarkName = wordParts[i].Split(':')[0];
                     strMarkValue = wordParts[i].Split(':')[1];
                     if (strBookmarkName.Contains("PO"))
                     {
                         PageOffice.WordWriter.DataRegion dataRegion = wordDoc.OpenDataRegion(strBookmarkName);
                         //验证是否为本地文件路径
                         string strTempLocalPath = "";
                         AQIForecast aqi = new AQIForecast();
                         strTempLocalPath = aqi.JudgeIsFilePath(wordParts[i]);

                         if (wordParts[i].Contains("base64"))
                         {
                             strMarkValue = wordParts[i].Substring(wordParts[i].IndexOf("base64") + 7);
                             string strImgSuffix = wordParts[i].Substring(wordParts[i].IndexOf("image/") + 6, wordParts[i].IndexOf(';') - wordParts[i].IndexOf("image/") - 6);
                             if (strImgSuffix == "jpeg")
                             {
                                 strImgSuffix = "jpg";
                             }
                             strMarkValue = strMarkValue.Replace("[/image]", "");

                             string strRandomPicName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + strBookmarkName;
                             string strFullPath = strBase + "AQI\\ReplaceImgInWord\\" + strRandomPicName;
                             strMarkValue += "=";
                             MemoryStream stream = new MemoryStream(Convert.FromBase64String(strMarkValue));
                             Bitmap img = new Bitmap(stream);
                             img.Save(strFullPath + "." + strImgSuffix);
                             stream.Close();
                             dataRegion.Value = "[image]" + strFullPath + "." + strImgSuffix + "[/image]";
                         }
                         else if (wordParts[i].Contains("../Product"))
                         {
                             strMarkValue = strMarkValue.Replace("../Product", strImgProductBaseUrl);
                             strMarkValue = strMarkValue.Replace("/", "\\");
                             strMarkValue = strMarkValue.Replace("[\\image]", "[/image]");
                             dataRegion.Value = strMarkValue;
                         }
                         else if (wordParts[i].Contains("../") && wordParts[i].Contains("noImg"))
                         {
                             continue;
                         }
                         else if (wordParts[i].Contains("../Temp"))
                         {
                             strMarkValue = strMarkValue.Replace("../", strBase);
                             strMarkValue = strMarkValue.Replace("/", "\\");
                             strMarkValue = strMarkValue.Replace("[\\image]", "[/image]");
                             dataRegion.Value = strMarkValue;
                         }
                         else
                         {
                             if (strTempLocalPath != "")
                             {
                                 strMarkValue = "[image]" + strTempLocalPath + "[/image]";
                                 dataRegion.Value = strMarkValue;
                             }
                             else
                             {
                                 strMarkValue = wordParts[i].Split(':')[1];
                                 dataRegion.Value = strMarkValue;
                             }
                         }
                     }
                 }
             }
        }

        PageOfficeCtrl1.SetWriter(wordDoc);// 注意不要忘记此代码，如果缺少此句代码，不会赋值成功。

        // 设置PageOffice组件服务页面
        PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
        PageOfficeCtrl1.Caption = "文档预览";
        // 设置保存文件
        PageOfficeCtrl1.SaveFilePage = "../../AQI/WordProduct/SaveFile.aspx?Year=2016&Num=999&ProductName=" + "PolWeatherAnalysis";
        //PageOfficeCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
        // 打开文档
        //PageOfficeLink.OpenWindow("../AQI/PageOfficePreview/PolWeatherAnalysisPreview.aspx?filePath=PolWeatherAnalysis.doc&{parameters}", "width=1200px;height=700px;");
        PageOfficeCtrl1.WebOpen(Request.ApplicationPath+"/AQI/PageOfficeWordModel/" + filePath, PageOffice.OpenModeType.docNormalEdit, "Tom");
    }
}