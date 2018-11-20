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

using Readearth.Data;
public partial class AQI_Consultation : System.Web.UI.Page
{
    public string m_ForecastDate;
    public string m_FirstTab;
    public string m_UserJson;
    private Database m_Database;
    public bool m_UnLogin;
    public string Limits;
    public string LoginName = "";
    public string userName;
    protected void Page_Load(object sender, EventArgs e)
    {

        //如果当前用户没有登录的话，那么返回到登录界面
        Limits = Request.Cookies["User"]["indexUser"];
        userName = Request.Cookies["User"]["name"]; 
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_Database = new Database();

            m_ForecastDate = dtNow.ToString("yyyy年MM月dd日");
            int backDays = int.Parse(ConfigurationManager.AppSettings["BackDays"]);
            H00.Value = m_ForecastDate;
            H00.Attributes.Add("tag", backDays.ToString());
            int LimitingTime = int.Parse(ConfigurationManager.AppSettings["LimitingTime"]);
            DateTime dateTimeString = dtNow.Date.AddHours(LimitingTime);
            H00.Attributes.Add("todayDateTime", dateTimeString.ToString("yyyy-MM-dd HH:mm:ss"));
            CreateTable(dtNow, backDays);
            CreateUsers();

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

        DateTime dtRow = dtNow;
        HtmlTableRow newRow = null;
        HtmlTableCell td = null;
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
            if (i == 1 || i == 4||i==6||i==9) tableClass = "tablerowColor";

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
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 1);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 3)
            {
                td.Attributes.Add("class", "tablerow4");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 2);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 5)
            {
                td.Attributes.Add("class", "tablerow");
                td.Attributes.Add("rowspan", "3");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 3);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 8)
            {
                td.Attributes.Add("class", "tablerow4");
                td.Attributes.Add("rowspan", "2");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 4);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();
            }
            if (i == 10)
            {
                td.Attributes.Add("class", tableClass);
                td.Attributes.Add("rowspan", "1");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 5);
                newRow.Cells.Add(td);
                td = new HtmlTableCell();

            }
            if (i == 11)
            {
                td.Attributes.Add("class", tableClass);
                td.Attributes.Add("rowspan", "1");
                td.InnerHtml = string.Format("<span id ='Ptd{0}1'></span>", 6);
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
                     if(i==6||i==1)
                        tableClass = "rowColor";
                    else  if(i==2||i==7)
                     tableClass = "tablerowRightTopColor";
                    else if (i == 3 || i == 8)
                        tableClass = "tablerowRightColor";
                    else if (i == 4 || i == 9)
                        tableClass = "tablerowRightBottomColor";
                    else tableClass = "row";
                }
                td = new HtmlTableCell();
                td.Attributes.Add("class", tableClass);
                sb.Append(string.Format("<div id = 'H{0}1{1}{2}' class = 'show'></div>", t, k, j));
                td.InnerHtml = sb.ToString();
                sb.Length = 0;
                newRow.Cells.Add(td);
            }
            pTable.Rows.Insert(i + 1, newRow);

        }

    }

    /// <summary>
    /// 在前台创建用户列表json对象
    /// </summary>
    private void CreateUsers()
    {
        string strSQL = "SELECT FldValue,FldKey FROM V_DictionaryValues WHERE FldKey = 201 OR FldKey = 202 ORDER BY FldKey,FldValue ";

        DataTable tbUsers = m_Database.GetDataTable(strSQL);
        StringBuilder sb = new StringBuilder();
        string fldKey = "";

        foreach (DataRow row in tbUsers.Rows)
        {
            if (row[1].ToString() != fldKey)
                sb.AppendFormat("',{0}:'{1}", row[1], row[0]);
            else
                sb.AppendFormat("|{0}", row[0]);

            fldKey = row[1].ToString();
        }

        if (sb.Length > 0)
        {
            sb.Remove(0, 2);
            sb.Insert(0, "{");
            sb.Append("'}");
        }

        m_UserJson = sb.ToString();
    }

}

