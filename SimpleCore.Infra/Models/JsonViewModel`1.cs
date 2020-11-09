using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class JsonViewModel<T>:JsonViewModel
    {
        public new T Data { get; set; }

        public static JsonViewModel Ok(string message = null, T data = default)
        {
            return new JsonViewModel()
            {
                Success = true,
                Fail = false,
                Message = message,
                Data = data
            };
        }

        public static JsonViewModel Error(string message = null, T data = default)
        {
            return new JsonViewModel()
            {
                Fail = true,
                Success = false,
                Message = message,
                Data = data
            };
        }


    }
}
