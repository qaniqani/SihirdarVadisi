using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Sihirdar.WebService.Provider.RiotApi.Interface;

namespace Sihirdar.WebService.Provider.RiotApi
{
    public class Cache : ICache
    {
        private readonly IDictionary<object, CacheItem> _cache = new Dictionary<object, CacheItem>();
        private readonly IDictionary<object, SlidingDetails> _slidingTimes = new Dictionary<object, SlidingDetails>();

        private const int DefaultMonitorWait = 1000;
        private const int MonitorWaitToUpdateSliding = 500;

        private readonly object _sync = new object();

        #region ICache interface
        /// <summary>
        /// Add a (key, value) pair to the cache with a relative expiry time (e.g. 2 mins).
        /// </summary>
        /// <typeparam name="K">Type of the key.</typeparam>
        /// <typeparam name="V">Type of the value which has to be a reference type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiry">The sliding time at the end of which the (key, value) pair should expire and
        /// be purged from the cache.</param>
        public void Add<K, V>(K key, V value, TimeSpan slidingExpiry) where V : class
        {
            Add(key, value, slidingExpiry, true);
        }

        /// <summary>
        /// Add a (key, value) pair to the cache with an absolute expiry date (e.g. 23:33:00 03/04/2030)
        /// </summary>
        /// <typeparam name="K">Type of the key.</typeparam>
        /// <typeparam name="V">Type of the value which has to be a reference type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiry">The absolute expiry date when the (key, value) pair should expire and
        /// be purged from the cache.</param>
        public void Add<K, V>(K key, V value, DateTime absoluteExpiry) where V : class
        {
            if (absoluteExpiry > DateTime.Now)
            {
                var diff = absoluteExpiry - DateTime.Now;
                Add(key, value, diff, false);
            }
        }

        /// <summary>
        /// Get a value from the cache.
        /// </summary>
        /// <typeparam name="K">Type of the key.</typeparam>
        /// <typeparam name="V">Type of the value which has to be a reference type.</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The value if the key exists in the cache, null otherwise.</returns>
        public V Get<K, V>(K key) where V : class
        {
            if (_cache.ContainsKey(key))
            {
                var cacheItem = _cache[key];

                if (cacheItem.RelativeExpiry.HasValue)
                {
                    if (Monitor.TryEnter(_sync, MonitorWaitToUpdateSliding))
                    {
                        try
                        {
                            _slidingTimes[key].Viewed();
                        }
                        finally
                        {
                            Monitor.Exit(_sync);
                        }
                    }
                }

                return (V)cacheItem.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove the value associated with the specified key from the cache.
        /// </summary>
        /// <typeparam name="K">Type of the key.</typeparam>
        /// <param name="key">The key.</param>
        public void Remove<K>(K key)
        {
            if (!Equals(key, null))
            {
                _cache.Remove(key);
                _slidingTimes.Remove(key);
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    _cache.Clear();
                    _slidingTimes.Clear();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
        }

        /// <summary>
        /// Enumerator for the keys of a specific type.
        /// </summary>
        /// <typeparam name="K">Type of the key.</typeparam>
        /// <returns>Enumerator for the keys of a specific type.</returns>
        public IEnumerable<K> Keys<K>()
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    return _cache.Keys.Where(k => k.GetType() == typeof(K)).Cast<K>().ToList();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                return Enumerable.Empty<K>();
            }
        }

        /// <summary>
        /// Enumerator for all keys.
        /// </summary>
        /// <returns>Enumerator for all keys.</returns>
        public IEnumerable<object> Keys()
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    return _cache.Keys.ToList();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Enumerator for the values of a specific type.
        /// </summary>
        /// <typeparam name="V">Type of the value which has to be a reference type.</typeparam>
        /// <returns>Enumerator for the values of a specific type.</returns>
        public IEnumerable<V> Values<V>() where V : class
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    return _cache.Values
                        .Select(cacheItem => cacheItem.Value)
                        .Where(v => v.GetType() == typeof(V))
                        .Cast<V>().ToList();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                return Enumerable.Empty<V>();
            }
        }

        /// <summary>
        /// Enumerator for all values.
        /// </summary>
        /// <returns>Enumerator for all values.</returns>
        public IEnumerable<object> Values()
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    return _cache.Values.Select(cacheItem => cacheItem.Value).ToList();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Total amount of (key, value) pairs in the cache.
        /// </summary>
        /// <returns>Total amount of (key, value) pairs in the cache.</returns>
        public int Count()
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    return _cache.Keys.Count;
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
            else
            {
                return -1;
            }
        }

        #endregion

        private void Add<K, V>(K key, V value, TimeSpan timeSpan, bool isSliding) where V : class
        {
            if (Monitor.TryEnter(_sync, DefaultMonitorWait))
            {
                try
                {
                    Remove(key);
                    _cache.Add(key, new CacheItem(value, isSliding ? timeSpan : (TimeSpan?)null));

                    if (isSliding)
                    {
                        _slidingTimes.Add(key, new SlidingDetails(timeSpan));
                    }

                    StartObserving(key, timeSpan);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
        }

        private void StartObserving<K>(K key, TimeSpan timeSpan)
        {
            Observable.Timer(timeSpan)
                .Subscribe(x => TryPurgeItem(key));
        }

        private void TryPurgeItem<K>(K key)
        {
            if (_slidingTimes.ContainsKey(key))
            {
                TimeSpan tryAfter;
                if (!_slidingTimes[key].CanExpire(out tryAfter))
                {
                    StartObserving(key, tryAfter);
                    return;
                }
            }

            Remove(key);
        }

        private class CacheItem
        {
            public CacheItem() { }

            public CacheItem(object value, TimeSpan? relativeExpiry)
            {
                Value = value;
                RelativeExpiry = relativeExpiry;
            }

            public object Value { get; set; }
            public TimeSpan? RelativeExpiry { get; set; }
        }

        private class SlidingDetails
        {
            private readonly TimeSpan _relativeExpiry;
            private DateTime _expireAt;

            public SlidingDetails(TimeSpan relativeExpiry)
            {
                this._relativeExpiry = relativeExpiry;
                Viewed();
            }

            public bool CanExpire(out TimeSpan tryAfter)
            {
                tryAfter = _expireAt - DateTime.Now;
                return (0 > tryAfter.Ticks);
            }

            public void Viewed()
            {
                _expireAt = DateTime.Now.Add(_relativeExpiry);
            }
        }
    }
}
