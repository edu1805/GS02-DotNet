using AutoMapper;
using WellWork.Application.DTOs;
using WellWork.Domain;

namespace WellWork.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Domain → Response DTO
        CreateMap<User, UserResponseDto>();

        // Create DTO → Domain
        CreateMap<UserCreateDto, User>()
            .ConstructUsing(dto =>
                new User(
                    Guid.NewGuid(),
                    dto.Username,
                    dto.PasswordHash
                )
            );

        // Update DTOs → Domain (aplicados manualmente no service)
        CreateMap<UserUpdatePasswordDto, User>()
            .ForAllMembers(opt => opt.Ignore());
    }
}