using System;
using Foundation;
using Security;

namespace FeedMapApp.Helpers
{
    public static class KeyChainUtil
    {
        public static bool NewKeyChainRecord(string key, string value)
        {
            var record = new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Label = key,
                ValueData = NSData.FromString(value, NSStringEncoding.UTF8)
            };
            RemoveKeyChainRecords(key);
            var result = SecKeyChain.Add(record);
            return (result == SecStatusCode.Success);
        }

        public static bool RemoveKeyChainRecords(string key)
        {
            var record = GetRecord(key);
            if (String.IsNullOrEmpty(GetKeyChainRecord(record)))
                return true;

            var result = SecKeyChain.Remove(record);
            return (result == SecStatusCode.Success);
        }

        public static string GetKeyChainRecord(string key)
        {
            var record = GetRecord(key);
            return GetKeyChainRecord(record);
        }

        public static string GetKeyChainRecord(SecRecord record)
        {
            SecStatusCode resultCode;
            var match = SecKeyChain.QueryAsRecord(record, out resultCode);

            if (resultCode == SecStatusCode.Success)
                return NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
            else
                return String.Empty;
        }

        private static SecRecord GetRecord(string key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Label = key
            };
        }
    }
}
