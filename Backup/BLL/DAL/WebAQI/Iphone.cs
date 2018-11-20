using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using AQIQuery.aQuery;
using System.Collections;
using DemoApplication;
using System.Web;
using ChinaAQI;
namespace MMShareBLL.DAL.WebAQI
{
    public class Iphone
    {
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Database m_Database;
        private const double EARTH_RADIUS = 6378.137;
        IList<Station> List = new List<Station>();
        string ip = HttpContext.Current.Request.UserHostAddress;
        public Iphone()
        {
            m_Database = new Database("AQIWEB");
        }
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        //距离已知经纬度地方的最近的站点信息
        public DataTable siteDataTable()
        {
            string site = Site.GetAllAQISiteIDString();
            string strSQL = "SELECT SiteID,StationID,Name,Latitude,Longitude FROM Site WHERE SiteID in (" + site + ")";
            DataTable dt = m_Database.GetDataTable(strSQL);
            return dt;
        }
        public string nearSite(string lat, string lng, DataTable dt)
        {
            double lat1 = double.Parse(lat);
            double lng1 = double.Parse(lng);
            double minDistance = 100000000000.0;
            int SiteID = 0;
            double lat2;
            double lng2;
            double distance;
            foreach (DataRow rows in dt.Rows)
            {
                lat2 = double.Parse(rows["Latitude"].ToString());
                lng2 = double.Parse(rows["Longitude"].ToString());
                distance = GetDistance(lat1, lng1, lat2, lng2);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    SiteID = int.Parse(rows["SiteID"].ToString());
                }
            }
            return SiteID.ToString() + "," + minDistance.ToString();
        }
        //只有数据没有经纬度

        public IList<DataStation> IPhoneDataSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return DataSiteData(lat, lng, IMEI, siteIDs, groupIDs, "iPhone");
        }
        public IList<DataStation> AndroidDataSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return DataSiteData(lat, lng, IMEI, siteIDs, groupIDs, "Android");
        }
        public IList<DataStation> DataSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs, string MobileType)
        {
            IList<DataStation> DataStationList = new List<DataStation>();
            string group = "";
            if (groupIDs == "-1")//全部区号
            {
                group = "102," + SiteGroup.GetAllDistrictGroupIDsString();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
            }
            else if (groupIDs == "-2")//都不要
                group = "";
            else
                group = groupIDs;
            if (group != "")
            {
                DataTable table = Data.GroupHourlyAQI(1,group);
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                string[] groupArray = group.Split(',');
                string filter;
                string groupIDNul = "";
                string lstAQI = "";
                if (table.Rows.Count < 1)
                    lstAQI = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                else
                    lstAQI = table.Rows[0][1].ToString();
                for (int k = 0; k < groupArray.Length; k++)
                {
                    groupIDNul = groupArray[k];
                    filter = string.Format("GroupID ={0}", groupIDNul);
                    DataRow[] rows = table.Select(filter);
                    DataStation staion = AddList(1, rows, false, 0.0, groupIDNul, lstAQI);
                    DataStationList.Add(staion);
                }
            }
            //站点
            string site = "";
            DataTable siteTable = Site.GetAllAQISites();
            if (MobileType == "Android")
                ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
            else
                ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
            int nearSiteID = int.Parse(nearSite(lat, lng, siteTable).Split(',')[0]);
            if (siteIDs == "-1")
            {
                site = Site.GetAllAQISiteIDString();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
            }
            else if (siteIDs == "-2")
            {
                if (lat == "-1" && lng == "-1")
                    site = "";
                else
                    site = nearSiteID.ToString(); ;
            }
            else
            {
                if (lat != "-1" && lng != "-1")
                {
                    if (siteIDs.IndexOf(nearSiteID.ToString()) >= 0)
                        site = siteIDs;
                    else
                        site = siteIDs + "," + nearSiteID.ToString();
                }
                else
                    site = siteIDs;
            }
            bool isNear = false;
            string siteID = "";
            double distance = 0.0;
            if (site != "")
            {
                string[] siteArray = site.Split(',');
                DataTable dt = Data.SiteHourlyAQI(1, site);
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                for (int i = 0; i < siteArray.Length; i++)
                {
                    siteID = siteArray[i];
                    string filter = string.Format("SiteID ={0}", siteID);
                    DataRow[] newrow = dt.Select(filter);
                    if (int.Parse(siteID) == nearSiteID)
                    {
                        isNear = true;
                        distance = double.Parse(nearSite(lat, lng, siteTable).Split(',')[1]);
                    }
                    string lstAQI = "";
                    if (dt.Rows.Count < 1)
                        lstAQI = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                    else
                        lstAQI = dt.Rows[0][1].ToString();
                    DataStation staion = AddList(0, newrow, isNear, distance, siteID, lstAQI);
                    DataStationList.Add(staion);
                }
            }
            return DataStationList;
        }
        public DataStation AddList(int tag, DataRow[] newrow, bool isNear, double distance, string id, string AQIlst)
        {
            DataStation staionNew = new DataStation();
            staionNew.setStationID(id);
            staionNew.setLST(AQIlst);
            if (isNear)
            {
                staionNew.setNearestStation(true);
                staionNew.setNeadestDistance(distance);
            }
            if (tag == 0)
                staionNew.setIsGroup(false);
            else
                staionNew.setIsGroup(true);
            if (newrow.Length < 1)
                staionNew.setIsHasData(false);
            else
            {
                for (int j = 0; j < newrow.Length; j++)
                {
                    int AQIValue;
                    double Value;
                    int grade;
                    string  quality;
                    if (newrow[j][7].ToString() == "" || newrow[j][7].ToString().IndexOf("-") >= 0)
                    {
                        AQIValue = -1;
                        Value = -1;
                        grade = -1;
                        quality = "-1";
                    }
                    else
                    {
                        AQIValue = int.Parse(newrow[j][7].ToString());
                        Value = Math.Round(double.Parse(newrow[j][6].ToString()) * 1000, 1);
                        grade = int.Parse(newrow[j][8].ToString());
                        quality = newrow[j][9].ToString();
                    }
                    switch (int.Parse(newrow[j][4].ToString()))
                    {
                        case 101:
                            staionNew.setPm25AQI(AQIValue);
                            staionNew.setPm25Value(Value);
                            staionNew.setPm25Grade(grade);
                            staionNew.setpm25Quality(quality);
                            break;
                        case 103:
                            staionNew.setPm10AQI(AQIValue);
                            staionNew.setPm10Value(Value);
                            staionNew.setPm10Grade(grade);
                            staionNew.setPm10Quality(quality);
                            break;
                        case 104:
                            staionNew.setO3AQI(AQIValue);
                            staionNew.setO3Value(Value);
                            staionNew.setO3Grade(grade);
                            staionNew.setO3Quality(quality);
                            break;
                        case 106:
                            staionNew.setSo2AQI(AQIValue);
                            staionNew.setSo2Value(Value);
                            staionNew.setSo2Grade(grade);
                            staionNew.setSo2Quality(quality);
                            break;
                        case 107:
                            staionNew.setNo2AQI(AQIValue);
                            staionNew.setNo2Value(Value);
                            staionNew.setNo2Grade(grade);
                            staionNew.setNo2Quality(quality);
                            break;
                        case 108:
                            staionNew.setCoAQI(AQIValue);
                            if (Value > 0)
                                Value = Value / 1000;
                            staionNew.setCoValue(Value);
                            staionNew.setCoGrade(grade);
                            staionNew.setCoQuality(quality);
                            break;
                        case 100:
                            staionNew.setPrimaryAQI(AQIValue);
                            staionNew.setPrimaryValue(Value);
                            staionNew.setPrimaryPollutantGrade(grade);
                            staionNew.setPrimaryPollutantQuality(quality);
                            staionNew.setPrimaryPollutantType(newrow[j][5].ToString());
                            break;
                    }
                    staionNew.updateData();

                }
            }
            return staionNew;
        }

        public IList<BasicStation> IPhoneBasicSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return BasicSiteData(lat, lng, IMEI, siteIDs, groupIDs, "iPhone");
        }
        public IList<BasicStation> AndroidBasicSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return BasicSiteData(lat, lng, IMEI, siteIDs, groupIDs, "Android");
        }
        //只有站点信息没有数据
        public IList<BasicStation> BasicSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs, string MobileType)
        {
            IList<BasicStation> BasicList = new List<BasicStation>();
            //分区
            string strSQL = "SELECT GroupName,GroupID FROM SiteGroup";
            DataTable groupNames = m_Database.GetDataTable(strSQL);
            string group = "";
            if (groupIDs == "-1")//全部区号
            {
                group = "102," + SiteGroup.GetAllDistrictGroupIDsString();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
            }
            else if (groupIDs == "-2")//都不要
                group = "";
            else
                group = groupIDs;
            if (group != "")
            {
                string filter;
                string groupIDNul = "";
                DataTable siteIDsGroup = siteDataTable();
                string[] groupArray = group.Split(',');
                for (int k = 0; k < groupArray.Length; k++)
                {
                    double sumLat = 0.0;
                    double sumlng = 0.0;
                    groupIDNul = groupArray[k];
                    BasicStation staionNew = new BasicStation();
                    DataTable dt = SiteGroup.GetSiteIDbyGroupIDs(groupIDNul);
                    if (MobileType == "Android")
                        ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetSiteIDbyGroupIDs", double.Parse(lng), double.Parse(lat));
                    else
                        ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetSiteIDbyGroupIDs", double.Parse(lng), double.Parse(lat));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        filter = string.Format("SiteID ={0}", dt.Rows[i][0]);
                        DataRow[] newRows = siteIDsGroup.Select(filter);
                        sumLat = sumLat + double.Parse(newRows[0][3].ToString());
                        sumlng = sumlng + double.Parse(newRows[0][4].ToString());
                    }//每个区的平均经纬度

                    filter = string.Format("GroupID ={0}", groupIDNul);
                    DataRow[] tempRows = groupNames.Select(filter);
                    if (tempRows.Length > 0)
                    {
                        staionNew.setStationID(tempRows[0][1].ToString());
                        staionNew.setStationName(tempRows[0][0].ToString());
                        double latt = sumLat / dt.Rows.Count;
                        double lngt = sumlng / dt.Rows.Count;
                        staionNew.setLatitude(Math.Round(latt, 6));
                        staionNew.setLongitude(Math.Round(lngt, 6));
                        staionNew.setGroupParentID(groupIDNul);
                        staionNew.setIsGroup(true);
                    }
                    BasicList.Add(staionNew);
                }
            }
            //站点
            string site = "";
            if (siteIDs == "-1")
            {
                site = Site.GetAllAQISiteIDString();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
            }
            else if (siteIDs == "-2")
                site = "";
            else
                site = siteIDs;
            if (site != "")
            {
                DataTable siteTable = Site.GetAllAQISites();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
                string[] siteArray = site.Split(',');
                string siteID = "";
                string siteString = SiteGroup.GetSiteIDbyGroupIDsString("101");//所有国控点的站点编号
                string siteStr = SiteGroup.GetSiteIDbyGroupIDsString("102");//所有市控的站点编号
                if (MobileType == "Android")
                {
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                }
                else
                {
                    ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                    ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                }
                for (int i = 0; i < siteArray.Length; i++)
                {
                    siteID = siteArray[i];
                    BasicStation staionNew = new BasicStation();
                    string parentID = "";
                    if (siteString.IndexOf(siteID) >= 0)
                        parentID = parentID + "101,";
                    string GroupID = SiteGroup.GetGetGroup_bySiteIDsString(siteID, true);
                    if (MobileType == "Android")
                        ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetGetGroup_bySiteIDsString", double.Parse(lng), double.Parse(lat));
                    else
                        ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetGetGroup_bySiteIDsString", double.Parse(lng), double.Parse(lat));
                    parentID = parentID + GroupID;
                    string filter = string.Format("SiteID ={0}", siteID);
                    DataRow[] rows = siteTable.Select(filter);
                    if (rows.Length > 0)
                    {
                        staionNew.setLatitude(Math.Round(double.Parse(rows[0][2].ToString()), 6));
                        staionNew.setLongitude(Math.Round(double.Parse(rows[0][3].ToString()), 6));
                        staionNew.setStationID(rows[0][0].ToString());
                        staionNew.setStationName(rows[0][1].ToString());
                        staionNew.setGroupParentID(parentID);
                        staionNew.setIsGroup(false);
                    }
                    BasicList.Add(staionNew);
                }
            }
            return BasicList;

        }
        public IList<Station> IPhoneSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return siteData(lat, lng, IMEI, siteIDs, groupIDs, "iPhone");
        }
        public IList<Station> AndroidSiteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs)
        {
            return siteData(lat, lng, IMEI, siteIDs, groupIDs, "Android");
        }
        //既有数据又有站点信息
        public IList<Station> siteData(string lat, string lng, string IMEI, string siteIDs, string groupIDs, string MobileType)
        {

            GroupDataList(groupIDs,IMEI,ip,MobileType,lat,lng);
            DataTable siteTable = Site.GetAllAQISites();
            if (MobileType == "Android")
                ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
            else
                ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));
            try
            {
                int nearSiteID = int.Parse(nearSite(lat, lng, siteTable).Split(',')[0]);
                string site = "";
                if (siteIDs == "-1")
                {
                    site = Site.GetAllAQISiteIDString();
                    if (MobileType == "Android")
                        ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
                    else
                        ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISiteIDString", double.Parse(lng), double.Parse(lat));
                }
                else if (siteIDs == "-2")
                {
                    if (lat == "-1" && lng == "-1")
                        site = "";
                    else
                        site = nearSiteID.ToString();
                }
                else
                {
                    if (lat != "-1" && lng != "-1")
                    {
                        if (siteIDs.IndexOf(nearSiteID.ToString()) >= 0)
                            site = siteIDs;
                        else
                            site = siteIDs + "," + nearSiteID.ToString();
                    }
                    else
                        site = siteIDs;
                }
                if (site != "")
                {
                    string[] siteArray = site.Split(',');
                    string siteID = "";
                    bool isNear = false;
                    double distance = 0.0;
                    string siteString = SiteGroup.GetSiteIDbyGroupIDsString("101");//所有国控点的站点编号
                    string siteStr = SiteGroup.GetSiteIDbyGroupIDsString("102");//所有市控的站点编号
                    if (MobileType == "Android")
                    {
                        ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                        ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                    }
                    else
                    {
                        ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                        ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetSiteIDbyGroupIDsString", double.Parse(lng), double.Parse(lat));
                    }
                    DataTable dt = Data.SiteHourlyAQI(1, site);
                    if (MobileType == "Android")
                        ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                    else
                        ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                    for (int i = 0; i < siteArray.Length; i++)
                    {
                        siteID = siteArray[i];
                        string parentID = "";
                        isNear = false;
                        if (siteString.IndexOf(siteID) >= 0)
                            parentID = parentID + "101,";
                        string filter = string.Format("SiteID ={0}", siteID);
                        DataRow[] rows = siteTable.Select(filter);
                        DataRow[] newrow = dt.Select(filter);

                        if (int.Parse(siteID) == nearSiteID)
                        {
                            isNear = true;
                            distance = double.Parse(nearSite(lat, lng, siteTable).Split(',')[1]);
                        }
                        string GroupID = SiteGroup.GetGetGroup_bySiteIDsString(siteID, true);
                        if (MobileType == "Android")
                            ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetGetGroup_bySiteIDsString", double.Parse(lng), double.Parse(lat));
                        else
                            ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetGetGroup_bySiteIDsString", double.Parse(lng), double.Parse(lat));
                        parentID = parentID + GroupID;
                        string lstAQI = "";
                        if (dt.Rows.Count < 1)
                            lstAQI = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                        else
                            lstAQI = dt.Rows[0][1].ToString();
                        List.Add(stationInformation(0, newrow, double.Parse(rows[0][2].ToString()), double.Parse(rows[0][3].ToString()), isNear, distance, parentID, rows, lstAQI));
                    }
                }
            }
            catch { 
                
            }
            return List;
        }
        public void GroupDataList(string groupID, string IMEI, string ip, string MobileType, string lat, string lng)
        {
            string strSQL = "SELECT GroupName,GroupID FROM SiteGroup";
            DataTable groupNames = m_Database.GetDataTable(strSQL);
            string groupIDs = "";
            if (groupID == "-1")//全部区号
            {
                groupIDs = "102," + SiteGroup.GetAllDistrictGroupIDsString();
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetAllDistrictGroupIDsString", double.Parse(lng), double.Parse(lat));
            }
            else if (groupID == "-2")//都不要
                groupIDs = "";
            else
                groupIDs = groupID;
            if (groupIDs != "")
            {
                DataTable table = Data.GroupHourlyAQI(1, groupIDs);
                if (MobileType == "Android")
                    ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                else
                    ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                DataTable siteIDs = siteDataTable();
                string[] groupArray = groupIDs.Split(',');

                string filter;
                string groupIDNul = "";
                for (int k = 0; k < groupArray.Length; k++)
                {
                    double sumLat = 0.0;
                    double sumlng = 0.0;
                    groupIDNul = groupArray[k];
                    DataTable dt = SiteGroup.GetSiteIDbyGroupIDs(groupIDNul);
                    if (MobileType == "Android")
                        ViewLog.AddViewLogAndroid(IMEI, ip, "GroupSite", "GetSiteIDbyGroupIDs", double.Parse(lng), double.Parse(lat));
                    else
                        ViewLog.AddViewLogiPhone(IMEI, ip, "GroupSite", "GetSiteIDbyGroupIDs", double.Parse(lng), double.Parse(lat));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        filter = string.Format("SiteID ={0}", dt.Rows[i][0]);
                        DataRow[] newRows = siteIDs.Select(filter);
                        if (newRows != null && newRows.Length > 0)  //  edited by 薛辉 on 2014-08-15  
                        {
                            sumLat = sumLat + double.Parse(newRows[0][3].ToString());
                            sumlng = sumlng + double.Parse(newRows[0][4].ToString());
                        }
                    }//每个区的平均经纬度

                    filter = string.Format("GroupID ={0}", groupIDNul);
                    DataRow[] rows = table.Select(filter);
                    DataRow[] tempRows = new DataRow[0];
                    if (rows.Length < 1)
                        tempRows = groupNames.Select(filter);
                    string lstAQI = "";
                    if (table.Rows.Count < 1)
                        lstAQI = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                    else
                        lstAQI = table.Rows[0][1].ToString();
                    List.Add(stationInformation(1, rows, sumLat / dt.Rows.Count, sumlng / dt.Rows.Count, false, 0.0, groupIDNul, tempRows, lstAQI));

                }
            }
        }
        public Station stationInformation(int tag, DataRow[] newrow, double lat, double lng, bool near, double distance, string groupID, DataRow[] rows, string lstAQI)
        {
            Station staionNew = new Station();
            staionNew.setLatitude(Math.Round(lat, 6));
            staionNew.setLongitude(Math.Round(lng, 6));
            staionNew.setLST(lstAQI);
            if (near)
            {
                staionNew.setNearestStation(true);
                staionNew.setNeadestDistance(distance);
            }
            if (newrow ==null || newrow.Length < 1)
            {
                staionNew.setIsHasData(false);
                if (tag == 0)
                {
                    if (rows != null && rows.Length > 0)
                    {
                        staionNew.setStationID(rows[0][0].ToString());
                        staionNew.setStationName(rows[0][1].ToString());
                    }
                    else {
                        staionNew.setStationID("");
                        staionNew.setStationName("");
                    }
                    staionNew.setGroupParentID(groupID);
                    staionNew.setIsGroup(false);
                }
                else
                {
                    if (rows != null && rows.Length > 0)
                    {
                        staionNew.setStationName(rows[0][0].ToString());
                    }
                    else {
                        staionNew.setStationName("");
                    }
                    staionNew.setStationID(groupID);
                    staionNew.setIsGroup(true);
                }
                return staionNew;
            }
            if (tag == 0)
                staionNew.setIsGroup(false);
            else
                staionNew.setIsGroup(true);

            staionNew.setGroupParentID(groupID);
            try
            {
                staionNew.setStationID(newrow[0][2].ToString());
                if (newrow[0][3].ToString() == "全市平均")
                    staionNew.setStationName("上海");
                else
                    staionNew.setStationName(newrow[0][3].ToString());
            }
            catch {
                staionNew.setStationID("");
                staionNew.setStationName("");

            }

            for (int j = 0; j < newrow.Length; j++)
            {
                try
                {
                    int AQIValue;
                    double Value;
                    int grade;
                    string quality;
                    if (newrow[j][7].ToString() == "" || newrow[j][7].ToString().IndexOf("-") >= 0)
                    {
                        AQIValue = -1;
                        Value = -1;
                        grade = -1;
                        quality = "-1";
                    }
                    else
                    {
                        AQIValue = int.Parse(newrow[j][7].ToString());
                        Value = Math.Round(double.Parse(newrow[j][6].ToString()) * 1000, 1);
                        grade = int.Parse(newrow[j][8].ToString());
                        quality = newrow[j][9].ToString();
                    }


                    switch (int.Parse(newrow[j][4].ToString()))
                    {
                        case 101:
                            staionNew.setPm25AQI(AQIValue);
                            staionNew.setPm25Value(Value);
                            staionNew.setPm25Grade(grade);
                            staionNew.setpm25Quality(quality);
                            break;
                        case 103:
                            staionNew.setPm10AQI(AQIValue);
                            staionNew.setPm10Value(Value);
                            staionNew.setPm10Grade(grade);
                            staionNew.setPm10Quality(quality);
                            break;
                        case 104:
                            staionNew.setO3AQI(AQIValue);
                            staionNew.setO3Value(Value);
                            staionNew.setO3Grade(grade);
                            staionNew.setO3Quality(quality);
                            break;
                        case 106:
                            staionNew.setSo2AQI(AQIValue);
                            staionNew.setSo2Value(Value);
                            staionNew.setSo2Grade(grade);
                            staionNew.setSo2Quality(quality);
                            break;
                        case 107:
                            staionNew.setNo2AQI(AQIValue);
                            staionNew.setNo2Value(Value);
                            staionNew.setNo2Grade(grade);
                            staionNew.setNo2Quality(quality);
                            break;
                        case 108:
                            staionNew.setCoAQI(AQIValue);
                            if (Value > 0)
                                Value = Value / 1000;
                            staionNew.setCoValue(Value);
                            staionNew.setCoGrade(grade);
                            staionNew.setCoQuality(quality);
                            break;
                        case 100:
                            staionNew.setPrimaryAQI(AQIValue);
                            staionNew.setPrimaryValue(Value);
                            staionNew.setPrimaryPollutantGrade(grade);
                            staionNew.setPrimaryPollutantQuality(quality);
                            staionNew.setPrimaryPollutantType(newrow[j][5].ToString());
                            break;
                    }
                }
                catch { }

            }
            staionNew.updateData();
            return staionNew;
        }

        public IList<Version> getAndroidVersion(string lat, string lng, string IMEI)
        {
            DataTable dt = VersionControl.GetVersionListbyOSName("Android");
            IList<Version> VersionList = new List<Version>();
            ViewLog.AddViewLogAndroid(IMEI, ip, "Warning", "GetVersionListAndroid", double.Parse(lng), double.Parse(lat));
            foreach (DataRow rows in dt.Rows)
            {
                Version versionContent = new Version();
                versionContent.setVersionName(rows[0].ToString());
                versionContent.setVersionNumber(double.Parse(rows[1].ToString()));
                versionContent.setVersionValue(rows[2].ToString());
                VersionList.Add(versionContent);

            }
            return VersionList;

        }
        public IList<Version> getIPhoneVersion(string lat, string lng,string IMEI)
        {
            DataTable dt = VersionControl.GetVersionListbyOSName("iPhone");
            IList<Version> VersionList = new List<Version>();
            ViewLog.AddViewLogAndroid(IMEI, ip, "Warning", "GetVersionListiPhone", double.Parse(lng), double.Parse(lat));
            foreach (DataRow rows in dt.Rows)
            {
                Version versionContent = new Version();
                versionContent.setVersionName(rows[0].ToString());
                versionContent.setVersionNumber(double.Parse(rows[1].ToString()));
                versionContent.setVersionValue(rows[2].ToString());
                VersionList.Add(versionContent);

            }
            return VersionList;

        }
        public IList<Station> AndroidGroupData(string  groupID, string IMEI, string lat, string lng)
        {
            DataTable dt = Site.GetSiteInfobyGroupID(int.Parse(groupID));
            ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GetSiteInfobyGroupID", double.Parse(lng), double.Parse(lat));
            IList<Station> GroupDataList = new List<Station>();
            DataTable siteTable = Site.GetAllAQISites();
            ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));

            int nearSiteID = int.Parse(nearSite(lat, lng, siteTable).Split(',')[0]);
            string siteID = "";
            double distance = 0.0;
            foreach (DataRow rows in dt.Rows)
            {
                Station staionNew = new Station();
                siteID=rows[1].ToString();
                staionNew.setStationID(siteID);
                staionNew.setStationName(rows[2].ToString());
                staionNew.setLatitude(double.Parse(rows[3].ToString()));
                staionNew.setLongitude(double.Parse(rows[4].ToString()));
                DataTable dtData = Data.SiteHourlyAQI(1, siteID);
                ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (int.Parse(siteID) == nearSiteID)
                {
                    staionNew.setNearestStation(true);
                    distance = double.Parse(nearSite(lat, lng, siteTable).Split(',')[1]);
                    staionNew.setNeadestDistance(distance);
                }
                if (dtData.Rows.Count > 0)
                {
                    foreach (DataRow newrow in dtData.Rows)
                    {
                        int AQIValue;
                        double Value;
                        int grade;
                        string quality;
                        if (newrow[7].ToString() == "" || newrow[7].ToString().IndexOf("-") >= 0)
                        {
                            AQIValue = -1;
                            Value = -1;
                            grade = -1;
                            quality = "-1";
                        }
                        else
                        {
                            AQIValue = int.Parse(newrow[7].ToString());
                            Value = Math.Round(double.Parse(newrow[6].ToString()) * 1000, 1);
                            grade = int.Parse(newrow[8].ToString());
                            quality = newrow[9].ToString();
                        }
                        switch (int.Parse(newrow[4].ToString()))
                        {
                            case 101:
                                staionNew.setPm25AQI(AQIValue);
                                staionNew.setPm25Value(Value);
                                staionNew.setPm25Grade(grade);
                                staionNew.setpm25Quality(quality);
                                break;
                            case 103:
                                staionNew.setPm10AQI(AQIValue);
                                staionNew.setPm10Value(Value);
                                staionNew.setPm10Grade(grade);
                                staionNew.setPm10Quality(quality);
                                break;
                            case 104:
                                staionNew.setO3AQI(AQIValue);
                                staionNew.setO3Value(Value);
                                staionNew.setO3Grade(grade);
                                staionNew.setO3Quality(quality);
                                break;
                            case 106:
                                staionNew.setSo2AQI(AQIValue);
                                staionNew.setSo2Value(Value);
                                staionNew.setSo2Grade(grade);
                                staionNew.setSo2Quality(quality);
                                break;
                            case 107:
                                staionNew.setNo2AQI(AQIValue);
                                staionNew.setNo2Value(Value);
                                staionNew.setNo2Grade(grade);
                                staionNew.setNo2Quality(quality);
                                break;
                            case 108:
                                staionNew.setCoAQI(AQIValue);
                                if (Value > 0)
                                    Value = Value / 1000;
                                staionNew.setCoValue(Value);
                                staionNew.setCoGrade(grade);
                                staionNew.setCoQuality(quality);
                                break;
                            case 100:
                                staionNew.setPrimaryAQI(AQIValue);
                                staionNew.setPrimaryValue(Value);
                                staionNew.setPrimaryPollutantGrade(grade);
                                staionNew.setPrimaryPollutantQuality(quality);
                                staionNew.setPrimaryPollutantType(newrow[5].ToString());
                                break;
                        }

                    }
                    staionNew.updateData();
                }
                else
                {
                    staionNew.setIsHasData(false);
                }
                GroupDataList.Add(staionNew);
            }

            
            return GroupDataList;

        }
        public IList<Station> IPhoneGroupData(string groupID, string IMEI, string lat, string lng)
        {
            DataTable dt = Site.GetSiteInfobyGroupID(int.Parse(groupID));
            ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GetSiteInfobyGroupID", double.Parse(lng), double.Parse(lat));
            IList<Station> GroupDataList = new List<Station>();
            DataTable siteTable = Site.GetAllAQISites();
            ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "GetAllAQISites", double.Parse(lng), double.Parse(lat));

            int nearSiteID = int.Parse(nearSite(lat, lng, siteTable).Split(',')[0]);
            string siteID = "";
            double distance = 0.0;
            foreach (DataRow rows in dt.Rows)
            {
                Station staionNew = new Station();
                siteID = rows[1].ToString();
                staionNew.setStationID(siteID);
                staionNew.setStationName(rows[2].ToString());
                staionNew.setLatitude(double.Parse(rows[3].ToString()));
                staionNew.setLongitude(double.Parse(rows[4].ToString()));
                DataTable dtData = Data.SiteHourlyAQI(1, siteID);
                ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (int.Parse(siteID) == nearSiteID)
                {
                    staionNew.setNearestStation(true);
                    distance = double.Parse(nearSite(lat, lng, siteTable).Split(',')[1]);
                    staionNew.setNeadestDistance(distance);
                }
                if (dtData.Rows.Count > 0)
                {
                    foreach (DataRow newrow in dtData.Rows)
                    {
                        int AQIValue;
                        double Value;
                        int grade;
                        string quality;
                        if (newrow[7].ToString() == "" || newrow[7].ToString().IndexOf("-") >= 0)
                        {
                            AQIValue = -1;
                            Value = -1;
                            grade = -1;
                            quality = "-1";
                        }
                        else
                        {
                            AQIValue = int.Parse(newrow[7].ToString());
                            Value = Math.Round(double.Parse(newrow[6].ToString()) * 1000, 1);
                            grade = int.Parse(newrow[8].ToString());
                            quality = newrow[9].ToString();
                        }
                        switch (int.Parse(newrow[4].ToString()))
                        {
                            case 101:
                                staionNew.setPm25AQI(AQIValue);
                                staionNew.setPm25Value(Value);
                                staionNew.setPm25Grade(grade);
                                staionNew.setpm25Quality(quality);
                                break;
                            case 103:
                                staionNew.setPm10AQI(AQIValue);
                                staionNew.setPm10Value(Value);
                                staionNew.setPm10Grade(grade);
                                staionNew.setPm10Quality(quality);
                                break;
                            case 104:
                                staionNew.setO3AQI(AQIValue);
                                staionNew.setO3Value(Value);
                                staionNew.setO3Grade(grade);
                                staionNew.setO3Quality(quality);
                                break;
                            case 106:
                                staionNew.setSo2AQI(AQIValue);
                                staionNew.setSo2Value(Value);
                                staionNew.setSo2Grade(grade);
                                staionNew.setSo2Quality(quality);
                                break;
                            case 107:
                                staionNew.setNo2AQI(AQIValue);
                                staionNew.setNo2Value(Value);
                                staionNew.setNo2Grade(grade);
                                staionNew.setNo2Quality(quality);
                                break;
                            case 108:
                                staionNew.setCoAQI(AQIValue);
                                if (Value > 0)
                                    Value = Value / 1000;
                                staionNew.setCoValue(Value);
                                staionNew.setCoGrade(grade);
                                staionNew.setCoQuality(quality);
                                break;
                            case 100:
                                staionNew.setPrimaryAQI(AQIValue);
                                staionNew.setPrimaryValue(Value);
                                staionNew.setPrimaryPollutantGrade(grade);
                                staionNew.setPrimaryPollutantQuality(quality);
                                staionNew.setPrimaryPollutantType(newrow[5].ToString());
                                break;
                        }

                    }
                    staionNew.updateData();
                }
                else
                {
                    staionNew.setIsHasData(false);
                }
                GroupDataList.Add(staionNew);
            }


            return GroupDataList;

        }
        public string AndroidinsertIntoVersion(string IMEI, string lat, string lng, string phoneNumer, string OSVersion, string SoftVersion)
        {
            int result = Phone.PhoneAndroid_AddNew(IMEI, phoneNumer, OSVersion, double.Parse(SoftVersion));
            ViewLog.AddViewLogAndroid(IMEI, ip, "Warning", "PhoneAndroid_AddNew", double.Parse(lng), double.Parse(lat));
            return result.ToString();
        }
        public string IphoneinsertIntoVersion(string IMEI, string lat, string lng, string phoneNumer, string OSVersion, string SoftVersion)
        {
            int result = Phone.PhoneIPhone_AddNew(IMEI, phoneNumer, OSVersion, double.Parse(SoftVersion));
            ViewLog.AddViewLogiPhone(IMEI, ip, "Warning", "PhoneAndroid_AddNew", double.Parse(lng), double.Parse(lat));
            return result.ToString();
        }
        public IList<ForecastData> IphoneForecastData(string IMEI, string lat, string lng, string groupID)
        {
            IList<ForecastData> ForecastDataList = new List<ForecastData>();
            ForecastData forecastValue = new ForecastData();
            DateTime now = DateTime.Now;
            string nowTime = now.ToString("yyyy年M月d日");
            string strSQL = "SELECT * FROM semc_air.DBO.tb_AirForecast";
            DataTable dt = m_Database.GetDataTable(strSQL);
            forecastValue.setGroupID(int.Parse(groupID));
            forecastValue.setLST(dt.Rows[0]["foreDate"].ToString());
            forecastValue.setSeg1(dt.Rows[0]["Seg1"].ToString());
            forecastValue.setSeg2(dt.Rows[0]["Seg2"].ToString());
            forecastValue.setSeg3(dt.Rows[0]["Seg3"].ToString());


            forecastValue.setAQI3(dt.Rows[0]["AQI3"].ToString());


            forecastValue.setGrade3(dt.Rows[0]["Grade3"].ToString());


            forecastValue.setParam3(dt.Rows[0]["Param3"].ToString());
            forecastValue.setDetail(dt.Rows[0]["Detail"].ToString());

            if (now.Hour >= 6 && now.Hour < 12)
            {
                forecastValue.setAQI2(dt.Rows[0]["AQI2"].ToString());
                forecastValue.setGrade2(dt.Rows[0]["Grade2"].ToString());
                forecastValue.setParam2(dt.Rows[0]["Param2"].ToString());
            }
            else
            {
                string forecast = dt.Rows[0]["foreDate"].ToString();
                if (forecast == nowTime || now.Hour<6)
                {
                    forecastValue.setAQI1(dt.Rows[0]["AQI1"].ToString());
                    forecastValue.setAQI2(dt.Rows[0]["AQI2"].ToString());
                    forecastValue.setGrade1(dt.Rows[0]["Grade1"].ToString());
                    forecastValue.setGrade2(dt.Rows[0]["Grade2"].ToString());
                    forecastValue.setParam1(dt.Rows[0]["Param1"].ToString());
                    forecastValue.setParam2(dt.Rows[0]["Param2"].ToString());
                }
                else
                {
                }

            }
            dt = Data.GroupHourlyAQI(1, "102", "100");
            ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
            int AQI = int.Parse(dt.Rows[0][7].ToString());
            AQIExtention aqiExt = new AQIExtention(AQI, 0);
            forecastValue.setHeath(aqiExt.Health);
            forecastValue.setAdjusted(aqiExt.Protection);
            ForecastDataList.Add(forecastValue);
            return ForecastDataList;
        }
        public IList<ForecastData> AndroidForecastData(string IMEI, string lat, string lng, string groupID)
        {
            IList<ForecastData> ForecastDataList = new List<ForecastData>();
            ForecastData forecastValue = new ForecastData();
            DateTime now = DateTime.Now;
            string nowTime = now.ToString("yyyy年M月d日");
            string strSQL = "SELECT * FROM semc_air.DBO.tb_AirForecast";
            //string strSQL = "SELECT * FROM Rainf.DBO.tb_AirForecast";
            DataTable dt = m_Database.GetDataTable(strSQL);
            forecastValue.setGroupID(int.Parse(groupID));
            forecastValue.setLST(dt.Rows[0]["foreDate"].ToString());
            forecastValue.setSeg1(dt.Rows[0]["Seg1"].ToString());
            forecastValue.setSeg2(dt.Rows[0]["Seg2"].ToString());
            forecastValue.setSeg3(dt.Rows[0]["Seg3"].ToString());


            forecastValue.setAQI3(dt.Rows[0]["AQI3"].ToString());


            forecastValue.setGrade3(dt.Rows[0]["Grade3"].ToString());


            forecastValue.setParam3(dt.Rows[0]["Param3"].ToString());
            forecastValue.setDetail(dt.Rows[0]["Detail"].ToString());

            if (now.Hour >= 6 && now.Hour < 12)
            {
                forecastValue.setAQI2(dt.Rows[0]["AQI2"].ToString());
                forecastValue.setGrade2(dt.Rows[0]["Grade2"].ToString());
                forecastValue.setParam2(dt.Rows[0]["Param2"].ToString());
            }
            else
            {
                string forecast = dt.Rows[0]["foreDate"].ToString();
                if (forecast == nowTime || now.Hour < 6)
                {
                    forecastValue.setAQI1(dt.Rows[0]["AQI1"].ToString());
                    forecastValue.setAQI2(dt.Rows[0]["AQI2"].ToString());
                    forecastValue.setGrade1(dt.Rows[0]["Grade1"].ToString());
                    forecastValue.setGrade2(dt.Rows[0]["Grade2"].ToString());
                    forecastValue.setParam1(dt.Rows[0]["Param1"].ToString());
                    forecastValue.setParam2(dt.Rows[0]["Param2"].ToString());
                }
                else
                {
                }

            }
            dt = Data.GroupHourlyAQI(1, "102", "100");
            ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
            int AQI = int.Parse(dt.Rows[0][7].ToString());
            AQIExtention aqiExt = new AQIExtention(AQI, 0);
            forecastValue.setHeath(aqiExt.Health);
            forecastValue.setAdjusted(aqiExt.Protection);
            ForecastDataList.Add(forecastValue);
            return ForecastDataList;
        }
        public DataSet IphoneChart(string IMEI, string lat, string lng, string ID, string flag)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (flag == "1")//1是区号0是站点号
            {
                dt = Data.GroupHourlyAQI(24, ID, "100");//实时空气质量指数
                ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt,"TableRT"));
                }
                dt = Data.GroupHourlyAQI(24, ID, "120");
                ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "Table24"));
                }
                dt = Data.GroupDailyAQI(30, ID, "300");
                ViewLog.AddViewLogiPhone(IMEI, ip, "GroupData", "GroupDailyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "Table30"));
                }
            }
            else
            {
                dt = Data.SiteHourlyAQI(24, ID, "100");//实时空气质量指数
                ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "TableRT"));
                }
                dt = Data.SiteDailyAQI(30, ID, "300");
                ViewLog.AddViewLogiPhone(IMEI, ip, "SiteData", "SiteDailyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "Table30"));
                }
            }
            return ds;
        }
        public DataSet AndroidChart(string IMEI, string lat, string lng, string ID,string flag)
        {
            DataSet ds=new DataSet();
            DataTable dt=new DataTable();
            if (flag == "1")//1是区号0是站点号
            {
                dt = Data.GroupHourlyAQI(24, ID, "100");//实时空气质量指数
                ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt,"TableRT"));
                }
                dt = Data.GroupHourlyAQI(24,ID,"120");
                ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt,"Table24"));
                }
                dt = Data.GroupDailyAQI(30, ID, "300");
                ViewLog.AddViewLogAndroid(IMEI, ip, "GroupData", "GroupDailyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "Table30"));
                }
            }
            else 
            {
                dt = Data.SiteHourlyAQI(24, ID, "100");//实时空气质量指数
                ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "SiteHourlyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt,"TableRT"));
                }
                dt = Data.SiteDailyAQI(30, ID, "300");
                ViewLog.AddViewLogAndroid(IMEI, ip, "SiteData", "SiteDailyAQI", double.Parse(lng), double.Parse(lat));
                if (dt.Rows.Count > 0)
                {
                    ds.Tables.Add(newDataTable(dt, "Table30"));
                }
            }
            return ds;
        }
        public DataTable newDataTable(DataTable dt, string tableName)
        {
            DataTable dm = new DataTable();
            dm.Columns.Add("LST_AQI", typeof(string));
            dm.Columns.Add("AQI", typeof(int));
            dm.TableName = tableName;
            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = dm.NewRow();
                if (tableName == "Table30")
                {
                    newRow[0] = DateTime.Parse(row[0].ToString()).ToString("yyyy/MM/dd HH:00:00");
                    newRow[1] = int.Parse(row[6].ToString());
                }
                else
                {
                    newRow[0] = DateTime.Parse(row[1].ToString()).ToString("yyyy/MM/dd HH:00:00");
                    newRow[1] = int.Parse(row[7].ToString());
                }
                dm.Rows.Add(newRow);  // 将DataRow添加到DataTable中 
            }
            return dm;
        }

    }
}
