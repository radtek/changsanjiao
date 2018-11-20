using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Readearth.Data;
using System.Text;
using System.Web.Services;

public partial class DBCityChartGW : System.Web.UI.Page
{
    public string m_FromDate;
    public string m_ToDate;
    public string m_FirstTab;
    public string m_Station;
    protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dtNow = DateTime.Now;
            m_FromDate = dtNow.AddMonths(0).ToString("yyyy年MM月dd日");
            m_ToDate = dtNow.AddDays(0).ToString("yyyy年MM月dd日");
            m_Station = Request["Station"];

        }
    }

    private void CreateTable(DateTime dtNow, int backDays)
    {
   
    }
    [WebMethod]
    public static string GetChartElements(string fromDate, string toDate, string eName, string type, string period, string duration)
    {
        string strSQL = "";
        string strReturn = "";
        string x = "";
        string y = "";

        string from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd 00:00:00");
        string to = DateTime.Parse(toDate).ToString("yyyy-MM-dd 23:59:59");


        #region
        string e1 = "";
        string maxValue = "DustMassCon<=500";
        if (eName == "PM25")
        {
            e1 = "2";
        }
        else if (eName == "PM10")
        {
            e1 = "3";
        }
        else if (eName == "PM1")
        {
            e1 = "4";
        }

        #endregion


        strSQL += " SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(16),[DateTIME], 120)) AS [END], " +
                      " DustMassCon as 'value',[DateTime],stationID as 'Site' from tbDustS where  " +
                     " (DateTime between '" + from + "' and '" + to + "' ) " +
                     " and stationID in ('58367','58362','58460','58366','58370','58363','99114','99116','99115','99119','99118','99110','99989') and DeviceType='" + e1 + "' and " + maxValue + " " +
                     "  ORDER BY datetime asc ;";//实况


        string[] SitesArray = { "58367", "58362", "58460", "58366", "58370",
                                    "58363", "99114", "99116", "99115", "99119", "99118", "99110", "99989" };

        //先全部查询出来
        Database m_Databases = new Database("DBCONFIGGW");
        DataSet dst = m_Databases.GetDataset(strSQL);

        string strReturns = "";
        try
        {
            for (int i = 0; i < SitesArray.Length; i++)
            {
                for (int index = 0; index < dst.Tables.Count; index++)
                {
                    DataTable dtElement = dst.Tables[index];
                    DataTable dtElementNew = dst.Tables[index].Clone();
                    DataRow[] rows;

                    rows = dtElement.Select("Site='" + SitesArray[i] + "'");

                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            dtElementNew.Rows.Add(row.ItemArray);
                        }
                    }
                    else
                    {
                        //补下数据
                        //DataRow row1 = dtElementNew.NewRow();
                        //row1[0] = "";
                        //row1[1] = "";
                        //row1[2] = from;
                        //row1[3] = SitesArray[i];
                        //DataRow row2 = dtElementNew.NewRow();
                        //row2[0] = "";
                        //row2[1] = "";
                        //row2[2] = to;
                        //row2[3] = SitesArray[i];
                        //dtElementNew.Rows.Add(row1);
                        //dtElementNew.Rows.Add(row2);
                        //continue;
                    }

                    x = ""; y = "";
                    //这里补数据
                    if (dtElementNew == null || dtElementNew.Rows.Count <= 0)
                        continue;

                    DateTime bt = DateTime.Parse(dtElementNew.Rows[0][2].ToString());
                    DateTime endt = DateTime.Parse(dtElementNew.Rows
                        [dtElementNew.Rows.Count - 1][2].ToString());
                    for (DateTime dt = bt; dt <= endt; dt = dt.AddMinutes(1))
                    {
                        bool bl = false;
                        foreach (DataRow dr in dtElementNew.Rows)
                        {
                            if (DateTime.Parse(dr[2].ToString()).
                                ToString("yyyy-MM-dd HH:mm:00")
                                == dt.ToString("yyyy-MM-dd HH:mm:00"))
                            {
                                x = x + "|" + dr[0].ToString();
                                y = y + "|" + dr[1].ToString();
                                bl = true;
                                break;
                            }
                        }
                        if (!bl)
                        {
                            x = x + "|" + ConvertDateTimeInt(dt);
                            y = y + "|" + "NULL";
                        }
                    }

                    strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
                }
                if (strReturn != ",")
                    strReturns += "{" + strReturn.TrimStart(',') + "}&";
            }
            if (strReturns != "&")
                strReturns = strReturns.TrimEnd('&');

            return strReturns;
        }
        catch (Exception ex)
        {
            m_Log.Error("GetAQIChart", ex);
            return ex.ToString();
        }
    }
    public static string ConvertDateTimeInt(System.DateTime time)
    {

        //double intResult = 0;
        //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        //intResult = (time - startTime).TotalSeconds;
        //return intResult.ToString();


        double intResult = 0;
        System.DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0);
        intResult = (time - startTime).TotalSeconds;
        return intResult.ToString();

    }

}
