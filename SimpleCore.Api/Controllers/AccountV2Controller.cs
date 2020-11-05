using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCore.Api.Controllers.V2
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Index()
        {
            return this.User;
        }

        [HttpGet]
        [Route("KK")]
        public object KK()
        {
            return this.User;
        }

    }
}
