using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using eshop.DAL;

namespace AQIQuery.DAL
{
     class GroupSite
    {
        /// <summary>
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetAllAQISite
        /// </summary>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间, 东京时间为2014-2-20
        public static string GetAllAQISiteIDString()
        {
           // SqlParameter[] para = null;
            string result = string.Empty;
            try
            {
                DataTable dt = GroupSite.GetAllAQISiteID();
                if (dt.Equals(null)) return result;
                string siteID = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        siteID = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                       siteID = siteID+string.Format(",{0}",dt.Rows[i][0].ToString());
                    }
                }
                result = siteID;

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
        /// 根据GroupID获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupID
        /// </summary>
        /// <param name="GroupID">站点分组的ID号</param>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间
        public static string GetSiteIDbyGroupIDString(int GroupID)
        {

            string result = string.Empty;
            try
            {
                DataTable dt = GroupSite.GetSiteIDbyGroupID(GroupID);
                string siteID = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        siteID = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                       siteID = siteID+string.Format(",{0}",dt.Rows[i][0].ToString());
                    }
                }
                result = siteID;
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
        /// 获取全部AQI站点（包含国控点和市控点）
        /// 用到了存储过程：SiteGroupSites_GetAllAQISite
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间
        public static DataTable GetAllAQISiteID()
        {
            // SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroupSites_GetAllAQISite");
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
        /// <summary>
        ///根据GroupID获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupID
        /// </summary>
        /// <param name="GroupID">站点分组的ID号</param>
        /// <returns>返回DataTable</returns>
        /// 日期：2014-2-19日于太平洋上空，旧金山时间

        public static DataTable GetSiteIDbyGroupID(int GroupID)
        {

            SqlParameter[] para = {
                                     new SqlParameter("@GroupID", GroupID)
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroupSites_GetSitebyGroupID", para);
                DataTable dt = ds.Tables[0];
                result = dt;
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
        /// 根据GroupIDs获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupIDs
        /// </summary>
        /// <param name="GroupIDs">多个分组的ID号，用","分隔</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetSiteIDbyGroupIDs(string GroupIDs)
        {

            SqlParameter[] para = {
                                     new SqlParameter("@GroupIDs", GroupIDs)
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "SiteGroupSites_GetSitebyGroupIDs", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("SiteGroupSites_GetSitebyGroupIDs:"+str);
                //throw;
            }

            return result;
        }

        /// <summary>
        /// 根据GroupIDs获取其中的AQI站点编号
        /// 用到了存储过程：SiteGroupSites_GetSitebyGroupIDs
        /// </summary>
        /// <param name="GroupIDs">多个分组的ID号，用","分隔</param>
        /// <returns>返回string ，每个站点之间用","分隔开</returns>
        public static string GetSiteIDbyGroupIDsString(string GroupIDs)
        {

            string result = string.Empty;
            try
            {
                DataTable dt = GroupSite.GetSiteIDbyGroupIDs(GroupIDs);
                string siteID = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        siteID = dt.Rows[i][0].ToString();
                    }
                    else
                    {
                        siteID = siteID + string.Format(",{0}", dt.Rows[i][0].ToString());
                    }
                }
                result = siteID;
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
        /// 根据GroupIDs获取其中的AQI站点信息，包括站点编码、站点名称、经度、纬度等信息
        /// </summary>
        /// <param name="GroupID">区县分组编码</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetSiteInfobyGroupID(int GroupID)
        {

            SqlParameter[] para = {
                                     new SqlParameter("@GroupID", GroupID)
                                  };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "[Site_GetSitebyGroupID]", para);
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("[Site_GetSitebyGroupID]:" + str);
                //throw;
            }

            return result;
        }

         /// <summary>
        /// 获取所有AQI站点信息，包括站点编码、站点名称、经度、纬度等信息
        /// </summary>
        /// <returns>返回DataTable</returns>
        /// 日期:2014-5-8
        public static DataTable Site_GetAllAQISites()
        {

            //SqlParameter[] para = {
            //                         new SqlParameter("@GroupID", GroupID)
            //                      };
            //SqlParameter[] para = null;
            DataTable result = null;
            try
            {
                DataSet ds = SQLHelper.ExecuteDataset(SQLHelper.connectionStringAQI, CommandType.StoredProcedure, "[Site_GetAllAQISites]");
                DataTable dt = ds.Tables[0];
                result = dt;
            }
            catch (Exception e)
            {
                string str = e.ToString();
                Console.WriteLine("[Site_GetAllAQISites]:" + str);
                //throw;
            }

            return result;
        }

         



    }
}
