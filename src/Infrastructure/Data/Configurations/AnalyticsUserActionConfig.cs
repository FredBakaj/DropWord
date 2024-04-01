using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class AnalyticsUserActionConfig : BaseConfig, IEntityTypeConfiguration<AnalyticsUserActionEntity>
{
    public void Configure(EntityTypeBuilder<AnalyticsUserActionEntity> builder)
    {
        builder.ToTable("AnalyticsUserAction");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
    }
}
