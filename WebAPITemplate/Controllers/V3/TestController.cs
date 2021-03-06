﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPITemplate.Controllers.V3
{ 
    [ApiVersion("3")]
    [ApiVersion("4")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet, MapToApiVersion("3")]
        public string Get()
        {
            return "v3 안녕하세요!";
        }

        [HttpGet, MapToApiVersion("4")]
        public string Get_V4()
        {
            return "v4 안녕하세요!";
        }
    }
}