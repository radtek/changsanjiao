using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Readearth.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using log4net;

namespace MMShareBLL.DAL
{
    public class UserManager
    {
        protected static readonly log4net.ILog m_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Database m_Database;
        //public string m_Authority;

        public UserManager()
        {
            m_Database = new Database();
        }
        public UserManager(Database db)
        {
            m_Database = db;
        }
        private string GetTime(string alias, int JB)
        {
            DateTime dtNow = DateTime.Now;
            //LoginTime loginTime = new LoginTime();
            //loginTime.Local = dtNow.ToString("yyyy年MM月dd日");
            //loginTime.Universal = dtNow.ToUniversalTime().ToString("yyyy年MM月dd日");

            //return loginTime;
            //return "{Local:'2011年03月05日',Universal:'2011年03月05日'}";
            return "Alias:'" + alias + "',Local:'" + dtNow.ToString("yyyy年MM月dd日") + "',JB:" + JB.ToString();
        }
        /// <summary>
        /// 通过用户名和密码，验证用户的合法性
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pssword">密码</param>
        /// <returns>成功登录返回用户别名</returns>
        public string Login(string userName, string pssword, string ip)
        {
            string strSQL = "SELECT ALIAS,BZ,T_Classes.Authority FROM T_USER LEFT JOIN T_Classes ON  T_USER.BZ=T_Classes.ID WHERE T_USER.USERNAME = @Username AND T_USER.SN = @Password";
            //SELECT ALIAS,BZ,T_Classes.Authority FROM T_USER LEFT JOIN T_Classes ON  T_USER.BZ=T_Classes.ID WHERE T_USER.USERNAME = 'admin' AND T_USER.SN = 'admin'
            string strAlias = string.Empty;
            string authority = "";
            string funAuthority;
            try
            {
                SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@Username", userName),
                    new SqlParameter("@Password", pssword)
				};
                SqlDataReader drUser = m_Database.GetDataReader(strSQL, para);
                if (drUser.HasRows)
                {
                    if (drUser.Read())
                    {
                        //HttpCookie newCookie = new HttpCookie("User");
                        //newCookie.Values.Add("Name", context.Request["userName"]);
                        //newCookie.Values.Add("Index", loginInfo[5]);
                        //Page.Response.Cookies.Add(newCookie);
                        strAlias = GetTime(drUser.GetString(0), drUser.GetInt32(1));
                        authority = drUser.GetString(2);
                        Authority m = (Authority)JsonConvert.DeserializeObject(authority, typeof(Authority));//((Newtonsoft.Json.Linq.JContainer)(m)).First
                        funAuthority = m.function;
                        //插入登录日志信息
                        User user = new User(userName, pssword, drUser.GetString(0));
                        Utility.InsertLog(user, ip, "登录系统");

                        strAlias = "{" + strAlias + ",LoginCount:'" + GetLoginCount(userName) + "',UserName:'" + userName + "',LoginIP:'" + ip + "',UserAuthority:'" + funAuthority + "'}";
                    }
                }
                drUser.Close();
            }
            catch (Exception ex)
            {
                m_Log.Error(ex.Source + ":" + ex.Message);
                throw ex;
            }

            return strAlias;
        }
        public string Exit(string userName, string ip)
        {
            string strSQL = string.Format("SELECT SN FROM T_USER WHERE USERNAME ='{0}'", userName);
            SqlDataReader dr = m_Database.GetDataReader(strSQL);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    string pssword = dr.GetString(0);
                    User user = new User(userName, pssword, ip);
                    Utility.InsertLog(user, ip, "注销系统");

                }
            }
            dr.Close();
            return "1:'注销成功'";


        }
        /// <summary>
        /// 获取用户的登录次数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string GetLoginCount(string userName)
        {
            string strSQL = "SELECT COUNT(*) FROM T_LOG WHERE USERNAME = '" + userName + "' AND OPERATORCONTEXT = '登录系统'";
            return m_Database.GetFirstValue(strSQL);

        }

        /// <summary>
        /// 根据客户端登陆的IP地址，返回用户名和密码，用户名和密码
        /// </summary>
        /// <returns></returns>
        public string LoginByIP(string clientIP)
        {
            string userInfo = "";
            string strSQL = string.Format("SELECT USERNAME,SN FROM T_USER WHERE BOUNDIP ='{0}'", clientIP);
            DataTable dtUser = m_Database.GetDataTable(strSQL);
            if (dtUser.Rows.Count > 0)
                userInfo = dtUser.Rows[0][0] + "|" + dtUser.Rows[0][1];
            return userInfo;


        }
        public string ChangePassword(string userName, string passold, string passnew)
        {
            string strSQL = "SELECT UserName,SN FROM T_User WHERE UserName='" + userName + "' AND SN='" + passold + "'";
            SqlDataReader UserPsw = m_Database.GetDataReader(strSQL);
            if (UserPsw.HasRows)
            {
                strSQL = "UPDATE  T_User SET SN='" + passnew + "'  WHERE UserName='" + userName + "'";
                m_Database.Execute(strSQL);
                return "1";
            }
            else
            {
                return "2";
            }

        }
        //public User UserAuthority()
        //{

        //}

    }
}
