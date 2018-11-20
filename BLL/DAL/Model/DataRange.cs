using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 监测数据的数值范围
    /// </summary>
    public class DataRange
    {
        decimal min = 0m;
        decimal max = 0.02m;

        public DataRange() { }

        public DataRange(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Default:0
        /// </summary>
        public decimal Min { get { return min; } set { min = value; } }

        /// <summary>
        /// Default:0.02
        /// </decimal>
        public decimal Max { get { return max; } set { max = value; } }
    }
}
