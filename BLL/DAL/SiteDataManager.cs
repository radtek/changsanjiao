using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MMShareBLL.Model;
using Readearth.Data;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 站点数据管理
    /// </summary>
    public class SiteDal
    {
        //Procedure
        private const string PROC_QUERYSITEBYID_DMS = "getSiteDataById_DMS";
        private const string PROC_QUERYSITE_DMS = "getSiteData_DMS";
        private const string PROC_QUERYSITEBYCOUNTY_DMS = "getCountyById_DMS";
        

        //Parameter
        private const string PARAM_ID = "@siteId";
        private const string PARAM_CountID = "@countyId";


        private Database m_DatabaseS;
        public SiteDal()
        {
            m_DatabaseS = new Database("conStr_SEMC_DMC");//new 
        }

        public Dictionary<int, Site> Query()
        {
            Dictionary<int, Site> theData = new Dictionary<int, Site>();
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_QUERYSITE_DMS))
            {
                while (sdr.Read())
                {
                    Site site = new Site();
                    site.Id = Convert.ToInt32(sdr["SiteID"]);
                    site.DCode = site.Id;
                    site.Name = sdr["SiteName"] as string;
                    site.Longitude = float.Parse(sdr["Longitude"].ToString());
                    site.Latitude = float.Parse(sdr["Latitude"].ToString());
                    object siteCode = sdr["ModeId"];
                    object dmcCode = sdr["DMCId"];
                    object orderId = sdr["OrderId"];
                    object byname2 = sdr["Byname2"];
                    if (DBNull.Value != siteCode && null != siteCode)
                        site.SiteCode = Convert.ToInt32(siteCode);
                    if (DBNull.Value != dmcCode && null != dmcCode)
                        site.DCode = Convert.ToInt32(dmcCode);
                    if (DBNull.Value != orderId && null != orderId)
                        site.OrderId = Convert.ToInt32(orderId);
                    if (DBNull.Value != byname2)
                        site.ByName2 = byname2.ToString();
                    theData[site.Id] = site;
                }
            }
            return theData;
        }

        public Dictionary<int, Site> Query1(int intCountID)
        {
            SqlParameter paramCountID = new SqlParameter(PARAM_CountID, System.Data.SqlDbType.Int);
            paramCountID.Value = intCountID;

            Dictionary<int, Site> theData = new Dictionary<int, Site>();
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_QUERYSITEBYCOUNTY_DMS, paramCountID))
            {
                while (sdr.Read())
                {
                    Site site = new Site();
                    site.Id = Convert.ToInt32(sdr["SiteID"]);
                    site.DCode = site.Id;
                    site.Name = sdr["SiteName"] as string;
                    site.Longitude = float.Parse(sdr["Longitude"].ToString());
                    site.Latitude = float.Parse(sdr["Latitude"].ToString());
                    object siteCode = sdr["ModeId"];
                    object dmcCode = sdr["DMCId"];
                    object orderId = sdr["OrderId"];
                    object byname2 = sdr["Byname2"];
                    if (DBNull.Value != siteCode && null != siteCode)
                        site.SiteCode = Convert.ToInt32(siteCode);
                    if (DBNull.Value != dmcCode && null != dmcCode)
                        site.DCode = Convert.ToInt32(dmcCode);
                    if (DBNull.Value != orderId && null != orderId)
                        site.OrderId = Convert.ToInt32(orderId);
                    if (DBNull.Value != byname2)
                        site.ByName2 = byname2.ToString();
                    theData[site.Id] = site;
                }
            }
            return theData;
        }

        public Site Query(int siteID)
        {
            SqlParameter paramSiteID = new SqlParameter(PARAM_ID, System.Data.SqlDbType.Int);
            paramSiteID.Value = siteID;
            try
            {
                County county = null;
                using (SqlDataReader reader = m_DatabaseS.GetDataReader( PROC_QUERYSITEBYID_DMS, paramSiteID))
                {
                    if (reader.Read())
                    {
                        CountyDal cd = new CountyDal();
                        county = new County();
                        county.Id = Convert.ToInt32(reader["CountyID"]);
                        county.Name = reader["CountyName"] as string;
                        cd.DistillSiteData(reader, ref county);
                    }
                }

                if (county != null && county.Sites.Count > 0)
                    return county.Sites[0];
                else
                    return null;
            }
            catch { throw; }
        }
    }

}
