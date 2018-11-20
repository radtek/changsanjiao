using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 系统字典
    /// </summary>
    public class DictionaryValueItem
    {
        private int code;
        private string description;
        private string key;
        private string value;
        private string standbyField1;
        private string standbyField2;
        private string standbyField3;
        private string standbyField4;
        private string standbyField5;
        private DateTime? addTime;
        private DateTime? updateTime;
        private int valueID;

        /// <summary>
        /// 获取或设置字典代码
        /// </summary>
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public int ValueID
        {
            get { return valueID; }
            set { valueID = value; }
        }

        /// <summary>
        /// 获取或设置字典的描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 获取或设置字典键
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取或设置字典的值
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// 备选属性1
        /// </summary>
        public string StandbyField1
        {
            get { return standbyField1; }
            set { standbyField1 = value; }
        }

        /// <summary>
        /// 备选属性2
        /// </summary>
        public string StandbyField2
        {
            get { return standbyField2; }
            set { standbyField2 = value; }
        }

        public string StandbyField3
        {
            get { return standbyField3; }
            set { standbyField3 = value; }
        }

        public string StandbyField4
        {
            get { return standbyField4; }
            set { standbyField4 = value; }
        }

        public string StandbyField5
        {
            get { return standbyField5; }
            set { standbyField5 = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }

        public DictionaryValueItem Clone()
        {
            DictionaryValueItem newModel = new DictionaryValueItem();
            newModel.addTime = this.addTime;
            newModel.code = this.code;
            newModel.description = this.description;
            newModel.key = this.key;
            newModel.standbyField1 = this.standbyField1;
            newModel.standbyField2 = this.StandbyField2;
            newModel.standbyField3 = this.StandbyField3;
            newModel.standbyField4 = this.StandbyField4;
            newModel.standbyField5 = this.StandbyField5;
            newModel.updateTime = this.updateTime;
            newModel.value = this.value;
            newModel.valueID = this.valueID;
            return newModel;
        }
    }
}
