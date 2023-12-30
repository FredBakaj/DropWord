using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class SentenceConfig : BaseConfig, IEntityTypeConfiguration<SentenceEntity>
{
    public void Configure(EntityTypeBuilder<SentenceEntity> builder)
    {
        builder.ToTable("Sentence");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);

        builder
            .HasMany(s => s.FirstSentencesPairs)
            .WithOne(sp => sp.FirstSentence)
            .HasForeignKey(sp => sp.FirstSentenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(s => s.SecondSentencesPairs)
            .WithOne(sp => sp.SecondSentence)
            .HasForeignKey(sp => sp.SecondSentenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}