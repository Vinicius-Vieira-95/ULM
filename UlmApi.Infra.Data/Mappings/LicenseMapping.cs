using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UlmApi.Domain.Entities;

namespace UlmApi.Infra.Data.Mappings
{
    public class LicenseMapping : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.ToTable("LICENSE");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Label);

            builder.Property(p => p.Key)
                .IsRequired();
            
            builder.Property(p => p.Quantity)
                .IsRequired();
            
            builder.Property(p => p.Price);

            builder.Property(p => p.ExpirationDate)
                .IsRequired();

            builder.Property(p => p.AquisitionDate)
                .IsRequired();

            builder.Property(p => p.Justification);

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Archived);

            builder.HasOne<Solution>(p => p.Solution)
                .WithMany(o => o.Licenses)
                .HasForeignKey(p => p.SolutionId);
            
            builder.HasOne<Application>(p => p.Application)
                .WithMany(o => o.Licenses)
                .HasForeignKey(p => p.ApplicationId);
        }
    }
}