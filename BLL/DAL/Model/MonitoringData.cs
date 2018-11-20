
namespace MMShareBLL.Model
{
    /// <summary>
    /// 表示对于某一污染因子的监测或预报数据
    /// </summary>
    public class MonitoringData
    {
        private int? api;
        private int? aqi;
        private float thickness;
        private DataParameter factor;
        private bool valid = true;
        private int opCode;
        private int qcCode;
        private int dataId;
        private string oPName;

        /// <summary>
        /// 获取或设置该项监测或预报的污染浓度
        /// </summary>
        public float Thickness
        {
            get
            {
                return thickness;
            }
            set
            {
                thickness = value;
            }
        }

        /// <summary>
        /// 获取或设置该项监测或预报所计算出的空气污染指数。仅当污染因子的CanComputeAPI属性为True时有效。
        /// </summary>
        public int? API
        {
            get
            {
                return api;
            }
            set
            {
                api = value;
            }
        }

        /// <summary>
        /// 获取或设置该项监测或预报所计算出的AQI空气污染指数。仅当污染因子的CanComputeAQI属性为True时有效。
        /// </summary>
        public int? AQI
        {
            get
            {
                return aqi;
            }
            set
            {
                aqi = value;
            }
        }

        /// <summary>
        /// 获取或设置监测或预报的污染因子
        /// </summary>
        public DataParameter Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public int OPCode
        {
            get { return opCode; }
            set { opCode = value; }
        }

        public int QCCode
        {
            get { return qcCode; }
            set { qcCode = value; }
        }

        public int DataID
        {
            get { return dataId; }
            set { dataId = value; }
        }

        public string OPName
        {
            get { return oPName; }
            set { oPName = value; }
        }

        public MonitoringData Clone()
        {
            MonitoringData newObj = new MonitoringData();
            if (this.Factor != null)
                newObj.Factor = this.Factor.Clone();
            newObj.api = this.api;
            newObj.aqi = this.aqi;
            newObj.thickness = this.thickness;
            newObj.valid = this.valid;
            newObj.opCode = this.opCode;
            newObj.qcCode = this.qcCode;
            newObj.dataId = this.dataId;
            newObj.oPName = this.oPName;
            return newObj;
        }
    }
}
