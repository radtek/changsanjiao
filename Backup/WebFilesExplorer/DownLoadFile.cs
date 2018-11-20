using System;
using System.Collections.Generic;
using System.Web;

public static class DownloadFile
{
    public static void ResponseFile(string path, HttpContext  Context,bool hasfileName)
    {
         Context = HttpContext.Current;

        System.IO.Stream iStream = null;
        byte[] buffer = new Byte[10000];
        int length;
        long dataToRead;
        string filename;
        if (!hasfileName)
        {
            filename = System.IO.Path.GetFileName(path);
        }
        else
        {
            filename = "down_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".zip";
        }

        try
        {
            iStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            dataToRead = iStream.Length;
             Context.Response.ContentType = "application/octet-stream";
             Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8));

            while (dataToRead > 0)
            {
                if ( Context.Response.IsClientConnected)
                {
                    length = iStream.Read(buffer, 0, 10000);
                     Context.Response.OutputStream.Write(buffer, 0, length);
                     Context.Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
             Context.Response.Write(ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                iStream.Close();
            }
        }
    }


}
