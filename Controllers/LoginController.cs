using haber1.Context;
using haber1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static haber1.Controllers.HaberlerController;

namespace haber1.Controllers
{
    [Route("api/v1/[controller]")]
    public class LoginController : Controller
    {
        HaberContext context;

        public LoginController()
        {
            context = new HaberContext();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User login)
        {
            sonuc snc = new sonuc();
            snc.success = false;
            IActionResult response = Unauthorized();
            User user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJWT(user);
               
                if (response != null)
                {
                    snc.success = true; snc.mesaj = tokenString; snc.variables = user.UserType;
                }
                else
                {
                    snc.success=false;
                    snc.mesaj = "Böyle bir kullanıcı yok";
                }
            }
            

            else
            {
                snc.mesaj = "Böyle bir kullanıcı yok"; 
            }
            return Ok(snc);

        }

        private User AuthenticateUser(User login)
        {
            var userDb = context.Users.SingleOrDefault(usr => usr.Username == login.Username && usr.Password == login.Password);
            return userDb;
        }

        string GenerateJWT(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("metemetemetemetemete"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
           

            var claims = new[]
            {
                new Claim("user", userInfo.Username.ToString()),
                 new Claim("userid", userInfo.UserId.ToString()),
                  new Claim("usertype", userInfo.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7181/",
                audience: "https://localhost:7181/",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
