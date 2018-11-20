using System;
using System.Collections.Generic;



namespace MMShareBLL.Model
{
    public class OperationType
    {

        private int _id;
        private string _name;
        private int? _onesupno;
        private int? _twosupno;
        private string _remark;
        private bool isDefaultView;
        private List<DownContext> downItems = new List<DownContext>();
        private OperationType parent;
        private Dictionary<int, OperationType> itmes = new Dictionary<int, OperationType>();
        private string type;
        private string baseUrl;

        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }

        public string Type
        {
            get { return type; }
            set { this.type = value; }
        }

        public string BaseUrl
        {
            get { return baseUrl; }
            set { this.baseUrl = value; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 所属类别的顶级
        /// </summary>
        public int? OnesupNO
        {
            set { _onesupno = value; }
            get { return _onesupno; }
        }

        /// <summary>
        /// 所属类别的上一级
        /// </summary>
        public int? TwosupNO
        {
            set { _twosupno = value; }
            get { return _twosupno; }
        }

        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        public bool IsDefaultView
        {
            set { isDefaultView = value; }
            get { return isDefaultView; }
        }

        public List<DownContext> DownItems
        {
            set { downItems = value; }
            get { return downItems; }
        }

        public Dictionary<int, OperationType> Items
        {
            get { return itmes; }
        }

        public OperationType Parent
        {
            set { parent = value; }
            get { return parent; }
        }


        public OperationType Clone()
        {
            OperationType instance = new OperationType();
            instance._id = this._id;
            instance._name = this._name;
            instance._onesupno = this._onesupno;
            instance._twosupno = this._twosupno;
            instance._remark = this._remark;
            instance.isDefaultView = this.isDefaultView;
            foreach (DownContext dc in this.downItems)
                instance.downItems.Add(dc.Clone());
            foreach (int childId in this.itmes.Keys)
                instance.itmes[childId] = this.itmes[childId];
            instance.parent = this.parent;
            instance.type = this.type;
            instance.baseUrl = this.baseUrl;

            return instance;
        }

    }
}

