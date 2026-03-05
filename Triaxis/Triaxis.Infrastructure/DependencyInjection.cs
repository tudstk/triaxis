using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Infrastructure.Identity;
using Triaxis.Infrastructure.Persistence;
using Triaxis.Infrastructure.Persistence.Interceptors;
using Triaxis.Infrastructure.Persistence.Seeding;

namespace Triaxis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<TriaxisDbContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();
            var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>();

            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(softDeleteInterceptor, auditInterceptor);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<TriaxisDbContext>());

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<TriaxisDbContext>()
        .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddHostedService<DefaultVisitDefinitionSeeder>();

#pragma warning disable CS0618
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            };
        });
#pragma warning restore CS0618

        return services;
    }
}
