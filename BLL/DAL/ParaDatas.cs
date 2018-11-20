using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 数据查询的接口，这个可以作为共用的接口给外部访问
    /// </summary>
    public  class ParaDatas
    {
        private  Database m_DatabaseS;
        private Database m_Database_dmc;
        private  string EXCUTESQL = " select  SiteID, ParameterID, LST, Value  from SEMC_DMS.DBO.Data  " +
                                   " where SiteID in ({0}) and ParameterID in ({1}) and " +
                                   " (CONVERT(VARCHAR(19),LST,120) between '{2}' and '{3}') " +
                                   " and (qccode in (0,1,2,3,4,5,6,10,11) ) and value>0 and DurationID=10 " +
                                   " order by ParameterID,LST desc ";

        private string EXCUTESQLCITY = "    SELECT ParameterID,LST, Value FROM D_GroupData WHERE GroupID='SG00000001'" +
                                       " AND LST BETWEEN '{1}' AND '{2}' "+
                                       " AND Duration='0' AND ParameterID IN({0}) order by lst  asc ";

        public  ParaDatas()
        {
            m_DatabaseS = new Database("SEMCDMS");
            m_Database_dmc = new Database("conStr_SEMC_DMC");
        }

        /// <summary>
        /// 获取站点小时数据实时
        /// </summary>
        /// <param name="lst1"></param>
        /// <param name="lst2"></param>
        /// <param name="SiteID"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public  DataTable SiteHourlyData(DateTime lst1, DateTime lst2, string SiteID, string paras) {
           
            try
            {
                string sql = string.Format(EXCUTESQL, SiteID, paras,
                                             lst1.ToString("yyyy-MM-dd HH:mm:ss"),
                                             lst2.ToString("yyyy-MM-dd HH:mm:ss"));
               return  m_DatabaseS.GetDataTable(sql);
            }
            catch { }
            return null ; 
        }


        /// <summary>
        /// 获取全市小时数据实时
        /// </summary>
        /// <param name="lst1"></param>
        /// <param name="lst2"></param>
        /// <param name="SiteID"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public DataTable CityHourlyData(DateTime lst1, DateTime lst2, string paras)
        {

            try
            {
                string sql = string.Format(EXCUTESQLCITY,  paras,
                                             lst1.ToString("yyyy-MM-dd HH:mm:ss"),
                                             lst2.ToString("yyyy-MM-dd HH:mm:ss"));
                return m_Database_dmc.GetDataTable(sql);
            }
            catch { }
            return null;
        }



    }
}
