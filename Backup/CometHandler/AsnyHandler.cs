using System;
using System.Collections.Generic;
using System.Text;

using System.Web;

namespace CometHandler
{

    /// <summary>
    /// 异步IHttpHandler，用于实现预报会商，会商内容的即时展现
    /// 作者：张伟锋         日期：2013年-07-28日
    /// </summary>
    public class AsnyHandler : IHttpAsyncHandler
    {
        #region IHttpAsyncHandler 成员

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            //myAsynResult为实现了IAsyncResult接口的类，当不调用cb的回调函数时，该请求不会返回到给客户端，会一直处于连接状态
            MyAsyncResult asyncResult = new MyAsyncResult(context, cb, extraData);
            String content = context.Request.Params["content"];

            //向Message类中添加该消息
            Messages.Instance().AddMessage(content, asyncResult);
            return asyncResult;

        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }

        #endregion

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; ; }
        }

        public void ProcessRequest(HttpContext context)
        {
        }

        #endregion
    }


    /// <summary>
    /// 自定义IAsyncResult 实现我们额外的异步方法
    /// </summary>
    public class MyAsyncResult : IAsyncResult
    {
        //回调参数
        private object _param;
        //是否异步执行完成
        private bool _asyncIsComplete;
        //回调方法
        private AsyncCallback _callBack;
        //当前上下文
        private HttpContext _context;

        public MyAsyncResult(HttpContext context,AsyncCallback callBack, object stateParam )
        {
            this._callBack = callBack;
            this._param = stateParam;
            _asyncIsComplete = false;
            this._context = context;
        }


        public object AsyncState
        {
            get { return this._param; }
        }

        /// <summary>
        /// 等待句柄用于同步功能，关于等待句柄会在后续章节陈述
        /// </summary>
        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 该属性表示不需要异步任务同步完成
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return false; }
        }
        /// <summary>
        /// 该属性表示异步任务是否已经执行完成
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return this._asyncIsComplete;
            }
        }

        /// <summary>
        /// 自定义的额外功能,需要注意的是，执行完异步功能后
        /// 要将_asyncIsComplete设置为true表示任务执行完毕而且
        /// 执行回调方法，否则异步工作无法结束页面会卡死
        /// </summary>
        public void Write(string content)
        {
            //这里先不用waitHandle句柄来实现同步
            lock (this)
            {
                if (this.AsyncState!= null)
                {
                    this._context.Response.Write(content);
                    this._asyncIsComplete = true;
                    this._callBack(this);
                }
            }
        }

    }
}
