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
        //记录所有请求的客户端
        List<MyAsyncResult> clients = new List<MyAsyncResult>();

        #region 实现该类的单例
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
            //当传入的内容为"-1"时，表示为建立连接请求，即为了维持一个从客户端到服务器的连接而建立的连接
            //此时将该连接保存到 List<myAsynResult> clients中，待再有消息发送过来时，该连接将会被遍历，并且会将该连接输出内容后，结束该连接
            if (content == "-1")
            {
                clients.Add(asyncResult);
            }
            else
            {
                //将当前请求的内容输出到客户端
                //asyncResult.Content = content;
                //asyncResult.Send(null);

                //否则将遍历所有已缓存的client,并将当前内容输出到客户端
                foreach (MyAsyncResult result in clients)
                {
                    result.Write(content);
                }

                //清空所有缓存
                clients.Clear();
            }
        }
    }
}
