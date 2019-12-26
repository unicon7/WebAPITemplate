using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPITemplate.Contracts.V1;
using WebAPITemplate.Options.JWT;

namespace WebAPITemplate.Controllers.V1
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly JWTSetting _jwtSetting;
        public TokenController(JWTSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }

        [HttpPost]
        public Object Post([FromBody]TokenReq tokenReq) 
        {

            List<string> ErrorMessages = new List<string>();
            ErrorMessages.Add("okokok");

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, tokenReq.Username),
                    new Claim(ClaimTypes.Name, tokenReq.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSetting.Secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                tokenReq.Username,
                tokenReq.Password,
                Token = tokenHandler.WriteToken(token),
                ErrorMessages
            });
        }
    }
}
