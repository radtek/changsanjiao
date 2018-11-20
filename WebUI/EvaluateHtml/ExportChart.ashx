<%@ WebHandler Language="C#" Class="ExportChart" %>

using System;
using System.Web;
using System.IO;
using MMShareBLL.DAL;
using Aspose.Words.Tables;
using System.Configuration;
public class ExportChart : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        HttpRequest request = context.Request;
        if (request.Form["svgContent"] != null)
        {
            //string siteFileUrl = System.Configuration.ConfigurationManager.AppSettings["AQISiteReportURL"].ToString();
            
            //string ItemName = context.Request["ItemName"];//获取参数    {key:value,}
            //string month =DateTime.Parse(context.Request["dateTime"]).ToString("yyyy_MM");//获取参数    {key:value,}
            // Get HTTP POST form variables, ensuring they are not null.
            string filename = "123";
            //string type = request.Form["type"];
            string type = "image/jpeg";
            int width = 1700;
            string svg = request.Form["svgContent"];
            string strWordTempContent = request.Form["wordTempContent"];
            string strProductName = request.Form["productName"];
            string strImgFilePath = @"E:\Chart\";
            if (
              type != null &&              
              svg != null)
            {
                string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string strEnvScoreImgFilePath = strBase + "AQI\\ReplaceImgInWord\\";
                string strPM25ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM25ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strPM10ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM10ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strNO2ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScoreNO2ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");

                strImgFilePath = strBase + "AQI\\ReplaceImgInWord\\";
                // Create a new chart export object using form variables.
                //Tek4.Highcharts.Exporting.Exporter export = new Tek4.Highcharts.Exporting.Exporter(filename, type, width, svg);
                string[] allSvgs = svg.Split('&');
                if (allSvgs.Length > 0)
                {
                    for (int i = 0; i < allSvgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            filename = strPM25ImgName;
                        }
                        if (i == 1)
                        {
                            filename = strPM10ImgName;
                        }
                        if (i == 2)
                        {
                            filename = strNO2ImgName;
                        }
                        //Tek4.Highcharts.Exporting.ExporterNew export = new Tek4.Highcharts.Exporting.ExporterNew(filename, type, width, allSvgs[i]);
                        //string strFilePath = export.WriteToHttpResponse(context.Response, strImgFilePath);
                                              
                        //string strFilePath = export.SaveImgToPath(strImgFilePath);
                    }
                }
              //  SaveEnvForeScoreWord(strWordTempContent, strProductName);
                // Short-circuit this ASP.NET request and end. Short-circuiting
                // prevents other modules from adding/interfering with the output.
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //context.Response.End();
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    //保存环境气象预报质量评定通报word文档
    public string SaveEnvForeScoreWord(string wordTempContent, string productName)
    {
        if (wordTempContent != "")
        {
            string[] wordParts = wordTempContent.Split('&');
            string strSaveBaseUrl = System.Configuration.ConfigurationManager.AppSettings["WordProductFilePath"];
            string strModelBaseUrl = ConfigurationManager.AppSettings["WordModelFilePath_2"];
            string strImgProductBaseUrl = ConfigurationManager.AppSettings["ImgProductBaseURL"];

            if (wordParts.Length > 0)
            {
                //modelName为Word模板的文件名
                string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string strModelPath = strBase + strModelBaseUrl + productName + ".doc";

                MMShareBLL.DAL.WordHelper wordHelper = new WordHelper(strModelPath);
                Aspose.Words.Tables.Table templateTable = (Table)wordHelper.Document.GetChild(Aspose.Words.NodeType.Table, 0, true);

                string newFileName = "";
                string strIssueNum = "";
                string strBookmarkName = "";
                string strMarkValue = "";
                for (int i = 0; i < wordParts.Length; i++)
                {
                    strBookmarkName = wordParts[i].Split('=')[0];
                    strMarkValue = wordParts[i].Split('=')[1];
                    if (wordParts[i].Split('=')[0] == "PO_issueNum")
                    {
                        strIssueNum = wordParts[i].Split('=')[1];
                    }

                    if (wordParts[i].Split('=')[1].Contains("base64"))
                    {
                        string strImgSuffix = strMarkValue.Substring(strMarkValue.IndexOf("image/") + 6, strMarkValue.IndexOf(';') - strMarkValue.IndexOf("image/") - 6);
                        if (strImgSuffix == "jpeg")
                        {
                            strImgSuffix = "jpg";
                        }
                        strMarkValue = wordParts[i].Split('=')[1].Substring(wordParts[i].Split('=')[1].IndexOf("base64") + 7);
                        strMarkValue = strMarkValue.Replace("[/image]", "");

                        string strRandomPicName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + strBookmarkName;
                        string strFullPath = strBase + "AQI\\ReplaceImgInWord\\" + strRandomPicName;
                        strMarkValue += "=";
                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(strMarkValue));
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(stream);
                        img.Save(strFullPath + "." + strImgSuffix);
                        stream.Close();
                        wordHelper.InsertPic(wordParts[i].Split('=')[0], strFullPath + "." + strImgSuffix, 280, 380);
                    }

                    //else if (wordParts[i].Split('=')[1].Contains("../../"))
                    //{
                    //    strMarkValue = strMarkValue.Replace("[image]","");
                    //    strMarkValue = strMarkValue.Replace("[/image]", "");
                    //    strMarkValue = strMarkValue.Replace("../../", strBase );
                    //    strMarkValue = strMarkValue.Replace("/", "\\");
                    //    wordHelper.InsertPic(wordParts[i].Split('=')[0], strMarkValue, 200, 300);
                    //}
                    else if (wordParts[i].Split('=')[1].Contains("../"))
                    {
                        strMarkValue = strMarkValue.Replace("[image]", "");
                        strMarkValue = strMarkValue.Replace("[/image]", "");
                        if (wordParts[i].Split('=')[1].Contains("noImg"))
                        {
                            strMarkValue = strMarkValue.Replace("../", strBase);
                        }
                        else if (wordParts[i].Split('=')[1].Contains("../Product"))
                        {
                            if (wordParts[i].Split('=')[1].Contains("?V="))
                            {
                                strMarkValue = strMarkValue.Substring(0, strMarkValue.IndexOf("?V="));
                                strMarkValue = strMarkValue.Replace("../Product", strImgProductBaseUrl);
                            }
                            else
                            {
                                strMarkValue = strMarkValue.Replace("../Product", strImgProductBaseUrl);
                            }

                        }
                        else if (wordParts[i].Split('=')[1].Contains("../Temp"))
                        {
                            strMarkValue = strMarkValue.Replace("../", strBase);

                        }
                        strMarkValue = strMarkValue.Replace("/", "\\");
                        wordHelper.InsertPic(wordParts[i].Split('=')[0], strMarkValue, 200, 300);
                    }
                    else
                    {
                        strMarkValue = wordParts[i].Split('=')[1];
                        //wordHelper.Replace(wordParts[i].Split('=')[0], wordParts[i].Split('=')[1]);
                        wordHelper.Replace(wordParts[i].Split('=')[0], wordParts[i].Split('=')[1].TrimStart(' '));

                    }
                }

                //插入HighCharts绘制的图片（已保存在服务器固定路径）
                string strEnvScoreImgFilePath = strBase + "AQI\\ReplaceImgInWord\\";
                string strPM25ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM25ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strPM10ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM10ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strNO2ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScoreNO2ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");

                string strPM10MarkName = "PO_imgPM10";
                string strPM25MarkName = "PO_imgPM25";
                string strNO2MarkName = "PO_imgNO2";
                if (File.Exists(strEnvScoreImgFilePath + strPM10ImgName + "." + "jpg"))
                {
                    wordHelper.InsertPic(strPM10MarkName, strEnvScoreImgFilePath + strPM10ImgName + "." + "jpg", 280, 380);
                }
                if (File.Exists(strEnvScoreImgFilePath + strPM25ImgName + "." + "jpg"))
                {
                    wordHelper.InsertPic(strPM25MarkName, strEnvScoreImgFilePath + strPM25ImgName + "." + "jpg", 280, 380);
                }
                if (System.IO.File.Exists(strEnvScoreImgFilePath + strNO2ImgName + "." + "jpg"))
                {
                    wordHelper.InsertPic(strNO2MarkName, strEnvScoreImgFilePath + strNO2ImgName + "." + "jpg", 280, 380);
                }
                //newFileName = productName + DateTime.Now.Year.ToString() + "_" + strIssueNum + ".doc";
                newFileName = productName + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".doc";
                if (!Directory.Exists(strBase + strSaveBaseUrl + productName + "\\"))
                {
                    Directory.CreateDirectory(strBase + strSaveBaseUrl + productName + "\\");
                }
                string strNewPath = strBase + strSaveBaseUrl + productName + "\\" + newFileName;
                wordHelper.SaveAs(strNewPath, Aspose.Words.SaveFormat.Doc);

                string strImgBaseUrl = ConfigurationManager.AppSettings["EnvImgProductBaseURL"];
                wordTempContent += "&" + strPM10MarkName + "=[image]" + strImgBaseUrl + strPM10ImgName + "." + "jpg" + "[/image]&";
                wordTempContent += "&" + strPM25MarkName + "=[image]" + strImgBaseUrl + strPM25ImgName + "." + "jpg" + "[/image]&";
                wordTempContent += "&" + strNO2MarkName + "=[image]" + strImgBaseUrl + strNO2ImgName + "." + "jpg[/image]";
                //json文本保存结
                string strTextSaveResult = SaveWordContentToText(wordTempContent, productName);
                if (strTextSaveResult == "success")
                {
                    return "success";
                }
            }
        }
        return "fail";
    }

    public string SaveWordContentToText(string wordPartContent, string pruductFileName)
    {
        string jsonContent = "";
        string strWordPartJson = ConfigurationManager.AppSettings["WordPartJsonFile"];
        if (wordPartContent != "")
        {
            string[] wordParts = wordPartContent.Split('&');
            if (wordParts.Length > 0)
            {
                for (int i = 0; i < wordParts.Length; i++)
                {
                    if (wordParts[i] != "")
                    {
                        //string strCellContent = wordParts[i].Split('=')[1];
                        //if (strCellContent.Contains(""))
                        //{
                        //    strCellContent += "=";
                        //}
                        if (i < wordParts.Length - 1)
                        {
                            jsonContent += "{\"" + wordParts[i].Split('=')[0] + "\":\"" + wordParts[i].Split('=')[1] + "\"},";
                        }
                        else
                        {
                            jsonContent += "{\"" + wordParts[i].Split('=')[0] + "\":\"" + wordParts[i].Split('=')[1] + "\"}";
                        }
                    }
                }
            }
            jsonContent = "[" + jsonContent + "]";
            jsonContent = jsonContent.Replace("\n", "");
            string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string strDate = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = pruductFileName + "_" + strDate + ".txt";

            if (!Directory.Exists(strBase + strWordPartJson + pruductFileName))
            {
                Directory.CreateDirectory(strBase + strWordPartJson + pruductFileName + "\\");
            }
            string strSavePath = strBase + strWordPartJson + pruductFileName + "\\" + fileName;
            if (File.Exists(strSavePath))
            {
                File.Delete(strSavePath);
            }
            using (FileStream fs = new FileStream(strSavePath, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(jsonContent);
                sw.Close();
            }
            return "success";
        }
        return "fail";

    }

}