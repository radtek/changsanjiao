using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using MMShareBLL.Model;


namespace MMShareBLL.DAL
{
    /// <summary>
    /// 为数据访问层提供公共方法
    /// </summary>
    public class Pub
    {
        /// <summary>
        /// 判断SqlDataReader中是否存在某一列
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ExistsColumn(SqlDataReader reader, string column)
        {
            if (string.IsNullOrEmpty(column))
                return false;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (column.ToLower() == reader.GetName(i).ToLower() && reader[column] != DBNull.Value)
                    return true;
            }
            return false;
        }

        public static void CheckParams(IList<DataParameter> parameters, SqlDataReader reader)
        {
            IList<DataParameter> checkedParams = new List<DataParameter>();
            foreach (DataParameter param in parameters)
            {
                if (Pub.ExistsColumn(reader, param.DataColumn))
                {
                    checkedParams.Add(param.Clone());
                }
            }
            parameters = checkedParams;
        }

        public static float[] ParseToArray(string strData)
        {
            try
            {
                string[] strArray = strData.Split(new char[] { ',' });
                float[] convrtTo = new float[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    convrtTo[i] = float.Parse(strArray[i].Replace("f", ""));
                }
                return convrtTo;
            }
            catch { return null; }
        }
    }
}
