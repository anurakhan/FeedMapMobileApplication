using System;
using System.IO;

namespace FeedMapApp.Helpers.DirectoryHelpers
{
    public class FoodMarkerImageDirectory : IDirectory
    {
        public FoodMarkerImageDirectory()
        {
            
        }

        public string GetDir()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "..", "tmp");
            path = Path.Combine(path, "feedmapphotos");
            return path;
        }
    }
}
