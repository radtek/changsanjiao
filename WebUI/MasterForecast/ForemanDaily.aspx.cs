using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Readearth.Data;

public partial class HealthyWeather_ForemanDaily : System.Web.UI.Page
{
    public string userName;
    public static Database m_database;
    protected void Page_Load(object sender, EventArgs e)
    {
        //m_database = new Database("DBCONFIG");
        userName = Request.Cookies["User"]["name"];
        
    }
    //[WebMethod]
    //public static string Save(string forecaster,string aqi,string poll,string foreTime,string lst) {
    //    string sql = "INSERT INTO T_ForemanDaily (forecaster,AQI,pollution,foreTime,LST) VALUES ('"+forecaster+"','"+aqi+"','"+poll+"','"+foreTime+"','"+lst+"')";
    //    string sql_fore = "SELECT * FROM T_ForemanDaily WHERE LST='" + lst + "'";
    //    string update_fore = "UPDATE T_ForemanDaily SET forecaster='" + forecaster + "',AQI='" + aqi + "',pollution='" + poll + "', foreTime='"+foreTime+"' WHERE lst='"+lst+"'";
    //    //把状态插入到T_StateFO
    //    string insert = "INSERT INTO T_State (ModuleType,ReTime,DeadLine,State,Type) VALUES ('foremanDaily',GETDATE(),'','3','1')";
    //    string query = "select * from T_State WHERE ReTime='" + lst + "'and ModuleType='foremanDaily' and State='3'";
    //    string update = "update T_State set DeadLine=GETDATE() where convert(varchar(100),ReTime,23)='2017-07-06' and ModuleType='foremanDaily' and State='3'";
    //    try
    //    {
    //        //foremanDaily表的操作
    //        DataTable dTable = m_database.GetDataTable(sql_fore);
    //        if (dTable != null && dTable.Rows.Count > 0)
    //        {
    //            m_database.Execute(update_fore);
    //        }
    //        else
    //        {
    //            m_database.Execute(sql);
    //        }//先判断是否已经保存过数据   T_StateFO表的操作
    //        DataTable dt = m_database.GetDataTable(query);
    //        if (dt.Rows.Count > 0 && dt != null)
    //        {
    //            //保存过数据，状态表里面有状态，需要修改
    //            m_database.Execute(update);
    //        }
    //        else {
    //            //未保存过，需要把状态插入到表中
    //            m_database.Execute(insert);
    //        }
    //        return "ok";
    //    }catch(Exception){
    //        return "";
    //    }
    //}

    //[WebMethod]
    //public static DataTable Query(string pubTime) {
    //    string sql_query = "SELECT forecaster,AQI,pollution,foreTime FROM T_ForemanDaily WHERE LST='"+pubTime+"' ORDER BY LST asc";
    //    m_database = new Database("DBCONFIG");
    //    DataTable dt = m_database.GetDataTable(sql_query);
    //    return dt;
    //}

    //[WebMethod]
    //public static string GetChart(string pubTime) {
    //    DateTime dtime=new DateTime(1970,1,1);
    //    string startTime = (DateTime.Parse(pubTime).AddDays(-14)).ToString();
    //    TimeSpan ts = DateTime.Parse(pubTime).Subtract(DateTime.Parse(startTime));
    //    int day = ts.Days;
    //    string sql = "SELECT DATEDIFF(S,'1970-01-01 00:00:00',CONVERT(CHAR(10),LST, 120)) AS [END],AQI,Pollution,forecaster from T_ForemanDaily where LST between '" + startTime + "' and '" + pubTime + "'";
    //    DataTable dTable = m_database.GetDataTable(sql);
    //    if (dTable.Rows.Count <= day)
    //    {    //缺少数据，需要补
    //        for (int i = 0; i <= day; i++)
    //        {
    //            DateTime dTime = DateTime.Parse(startTime).AddDays(i);
    //            string ss =((dTime.Subtract(dtime).TotalMilliseconds)/1000).ToString();
    //            //if (ss == "1499126400")
    //            //{
    //                string condition = "END='"+ss+"'";
                    
    //                    DataRow[] dr = dTable.Select(condition);
    //                    //flag = true;
                   
                    
    //                if(dr.Length<=0)
    //                {
    //                    DataRow newrow = dTable.NewRow();
    //                    newrow["END"] = ss;
    //                    newrow["aqi"] = "null";
    //                    newrow["pollution"] = "--";
    //                    newrow["forecaster"] = "--";
    //                    dTable.Rows.Add(newrow);
    //                }
    //           // }
    //        }
    //    }
    //    DataView dv = new DataView(dTable);
    //    dv.Sort="END asc";     //排序  要不然图标X轴时间不按顺序
    //    dTable = dv.ToTable();
    //    StringBuilder json = new StringBuilder();
    //    json.Append("[");
    //    string time="";
    //    for (int i = 0; i < dTable.Rows.Count; i++)
    //    {
    //        time += dTable.Rows[i][0].ToString() + "|";
    //        json.Append("{name:\"预报员：" + dTable.Rows[i][3] + "<br/>首要污染物：" + dTable.Rows[i][2] + "\",y:" + dTable.Rows[i][1] + ",color:\'" + GetColor(dTable.Rows[i][1].ToString()) + "\'},");
    //    }
    //    json.Append("]");
    //    json.Append("]");
    //    time = time.TrimEnd('*');
    //    string Json = json.ToString();
    //    Json = Json.Remove(Json.Length - 2, 1);
    //    string ret = Json + "*" + time;
    //    return ret;
    //}
    //public static string GetColor(string value) {
    //    if (value == "null") return "null";
    //    float val = float.Parse(value);
    //    string color = "";
    //    if (val <= 50) color = "#00ff00";
    //    else if (val <= 100) color = "#ffff00";
    //    else if (val <= 150) color = "#ff9900";
    //    else if (val <= 200) color = "#ff0000";
    //    else if (val <= 300) color = "#9900ff";
    //    else if (val > 300) color = "#980000";
    //    return color;
    //}
}
