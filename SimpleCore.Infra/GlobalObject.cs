using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleCore
{
    public static class GlobalObject
    {
        static IConfiguration _Configuration;
        public static IConfiguration Configuration {
            get {
                return _Configuration;
            }
            set {
                if (_Configuration != null) {
                    throw new InvalidOperationException("Global.Configuration只能赋值一次");
                }
                _Configuration = value;
                JWT jwt = new JWT();
                _Configuration.Bind("JWT",jwt);
                _jwt = jwt;
            }
        }

        static JWT _jwt;
        public static JWT JWTSettings {
            get { return _jwt; }
        }

        static MemeryCache<ConcurrentDictionary<string, object>> _UserDatas;
        readonly static object Locker = new object();

        public static ConcurrentDictionary<string, object> GetUserData(string sessionId) {
            if (_UserDatas == null) {
                lock (Locker) {
                    if (_UserDatas == null) _UserDatas = new MemeryCache<ConcurrentDictionary<string, object>>(Configuration.GetValue<int?>("Session:LiveMilliseconds")??5*60*100);
                }
            }
            return _UserDatas.GetOrAdd(sessionId, (k)=>new ConcurrentDictionary<string, object>());
        }
    }
}
