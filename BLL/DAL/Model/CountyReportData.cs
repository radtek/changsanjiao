using System;
using System.Collections.Generic;


namespace MMShareBLL.Model
{
    /// <summary>
    /// 分区日报数据
    /// </summary>
    public class CountyReportData
    {
        private int id;
        private DateTime day;
        private DailyState status;
        private string lastModifyPersion;
        private DateTime lastModifyTime;
        private List<GroupData> data;
        private bool isAuto = false;

        public CountyReportData()
        {
            data = new List<GroupData>();
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置分区日报的日期
        /// </summary>
        public DateTime Day
        {
            get { return day; }
            set { day = value; }
        }

        /// <summary>
        /// 获取或设置分区日报的状态
        /// </summary>
        public DailyState Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 获取或设置分区日报最后的修改人
        /// </summary>
        public string LastModifyPersion
        {
            get { return lastModifyPersion; }
            set { lastModifyPersion = value; }
        }

        /// <summary>
        /// 获取或设置分区日报最后的修改时间
        /// </summary>
        public DateTime LastModifyTime
        {
            get { return lastModifyTime; }
            set { lastModifyTime = value; }
        }

        /// <summary>
        ///  获取或设置分区日报的数据列表
        /// </summary>
        public List<GroupData> Data
        {
            get { return data; }
        }

        /// <summary>
        /// 指示是否由系统自动生成
        /// </summary>
        public bool IsAuto
        {
            get { return isAuto; }
            set { isAuto = value; }
        }

        public GroupData FindData(string groupNum)
        {
            foreach (GroupData gd in data)
            {
                if (gd.Group.GroupID == groupNum)
                    return gd;
            }
            return null;
        }
    }
}
