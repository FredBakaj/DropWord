using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class AutoChatBotConfig :BaseConfig, IEntityTypeConfiguration<AutoChatBotEntity>
{
    public void Configure(EntityTypeBuilder<AutoChatBotEntity> builder)
    {
        builder.ToTable("AutoChatBot");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1) // Начальное значение 1, шаг 1
            .HasColumnType("int");
        
        builder
            .HasMany(s => s.AutoChatDates)
            .WithOne(sp => sp.AutoChatBot)
            .HasForeignKey(sp => sp.AutoChatBotId)
            .OnDelete(DeleteBehavior.NoAction);
        
        
    }
}
