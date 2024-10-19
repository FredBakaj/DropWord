using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class UserSettingsConfig : BaseConfig, IEntityTypeConfiguration<UserSettingsEntity>
{
    public void Configure(EntityTypeBuilder<UserSettingsEntity> builder)
    {
        builder.ToTable("UserSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.UserSettings)
            .HasForeignKey<UserSettingsEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
