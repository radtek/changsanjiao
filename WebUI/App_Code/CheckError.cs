using System;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;


namespace Utility1.GradeGuide1
{
    class CheckError1
    {
        /// <summary>
        /// ��������Ϣд����־�ļ���ÿ��һ��������־��
        /// </summary>
        /// <param name="message">������Ϣ</param>
        private string filePath;
        public CheckError1()
        {
        }
        public string Month(DataTable dataTabledt)
        {
            string message = "";
            for (int i = 1; i <dataTabledt.Rows.Count; i++)
            {
                if (dataTabledt.Rows[i - 1][2] == dataTabledt.Rows[i][2] &&
                    dataTabledt.Rows[i - 1][3] == dataTabledt.Rows[i][3] &&
                    dataTabledt.Rows[i - 1][4] == dataTabledt.Rows[i][4])
                {
                    string str1 = dataTabledt.Rows[i - 1][1].ToString();
                    string str2 = dataTabledt.Rows[i][1].ToString();
                    if (Compare(str1,str2))
                    {
                        message +="��"+ i +"��"+"�·�����"+dataTabledt.Rows[i - 1][1] + "/////" + dataTabledt.Rows[i][1];
                       // Messagewrite(message, "�·�����", "�·�����");
                    }
                }               
            }
            return message;
        }
        /// <summary>
        /// �����ж��·��Ƿ��ظ�
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public bool Compare(string s1, string s2)
        {
            string[] str = s1.Split(',');
            string[] str1 = s2.Split(',');
            bool panduan = false;
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j <str1.Length  ; j++)
                {
                    if (str[i] == str1[j])
                    {
                        panduan = true;
                    }
                }               
            }
            return panduan;
        }

        #region �жϷ�Χ����

        /// <summary>
        /// ��ֵ��Χ�Լ����ŵ��ж�
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="_type"></param>
        public void Range(DataTable dataTable)
        {
            const string _type = "��ֵ��Χ����";
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                var priorup = dataTable.Rows[i - 1][3].ToString(); //ǰһ��������
                var priordown = dataTable.Rows[i - 1][4].ToString(); //ǰһ��������
                var forwardup = dataTable.Rows[i][3].ToString();
                var forwarddown = dataTable.Rows[i][4].ToString();
                //���������ȣ�ֱ�ӽ����ж�
                if (dataTable.Rows[i][5]==dataTable.Rows[i - 1][5])
                {
                    var message = "";
                    if (priordown.Contains("9999"))
                    {
                        if (Convert.ToString(Regex.Match(priorup, @"\d+")) !=Convert.ToString(Regex.Match(forwarddown, @"\d+")))
                        {
                            message = dataTable.Rows[i][1] + "////" + dataTable.Rows[i - 1][1];
                            Messagewrite(message, _type, "��ֵ����");
                        }
                        else
                        {
                            if (priorup.Contains("[") && forwarddown.Contains("]") || !priorup.Contains("[")&&!forwarddown.Contains("]"))
                            {
                                message = dataTable.Rows[i][1] + "////" + dataTable.Rows[i - 1][1];
                                Messagewrite(message, _type, "��������");
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(Regex.Match(priordown, @"\d+")) !=Convert.ToString(Regex.Match(forwardup, @"\d+")))
                        {
                            if (Convert.ToString(Regex.Match(priorup, @"\d+")) !=
                                Convert.ToString(Regex.Match(forwarddown, @"\d+")))
                            {
                                message = dataTable.Rows[i][1] + "////" + dataTable.Rows[i - 1][1];
                                Messagewrite(message, _type, "��ֵ����");
                            }
                            else
                            {
                                if (forwarddown.Contains("[") && priorup.Contains("]"))
                                {
                                    message = dataTable.Rows[i][1] + "////" + dataTable.Rows[i - 1][1];
                                    Messagewrite(message, _type, "��������");
                                }
                            } 
                        }
                        else
                        {
                            {
                                if (priordown.Contains("[") && forwardup.Contains("]"))
                                {
                                    message = dataTable.Rows[i][1] + "////" + dataTable.Rows[i - 1][1];
                                    Messagewrite(message, _type, "��������");
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// ��ֵ��Χ�Լ����ŵ��ж�//�򵥵ķ�Χ�ж�
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="_type"></param>
        public string Range1(DataTable dataTable)
        {
            var message = "";
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                var priorup = dataTable.Rows[i - 1][3].ToString(); //ǰһ��������
                var priordown = dataTable.Rows[i - 1][4].ToString(); //ǰһ��������
                var forwardup = dataTable.Rows[i][3].ToString();
                var forwarddown = dataTable.Rows[i][4].ToString();
                //���������ȣ�ֱ�ӽ����ж�
                if (dataTable.Rows[i][5] == dataTable.Rows[i - 1][5])
                {
                    if (priorup != forwardup || priordown != forwarddown)
                    {
                        if (Regex.Match(priorup, @"\d+").ToString() == Regex.Match(forwarddown, @"\d+").ToString() ||
                            Regex.Match(priordown, @"\d+").ToString() == Regex.Match(forwardup, @"\d+").ToString())
                        {
                            if (priordown.Contains("9999") || forwardup.Contains("9999"))
                            {
                                if (priorup.Contains("[") && forwarddown.Contains("]") ||
                                    !priorup.Contains("[") && !forwarddown.Contains("]"))
                                {
                                    message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                                    //Messagewrite(message, _type, "��������");
                                }
                            }
                            else if (forwarddown.Contains("9999") || priorup.Contains("9999"))
                            {
                                if (priordown.Contains("]") && forwardup.Contains("[") ||
                                    !priordown.Contains("]") && !forwardup.Contains("["))
                                {
                                    message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                                    // Messagewrite(message, _type, "��������");
                                }
                            }
                            else
                            {
                                if (!priorup.Contains("[") && !forwarddown.Contains("]") ||
                                    !priordown.Contains("]") && !forwardup.Contains("["))
                                {
                                    message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                                    //Messagewrite(message, _type, "��������");
                                }
                                if (Regex.Match(priorup, @"\d+").ToString() == Regex.Match(forwarddown, @"\d+").ToString() && priorup.Contains("[") && forwarddown.Contains("]"))
                                {
                                    message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                                }
                                else if (Regex.Match(priordown, @"\d+").ToString() == Regex.Match(forwardup, @"\d+").ToString() && priordown.Contains("]") && forwardup.Contains("["))
                                {
                                    message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                                }
                            }
                        }

                        else
                            message += "\t\n��������:" + dataTable.Rows[i][1] + "---" + dataTable.Rows[i - 1][1];
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// �����д�뵽��־�ļ�
        /// </summary>
        /// <param name="_message"></param>
        /// <param name="logType"></param>
        /// <param name="type"></param>
        private void Messagewrite(string _message, string logType, string type)
        {
            string folder = filePath + "\\" + logType;
            string log = folder + "\\" + logType + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //��鲢�����ļ���
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //��鲢����������־
            if (!File.Exists(log))
            {
                File.Create(log).Close();
            }
            try
            {
                //Thread.Sleep(2000);
                //��������Ϣд����־
                using (StreamWriter sw = File.AppendText(log))
                {
                    sw.WriteLine("Time: {0}+{1}", DateTime.Now.ToString(), type);
                    sw.WriteLine(_message);
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                //    count++;
                //    while (count > 3) return;
                //    //��������ļ������ͻ������ִ�д˺�����
                //    WriteToLog(_message, logType);
                //
            }
        }

     
    }
}


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 