using Kladovka.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kladovka.Infrastructure.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(p => p.Sales)
              .HasColumnType("decimal(18,2)")
              .IsRequired(false);    
            
            builder.Property(p => p.Discount)
              .HasColumnType("decimal(18,2)")
              .IsRequired(false);
        }
    }
}
