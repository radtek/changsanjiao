using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MMShareBLL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;

namespace MMShareBLL.DAL
{
    class ManageSystem
    {
       private Database m_Database;
       public ManageSystem()
        {
            m_Database = new Database();
        }
        public string UserNameInit()
        {
            string strSQL = "SELECT UserName FROM T_user  WHERE BZ in (1,2) ORDER BY UserName";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.Append("所有" + "|");
                foreach (DataRow rows in dt.Rows)
                {
                    sb.Append(rows[0].ToString() + "|");
                }
                return sb.Remove(sb.Length - 1, 1).ToString();
            }
            else
                return "";
        }
        public string UserManageFun(string userName)
        {
            string strSQL;
            if (userName == "所有")
                strSQL = "SELECT UserName,SN,Alias,BZ,EMail,DateTime FROM T_user WHERE BZ in (1,2) ORDER BY UserName";
            else
                strSQL = "SELECT UserName,SN,Alias,BZ,EMail,DateTime FROM T_user WHERE UserName='" + userName + "' ORDER BY UserName";
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='LogDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0' style='table-layout: fixed'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitleUserLeft' style='width:4%'><input type='checkbox' name='AllCheck' onclick='AllSelect()'/></td>");
            sb.AppendLine("<td class='tabletitleUser'>用户名</td>");
            sb.AppendLine("<td class='tabletitleUser'>别名</td>");
            sb.AppendLine("<td class='tabletitleUser'>角色</td>");
            sb.AppendLine("<td class='tabletitleUser'>邮箱</td>");
            sb.AppendLine("<td class='tabletitleUser'>修改时间</td>");
            sb.AppendLine("<td class='tabletitleUser' style='width:5%'>编辑</td>");
            sb.AppendLine("<td class='tabletitleUser' style='width:5%'>删除</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr onmouseover='mouseOver(this)' onmouseout='mouseOut(this)'>"));
                sb.AppendLine(string.Format("<td class='LogtablerowLeft' style='width:4%'><input type='checkbox' name='UserCheck' value ='{0}'></td>",dr["UserName"].ToString()));
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 1)
                        continue;
                    if (i == 3)
                    {
                        if(dr[i].ToString()=="1")
                            sb.AppendLine(string.Format("<td class='Logtablerow'>{0}</td>", "环境监测中心"));
                        if(dr[i].ToString()=="2")
                            sb.AppendLine(string.Format("<td class='Logtablerow'>{0}</td>", "气象局"));
                    }
                    else if(i==0)
                        sb.AppendLine(string.Format("<td class='Logtablerow'>{0}</td>", dr[i].ToString()));
                    else
                        sb.AppendLine(string.Format("<td class='Logtablerow'>{0}</td>", dr[i].ToString()));
                }
                sb.AppendLine(string.Format("<td class='Logtablerow' style='width:5%' ><a href=\"javascript:editorUser('{0}','{1}');\"><img src='images/edit.png'/></a></td>", dr["UserName"].ToString(), dr["SN"].ToString()));
                sb.AppendLine(string.Format("<td class='Logtablerow' style='width:5%'><a href=\"javascript:deleteUser('{0}','{1}');\"><img src='images/deleteIcon.png'/></a></td>", dr["UserName"].ToString(), dr["SN"].ToString()));
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }
        public string ReEditor(string userName, string password)
        {
            string strSQL;
            strSQL = "SELECT UserName,SN,Alias,BZ,EMail FROM T_user WHERE UserName='" + userName + "' AND SN='" + password + "' ORDER BY UserName";
            DataTable dt = m_Database.GetDataTable(strSQL);
            string sb="";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb = sb + dt.Rows[0][i].ToString() + "|";
                }
            }
            sb = sb.Substring(0, sb.Length-1);
            return sb;

        }
        public string OkEditor(string name, string psw, string alias, string bz, string email, string nameOld, string passWordOld)
        {
            string strSQL;
            try
            {
                strSQL = @"UPDATE T_user SET UserName='" + name + "',SN='" + psw + "',Alias='" + alias + "',BZ='" + bz + "',EMail='" + email + "',DateTime=GetDate() WHERE  UserName='" + nameOld + "' AND SN='" + passWordOld + "'";
                m_Database.Execute(strSQL);
                return "更新成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        public string AddUserEditor(string name, string psw, string alias, string bz, string email)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT UserName,SN FROM T_user WHERE UserName='" + name + "'";
                DataTable dt = m_Database.GetDataTable(strSQL);
                if (dt.Rows.Count > 0)
                {
                    return "用户已存在";
                }
                else
                {
                    strSQL = @"INSERT INTO T_user(UserName,SN,BZ,Alias,EMail,DateTime)  VALUES('" + name + "', '" + psw + "','" + bz + "','" + alias + "','" + email + "',GetDate())";
                    m_Database.Execute(strSQL);
                    return "插入成功";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        public string DeleteUser(string userName, string password)
        {
            string strSQL;
            try
            {
                strSQL = "DELETE FROM T_user WHERE UserName='" + userName + "' AND SN='" + password + "'";
                m_Database.Execute(strSQL);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string DeleteCheckUser(string checkUser)
        {
            string strSQL;
            try
            {
                strSQL = "DELETE FROM T_user WHERE UserName in " + checkUser + "";
                m_Database.Execute(strSQL);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
