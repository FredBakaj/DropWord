using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class UserSentencesCollectionConfig : BaseConfig, IEntityTypeConfiguration<UserSentencesCollectionEntity>
{
    public void Configure(EntityTypeBuilder<UserSentencesCollectionEntity> builder)
    {
        builder.ToTable("UserSentencesCollection");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserSentencesCollections)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.SentencesPairs)
            .WithMany(x => x.UserSentencesCollections)
            .UsingEntity(j => j.ToTable("SentencesPairEntityUserSentencesCollectionEntity"));
    }
}