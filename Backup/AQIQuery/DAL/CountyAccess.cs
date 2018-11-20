using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace AQIQuery.DAL
{
    class CountyAccess
    {

        /// <summary>
        /// 区县用户获取数据日志
        /// </summary>
        /// <param name="DataUserID">用户ID</param>
        /// <returns>1代表成功，0代表失败</returns>
        public static int CountyAccessLog_AddNew(int DataUserID)
        {
            int result = 0;
            SqlParameter[] para = {
                                      new SqlParameter("@DataUserID",DataUserID)
                                  };
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "CountyAccessLog_AddNew", para);
                result = 1;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("[CountyAccessLog_AddNew]:" + str);
                //return false;
                //throw;
            }

            return result;

        }
    }
}
