using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class UserLearningInfoConfig : BaseConfig, IEntityTypeConfiguration<UserLearningInfoEntity>
{
    public void Configure(EntityTypeBuilder<UserLearningInfoEntity> builder)
    {
        builder.ToTable("UserLearningInfo");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.UserLearningInfo)
            .HasForeignKey<UserLearningInfoEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}