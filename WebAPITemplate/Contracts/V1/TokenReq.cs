using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPITemplate.Attributes;

namespace WebAPITemplate.Contracts.V1
{    
    public class TokenReq
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [AuthPassword("Username")]
        public string Password { get; set; }
    }
}
