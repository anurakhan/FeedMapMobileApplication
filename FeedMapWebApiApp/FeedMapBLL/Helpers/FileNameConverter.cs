using System;
using System.IO;

namespace FeedMapBLL.Helpers
{
    public class ImageFileNameConverter
    {

        public ImageFileNameConverter()
        {
        }

        private string AttachTimeStamp(string s)
        {
            return s + "_" + DateTime.UtcNow.ToString("yyyy_MM_dd");
        }

        public string ClientFileNameConvert(string fileName, string id = "")
        {
            string fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            fileNameNoExt = fileNameNoExt.Replace(" ", "_");
            return fileNameNoExt + Path.GetExtension(fileName);
        }

        public string FileNameGenerate()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
