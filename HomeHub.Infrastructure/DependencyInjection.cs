namespace HomeHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(cs));

            services
                .AddIdentityCore<AppUser>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AppDbContext>();

            //TODO: Adapters (hexagonal)(Extraer a un metodo addServices?)
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
