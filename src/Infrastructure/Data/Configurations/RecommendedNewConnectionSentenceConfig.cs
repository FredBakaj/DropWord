using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class RecommendedNewConnectionSentenceConfig : BaseConfig, IEntityTypeConfiguration<RecommendedNewConnectionSentenceEntity>
{
    public void Configure(EntityTypeBuilder<RecommendedNewConnectionSentenceEntity> builder)
    {
        builder.ToTable("RecommendedNewConnectionSentence");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.RecommendedNewFirstSentence)
            .WithMany(x => x.RecommendedNewConnectionSentences)
            .HasForeignKey(x => x.RecommendedNewFirstSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.RecommendedNewSecondSentence)
            .WithMany(x => x.RecommendedNewConnectionSentences)
            .HasForeignKey(x => x.RecommendedNewSecondSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.RecommendedNewConnectionWithUsers)
            .WithOne(x => x.RecommendedNewConnectionSentence)
            .HasForeignKey(x => x.RecommendedNewConnectionSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
