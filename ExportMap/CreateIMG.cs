using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ESRI.ArcGIS;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using System.Data;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ExportMap
{
    class CreateIMG
    {
       public  CreateIMG()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AreaData"></param>
        /// <param name="time">发布时间</param>
        /// <param name="filename">上传图片名 </param>
        /// <param name="basepath">服务器路径</param>
        /// <returns></returns>
       public string DealData(Dictionary<string, string> AreaData, DateTime time, string filename, string basepath)
        {
            try
            {
                RuntimeManager.BindLicense(ProductCode.EngineOrDesktop);
                string path = string.Format("{0}/{1}/{2}/", basepath, time.ToString("yyyy"), time.ToString("yyyyMMddHH"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string txtname = path + "Air.csv";
                WriteTxt(AreaData, txtname);
                string exportImageName = path + filename;
                string MxdFileName = string.Format("{0}/shp/diffusion_sh.mxd", basepath);
                string mxdFileName = CopyMxd(MxdFileName, path);
                IMapDocument m_MapDoc = new MapDocumentClass();
                m_MapDoc.Open(mxdFileName, null);
                IPageLayout pPageLayout = m_MapDoc.PageLayout;
                ChangeElement(pPageLayout, pPageLayout, time);
                FileInfo file = new FileInfo(exportImageName);
                if (!file.Directory.Exists)
                    file.Directory.Create();
                CreatePNG(pPageLayout, exportImageName);
                m_MapDoc.Close();
                return exportImageName;
            }
            catch
            {
                return "";
            }
        }


        public string CopyMxd(string MxdFileName, string directoryPath)
        {
            if (!(Directory.Exists(directoryPath)))
                Directory.CreateDirectory(directoryPath);
            FileInfo mxdFileInfo = new FileInfo(MxdFileName);
            FileInfo copyTomxdFileInfo = new FileInfo(directoryPath + mxdFileInfo.Name);
            if (copyTomxdFileInfo.Exists)
            {
                copyTomxdFileInfo.Delete();
            }
            File.Copy(mxdFileInfo.FullName, copyTomxdFileInfo.FullName);
            return copyTomxdFileInfo.FullName;

        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);
        const uint SPI_SETFONTSMOOTHING = 0x004B;

        const uint SPIF_UPDATEINIFILE = 0x1;

        const uint SPI_SETFONTSMOOTHINGTYPE = 0x200B;
        //cleartype类型
        const uint FE_FONTSMOOTHINGCLEARTYPE = 2;
        private int EnableFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, (IntPtr)pv, SPIF_UPDATEINIFILE);
            pv = (int)FE_FONTSMOOTHINGCLEARTYPE;
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHINGTYPE, 0, (IntPtr)pv, SPIF_UPDATEINIFILE);
            return pv;
        }
        /// <summary>
        /// 生成PNG
        /// </summary>
        private void CreatePNG(IPageLayout pPageLayout, string pngName)
        {
            var pActiveView = pPageLayout as IActiveView;
            IExport pExport = new ExportPNGClass();
            pExport.ExportFileName = pngName;
            const int iscreen = 300;
            const int iout = 300;
            pExport.Resolution = iout;

            tagRECT exportRECT;
            exportRECT.left = 0;
            exportRECT.top = 0;
            exportRECT.right = pActiveView.ExportFrame.right * (iout / iscreen);
            exportRECT.bottom = pActiveView.ExportFrame.bottom * (iout / iscreen);

            EnableFontSmoothing();
            IEnvelope ppixelboundenv = new EnvelopeClass();
            ppixelboundenv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
            pExport.PixelBounds = ppixelboundenv;
            int hdc = pExport.StartExporting();
            pActiveView.Output(hdc, (int)pExport.Resolution, ref exportRECT, null, null);
            pActiveView.Refresh();
            pExport.FinishExporting();
            // UploadStream(pExport.ExportFileName, desfilename, 0);
            //InsertImageToDB(rasterField + GetProducTimeCodes() + "_" + time.ToString("yyyyMMddHHmm") + "_" + GetPeriod(m_ForecastInterval) + ".PNG", rasterField);

            pExport.Cleanup();
        }


        /// <summary>
        /// 更换地图中标题,日期等元素;
        /// </summary>
        /// <param name="pageLayout"></param>
        /// <param name="pPageLayout"></param>
        /// <param name="time"></param>
        private void ChangeElement(IPageLayout pageLayout, IPageLayout pPageLayout, DateTime time)
        {
            IActiveView actiview = pPageLayout as IActiveView;
            IGraphicsContainer pGC = pageLayout as IGraphicsContainer;
            pGC.Reset();

            actiview.Refresh();
            IElement pElement = pGC.Next();
            int count = 0;
            while (pElement != null && count < 10)
            {
                IElementProperties pElementProperties = pElement as IElementProperties;
                ITextElement pTextElement = pElementProperties as ITextElement;
                int yy = time.Year;
                int mm = time.Month;
                int dd = time.Day;
                int hh = time.Hour;
                if (pElementProperties.Name == "title")
                {
                    if (hh == 8)
                        pTextElement.Text = "预报时间:" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日";
                    if (hh == 20)
                        pTextElement.Text = "预报时间:" + time.AddDays(1).ToString("yyyy") + "年" + time.AddDays(1).ToString("MM") + "月" + time.AddDays(1).ToString("dd") + "日";
                    count++;
                }

                if (pElementProperties.Name == "time")
                {
                    pTextElement.Text = "上海中心气象台" + yy.ToString() + "年" + mm.ToString().PadLeft(2, '0') + "月" + dd.ToString().PadLeft(2, '0') + "日" + hh.ToString().PadLeft(2, '0') + "时发布";
                    count++;
                }

                pElement = pGC.Next();
            }

        }
        public void WriteTxt(Dictionary<string, string> AreaData, string directoryName)
        {
            //判断文件夹是否存在，如果不存在则创建文件夹
            FileInfo fileInfo = new FileInfo(directoryName);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            StreamWriter sw = new StreamWriter(directoryName, false, Encoding.Unicode);
            sw.Write("FID,SCODE\r\n");

            foreach (var dic in AreaData)
            {
                string valie = dic.Value;
                string area = getSiteCode(dic.Key).ToString(); ;
                sw.Write("{0},{1}\r\n", area, valie);
            }
            sw.Close();
        }
        private int getSiteCode(string name)
        {
            int pid = 0;
            if (name.Contains("青浦区"))
                pid = 0;
            if (name.Contains("嘉定区"))
                pid = 1;
            if (name.Contains("奉贤区"))
                pid = 2;
            if (name.Contains("宝山区"))
                pid = 3;
            if (name.Contains("松江区"))
                pid = 4;
            if (name.Contains("浦东新区"))
                pid = 5;
            if (name.Contains("闵行区"))
                pid = 6;
            if (name.Contains("金山区"))
                pid = 7;
            if (name.Contains("崇明县"))
                pid = 8;
            if (name.Contains("中心城区"))
                pid = 9;
            return pid;
        }

    }
}
