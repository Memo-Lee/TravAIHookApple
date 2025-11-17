using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TravAIHookAppleWebApi.Data.Entities;

namespace TravAIHookAppleWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppLogs> AppLogs { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
