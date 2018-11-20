using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMShareBLL.DAL;
using System.Configuration;
using System.IO;

public partial class SaveFile : System.Web.UI.Page
{
    string fileName = "";
    string strProductName="";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //存储word产品的基础路径，里面包含根据产品类别的子文件夹
            strProductName = Request["ProductName"];
            string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string strWordProFileBasePath = ConfigurationManager.AppSettings["WordProductFilePath"];
            string strWordJsonFileBasePath = ConfigurationManager.AppSettings["WordPartJsonFile"];
            string strFilrPath = strBase + strWordProFileBasePath + strProductName;
            if (!Directory.Exists(strFilrPath))
            {
                Directory.CreateDirectory(strFilrPath);
            }
            strProductName = Request["ProductName"];
            string strFileNamePrefix = "";
            //switch (strProductName)
            //{
            //    case "FutureTenDays":
            //        strFileNamePrefix = "未来10天本市空气质量预报";
            //        break;
            //    case "PolWeatherAnalysis":
            //        strFileNamePrefix = "上海市污染天气过程跟踪解析专报";
            //        break;
            //    case "ImportantWeather":
            //        strFileNamePrefix = "华东区域环境气象专报";
            //        break;
            //    case "WeekPolWeather":
            //        strFileNamePrefix = "长三角地区一周污染天气展望";
            //        break;
            //    case "MainCityForecast":
            //        strFileNamePrefix = "华东区域重点城市预报";
            //        break;
            //    default:
            //        strFileNamePrefix = "";
            //        break;
            //}
            //fileName = strProductName + Request["Year"] + "_" + Request["Num"] + "期.doc";
            fileName = strProductName +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".doc";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();           
                                    
            //应该将文件保存到服务器word文档存储的路径，发布时也从该路径读取
            fs.CustomSaveResult = "需要传回前台页面";
            //fs.SaveToFile("../"+Request.ApplicationPath+"/AQI/WordProduct/" + fs.FileName);            
            fs.SaveToFile(Server.MapPath(strProductName + "/") + fileName);
            fs.Close();


            ////将在PageOffice界面编辑之后的内容保存回Json
            PageOffice.WordReader.WordDocument doc = new PageOffice.WordReader.WordDocument();
            int intMarkCount = doc.DataRegions.Count;
            for (int i = 0; i < intMarkCount; i++)
            {
                //doc.DataRegions
            }
            //获取提交的数值
            PageOffice.WordReader.DataRegion dataUserName = doc.OpenDataRegion("PO_userName");
            PageOffice.WordReader.DataRegion dataDeptName = doc.OpenDataRegion("PO_deptName");
            AQIForecast aqi = new MMShareBLL.DAL.AQIForecast();
            aqi.SaveWordContentToText("", strProductName);
        }
        catch
        {

        }
    }
}
