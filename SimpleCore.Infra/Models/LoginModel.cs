using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class LoginModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public User User { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string Token { get; set; }
    }
}
