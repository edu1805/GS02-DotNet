using AutoMapper;
using WellWork.Application.DTOs;
using WellWork.Domain;

namespace WellWork.Application.Mappings;

public class GeneratedMessageMappingProfile : Profile
{
    public GeneratedMessageMappingProfile()
    {
        CreateMap<GeneratedMessage, GeneratedMessageDto>();
    }
}