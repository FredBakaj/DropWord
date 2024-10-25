using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class AutoChatAnalysisHistoryConfig: BaseConfig, IEntityTypeConfiguration<AutoChatAnalysisHistoryEntity>
{
    public void Configure(EntityTypeBuilder<AutoChatAnalysisHistoryEntity> builder)
    {
        builder.ToTable("AutoChatAnalyzeHistory");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
    }
}
