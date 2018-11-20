using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Aspose.Cells;
using Readearth.Data;
using System.Drawing;
using System.IO;
using AQIQuery.aQuery;
public partial class AQI_TransStaion : System.Web.UI.Page
{
    public string m_ForecastDate;
    public Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;

 
            m_ForecastDate = dtNow.AddDays(-1).ToString("yyyy年MM月dd日");
            
            H00.Value = m_ForecastDate;
            creatTable();
            

        }

    }
    private void creatTable()
    {
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
        StringBuilder sb = new StringBuilder();
        string[] SiteName = { "徐汇漕溪路", "闸北共和新路", "黄浦中山南路", "静安延安西路", "浦东东方路", "交通站平均", "国控点平均</br>(常规污染物)" };

        // int[] siteArray = { 254, 255, 256, 257, 258, 0 };//{ 83, 84, 85, 86, 87,0};//select * from dbo.Site
        //int[] parameterArray = { 2, 6, 7, 21, 22, 145, 92, 93, 94, 95, 106, 105, 76, 61, 62, 38 };//
        string[] SiteID = { "255", "257", "254", "256", "258", "0", "888888" };//{ "84", "86", "83", "85", "87", "0" };
        string[] paremterID = { "21", "22", "2", "6", "145", "7", "92", "93", "94", "95", "106", "105", "76", "61", "62", "38" };
        for (int i = 0; i < SiteName.Length; i++)
        {
            newRow = new HtmlTableRow();
            td = new HtmlTableCell();
            if (i == 5 || i == 6) {
                td.Attributes.Add("class", "tableTLeftNew");
            }
            else
            {
                td.Attributes.Add("class", "tableTLeft");
            }
            td.InnerHtml = SiteName[i];
            newRow.Cells.Add(td);
            for (int j = 0; j < paremterID.Length; j++)
            {
                td = new HtmlTableCell();
                sb.Append(string.Format("<div id = 'H{0}{1}'></div>",SiteID[i],paremterID[j]));
                if(j==paremterID.Length-1)
                    td.Attributes.Add("class", "tableTRight");
                else 
                    td.Attributes.Add("class", "tableT");
                td.InnerHtml = sb.ToString();
                sb.Length = 0;
                newRow.Cells.Add(td);
            }
            forecastTable.Rows.Insert(i + 3, newRow);
        }
        newRow = new HtmlTableRow();
        td = new HtmlTableCell();
        td.Attributes.Add("class", "tableTLeftBott");
        td.InnerHtml = "简要情况";
        newRow.Cells.Add(td);

        td = new HtmlTableCell();
        td.Attributes.Add("colspan", "17");
        td.Attributes.Add("class", "tableTRightBott");
        td.InnerHtml = "&nbsp;&nbsp";
        newRow.Cells.Add(td);
        forecastTable.Rows.Insert(forecastTable.Rows.Count, newRow);


        newRow = new HtmlTableRow();
        td = new HtmlTableCell();
        td.Attributes.Add("class", "lastRowNew");
        td.Attributes.Add("colspan", "18");
        td.InnerHtml = "注：“-”表示该点位无此参数设备；“Null”表示由于仪器校准、故障、有效数据不足等原因，该点位相应时段无数据。";
        newRow.Cells.Add(td);
        forecastTable.Rows.Insert(forecastTable.Rows.Count, newRow);

        newRow = new HtmlTableRow();
        td = new HtmlTableCell();
        td.Attributes.Add("class", "lastRow");
        td.Attributes.Add("colspan", "18");
        td.InnerHtml = "编制：<div class='div' >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>审核：<div class='div' >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>签发：<div class='div' >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>上海市环境监测中心";
        newRow.Cells.Add(td);
        forecastTable.Rows.Insert(forecastTable.Rows.Count, newRow);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        string time = DateTime.Parse(Content).ToString("yyyy/MM/dd");
        int[] siteArray = { 255, 257, 254, 256, 258, 0, 888888 };
        int[] parameterArray = { 21, 22, 2, 6, 145, 7, 92, 93, 94, 95, 106, 105, 76, 61, 62, 38 };
        int[] CparameterArray = { 309, 307, 310, 308, 301, 302, 888888, 888888, 888888, 888888, 888888, 888888, 88888, 88888, 888888, 88888 };//
       // string strSQL = string.Format("SELECT SiteID,ParameterID,LST,CONVERT(decimal(10, 1), Value * 1000) AS VALUE FROM SEMC_DMS.DBO.TransData WHERE LST='{0}'", time);
        m_Database = new Database();
        //DataTable dtSiteData = m_Database.GetDataTable(strSQL);


        string sql = "SELECT  SiteID, ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value " +
                    "FROM SEMC_DMS.DBO.Data WHERE siteID IN ( 255, 257, 254, 256, 258) AND parameterID IN " +
                        "(21, 22, 2, 6, 145, 7) and LST='{0}' " +
                            "and DurationID=11  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 group by parameterID,siteID,LST ";
        string sql2 = "SELECT  SiteID, ParameterID, CONVERT(VARCHAR(10),LST,111) as LST,avg(Value) as Value " +
                   "FROM SEMC_DMS.DBO.Data WHERE siteID IN ( 255, 257, 254, 256, 258) AND parameterID IN " +
                       "( 92, 93, 94, 95, 106, 105, 76, 61, 62, 38) and LST='{0}' " +
                           "and DurationID=10  and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 group by parameterID,siteID,LST ";


        string strSQL = string.Format(sql, time);
        DataTable dtSiteData = m_Database.GetDataTable(strSQL);//常规污染物
        strSQL = string.Format(sql2, time);
        DataTable dtSiteDataII = m_Database.GetDataTable(strSQL);//其他污染物



        //********************************************************
        string siteString = "102";// SiteGroup.GetSiteIDbyGroupIDsString("101");
        DateTime dtimeStart = DateTime.Parse(time + " 00:00:00");
        DateTime dtimeEnd = DateTime.Parse(time + " 00:00:00");
        DataTable dtChinaData = Data.GroupDailyAQI(dtimeStart, dtimeEnd, siteString, "309,307,310,308,301,302");
        //********************************************************

        string templateFile = Server.MapPath("../交通站日报表.xls");
        WorkbookDesigner designer = new WorkbookDesigner();
        designer.Open(templateFile);

        Aspose.Cells.Worksheet worksheet = designer.Workbook.Worksheets[0];

        worksheet.Cells[1, 0].PutValue("日期:" + Content);
        string filter = "";
       // if (dtSiteData.Rows.Count > 0)
        //{
        for (int i = 0; i < siteArray.Length; i++)
        {
            for (int j = 0; j < parameterArray.Length; j++)
            {
                filter = string.Format("SiteID = '{0}' AND ParameterID = {1}", siteArray[i], parameterArray[j]);
                if (i <= 4)
                {
                    if (j > 5)
                    {
                        #region
                        DataRow[] rows = dtSiteDataII.Select(filter);
                        if (rows.Length > 0)
                        {
                            if (rows[0][1].ToString() == "92" ||
                               rows[0][1].ToString() == "93" ||
                               rows[0][1].ToString() == "94" ||
                               rows[0][1].ToString() == "95" ||
                               rows[0][1].ToString() == "106" ||
                               rows[0][1].ToString() == "105" ||
                               rows[0][1].ToString() == "61" ||
                               rows[0][1].ToString() == "76" ||
                               rows[0][1].ToString() == "62")
                            {
                                // 黄色的除以1000，保留三位小数
                                worksheet.Cells[5 + i, j + 1].PutValue(Math.Round(double.Parse(rows[0][3].ToString()), 3).ToString());
                            }
                            else if (rows[0][1].ToString() == "38")
                            {
                                //绿色的除以1000，保留0位小数
                                worksheet.Cells[5 + i, j + 1].PutValue(Math.Round(double.Parse(rows[0][3].ToString()),0).ToString());
                            }
                        }
                        else
                        {
                            if (siteArray[i] == 254 || siteArray[i] == 256 || siteArray[i] == 258 || siteArray[i] == 257)
                            {
                                if (parameterArray[j] == 61 || parameterArray[j] == 62)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 254 || siteArray[i] == 258) && parameterArray[j] == 7)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 256 || siteArray[i] == 258) && parameterArray[j] == 38)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 257) && parameterArray[j] == 7)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else
                                    worksheet.Cells[5 + i, j + 1].PutValue("Null");
                            }
                            else
                            {
                                worksheet.Cells[5 + i, j + 1].PutValue("Null");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        DataRow[] rows = dtSiteData.Select(filter);
                        if (rows.Length > 0)
                        {
                            if (j != 3)
                            {

                                worksheet.Cells[5 + i, j + 1].PutValue(Math.Round(double.Parse(rows[0][3].ToString()) * 1000,1).ToString());
                            }
                            else
                            {
                                worksheet.Cells[5 + i, j + 1].PutValue(Math.Round(double.Parse(rows[0][3].ToString()), 3).ToString());

                            }
                        }
                        else
                        {
                            if (siteArray[i] == 254 || siteArray[i] == 256 || siteArray[i] == 258 || siteArray[i] == 257)
                            {
                                if (parameterArray[j] == 61 || parameterArray[j] == 62)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 254 || siteArray[i] == 258) && parameterArray[j] == 7)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 256 || siteArray[i] == 258) && parameterArray[j] == 38)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else if ((siteArray[i] == 257) && parameterArray[j] == 7)
                                    worksheet.Cells[5 + i, j + 1].PutValue("-");
                                else
                                    worksheet.Cells[5 + i, j + 1].PutValue("Null");
                            }
                            else
                            {
                                worksheet.Cells[5 + i, j + 1].PutValue("Null");
                            }
                        }
                        #endregion
                    }
                }

                if (siteArray[i] == 0)
                {
                    if (j <= 5)
                    {
                        filter = string.Format(" ParameterID = {0}", parameterArray[j]);
                        DataRow[] rowsAVG = dtSiteData.Select(filter);
                        Double avgValue = 0d;
                        try
                        {
                            foreach (DataRow row in rowsAVG)
                            {
                                avgValue += double.Parse(row["Value"].ToString());
                            }
                            if (avgValue == 0d) { worksheet.Cells[5 + i, j + 1].PutValue("Null"); }
                            else
                            {
                                if (j != 3)
                                      worksheet.Cells[5 + i, j + 1].PutValue(Math.Round((avgValue / rowsAVG.Length) * 1000, 1).ToString());
                                else
                                    worksheet.Cells[5 + i, j + 1].PutValue(Math.Round((avgValue / rowsAVG.Length), 3).ToString());
                            }
                        }
                        catch
                        {
                            worksheet.Cells[5 + i, j + 1].PutValue("Null"); 
                        }

                    }
                    else
                    {
                        filter = string.Format("ParameterID = {0}", parameterArray[j]);
                        DataRow[] rowsAVG = dtSiteDataII.Select(filter);
                        Double avgValue = 0d;
                        try
                        {
                            foreach (DataRow row in rowsAVG)
                            {
                                avgValue += double.Parse(row["Value"].ToString());
                            }
                            if (avgValue == 0d) { worksheet.Cells[5 + i, j + 1].PutValue("Null"); }
                            else
                            {
                                if (j == 15)
                                    worksheet.Cells[5 + i, j + 1].PutValue(Math.Round((avgValue / rowsAVG.Length), 0).ToString());
                                else
                                worksheet.Cells[5 + i, j + 1].PutValue(Math.Round((avgValue / rowsAVG.Length), 3).ToString());
                            }
                        }
                        catch
                        {
                            worksheet.Cells[5 + i, j + 1].PutValue("Null"); 
                        }

                    }
                }


                #region
                if (siteArray[i] == 888888)
                {
                    string sqlFilter = " AQIItemID=" + CparameterArray[j] + "";
                    DataRow[] rowss = dtChinaData.Select(sqlFilter);
                    if (rowss != null && rowss.Length > 0)
                    {
                        // 求平均
                        //Double avgValue = 0d;
                        //foreach (DataRow row in rowss)
                        //{
                        //    avgValue += double.Parse(row["Value"].ToString());
                        //}
                        double values = double.Parse(rowss[0]["Value"].ToString());
                        if (CparameterArray[j] != 308)
                        {
                            values = values * 1000;//mg转成ug/m3
                            values = Math.Round(values, 1);
                        }
                        else
                        {
                            values = Math.Round(values, 3);
                        }

                        worksheet.Cells[5 + i, j + 1].PutValue(values.ToString());
                    }
                    else
                    {
                        if (CparameterArray[j] == 309 ||
                            CparameterArray[j] == 307 ||
                            CparameterArray[j] == 310 ||
                            CparameterArray[j] == 308 ||
                            CparameterArray[j] == 301 ||
                            CparameterArray[j] == 302)
                        {
                            worksheet.Cells[5 + i, j + 1].PutValue("Null");
                        }
                        else
                        {
                            worksheet.Cells[5 + i, j + 1].PutValue("-");
                        }
                    }
                }
                #endregion 
                }
        }
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        string FileName = "交通日报表.xls";
        if (UserAgent.IndexOf("firefox") == -1)
        {//非火狐浏览器
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);
        }
        designer.Process();
        designer.Save(FileName, SaveType.OpenInExcel, FileFormatType.Excel97To2003, Response);
        
    }
}
