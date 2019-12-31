using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
                    new Claim(ClaimTypes.Email, tokenReq.Username),
                    new Claim(ClaimTypes.Name, tokenReq.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSetting.Secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = GenerateRefreshToken();

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                RereshToken = refreshToken
            });
        }

        //[HttpPost]
        //public Object Post([FromBody]RefreshTokenReq refreshTokenReq)
        //{
 
        //    var principal = GetPrincipalFromExpiredToken(refreshTokenReq.Token);
            
        //    //List<string> ErrorMessages = new List<string>();
        //    //ErrorMessages.Add("okokok");

        //    // authentication successful so generate jwt token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(JwtRegisteredClaimNames.Email, tokenReq.Username),
        //            new Claim(ClaimTypes.Name, tokenReq.Username)
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSetting.Secret), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return Ok(new
        //    {
        //        Token = tokenHandler.WriteToken(token),
        //        RefreshToken =
        //    }); ;
        //}

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) 
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtSetting.Secret),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            //unicon7: SecurityAlgorithms.HmacSha256을 체크하는 이유 SecurityAlgorithms.None을 사용해서 ValidateToken을 통과할수 있기때문에!
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        //unicon7 : GUIDs is not random
        [NonAction]

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
