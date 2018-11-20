#region  System.Runtime.Serialization.Json
/*
 * ============================================================== 
 * 在 .NET 3.5 之后，定义在命名空间 System.Runtime.Serialization.Json 中的 DataContractJsonSerializer 可以帮助我们直接将一个对象格式化成 JSON，
 * 或者将一个 JSON 反序列化为一个 .NET 中的对象实例。
 *  System.Type type = typeof( Result );  
 *        System.Runtime.Serialization.Json.DataContractJsonSerializer serializer 
 *               = new System.Runtime.Serialization.
 *   Json.DataContractJsonSerializer(type); 
 *
 */
#endregion

using System;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
/// <summary>
/// JsonHelper 的摘要说明
/// </summary>
namespace ExtHandler
{
    public sealed class JsonHelper
    {
        /// <summary>
        /// 将对象转化为JSON数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj)
        {
            string josnData = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(obj); 
            if (obj == null) {
                return "";
            }
            //如果为DataTable对象
            if (obj.GetType() == typeof(System.Data.DataTable))
            {
                DataTable dTable = (DataTable)obj;
                josnData = JsonConvert.SerializeObject(obj, new DataTableConverter());
                //JsonConvert.DeserializeObject(josnData,new DataTableConverter());
                //需要返回总的记录数的话
                if (dTable.TableName.Contains("data:{0}"))
                {
                    josnData ="{" + string.Format(dTable.TableName, josnData) + "}" ;
                }
                return josnData;
            }
            else if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }
            else if (obj.GetType() == typeof(DataSet))
            {
                //DataSet ds = (DataSet)obj;
                //for (int i = 0; i < ds.Tables.Count - 1; i++)
                //{
                //    DataTable dTable = ds.Tables[i];
                    josnData = JsonConvert.SerializeObject(obj, new DataTableConverter());
                    return josnData;
                //}
            }
            return JsonConvert.SerializeObject(obj);
        }
    }
}
