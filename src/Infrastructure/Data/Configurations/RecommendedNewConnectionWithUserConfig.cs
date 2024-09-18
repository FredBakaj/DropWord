using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class RecommendedNewConnectionWithUserConfig : BaseConfig, IEntityTypeConfiguration<RecommendedNewConnectionWithUserEntity>
{
    public void Configure(EntityTypeBuilder<RecommendedNewConnectionWithUserEntity> builder)
    {
        builder.ToTable("RecommendedNewConnectionWithUser");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.RecommendedNewConnectionSentence)
            .WithMany(x => x.RecommendedNewConnectionWithUsers)
            .HasForeignKey(x => x.RecommendedNewConnectionSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.RecommendedNewConnectionWithUser)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
