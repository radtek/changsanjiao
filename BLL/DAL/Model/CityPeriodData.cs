using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public class CityPeriodData
    {
        private int id;
        private DateTime dailyDate;
        private int api;
        private DateTime lastModifyTime;
        private string lastModifyPersion;
        private bool fullData = false;
        private Dictionary<int, PeriodData> periodData;

        public CityPeriodData()
        {
            periodData = new Dictionary<int, PeriodData>();
        }

        //序号
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        //分段日报时间
        public DateTime DailyDate
        {
            get { return dailyDate; }
            set { dailyDate = value; }
        }
        //分段API
        public int API
        {
            get { return api; }
            set { api = value; }
        }
        //分段修改时间
        public DateTime LastModifyTime
        {
            get { return lastModifyTime; }
            set { lastModifyTime = value; }
        }
        //分段修改人
        public string LastModifyPersion
        {
            get { return lastModifyPersion; }
            set { lastModifyPersion = value; }
        }
        //分段日报数据是否完整
        public bool FullData
        {
            get { return fullData; }
            set { fullData = value; }
        }
        //分段数据
        public Dictionary<int,PeriodData> PeriodDataDic
        {
            get { return periodData; }
            set { periodData = value; }
        }
    }
}
