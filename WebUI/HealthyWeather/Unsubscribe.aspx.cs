using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Web.Services;
using System.Text;

public partial class HealthyWeather_Unsubscribe : System.Web.UI.Page
{
    public static Database m_Database;
    //public static String m_UserGroup, m_Name;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            m_Database = new Database("DBCONFIGII");
            startDate.Value = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd");
            endDate.Value = DateTime.Now.Date.ToString("yyyy-MM-dd");
            
        }
        

    }
    [WebMethod]
    public static DataTable getUserGroup()
    {
        List<DataTable> myDataUserGr = new List<DataTable>();
        String sqlUserGroup = "SELECT GROUPNAME FROM T_PubUser GROUP BY GROUPNAME";
        DataTable dtUserGroup = m_Database.GetDataTable(sqlUserGroup);
        /*if (dtUserGroup.Rows.Count> 0) {
            m_UserGroup = dtUserGroup.Rows[0][0].ToString();
        }*/
        return dtUserGroup;
    }
    [WebMethod]
    public static DataTable getName(String userGroup)
    {
        List<DataTable> myDataName = new List<DataTable>();
        String sqlName = "SELECT NAME FROM T_PubUser WHERE GROUPNAME='" + userGroup + "' GROUP BY NAME";
        return m_Database.GetDataTable(sqlName);

    }
    [WebMethod]
    public static DataTable getAllName()
    {
        List<DataTable> myDataName = new List<DataTable>();
        String sqlAllName = "SELECT NAME FROM T_PubUser GROUP BY NAME";
        return m_Database.GetDataTable(sqlAllName);

    }

    [WebMethod]
    public static DataTable getDiseaseType() {
        List<DataTable> myData = new List<DataTable>();
        string sql = "SELECT HEATHYTYPE FROM T_CancelRequest GROUP BY HEATHYTYPE";
        return m_Database.GetDataTable(sql);
    }

    [WebMethod]
    public static string Agree(string userID,string applyUser, string group, string applyTime, string diseaseType,string applyType)
    {
        string tip = "";
        string[] userStr = applyUser.Split(',');
        string[] groupStr = group.Split(',');
        string[] applyTimeStr = applyTime.Split(',');
        string[] diseaseTypeStr = diseaseType.Split(',');
        string[] applyTypeStr=applyType.Split(',');
        string[] idStr = userID.Split(',');
        for (int i = 0; i < idStr.Length; i++)
        {
            try
            {
                string sqlUpdate = "UPDATE T_CancelRequest SET processdate=convert(varchar(19),getdate(),120), PROCESSRESULT='已处理', Handled='1' " +
                              "WHERE USERID ='" + idStr[i] + "' and HEATHYTYPE='" + diseaseTypeStr[i] + "' and type='" + applyTypeStr[i] + "'";
                if (diseaseTypeStr[i] == "")
                {
                    sqlUpdate = "UPDATE T_CancelRequest SET processdate=convert(varchar(19),getdate(),120), PROCESSRESULT='已处理', Handled='1'  " +
                              "WHERE USERID ='" + idStr[i] + "' and type='" + applyTypeStr[i] + "'";
                }
                m_Database.Execute(sqlUpdate);
            }
            catch (Exception e) { tip += "error"; }
        }
        #region  用户管理权限处理
        string sqlPower = "";
        for (int i = 0; i < applyTypeStr.Length; i++)
        {
            try
            {
                if (applyTypeStr[i] == "邮件")
                {
                    sqlPower = "UPDATE T_PUBUSER SET CANEMAIL='0',EMAIL_PUBLVL='',EMAIL_PUBTIME='' WHERE USERID='" + idStr[i] + "'";
                }
                else if (applyTypeStr[i] == "短信")
                {
                    sqlPower = "UPDATE T_PUBUSER SET CANMESSAGE='0',Message_PubLvl='',Message_PubTime='' WHERE USERID='" + idStr[i] + "'";
                }
                //以后可以加FTP
                m_Database.Execute(sqlPower);
            }
            catch (Exception e1) { tip += "error"; }
        }
        return tip;
        #endregion
        //以前做的
        /*
        #region  权限处理
        String sqlUpdatePower = "";
        for (int i = 0; i < diseaseTypeStr.Length;i++ )
        {
            string v = "";
            switch (diseaseTypeStr[i].ToString())
            {
                case "儿童感冒": v = "2"; break;
                case "青年感冒": v = "3"; break;
                case "老年感冒": v = "4"; break;
                case "COPD": v = "5"; break;
                case "儿童哮喘": v = "6"; break;
                case "中暑": v = "7"; break;
                case "重污染": v = "8"; break;
            }
            String sqlWhere = "WHERE USERID = '" + idStr[i]+ "'";
            //如果申请的健康类型与 T_PUBUSER表中的健康类型相同，则在更新时使用
            String updateValue = "CANEMAIL='0',EMAIL_PUBLVL='',EMAIL_PUBTIME='',CANMESSAGE='0',Message_PubLvl='',Message_PubTime=''";
            String selHT = "SELECT HealthyType FROM T_PubUser " + sqlWhere + "";
            DataTable dt = m_Database.GetDataTable(selHT);
            foreach (DataRow dr in dt.Rows)
            {
                string str = dr["HealthyType"].ToString().Replace("_","");
                int num = str.IndexOf(v);
                if (num!=-1)
                {
                    if (num == str.Length-1)
                    {
                        if (str.Length == 1)
                        {
                            sqlUpdatePower = "UPDATE T_PUBUSER SET HEALTHYTYPE=REPLACE(HEALTHYTYPE,'" + v + "','')," + updateValue + " " + sqlWhere + "";
                        }
                        else {
                            v = "_" + v;
                            sqlUpdatePower = "UPDATE T_PUBUSER SET HEALTHYTYPE=REPLACE(HEALTHYTYPE,'" + v + "','') " + sqlWhere + "";
                        }
                    }
                    else
                    {
                        v = v + "_";
                        sqlUpdatePower = "UPDATE T_PUBUSER SET HEALTHYTYPE=REPLACE(HEALTHYTYPE,'" + v + "','')" + sqlWhere + "";
                    }
                    m_Database.Execute(sqlUpdatePower);
                }
            }
        }
        #endregion
         * */
    }


    [WebMethod]
    public static void Del(string ID)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            string sql_del = "DELETE FROM T_CANCELREQUEST WHERE ID='{0}'";
            string[] idStr = ID.Split(',');
            foreach (string id in idStr) {
                try
                {
                    m_Database.Execute(string.Format(sql_del, id));
                }
                catch { }
            }
        }
    }


}