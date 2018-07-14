using System;
using System.IO;

namespace FeedMapApp.Models
{
    public class DirectoryAccess
    {
        string m_DirPath;

        public DirectoryAccess(bool temp, string dirPath = "")
        {
            m_DirPath = dirPath;
            if (temp)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var tmp = Path.Combine(documents, "..", "tmp");
                m_DirPath = tmp;
            }
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
    }
}
