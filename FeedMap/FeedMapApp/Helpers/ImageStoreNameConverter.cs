using System;
namespace FeedMapApp.Helpers
{
    public class ImageStoreNameConverter
    {
        public ImageStoreNameConverter()
        {
        }

        public string GenerateMainStorageName(string imageName)
        {
            return imageName + "_Main_Image";
        }
    }
}
