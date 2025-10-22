using AutoMapper;
using TriInkom.DTO;
using TriInkom.Entities;

namespace TriInkom.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //entity -> dto
            CreateMap<Application, ApplicationDto>();

            //dto -> entity
            CreateMap<CreateApplicationDto, Application>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EApplicationStatus.Published))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));
        }
    }
}
