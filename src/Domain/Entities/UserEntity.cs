﻿namespace DropWord.Domain.Entities;

public class UserEntity : BaseAuditableEntity<long>
{
    public StateTreeEntity StateTree { get; set; } = null!;
    public UserSettingsEntity UserSettings { get; set; } = null!;
    public UserLearningInfoEntity UserLearningInfo { get; set; } = null!;

    public List<SentencesPairEntity> SentencesPairs { get; set; } = null!;
    public List<UsingSentencesPairEntity> UsingSentencesPairs { get; set; } = null!;
    public List<UserSentencesCollectionEntity> UserSentencesCollections { get; set; } = null!;
}