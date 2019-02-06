using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FeedMapWebApiApp
{
    public class TokenManagerSingleton
    {
        public static TokenManagerSingleton _singleton;
        private ConcurrentDictionary<string, Tuple<int, DateTime>> _tokenPool;
        private Queue<string> _tokenQueue;
        private int _timeSpanMinutes;
        private int _maxActiveUsers;

        public TokenManagerSingleton(int timeSpanMinutes, int maxActiveUsers)
        {
            _tokenPool = new ConcurrentDictionary<string, Tuple<int, DateTime>>();
            _tokenQueue = new Queue<string>();
            _timeSpanMinutes = timeSpanMinutes;
            _maxActiveUsers = maxActiveUsers;
        }

        public static TokenManagerSingleton GetInstance(int timeSpanMinutes,
                                                        int maxActiveUsers)
        {
            if (_singleton == null) 
            {
                _singleton = new TokenManagerSingleton(timeSpanMinutes,
                                                       maxActiveUsers);
                return _singleton;
            } 
            return _singleton;
        }

        public string AddUserToPool(int userId)
        {
            string token = Guid.NewGuid().ToString();

            if (_tokenPool.Count >= _maxActiveUsers)
            {
                bool hasRemovedFromPoolAtLeastOne = false;
                while (!hasRemovedFromPoolAtLeastOne)
                {
                    string dequedToken = _tokenQueue.Dequeue();
                    Tuple<int, DateTime> val;
                    hasRemovedFromPoolAtLeastOne =
                        _tokenPool.TryRemove(dequedToken, out val);
                }
            }

            _tokenPool.TryAdd(token, 
                           new Tuple<int, DateTime>
                           (userId, 
                            DateTime.UtcNow.AddMinutes(_timeSpanMinutes)));
            
            _tokenQueue.Enqueue(token);

            return token;
        }

        public bool HasTokenInPool(string inToken, ref int id)
        {
            if (!_tokenPool.ContainsKey(inToken)) return false;
            Tuple<int, DateTime> item = _tokenPool[inToken];
            if (DateTime.UtcNow > item.Item2)
            {
                Tuple<int, DateTime> val;
                _tokenPool.TryRemove(inToken, out val);
                return false;
            }
            id = item.Item1;
            return true;
        }
    }
}
