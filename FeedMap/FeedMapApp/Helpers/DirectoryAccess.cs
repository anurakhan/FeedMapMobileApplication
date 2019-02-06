using System;
using System.IO;
using FeedMapApp.Helpers.DirectoryHelpers;

namespace FeedMapApp.Models
{
    public class DirectoryAccess
    {
        string m_DirPath;

        public DirectoryAccess(IDirectory directory)
        {
            m_DirPath = directory.GetDir();
        }

        public void UploadFile(byte[] buffer, string fileName, string additionalPath = "") 
        {
            var path = Path.Combine(m_DirPath, additionalPath);
            path = Path.Combine(path, fileName);
            File.WriteAllBytes(path, buffer);
        }

        public byte[] GetFile(string fileName, string additionalPath = "")
        {
            var path = Path.Combine(m_DirPath, additionalPath);
            path = Path.Combine(path, fileName);
            return File.ReadAllBytes(path);
        }

        public void ClearFolder(string additionalPath = "")
        {
            var path = Path.Combine(m_DirPath, additionalPath);

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
