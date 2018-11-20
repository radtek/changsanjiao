using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL
{
    public class AQIPeriodCell
    {
        public AQIPeriodCell()
        {

        }

        

        //时间间隔
        public string Period
        {
            get;
            set;
        }

        //发布组织
        public string Organization
        {
            get;
            set;
        }
        //时段
        public string Duration
        {
            get;
            set;
        }
        //AQI的ID
        public string AQIId
        {
            get;
            set;
        }
        //LST
        public string LST
        {
            get;
            set;
        }
        //ForecastDate
        public string ForecastDate
        {
            get;
            set;
        }
        //检测值
        public string Value
        {
            get;
            set;
        }
        //AQI数值
        public string AQI
        {
            get;
            set;
        }

        //Parameter,表示首要污染物
        public string Parameter
        {
            get;
            set;
        }
    }
}
