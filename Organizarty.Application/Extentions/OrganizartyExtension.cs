using Organizarty.Domain.UseCases.Users;
using Organizarty.Application.Services;

namespace Organizarty.Application.Extensions;

public static class OrganizartyExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISignUseCase, SignService>();

        return services;
    }
}
