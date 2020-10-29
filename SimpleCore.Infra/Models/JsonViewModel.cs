using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class JsonViewModel
    {
        public static JsonViewModel Ok(object data, string message=null) {
            return new JsonViewModel() {
                Success = true,
                Message = message,
                Data = data
            };
        }


        public static JsonViewModel Ok(string message = null)
        {
            return new JsonViewModel()
            {
                Success = true,
                Message = message,
                Data = null
            };
        }

        public static JsonViewModel Error(object data, string message = null)
        {
            return new JsonViewModel()
            {
                Fail = true,
                Message = message,
                Data = data
            };
        }

        public static JsonViewModel Error(string message = null)
        {
            return new JsonViewModel()
            {
                Fail = true,
                Message = message,
                Data = null
            };
        }
        public bool Success { get; set; }

        public bool Fail { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
