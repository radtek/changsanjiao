using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public class DownTask
    {
        private int _taskid;
        private DateTime? _accepttime = null;
        private DateTime? _finishtime = null;
        private string _dealinfo;
        private int? _trytimes = null;
        private int? _status = null;
        private int? _category = null;
        private string _taskname;
        private string _taskurl;
        private int? _tasktype = null;
        private string _imgsavepath;
        private string _contenttext;
        private DateTime? _needdowntime = null;
        private DateTime? _createtime = null;


        public int TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }

        /// <summary>
        /// 任务接受时间
        /// </summary>
        public DateTime? Accepttime
        {
            set { _accepttime = value; }
            get { return _accepttime; }
        }
        /// <summary>
        /// 任务完成时间
        /// </summary>
        public DateTime? Finishtime
        {
            set { _finishtime = value; }
            get { return _finishtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Dealinfo
        {
            set { _dealinfo = value; }
            get { return _dealinfo; }
        }
        /// <summary>
        /// 尝试次数
        /// </summary>
        public int? Trytimes
        {
            set { _trytimes = value; }
            get { return _trytimes; }
        }
        /// <summary>
        /// 完成状态，0：初始1：成功2：失败
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 对应字典表id
        /// </summary>
        public int? Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskName
        {
            set { _taskname = value; }
            get { return _taskname; }
        }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string TaskURL
        {
            set { _taskurl = value; }
            get { return _taskurl; }
        }

        /// <summary>
        /// 类型，分为1：图片，2：文本
        /// </summary>
        public int? TaskType
        {
            set { _tasktype = value; }
            get { return _tasktype; }
        }

        /// <summary>
        /// 图片或文件保存路径
        /// </summary>
        public string ImgSavePath
        {
            set { _imgsavepath = value; }
            get { return _imgsavepath; }
        }

        /// <summary>
        /// 文本内容，XML格式
        /// </summary>
        public string ContentText
        {
            set { _contenttext = value; }
            get { return _contenttext; }
        }

        /// <summary>
        /// 执行任务时间，主要看时间
        /// </summary>
        public DateTime? NeedDownTime
        {
            set { _needdowntime = value; }
            get { return _needdowntime; }
        }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime? Createtime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public DownTask Clone()
        {
            DownTask newModel = new DownTask();
            newModel._taskid = this._taskid;
            newModel._accepttime = this._accepttime;
            newModel._finishtime = this._finishtime;
            newModel._dealinfo = this._dealinfo;
            newModel._trytimes = this._trytimes;
            newModel._status = this._status;
            newModel._category = this._category;
            newModel._taskname = this._taskname;
            newModel._taskurl = this._taskurl;
            newModel._tasktype = this._tasktype;
            newModel._imgsavepath = this._imgsavepath;
            newModel._contenttext = this._contenttext;
            newModel._needdowntime = this._needdowntime;
            newModel._createtime = this._createtime;
            return newModel;
        }
    }
}
