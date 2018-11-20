using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Readearth.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class ReportProduce_AQIPeriodForecast : System.Web.UI.Page
{
    public string m_ForecastDate;
    public string m_FirstTab;
    public string m_UserJson;
    public string m_PeopleJson;
    public string m_dianxinUser;
    private Database m_Database;
    public bool m_UnLogin;
    public string Limits;
    public string LoginName = "";
    public string userName;

    protected void Page_Load(object sender, EventArgs e)
    {        
        userName = Request.Cookies["User"]["name"];
        //strSQL = "SELECT UserName,Alias FROM T_User WHERE UserName='" + userName + "'";
        //DataTable dt = m_Database.GetDataTable(strSQL);
        //if (dt.Rows.Count > 0)
        //    LoginName = dt.Rows[0][1].ToString();
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_ForecastDate = dtNow.ToString("yyyy年MM月dd日");
            int backDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
           
            CreateTable(dtNow, backDays);                     
        }

    }

    private void CteateTableTitle()
    {
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
        for (int i = 0; i < 2; i++)
        {
            newRow = new HtmlTableRow();
            for (int j = 0; j < 23; j++)
            {                
                switch (j)
                {
                    case 0:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "2");
                            td.Attributes.Add("class", "tabletitleOutPeiod");
                            td.InnerHtml = string.Format("预报时效");
                            newRow.Cells.Add(td);
                        }                        
                        break;
                    case 1:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "2");
                            td.Attributes.Add("class", "tabletitleDateOut");
                            td.InnerHtml = string.Format("日期");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 2:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "2");
                            td.Attributes.Add("class", "tabletitleOutP");
                            td.InnerHtml = string.Format("时段");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 3:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM2.5");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 4:
                       if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 5:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 6:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 7:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 8:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 9:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 10:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 11:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 12:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 13:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 14:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 15:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 16:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 17:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 18:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 19:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 20:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 21:
                        if (i == 0)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "2");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("PM10");
                            newRow.Cells.Add(td);
                        }
                        else if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("监测中心");
                            newRow.Cells.Add(td);
                        }
                        break;
                    case 22:
                        if (i == 1)
                        {
                            td = new HtmlTableCell();
                            td.Attributes.Add("rowspan", "1");
                            td.Attributes.Add("colspan", "1");
                            td.Attributes.Add("class", "tabletitleOut");
                            td.InnerHtml = string.Format("气象局");
                            newRow.Cells.Add(td);
                        }
                        break;
                    default:
                        break;
                }

            }
            contentTable.Rows.Insert(i, newRow);
        }
        

    }

    /// <summary>
    /// 根据当前的日期和返回的数量
    /// </summary>
    /// <param name="dtNow">当前时间</param>
    /// <param name="backCount">可参考的历史数据</param>
    /// <returns></returns>
    private void CreateTable(DateTime dtNow, int backDays)
    {
        StringBuilder sb = new StringBuilder();
        HtmlTable pTable = new HtmlTable();
        DateTime dtRow = dtNow;
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
        for (int i = 0; i <= 23; i++)
        {

            newRow = new HtmlTableRow();
            td = new HtmlTableCell();
            string tableClass = string.Empty;
            string shiDuan = string.Empty;
            int m = 0;//属于参考的0，正式填写的1
            int t = 0;//属于那一天 3,4,5
            int k = 0;//时段， 上半夜（20-0）4，下半夜（0-6）1，上午（6-12）2，下午（12-28）3，夜间（20-6）6，日平均 7
            //if (i == 8 || i == 18) tableClass = "tablerow4Out";
            //else
            //    tableClass = "tablerowOut";
            tableClass = "tablerowOut";
            if (i == 3 || i == 9 || i == 13 || i == 19)
                tableClass = "tablerowColorOut";

            switch (i)
            {
                case 0:
                    shiDuan = "上半夜";
                    tableClass = "tablerowOutspan";
                    t = backDays + 1;
                    k = 4;
                    m = 0;
                    break;
                case 1:
                    t = backDays + 1;
                    k = 4;
                    m = 1;
                    break;
                case 2:
                    shiDuan = "下半夜";
                    tableClass = "tablerowColorOutP";
                    t = backDays + 2;
                    k = 1;
                    m = 0;
                    break;
                case 3:
                    t = backDays + 2;
                    k = 1;
                    m = 1;
                    break;
                case 4:
                    shiDuan = "夜间";
                    tableClass = "tablerowLeftTopColorOut";
                    t = backDays + 1;
                    k = 6;
                    m = 0;
                    break;
                case 5:
                    t = backDays + 1;
                    k = 6;
                    m = 1;
                    break;
                case 6:
                    shiDuan = "上午";
                    tableClass = "tablerowLeftColorOut";
                    t = backDays + 2;
                    k = 2;
                    m = 0;
                    break;
                case 7:
                    t = backDays + 2;
                    k = 2;
                    m = 1;
                    break;
                case 8:
                    shiDuan = "下午";
                    tableClass = "tablerowLeftBottomColorOut";
                    t = backDays + 2;
                    k = 3;
                    m = 0;
                    break;
                case 9:
                    t = backDays + 2;
                    k = 3;
                    m = 1;
                    break;
                case 10:
                    shiDuan = "上半夜";
                    tableClass = "tablerowOutspan";
                    t = backDays + 2;
                    k = 4;
                    m = 0;
                    break;
                case 11:
                    t = backDays + 2;
                    k = 4;
                    m = 1;
                    break;
                case 12:
                    shiDuan = "下半夜";
                    tableClass = "tablerowColorOutP";
                    t = backDays + 3;
                    k = 1;
                    m = 0;
                    break;
                case 13:
                    t = backDays + 3;
                    k = 1;
                    m = 1;
                    break;
                case 14:
                    shiDuan = "夜间";
                    tableClass = "tablerowLeftTopColorOut";
                    t = backDays + 2;
                    k = 6;
                    m = 0;
                    break;
                case 15:
                    t = backDays + 2;
                    k = 6;
                    m = 1;
                    break;
                case 16:
                    shiDuan = "上午";
                    tableClass = "tablerowLeftColorOut";
                    t = backDays + 3;
                    k = 2;
                    m = 0;
                    break;
                case 17:
                    t = backDays + 3;
                    k = 2;
                    m = 1;
                    break;
                case 18:
                    shiDuan = "下午";
                    tableClass = "tablerowLeftBottomColorOut";
                    t = backDays + 3;
                    k = 3;
                    m = 0;
                    break;
                case 19:
                    t = backDays + 3;
                    k = 3;
                    m = 1;
                    break;
                case 20:
                    shiDuan = "日平均";
                    tableClass = "tablerowOutspan";
                    t = backDays + 2;
                    k = 7;
                    m = 0;
                    break;
                case 21:
                    t = backDays + 2;
                    tableClass = "tablerowOutspan";
                    k = 7;
                    m = 1;
                    break;
                case 22:
                    shiDuan = "日平均";
                    tableClass = "tablerowOutspan";
                    t = backDays + 3;
                    k = 7;
                    m = 0;
                    break;
                case 23:
                    t = backDays + 3;
                    k = 7;
                    tableClass = "tablerowOutspan";
                    m = 1;
                    break;
            }
            if (i == 0)
            {
                td.Attributes.Add("rowspan", "10");
                td.Attributes.Add("class", "tablerow4Out");
                td.InnerHtml = string.Format("24小时");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            else if (i == 10)
            {
                td.Attributes.Add("rowspan", "10");
                td.Attributes.Add("class", "tablerow4Out");
                td.InnerHtml = string.Format("48小时");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            else if (i == 20)
            {
                td.Attributes.Add("rowspan", "4");
                td.Attributes.Add("class", "tablerow4Out");
                td.InnerHtml = string.Format("日平均");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }

            if (i == 0)
            {
                td.Attributes.Add("class", "tablerowOutP");
                td.Attributes.Add("rowspan", "6");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 1);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 6)
            {
                td.Attributes.Add("class", "tablerow4OutP");
                td.Attributes.Add("rowspan", "4");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 2);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 10)
            {
                td.Attributes.Add("class", "tablerowOutP");
                td.Attributes.Add("rowspan", "6");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 3);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 16)
            {
                td.Attributes.Add("class", "tablerow4OutP");
                td.Attributes.Add("rowspan", "4");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 4);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 20)
            {
                td.Attributes.Add("class", "tablerowOutP");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 5);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();

            }
            if (i == 22)
            {
                td.Attributes.Add("class", "tablerowOutP");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 6);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }


            td = new HtmlTableCell();
            if (i % 2 == 0)
            {
                td.Attributes.Add("class", tableClass);
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = shiDuan;
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            //if (i == 8 || i == 18) tableClass = "tablerow4Out";
            //else
            //    tableClass = "tablerowOut";
            tableClass = "tablerowOut";

            if (i == 3 || i == 9 || i == 13 || i == 19) tableClass = "tablerowColorOut";
            if (i % 2 == 1)
            {
                for (int j = 1; j <= 6; j++)
                {
                    if (j == 6)
                    {
                        if (i == 13 || i == 3)
                            tableClass = "rowColorOut";
                        else if (i == 4 || i == 14)
                            tableClass = "tablerowRightTopColorOut";
                        else if (i == 6 || i == 16 || i == 5 || i == 7 || i == 8 || i == 16 || i == 15 || i == 17 || i == 18)
                            tableClass = "tablerowRightColorOut";
                        else if (i == 9 || i == 19)
                            tableClass = "tablerowRightBottomColorOut";
                        else tableClass = "rowOut";
                    }
                    string onClick = string.Empty;
                    td = new HtmlTableCell();
                    //if (i % 2 == 1 && j != 6 && i != 23 && i != 21)
                    //{
                    //    onClick = "onclick = 'showInput(event,this)'";
                //}
                    //else
                    //    onClick = "";      
                    onClick = "onclick = 'showInput(event,this)'";
                    td.Attributes.Add("class", tableClass);
                    string classDiv = "show";
                    if (onClick != "")
                    {
                        classDiv = "divInputType show";
                        //classDiv = "divInputType limitEdit show";
                    }
                    td.Attributes.Add("colspan", "2");
                    if (j == 6)
                        sb.Append(string.Format("<div id = 'H{0}{3}{1}{2}' class = '{4}' {5}>/</div>", t, k, j, "1", classDiv, onClick));//(j+1)%2环境监测中心0，气象局1
                    else
                    {
                        sb.Append(string.Format("<div id = 'H{0}{3}{1}{2}' class = '{4}' {5}>/</div>", t, k, j, "1", classDiv, onClick));//(j+1)%2环境监测中心0，气象局1
                    }
                    td.InnerHtml = sb.ToString();
                    sb.Length = 0;
                    newRow.Cells.Add(td);
                }
            }
            else
            {
                for (int j = 1; j <= 12; j++)
                {
                    if (j == 12)
                    {
                        if (i == 13 || i == 3)
                            tableClass = "rowColorOut";
                        else if (i == 4 || i == 14)
                            tableClass = "tablerowRightTopColorOut";
                        else if (i == 6 || i == 16 || i == 5 || i == 7 || i == 8 || i == 16 || i == 15 || i == 17 || i == 18)
                            tableClass = "tablerowRightColorOut";
                        else if (i == 9 || i == 19)
                            tableClass = "tablerowRightBottomColorOut";
                        else tableClass = "rowOut";
                    }
                    string onClick = string.Empty;
                    td = new HtmlTableCell();
                    //if (i % 2 == 1 && j != 6 && i != 23 && i != 21)
                    //{
                    //    onClick = "onclick = 'showInput(event,this)'";
                    //}
                    //else
                    //    onClick = "";      
                    onClick = "onclick = 'showInput(event,this)'";
                    td.Attributes.Add("class", tableClass);
                    string classDiv = "show";
                    if (onClick != "")
                    {
                        //classDiv = "divInputType show";
                        classDiv = "divInputType limitEdit show";
                    }
                    sb.Append(string.Format("<div id = 'P{0}{3}{1}{2}' class = '{4}' {5}>/</div>", t, k, (j + 1) / 2, (j + 1) % 2 + 1, classDiv, onClick));//(j+1)%2环境监测中心1，气象局0
                    td.InnerHtml = sb.ToString();
                    sb.Length = 0;
                    newRow.Cells.Add(td);
                }
            }
            contentTable.Rows.Insert(i + 2, newRow);           
        }
    }
    /// <summary>
    /// 创建汇总信息
    /// </summary>
    private void CreateCollectionTable(DateTime dtNow, int backDays)
    {
        DateTime dtRow = dtNow;
        HtmlTable pTable = new HtmlTable();
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= 11; i++)
        {
            newRow = new HtmlTableRow();
            td = new HtmlTableCell();
            string tableClass = string.Empty;
            string shiDuan = string.Empty;
            int t = 0;//属于那一天 3,4,5
            int k = 0;//时段， 上半夜（20-0）4，下半夜（0-6）1，上午（6-12）2，下午（12-28）3，夜间（20-6）6，日平均 7
            if (i == 4 || i == 9) tableClass = "tablerow4";
            else
                tableClass = "tablerow";
            if (i == 1 || i == 4 || i == 6 || i == 9) tableClass = "tablerowColor";

            switch (i)
            {
                case 0:
                    shiDuan = "上半夜";
                    t = backDays + 1;
                    k = 4;
                    break;
                case 1:
                    shiDuan = "下半夜";
                    t = backDays + 2;
                    k = 1;
                    break;
                case 2:
                    shiDuan = "夜间";
                    tableClass = "tablerowLeftTopColor";
                    t = backDays + 1;
                    k = 6;
                    break;
                case 3:
                    shiDuan = "上午";
                    tableClass = "tablerowLeftColor";
                    t = backDays + 2;
                    k = 2;
                    break;
                case 4:
                    shiDuan = "下午";
                    tableClass = "tablerowLeftBottomColor";
                    t = backDays + 2;
                    k = 3;
                    break;
                case 5:
                    shiDuan = "上半夜";
                    t = backDays + 2;
                    k = 4;
                    break;
                case 6:
                    shiDuan = "下半夜";
                    t = backDays + 3;
                    k = 1;
                    break;
                case 7:
                    shiDuan = "夜间";
                    tableClass = "tablerowLeftTopColor";
                    t = backDays + 2;
                    k = 6;
                    break;
                case 8:
                    shiDuan = "上午";
                    tableClass = "tablerowLeftColor";
                    t = backDays + 3;
                    k = 2;
                    break;
                case 9:
                    shiDuan = "下午";
                    tableClass = "tablerowLeftBottomColor";
                    t = backDays + 3;
                    k = 3;
                    break;
                case 10:
                    shiDuan = "日平均";
                    t = backDays + 2;
                    k = 7;
                    break;
                case 11:
                    shiDuan = "日平均";
                    t = backDays + 3;
                    k = 7;
                    break;
            }
            if (i == 0)
            {
                td.Attributes.Add("rowspan", "5");
                td.Attributes.Add("class", "tablerow4");
                td.InnerHtml = string.Format("24小时");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();

            }
            else if (i == 5)
            {
                td.Attributes.Add("rowspan", "5");
                td.Attributes.Add("class", "tablerow4");
                td.InnerHtml = string.Format("48小时");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            else if (i == 10)
            {
                td.Attributes.Add("rowspan", "2");
                td.Attributes.Add("class", "tablerow4");
                td.InnerHtml = string.Format("日平均");
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }

            if (i == 0)
            {
                td.Attributes.Add("class", "tablerow");
                td.Attributes.Add("rowspan", "3");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 1);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 3)
            {
                td.Attributes.Add("class", "tablerow4");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 2);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 5)
            {
                td.Attributes.Add("class", "tablerow");
                td.Attributes.Add("rowspan", "3");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 3);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 8)
            {
                td.Attributes.Add("class", "tablerow4");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 4);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 10)
            {
                td.Attributes.Add("class", tableClass);
                td.Attributes.Add("rowspan", "1");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 5);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();

            }
            if (i == 11)
            {
                td.Attributes.Add("class", tableClass);
                td.Attributes.Add("rowspan", "1");
                td.InnerHtml = string.Format("<span id ='PMd{0}1'></span>", 6);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }


            td = new HtmlTableCell();
            td.Attributes.Add("class", tableClass);
            td.InnerHtml = shiDuan;
            newRow.Cells.Add(td);
            if (i == 4 || i == 9) tableClass = "tablerow4";
            else
                tableClass = "tablerow";
            if (i == 1 || i == 4 || i == 6 || i == 9) tableClass = "tablerowColor";

            for (int j = 1; j <= 6; j++)
            {
                if (j == 6)
                {
                    if (i == 6 || i == 1)
                        tableClass = "rowColor";
                    else if (i == 2 || i == 7)
                        tableClass = "tablerowRightTopColor";
                    else if (i == 3 || i == 8)
                        tableClass = "tablerowRightColor";
                    else if (i == 4 || i == 9)
                        tableClass = "tablerowRightBottomColor";
                    else tableClass = "row";
                }
                td = new HtmlTableCell();
                td.Attributes.Add("class", tableClass);
                sb.Append(string.Format("<div id = 'PH{0}1{1}{2}' class = 'show'></div>", t, k, j));
                td.InnerHtml = sb.ToString();
                sb.Length = 0;
                newRow.Cells.Add(td);
            }
            contentTable.Rows.Insert(i, newRow);
            
        }
    }

}