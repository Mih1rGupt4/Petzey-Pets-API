using Microsoft.IdentityModel.Tokens;
using Petzey.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("jwttoken")]
        public IHttpActionResult GetJWTToken([FromBody] TokenDTO user)
        {
            if (user == null)
            {
                return BadRequest("Body of the request is empty");
            }

            string Token = CreateJWT(user);


            return Ok($"{Token}");



        }

        private string CreateJWT(TokenDTO user)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes("X5eXk4xyojNFum1kl2Ytv8dlNP4-c57dO6QGTVBwaNk");
            ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
            {
         new Claim(ClaimTypes.Role,user.Role),
         new Claim(ClaimTypes.Name,user.Name),
     new Claim(ClaimTypes.Sid,user.UID)
            });
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,

            };
            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }
    }
}