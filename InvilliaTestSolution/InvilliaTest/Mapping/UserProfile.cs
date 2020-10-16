using AutoMapper;
using Domain.Model.Entity;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                c => c.UserTypeId, d => d.MapFrom( s => s.UserType.TypeId))
                .ForMember(
                c => c.UserType, d => d.MapFrom(s => s.UserType.Type));
        }
    }
}
