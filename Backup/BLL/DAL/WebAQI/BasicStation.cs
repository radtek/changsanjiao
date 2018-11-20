using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL.WebAQI
{
    public class BasicStation
    {
        public string stationID;//站点ID
        public string stationName;//站点名
        public string groupParentID;//站点所属上级
        public bool isGroup = false;//因为站点名和区号可能相等，所以加一个标示，如果是true的话是区号。
        /**
	 * 经度
	 */
        public double longitude;

        /**
         * 纬度
         */
        public double latitude;
        public string getStationID()
        {
            return stationID;
        }

        public void setStationID(string stationID)
        {
            this.stationID = stationID;
        }
        public string getStationName()
        {
            return stationName;
        }

        public void setStationName(string stationName)
        {
            this.stationName = stationName;
        }
        public string getGroupParentID()
        {
            return groupParentID;
        }

        public void setGroupParentID(string groupParentID)
        {
            this.groupParentID = groupParentID;
        }

        public double getLongitude()
        {
            return longitude;
        }

        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }

        public double getLatitude()
        {
            return latitude;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }
        public bool getIsGroup()
        {
            return isGroup;
        }

        public void setIsGroup(bool isGroup)
        {
            this.isGroup = isGroup;
        }

    }
}
