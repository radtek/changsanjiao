using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using MMShareBLL.Model;
using Readearth.Data;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 与污染因子相关的数据库操作
    /// </summary>
    public class FactorDataManager
    {
        //procedure name
        private const string PROC_QUERYALL = "Parameters_Query";
        private const string PROC_QUERYBYID = "Parameters_QueryById";

        private const string PROC_QUERY_MAPPING = "Parameters_QueryMapping";
        private const string PROC_QUERY_MAPPING_BYID = "Parameters_QueryMappingById";

        private const string PROC_QUERY_BYINGESTCODE = "Parameters_QueryByIngestCode";
        private const string PROC_QUERY_MAPPING_BYINGESTCODE = "Parameters_QueryMappingByIngestCode";

        private const string PROC_QUERYPARAMETER = "Parameters_Query_DMS";
        private const string PROC_QUERYPARAMETERBYID = "Parameters_QueryByid_DMS";
        private const string PROC_QUERYPARAMETERBYINGESTCODE = "Parameters_QueryByIngestCode_DMS";

        //parameters
        private const string PARAMID = "@id";
        private const string PARAM_DATAMODE = "dataMode";
        private const string PARAM_INGESTCODE = "@ingestCode";
        private Database m_DatabaseS;
        public FactorDataManager()
        {
            m_DatabaseS = new Database("SEMCDMC");
        }
        /// <summary>
        /// 获得DMS参数列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, DataParameter> QueryParameter()
        {
            try
            {
                Dictionary<int, DataParameter> theData = new Dictionary<int, DataParameter>();

                using (SqlDataReader dataReader = m_DatabaseS.GetDataReader(PROC_QUERYPARAMETER))
                {
                    DictionaryData dData = new DictionaryData();
                    Dictionary<int, DictionaryValueItem> dictData = dData.GetDictionary(9);
                    while (dataReader.Read())
                    {
                        DataParameter factor = distillParameterData(dictData, dataReader);
                        theData.Add(factor.Id, factor);
                    }
                    return theData;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// 根据编号获得DMS参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataParameter QueryParameter(int id)
        {
            SqlParameter paramID = new SqlParameter(PARAMID, System.Data.SqlDbType.SmallInt);
            paramID.Value = id;
            try
            {
                DataParameter theData = null;

                using (SqlDataReader dataReader = m_DatabaseS.GetDataReader( PROC_QUERYPARAMETERBYID, paramID))
                {
                    DictionaryData dData = new DictionaryData();
                    Dictionary<int, DictionaryValueItem> dictData = dData.GetDictionary(9);
                    if (dataReader.Read())
                        theData = distillParameterData(dictData, dataReader);
                    return theData;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// 根据统一编码获得DMS参数信息
        /// </summary>
        /// <param name="ingestCode"></param>
        /// <returns></returns>
        public DataParameter QueryParameter(string ingestCode)
        {
            SqlParameter paramIngestCode = new SqlParameter(PARAM_INGESTCODE, SqlDbType.NVarChar, 50);
            paramIngestCode.Value = ingestCode;
            try
            {
                DataParameter theData = null;

                using (SqlDataReader dataReader = m_DatabaseS.GetDataReader( PROC_QUERYPARAMETERBYINGESTCODE, paramIngestCode))
                {
                    DictionaryData dData = new DictionaryData();
                    Dictionary<int, DictionaryValueItem> dictData = dData.GetDictionary(9);
                    if (dataReader.Read())
                        theData = distillParameterData(dictData, dataReader);
                    return theData;
                }
            }
            catch { throw; }
        }

        private DataParameter distillParameterData(Dictionary<int, DictionaryValueItem> dictData, SqlDataReader dataReader)
        {
            DataParameter factor = new DataParameter();
            factor.UnitType = UnitParserType.NoConvert;
            factor.Id = Convert.ToInt32(dataReader["ParameterID"]);
            factor.Name = dataReader["Name"] as string;
            factor.Byname = dataReader["Description"] as string;
            factor.Code = dataReader["IngestCode"] as string;

            if (DBNull.Value != dataReader["UnitID"])
            {
                factor.Unit = new Unit();
                factor.Unit.Id = Convert.ToInt32(dataReader["UnitID"]);
                factor.Unit.Name = dataReader["Unit"] as string;
            }
            float[] thincknessRange = getThincknessRange(dictData, factor.Id);
            if (thincknessRange != null)
            {
                factor.CanComputeApi = true;
                factor.ConcentrationLimits = thincknessRange;
            }
            factor.DailyPercent = Convert.ToDecimal(dataReader["DailyPercent"]);
            return factor;
        }

        private float[] getThincknessRange(Dictionary<int, DictionaryValueItem> dictData, int parameterID)
        {
            float[] thincknessRange = null;
            foreach (DictionaryValueItem mapping in dictData.Values)
            {
                if (parameterID.ToString() == mapping.Key)
                {
                    string strRange = mapping.Value;
                    if (!string.IsNullOrEmpty(strRange))
                    {
                        thincknessRange = Pub.ParseToArray(strRange);
                    }
                    break;
                }
            }
            return thincknessRange;
        }
    }
}
