using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMShareBLL.DAL;
using System.Data;
using System.IO;

public partial class ReportProduce_AQIAreaForecast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            string [] strAQIs = Request["AQI"].ToString().Split(',');
            string[] strAQILevels = Request["aqiLevel"].ToString().Split(',');
            string[] strFirsrPols = Request["firstPol"].ToString().Split(',');
            string[] strHazeLevels = Request["hazeLevel"].ToString().Split(',');
            DataTable pPageTable = new DataTable();
            pPageTable.Columns.Add();
            pPageTable.Columns.Add();
            pPageTable.Columns.Add();
            pPageTable.Columns.Add();
            DataRow pRow;
            for(int i=0;i<10;i++)
            {
                pRow = pPageTable.NewRow();
                pRow[0] = strAQILevels[i];
                pRow[1] = strFirsrPols[i];
                pRow[2] = strAQIs[i];
                pRow[3] = strHazeLevels[i];
                pPageTable.Rows.Add(pRow);                
            }
                      
            AQIForecast aqifore = new AQIForecast();
            //32Down目录下word文档名称
            string strWordPath32Down = Request["txtHideModelUrlDown"];
            //32D根目录下word文档名称
            string strWordPath32 = Request["txtHideModelUrl"];

            //32Down目录下word文档名称前缀
            string strWordPrefix32Down = Request["txtHideDocNamePrefixDown"];
            //32D根目录下word文档名称前缀
            string strWordPrefix32 = Request["txtHideDocNamePrefix32"];

            //32Down目录下word文档名称后缀
            string strWordSufix32Down = Request["txtHideDocNameSufixDown"];
            //32D根目录下word文档名称后缀
            string strWordSufix32 = Request["txtHideDocNameSufix32"];

            string str = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            //暂存生成word存储路径
            string strWordProPath = Request["txtHideWordTempProductPath"];

            string strNewPath32Down = str + strWordProPath + strWordPrefix32Down + DateTime.Now.ToString("yyyyMMdd") +strWordSufix32Down+ ".doc";
            string strNewPath32 = str + strWordProPath + strWordPrefix32 + DateTime.Now.ToString("yyyy-MM-dd") +strWordSufix32+ ".doc";
            //判断文件名是否已存在
            
            //根据word模板填充生成新的word文档
            aqifore.CreateWordFromModel(str + strWordPath32Down, pPageTable, strNewPath32Down);
            aqifore.CreateWordFromModel(str + strWordPath32, pPageTable, strNewPath32);
                       
        }
    }    
}