using System;

namespace MMShareBLL.Model
{
    public class DownContext
    {
        private int id;
        private int category;
        private string text = null;
        private string imgsavepath;
        private DateTime releasetime;
        private int taskid;
        private bool removed;

        private DownTask task;

        public DownTask Task
        {
            set { task = value; }
            get { return task; }
        }

        public int ID
        {
            set { id = value; }
            get { return id; }
        }

        public int Category
        {
            set { category = value; }
            get { return category; }
        }

        public string Text
        {
            set { text = value; }
            get { return text; }
        }

        public string ImgSavePath
        {
            set { imgsavepath = value; }
            get { return imgsavepath; }
        }

        public DateTime ReleaseTime
        {
            set { releasetime = value; }
            get { return releasetime; }
        }

        public int TaskID
        {
            set { taskid = value; }
            get { return taskid; }
        }

        public bool Removed
        {
            set { removed = value; }
            get { return removed; }
        }

        public string TaskName { get { if (null != task) return task.TaskName; else return ""; } }

        public DownContext Clone()
        {
            DownContext newModel = new DownContext();
            newModel.category = this.category;
            newModel.id = this.id;
            newModel.imgsavepath = this.imgsavepath;
            newModel.releasetime = this.releasetime;
            newModel.removed = this.removed;
            if (null != this.task)
                newModel.task = this.task.Clone();
            newModel.taskid = this.taskid;
            newModel.text = this.text;
            return newModel;
        }
    }
}

