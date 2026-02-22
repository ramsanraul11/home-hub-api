namespace HomeHub.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<RefreshHandler>();
            services.AddScoped<LogoutHandler>();
            services.AddScoped<CreateHouseholdHandler>();
            services.AddScoped<ListMyHouseholdsHandler>();
            services.AddScoped<GetHouseholdHandler>();
            return services;
        }
    }
}
