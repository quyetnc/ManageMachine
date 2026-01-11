using ManageMachine.Application.Common;
using ManageMachine.Infrastructure.Authentication;
using ManageMachine.Infrastructure.Persistence;
using ManageMachine.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ManageMachine.Infrastructure.Persistence.Seeders;

namespace ManageMachine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<DbSeeder>(); // Register Seeder

            return services;
        }
    }
}
