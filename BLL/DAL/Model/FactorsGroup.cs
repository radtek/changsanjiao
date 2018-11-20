using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public class FactorsGroup
    {
        private List<DataParameter> factorItems;
        public FactorsGroup()
        {
            FactorItem = new List<DataParameter>();
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 分组编号
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 分组下因子列表
        /// </summary>
        public List<DataParameter> FactorItem 
        {
            get
            {
                return factorItems;
            }
            set
            {
                factorItems = value;
            }
        }
    }
}
