using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class JWT
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }
        public string SecurityKey { get; set; }

        public static User ParseToken(string token,Func<Guid,string,string,bool,User> factory=null) {
            string secret = GlobalObject.JWTSettings.SecurityKey;
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            
            var claims = handler.ValidateToken(token, validations, out var _0);
            var idStr = claims.FindFirstValue(ClaimTypes.PrimarySid);
            var nameStr = claims.FindFirstValue(ClaimTypes.Name);
            var anonymous = claims.FindFirstValue(ClaimTypes.Anonymous);
            var sessionId = claims.FindFirstValue(ClaimTypes.Sid);
            return factory!=null?
                factory(Guid.Parse(idStr), nameStr,sessionId, anonymous == "true")
                :new User(Guid.Parse(idStr),nameStr,sessionId,anonymous=="true",null);
        }

        public static string GenerateToken(IUser user)
        {
            var jwtSetting = new JWT();
            GlobalObject.Configuration.Bind("JWT", jwtSetting);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Anonymous, user.Anonymous.ToString())

             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
              issuer: jwtSetting.Issuer,
              audience: jwtSetting.Audience,
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
