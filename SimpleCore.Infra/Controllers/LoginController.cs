using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SimpleCore.Models;

namespace SimpleCore.Controllers
{
    public class LoginController : ControllerBase
    {
        protected LoginModel DoLogin(LoginModel model,IUser user)
        {
            
            model.Token = this.FlushToken(user.Id, user.Name, false);
            model.User = new User(user.Id, user.Name, this.User.SessionId, false, null);

            return model;
        }

        protected async Task<LoginModel> DoLoginAsync(LoginModel model, Func<LoginModel, Task<IUser>> checkUserHandler) {
            var user = await checkUserHandler(model);
            if (user == null) return null;
            return DoLogin(model,user);
        }

        protected async Task DoLogoutAsync(User user,Func<User,User, Task<bool>> beforeLogout=null) {
            if (beforeLogout != null && await beforeLogout(this.User,user)) {
               
                this.FlushToken(user.Id,user.Name, true);
            }
           

        }
    }
}
