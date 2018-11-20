using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
//using eshop.DAL;
using System.Data;

namespace AQIQuery.DAL
{
    class UpdateTime
    {
        /// <summary>
        /// 根据UpdateName获取最新的更新时间
        /// 用到了存储过程：Updatetime_Get_byName
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-3-18日
        public static DateTime Updatetime_Get_byName(string UpdateName)
        {
            DateTime result=Convert.ToDateTime("1900-1-1 0:00:00");
            SqlParameter[] para = {
                                     new SqlParameter("@Name", UpdateName)
                                  };
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Updatetime_Get_byName", para);
                DataTable dt = ds.Tables[0];
                if (dt.Equals(null)) return result;
                string dtString =dt.Rows[0][0].ToString();
                return DateTime.Parse(dtString);

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Updatetime_Get_byName:"+str);
                //throw;
                 
            }

            return result;

        }

        /// <summary>
        /// UpdateName更新最新的更新时间
        /// 用到了存储过程：Updatetime_Update
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-4-13日
        public static int Updatetime_Update(string UpdateName, DateTime Updatetime_Update)
        {
            int r1 = 0;
            DateTime result = Convert.ToDateTime("1900-1-1 0:00:00");
            SqlParameter[] para = {
                                     new SqlParameter("@Name", UpdateName),
                                     new SqlParameter("@Updatetime_Update", Updatetime_Update)
                                  };
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Updatetime_Update", para);
                r1 = 1;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Updatetime_Update:" + str);
                //throw;

            }

            return r1;

        }


        /// <summary>
        /// DMS的更新最新的更新时间
        /// 用到了存储过程：Updatetime_Update_DMSNow
        /// </summary>
        /// <returns>返回1代表成功，0代表失败</returns>
        /// 日期：2014-4-17日
        public static int Updatetime_Update_DMSNow()
        {
            int r1 = 0;
            SqlParameter[] para = null;
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "Updatetime_Update_DMSNow",para);
                r1 = 1;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("Updatetime_Update_DMSNow:" + str);
                //throw;

            }

            return r1;

        }
    }
}
