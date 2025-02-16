﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Comforecast_AllCityForecast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        H00.Value = DateTime.Now.ToString("yyyy年MM月dd日");
        CreateTable();
    }
    private void CreateTable()
    {
        StringBuilder sb = new StringBuilder();
        string[] siteID = { "50745", "50873", "54453", "54337", "54324", "54237", "54497", "54347", "54094","50850","50853","54827" };
        //string[] siteID = {"50745","50873","54453","54337","54324","54237","54497","54347"};
        string[] siteName = { "齐齐哈尔", "佳木斯", "葫芦岛", "锦州", "朝阳", "阜新", "丹东", "辽阳", "牡丹江", "大庆", "绥化", "山东泰安" };
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
        int[] itemCode = { 1, 2, 6, 5, 7, 3 };
        string tableClass = string.Empty;
        for (int i = 0; i < siteID.Length; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                newRow = new HtmlTableRow();
                if (j == 1)
                {
                    td = new HtmlTableCell();
                    td.Attributes.Add("rowspan", "3");
                    td.Attributes.Add("class", "tableRowNameX");
                    td.InnerHtml = siteName[i];
                    newRow.Cells.Add(td);
                }
                td = new HtmlTableCell();
                
                if(j==3)
                    td.Attributes.Add("class", "tableRow1X");
                else
                    td.Attributes.Add("class", "tableRow1");

                td.InnerHtml = string.Format("<span id ='Ptd{1}{0}'></span>", siteID[i], j);
                newRow.Cells.Add(td);
                //首要污染物H{0}{1}{2} 0代表站点号，1代表时效，2代表污染物类型（其中0代表首要污染物）
                td = new HtmlTableCell();
               // td.Attributes.Add("class", "tableRow");

                if (j == 3)
                    td.Attributes.Add("class", "tableRowX");
                else
                    td.Attributes.Add("class", "tableRow");

                sb.Append(string.Format("<div id ='H{0}{1}AQI' class = 'float' ></div><span class='float'>/</span><div id ='PH{0}{1}AQI' class = 'float'></div>", siteID[i], j));
                td.InnerHtml = sb.ToString();
                sb.Length = 0;
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
               // td.Attributes.Add("class", "tableRow");
                if (j == 3)
                    td.Attributes.Add("class", "tableRowX");
                else
                    td.Attributes.Add("class", "tableRow");

                td.InnerHtml = string.Format("<span id ='H{0}{1}primeplu'></span>", siteID[i], j);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
                //td.Attributes.Add("class", "tableRow2");

                if (j == 3)
                    td.Attributes.Add("class", "tableRow2X");
                else
                    td.Attributes.Add("class", "tableRow2");

                sb.Append(string.Format("<div id ='H{0}{1}color' class = 'divColor' style=' width:15px;' ></div><div class='divtext' id='H{0}{1}text'></div><div id ='H{0}{1}kqzl' class = 'divAQI'>-</div>", siteID[i], j));
                td.InnerHtml = sb.ToString();
                sb.Length = 0;
                newRow.Cells.Add(td);
                string onClick = string.Empty;
                for (int k = 0; k < itemCode.Length; k++)
                {
                    td = new HtmlTableCell();
                    //td.Attributes.Add("class", "tableRow");

                    if (j == 3)
                        td.Attributes.Add("class", "tableRowX");
                    else
                        td.Attributes.Add("class", "tableRow");

                    sb.Append(string.Format("<div id ='H{0}{1}{2}' class = 'divInputType float' onclick = 'showInput(event,this)'></div><span class='float'>/</span><div id ='PH{0}{1}{2}' class = 'float'></div>", siteID[i], j, itemCode[k]));//(j+1)%2环境监测中心1，气象局0
                    td.InnerHtml = sb.ToString();
                    newRow.Cells.Add(td);
                    sb.Length = 0;
                }
                comforecastTable.Rows.Insert(comforecastTable.Rows.Count, newRow);
            }
        }


    }
}