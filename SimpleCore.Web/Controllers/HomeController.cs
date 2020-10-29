using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCore.Controllers;
using SimpleCore.Models;

namespace SimpleCore.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class HomeController : LoginController
    {
        [HttpGet]
        public async Task<object> Index() {
            return this.User;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(LoginModel), 200)]
        public async Task<LoginModel> Login(LoginModel model)
        {
            return await base.DoLoginAsync(model, async (m) => {
                if (m.Username == m.Password)
                {
                    return new User(Guid.NewGuid(), m.Username,null,false,null);
                }
                return null;
            });
        }
    }
}
