using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UlmApi.Domain.Entities;

namespace UlmApi.Infra.Data.Mappings
{
    public class SolutionMapping : IEntityTypeConfiguration<Solution>
    {
        public void Configure(EntityTypeBuilder<Solution> builder)
        {
            builder.ToTable("SOLUTION");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired();

            builder.Property(s => s.OwnerId)
                .IsRequired();

            builder.Property(s => s.OwnerName)
                .IsRequired();

            builder.HasOne<Product>(p => p.Product)
                .WithMany(o => o.Solutions)
                .HasForeignKey(p => p.ProductId);

        }
    }
}