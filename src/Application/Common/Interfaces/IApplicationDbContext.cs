using DropWord.Domain.Entities;

namespace DropWord.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<UserEntity> Users {get;}
    public DbSet<UserSettingsEntity> UserSettings {get;}
    public DbSet<StateTreeEntity> StateTree {get;}
    public DbSet<SentenceEntity> Sentence {get;}
    public DbSet<SentencesPairEntity> SentencesPair {get;}
    public DbSet<UserLearningInfoEntity> UserLearningInfo {get;}
    public DbSet<UserSentencesCollectionEntity> UserSentencesCollection {get;}
    public DbSet<UsingSentencesPairEntity> UsingSentencesPair {get;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
