using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class ControllerBase: Microsoft.AspNetCore.Mvc.ControllerBase
    {
        

        User _User;

        public new User User {
            get {
                if (this._User == null) {
                    var token = this.Request.Cookies["Autherization"];
                    if (token == null) {
                        token = Request.Headers["Authorization"];
                        if(token!=null) token = token.Replace("Bearer ", "");
                    }
                    if (token == null) {
                        var id = Guid.NewGuid();
                        var sessionId = id.ToString();
                        this._User = new User(id,"匿名用户", sessionId, true,GlobalObject.GetUserData(sessionId));
                        this.FlushToken(id,"匿名用户",true);
                    }
                    if (this._User == null)
                    {
                        if (string.IsNullOrWhiteSpace(token)) throw new Exception("请在header或cookie中添加Authorization域");
                        this._User = JWT.ParseToken(token, (id, name, sessionId, anonymous) => new User(id, name, sessionId, anonymous, GlobalObject.GetUserData(sessionId)));
                    }
                }
                return this._User;
            }
            set {
                this._User = value;
            }
        }
        internal string FlushToken(Guid id,string name,bool anonymous=false) {
            this.User.Update(id,name, anonymous);
            var token = JWT.GenerateToken(this._User);
            if (this.Response.Headers.ContainsKey("Authorization")) this.Response.Headers.Remove("Authorization");
            this.Response.Headers.Add("Authorization", "Bearer " + token);
            
            this.Response.Cookies.Append("Authorization", token);
            return token;
        }


        protected JsonViewModel Error(string message=null,object data= null) {
            return JsonViewModel.Error(message,data);
        }

        protected JsonViewModel Success(string message=null,object data=null)
        {
            return JsonViewModel.Ok(message,data);
        }

    }
}
