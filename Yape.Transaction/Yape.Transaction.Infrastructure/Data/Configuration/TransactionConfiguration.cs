using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transaction");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");


            builder.Property(e => e.SourceAccountId)
                .HasColumnName("sourceaccountid")
                .IsRequired();

            builder.Property(e => e.TargetAccountId)
                .HasColumnName("targetaccountid")
                .IsRequired();

            builder.Property(e => e.TransferTypeId)
                .HasColumnName("transfertypeid")
                .IsRequired();

            builder.Property(e => e.State)
                .HasColumnName("state")
                .IsRequired();

            builder.Property(e => e.Value)
                .HasColumnName("value")
                .IsRequired();

        }
    }
    
}
