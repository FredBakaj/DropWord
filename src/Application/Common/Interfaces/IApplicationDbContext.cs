using DropWord.Domain.Entities;

namespace DropWord.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<UserEntity> Users { get; }
    public DbSet<UserSettingsEntity> UserSettings { get; }
    public DbSet<StateTreeEntity> StateTree { get; }
    public DbSet<SentenceEntity> Sentence { get; }
    public DbSet<SentencesPairEntity> SentencesPair { get; }
    public DbSet<UserLearningInfoEntity> UserLearningInfo { get; }
    public DbSet<UserSentencesCollectionEntity> UserSentencesCollection { get; }
    public DbSet<UsingSentencesPairEntity> UsingSentencesPair { get; }
    public DbSet<AnalyticsUserActionEntity> AnalyticsUserAction { get; }
    public DbSet<FeedbackEntity> Feedback { get; }
    public DbSet<RecommendedNewFirstSentenceEntity> RecommendedNewFirstSentence { get; }
    public DbSet<RecommendedNewSecondSentenceEntity> RecommendedNewSecondSentence { get; }
    public DbSet<RecommendedNewConnectionSentenceEntity> RecommendedNewConnectionSentence { get; }
    public DbSet<RecommendedNewConnectionWithUserEntity> RecommendedNewConnectionWithUser { get; }
    public DbSet<AutoChatDataEntity> AutoChatData { get; }
    public DbSet<AutoChatBotEntity> AutoChatBot { get; }
    public DbSet<AutoChatHistoryEntity> AutoChatHistory { get; }
    

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
