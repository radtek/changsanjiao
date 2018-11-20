using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 因子参数
    /// </summary>
    public class DataParameter
    {
        private int id;
        private string code;
        private string name;
        private string byname;
        private bool canComputeApi = false;
        private UnitParserType unitType = UnitParserType.NoConvert;
        private float[] concentrationLimits = null;
        private float max;
        private float min;
        private float mr;
        private string remark;
        private Unit unit;
        private string dataColumn;
        private string calcExpression;
        private ParameterCollectionType collectionType = ParameterCollectionType.Avg;
        private string color;
        private bool defaultChecked = false;
        private bool calcCitywideApi = false;
        private bool visible = true;
        private decimal dailyPercent = 0;
        private int orderId;

        /// <summary>
        /// 获取或设置污染因子的编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置污染因子的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 获取或设置污染因子的别名，一般指其中文名
        /// </summary>
        public string Byname
        {
            get { return byname; }
            set { byname = value; }
        }

        /// <summary>
        /// 获取或设置污染因子是否能计算API
        /// </summary>
        public bool CanComputeApi
        {
            get { return canComputeApi; }
            set { canComputeApi = value; }
        }

        /// <summary>
        /// 获取或设置污染因子的单位转换方案
        /// </summary>
        public UnitParserType UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        /// <summary>
        /// 获取或设置污染因子的一组浓度范围，该属性
        /// </summary>
        public float[] ConcentrationLimits
        {
            get { return concentrationLimits; }
            set { concentrationLimits = value; }
        }

        /// <summary>
        /// 获取或设置该污染因子最大的有效值
        /// </summary>
        public float Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// 获取或设置该污染因子最小有效值
        /// </summary>
        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// 获取或设置该污染因子的分子量
        /// </summary>
        public float MolecularWeight
        {
            get { return mr; }
            set { mr = value; }
        }

        /// <summary>
        /// 获取或设置该污染因子的备注信息
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 获取或设置该污染因子的单位信息
        /// </summary>
        public Unit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        /// <summary>
        /// 该污参数和计算表达式
        /// </summary>
        public string CalcExpression
        {
            get { return calcExpression; }
            set { calcExpression = value; }
        }

        /// <summary>
        /// 指示污染因子从集合中的提取方式
        /// </summary>
        public ParameterCollectionType CollectionType
        {
            get { return collectionType; }
            set { collectionType = value; }
        }

        /// <summary>
        /// 该参数在统计图中所表现的颜色
        /// </summary>
        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// 指示在一组复选框中，该参数是否会默认选中
        /// </summary>
        public bool DefaultChecked
        {
            get { return defaultChecked; }
            set { defaultChecked = value; }
        }

        /// <summary>
        /// 监测因子的统一代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 是否加入全市API计算
        /// </summary>
        public bool CalcCitywideApi
        {
            get { return calcCitywideApi; }
            set { calcCitywideApi = value; }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// 有效率
        /// </summary>
        public decimal DailyPercent
        {
            get { return dailyPercent; }
            set { dailyPercent = value; }
        }

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public string DataColumn
        {
            get { return dataColumn; }
            set { dataColumn = value; }
        }

        public DataParameter Clone()
        {
            DataParameter newObj = new DataParameter();
            newObj.id = this.id;
            newObj.name = this.name;
            newObj.byname = this.byname;

            newObj.canComputeApi = this.canComputeApi;
            newObj.UnitType = this.unitType;
            newObj.CalcExpression = this.calcExpression;
            if (this.concentrationLimits != null)
            {
                newObj.concentrationLimits = new float[this.concentrationLimits.Length];
                this.concentrationLimits.CopyTo(newObj.concentrationLimits, 0);
            }
            newObj.max = this.max;
            newObj.min = this.min;
            newObj.code = this.code;
            newObj.mr = this.mr;
            newObj.remark = this.remark;
            if (this.unit != null)
                newObj.unit = this.unit.Clone();

            newObj.color = this.color;
            newObj.defaultChecked = this.defaultChecked;
            newObj.calcCitywideApi = this.calcCitywideApi;
            newObj.visible = this.visible;
            newObj.dataColumn = this.dataColumn;
            newObj.dailyPercent = this.dailyPercent;
            newObj.orderId = this.orderId;
            return newObj;
        }
    }
}
