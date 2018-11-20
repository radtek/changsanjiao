using System;
using System.Collections.Generic;
using System.Text;


namespace MMShareBLL.DAL
{
    public class JsonDeserial
    {
        public JsonDeserial()
        {

        }

        public List<AQIPeriodCell> GetAQICellsFromJson(string inputJson)
        {
            List<AQIPeriodCell> cellList=new List<AQIPeriodCell>();
            string[] strPairs;
            string strKey = "";
            string strValue = "";
            AQIPeriodCell aqiCell;
            if(inputJson!=null && inputJson!="")
            {
                inputJson = inputJson.Trim('[',']');
                string[] strJsons = inputJson.Split('}');
                string strSingle = "";
                for(int i=0;i<strJsons.Length;i++)
                {
                    strJsons[i] = strJsons[i].Trim(',');
                    strJsons[i] = strJsons[i].Trim('{');
                    strPairs = strJsons[i].Split(',');
                    aqiCell=new AQIPeriodCell();
                    //单条Json记录
                    for (int j = 0; j < strPairs.Length; j++)
                    {
                        strPairs[j].TrimStart('{');
                        if (strPairs[j].Split(':')[0].Trim('"') == "LST")
                        {
                            aqiCell.LST = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "ForecastDate")
                        {
                            aqiCell.ForecastDate = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "PERIOD")
                        {
                            aqiCell.Period = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "Module")
                        {
                            aqiCell.Organization = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "durationID")
                        {
                            aqiCell.Duration= strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "ITEMID")
                        {
                            aqiCell.AQIId = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "Value")
                        {
                            aqiCell.Value= strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "AQI")
                        {
                            aqiCell.AQI = strPairs[j].Split(':')[1].Trim('"');
                        }
                        if (strPairs[j].Split(':')[0].Trim('"') == "Parameter")
                        {
                            aqiCell.Parameter = strPairs[j].Split(':')[1].Trim('"');
                        }
                            
                    }
                    cellList.Add(aqiCell);
                }
            }
            return cellList;
        }
    }
  
 
}
