using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    [Serializable()]
    public class Weather
    {
        private string forecastDay;
        private string conditionsAM;
        private string conditionsPM;
        private string conditionsNight;
        private string highTemperature;
        private string lowTemperature;
        private string direction;
        private string speed;

        public Weather() { }

        public string ForecastDay
        {
            get { return this.forecastDay; }
            set { this.forecastDay = value; }
        }
        public string ConditionsAM
        {
            get { return this.conditionsAM; }
            set { this.conditionsAM = value; }
        }
        public string ConditionsPM
        {
            get { return this.conditionsPM; }
            set { this.conditionsPM = value; }
        }
        public string ConditionsNight
        {
            get { return this.conditionsNight; }
            set { this.conditionsNight = value; }
        }
        public string HighTemperature
        {
            get { return this.highTemperature; }
            set { this.highTemperature = value; }
        }
        public string LowTemperature
        {
            get { return this.lowTemperature; }
            set { this.lowTemperature = value; }
        }
        public string Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }
        public string Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
    }
}
