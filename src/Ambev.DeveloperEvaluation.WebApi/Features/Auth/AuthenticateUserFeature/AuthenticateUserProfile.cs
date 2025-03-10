using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// AutoMapper profile for authentication-related mappings.
/// </summary>
public sealed class AuthenticateUserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserProfile"/> class.
    /// </summary>
    public AuthenticateUserProfile()
    {
        // Mapeamento de AuthenticateUserRequest -> AuthenticateUserCommand
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();

        // Mapeamento de AuthenticateUserResult -> AuthenticateUserResponse (Corrigindo o erro)
        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();

        // Mapeamento de User -> AuthenticateUserResponse
        CreateMap<User, AuthenticateUserResponse>()
            .ForMember(dest => dest.Token, opt => opt.Ignore()) // O Token n�o � mapeado diretamente
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
