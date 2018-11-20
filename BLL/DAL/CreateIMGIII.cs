using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Readearth.Data;
using System.Data;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace MMShareBLL.DAL
{
    class CreateIMGIII
    {
        Dictionary<string, string> m_AreaInfo; Dictionary<string, string> m_AreaData; DateTime m_Time; string m_Filename; string m_Basepath; string m_pubTime; string type;
        public CreateIMGIII(Dictionary<string, string> AreaData, DateTime time, string filename, string basepath,string pt,string type)
        {
            this.m_AreaData = AreaData;
            this.m_Time = time;
            this.m_pubTime=pt;
            this.m_Filename = filename;
            this.m_Basepath = basepath;
            this.type = type;
             m_AreaInfo = new System.Collections.Generic.Dictionary<string, string>();
             m_AreaInfo.Add("闵行区", "325;539");
             m_AreaInfo.Add("青浦区", "110;521");
             m_AreaInfo.Add("松江区", "211;603");
             m_AreaInfo.Add("金山区", "191;748");
             m_AreaInfo.Add("中心城区", "372;475");
             m_AreaInfo.Add("奉贤区", "396;695");
             m_AreaInfo.Add("浦东新区", "450;448");
             m_AreaInfo.Add("崇明县", "268;138");
             m_AreaInfo.Add("宝山区", "352;372");
             m_AreaInfo.Add("嘉定区", "245;380");
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AreaData"></param>
        /// <param name="time">发布时间</param>
        /// <param name="filename">上传图片名 </param>
        /// <param name="basepath">服务器路径</param>
        /// <returns></returns>
       public string DealData()
        {
            try
            {
                string path = string.Format("{0}/{1}/{2}/", m_Basepath, m_Time.ToString("yyyy"), m_Time.ToString("yyyyMMddHH"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string exportImageName = path + m_Filename;
                string basemap = string.Format("{0}/{1}", m_Basepath, "sh.jpg");
                string districtmap = string.Format("{0}/{1}", m_Basepath, "map-2.png");
                Dictionary<string, string> AreaPath = GetAreapngs(m_AreaData);

                Bitmap bitmapSource = new Bitmap(833, 1111);
                Graphics resultGraphics;    //用来绘图的实例   
                resultGraphics = Graphics.FromImage(bitmapSource);
               // resultGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                //绘制背景
                Image imgPart = Image.FromFile(basemap);
                resultGraphics.DrawImage(imgPart, 0, 0, imgPart.Width, imgPart.Height);

                //Bitmap image = new Bitmap(basemap);
                //Graphics resultGraphics = Graphics.FromImage(image);
                foreach (var dic in AreaPath)
                {

                    string pngpath = dic.Value;
                    string area = dic.Key;
                   // Bitmap bitmap = new Bitmap(pngpath);
                    imgPart = Image.FromFile(pngpath);
                    resultGraphics.DrawImage(imgPart, Convert.ToInt32(m_AreaInfo[area].Split(';')[0]), Convert.ToInt32(m_AreaInfo[area].Split(';')[1]), imgPart.Width, imgPart.Height);
                    //AreaPath[area] = datapath;
                }
                imgPart = Image.FromFile(districtmap);
                resultGraphics.DrawImage(imgPart, 0, 0, imgPart.Width, imgPart.Height);
                int yy = m_Time.Year;
                int mm = m_Time.Month;
                int dd = m_Time.Day;
                int hh = m_Time.Hour;
                string title = "";
                string retime = "";
                title = "预报时间:" + DateTime.Parse(m_pubTime).ToString("yyyy") + "年" + 
                    DateTime.Parse(m_pubTime).ToString("MM") + "月" + DateTime.Parse(m_pubTime).ToString("dd") + "日";

                SetTextElement(title, 36, 80, 19, bitmapSource, "宋体", Color.Black);
                SetTextElement(this.type, 30, 20, 30, bitmapSource, "楷体", Color.Blue);


                retime = "长三角环境气象预报预警中心" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日 " + hh.ToString() + "时发布";
                SetTextElement(retime, 72, 1060, 20, bitmapSource, "宋体", Color.Black);
                bitmapSource.Save(exportImageName,ImageFormat.Gif);
                return exportImageName;
            }
            catch
            {
                return "";
            }

        }


       public string DealDataII()
       {
           try
           {
               string path = string.Format("{0}/{1}/{2}/", m_Basepath, m_Time.ToString("yyyy"), m_Time.ToString("yyyyMMddHH"));
               if (!Directory.Exists(path))
                   Directory.CreateDirectory(path);

               string exportImageName = path + m_Filename;
               string basemap = string.Format("{0}/{1}", m_Basepath, "shII.jpg");
               string districtmap = string.Format("{0}/{1}", m_Basepath, "map-2.png");
               Dictionary<string, string> AreaPath = GetAreapngs(m_AreaData);

               Bitmap bitmapSource = new Bitmap(833, 1111);
               Graphics resultGraphics;    //用来绘图的实例   
               resultGraphics = Graphics.FromImage(bitmapSource);
               // resultGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
               //绘制背景
               Image imgPart = Image.FromFile(basemap);
               resultGraphics.DrawImage(imgPart, 0, 0, imgPart.Width, imgPart.Height);

               //Bitmap image = new Bitmap(basemap);
               //Graphics resultGraphics = Graphics.FromImage(image);
               foreach (var dic in AreaPath)
               {

                   string pngpath = dic.Value;
                   string area = dic.Key;
                   // Bitmap bitmap = new Bitmap(pngpath);
                   imgPart = Image.FromFile(pngpath);
                   resultGraphics.DrawImage(imgPart, Convert.ToInt32(m_AreaInfo[area].Split(';')[0]), Convert.ToInt32(m_AreaInfo[area].Split(';')[1]), imgPart.Width, imgPart.Height);
                   //AreaPath[area] = datapath;
               }
               imgPart = Image.FromFile(districtmap);
               resultGraphics.DrawImage(imgPart, 0, 0, imgPart.Width, imgPart.Height);
               int yy = m_Time.Year;
               int mm = m_Time.Month;
               int dd = m_Time.Day;
               int hh = m_Time.Hour;
               string title = "";
               string retime = "";
               title = "预报时间:" + DateTime.Parse(m_pubTime).ToString("yyyy") + "年" +
                   DateTime.Parse(m_pubTime).ToString("MM") + "月" + DateTime.Parse(m_pubTime).ToString("dd") + "日";

               SetTextElement(title, 36, 80, 19, bitmapSource, "宋体", Color.Black);
               SetTextElement(this.type, 30, 20, 30, bitmapSource, "楷体", Color.Blue);


               retime = "长三角环境气象预报预警中心" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日 " + hh.ToString() + "时发布";
               SetTextElement(retime, 72, 1060, 20, bitmapSource, "宋体", Color.Black);
               bitmapSource.Save(exportImageName, ImageFormat.Gif);
               return exportImageName;
           }
           catch
           {
               return "";
           }

       }
       public Dictionary<string, string> GetAreapngs(Dictionary<string, string> AreaData)
       {
           Dictionary<string, string> AreaPath = new System.Collections.Generic.Dictionary<string, string>();
         
           foreach (var dic in AreaData)
           {
               string valie = dic.Value;
               string area = dic.Key;
               string datapath = string.Format("{0}/{1}/{2}/{3}", m_Basepath, "maps", area, valie.ToString() + ".png");
               AreaPath.Add(area, datapath);
           }
           return AreaPath;
       }

       /// <summary>
       /// 设置文本要素
       /// </summary>
       /// <param name="s"></param>
       /// <param name="x"></param>
       /// <param name="y"></param>
       /// <param name="size"></param>
       /// <param name="bmp"></param>
       private static void SetTextElement(string s, int x, int y, int size, Image bmp, string texttype, Color color)
       {
           using (Graphics g = Graphics.FromImage(bmp))
           {
               using (Font f = new Font(texttype, size, FontStyle.Bold))
               {
                   using (Brush b = new SolidBrush(color))
                   {
                       string addText = s;
                       g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                       g.DrawString(addText, f, b, x, y);
                   }
               }
           }

       }

       


        ///// <summary>
        ///// 更换地图中标题,日期等元素;
        ///// </summary>
        ///// <param name="pageLayout"></param>
        ///// <param name="pPageLayout"></param>
        ///// <param name="time"></param>
        //private void ChangeElement(IPageLayout pageLayout, IPageLayout pPageLayout, DateTime time)
        //{
        //    IActiveView actiview = pPageLayout as IActiveView;
        //    IGraphicsContainer pGC = pageLayout as IGraphicsContainer;
        //    pGC.Reset();

        //    actiview.Refresh();
        //    IElement pElement = pGC.Next();
        //    int count = 0;
        //    while (pElement != null && count < 10)
        //    {
        //        IElementProperties pElementProperties = pElement as IElementProperties;
        //        ITextElement pTextElement = pElementProperties as ITextElement;
        //        int yy = time.Year;
        //        int mm = time.Month;
        //        int dd = time.Day;
        //        int hh = time.Hour;
        //        if (pElementProperties.Name == "title")
        //        {
        //            if (hh == 8)
        //                pTextElement.Text = "预报时间:" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日";
        //            if (hh == 20)
        //                pTextElement.Text = "预报时间:" + time.AddDays(1).ToString("yyyy") + "年" + time.AddDays(1).ToString("MM") + "月" + time.AddDays(1).ToString("dd") + "日";
        //            count++;
        //        }

        //        if (pElementProperties.Name == "time")
        //        {
        //            pTextElement.Text = "上海中心气象台" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日" + hh.ToString().PadLeft(2, '0') + "时发布";
        //            count++;
        //        }

        //        pElement = pGC.Next();
        //    }

        //}
       

    }
}
