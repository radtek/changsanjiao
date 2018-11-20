using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.Model
{
    public class AQIDictionary
    {
        private int code;
        private string description;
        private string tooltip;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }
    }
}
