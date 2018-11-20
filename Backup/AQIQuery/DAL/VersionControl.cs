using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace AQIQuery.DAL
{
     class VersionControl
    {
        /// <summary>
        /// 根据操作系统名称（OSName）获取全部的更新项目名称和版本，版本号为保留两位小数的实数
        /// 用到了存储过程：[VersionControl_Get_byOSName]
        /// </summary>
        /// 
        /// <returns>返回DataTable</returns>
        /// 日期：2014-4-24日
        public static DataTable GetVersionListbyOSName(string OSName)
        {
            SqlParameter[] para = {
                                      new SqlParameter("@OSName", OSName),
                                  };
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure,
                                                        "VersionControl_Get_byOSName",para);
                result = ds.Tables[0];
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
