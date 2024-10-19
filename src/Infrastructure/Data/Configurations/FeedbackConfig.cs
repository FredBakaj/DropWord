using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class FeedbackConfig : BaseConfig, IEntityTypeConfiguration<FeedbackEntity>
{
    public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
    {
        builder.ToTable("Feedback");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
    }
}
