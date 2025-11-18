using AutoMapper;
using WellWork.Application.DTOs;
using WellWork.Domain;

namespace WellWork.Application.Mappings;

public class CheckInMappingProfile : Profile
{
    public CheckInMappingProfile()
    {
        // Domain → Response DTO
        CreateMap<CheckIn, CheckInResponseDto>()
            .ForMember(dest => dest.GeneratedMessage, opt =>
                opt.MapFrom(src => src.GeneratedMessage));

        // Create DTO → Domain
        CreateMap<CheckInCreateDto, CheckIn>()
            .ConstructUsing(dto =>
                new CheckIn(
                    Guid.NewGuid(),
                    dto.UserId,
                    dto.Mood,
                    dto.Energy,
                    dto.Notes
                )
            );
    }
}