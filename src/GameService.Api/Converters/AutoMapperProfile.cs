using AutoMapper;
using GameService.Api.Contracts;
using GameService.Core.Models;

namespace GameService.Api.Converters
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameRequest, GameRequestSpecification>();
        }
    }
}
