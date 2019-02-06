using System;
using System.IO;

namespace FeedMapApp.Helpers.DirectoryHelpers
{
    public class FoodMarkerPendingImageDirectory : IDirectory
    {
        public FoodMarkerPendingImageDirectory()
        {

        }

        public string GetDir()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "..", "tmp");
            path = Path.Combine(path, "pendingphotos");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
    }
}
