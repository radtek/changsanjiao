using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using System.Collections.Generic;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 对区县数据的获取
    /// </summary>
    public class CountyDal
    {

        //Procedure
        //private const string PROC_GETALLCOUNTYDATA = "GetAllCountyData";
        //private const string PROC_GETALLCOUNTYDATABYID = "GetAllCountyDataById";

        private const string PROC_GETALLCOUNTYDATA_DMS = "getCounty_DMS";
        private const string PROC_GETALLCOUNTY1DATA_DMS = "getCounty1_DMS";
        private const string PROC_GETCOUNTDATABYID_DMS = "getCountyById_DMS";

        //Parameters
        private const string PARAM_COUNTYID = "@countyId";

        private Database m_DatabaseS;
        public CountyDal()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        /// <summary>
        /// 查询完整的区县数据，包括该区县下的监测站点的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, County> Query()
        {

            Dictionary<int, County> countyData = new Dictionary<int, County>();

            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_GETALLCOUNTYDATA_DMS))
            {
                County county = null;

                while (sdr.Read())
                {
                    int cid = Convert.ToInt32(sdr["CountyID"]);
                    if (countyData.ContainsKey(cid))
                        county = countyData[cid];
                    else
                    {
                        county = new County();
                        countyData.Add(cid, county);
                    }
                    county.Id = cid;
                    county.Name = sdr["CountyName"] as string;
                    DistillSiteData(sdr, ref county);
                }
            }
            return countyData;
        }

        /// <summary>
        /// 得到所有的区
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, County> Query1()
        {

            Dictionary<int, County> countyData = new Dictionary<int, County>();

            using (SqlDataReader sdr = m_DatabaseS.GetDataReader(PROC_GETALLCOUNTY1DATA_DMS))
            {
                County county = null;

                while (sdr.Read())
                {
                    int cid = Convert.ToInt32(sdr["CountyID"]);
                    if (countyData.ContainsKey(cid))
                        county = countyData[cid];
                    else
                    {
                        county = new County();
                        countyData.Add(cid, county);
                    }
                    county.Id = cid;
                    county.Name = sdr["Name"] as string;
                }
            }
            return countyData;
        }

        /// <summary>
        /// 根据区县编号获取区县信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public County Query(int id)
        {
            SqlParameter paramID = new SqlParameter(PARAM_COUNTYID, SqlDbType.Int);
            paramID.Value = id;
            try
            {
                County county = null;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_GETCOUNTDATABYID_DMS, paramID))
                {

                    while (reader.Read())
                    {
                        if (county == null)
                        {
                            county = new County();
                            county.Id = Convert.ToInt32(reader["CountyID"]);
                            county.Name = reader["CountyName"] as string;
                        }
                        DistillSiteData(reader, ref county);
                    }
                }
                return county;
            }
            catch { throw; }
        }

        public void DistillSiteData(SqlDataReader sdr, ref County county)
        {
            if (sdr["SiteID"] == DBNull.Value)
                return;
            Site site = new Site();
            site.Id = Convert.ToInt32(sdr["SiteID"]);
            site.DCode = site.Id;
            site.Name = sdr["SiteName"] as string;
            site.Longitude = float.Parse(sdr["Longitude"].ToString());
            site.Latitude = float.Parse(sdr["Latitude"].ToString());
            //object siteCode = sdr["ModeId"];--ModeId无用
            object dmcCode = sdr["DMCId"];
            object orderId = sdr["OrderId"];
            object siteCode = sdr["SiteCode"];
            object byname2 = sdr["Byname2"];
            //if (DBNull.Value != siteCode && null != siteCode)
            //    site.SiteCode = Convert.ToInt32(siteCode);
            if (DBNull.Value != dmcCode && null != dmcCode)
                site.DCode = Convert.ToInt32(dmcCode);
            if (DBNull.Value != orderId && null != orderId)
            {
                site.OrderId = Convert.ToInt32(orderId);

            }
            if (DBNull.Value != siteCode && null != siteCode)
            {
                site.SiteCode = Convert.ToInt32(siteCode);
            }
            if (DBNull.Value != byname2)
                site.ByName2 = byname2.ToString();
            site.Byname = sdr["byName"] as string;
            site.SiteType = SiteType.NoSet;
            site.County = county;
            county.Sites.Add(site);
        }
    }
}

