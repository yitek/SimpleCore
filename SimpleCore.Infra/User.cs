using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class User : IUser
    {
        public User() { }
        public User(Guid id, string name,string sessionId=null,bool anonymous=true,ConcurrentDictionary<string,object> data=null) {
            this.Id = id;
            this.Name = name;
            this.SessionId = sessionId;
            this.Anonymous = anonymous;
            this._Data = data;
            
        }
        public Guid Id { get;private set; }

        public string Name { get; private set; }

        public string SessionId { get; private set; }

        public bool Anonymous { get; private set; }

        internal void Update(Guid id, string name,bool anonynous=false) {
            this.Id = id;
            this.Name = name;
            this.Anonymous = anonynous;
        }

        readonly private ConcurrentDictionary<string, object> _Data;
        public object this[string key] {
            get {
                
                this._Data.TryGetValue(key, out var rs);
                return rs;
            }
            set {
                this._Data.AddOrUpdate(key,value,(key,old)=>value);
            }
        }
    }
}
