using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITemplate.Contracts.V1
{
    public class TokenReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
