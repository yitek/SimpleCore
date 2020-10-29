using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class MemeryCache<T>:IDisposable
    {
        public class CacheItem {
            public string Key { get; set; }
            public T Data { get; set; }

            public DateTime ExpireTime { get; set; }

            public CacheItem Next { get; set; }

        }
        public enum EachActions { 
            Continue,
            Break,
            RemoveAndContinue,
            RemoveAndBreak
           
        }
        CacheItem _First;

        readonly ReaderWriterLockSlim _Locker;

        public int LiveMilliseconds{get;set;}

        public Task ClearTask { get; private set; }

        public MemeryCache(int liveMilliseconds) {
            this.LiveMilliseconds = liveMilliseconds;
            this._Locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this.ClearTask = Task.Run(()=> {
                Task.Delay(this.LiveMilliseconds);
                CacheItem last = null;
                var node = this._First;
                var now = DateTime.Now;
                var c = 0;

                this._Locker.EnterWriteLock();
                try
                {
                    while (node != null)
                    {
                        if (++c > 4000)
                        {
                            c = 0; now = DateTime.Now;
                        }

                        
                        if (node.ExpireTime < now)
                        {
                            if (last != null) last.Next = node.Next;
                            else _First = node.Next;
                            node = node.Next;
                        }
                        else
                        {
                            last = node;
                            node = node.Next;
                        }
                    }
                }
                finally
                {
                    this._Locker.ExitWriteLock();
                }
            });
        }

        

        public T Get(string key) {
            this._Locker.EnterReadLock();
            CacheItem last;
            var node = this._First;
            var now = DateTime.Now;
            var c = 0;
            try
            {
                while (node != null)
                {
                    if (++c > 4000)
                    {
                        c = 0; now = DateTime.Now;
                    }
                    
                    if (node.Key == key && node.ExpireTime>=now) {
                        lock (node) {
                            node.ExpireTime = now.AddMilliseconds(this.LiveMilliseconds);
                            return node.Data;
                        }
                    }
                    last = node;
                    node = node.Next;

                }
            }
            finally
            {
                this._Locker.ExitReadLock();
            }
            return default;
        }

        public T GetOrAdd(string key,Func<string,T> creator)
        {
            this._Locker.EnterUpgradeableReadLock();
            CacheItem last = null;
            var node = this._First;
            var now = DateTime.Now;
            var c = 0;
            try
            {
                while (node != null)
                {
                    if (++c > 4000)
                    {
                        c = 0; now = DateTime.Now;
                    }

                    if (node.Key == key && node.ExpireTime >= now)
                    {
                        lock (node)
                        {
                            node.ExpireTime = now.AddMilliseconds(this.LiveMilliseconds);
                            return node.Data;
                        }
                    }
                    last = node;
                    node = node.Next;

                }
                this._Locker.EnterWriteLock();
                try {
                    if (this._LastAppendItem!=null && this._LastAppendItem.Key == key && this._LastAppendItem.ExpireTime<now) {
                        return this._LastAppendItem.Data;
                    }
                    var item = this._LastAppendItem = new CacheItem()
                    {
                        Data = creator(key),
                        ExpireTime = DateTime.Now.AddMilliseconds(this.LiveMilliseconds),
                        Key = key
                    };
                    if (last == null) this._First = item;
                    else last.Next = item;
                } finally {
                    this._Locker.ExitWriteLock();
                }
            }
            finally
            {
                this._Locker.ExitUpgradeableReadLock();
            }
            return default;
        }
        CacheItem _LastAppendItem;

        public void Set(string key,T value) {
            if (this.ClearTask == null) return;
            CacheItem last = null;
            var node = this._First;
            var now = DateTime.Now;
            var c = 0;

            this._Locker.EnterWriteLock();
            try {
                while (node != null)
                {
                    if (++c > 4000)
                    {
                        c = 0; now = DateTime.Now;
                    }
                    
                    if (node.Key == key)
                    {
                        lock (node)
                        {
                            node.ExpireTime = now.AddMilliseconds(this.LiveMilliseconds);
                            node.Data = value;
                        }
                        return;
                    }
                    if (node.ExpireTime < now)
                    {
                        if (last != null) last.Next = node.Next;
                        else _First = node.Next;
                        node = node.Next;
                    }
                    else {
                        last = node;
                        node = node.Next;
                    }
                }
                var item =this._LastAppendItem = new CacheItem() {
                    Data = value,
                    ExpireTime = DateTime.Now.AddMilliseconds(this.LiveMilliseconds),
                    Key = key
                };
                if (last == null) this._First = item;
                else last.Next = item;
            } finally {
                this._Locker.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
