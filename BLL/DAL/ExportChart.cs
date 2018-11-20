using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Svg;
using Svg.Transforms;
using System.Xml;
using sharpPDF;
using System.Drawing;

namespace MMShareBLL.DAL
{
    public class ExportChart
    {
           /// <summary>
        

        public ExportChart()
        {

        }

        /// <summary>
        /// Initializes a new chart Export object using the specified file name, 
        /// output type, chart width and SVG text data.
        /// </summary>
        /// <param name="fileName">The file name (without extension) to be used 
        /// for the exported chart.</param>
        /// <param name="type">The requested MIME type to be generated. Can be
        /// 'image/jpeg', 'image/png', 'application/pdf' or 'image/svg+xml'.</param>
        /// <param name="width">The pixel width of the exported chart image.</param>
        /// <param name="svg">An SVG chart document to export (XML text).</param>
        /// 

       

        /// <summary>
        /// Creates an SvgDocument from the SVG text string.
        /// </summary>
        /// <returns>An SvgDocument object.</returns>       

        private SvgDocument CreateSvgDocument(string width,string svg)
        {
            SvgDocument svgDoc = new SvgDocument();

            // Create a MemoryStream from SVG string.
            using (MemoryStream streamSvg = new MemoryStream(
              Encoding.UTF8.GetBytes(svg)))
            {
                svgDoc = SvgDocument.Open(streamSvg);
            }

            // Scale SVG document to requested width.
            svgDoc.Transforms = new SvgTransformCollection();
            //float scalar = (float)this.Width / (float)svgDoc.Width;
            //svgDoc.Transforms.Add(new SvgScale(scalar, scalar));
            //svgDoc.Width = new SvgUnit(svgDoc.Width.Type, svgDoc.Width * scalar);
            //svgDoc.Height = new SvgUnit(svgDoc.Height.Type, svgDoc.Height * scalar);
            float scalar = Convert.ToSingle(width) / (float)svgDoc.Width;
            svgDoc.Transforms.Add(new SvgScale(scalar, scalar));
            svgDoc.Width = new SvgUnit(svgDoc.Width.Type, svgDoc.Width * scalar);
            svgDoc.Height = new SvgUnit(svgDoc.Height.Type, svgDoc.Height * scalar);

            return svgDoc;
        }
        //自定义方法，将图片保存在固定路径
        public string SaveImgToPath(string contentType, string filePath,string fileName,string svg)
        {
            string strFileFullPath = "";
            switch (contentType)
            {
                case "image/jpeg":

                    using (MemoryStream seekableStream = new MemoryStream())
                    {
                        //CreateSvgDocument().Draw().Save(
                        //    seekableStream,
                        //    ImageFormat.Jpeg);
                        CreateSvgDocument("1700", svg).Draw().Save(filePath + fileName+".jpg");
                        //Image img = Image.FromStream(seekableStream);
                        //img.Save(filePath + this.FileName);
                        seekableStream.Close();
                        strFileFullPath = filePath + fileName;
                    }

                    //CreateSvgDocument().Draw().Save(
                    //  outputStream,
                    //  ImageFormat.Jpeg);
                    break;

                //case "image/png":
                //    // PNG output requires a seekable stream.
                //    using (MemoryStream seekableStream = new MemoryStream())
                //    {
                //        CreateSvgDocument().Draw().Save(
                //            seekableStream,
                //            ImageFormat.Png);
                //        Image img = Image.FromStream(seekableStream);
                //        img.Save(filePath + fileName+".png");
                //        seekableStream.Close();
                //        strFileFullPath = filePath + fileName;
                //    }
                //    break;

                //case "application/pdf":
                //    SvgDocument svgDoc = CreateSvgDocument();
                //    Bitmap bmp = svgDoc.Draw();

                //    pdfDocument doc = new pdfDocument(this.Name, null);
                //    pdfPage page = doc.addPage(bmp.Height, bmp.Width);
                //    page.addImage(bmp, 0, 0);
                //    doc.createPDF(outputStream);
                //    break;

                //case "image/svg+xml":
                //    using (StreamWriter writer = new StreamWriter(outputStream))
                //    {
                //        writer.Write(this.Svg);
                //        writer.Flush();
                //    }

                //    break;

                //default:
                //    throw new InvalidOperationException(string.Format(
                //      "ContentType '{0}' is invalid.", this.ContentType));
            }
            return strFileFullPath;
        }

        public string SaveHighChartImgs(string svgs)
        {
            if (svgs != "")
            {
                int intSuccessCount = 0;
                string strFileName = "";
                string strImgFilePath = @"E:\Chart\";
                string strBase = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string strEnvScoreImgFilePath = strBase + "AQI\\ReplaceImgInWord\\";
                string strPM25ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM25ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strPM10ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScorePM10ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");
                string strNO2ImgName = System.Configuration.ConfigurationManager.AppSettings["EnvScoreNO2ImgPrefix"] + "_" + DateTime.Now.ToString("yyyyMMdd");

                strImgFilePath = strBase + "AQI\\ReplaceImgInWord\\";
                string[] allSvgs = svgs.Split('%');
                if (allSvgs.Length > 0)
                {
                    for (int i = 0; i < allSvgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            strFileName = strPM25ImgName;
                        }
                        if (i == 1)
                        {
                            strFileName = strPM10ImgName;
                        }
                        if (i == 2)
                        {
                            strFileName = strNO2ImgName;
                        }
                        //Tek4.Highcharts.Exporting.ExporterNew export = new Tek4.Highcharts.Exporting.ExporterNew(filename, type, width, allSvgs[i]);
                        //string strFilePath = export.WriteToHttpResponse(context.Response, strImgFilePath);
                        try
                        {
                            string strFilePath = SaveImgToPath("image/jpeg", strImgFilePath, strFileName, allSvgs[i]);
                            intSuccessCount++;
                        }
                        catch
                        {

                        }
                    }
                    if (intSuccessCount == allSvgs.Length)
                    {
                        return "success";
                    }
                }
            }
            return "fail";
        }
    }
}
