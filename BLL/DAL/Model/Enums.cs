using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 监测站点的类型
    /// </summary>
    public enum SiteType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 表示一般的站点
        /// </summary>
        General = 1,
        /// <summary>
        /// 国控点
        /// </summary>
        National = 2,
        /// <summary>
        /// 参考点,该点在不参与全市平均的计算
        /// </summary>
        Reference = 3,
        /// <summary>
        /// 既是国控点也是参考点,该点在不参与全市平均的计算
        /// </summary>
        National_Reference = 4

    }

    /// <summary>
    /// 验证数据状态
    /// </summary>
    public enum DataValidated
    {
        Vaidated,//有效
        Invalidated,//无效
        All,
        NoSet,
    }

    /// <summary>
    /// 污染因子单位的处理方式
    /// </summary>
    public enum UnitParserType
    {
        /// <summary>
        /// 不做转换
        /// </summary>
        NoConvert,

        /// <summary>
        /// 从ppb转换到mg/m3
        /// </summary>
        PPB,

        /// <summary>
        /// 从ug/m3转换到mg/m3
        /// </summary>
        UG,

        /// <summary>
        /// 从℃转换到K
        /// </summary>
        Centigrade
    }


    /// <summary>
    /// 指示污染因子在集合中的取值方式
    /// </summary>
    public enum ParameterCollectionType
    {
        /// <summary>
        /// 取平均值
        /// </summary>
        Avg,
        /// <summary>
        /// 取最大值
        /// </summary>
        Max,
    }

    /// <summary>
    /// 组成时间的某一元素
    /// </summary>
    public enum DateElement
    {
        /// <summary>
        /// 秒
        /// </summary>
        Second,
        /// <summary>
        /// 分
        /// </summary>
        Minute,
        /// <summary>
        /// 时
        /// </summary>
        Hour,
        /// <summary>
        /// 天
        /// </summary>
        Day,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }

    /// <summary>
    /// 数值预报的模式
    /// </summary>
    public enum DataMode
    {
        NAQPMS2004 = -1,
        NAQPMS2008 = 1,
        CMAQ4_4 = 2,
        CMAQ4_6 = 3,
        CAMx = 4,
        WRF_Chem = 5,
        MM5 = 6,
        WRF = 7,
        CMAQ = 8,
        None = 0
    }

    /// <summary>
    /// 上报的数据源
    /// </summary>
    public enum ReportDataSource
    {
        Daily_StateSiteDailyData_Dbf = 0,
        Daily_StateSiteHourData_Dbf = 1,
        Daily_RB_Excel = 2,
        Daily_Excel2 = 3,
        Forecast_Dbf = 4,
        Forecast_Result = 5,
        Forecast_Note = 6,
        Daily_County = 7,
        Daily_CiwywideHourData_Dbf = 8,
        Daily_Ozone_Dbf = 9,
        Daily_Publish = 10,
        Forecast_Publish = 11,
        Daily_RB_Txt = 12,
        Daily_Changshanjiao_r = 13,
        Daily_Changshanjiao_f = 14,
        Daily_Note = 15,
        Daily_Ozone_Email = 16,
        Daily_Inc = 17,
        Forecast_Inc = 18,
        WeekEvaluate_Email = 19,
        WeekEvaluate_Fax = 20,
        HazeDayReportWordDic = 21,
        Note = 22,
        Daily_LocomotiveTV_Email = 23,
        Daily_LocomotiveTV_Fax = 24,
        Period_LocomotiveTV_Email = 25,
        Period_LocomotiveTV_Fax = 26,
        Period_LocomotiveLiveTV_Email = 27,
        Period_RTDAILYTV_Email = 28,
        Period_RTDAILYTV_Fax = 29,
        MobileNewspaper = 30,
        AQI_Tvstation = 31,
        AQIWeekReport_Email = 32,
        Null = -1
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        Dbf,
        Excel,
        Txt,
        Other,
        Word
    }

    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// 实测数据
        /// </summary>
        Fact,
        /// <summary>
        /// 数值预报
        /// </summary>
        Forecast,
        /// <summary>
        /// 综合预报
        /// </summary>
        ColligationForecast,
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet
    }

    /// <summary>
    /// 上报重试任务的状态
    /// </summary>
    public enum ReportTaskState
    {
        /// <summary>
        /// 正在执行
        /// </summary>
        Running,

        /// <summary>
        /// 成功
        /// </summary>
        Successed,

        /// <summary>
        /// 挂起
        /// </summary>
        WaitSleep,

        /// <summary>
        /// 没有成功完成
        /// </summary>
        Stoped,

        /// <summary>
        /// 正在传送
        /// </summary>
        Sending,

        /// <summary>
        /// 过期
        /// </summary>
        Overdue,

        /// <summary>
        /// 等待队列
        /// </summary>
        Waiting,

        /// <summary>
        /// 正在处理
        /// </summary>
        Executeing,

        /// <summary>
        /// 传送过程中出现错误
        /// </summary>
        Error,

        /// <summary>
        /// 未设置
        /// </summary>,
        NoSet
    }

    /// <summary>
    /// 日报状态
    /// </summary>
    public enum DailyState
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 审核过的
        /// </summary>
        AuditOff = 1,
        AuditOffByOther = 2,
        /// <summary>
        /// 未经审核
        /// </summary>
        Unaudited = 3,
        /// <summary>
        /// 正在发送
        /// </summary>
        Sedding = 4,
        /// <summary>
        /// 已发送
        /// </summary>
        Sended = 5
    }

    /// <summary>
    /// 监测数据的均值期间
    /// </summary>
    public enum Duration
    {
        /// <summary>
        /// 小时数据
        /// </summary>
        Hour = 0,
        /// <summary>
        /// 日均值
        /// </summary>
        Day = 1,
        /// <summary>
        /// 月均值
        /// </summary>
        Month = 2,
        /// <summary>
        /// 季度均值
        /// </summary>
        Quarter = 3,
        /// <summary>
        /// 年均值
        /// </summary>
        Year = 4,
        /// <summary>
        /// 半年均值
        /// </summary>
        HalfYear = 5,
        /// <summary>
        /// 无效
        /// </summary>
        None
    }

    /// <summary>
    /// 监测数据统计指标
    /// </summary>
    public enum StatIndicators
    {
        /// <summary>
        /// 最大值
        /// </summary>
        Max,
        /// <summary>
        /// 最小值
        /// </summary>
        Min,
        /// <summary>
        /// 平均值
        /// </summary>
        Avg,
        /// <summary>
        /// 中位数
        /// </summary>
        Median,
        /// <summary>
        /// 样本数据
        /// </summary>
        SampleCount,
        /// <summary>
        /// 污染浓度
        /// </summary>
        Thickness,
        /// <summary>
        /// 有效数
        /// </summary>
        Valid,
        /// <summary>
        /// 有效数QCCode!=9指标统计中有效数（秦龙2013.6.2添加）
        /// </summary>
        Valid2,
        /// <summary>
        /// 捕集率
        /// </summary>
        SamplePercent,
        /// <summary>
        /// 有效率
        /// </summary>
        ValidPercent,
        /// <summary>
        /// 应有数
        /// </summary>
        Count
    }

    /// <summary>
    /// 返回的发送状态
    /// </summary>
    public enum SendMessage
    {
        /// <summary>
        /// 服务尚未配置正确
        /// </summary>
        ConfigError,
        /// <summary>
        /// 发送过程中出现异常
        /// </summary>
        Exception,
        /// <summary>
        /// 已发送,但未取得消息
        /// </summary>
        SendNoMessage,
        /// <summary>
        /// 发送失败
        /// </summary>
        Fail,
        /// <summary>
        /// 未通过用户验证
        /// </summary>
        AuthenticationNotPassed,
        /// <summary>
        /// 参数不正确
        /// </summary>
        ArgumentException,
        /// <summary>
        /// 发送成功
        /// </summary>
        Success
    }

    /// <summary>
    /// 霾污染数据源类型
    /// </summary>
    public enum HazeDataSourceType
    {
        HazeVirtualSite = 0,
        HazeLevel = 1,
    }

    /// <summary>
    /// 霾污染日报的操作类型：创建、修改、查看、保存
    /// </summary>
    public enum HazeDailyReportOperType
    {
        None = -1,
        Create = 0,
        Update = 1,
        View = 2,
        Save = 3,
        ReBuild = 4
    }

    /// <summary>
    /// 臭氧指标统计的类别： 站点/分组， 小时/日/月/年
    /// </summary>
    public enum OzoneIndexType
    {
        SiteHour = 0,
        SiteDay = 1,
        SiteMonth = 2,
        SiteYear = 3,
        GroupHour = 4,
        GroupDay = 5,
        GroupMonth = 6,
        GroupYear = 7,
    }
    /// <summary>
    /// 一般处理流程的步骤
    /// </summary>
    public enum ProcessStep
    {
        Step1 = 0,
        Step2 = 1,
        Step3 = 2,
        Step4 = 3,
        Step5 = 4,
        Step6 = 5
    }
}
