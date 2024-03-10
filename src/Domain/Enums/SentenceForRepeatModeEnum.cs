namespace DropWord.Domain.Enums;

public enum SentenceForRepeatModeEnum
{
    StepByQueue = 0, //get sentences by added of queue
    OldDataMinCount = 1, //get sentence that added very old and have minimum count
}
