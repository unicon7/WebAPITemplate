using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPITemplate.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Get API입니다.
        /// </summary>
        /// <remarks>xml comment를 출력하는 방법입니다.</remarks>
        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(new { Hello = "v1 안녕하세요!", Test = "이상하네요" });
        }        
    }
}