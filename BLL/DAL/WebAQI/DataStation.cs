using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL.WebAQI
{
   public  class DataStation
    {
        public string stationID;//站点ID
        public string LST_AQI;
        public bool isHasData = true;
        public bool isGroup = false;//因为站点名和区号可能相等，所以加一个标示，如果是true的话是区号。

        /**
         * 是否为离用户最近的站点
         */
        public bool bNearestStation = false;

        /**
	 * 最近的距离
	 */
        public double nearestDistance;

        /**
         * pm25的AQI指数
         */
        public int pm25AQI;

        /**
         * pm25的浓度值
         */
        public double pm25Value;
        public int pm25Grade;
        public string pm25Quality;

        /**
         * pm10的AQI指数
         */
        public int pm10AQI;

        /**
         * pm10的浓度值
         */
        public double pm10Value;
        public int pm10Grade;
        public string pm10Quality;

        /**
         * o3的AQI指数
         */
        public int o3AQI;

        /**
         * o3的浓度值
         */
        public double o3Value;
        public int o3Grade;
        public string o3Quality;

        /**
         * co的AQI指数
         */
        public int coAQI;

        /**
         * co的浓度值
         */
        public double coValue;
        public int coGrade;
        public string coQuality;

        /**
         * so2的AQI指数
         */
        public int so2AQI;

        /**
         * so2的浓度值
         */
        public double so2Value;
        public int so2Grade;
        public string so2Quality;

        /**
         * no2的AQI值
         */
        public int no2AQI;

        /**
         * no2的浓度值
         */
        public double no2Value;
        public int no2Grade;
        public string no2Quality;

        /**
     * 首要污染物的类型
     * 
     * @return
     */
        public string primaryPollutantType;
        public int primaryPollutantAQI;
        public double primaryPollutantValue;
        public int primaryPollutantGrade;
        public string primaryPollutantQuality;
        public void updateData()
        {
            if (primaryPollutantGrade == 1)
            {
                primaryPollutantType = "—";
            }
        }

        /**
         * 首要污染物的AQI
         * 
         * @return
         */
        public int getPrimaryAQI()
        {
            return primaryPollutantAQI;
        }
        public void setPrimaryAQI(int primaryPollutantAQI)
        {
            this.primaryPollutantAQI = primaryPollutantAQI;
        }
        public int getPrimaryPollutantGrade()
        {
            return primaryPollutantGrade;
        }
        public void setPrimaryPollutantGrade(int primaryPollutantGrade)
        {
            this.primaryPollutantGrade = primaryPollutantGrade;
        }
        public string getPrimaryPollutantQuality()
        {
            return primaryPollutantQuality;

        }
        public void setPrimaryPollutantQuality(string primaryPollutantQuality)
        {
            this.primaryPollutantQuality = primaryPollutantQuality;
        }

        /**
         * 首要污染物浓度
         * 
         * @return
         */
        public double getPrimaryValue()
        {
            return primaryPollutantValue;
        }
        public void setPrimaryValue(double primaryPollutantValue)
        {
            this.primaryPollutantValue = primaryPollutantValue;
        }
        /**
             * 首要污染物的类型
             * 
             * @return
             */
        public string getPrimaryPollutantType()
        {
            return primaryPollutantType;
        }

        public void setPrimaryPollutantType(string primaryPollutantType)
        {
            this.primaryPollutantType = primaryPollutantType;
        }

        public string getLST()
        {
            return LST_AQI;
        }

        public void setLST(string LST_AQI)
        {
            this.LST_AQI = LST_AQI;
        }
        public string getStationID()
        {
            return stationID;
        }

        public void setStationID(string stationID)
        {
            this.stationID = stationID;
        }
        public bool isNearestStation()
        {
            return bNearestStation;
        }

        public void setNearestStation(bool nearestStation)
        {
            this.bNearestStation = nearestStation;
        }
        public bool getIsHasData()
        {
            return isHasData;
        }

        public void setIsHasData(bool isHasData)
        {
            this.isHasData = isHasData;
        }
        public bool getIsGroup()
        {
            return isGroup;
        }

        public void setIsGroup(bool isGroup)
        {
            this.isGroup = isGroup;
        }
        public double getNeadestDistance()
        {
            return nearestDistance;
        }

        public void setNeadestDistance(double neadestDistance)
        {
            this.nearestDistance = neadestDistance;
        }

        public int getPm25AQI()
        {
            return pm25AQI;
        }

        public void setPm25AQI(int pm25aqi)
        {
            pm25AQI = pm25aqi;
        }

        public double getPm25Value()
        {
            return pm25Value;
        }

        public void setPm25Value(double pm25Value)
        {
            this.pm25Value = pm25Value;
        }

        public int getPm10AQI()
        {
            return pm10AQI;
        }

        public void setPm10AQI(int pm10aqi)
        {
            pm10AQI = pm10aqi;
        }

        public double getPm10Value()
        {
            return pm10Value;
        }

        public void setPm10Value(double pm10Value)
        {
            this.pm10Value = pm10Value;
        }

        public int getO3AQI()
        {
            return o3AQI;
        }

        public void setO3AQI(int o3aqi)
        {
            o3AQI = o3aqi;
        }

        public double getO3Value()
        {
            return o3Value;
        }

        public void setO3Value(double o3Value)
        {
            this.o3Value = o3Value;
        }

        public int getCoAQI()
        {
            return coAQI;
        }

        public void setCoAQI(int coAQI)
        {
            this.coAQI = coAQI;
        }

        public double getCoValue()
        {
            return coValue;
        }

        public void setCoValue(double coValue)
        {
            this.coValue = coValue;
        }

        public int getSo2AQI()
        {
            return so2AQI;
        }

        public void setSo2AQI(int so2aqi)
        {
            so2AQI = so2aqi;
        }

        public double getSo2Value()
        {
            return so2Value;
        }

        public void setSo2Value(double so2Value)
        {
            this.so2Value = so2Value;
        }

        public int getNo2AQI()
        {
            return no2AQI;
        }

        public void setNo2AQI(int no2aqi)
        {
            no2AQI = no2aqi;
        }

        public double getNo2Value()
        {
            return no2Value;
        }

        public void setNo2Value(double no2Value)
        {
            this.no2Value = no2Value;
        }




        public int getPm25Grade()
        {
            return pm25Grade;
        }

        public void setPm25Grade(int pm25Grade)
        {
            this.pm25Grade = pm25Grade;
        }

        public string getpm25Quality()
        {
            return pm25Quality;
        }

        public void setpm25Quality(string pm25Quality)
        {
            this.pm25Quality = pm25Quality;
        }


        public int getPm10Grade()
        {
            return pm10Grade;
        }

        public void setPm10Grade(int pm10Grade)
        {
            this.pm10Grade = pm10Grade;
        }

        public string getPm10Quality()
        {
            return pm10Quality;
        }

        public void setPm10Quality(string pm10Quality)
        {
            this.pm10Quality = pm10Quality;
        }


        public int getO3Grade()
        {
            return o3Grade;
        }

        public void setO3Grade(int o3Grade)
        {
            this.o3Grade = o3Grade;
        }

        public string getO3Quality()
        {
            return o3Quality;
        }

        public void setO3Quality(string o3Quality)
        {
            this.o3Quality = o3Quality;
        }


        public int getNo2Grade()
        {
            return no2Grade;
        }

        public void setNo2Grade(int no2Grade)
        {
            this.no2Grade = no2Grade;
        }

        public string getNo2Quality()
        {
            return no2Quality;
        }

        public void setNo2Quality(string no2Quality)
        {
            this.no2Quality = no2Quality;
        }

        public int getSo2Grade()
        {
            return so2Grade;
        }

        public void setSo2Grade(int so2Grade)
        {
            this.so2Grade = so2Grade;
        }

        public string getSo2Quality()
        {
            return so2Quality;
        }

        public void setSo2Quality(string so2Quality)
        {
            this.so2Quality = so2Quality;
        }

        public int getcCoGrade()
        {
            return coGrade;
        }

        public void setCoGrade(int coGrade)
        {
            this.coGrade = coGrade;
        }

        public string getCoQuality()
        {
            return coQuality;
        }

        public void setCoQuality(string coQuality)
        {
            this.coQuality = coQuality;
        }

    }
}
