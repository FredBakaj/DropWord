using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class AutoChatDataConfig: BaseConfig, IEntityTypeConfiguration<AutoChatDataEntity>
{
    public void Configure(EntityTypeBuilder<AutoChatDataEntity> builder)
    {
        builder.ToTable("AutoChatData");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder
            .HasOne(s => s.User)
            .WithMany(sp => sp.AutoChatDates)
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne(s => s.AutoChatBot)
            .WithMany(sp => sp.AutoChatDates)
            .HasForeignKey(sp => sp.AutoChatBotId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasMany(s => s.AutoChatHistories)
            .WithOne(sp => sp.AutoChatData)
            .HasForeignKey(sp => sp.AutoChatDataId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
