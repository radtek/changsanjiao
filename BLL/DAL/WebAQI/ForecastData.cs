using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL.WebAQI
{
    public  class ForecastData
    {
        //空气预报信息
        public int  groupID;//站点ID
        public string LST_AQI;//预报时间
        public string heath;//健康情况
        public string adjusted;//建议措施
        public string seg1;//第一段时间
        public string seg2;
        public string seg3;
        public string AQI1;//AQI
        public string AQI2;
        public string AQI3;
        public string grade1;//级别
        public string grade2;
        public string grade3;
        public string param1;//首要污染物
        public string param2;
        public string param3;
        public string detail;//描述信息
        public int getGroupID()
        {
            return groupID;
        }
        public void setGroupID(int groupID)
        {
            this.groupID = groupID;
        }
        public string getLST()
        {
            return LST_AQI;
        }

        public void setLST(string LST_AQI)
        {
            this.LST_AQI = LST_AQI;
        }
        public string getHeath()
        {
            return heath;
        }

        public void setHeath(string heath)
        {
            this.heath = heath;
        }
        public string getAdjusted()
        {
            return adjusted;
        }

        public void setAdjusted(string adjusted)
        {
            this.adjusted = adjusted;
        }


        public string getSeg1()
        {
            return seg1;
        }
        public void setSeg1(string seg1)
        {
            this.seg1 = seg1;
        }
        public string getSeg2()
        {
            return seg2;
        }
        public void setSeg2(string seg2)
        {
            this.seg2 = seg2;
        }
        public string getSeg3()
        {
            return seg3;
        }
        public void setSeg3(string seg3)
        {
            this.seg3 = seg3;
        }


        public string getAQI1()
        {
            return AQI1;
        }
        public void setAQI1(string AQI1)
        {
            this.AQI1 = AQI1;
        }
        public string getAQI2()
        {
            return AQI2;
        }
        public void setAQI2(string AQI2)
        {
            this.AQI2 = AQI2;
        }
        public string getAQI3()
        {
            return AQI3;
        }
        public void setAQI3(string AQI3)
        {
            this.AQI3 = AQI3;
        }
        public string getGrade1()
        {
            return grade1;
        }
        public void setGrade1(string grade1)
        {
            this.grade1 = grade1;
        }
        public string getGrade2()
        {
            return grade2;
        }
        public void setGrade2(string grade2)
        {
            this.grade2 = grade2;
        }
        public string getGrade3()
        {
            return grade3;
        }
        public void setGrade3(string grade3)
        {
            this.grade3 = grade3;
        }
        public string getParam1()
        {
            return param1;
        }
        public void setParam1(string param1)
        {
            this.param1 = param1;
        }
        public string getParam2()
        {
            return param2;
        }
        public void setParam2(string param2)
        {
            this.param2 = param2;
        }
        public string getParam3()
        {
            return param3;
        }
        public void setParam3(string param3)
        {
            this.param3 = param3;
        }
        public string getDetail()
        {
            return detail;
        }
        public void setDetail(string detail)
        {
            this.detail = detail;
        }


        
    }
}
