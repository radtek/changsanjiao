using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using System.Collections.Generic;
using Readearth.Data;
using MMShareBLL.Model;

namespace MMShareBLL.DAL
{
    /// <summary>
    /// 提供对于污染等级的数据库访问
    /// </summary>
    public class PolluteLevelData
    {

        //Procedures
        private const string PROC_QUERYALL = "UP_TblPolluteLevel_QueryAll";
        private const string PROC_QUERYBYAPI = "UP_TblPolluteLevel_QueryByAPI";

        //Parameters
        private const string PARAM_API = "@api";  //float
         private Database m_DatabaseS;
         public PolluteLevelData()
         {
            m_DatabaseS = new Database("SEMCDMC");
        }
        /// <summary>
        /// 获得所有的污染等级列表
        /// </summary>
        /// <returns></returns>
         public List<PolluteLevel> Query()
        {
            List<PolluteLevel> reList = new List<PolluteLevel>();
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader(PROC_QUERYALL))
            {
                while (sdr.Read())
                {
                    PolluteLevel model = distillData(sdr);
                    reList.Add(model);
                }
            }
            return reList;
        }

        /// <summary>
        /// 传入指定API值，获取其污染等级。
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public PolluteLevel Query(float api)
        {
            SqlParameter paramAPI = new SqlParameter(PARAM_API, SqlDbType.Float);
            paramAPI.Value = api;
            PolluteLevel model = null;
            using (SqlDataReader sdr = m_DatabaseS.GetDataReader( PROC_QUERYBYAPI, paramAPI))
            {
                if (sdr.Read())
                    model = distillData(sdr);
            }
            return model;
        }

        private PolluteLevel distillData(SqlDataReader sdr)
        {
            PolluteLevel model = new PolluteLevel();
            model.LevelID = Convert.ToInt32(sdr["fldLevelID"]);
            model.StartRegion = float.Parse(sdr["fldStartRegion"].ToString());
            model.EndRegion = float.Parse(sdr["fldEndRegion"].ToString());
            model.Name = sdr["fldName"] as string;
            model.Symbol = sdr["fldSymbol"] as string;
            model.Grade = sdr["fldGrade"] as string;
            model.HealthImpact = sdr["fldHealthImpact"] as string;
            model.Measure = sdr["fldMeasure"] as string;
            model.ColorName = sdr["fldColorName"] as string;
            model.ColorValue = sdr["fldColorValue"] as string;
            return model;
        }
    }
}

