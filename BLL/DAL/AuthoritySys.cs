using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Readearth.Data;
using System.Web;

namespace MMShareBLL.DAL
{
    public class AuthoritySys
    {
        public static Database m_Database;
        public static string _sql = @"select a.ModuleName+'-'+a.EntityName as menu5,b.Hint as menu5C,a.MenuName as menu4C,a.Class as menu3C,a.OrderID as order1,
            a.ModuleName as menu2,replace(c.childModuleCName,'</br>','') as menu2C,c.orderID as order2,c.ModuleName as menu1,d.ModuleCName as menu1C,a.EntityName as tr
            from T_ImageProduct2 a,T_Entity b,T_ModuleChild c,T_Module d where a.EntityName =b.EntityName and 
            a.ModuleName =c.childModuleName and c.ModuleName =d.ModuleName ";

        public string ShowUser()
        {
            Database thDB = new Database("DBCONFIGII");
            string strSQl = "select Alias,UserName from T_user";
            DataTable alluser = thDB.GetDataTable(strSQl);
            return DataTableToJson1("data", alluser);

        }


        private string DataTableToJson1(string jsonName, System.Data.DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        public void EditAuthority(string name, string authorities)
        {
            string authority = HttpUtility.UrlDecode(authorities);
            string[] arr = authorities.Split('/');
            string del = "delete from T_UserAuthority where userName='" + name + "'";
            string insert_sql = "insert into T_UserAuthority(username,masterModuleName,childModuleName,entityName) values ";
            m_Database.Execute(del);  //先删掉，再插入
            for (int i = 0; i < arr.Length; i++)
            {
                try
                {
                    string[] auth = arr[i].Split('-');
                    string values = "('" + name + "','" + auth[0] + "','" + auth[1] + "','" + auth[auth.Length - 1] + "')";
                    //if (auth.Length == 5) { 
                    //    values = "('" + name + "','" + auth[0] + "','" + auth[1] + "','" + auth[4] + "')";
                    //}
                    m_Database.Execute(insert_sql + values);
                }
                catch { }
            }
        }

        public string ShowReadOnlyTree(string UserName)
        {
            m_Database = new Database("DBCONFIGII");
            string sql = @"select a.ModuleName+'-'+a.EntityName as menu5,REPLACE(b.Hint, CHAR(13) + CHAR(10), '') as menu5C,a.MenuName as menu4C,a.Class as menu3C,a.OrderID as order1,
            a.ModuleName as menu2,replace(c.childModuleCName,'</br>','') as menu2C,c.orderID as order2,c.ModuleName as menu1,d.ModuleCName as menu1C,a.EntityName as tr
            from T_ImageProduct2 a,T_Entity b,T_ModuleChild c,T_Module d where a.EntityName =b.EntityName and 
            a.ModuleName =c.childModuleName and c.ModuleName =d.ModuleName ";
            DataTable dt = m_Database.GetDataTable(sql);
            //DataRow row=dt.NewRow();
            //row["menu1C"] = "交互分析";
            //dt.Rows.Add(row);
            return DataTableToJson(dt, UserName);
        }

        public string DataTableToJson(DataTable dt, string user)
        {
            StringBuilder json = new StringBuilder();
            List<string> listAuth = new List<string>();
            //如果用户不为空，则获取相应的权限
            if (user != "") { listAuth = GetAuthority(user, dt); }
            json.Append("{\"total\":" + dt.Rows.Count + ",\"rows\":[");
            #region  1级菜单  写死了
            string sql = "select * from T_Module";
            DataTable dTable = m_Database.GetDataTable(sql);
            for (int j = 0; j < dTable.Rows.Count; j++)
            {
                //1级菜单，如果是webgis，因为交互分析只有一级菜单，没有二级.
                if (j == 1)
                {
                    //如果获取的权限中包含webGis，则说明该用户有这个权限，需要勾选该权限
                    if (listAuth.Contains("webGIS-webGIS-webGIS"))
                    {
                        json.Append("{\"id\":\"" + dTable.Rows[j][2] + "\",\"name\":\"" + dTable.Rows[j][1] + "\",\"iconCls\":\"icon-function\",\"checked\":\"true\"},");
                    }
                    else
                    {
                        json.Append("{\"id\":\"" + dTable.Rows[j][2] + "\",\"name\":\"" + dTable.Rows[j][1] + "\",\"iconCls\":\"icon-function\"},");
                    }
                }//其他的则正常
                else
                {
                    json.Append("{\"id\":\"" + dTable.Rows[j][2] + "\",\"name\":\"" + dTable.Rows[j][1] + "\",\"iconCls\":\"icon-function\"},");
                }
            }
            #endregion  1级菜单结束

            //通过循环获取2，3，4，5等级的菜单，i表示相对应的等级
            for (int i = 2; i <= 5; i++)
            {
                GetLevel(dt, i, json, user, listAuth);
            }
            json.Append("]}");
            string Json = json.ToString();
            return Json.Remove(json.Length - 3, 1);
        }
        //获取菜单
        public StringBuilder GetLevel(DataTable dt, int levels, StringBuilder sb, string userName, List<string> listAuth)
        {
            List<string> list = new List<string>();
            string menu1 = "", menu2 = "", menu3C = "", menu4C = "", menu5 = "";
            string menu = "";
            string val_Id = "";
            string name = "";
            string parentId = "";
            string compare = "";  //与获取到的权限作比较
            string check = "";  //储存是否勾选checked为true
            switch (levels)
            {
                case 1: menu = "menu1"; break;
                case 2: menu = "menu2"; break;
                case 3: menu = "menu3C"; break;
                case 4: menu = "menu4C"; break;
                case 5: menu = "tr"; break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        menu1 = dt.Rows[i]["menu1"].ToString();
                        menu2 = dt.Rows[i]["menu2"].ToString();
                        menu3C = dt.Rows[i]["menu3C"].ToString();
                        if (menu3C == "CFS预报产品(50天)")
                        {   //特殊处理，前段如果不去掉“(50天)”，则显示有问题
                            menu3C = menu3C.Replace("(50天)", "");
                        }
                        menu4C = dt.Rows[i]["menu4C"].ToString();
                        compare = menu1 + "-" + menu2 + "-" + dt.Rows[i]["tr"].ToString();
                        //2级菜单
                        if (levels == 2)
                        {
                            val_Id = menu1 + "-" + dt.Rows[i][menu].ToString();
                            parentId = menu1;
                            name = dt.Rows[i]["menu2C"].ToString();
                        }
                        //3级菜单
                        else if (levels == 3)
                        {
                            string str = dt.Rows[i][menu].ToString();
                            //同上
                            if (str == "CFS预报产品(50天)")
                            {
                                str = str.Replace("(50天)", "");
                            }
                            val_Id = menu1 + "-" + menu2 + "-" + str;
                            parentId = menu1 + "-" + menu2;
                            name = menu3C;
                        }
                        //4级菜单
                        else if (levels == 4)
                        {
                            parentId = menu1 + "-" + menu2 + "-" + menu3C;
                            if (menu4C != "")
                            {
                                val_Id = menu1 + "-" + menu2 + "-" + menu3C + "-" + dt.Rows[i][menu].ToString();
                                name = menu4C;
                            }
                            //如果menu4C为空，表示只有四级菜单，menu5C表示四级菜单
                            else if (menu4C == "")
                            {
                                val_Id = menu1 + "-" + menu2 + "-" + menu3C + "-" + dt.Rows[i]["tr"].ToString();
                                name = dt.Rows[i]["menu5C"].ToString();
                                if (listAuth.Count > 0)
                                {
                                    //四级菜单和5级菜单是最低的因此需要比较是否有改权限
                                    if (listAuth.Contains(compare))
                                    {
                                        check = "\"checked\":\"true\"";
                                    }
                                }
                            }

                        }
                        else if (levels == 5)
                        {
                            //不等于空则说明有5级菜单
                            if (menu4C != "")
                            {
                                val_Id = menu1 + "-" + menu2 + "-" + menu3C + "-" + menu4C + "-" + dt.Rows[i][menu].ToString();
                                parentId = menu1 + "-" + menu2 + "-" + menu3C + "-" + menu4C;
                                name = dt.Rows[i]["menu5C"].ToString();
                                if (listAuth.Count > 0)
                                {
                                    if (listAuth.Contains(compare))
                                    {
                                        check = "\"checked\":\"true\"";
                                    }
                                }
                            }
                        }
                        if (!list.Contains(val_Id))
                        {
                            list.Add(val_Id);
                            //最底层的菜单
                            if ((menu4C == "" && levels == 4) || (menu4C != "" && levels == 5))
                            {
                                if (check != "")
                                {
                                    sb.Append("{\"id\":\"" + val_Id + "\",\"name\":\"" + name + "\",\"iconCls\":\"icon-function\",\"_parentId\":\"" + parentId + "\"," + check + "},");
                                    check = "";
                                }
                                else
                                {
                                    sb.Append("{\"id\":\"" + val_Id + "\",\"name\":\"" + name + "\",\"iconCls\":\"icon-function\",\"_parentId\":\"" + parentId + "\"},");
                                }
                            }
                            else
                            {
                                sb.Append("{\"id\":\"" + val_Id + "\",\"name\":\"" + name + "\",\"state\":\"closed\",\"iconCls\":\"icon-function\",\"_parentId\":\"" + parentId + "\"},");
                            }

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            return sb;
        }
        //获取权限
        public List<string> GetAuthority(string userName, DataTable dTable)
        {
            string sql = "select masterModuleName,childModuleName,entityname from T_UserAuthority where username='" + userName + "'";
            DataTable dt = m_Database.GetDataTable(sql);
            string condition = "";
            string auth = "";
            List<string> authorities = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //webGis要单独处理，因为是一级菜单
                        if (dt.Rows[i][0].ToString() == "webGIS")
                        {
                            authorities.Add("webGIS-webGIS-webGIS");
                        }
                        else
                        {
                            condition = "menu1='" + dt.Rows[i][0] + "' and menu2='" + dt.Rows[i][1] + "' and tr='" + dt.Rows[i][2] + "'";
                            DataRow[] dr = dTable.Select(condition);
                            auth = dr[0]["menu1"].ToString() + "-" + dr[0]["menu2"].ToString() + "-" + dr[0]["tr"].ToString();
                            authorities.Add(auth);
                        }
                    }
                    catch (Exception) { }
                }
            }
            return authorities;

        }
    }
}