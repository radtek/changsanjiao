using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 因子分组中的因子
    /// </summary>
    public class FactorGroupItem
    {
        public int ID{ get; set; }
        public string GroupID { get; set; }
        public int ParameterId { get; set; }
    }
}
