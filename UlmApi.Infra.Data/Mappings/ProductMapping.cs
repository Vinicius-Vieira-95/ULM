using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UlmApi.Domain.Entities;

namespace UlmApi.Infra.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("PRODUCT");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

        }
    }
}
