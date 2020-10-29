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

        protected async Task<LoginModel> DoLoginAsync(LoginModel model, Func<LoginModel, Task<IUser>> handleLogin) {
            var user = await handleLogin(model);
            return DoLogin(model,user);
        }
    }
}
