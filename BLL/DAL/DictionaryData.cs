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
    /// 提供字典表数据的提取
    /// </summary>
    public class DictionaryData
    {
        //procedures
        private const string PROC_QUERYCATEGORYDICTIONARY = "pr_querydictionaryCateogry";
        private const string PROC_QUERYDICTIONARY = "pr_querydictionary";
        private const string PROC_QUERYDICTIONARYBYCOE = "pr_querydictionarybyid";
        private const string PROC_DICTIONARY_INSERT = "pr_dictionary_insert";
        private const string PROC_DICTIONARY_UPDATE = "pr_dictionary_update";
        private const string PROC_DICTIONARY_REMOVE = "pr_removeDictionary";

        //parameters
        private const string PARAMCODE = "@code";
        private const string PARAM_VALUEID = "@ValueID";
        private const string PARAM_DICTIONARYID = "@dictionaryId";
        private const string PARAM_KEY = "@key";
        private const string PARAM_VALUE = "@value";
        private const string PARAM_DESC = "@desc";

        private const string PARAM_STANDBYFIELD1 = "@standbyField1";
        private const string PARAM_STANDBYFIELD2 = "@standbyField2";
        private const string PARAM_STANDBYFIELD3 = "@standbyField3";
        private const string PARAM_STANDBYFIELD4 = "@standbyField4";
        private const string PARAM_STANDBYFIELD5 = "@standbyField5";
        private Database m_DatabaseS;
        public DictionaryData() {
            m_DatabaseS = new Database("SEMCDMC");
        }

        public Collection<AQIDictionary> GetCategory()
        {
            Collection<AQIDictionary> theData = new Collection<AQIDictionary>();
            using (SqlDataReader reader = m_DatabaseS.GetDataReader(PROC_QUERYCATEGORYDICTIONARY))
            {
                while (reader.Read())
                {
                    AQIDictionary dict = new AQIDictionary();
                    dict.Code = Convert.ToInt32(reader["ID"]);
                    dict.Description = reader["Description"] as string;
                    dict.Tooltip = reader["fldTooltip"] as string;
                    theData.Add(dict);
                }
            }
            return theData;
        }

        /// <summary>
        /// 获得所有的字典信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<int, DictionaryValueItem>> GetDictionary()
        {
            Dictionary<int, Dictionary<int, DictionaryValueItem>> dictionary = new Dictionary<int, Dictionary<int, DictionaryValueItem>>();
            try
            {
                using (SqlDataReader dataReader = m_DatabaseS.GetDataReader( PROC_QUERYDICTIONARY))
                {
                    while (dataReader.Read())
                    {
                        int code = Convert.ToInt32(dataReader["id"]);
                        Dictionary<int, DictionaryValueItem> theData = null;
                        if (dictionary.ContainsKey(code))
                            theData = dictionary[code];
                        else
                        {
                            theData = new Dictionary<int, DictionaryValueItem>();
                            dictionary.Add(code, theData);
                        }
                        DictionaryValueItem dictionaryValue = distillData(dataReader);
                        if (null != dictionaryValue)
                            theData[dictionaryValue.ValueID] = dictionaryValue;
                    }
                    return dictionary;
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// 根据字典代码获取相关配置信息
        /// </summary>
        /// <param name="code">字典码</param>
        /// <returns></returns>
        public Dictionary<int, DictionaryValueItem> GetDictionary(int code)
        {
            SqlParameter paramCode = new SqlParameter(PARAMCODE, System.Data.SqlDbType.Int);
            paramCode.Value = code;
            Dictionary<int, DictionaryValueItem> theData = new Dictionary<int, DictionaryValueItem>();
            try
            {
                using (SqlDataReader dataReader = m_DatabaseS.GetDataReader( PROC_QUERYDICTIONARYBYCOE, paramCode))
                {
                    while (dataReader.Read())
                    {
                        DictionaryValueItem dictionary = distillData(dataReader);
                        theData[dictionary.ValueID] = dictionary;
                    }
                    return theData;
                }
            }
            catch { throw; }
        }

        public bool CreateDictionary(DictionaryValueItem dictionary)
        {
            SqlParameter[] param = getParameter(dictionary).ToArray();
            return m_DatabaseS.Execute( PROC_DICTIONARY_INSERT, param) == 1;

        }

        public bool UpdateDictionary(DictionaryValueItem dictionary)
        {
            List<SqlParameter> parameters = getParameter(dictionary);
            SqlParameter paramValueId = new SqlParameter(PARAM_VALUEID, SqlDbType.Int);
            paramValueId.Value = dictionary.ValueID;
            parameters.Add(paramValueId);
            return m_DatabaseS.Execute( PROC_DICTIONARY_UPDATE, parameters.ToArray()) == 1;
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool RemoveDictionary(int identity)
        {
            try
            {
                SqlParameter paramValueId = new SqlParameter(PARAM_VALUEID, SqlDbType.Int);
                paramValueId.Value = identity;
                m_DatabaseS.Execute( PROC_DICTIONARY_REMOVE, paramValueId);
                return true;
            }
            catch { throw; }
        }

        public int RemoveDictionary(int[] valueIdArray)
        {
            try
            {
                SqlParameter paramValueId = new SqlParameter(PARAM_VALUEID, SqlDbType.Int);
                int sum = 0;
                using (SqlConnection conn = new SqlConnection(m_DatabaseS.ConnectionString))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    try
                    {
                        foreach (int valueId in valueIdArray)
                        {
                            paramValueId.Value = valueId;
                            sum += m_DatabaseS.ExecuteNonQuery(tran, CommandType.StoredProcedure, PROC_DICTIONARY_REMOVE, paramValueId);
                        }
                        tran.Commit();
                    }
                    catch { tran.Rollback(); throw; }
                }

                return sum;
            }
            catch { throw; }

        }

        private List<SqlParameter> getParameter(DictionaryValueItem dictionary)
        {
            SqlParameter paramDictionaryId = new SqlParameter(PARAM_DICTIONARYID, SqlDbType.Int);
            SqlParameter paramKey = new SqlParameter(PARAM_KEY, SqlDbType.NVarChar, 1000);
            SqlParameter paramValue = new SqlParameter(PARAM_VALUE, SqlDbType.NVarChar, 1000);
            SqlParameter paramDesc = new SqlParameter(PARAM_DESC, SqlDbType.NVarChar, 1000);
            SqlParameter paramStandbyField1 = new SqlParameter(PARAM_STANDBYFIELD1, SqlDbType.NVarChar, 1000);
            SqlParameter paramStandbyField2 = new SqlParameter(PARAM_STANDBYFIELD2, SqlDbType.NVarChar, 1000);
            SqlParameter paramStandbyField3 = new SqlParameter(PARAM_STANDBYFIELD3, SqlDbType.NVarChar, 1000);
            SqlParameter paramStandbyField4 = new SqlParameter(PARAM_STANDBYFIELD4, SqlDbType.NVarChar, 1000);
            SqlParameter paramStandbyField5 = new SqlParameter(PARAM_STANDBYFIELD5, SqlDbType.NVarChar, 1000);

            paramDictionaryId.Value = dictionary.Code;
            paramKey.Value = dictionary.Key;
            paramValue.Value = dictionary.Value;
            if (null != dictionary.Description)
                paramDesc.Value = dictionary.Description;
            else
                paramDesc.Value = DBNull.Value;

            if (null != dictionary.StandbyField1)
                paramStandbyField1.Value = dictionary.StandbyField1;
            else
                paramStandbyField1.Value = DBNull.Value;

            if (null != dictionary.StandbyField2)
                paramStandbyField2.Value = dictionary.StandbyField2;
            else
                paramStandbyField2.Value = DBNull.Value;

            if (null != dictionary.StandbyField3)
                paramStandbyField3.Value = dictionary.StandbyField3;
            else
                paramStandbyField3.Value = DBNull.Value;

            if (null != dictionary.StandbyField4)
                paramStandbyField4.Value = dictionary.StandbyField4;
            else
                paramStandbyField4.Value = DBNull.Value;

            if (null != dictionary.StandbyField5)
                paramStandbyField5.Value = dictionary.StandbyField5;
            else
                paramStandbyField5.Value = DBNull.Value;

            SqlParameter[] param = new SqlParameter[] { paramDictionaryId, paramKey, paramValue, paramDesc, paramStandbyField1, paramStandbyField2, paramStandbyField3, paramStandbyField4, paramStandbyField5 };
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddRange(param);
            return parameters;
        }

        private DictionaryValueItem distillData(SqlDataReader dataReader)
        {
            if (DBNull.Value == dataReader["FldKey"] || null == dataReader["FldKey"] || DBNull.Value == dataReader["FldKey"] || null == dataReader["FldKey"])
                return null;

            DictionaryValueItem dictionary = new DictionaryValueItem();
            dictionary.Code = Convert.ToInt32(dataReader["id"]);
            if (DBNull.Value != dataReader["FldValueID"])
                dictionary.ValueID = Convert.ToInt32(dataReader["FldValueID"]);
            dictionary.Description = dataReader["Description"] as string;
            dictionary.Key = dataReader["FldKey"] as string;
            dictionary.Value = dataReader["FldValue"] as string;
            dictionary.StandbyField1 = dataReader["FldStandbyField1"] as string;
            dictionary.StandbyField2 = dataReader["FldStandbyField2"] as string;
            dictionary.StandbyField3 = dataReader["FldStandbyField3"] as string;
            dictionary.StandbyField4 = dataReader["FldStandbyField4"] as string;
            dictionary.StandbyField5 = dataReader["FldStandbyField5"] as string;

            if (DBNull.Value != dataReader["FldAddTime"])
                dictionary.AddTime = Convert.ToDateTime(dataReader["FldAddTime"]);
            if (DBNull.Value != dataReader["FldUpdateTime"])
                dictionary.UpdateTime = Convert.ToDateTime(dataReader["FldUpdateTime"]);
            return dictionary;
        }
    }
}
