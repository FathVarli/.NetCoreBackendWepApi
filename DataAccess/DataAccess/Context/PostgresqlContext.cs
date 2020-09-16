using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess.Context
{
    public class PostgresqlContext : DbContext
    {
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseNpgsql(@"yourdbstring");
            optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<ResetPassword> ResetPasswords { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
