using Organizarty.Domain.UseCases.Users;
using Organizarty.Application.app.Services;

namespace Organizarty.Application.app.Extensions;

public static class OrganizartyExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISignUseCase, SignService>();

        return services;
    }
}
