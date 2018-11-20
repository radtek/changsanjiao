using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Pdf;
using System.IO;
using Aspose.Pdf.Devices;


namespace MMShareBLL.DAL
{
    public class AsposeUtility
    {
        //将PDF转为图片
        public string ConvertPDFToImg(string pdfFile,string imgFile)
        {
            if (File.Exists(pdfFile) && imgFile != "")
            {
                try
                {
                    Document pdfDocument = new Document(pdfFile);
                    using (FileStream imageStream = new FileStream(imgFile, FileMode.Create))
                    {
                        //create JPEG device with specified attributes
                        //Width, Height, Resolution, Quality
                        //Quality [0-100], 100 is Maximum
                        //create Resolution object
                        Resolution resolution = new Resolution(300);
                        //JpegDevice jpegDevice = new JpegDevice(500, 700, resolution,100);
                        JpegDevice jpegDevice = new JpegDevice(resolution, 100);

                        //convert a particular page and save the image to stream
                        jpegDevice.Process(pdfDocument.Pages[1], imageStream);
                        //close stream
                        imageStream.Close();
                        return imgFile;
                    }
                }
                catch { }
            }
            return "";
        }

        //只根据本地选取的PDF文件名，自动在服务器上转换生成同名的图片文件
        public string ConvertPDFToImgAuto(string pdfFile)
        {
            //if (File.Exists(pdfFile))
            //{
            //    try
            //    {
            //        Document pdfDocument = new Document(pdfFile);
            //        using (FileStream imageStream = new FileStream(imgFile, FileMode.Create))
            //        {
            //            //create JPEG device with specified attributes
            //            //Width, Height, Resolution, Quality
            //            //Quality [0-100], 100 is Maximum
            //            //create Resolution object
            //            Resolution resolution = new Resolution(300);
            //            //JpegDevice jpegDevice = new JpegDevice(500, 700, resolution,100);
            //            JpegDevice jpegDevice = new JpegDevice(resolution, 100);

            //            //convert a particular page and save the image to stream
            //            jpegDevice.Process(pdfDocument.Pages[1], imageStream);
            //            //close stream
            //            imageStream.Close();
            //            return imgFile;
            //        }
            //    }
            //    catch { }
            //}
            return "";
        }
    }
}
