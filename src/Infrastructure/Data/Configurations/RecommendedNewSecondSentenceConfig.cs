using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class RecommendedNewSecondSentenceConfig : BaseConfig, IEntityTypeConfiguration<RecommendedNewSecondSentenceEntity>
{
    public void Configure(EntityTypeBuilder<RecommendedNewSecondSentenceEntity> builder)
    {
        builder.ToTable("RecommendedNewSecondSentence");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasMany(x => x.RecommendedNewConnectionSentences)
            .WithOne(x => x.RecommendedNewSecondSentence)
            .HasForeignKey(x => x.RecommendedNewSecondSentenceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
