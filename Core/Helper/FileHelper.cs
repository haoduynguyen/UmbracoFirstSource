using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Customs.Helper
{
    public class FileHelper
    {
        public static bool SaveByteArrayAsImage(string fullOutputPath, string base64String,out Image image)
        {
            MemoryStream ms = null;
            bool isSuccess = false;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
                fullOutputPath = fullOutputPath + GetFilenameExtension(image.RawFormat);
                image.Save(fullOutputPath);
                isSuccess = System.IO.File.Exists(fullOutputPath);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
            return isSuccess;
        }

        public static bool SaveByteArray(string fullOutputPath, string base64String)
        {
            System.IO.File.WriteAllText(fullOutputPath, base64String);
            return System.IO.File.Exists(fullOutputPath);
        }


        public static string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString();
        }

        public static string GetFilenameExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders()
                                 .First(x => x.FormatID == format.Guid)
                                 .FilenameExtension
                                 .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                 .First()
                                 .Trim('*')
                                 .ToLower();
        }
    }
}