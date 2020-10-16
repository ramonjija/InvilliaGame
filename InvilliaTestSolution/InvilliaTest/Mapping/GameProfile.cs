using AutoMapper;
using Domain.Model.Entity;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Mapping
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GetGameDto>()
                .ForMember(c => c.BorrowedUserId, d => d.MapFrom(s => s.BorrowedGame.Friend.UserId))
                .ForMember(c => c.BorrowedUserName, d => d.MapFrom(s => s.BorrowedGame.Friend.UserName));

            CreateMap<Game, CreateGameDto>();
            CreateMap<Game, UpdateGameDto>();
            CreateMap<Game, DeleteGameDto>();
        }
    }
}
