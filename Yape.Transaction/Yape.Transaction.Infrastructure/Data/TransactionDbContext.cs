using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TransactionEntity> Transactions { get; set; }
    }
}
