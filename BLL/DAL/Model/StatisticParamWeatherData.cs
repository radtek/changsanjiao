using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 统计图形"点位气象小时变化图"的参数配置字典实体
    /// </summary>
    public class StatisticParamWeatherData
    {
        private int code;
        private DataParameter factor;
        private string color;
        private string seriesType;
        private bool showZeroValue = false;


        /// <summary>
        /// 统计代码
        /// </summary>
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 统计图片所包含的参数列表
        /// </summary>
        public DataParameter OwnerFactor
        {
            get { return factor; }
            set { factor = value; }
        }

        /// <summary>
        /// 可选属性，该属性定义了一个颜色值
        /// </summary>
        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// 当值为0时是否显示该统计图
        /// </summary>
        public bool ShowZeroValue
        {
            get { return showZeroValue; }
            set { showZeroValue = value; }
        }

        /// <summary>
        /// 可选属性，指定该参数所生成的图形，默认为Column。正确取值有：Marker、Spline、Line、AreaLine、Column、Cylinder、Bar、Bubble、AreaSpline
        /// </summary>
        public string SeriesType
        {
            get { return seriesType; }
            set { seriesType = value; }
        }
    }
}
