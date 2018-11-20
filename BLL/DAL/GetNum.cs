using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Readearth.Data;
using Newtonsoft.Json;
using System.Web;
 

namespace MMShareBLL.DAL
{
    public class HeathWeatheReport
    {
        public static Database m_Database = new Database("DBCONFIGII");
        /// <summary>
        /// 获取发送ftp、邮件、短信的数量以及统计总和
        /// </summary>
        /// <param name="sd">时段，是上午还是下午</param>
        /// <returns></returns>
        public static string GetSendNum(string Date)
        {
            try
            {
                Date = HttpUtility.UrlDecode(Date);
                Date = (Convert.ToDateTime(Date).ToString("yyyy-MM-dd")).ToString();
                string startHour = " 00:00:00";
                string endHour = " 23:59:59";

                string sql = " select * from ( Select SENDUSER,RECEIVEUSER,TYPE,HealthyType,t1.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功'ELSE '失败'END AS SendStatus," +
                             "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是'ELSE '否'END AS ISALL,t2.WindowsUser FROM V_SENDLOG  	 t1 left join T_User t2  on t1.SendUser=t2.Alias where Type<>'FTP' " +

                             "union all Select SENDUSER,reciver,TYPE,HealthyType,t3.EMAIL,PHONE,CASE SendStatus WHEN 1 THEN '成功' ELSE '失败' END AS SendStatus, " +
                             "CONVERT(VARCHAR(19),SENDDATE,121) as 'SENDDATE',CASE ISALL WHEN 1 THEN '是' ELSE '否'END AS ISALL,t4.WindowsUser FROM V_SENDLOGII    t3 left join T_User t4  on t3.SendUser=t4.Alias where Type='FTP' ) T  " +
                            " WHERE T.SENDDATE BETWEEN '" + Date + startHour + "' and '" + Date + endHour + "' AND T.WindowsUser='长三角'";

                DataTable dt = m_Database.GetDataTable(sql);
                string[] status = { "状态", "成功" };
                string[] e_status = { "title", "sucess", "failure", "total" };
                string[] type = { "FTP", "短信", "邮件" };
                DataTable ndt = new DataTable("GetNum");
                DataColumn dc2 = new DataColumn("FTP", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("短信", Type.GetType("System.String"));
                DataColumn dc4 = new DataColumn("邮件", Type.GetType("System.String"));
                ndt.Columns.Add(dc2);
                ndt.Columns.Add(dc3);
                ndt.Columns.Add(dc4);
                int ftp = 0, mess = 0, email = 0;//各类型的总和
                for (int i = 0; i < status.Length; i++)
                {
                    DataRow[] row = dt.Select("SendStatus='" + status[i] + "'");
                    int ftp_count = 0, mess_count = 0, email_count = 0;
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (row[j]["TYPE"].ToString() == type[0])
                        {
                            ftp_count++;
                        }
                        else if (row[j]["TYPE"].ToString() == type[1])
                        {
                            mess_count++;
                        }
                        else if (row[j]["TYPE"].ToString() == type[2])
                        {
                            email_count++;
                        }
                    }
                    ftp += ftp_count;
                    mess += mess_count;
                    email += email_count;
                    if (i == status.Length - 1)
                    {     //最后一行计算总和
                        DataRow dr1 = ndt.NewRow();
                        dr1["FTP"] = ftp;
                        dr1["短信"] = mess;
                        dr1["邮件"] = email;
                        ndt.Rows.Add(dr1);
                    }
                    else if (i != 0)
                    {
                        DataRow dr2 = ndt.NewRow();
                        dr2["FTP"] = ftp_count;
                        dr2["短信"] = mess_count;
                        dr2["邮件"] = email_count;
                        ndt.Rows.Add(dr2);
                    }
                }
                string JsonString = string.Empty;
                JsonString = JsonConvert.SerializeObject(ndt);
                return JsonString;
            }
            catch(Exception ex) {
                return "返回数据失败！ 原因:"+ex.Message;
            }
        }
    }
}
