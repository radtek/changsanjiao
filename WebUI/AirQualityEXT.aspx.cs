using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Readearth.Data;
using System.Data;
using Readearth.Data.Entity;
using System.Web.Services;

public partial class AirQualityEXT : System.Web.UI.Page
{
    public static string m_json;
    public static string id;
    public static Database m_Database;
    public static Entity entity;
    public static string parentTxt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            m_Database = new Database();
            m_json = Request["json"];
            id = Request["id"];
            parentTxt = Request["parentText"];
            ////m_json = "{N:;C:2;R:2;S:1,2;P:}";
            ////id = "200hPaTemperature";
            entity = new Entity(m_Database, id);
            if (id.Split(',').Length > 1)
            {
                m_json = Request["json1"];
            }
            StringBuilder sb = new StringBuilder();
            string[] types = m_json.Split('*');
            if (types.Length > 0)
            {
                m_json = types[0];
                sb.Append("<ul>");
                for (int i = 0; i < types.Length; i++)
                {
                    string[] info = types[i].Substring(1, types[i].Length - 2).Split(new char[] { ';', ':' });
                    if (i == 0)
                    {
                        //if (china.IndexOf(id) >= 0)
                        //    sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                        //else
                            sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='foucs' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                    }
                    else
                    {
                        //if (east.IndexOf(id) >= 0)
                        //    sb.AppendFormat("<li ><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString());
                        //else 
                            sb.AppendFormat("<li onclick=\"CityChange('{2}','{1}')\"><p class='line' id='{1}'>{0}</p></li>", info[1], "L" + i.ToString(), types[i]);
                    }
                }
                sb.Append("</ul>");
            }

            
            moduleTypes.InnerHtml = sb.ToString();
    }

    [WebMethod]
    public static string GetDate(string type) {
        if (id== "AGCM"||id== "CSM") {
            type = "Day";
        }else if (id == "MATEWinter") {
            type = "Year";
        }
        string strSQL = "SELECT max(ForecastDate) AS 'ForecastDate' FROM " + entity.TableName + " WHERE " + entity.Condition + "";
        if (parentTxt == "东亚重要环流型预测") {   //这个模块有日月切换
            strSQL = "SELECT max(ForecastDate) AS 'ForecastDate' FROM "+entity.TableName+" WHERE " + entity.Condition + " and folder like '%" + type + "%'";
        }
        try
        {
            string time = "";
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows[0][0].ToString() == "")
            {
                strSQL = "SELECT max(ForecastDate) AS 'ForecastDate' FROM " + entity.TableName + "";
                dt = m_Database.GetDataTable(strSQL);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                string strTime = dt.Rows[0][0].ToString();
                if (strTime != "")   //如果数据库里面有数据，则显示数据库里面最大的日期，否则显示当前时间
                {
                    time = DateTime.Parse(strTime).ToString("yyyy-MM-dd");
                    if (m_json.IndexOf("T:0") > 0) {   //隐藏掉查询条件的页面
                        time = (type == "Day") ? DateTime.Parse(strTime).ToString("yyyy-MM-dd") : (type == "Month" ? DateTime.Parse(strTime).ToString("yyyy-MM") : DateTime.Parse(strTime).ToString("yyyy"));
                    }
                }
                else {
                    time = (type == "Day") ? DateTime.Now.ToString("yyyy-MM-dd") : (type == "Month" ? DateTime.Now.ToString("yyyy-MM") : DateTime.Now.ToString("yyyy"));
                }

            }
            return time;
        }
        catch {
            return "";
        }
        
    }
    [WebMethod]
    public static DataSet GetSelectVal()
    {
        DataSet ds = new DataSet();
        string code = "code", mc = "MC";
        //if (id == "200hPaWind" || id == "500hPaWind" || id == "700hPaWind" || id == "850hPaWind" || id == "10mWind") {
        //    code = "DM";
        //    mc = "MC";
        //}
        string sqlStation = "select code," + mc + " as MC from [dbo].[D_S2S_Station] where MC!='多中心对比' order by DM asc";
        //if (parentTxt == "多中心对比图") {
        //    sqlStation = "select code," + mc + " as MC from [dbo].[D_S2S_Station] where MC='多中心对比' order by DM asc";
        //}
       
        string sqlArea = "select " + code + " as code," + mc + " as MC from [dbo].[D_S2S_Area]  order by DM asc";
        string sqlPeriod = "select " + code + " as code," + mc + " as MC from [dbo].[D_S2S_Period]  order by DM asc";
        try
        {
            DataTable dtStatioin = m_Database.GetDataTable(sqlStation);
            DataTable dtArea = m_Database.GetDataTable(sqlArea);
            DataTable dtPeriod = m_Database.GetDataTable(sqlPeriod);
            ds.Tables.Add(dtStatioin);
            ds.Tables.Add(dtArea);
            ds.Tables.Add(dtPeriod);
        }
        catch { }
        return ds;
    }
    [WebMethod]
    public static string GetExplain(string parentTxt) {
        string sqlTitle = "select entityName,title,explain from [dbo].[D_Extension_Title] where entityName='"+id+"'";
        string txt = "";
        try
        {
            DataTable dtTitle = m_Database.GetDataTable(sqlTitle);
            if (dtTitle != null && dtTitle.Rows.Count > 0)
            {
                txt += dtTitle.Rows[0]["explain"].ToString()+"#" + dtTitle.Rows[0]["title"].ToString();
            }

        }
        catch { }
        return txt;
    }

    [WebMethod]
    public static string ChangeCondition(string id,string parentTxt,string time,string period) {
        Entity en = new Entity(m_Database, id);
        string queryStation = "select code from D_S2S_Station order by ShowId asc";
        string queryArea = "select code from D_S2S_Area order by ShowId asc";
        DataTable dtStation = m_Database.GetDataTable(queryStation);
        DataTable dtArea = m_Database.GetDataTable(queryArea);
        foreach (DataRow drStation in dtStation.Rows) {
            string stationCode = drStation["code"].ToString();
            foreach (DataRow drArea in dtArea.Rows) {
                string query = "select * from " + en.TableName + "";
                string areaCode = drArea["code"].ToString();
                string where = " where " + en.Condition + " and forecastdate between'" + time + " 00:00:00' and '" + time + " 23:59:59' and area='" + areaCode + "'";
                
                if (period != "0")
                {
                    where = where + " and parPeriod='" + period + "'";
                }
                if (parentTxt != "多中心对比图") {
                    where = where + " and station ='"+stationCode+"'";
                }
                query = query + where;
                DataTable dt = m_Database.GetDataTable(query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return stationCode+"#"+areaCode;
                }
            }
        }
        return "ecmf#EASTASIA";
    }
}