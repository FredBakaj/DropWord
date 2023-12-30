using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class UsingSentencesPairConfig : BaseConfig, IEntityTypeConfiguration<UsingSentencesPairEntity>
{
    public void Configure(EntityTypeBuilder<UsingSentencesPairEntity> builder)
    {
        builder.ToTable("UsingSentencesPair");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UsingSentencesPairs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SentencesPair)
            .WithMany(x => x.UsingSentencesPairs)
            .HasForeignKey(x => x.SentencesPairId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}