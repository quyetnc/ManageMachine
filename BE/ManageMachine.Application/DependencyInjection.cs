using Microsoft.Extensions.DependencyInjection;
using ManageMachine.Application.Common;
using ManageMachine.Application.Services;
using ManageMachine.Application.Services.Implementations;
using System.Reflection;

namespace ManageMachine.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMachineService, MachineService>();
            services.AddScoped<IMachineTypeService, MachineTypeService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
