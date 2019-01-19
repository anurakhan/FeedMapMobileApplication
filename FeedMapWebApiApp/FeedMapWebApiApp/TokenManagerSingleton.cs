using System;
using System.Collections.Generic;

namespace FeedMapWebApiApp
{
    public class TokenManagerSingleton
    {
        public static TokenManagerSingleton _singleton;
        private Dictionary<string, Tuple<int, DateTime>> _tokenPool;
        private int _timeSpanMinutes;

        public TokenManagerSingleton(int timeSpanMinutes)
        {
            _tokenPool = new Dictionary<string, Tuple<int, DateTime>>();
            _timeSpanMinutes = timeSpanMinutes;
        }

        public static TokenManagerSingleton GetInstance(int timeSpanMinutes)
        {
            if (_singleton == null) 
            {
                _singleton = new TokenManagerSingleton(timeSpanMinutes);
                return _singleton;
            } 
            return _singleton;
        }

        public string AddUserToPool(int userId)
        {
            string token = Guid.NewGuid().ToString();
            
            _tokenPool.Add(token, 
                           new Tuple<int, DateTime>
                           (userId, 
                            DateTime.UtcNow.AddMinutes(_timeSpanMinutes)));
            return token;
        }

        public bool HasTokenInPool(string inToken)
        {
            if (!_tokenPool.ContainsKey(inToken)) return false;
            Tuple<int, DateTime> item = _tokenPool[inToken];
            if (DateTime.UtcNow > item.Item2)
            {
                _tokenPool.Remove(inToken);
                return false;
            }
            return true;
        }
    }
}
