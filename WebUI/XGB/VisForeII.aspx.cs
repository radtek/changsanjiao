using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Services;
using Readearth.Data;
using System.Data;
using System.Linq;

public partial class EastListII : System.Web.UI.Page
{
    public string m_json;
    public string id;
    public static Database m_Database;
    public static DataTable siteDT = new DataTable();
    public static DataTable shiKDT = new DataTable();
    public static DataTable foreDT = new DataTable();
    public static string[] siteidArr;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_Database = new Database("EleFore");
        if (!Page.IsPostBack)
        {
            m_json = Request["json"];
            StringBuilder sb = new StringBuilder();
            string[] types = m_json.Split('*');
            if (types.Length > 1)
            {
                m_json = types[0];
                sb.Append("<ul>");
                for (int i = 0; i < types.Length; i++)
                {
                    string[] info = types[i].Substring(1, types[i].Length - 2).Split(new char[] { ';', ':' });
                    if (i == 0)
                        sb.AppendFormat("<li onclick=\"TypeChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                    else
                        sb.AppendFormat("<li onclick=\"TypeChange('{2}','{1}')\"><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                }
                sb.Append("</ul>");
            }
            moduleTypes.InnerHtml = sb.ToString();
            id = Request["id"];
        }

    }

    [WebMethod]
    public static string GetTable(string siteids,string time)
    {
        string sql = "SELECT lst,siteid,sitename,vis_scale_predicted as vis FROM predicted_data_vis where forecastdate='" + time + "' and siteid in (" + siteids + ") order by lst asc;";
        foreDT = m_Database.GetDataTableMySQL(sql);
        Database db = new Database("CIMSS");
        string shiKSQL = "  SELECT vis,Station_Id_C as siteid,collect_time AS lst, Station_Name AS sitename FROM T_CIMISS_SURF_CHN_MUL_HOR WHERE collect_time between'{0}' and '{1}' AND Station_Id_C in ({2}) order by lst asc";
        DateTime maxTime, minTime;
        GetMaxMinTime(time, siteids, out maxTime, out minTime);
        shiKSQL = string.Format(shiKSQL, minTime, maxTime, siteids);
        DataTable tempDT = db.GetDataTable(shiKSQL);
        DataView dv = new DataView(tempDT);
        shiKDT = dv.ToTable(true);
        StringBuilder sb = new StringBuilder();
        //表头
        sb.Append("<table style='width:100%;'><thead><tr>");
        sb.Append("<td>站点</td><td>平均偏差</td><td>均方根偏差</td>");
        sb.Append("</tr></thead>");
        //表内容
        sb.Append("<tbody>");
        //GetTR(sb);
        foreach (string siteid in siteidArr)
        {
            string con = "siteid='" + siteid + "'";
            GetTr(sb, siteid, con, con, foreDT, shiKDT);
        }
        DataTable foreDTAvg = ForeDTSiteAvg(foreDT);
        DataTable shiKDTAvg = ForeDTSiteAvg(shiKDT);
        string allSiteCon = "siteid='0000'";
        GetTr(sb, "0000", allSiteCon, allSiteCon, foreDTAvg, shiKDTAvg);
        sb.Append("</tbody></table>");
        return sb.ToString();
    }
    public static DataTable ForeDTSiteAvg(DataTable dt)
    {
        //dt.Columns["val"].DataType=
        DataTable result = foreDT.Clone();
        DateTime minTime = foreDT.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Min();
        DateTime maxTime = shiKDT.AsEnumerable().Select(t => t.Field<DateTime>("lst")).Max();
        for (DateTime lst = minTime; lst <= maxTime; lst = lst.AddHours(1))
        {
            DataRow[] rows = dt.Select("lst='" + lst + "'");
            bool flag = false;
            foreach (DataRow r in rows)
            {
                if (r.ItemArray[0].ToString() == "")
                {
                    flag = true;
                }
            }
            if (flag)
            {
                continue;
            }
            float val = rows.AsEnumerable().Select(t => t.Field<float>("val")).Average();
            DataRow dr = result.NewRow();
            dr["lst"] = lst;
            dr["val"] = val;
            dr["siteid"] = "0000";  //表示求的所有站点的平均值
            result.Rows.Add(dr);
        }
        return result;
    }
    private static void GetTr(StringBuilder sb, string siteid, string foreCondition, string shiKCondition, DataTable foreDT, DataTable shiKDT)
    {
        sb.Append("<tr>");
        DataRow[] foreRows = foreDT.Select("siteid='" + siteid + "'");
        DataRow[] shiKRows = shiKDT.Select("siteid='" + siteid + "'");
        double RASE = 0, avgPiancha = 0;
        if (shiKRows.Length > 0)
        {
            double temp = 0, sum = 0, avg = 0, piancha = 0;
            foreach (DataRow foreRow in foreRows)
            {
                sum += float.Parse(foreRow["vis"].ToString());
            }
            avg = sum / foreRows.Length;
            for (int i = 0; i < shiKRows.Length; i++)  //均方根偏差要有实况才能算
            {
                #region 计算均方根偏差
                string foreV = foreRows[i]["vis"].ToString();
                string shiKV = shiKRows[i]["vis"].ToString();
                if (foreV == "" || shiKV == "")
                {
                    continue;
                }
                shiKV = GetVisGrade(float.Parse(shiKV));
                temp += Math.Pow(float.Parse(foreV) - float.Parse(shiKV), 2);
                #endregion
                #region  计算平均偏差
                piancha += Math.Abs(float.Parse(foreV) - avg);
                #endregion
            }
            RASE = Math.Pow(temp / foreRows.Length, 0.5);
            avgPiancha = piancha / foreRows.Length;
            sb.Append("<td>" + GetSite(siteid) + "</td>");
            sb.Append("<td>" + avgPiancha.ToString("f2") + "</td>");
            sb.Append("<td>" + RASE.ToString("f2") + "</td>");
            //sb.Append("<td>" + RASE.ToString("f2") + "</td>");
        }
        sb.Append("</tr>");
    }
    private static void GetMaxMinTime(string time, string siteids, out DateTime maxTime, out DateTime minTime)
    {
        string sqlTime = "SELECT max(lst) as maxLST,min(lst) as minLST from predicted_data_vis where forecastdate='" + time + "' and siteid in (" + siteids + ");";
        DataTable dtLST = m_Database.GetDataTableMySQL(sqlTime);
        maxTime = DateTime.Parse(dtLST.Rows[0][0].ToString());
        minTime = DateTime.Parse(dtLST.Rows[0][1].ToString());
    }
    
    
    public static string GetSite(string siteid)
    {
        string txt = "";
        DataRow[] dr = siteDT.Select("siteid='" + siteid + "'");
        txt = dr[0]["sitename"].ToString();
        return txt;
    }
    public static string GetVisGrade(float val)
    {
        string txt = "";
        if (val < 500)
        {
            txt = "4";
        }
        else if (val < 1000)
        {
            txt = "3";
        }
        else if (val < 3000)
        {
            txt = "2";
        }
        else if (val < 5000)
        {
            txt = "1";
        }
        else
        {
            txt = "0";
        }
        return txt;
    }
}