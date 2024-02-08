﻿using DropWord.Application.Common.Interfaces;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;

namespace DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;

public record GetSentenceForRepeatQuery : IRequest<SentenceForRepeatDto>
{
    public long UserId { get; set; }
}

// public class GetSentenceForRepeatQueryValidator : AbstractValidator<GetSentenceForRepeatQuery>
// {
//     public GetSentenceForRepeatQueryValidator()
//     {
//     }
// }

public class GetSentenceForRepeatQueryHandler : IRequestHandler<GetSentenceForRepeatQuery, SentenceForRepeatDto>
{
    private readonly IApplicationDbContext _context;

    public GetSentenceForRepeatQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SentenceForRepeatDto> Handle(GetSentenceForRepeatQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.UserSettings)
            .Include(x => x.UserLearningInfo)
            .FirstAsync(x => x.Id == request.UserId);


        //Получение ид последнего слова которое повторял пользователь
        var lastUseForDaySentenceId = user.UserLearningInfo.LastUseForDaySentencesId;
        //Если в базе есть ид последнего слова
        if (lastUseForDaySentenceId != null)
        {
            //Получить дату последнего слова
            var usingSentencesPairCreatedDate = await _context.UsingSentencesPair
                .Where(y => y.Id == lastUseForDaySentenceId)
                .Select(y => y.Created.Date).FirstAsync();

            // получить слово которое было добавлено после текущего и в тойже дате что и текущее
            var usingSentencesPair = await _context.UsingSentencesPair
                .Where(x => x.Id > lastUseForDaySentenceId)
                .Where(x => x.Created.Date == usingSentencesPairCreatedDate)
                .FirstOrDefaultAsync();

            //если есть слов которые было добавлено после текущего в тот же день
            if (usingSentencesPair != null)
            {
                //получаем пару слов
                var sentencesPair = await _context.SentencesPair
                    .Include(x => x.FirstSentence)
                    .Include(x => x.SecondSentence)
                    .FirstAsync(x => x.Id == usingSentencesPair.SentencesPairId);

                return CreateResponse(usingSentencesPair.Id,
                    sentencesPair.FirstSentence.Sentence,
                    sentencesPair.SecondSentence.Sentence,
                    user.UserSettings.LearnSentencesModeEnum);
            }
            //Если нету слов которые были добавленны после текущего в тотже день
            else
            {
                //получение ид слова, которое было добавлено первым в дату старше чем текущее слово
                var usingSentencesPairId = await _context.UsingSentencesPair
                    .Where(x => x.Created.Date < usingSentencesPairCreatedDate)
                    .GroupBy(x => x.Created.Date)
                    .Select(x => new { UsingSentencesPairId = x.Min(y => y.Id), Created = x.Select(y => y.Created), })
                    .FirstOrDefaultAsync();
                // если есть слова которые были добавленны в предедущие дни чем текущее слово
                if (usingSentencesPairId != null)
                {
                    var sentencesPair = await _context.UsingSentencesPair
                        .Where(x => x.Id == usingSentencesPairId.UsingSentencesPairId)
                        .Include(x => x.SentencesPair.FirstSentence)
                        .Include(x => x.SentencesPair.SecondSentence)
                        .Select(x => x.SentencesPair)
                        .FirstAsync();
                    //получение пары слов


                    return CreateResponse(usingSentencesPairId.UsingSentencesPairId,
                        sentencesPair.FirstSentence.Sentence,
                        sentencesPair.SecondSentence.Sentence,
                        user.UserSettings.LearnSentencesModeEnum);
                }
                else
                {
                    throw new OutOfSentencesToRepeatException("Finished all the sentences for repetition");
                }
            }
        }
        //если в базе пустое ид последней пары слов для повторения
        else
        {
            //берет слово которое было добавлено последним через Изучение Слова
            var lastUsingSentencesPair = await _context.UsingSentencesPair
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            //Если таблица не пуста
            if (lastUsingSentencesPair != null)
            {
                //берёт первое слово которое было добавлено в день когда было добавлено последнее
                var usingSentencesPair = await _context.UsingSentencesPair
                    .Where(x => x.Created.Date == lastUsingSentencesPair.Created.Date)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync();

                var sentencesPair = await _context.SentencesPair
                    .Include(x => x.FirstSentence)
                    .Include(x => x.SecondSentence)
                    .FirstOrDefaultAsync(x => x.Id == usingSentencesPair!.SentencesPairId);


                return CreateResponse(usingSentencesPair!.Id,
                    sentencesPair!.FirstSentence.Sentence,
                    sentencesPair.SecondSentence.Sentence,
                    user.UserSettings.LearnSentencesModeEnum);
            }

            throw new EmptyCollectionOfSentencesToRepeatException("Not have a sentence");
        }
    }

    private SentenceForRepeatDto CreateResponse(int usingSentencesPairId, string firstSentence, string secondSentence,
        LearnSentencesModeEnum learnSentencesModeEnum)
    {
        var sentenceToLearnLabel = SentenceToLearnLabelEnum.First; //добавить логику определения sentenceToLearnLabel по learnSentencesModeEnum
        return new SentenceForRepeatDto()
        {
            UsingSentencesPairId = usingSentencesPairId,
            FirstSentence = firstSentence,
            SecondSentence = secondSentence,
            SentenceToLearnLabel = sentenceToLearnLabel
        };
    }
}