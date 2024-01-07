using DropWord.Domain.Entities;

namespace DropWord.Application.UseCase.StateTree.Queries.GetStateAndAction;

public class StateAndActionDto
{
    public string State { get; set; } = null!;
    public string Action { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<StateTreeEntity, StateAndActionDto>();
        }
    }
}
