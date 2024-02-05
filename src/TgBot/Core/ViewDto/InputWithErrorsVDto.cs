﻿namespace DropWord.TgBot.Core.ViewDto;

public class InputWithErrorsVDto : BaseVDto
{
    public string RightSentence { get; set; } = null!;
    public string CorrectedSentence { get; set; } = null!;
    public string NextSentence { get; set; } = null!;
}
