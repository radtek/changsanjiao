using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 污染因子的单位
    /// </summary>
    public class Unit 
    {
        private int id;
        private string name;

        /// <summary>
        /// 获取或设置单位的编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 获取或设置单位名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Unit Clone()
        {
            Unit newObj = new Unit();
            newObj.id = this.id;
            newObj.name = this.name;
            return newObj;
        }
    }
}
