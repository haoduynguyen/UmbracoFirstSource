using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customs.Helper
{
    public class ImageHelper
    {
        public static bool CheckFileExt(string filename, String[] allowedExtensions)
        {

            bool fileOK = false;
            String fileExtension = System.IO.Path.GetExtension(filename).ToLower();
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i].ToLower())
                {
                    fileOK = true;
                    break;
                }
            }
            return fileOK;
        }
    }
}
