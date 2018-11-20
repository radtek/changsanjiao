using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MMShareBLL.DAL;

public partial class ReportProduce_WeekPolWeather : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            string strUploadFileName = Request.Files[0].FileName;
        }
    }
    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        bool fileOK = false;
        string path = Server.MapPath("~/Temp/");
        if (FileUpload1.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
            //String[] allowedExtensions = { ".gif", ".png", ".bmp", ".jpg" };
            //for (int i = 0; i < allowedExtensions.Length; i++)
            //{
            //    if (fileExtension == allowedExtensions[i])
            //    {
            //        fileOK = true;
            //    }
            //}
            String[] allowedExtensions = { ".gif", ".png", ".bmp", ".jpg",".pdf" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
        }
        if (fileOK)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileUpload1.SaveAs(path + FileUpload1.FileName);
                System.IO.MemoryStream m = new System.IO.MemoryStream();
                string fileSuffix = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                //图片文件
                if (fileSuffix != ".pdf")
                {                    
                    System.Drawing.Bitmap bp = new System.Drawing.Bitmap(path + FileUpload1.FileName);
                    if (fileSuffix == ".gif" || fileSuffix == ".GIF")
                    {
                        bp.Save(m, System.Drawing.Imaging.ImageFormat.Gif);
                    }
                    else if (fileSuffix == ".jpg")
                    {
                        bp.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else if (fileSuffix == ".png")
                    {
                        bp.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else if (fileSuffix == ".bmp")
                    {
                        bp.Save(m, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    byte[] b = m.GetBuffer();
                    string base64string = Convert.ToBase64String(b);
                    if (SelImgID.Value == "PO_TomHazeDropZone")
                    {
                        PO_TomHazeDropZone.Src = "../Temp/" + FileUpload1.FileName;
                    }
                    if (SelImgID.Value == "TomHazeDropZone_Div")
                    {
                        TomHazeDropZone_Div.InnerHtml = "<img id='PO_TomHazeDropZone' runat='server' class='scrollImg Img_024' src='" + "../Temp/" + FileUpload1.FileName + "' alt='' />";
                    }
                    if (SelImgID.Value == "PO_TomAQIDropZone" || SelImgID.Value == "PO_TomAQIDropZone")
                    {
                        PO_TomAQIDropZone.Src = "../Temp/" + FileUpload1.FileName;
                    }
                }
                //PDF文件
                else if (fileSuffix == ".pdf")
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(FileUpload1.FileName);
                    AsposeUtility asposeUti = new AsposeUtility();
                    string strImgFilePath = path + fileName + ".jpg";
                    string strPDFImgPath = asposeUti.ConvertPDFToImg(path + FileUpload1.FileName, strImgFilePath);

                    if (File.Exists(strImgFilePath))
                    {
                        if (SelImgID.Value == "PO_TomHazeDropZone")
                        {
                            PO_TomHazeDropZone.Src = "../Temp/" + fileName + ".jpg";
                        }
                        if (SelImgID.Value == "PO_TomAQIDropZone")
                        {
                            PO_TomAQIDropZone.Src = "../Temp/" + fileName + ".jpg";
                        }
                    }
                    //if (SelImgID.Value == "TomHazeDropZone_Div" || SelImgID.Value == "PO_TomHazeDropZone")
                    //{
                    //    TomHazeDropZone_Div.InnerHtml ="<iframe src='" + "../Temp/" + FileUpload1.FileName + "' height='100%' width='100%' frameborder='0'></iframe>";
                    //}
                    //if (SelImgID.Value == "PO_TomAQIDropZone" || SelImgID.Value == "PO_TomAQIDropZone")
                    //{
                    //    PO_TomAQIDropZone.Src = "../Temp/" + FileUpload1.FileName;
                    //}
                }                                
                //if(SelImgID.Value=="PO_tomHazeImg")
                //{
                //    PO_tomHazeImg.Src = "data:image/gif;base64," + base64string;
                //}
                //if (SelImgID.Value == "PO_aferHazeImg")
                //{
                //    PO_aferHazeImg.Src = "data:image/gif;base64," + base64string;
                //}
                //if (SelImgID.Value == "PO_afterHazeImg2")
                //{
                //    PO_afterHazeImg2.Src = "data:image/gif;base64," + base64string;
                //}
            }
            catch (Exception ex)
            {
                //LabMessage1.Text = "文件上传不成功.";
            }
        }
        else
        {
            //LabMessage1.Text = "只能够上传图片文件.";
        }
    }

    protected void BtnPDF_Click(object sender, EventArgs e)
    {
        bool fileOK = false;
        string path = Server.MapPath("~/Temp/");
        if (FileUpload1.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
            String[] allowedExtensions = { ".gif", ".png", ".bmp", ".jpg", ".pdf" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                }
            }
        }
        if (fileOK)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileUpload1.SaveAs(path + FileUpload1.FileName);
                System.IO.MemoryStream m = new System.IO.MemoryStream();
                string fileSuffix = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();  
                //PDF文件
                if (fileSuffix == ".pdf")
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(FileUpload1.FileName);
                    AsposeUtility asposeUti = new AsposeUtility();
                    string strImgFilePath = path + fileName + ".jpg";
                    string strPDFImgPath = asposeUti.ConvertPDFToImg(path + FileUpload1.FileName, strImgFilePath);

                    if (File.Exists(strImgFilePath))
                    {
                        selDisImg.Src = "../Temp/" + fileName + ".jpg"; ;
                    }

                }

            }
            catch (Exception ex)
            {
                //LabMessage1.Text = "文件上传不成功.";
            }
        }
        else
        {
            //LabMessage1.Text = "只能够上传图片文件.";
        }
    }
}