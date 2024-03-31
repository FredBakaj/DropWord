using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class SentencesPairConfig : BaseConfig, IEntityTypeConfiguration<SentencesPairEntity>
{
    public void Configure(EntityTypeBuilder<SentencesPairEntity> builder)
    {
        builder.ToTable("SentencesPair");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.SentencesPairs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.FirstSentence)
            .WithMany(x => x.FirstSentencesPairs)
            .HasForeignKey(x => x.FirstSentenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SecondSentence)
            .WithMany(x => x.SecondSentencesPairs)
            .HasForeignKey(x => x.SecondSentenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
