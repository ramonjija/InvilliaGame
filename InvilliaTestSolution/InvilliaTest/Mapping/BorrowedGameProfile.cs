using AutoMapper;
using Domain.Model.Aggregate;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Mapping
{
    public class BorrowedGameProfile : Profile
    {
        public BorrowedGameProfile()
        {
            CreateMap<BorrowedGame, GetBorrowedGameDto>()
                .ForMember(c => c.GameId, d => d.MapFrom(s => s.Game.GameId))
                .ForMember(c => c.GameName, d => d.MapFrom(s => s.Game.GameName));

            CreateMap<BorrowedGame, CreateBorrowedGameDto>()
               .ForMember(c => c.GameId, d => d.MapFrom(s => s.Game.GameId))
               .ForMember(c => c.GameName, d => d.MapFrom(s => s.Game.GameName));


            CreateMap<BorrowedGame, ReturnBorrowedGameDto>()
               .ForMember(c => c.GameId, d => d.MapFrom(s => s.Game.GameId))
               .ForMember(c => c.GameName, d => d.MapFrom(s => s.Game.GameName));
        }
    }
}
