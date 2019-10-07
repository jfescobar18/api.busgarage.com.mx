using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Utils
{
    public class FileUtils
    {
        public static void UploadImage(HttpRequest httpRequest, string folder, ref Dictionary<string, object> dict, ref List<string> filenames)
        {
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 10;
                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                            string ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')),
                            extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                dict.Add("message", "Please upload files of type .jpg, .png");
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                dict.Add("message", "Please upload a image upto 10 mb");
                            }
                            else
                            {
                                postedFile.SaveAs(HttpContext.Current.Server.MapPath($"~/{folder}/" + postedFile.FileName));
                                filenames.Add(postedFile.FileName);
                                dict.Add("message", "Image updated Successfully");
                            }
                        }
                    }
                }
                else
                {
                    dict.Add("message", "Please upload a image");
                }
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {   
                if (File.Exists(HttpContext.Current.Server.MapPath("~/" + path)))
                {  
                    File.Delete(HttpContext.Current.Server.MapPath("~/" + path));
                }
            }
            catch (IOException ioExp)
            {
                throw ioExp;
            }
        }

        public static void ReplaceFile(string path, HttpRequest httpRequest, string folder, ref Dictionary<string, object> dict, ref List<string> filenames)
        {
            DeleteFile(path);
            UploadImage(httpRequest, folder, ref dict, ref filenames);
        }
    }
}