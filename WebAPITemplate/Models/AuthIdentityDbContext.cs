using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITemplate.Models
{
    public class AuthIdentityDbContext : IdentityDbContext<AuthIdentityUser>
    {
        public AuthIdentityDbContext(DbContextOptions option) : base(option)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }    
}
