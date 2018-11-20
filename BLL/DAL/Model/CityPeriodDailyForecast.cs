using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 分时段预报
    /// </summary>
    public class CityPeriodDailyForecast
    {
        private int id;
        /// <summary>
        /// 序号
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private DateTime forecastdate;
        /// <summary>
        /// 分段预报时
        /// </summary>
        public DateTime ForecastDate
        {
            get { return forecastdate; }
            set { forecastdate = value; }
        }
        private int afternoonapi;
        /// <summary>
        /// 今日下午预报API
        /// </summary>
        public int AfternoonAPI
        {
            get { return afternoonapi; }
            set { afternoonapi = value; }
        }
        private string afternoonapirange;
        /// <summary>
        /// 今日下午预报API范围
        /// </summary>
        public string AfternoonAPIRange
        {
            get { return afternoonapirange; }
            set { afternoonapirange = value; }
        }
        private string afternoongraderange;

        /// <summary>
        /// 今日下午预报API等级
        /// </summary>
        public string AfternoonGradeRange
        {
            get { return afternoongraderange; }
            set { afternoongraderange = value; }
        }
        private int tonightapi;
        /// <summary>
        /// 今晚预报API
        /// </summary>
        public int TonightAPI
        {
            get { return tonightapi; }
            set { tonightapi = value; }
        }
        private string tonightapirange;
        /// <summary>
        /// 今晚预报API范围
        /// </summary>
        public string TonightAPIRange
        {
            get { return tonightapirange; }
            set { tonightapirange = value; }
        }
        private string tonightgraderange;
        /// <summary>
        /// 今晚预报API等级
        /// </summary>
        public string TonightGradeRange
        {
            get { return tonightgraderange; }
            set { tonightgraderange = value; }
        }
        private int tomorrowamapi;
        /// <summary>
        /// 明天上午预报API
        /// </summary>
        public int TomorrowAMAPI
        {
            get { return tomorrowamapi; }
            set { tomorrowamapi = value; }
        }
        private string tomorrowamapirange;
        /// <summary>
        /// 明天上午预报API范围
        /// </summary>
        public string TomorrowAMAPIRange
        {
            get { return tomorrowamapirange; }
            set { tomorrowamapirange = value; }
        }
        private string tomorrowamgraderange;
        /// <summary>
        /// 明天上午预报API等级
        /// </summary>
        public string TomorrowAMGradeRange
        {
            get { return tomorrowamgraderange; }
            set { tomorrowamgraderange = value; }
        }
        private int tomorrowamapimodify;
        /// <summary>
        /// 明天上午预报修正API
        /// </summary>
        public int TomorrowAMAPIModify
        {
            get { return tomorrowamapimodify; }
            set { tomorrowamapimodify = value; }
        }
        private string tomorrowamapimodifyrange;
        /// <summary>
        /// 明天上午预报修正API范围
        /// </summary>
        public string TomorrowAMAPIModifyRange
        {
            get { return tomorrowamapimodifyrange; }
            set { tomorrowamapimodifyrange = value; }
        }
        private string tomorrowamgrademodifyrange;
        /// <summary>
        /// 明天上午预报修正API等级
        /// </summary>
        public string TomorrowAMGradeModifyRange
        {
            get { return tomorrowamgrademodifyrange; }
            set { tomorrowamgrademodifyrange = value; }
        }
        private int tomorrowpmapi;
        /// <summary>
        /// 明天下午预报API
        /// </summary>
        public int TomorrowPMAPI
        {
            get { return tomorrowpmapi; }
            set { tomorrowpmapi = value; }
        }
        private string tomorrowpmapirange;
        /// <summary>
        /// 明天下午预报API范围
        /// </summary>
        public string TomorrowPMAPIRange
        {
            get { return tomorrowpmapirange; }
            set { tomorrowpmapirange = value; }
        }
        private string tomorrowpmgraderange;
        /// <summary>
        /// 明天下午预报API等级
        /// </summary>
        public string TomorrowPMGradeRange
        {
            get { return tomorrowpmgraderange; }
            set { tomorrowpmgraderange = value; }
        }
        private int tomorrowpmapimodify;
        /// <summary>
        /// 明天下午预报修正API
        /// </summary>
        public int TomorrowPMAPIModify
        {
            get { return tomorrowpmapimodify; }
            set { tomorrowpmapimodify = value; }
        }
        private string tomorrowpmapimodifyrange;
        /// <summary>
        /// 明天下午预报修正API范围
        /// </summary>
        public string TomorrowPMAPIModifyRange
        {
            get { return tomorrowpmapimodifyrange; }
            set { tomorrowpmapimodifyrange = value; }
        }
        private string tomorrowpmgrademodifyrange;
        /// <summary>
        /// 明天下午预报修正API等级
        /// </summary>
        public string TomorrowPMGradeModifyRange
        {
            get { return tomorrowpmgrademodifyrange; }
            set { tomorrowpmgrademodifyrange = value; }
        }
        private string lastupdateperson;
        /// <summary>
        /// 最后预报人
        /// </summary>
        public string LastUpdatePerson
        {
            get { return lastupdateperson; }
            set { lastupdateperson = value; }
        }
        private DateTime lastupdatetime;
        /// <summary>
        /// 最后预报时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return lastupdatetime; }
            set { lastupdatetime = value; }
        }
        private string resultforecast;
        /// <summary>
        /// 分时段气象及污染分析（预报）
        /// </summary>
        public string ResultForecast
        {
            get { return resultforecast; }
            set { resultforecast = value; }
        }
        private string resultfact;
        /// <summary>
        /// 分时段气象及污染分析（实测）
        /// </summary>
        public string ResultFact
        {
            get { return resultfact; }
            set { resultfact = value; }
        }
    }
}
