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
using System.IO;
using Readearth.Data;

public partial class AQI_chinaSC : System.Web.UI.Page
{
    public bool m_UnLogin;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["SYSTEMUSERID"] == null)
        //{
        //    m_UnLogin = true;
        //    return;
        //}
        DateTime dtNow = DateTime.Now.AddHours(-1);

        H00.Value = dtNow.ToString("yyyy年MM月dd日 HH时");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        DateTime dtFrom = DateTime.Parse(Content);
        string strSQL = string.Format("SELECT  T_Dust_Station.[address],pm10,tsp,vi,meteor_p,meteor_t,meteor_h,meteor_d,meteor_w FROM T_Dusty INNER JOIN T_Dust_Station on T_Dusty.mn=T_Dust_Station.mn WHERE cjTime='{0}'", dtFrom);
        Database m_Database = new Database();

        DataTable dtSiteData = m_Database.GetDataTable(strSQL);

        string name = "全国沙尘监测网";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("地点名称,PM10(微克/立方米),tsp(微克/立方米),能见度（公里）,气压（hPa）,气温(°C),湿度(%),风向(°),风速(米/秒)");
        foreach (DataRow dr in dtSiteData.Rows)
        {
            string dataLine = "";
            string value = "";
            for (int i = 0; i < dtSiteData.Columns.Count; i++)
            {
                if (i != 0)
                {
                    if (dr[i].ToString() == "0")
                        value = "/";
                    else if (i == 1 || i == 2)
                    {
                        double tempValue = Math.Round(double.Parse(dr[i].ToString()) * 1000, 1);//转化成微克/立方米（PM10,、tsp）
                        if (tempValue > 1000)
                            value = "/";
                        else
                            value = tempValue.ToString();
                    }
                    else
                        value = Math.Round(double.Parse(dr[i].ToString()), 1).ToString();
                }
                else
                    value = dr[i].ToString();
                dataLine = dataLine + value + ",";
            }
            sb.AppendLine(dataLine.Substring(0, dataLine.Length - 1));
        }
        StringWriter SW = new StringWriter(sb);
        SW.Close();
        string saveAsFileName = string.Format("{0}.csv", name);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(saveAsFileName));
        Response.ContentType = "application/ms-excel";
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Write(SW);
        Response.Flush();
        Response.End();
    }
}
