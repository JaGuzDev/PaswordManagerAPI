using AutoMapper;
using PasswordManager.Infrastructure.Entity;
using PasswordManager.Model.Dto;
using PasswordManager.Model.ViewModel;

namespace PasswordManager.Model.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /* From Model to ViewModel */

            CreateMap<AuthToken, AuthTokenViewModel>()
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.LastUpdatedBy, 
                    opt => opt.MapFrom(src => 
                        src.UpdatedBy != null && !string.IsNullOrEmpty(src.UpdatedBy.FullName)
                            ? src.UpdatedBy.FullName 
                            : src.CreatedBy.FullName))
                .ForMember(dest => dest.LastUpdatedOn,
                    opt => opt.MapFrom(src =>
                        src.UpdatedAt != null
                            ? src.UpdatedAt
                            : src.CreatedAt))
                .ReverseMap();

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.LastUpdatedBy,
                    opt => opt.MapFrom(src =>
                        src.UpdatedBy != null && !string.IsNullOrEmpty(src.UpdatedBy.FullName)
                            ? src.UpdatedBy.FullName
                            : src.CreatedBy.FullName))
                .ForMember(dest => dest.LastUpdatedOn,
                    opt => opt.MapFrom(src =>
                        src.UpdatedAt != null
                            ? src.UpdatedAt
                            : src.CreatedAt))
                .ReverseMap();            

            CreateMap<Entry, EntryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.LastUpdatedBy,
                    opt => opt.MapFrom(src =>
                        src.UpdatedBy != null && !string.IsNullOrEmpty(src.UpdatedBy.FullName)
                            ? src.UpdatedBy.FullName
                            : src.CreatedBy.FullName))
                .ForMember(dest => dest.LastUpdatedOn,
                    opt => opt.MapFrom(src =>
                        src.UpdatedAt != null
                            ? src.UpdatedAt
                            : src.CreatedAt))
                .ReverseMap();

            /* From ViewModel to Model*/

            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.AuthenticationSalt, opt => opt.MapFrom(src => src.AuthenticationSalt))
                .ForMember(dest => dest.EncryptionSalt, opt => opt.MapFrom(src => src.EncryptionSalt))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.BadPwdCount, opt => opt.MapFrom(src => src.BadPwdCount));

            /* From DTO to Model */

            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // To be hashed
                .ForMember(dest => dest.AuthenticationSalt, opt => opt.Ignore()) // To be set server-side
                .ForMember(dest => dest.EncryptionSalt, opt => opt.Ignore()) // To be set server-side
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BadPwdCount, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationResult, opt => opt.Ignore());

            /* From Model to Model*/

            CreateMap<User, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationResult, opt => opt.Ignore())
                .ForMember(dest => dest.AuthenticationSalt, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptionSalt, opt => opt.Ignore());

            CreateMap<Entry, Entry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InitializationVector, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationResult, opt => opt.Ignore());
        }
    }
}
