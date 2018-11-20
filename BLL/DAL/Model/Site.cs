using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    /// <summary>
    /// 表示监测站点的实体
    /// </summary>
    public class Site
    {
        private int id;
        private float latitude;
        private float longitude;
        private string name;
        private County count;
        private SiteType siteType;
        private string remark;
        private int siteCode;
        private int dCode;
        private int orderId;
        private string byname;
        private string byname2;
        private string pCode;

        public Site()
        {
        }

        public Site(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// 获取或设置该站点的编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 显示的站点编号，以及模式数据的站点编号
        /// </summary>
        public int SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }

        /// <summary>
        /// DMS站点编号
        /// </summary>
        public int DCode
        {
            get { return dCode; }
            set { dCode = value; }
        }

        /// <summary>
        ///  获取或设置该站点的纬度
        /// </summary>
        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        ///  获取或设置该站点的经度
        /// </summary>
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        ///  获取或设置该站点的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 简称
        /// </summary>
        public string Byname
        {
            get { return byname; }
            set { byname = value; }
        }

        /// <summary>
        /// 获取或设置该站点的备注信息
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 获取或设置该站点所在的区县信息
        /// </summary>
        public County County
        {
            get { return count; }
            set { count = value; }
        }

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        /// <summary>
        /// 获取或设置该站点的类型
        /// </summary>
        public SiteType SiteType
        {
            get { return siteType; }
            set { siteType = value; }
        }

        public string ByName2
        {
            get { return byname2; }
            set { byname2 = value; }
        }

        /// <summary>
        /// 国控点 站点编号
        /// </summary>
        public string PCode
        {
            get { return pCode; }
            set { pCode = value; }
        }
    }
}
