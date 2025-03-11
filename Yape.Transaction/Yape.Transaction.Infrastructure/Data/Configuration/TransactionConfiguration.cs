using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yape.Transaction.Infrastructure.Entity;

namespace Yape.Transaction.Infrastructure.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("ID");


            builder.Property(e => e.SourceAccountId)
                .HasColumnName("SourceAccountId")
                .IsRequired();

            builder.Property(e => e.TargetAccountId)
                .HasColumnName("TargetAccountId")
                .IsRequired();

            builder.Property(e => e.TransferTypeId)
                .HasColumnName("TransferTypeId")
                .IsRequired();

            builder.Property(e => e.State)
                .HasColumnName("State")
                .IsRequired();

            builder.Property(e => e.Value)
                .HasColumnName("Value")
                .IsRequired();

        }
    }
    
}
