using System;
using System.Collections.Generic;
using System.Web;

namespace CometHandler
{

    /// <summary>
    /// Summary description for Messages
    /// </summary>
    public class Messages
    {
        //��¼��������Ŀͻ���
        List<MyAsyncResult> clients = new List<MyAsyncResult>();

        #region ʵ�ָ���ĵ���
        private static readonly Messages _Instance = new Messages();
        private Messages()
        {
        }
        public static Messages Instance()
        {
            return _Instance;
        }
        #endregion

        public void AddMessage(string content, MyAsyncResult asyncResult)
        {
            //�����������Ϊ"-1"ʱ����ʾΪ�����������󣬼�Ϊ��ά��һ���ӿͻ��˵������������Ӷ�����������
            //��ʱ�������ӱ��浽 List<myAsynResult> clients�У���������Ϣ���͹���ʱ�������ӽ��ᱻ���������һὫ������������ݺ󣬽���������
            if (content == "-1")
            {
                clients.Add(asyncResult);
            }
            else
            {
                //����ǰ���������������ͻ���
                //asyncResult.Content = content;
                //asyncResult.Send(null);

                //���򽫱��������ѻ����client,������ǰ����������ͻ���
                foreach (MyAsyncResult result in clients)
                {
                    result.Write(content);
                }

                //������л���
                clients.Clear();
            }
        }
    }
}
