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
using MMShareBLL.DAL;
using Readearth.Data;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using MMShareBLL.Model;
using MMShareBLL.BLL;
using Aspose.Cells;

public partial class WorkGroup22 : System.Web.UI.Page
{
    public string m_ForecastDate;
    public string m_ForecastEndDate;
    public string m_FirstTab;
    public string m_UserJson;
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                userNames.Value = Request.Cookies["User"]["name"].ToString();
            }
            catch { }
        }
    }
    public string returnWeek(string dt)
    {
        string  week ="";
        switch (dt)
        {
            case "Monday":
                week = "一";
                break;
            case "Tuesday":
                week = "二";
                break;
            case "Wednesday":
                week = "三";
                break;
            case "Thursday":
                week = "四";
                break;
            case "Friday":
                week = "五";
                break;
            case "Saturday":
                week = "六";
                break;
            case "Sunday":
                week = "日";
                break;
        }
        return week;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Database m_Database = new Database();
        string dateTime = Element.Value;
        string[] groupArray = { "领班", "主班", "副班" };
        Workbook workbook = new Workbook(); //工作簿 
        Worksheet sheet1 = workbook.Worksheets[0]; //工作表
        sheet1.Name = "排班表";
        Cells cells = sheet1.Cells;//单元格
        string filter = "";
        DataRow[] tempRow;
        Aspose.Cells.Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        string strSQL = "Select DText, Users, workTime From  T_Scheduling where CONVERT(datetime, workTime) between '2016-05-01 00:00:00' and '2016-05-31 23:00:00' order by workTime ";
        DataTable dt = m_Database.GetDataTable(strSQL);
        if (dt.Rows.Count > 0)
        {
            DataTable timeTable = dt.DefaultView.ToTable(true, "workTime");
            string[] titleName = { "日期", "星期", "领班", "主班", "副班" };
            for (int k = 0; k < titleName.Length; k++)
            {
                cells[0, k].PutValue(titleName[k]);
            }
            for (int i = 0; i < timeTable.Rows.Count; i++)
            {
                DateTime time = DateTime.Parse(timeTable.Rows[i][0].ToString());
                cells[i+1, 0].PutValue(time.Day);
                cells[i + 1, 1].PutValue(returnWeek(time.DayOfWeek.ToString()));
                for (int j = 0; j < groupArray.Length; j++)
                {
                    filter = string.Format("DText='{0}' and workTime='{1}'", groupArray[j], timeTable.Rows[i][0].ToString());
                    tempRow = dt.Select(filter);
                    if(tempRow.Length>0)
                        cells[i + 1, 2 + j].PutValue(tempRow[0]["Users"].ToString());
                }

            }

        }
        string FileName = "排班表"+DateTime.Now.ToString("yyyyMMddhhmmss")+".xls";
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);

        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Response.Flush();
        Response.End();
    }
}
