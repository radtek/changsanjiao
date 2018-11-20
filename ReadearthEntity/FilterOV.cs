using System;
using System.Collections;

namespace Readearth.Data.Entity
{
    /// <summary>
    /// 此类用于生成一个数据集的操作集合
    /// </summary>
    public class FilterOV : CollectionBase
    {

        public FilterOV()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        public void Add(PropertyOV fieldValue)
        {
            List.Add(fieldValue);
        }

        public void Remove(PropertyOV fieldValue)
        {
            List.Remove(fieldValue);
        }

        public PropertyOV this[int index]
        {
            get
            {
                return (PropertyOV)List[index];
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
