using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Domain.Utility
{
    public class FileHelper
    {
        public static string UploadFileToServer(HttpPostedFileBase file, string path, string fileName)
        {
            
            if (!File.Exists(path))
            {
                file.SaveAs(path);
                return fileName;
            }
            else
            {
                // generate another file name if filename is existed
                int index = path.LastIndexOf('.');
                string extension = path.Substring(index + 1, path.Length - index - 1);
                int num = 1;
                
                path = path.Substring(0, index);
                path = path + "(1)." + extension;

                fileName = fileName.Substring(0, fileName.Length - extension.Length - 1);
                fileName = fileName + "(1)." + extension;

                while (File.Exists(path)) 
                { 
                    int index2 = path.LastIndexOf('.');
                    path = path.Substring(0, index2 - 3);
                    num++;
                    path = path + "("+ num +")." + extension;

                    int index3 = fileName.LastIndexOf('.');
                    fileName = fileName.Substring(0, index3 - 3);
                    fileName = fileName + "(" + num + ")." + extension;
                }
                file.SaveAs(path);
                return fileName;
            }
        }

        public static bool DeleteFileFromSystem(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }
                File.Delete(path);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}