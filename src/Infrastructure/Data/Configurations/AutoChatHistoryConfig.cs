using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class AutoChatHistoryConfig: BaseConfig, IEntityTypeConfiguration<AutoChatHistoryEntity>
{
    public void Configure(EntityTypeBuilder<AutoChatHistoryEntity> builder)
    {
        builder.ToTable("AutoChatHistory");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder
            .HasOne(s => s.AutoChatData)
            .WithMany(sp => sp.AutoChatHistories)
            .HasForeignKey(sp => sp.AutoChatDataId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
