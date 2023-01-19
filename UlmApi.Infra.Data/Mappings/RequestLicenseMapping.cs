using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UlmApi.Domain.Entities;

namespace UlmApi.Infra.Data.Mappings
{
    public class RequestLicenseMapping : IEntityTypeConfiguration<RequestLicense>
    {
        public void Configure(EntityTypeBuilder<RequestLicense> builder)
        {
            builder.ToTable("REQUEST_LICENSE");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.RegistrationDate);

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.UsageTime)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired();
            
            builder.Property(p => p.Reason)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Percentage)
                .IsRequired(false);

            builder.Property(p => p.Prediction)
                .IsRequired(false);

            builder.Property(p => p.Message)
                .IsRequired(false);

            builder.Property(p => p.Justification);

            builder.Property(p => p.JustificationForDeny);

            builder.HasOne<Application>(p => p.Application)
                .WithMany(o => o.RequestLicenses)
                .HasForeignKey(p => p.ApplicationId);

            builder.HasOne<Product>(p => p.Product)
                .WithMany(o => o.RequestLicenses)
                .HasForeignKey(p => p.ProductId);

            builder.HasOne<Solution>(p => p.Solution)
                .WithMany(o => o.RequestLicenses)
                .HasForeignKey(p => p.SolutionId);

            builder.HasOne<License>(p => p.License)
                .WithOne(o => o.Request)
                .HasForeignKey<RequestLicense>(o => o.LicenseId);
            
        }
    }
}