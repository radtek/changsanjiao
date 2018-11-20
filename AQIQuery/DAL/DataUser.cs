using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AQIQuery.DAL
{
    class DataUser
    {
        /// <summary>
        /// 区县用户使用，根据用户名和密码获取可以访问的GroupID权限。
        /// 用到了存储过程：[DataUser_Get_byUserandPassword]
        /// </summary>
        /// 
        /// <returns>返回GroupID,如果返回0，则失败</returns>
        /// 日期：2014-4-24日
        public static int GetAuthorizedGroupID(string UserName,string Password)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@UserName", UserName),
                                      new SqlParameter("@Password", Password)
                                  };
            int result = 0;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "DataUser_Get_byUserandPassword", para);
                DataTable dt =
                    ds.Tables[0];
                if (dt == null)
                    return result;
                result = int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine(str);
                //throw;
            }

            return result;
        }
    }
}
