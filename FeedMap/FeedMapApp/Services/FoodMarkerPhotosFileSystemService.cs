﻿using System;
using System.IO;
using FeedMapApp.Models;
using Foundation;
using UIKit;
using FeedMapApp.Helpers.DirectoryHelpers;


namespace FeedMapApp.Services
{
    public class FoodMarkerPhotosFileSystemService
    {
        private FoodMarker _marker;
        private DirectoryAccess _directoryAccess;
        private readonly string _path;


        public FoodMarkerPhotosFileSystemService(FoodMarker marker)
        {
            IDirectory directory = new FoodMarkerImageDirectory();
            _marker = marker;
            _directoryAccess = new DirectoryAccess(directory);
            _path = directory.GetDir();
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
                    _directoryAccess.UploadFile(buffer, Path.Combine(_path, i.ToString() + ".png"));
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
