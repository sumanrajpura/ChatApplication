using ChatApplication.Application.Common;
using ChatApplication.Application.Contracts;
using ChatApplication.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.Configure<OfficeHours>(configuration.GetSection(nameof(OfficeHours)));
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
