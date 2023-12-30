using AutoMapper;
using DropWord.TgBot.Core.Model;
using Telegram.Bot.Types;

namespace DropWord.TgBot.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UpdateBDto, Update>().ReverseMap();
        }
    }
}
