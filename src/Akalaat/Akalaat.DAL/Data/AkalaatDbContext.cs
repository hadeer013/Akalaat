using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Data
{
    public class AkalaatDbContext : IdentityDbContext<ApplicationUser>
    {
        public AkalaatDbContext(DbContextOptions<AkalaatDbContext> options) : base(options)
        {
        }
    }
}
