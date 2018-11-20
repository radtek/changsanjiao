using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using eshop.DAL;

namespace AQIQuery.DAL
{
    class Group
    {
        /// <summary>
        /// 获取GroupID，根据GroupName，限制为二级节点，名字不会重复，所以不会获取到两个或以上的GoupID。
        /// 用到了存储过程：SiteGroup_GetGroupIDbyName
        /// </summary>
        /// <param name="GroupName">二级分组名称</param>
        /// <returns>返回int，如果返回值为-1，说明返回无效</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间，东京时间为：2014-2-20 10:01
        public static int GetGroupIDbyName(string GroupName)
        {
            SqlParameter[] para = {
                                     new SqlParameter("@GroupName", GroupName)
                                  };

            int result = -1;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroup_GetGroupIDbyName",para);
                if (ds.Tables[0].Rows.Count < 1) return result;
                string rst = ds.Tables[0].Rows[0][0].ToString();
                result = int.Parse(rst);
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine(str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 获取所以分区ID和名称
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间，东京时间为：2014-2-20 10:16
        /// 用到了存储过程：SiteGroup_GetAllDistrict
        public static DataTable GetAllDistrict()
        {

            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroup_GetAllDistrict");
                result = ds.Tables[0];
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroup_GetAllDistrict: "+str);
                //throw;
            }

            return result;
        }


        /// <summary>
        /// 日期：2014-3-16日
        /// 获取所以分区ID和名称
        /// </summary>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        /// 用到了存储过程：SiteGroup_GetAllDistrict
        public static string GetAllDistrictGroupIDsString()
        {

            string result = string.Empty;
            try
            {
                DataTable dt = GetAllDistrict();
                if (dt.Equals(null)) return result;
                string GroupIDs = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        GroupIDs = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        GroupIDs = GroupIDs + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = GroupIDs;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("GetAllDistrictGroupIDsString: "+str);
                //throw;
            }
            return result;
        }


        /// <summary>
        /// 日期：2014-3-16
        /// 获取全市平均和所以分区ID和名称
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 用到了存储过程：SiteGroup_GetAllCityorDistrict
        public static DataTable GetAllCityorDistrict(bool isCity, bool isDistrict)
        {

            DataTable result = null;

            SqlParameter[] para = {
                                     new SqlParameter("@isCity", isCity),
                                     new SqlParameter("@isDistrict", isDistrict)
                                  };
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroup_GetAllCityorDistrict",para);
                result = ds.Tables[0];
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroup_GetAllCityorDistrict: " + str);
                //throw;
            }

            return result;
        }



        /// <summary>
        /// 日期：2014-3-16日
        /// 获取全市平均和所以分区ID
        /// 用到了存储过程：SiteGroup_GetAllCityorDistrict
        /// </summary>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static string GetAllCityorDistrictGroupIDsString(bool isCity,bool isDistrict)
        {

            string result = string.Empty;
            try
            {
                DataTable dt = GetAllCityorDistrict(isCity,isDistrict);
                if (dt.Equals(null)) return result;
                string GroupIDs = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        GroupIDs = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        GroupIDs = GroupIDs + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = GroupIDs;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroup_GetAllCityorDistrict: " + str);
                //throw;
            }
            return result;
        }


        
        /// <summary>
        /// 日期：2014-3-17
        /// 获取通过SiteIDs获取这些SiteIDs所在的GroupID，GroupName等
        /// 用到了存储过程：SiteGroup_GetGroup_bySiteIDs
        /// </summary>
        /// <SiteIDs>站点编号，以逗号分隔</SiteIDs>
        /// <mustBeProecss>必须是需要加工的分组吗</mustBeProecss>
        /// <returns>返回DataTable</returns>
        public static DataTable GetGetGroup_bySiteIDs(string SiteIDs, bool mustBeProcess)
        {

            DataTable result = null;

            SqlParameter[] para = {
                                     new SqlParameter("@SiteIDs", SiteIDs),
                                     new SqlParameter("@mustBeProcess", mustBeProcess)
                                  };
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroup_GetGroup_bySiteIDs", para);
                result = ds.Tables[0];
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroup_GetGroup_bySiteIDs: " + str);
                //throw;
            }

            return result;
        }

        /// 日期：2014-3-17
        /// 获取通过SiteIDs获取这些SiteIDs所在的GroupID，GroupName等
        /// 用到了存储过程：SiteGroup_GetGroup_bySiteIDs
        /// </summary>
        /// <SiteIDs>站点编号，以逗号分隔</SiteIDs>
        /// <mustBeProecss>必须是需要加工的分组吗</mustBeProecss>
        /// <returns>返回string, 各GroupID之间用","分隔</returns>
        public static string GetGetGroup_bySiteIDsString(string SiteIDs, bool mustBeProcess)
        {
            string result = string.Empty;
            try
            {
                DataTable dt = GetGetGroup_bySiteIDs(SiteIDs, @mustBeProcess);
                if (dt.Equals(null)) return result;
                string GroupIDs = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        GroupIDs = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        GroupIDs = GroupIDs + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = GroupIDs;

            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroup_GetAllCityorDistrict : " + str);
                //throw;
            }
            return result;
        }

    }
}
