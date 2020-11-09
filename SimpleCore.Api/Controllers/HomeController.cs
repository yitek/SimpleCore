using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SimpleCore.Api.Models;
using SimpleCore.Controllers;
using SimpleCore.Domains;
using SimpleCore.Models;

namespace SimpleCore.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [SimpleCore.Controllers.ApiController("1.0")]
    public class HomeController : LoginController
    {
        MyDbContext _context;
        /// <summary>
        /// 通用的不需要登录的api
        /// </summary>
        /// <param name="context"></param>
        public HomeController(MyDbContext context) {
            this._context = context;
        }
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
        [Route("/login")]
        [ProducesResponseType(typeof(JsonViewModel<LoginModel>), 200)]
        public async Task<JsonViewModel> Login(LoginModel model)
        {
            var result = await base.DoLoginAsync(model, async (m) => {
                if (m.Username == m.Password)
                {
                    return new User(Guid.NewGuid(), m.Username, null, false, null);
                }
                return null;
            });
            return result == null ? Error("登录失败", model) : Success("登录成功",result);
        }
        /// <summary>
        /// 获取当前用户权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/permissions")]
        [ProducesResponseType(typeof(JsonViewModel<PermissionViewModel>), 200)]

        public async Task<JsonViewModel> Permissions() {
            var perms = new PermissionViewModel();
            var access = perms.Access =await this._context.Func.ToListAsync();
            return Success("成功获取权限",perms);

        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("logout")]
        //[ProducesResponseType(typeof(LoginModel), 200)]
        public async Task<JsonViewModel> Logout(User model)
        {
            await base.DoLogoutAsync(model);
            return Success("登出成功");
           
        }


    }
}
