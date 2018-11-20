using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaAQI
{
    public class AQIExtention
    {
        private DateTime m_LST;
        private string m_SiteName;
        private int m_AQI;
        private int m_Intgrade;
        private string m_Color;
        private string m_Picture;
        private string m_Grade;
        private string m_Quality;
        private string m_FirstPItem;
        private string m_FirstPItemNoByGrade;
        private string m_FirstParameter;
        private string m_Health;
        private string m_Protection;
        private string m_FirstItem;//地听公司添加   2013年07月04日

        public AQIExtention()
        {

        }
        public AQIExtention(int AQI)
        {
            setGrade(AQI);
            setExtion(this.IntGrade);
            
        }
        public AQIExtention(int AQI, int firstPItemID)
        {
            setGrade(AQI);
            setExtion(this.IntGrade);
            setFirstItem(firstPItemID, this.IntGrade);
            setFirstPItem(firstPItemID, this.IntGrade);
        }

        //private int aqi;
        //private int intgrade;
        //private string grade;
        //private string quality;
        //private string firstPitemid;
        //private string health;
        //private string protection;
        public DateTime LST
        {
            get { return m_LST; }
            set { m_LST = value; }
        }
        public string SiteName
        {
            get { return m_SiteName; }
            set { m_SiteName = value; }
        }
        /// <summary>
        /// 获取或设置该AQIExtention的AQI值
        /// </summary>
        public int AQI
        {
            get { return m_AQI; }
            set { m_AQI = value; }
        }
        public int IntGrade
        {
            get { return m_Intgrade; }
            set { m_Intgrade = value; }
        }
        public string Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
        public string Picture
        {
            get { return m_Picture; }
            set { m_Picture = value; }
        }

        public string Grade
        {
            get { return m_Grade; }
            set { m_Grade = value; }
        }

        public string Quality
        {
            get { return m_Quality; }
            set { m_Quality = value; }
        }

        public string FirstPItem
        {
            get { return m_FirstPItem; }
            set { m_FirstPItem = value; }
        }
        public string FirstParameter
        {
            get { return m_FirstParameter; }
            set { m_FirstParameter = value; }
        }
        public string FirstItem
        {
            get { return m_FirstItem; }
            set { m_FirstItem = value; }
        }
        public string FirstPItemNoByGrade
        {
            get { return m_FirstPItemNoByGrade; }
            set { m_FirstPItemNoByGrade = value; }
        }
        public string Health
        {
            get { return m_Health; }
            set { m_Health = value; }
        }

        public string Protection
        {
            get { return m_Protection; }
            set { m_Protection = value; }
        }

        ////////////////////////////////////
        private void setGrade(int AQI)
        {
            this.AQI = AQI;
            if (AQI >= 0 & AQI <= 50)
            {

                this.IntGrade = 1;
                this.Grade = "一级";
                this.Quality = "优";
                this.Color = "green";
                this.Picture = "1.png";
                 

            }
            else if (AQI > 50 & AQI <= 100)
            {
                this.IntGrade = 2;
                this.Grade = "二级";
                this.Quality = "良";
                this.Color = "yellow";
                this.Picture = "2.png";

            }
            else if (AQI > 100 & AQI <= 150)
            {
                this.IntGrade = 3;
                this.Grade = "三级";
                this.Quality = "轻度污染";
                this.Color = "orange";
                this.Picture = "3.png";

            }
            else if (AQI > 150 & AQI <= 200)
            {
                this.IntGrade = 4;
                this.Grade = "四级";
                this.Quality = "中度污染";
                this.Color = "red";
                this.Picture = "4.png";
            }
            else if (AQI > 200 & AQI <= 300)
            {
                this.IntGrade = 5;
                this.Grade = "五级";
                this.Quality = "重度污染";
                this.Color = "purple";
                this.Picture = "5.png";
            }
            else if (AQI > 300)
            {
                this.IntGrade = 6;
                this.Grade = "六级";
                this.Quality = "严重污染";
                this.Color = "grayred";
                this.Picture = "6.png";
            }

        }

        private void setExtion(int IntGrade)
        {
            switch (IntGrade)
            {
                case 1:
                    this.Health = "空气质量令人满意，基本无空气污染。";
                    this.Protection = "各类人群可正常活动。";
                    break;
                case 2:
                    this.Health = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响。";
                    Protection = "极少数异常敏感人群应减少户外活动。";
                    break;
                case 3:
                    this.Health = "易感人群症状有轻度加剧，健康人群出现刺激症状。";
                    this.Protection = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼。";
                    break;
                case 4:
                    this.Health = "进一步加剧易感人群症状，可能对健康人群心脏、呼吸系统有影响。";
                    this.Protection = "儿童、老年人及心脏病、呼吸系统疾病患者避免长时间、高强度的户外锻炼，一般人群适量减少户外运动。";
                    break;
                case 5:
                    this.Health = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状。";
                    this.Protection = "儿童、老年人及心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动。";
                    break;
                case 6:
                    this.Health = "健康人群运动耐受力降低，有明显强烈症状，提前出现某些疾病。";
                    this.Protection = "儿童、老年人和病人应停留在室内，避免体力消耗，一般人群避免户外运动。";
                    break;

            }
        }


        private void setFirstPItem(int FirstPItemID, int IntGrade)
        {
            
            if (IntGrade == 1)
            {
                this.FirstPItem = "无"; this.FirstParameter = "无";
                return;
            }
            switch (FirstPItemID)
            {
                case 1: this.FirstPItem = "SO2 1小时"; this.FirstParameter = "SO2"; break;
                case 2: this.FirstPItem = "NO2 1小时"; this.FirstParameter = "N2"; break;
                case 3: this.FirstPItem = "PM10 1小时"; this.FirstParameter = "PM10"; break;
                case 4: this.FirstPItem = "PM10 24小时"; this.FirstParameter = "PM10"; break;
                case 5: this.FirstPItem = "CO 1小时"; this.FirstParameter = "CO"; break;
                case 6: this.FirstPItem = "O3 1小时"; this.FirstParameter = "O3"; break;
                case 7: this.FirstPItem = "O3 8小时"; this.FirstParameter = "O3"; break;
                case 8: this.FirstPItem = "PM2.5 1小时"; this.FirstParameter = "PM2.5"; break;
                case 9: this.FirstPItem = "PM2.5 24小时"; this.FirstParameter = "PM2.5"; break;

            }
        }

        private void setFirstItem(int firstItem, int intGrade)
        {

            switch (firstItem)
            {
                case 1: m_FirstItem = "PM2.5"; m_FirstPItemNoByGrade = "PM2.5"; break;
                case 2: m_FirstItem = "PM10"; m_FirstPItemNoByGrade = "PM10"; break;
                case 3: m_FirstItem = "NO2"; m_FirstPItemNoByGrade = "NO2"; break;
                case 4: m_FirstItem = "臭氧1h"; m_FirstPItemNoByGrade = "O3"; break;
                case 5: m_FirstItem = "臭氧8h"; m_FirstPItemNoByGrade = "O3"; break;
            }
            if (IntGrade == 1)
            {
                m_FirstItem = "无";
                return;
            }
        }
        


    }

}
