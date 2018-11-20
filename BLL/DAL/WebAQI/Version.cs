using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL.WebAQI
{
    public class Version
    {
        public string versionName;//版本名称
        public double  versionNumber;//版本号
        public string VersionValue;
        public string getVersionName()
        {
            return versionName;
        }

        public void setVersionName(string versionName)
        {
            this.versionName = versionName;
        }

        public double getVersionNumber()
        {
            return versionNumber;
        }

        public void setVersionNumber(double versionNumber)
        {
            this.versionNumber = versionNumber;
        }
        public string getVersionValue()
        {
            return VersionValue;
        }

        public void setVersionValue(string VersionValue)
        {
            this.VersionValue = VersionValue;
        }
    }
}
