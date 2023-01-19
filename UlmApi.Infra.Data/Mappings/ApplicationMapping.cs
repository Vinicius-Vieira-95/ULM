using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UlmApi.Domain.Entities;

namespace UlmApi.Infra.Data.Mappings
{
    public class ApplicationMapping : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("APPLICATION");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}