namespace HomeHub.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterHandler>();
            services.AddScoped<LoginHandler>();
            return services;
        }
    }
}
