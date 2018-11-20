using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public class PeriodData : DataCollectionBase
    {
        public PeriodData()
        {

        }

        private int periodCode;
        //时段代码
        public int PeriodCode
        {
            get { return periodCode; }
            set { periodCode = value; }
        }
        private string periodName;
        //时段名称
        public string PeriodName
        {
            get { return periodName; }
            set { periodName = value; }
        }

        private string periodDate;
        //时段具体时间
        public string PeriodDate
        {
            get { return periodDate; }
            set { periodDate = value; }
        }
        private bool fullData;
        //时段数据是否完整
        public bool FullData
        {
            get { return fullData; }
            set { fullData = value; }
        }
    }
}
