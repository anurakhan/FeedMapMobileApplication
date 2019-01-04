using System;
using System.IO;
using FeedMapApp.Models;
using Foundation;
using UIKit;

namespace FeedMapApp.Services
{
    public class FoodMarkerPhotosFileSystemService
    {
        private FoodMarker _marker;
        private readonly string _path;


        public FoodMarkerPhotosFileSystemService(FoodMarker marker)
        {
            _marker = marker;
            _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _path = Path.Combine(_path, "..", "tmp");
            _path = Path.Combine(_path, "feedmapphotos");
        }

        public void UploadFoodMarkerPhotosToDir()
        {
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            ClearFoodMarkerPhotosDir();

            int i = 0;
            foreach (var meta in _marker.FoodMarkerPhotos)
            {
                UIImage im;
                using (var url = new NSUrl(meta.ImageUrl))
                {
                    using (var data = NSData.FromUrl(url))
                    {
                        im = UIImage.LoadFromData(data);
                    }
                }
                using (NSData data = im.AsPNG())
                {
                    byte[] buffer = new byte[data.Length];
                    System.Runtime.InteropServices.Marshal.Copy(data.Bytes, buffer, 0, Convert.ToInt32(data.Length));
                    File.WriteAllBytes(Path.Combine(_path, i.ToString() + ".png"), buffer);
                }
                i++;
            }
        }

        private void ClearFoodMarkerPhotosDir()
        {
            var files = Directory.EnumerateFiles(_path);
            foreach (var file in files) File.Delete(file);
        }
    }
}
