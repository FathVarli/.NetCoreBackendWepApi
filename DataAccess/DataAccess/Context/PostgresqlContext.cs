using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess.Context
{
    public class PostgresqlContext : DbContext
    {
        public PostgresqlContext(DbContextOptions<PostgresqlContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseNpgsql(@"Server = localhost ; Port = 5432; Database = Core_Backend; User Id = postgres; Password = admin;");
            optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<ResetPassword> ResetPasswords { get; set; }
    }
}
