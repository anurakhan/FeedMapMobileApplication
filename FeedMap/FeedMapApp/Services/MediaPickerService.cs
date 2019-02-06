using System;
using System.Collections.Generic;
using FeedMapApp.Helpers.DirectoryHelpers;
using FeedMapApp.Models;
using Foundation;
using UIKit;

namespace FeedMapApp.Services
{
    public class MediaPickerService
    {
        private DirectoryAccess _directoryAccess;
        private int _index;

        public MediaPickerService()
        {
            IDirectory directory = new FoodMarkerPendingImageDirectory();
            _directoryAccess = new DirectoryAccess(directory);
            _index = 1;
        }

        public void ClearPending()
        {
            _directoryAccess.ClearFolder();
        }

        public void SaveMediaToPending(UIImage image)
        {
            using (NSData data = image.AsPNG())
            {
                byte[] buffer = new byte[data.Length];
                System.Runtime.InteropServices.Marshal.Copy(data.Bytes, buffer, 0, Convert.ToInt32(data.Length));
                _directoryAccess.UploadFile(buffer, _index.ToString() + ".png");
                _index++;
            }
        }

        public List<byte[]> GetMediaFromPending()
        {
            List<byte[]> ret = new List<byte[]>();
            for (int i = 1; i < _index; i++)
            {
                ret.Add(_directoryAccess.GetFile(i.ToString() + ".png"));
            }
            return ret;
        }
    }

}
