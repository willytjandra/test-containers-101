using Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).HasMaxLength(100);

        builder.Property(c => c.LastName).HasMaxLength(100);

        builder.Property(c => c.Email).HasMaxLength(255);

        builder.HasIndex(c => c.Email)
            .IsUnique();
    }
}
