using System;
using System.IO;

namespace FeedMapApp.Helpers.DirectoryHelpers
{
    public class TempDirectory : IDirectory
    {
        public TempDirectory()
        {
        }

        public string GetDir()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var tmp = Path.Combine(documents, "..", "tmp");
            return tmp;
        }
    }
}
