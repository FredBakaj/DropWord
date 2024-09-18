using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class RecommendedNewFirstSentenceConfig: BaseConfig, IEntityTypeConfiguration<RecommendedNewFirstSentenceEntity>
{
    public void Configure(EntityTypeBuilder<RecommendedNewFirstSentenceEntity> builder)
    {
        builder.ToTable("RecommendedNewFirstSentence");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasMany(x => x.RecommendedNewConnectionSentences)
            .WithOne(x => x.RecommendedNewFirstSentence)
            .HasForeignKey(x => x.RecommendedNewFirstSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
