using System;
using System.IO;

namespace FeedMapBLL.Helpers
{
    public class ImageFileNameConverter
    {

        public ImageFileNameConverter()
        {
        }

        public string Convert(string fileName, string id = "") 
        {
            string fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            fileNameNoExt = fileNameNoExt.Replace(" ", "_");
            if (!String.IsNullOrWhiteSpace(id)) fileNameNoExt = id + "_" + fileNameNoExt;
            fileNameNoExt += "_" + DateTime.UtcNow.ToString("yyyy_MM_dd");
            return fileNameNoExt + Path.GetExtension(fileName);
        }
    }
}
