using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Api.Models
{
    public class SmsContext : DbContext
    {
        public SmsContext(DbContextOptions<SmsContext> options)
            : base(options)
        {

        }

        public DbSet<SmsItem> SmsItems
        {
            get;
            set;
        }
    }
}