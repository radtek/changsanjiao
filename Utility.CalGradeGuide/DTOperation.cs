using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Readearth.Data;
using System.Data.SqlClient;

namespace Readearth.DB
{
    static class DTOperation
    {

        public static void DeleteMaskFile(string path, string maskstr)
        {

            if (Directory.Exists(path))
            {
                foreach (string p in Directory.GetFileSystemEntries(path))
                {
                    if (p.Contains(maskstr))
                        try
                        {
                            File.Delete(p);
                        }
                        catch
                        { ;}
                }
            }
        }


        public static DataTable CreateVoidDT(Database db,string tableName)
        {
            string sqlstr = "Select top 1 * From " + tableName;
            DataSet ds = db.GetDataset(sqlstr);
            DataTable dt = ds.Tables[0];
            dt.Clear();
            ds.Dispose();
            return dt;
        }


        public static bool InsertToDB(Database db,DataTable dt, string TableName)
        {
            using (SqlBulkCopy sqlBC = new SqlBulkCopy(db.ConnectionString))
            {
                try
                {
                    sqlBC.BatchSize = 1000;
                    sqlBC.BulkCopyTimeout = 60;
                    //设置要批量写入的表
                    sqlBC.DestinationTableName = TableName;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string columname = dt.Columns[i].ColumnName;
                        sqlBC.ColumnMappings.Add(columname, columname);
                    }
                    sqlBC.WriteToServer(dt);
                    sqlBC.Close();
                    dt.Dispose();
                    return true;
                }
                catch (Exception)
                {
                    sqlBC.Close();
                    dt.Dispose();
                    throw;
                }
            }
        }
    }
}
