﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class JsonViewModel
    {
       


        public static JsonViewModel Ok(string message = null,object data=null)
        {
            return new JsonViewModel()
            {
                Success = true,
                Fail = false,
                Message = message,
                Data = data
            };
        }

        public static JsonViewModel Error(string message = null,object data=null)
        {
            return new JsonViewModel()
            {
                Fail = true,
                Success = false,
                Message = message,
                Data = data
            };
        }

        

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }
        public bool Success { get; set; }

        public bool Fail { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
