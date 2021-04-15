using GameService.Application.Services;
using GameService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmulateGameService(this IServiceCollection services)
        {
            services.AddScoped<IEmulateGameService, EmulateGameService>();
            return services;
        }

        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Converters.AutoMapperProfile));
            return services;
        }
    }
}
