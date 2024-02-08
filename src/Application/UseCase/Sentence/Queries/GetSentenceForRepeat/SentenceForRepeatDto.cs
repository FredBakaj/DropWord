﻿using DropWord.Domain.Enums;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;

public class SentenceForRepeatDto
{
    public int UsingSentencesPairId { get; set; }
    public string FirstSentence { get; set; } = null!;
    public string SecondSentence { get; set; } = null!;
    public SentenceToLearnLabelEnum SentenceToLearnLabel { get; set; }
}