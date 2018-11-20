using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using MMShareBLL.Model;
using MMShareBLL.DAL;



namespace MMShareBLL.BLL
{
    /// <summary>
    /// 对站点数据的查询(加工后的数据)
    /// </summary>
    public class SiteDataManager
    {
        private DailyDataAccess dal = new DailyDataAccess();

        /// <summary>
        /// 获得某个期间站点的均值数据
        /// </summary>
        /// <returns></returns>
        public List< MMShareBLL.Model.SiteData> QuerySiteData(DateTime day, Duration duration, bool fillSiteData)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                List<MMShareBLL.Model.SiteData> dataList = dal.QuerySiteData(day, duration);
                if (fillSiteData)
                {
                    Area area = new Area();
                    foreach (MMShareBLL.Model.SiteData sd in dataList)
                    {
                        sd.Site = area.GetSite(sd.Site.Id, true);
                        if (null == sd.Site)
                            sd.Site = new Site();
                    }
                }
                return dataList;
            }
            catch { throw; }
        }

        public Dictionary<int, MMShareBLL.Model.SiteData> QuerySiteDataDictdata(DateTime day, Duration duration)
        {
            try
            {
                List<MMShareBLL.Model.SiteData> dataList = QuerySiteData(day, duration, true);
                Dictionary<int, MMShareBLL.Model.SiteData> dataContainer = new Dictionary<int, MMShareBLL.Model.SiteData>();
                foreach (MMShareBLL.Model.SiteData sd in dataList)
                {
                    if (null != sd.Site)
                        dataContainer[sd.Site.Id] = sd;
                }
                return dataContainer;
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime day, Duration duration)
        {
            try
            {
                day = DataCalculate.ReParseDateTime(day, duration, false);
                return dal.QuerySiteDataTable(day, duration);
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime d1, DateTime d2, int siteId, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.QuerySiteDataTable(d1, d2, siteId, duration);
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime d1, DateTime d2, int siteId, Duration duration, bool joinParameters)
        {
            try
            {
                DataTable t = GetData(d1, d2, siteId, duration);
                if (joinParameters)
                {
                    return spTalbe(t);
                }
                return t;
            }
            catch { throw; }
        }

        public DataTable GetData(DateTime day, Duration duration, bool joinParameters)
        {
            try
            {
                DataTable t = GetData(day, duration);
                if (joinParameters)
                {
                    return spTalbe(t);
                }
                return t;
            }
            catch { throw; }
        }

        public Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> GetData(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.GetSiteData(d1, d2, siteList, duration);
            }
            catch { throw; }
        }

        public Dictionary<int, MMShareBLL.Model.SiteData> GetAVGData(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.GetSiteAVGData(d1, d2, siteList, duration);
            }
            catch { throw; }
        }

        public Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> GetDataDic(DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.GetSiteDataDic(d1, d2, siteList, paramters, duration);
            }
            catch { throw; }
        }

        /// <summary>
        /// 返回的DataTable包括的列有：ID,SiteID,Duration,ParameterID,Value,API,LST,CreateTime
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="siteList"></param>
        /// <param name="paramters"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public DataTable GetData(DateTime d1, DateTime d2, int[] siteList, int[] paramters, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                return dal.GetSiteData(d1, d2, siteList,paramters, duration);
            }
            catch { throw; }
        }

        public List<DataParameter> GetSiteDataParameters(DateTime d1, DateTime d2, int[] siteList, Duration duration)
        {
            try
            {
                d1 = DataCalculate.ReParseDateTime(d1, duration, false);
                d2 = DataCalculate.ReParseDateTime(d2, duration, true);
                List<int> dataList = dal.GetSiteDataParameters(d1, d2, siteList, duration);
                ParameterManager pm = new ParameterManager();
                return pm.GetParameters(dataList);
            }
            catch { throw; }
        }

        public MMShareBLL.Model.SiteData GetData(DateTime time, int siteid, int[] paramters, Duration duration)
        {
            try
            {
                return dal.QuerySiteData(time, siteid, paramters, duration);
            }
            catch { throw; }
        }

        private DataTable spTalbe(DataTable t)
        {
            DataTable table = new DataTable();
            table.Columns.Add("sid");
            table.Columns.Add("时间");
            table.Columns.Add("站点");
            if (t.Rows.Count == 0)
            {
                table.Columns.Add("监测因子");
                table.Columns.Add("数值");
                table.Columns.RemoveAt(0);
                return table;
            }
            foreach (DataRow row in t.Rows)
            {
                object paramId = row[3];
                if (notExistsColumn(paramId, table))
                    table.Columns.Add(paramId.ToString());
                string time = Convert.ToDateTime(row[6]).ToString("yyyy-MM-dd");
                DataRow newRow = findRow(row[1].ToString(), time, table);
                float value = float.Parse(row[4].ToString());
                newRow[paramId.ToString()] = Math.Round(value, 3);
                //newRow[paramId.ToString()] = Math.Round(value, 3) + "/" + row[5];
            }
            ParameterManager pm = new ParameterManager();
            for (int i = 3; i < table.Columns.Count; i++)
            {
                DataColumn column = table.Columns[i];
                DataParameter param = pm.GetParameter(int.Parse(column.ColumnName), true);
                if (null != param)
                    column.ColumnName = HttpContext.Current.Server.HtmlEncode(param.Name);
            }
            table.Columns.RemoveAt(0);
            return table;
        }

        private DataRow findRow(string siteId, string time, DataTable dataSource)
        {
            foreach (DataRow row in dataSource.Rows)
            {
                if (row[0].ToString() == siteId && row[1].ToString() == time)
                    return row;
            }
            DataRow newRow = dataSource.NewRow();
            newRow[0] = siteId;
            Area area = new Area();
            Site site = area.GetSite(int.Parse(siteId), true);
            if (null != site)
                newRow[2] = HttpContext.Current.Server.HtmlEncode(site.Name);
            newRow[1] = time;
            dataSource.Rows.Add(newRow);
            return newRow;
        }

        private bool notExistsColumn(object paramId, DataTable dataSource)
        {
            foreach (DataColumn cl in dataSource.Columns)
            {
                if (cl.ColumnName == paramId.ToString())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获得各国控点在某一天在全市数据中的百分比
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<int, decimal>> GetDayValueRate(DateTime day, ref MMShareBLL.Model.SiteData referencesSitedata, Dictionary<int, Site> sites, DataValidated validated)
        {
            try
            {
                Dictionary<int, decimal> paramSum = new Dictionary<int, decimal>();
                int[] sitesArray = new int[sites.Count];
                sites.Keys.CopyTo(sitesArray, 0);
                Dictionary<int, Dictionary<DateTime, MMShareBLL.Model.SiteData>> dataSource = GetData(day, day, sitesArray, Duration.Day);
                SiteGroupManager sgm = new SiteGroupManager();

                Area ar = new Area();
                Dictionary<int, MMShareBLL.Model.SiteData> stateSitesData = new Dictionary<int, MMShareBLL.Model.SiteData>();
                foreach (int siteid in dataSource.Keys)
                {
                    Dictionary<DateTime, MMShareBLL.Model.SiteData> ts = dataSource[siteid];
                    foreach (MMShareBLL.Model.SiteData sd in ts.Values)
                    {
                        sd.Site = ar.GetSite(siteid, true);
                        stateSitesData[siteid] = sd;
                    }
                }

                foreach (MMShareBLL.Model.SiteData sd in stateSitesData.Values)
                {
                    if (sd.Site.SiteType == SiteType.Reference)
                    {
                        referencesSitedata = sd;
                        continue;
                    }
                    foreach (MonitoringData data in sd.DataCollection)
                    {
                        decimal sum = 0;
                        if (paramSum.ContainsKey(data.Factor.Id))
                            sum = paramSum[data.Factor.Id];
                        sum += (Decimal)data.Thickness;
                        paramSum[data.Factor.Id] = sum;
                    }
                }

                Dictionary<int, Dictionary<int, decimal>> theData = new Dictionary<int, Dictionary<int, decimal>>();
                foreach (MMShareBLL.Model.SiteData sd in stateSitesData.Values)
                {
                    if (sd.Site.SiteType == SiteType.Reference)
                        continue;
                    Dictionary<int, decimal> theSiteRate = new Dictionary<int, decimal>();
                    foreach (MonitoringData data in sd.DataCollection)
                    {
                        theSiteRate[data.Factor.Id] = Math.Round((decimal)data.Thickness / paramSum[data.Factor.Id], 3);
                    }
                    theData[sd.Site.Id] = theSiteRate;
                }
                return theData;
            }
            catch { throw; }
        }
    }
}
