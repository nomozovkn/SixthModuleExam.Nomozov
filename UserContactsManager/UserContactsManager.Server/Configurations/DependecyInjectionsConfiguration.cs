using FluentValidation;
using UserContacts.Bll.Helpers;
using UserContacts.Bll.Services;
using UserContacts.Bll.Validators;
using UserContacts.Manager.Bll.Helpers;

using UserContactsManager.Bll.Dtos;
using UserContactsManager.Bll.Repository.RepositoryService;

namespace UserContactsManager.Server.Configurations;

public static class DependecyInjectionsConfiguration
{
    public static void ConfigureDependecies(this IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
        services.AddScoped<IValidator<ContactCreateDto>, ContactCreateDtoValidator>();
        services.AddScoped<IValidator<ContactDto>, ContactDtoValidator>();
    }
}
