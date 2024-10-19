using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropWord.Infrastructure.Data.Configurations;

public class StateTreeConfig : BaseConfig, IEntityTypeConfiguration<StateTreeEntity>
{
    public void Configure(EntityTypeBuilder<StateTreeEntity> builder)
    {
        builder.ToTable("StateTree");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
        builder.HasOne(x => x.User)
            .WithOne(x => x.StateTree)
            .HasForeignKey<StateTreeEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
