using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITemplate.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class Test2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(new
            {
                Hello = "v2 test2입니다."
            });
        }
    }
}