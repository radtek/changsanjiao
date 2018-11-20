using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{

    /// <summary>
    /// 表示一段时间范围或是某一天
    /// </summary>
    public class PeriodOfTime
    {
        private DateTime start;
        private DateTime end;
        private DateTime date;
        private DateTime value;

        /// <summary>
        /// 一段时间的起点
        /// </summary>
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                this.start = value;
            }
        }

        /// <summary>
        /// 一段时间的终点
        /// </summary>
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                this.end = value;
            }
        }

        public DateTime Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// 截断时间
        /// </summary>
        /// <param name="holdElement"></param>
        /// <returns></returns>
        public DateTime TruncationValue(DateElement holdElement)
        {
            string formatString = "yyyy-MM-dd hh:mm:ss";
            switch (holdElement)
            {
                case DateElement.Minute: formatString = "yyyy-MM-dd hh:mm:00"; break;
                case DateElement.Hour: formatString = "yyyy-MM-dd hh:00:00"; break;
                case DateElement.Day: formatString = "yyyy-MM-dd 00:00:00"; break;
                case DateElement.Month: formatString = "yyyy-MM"; break;
                case DateElement.Year: formatString = "yyyy-1"; break;
            }
            return DateTime.Parse(date.ToString(formatString));
        }
    }
}
