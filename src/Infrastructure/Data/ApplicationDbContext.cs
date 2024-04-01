using System.Reflection;
using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DropWord.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserSettingsEntity> UserSettings => Set<UserSettingsEntity>();
    public DbSet<StateTreeEntity> StateTree => Set<StateTreeEntity>();
    public DbSet<SentenceEntity> Sentence => Set<SentenceEntity>();
    public DbSet<SentencesPairEntity> SentencesPair => Set<SentencesPairEntity>();
    public DbSet<UserLearningInfoEntity> UserLearningInfo => Set<UserLearningInfoEntity>();
    public DbSet<UserSentencesCollectionEntity> UserSentencesCollection => Set<UserSentencesCollectionEntity>();
    public DbSet<UsingSentencesPairEntity> UsingSentencesPair => Set<UsingSentencesPairEntity>();
    public DbSet<AnalyticsUserActionEntity> AnalyticsUserAction => Set<AnalyticsUserActionEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
