using HomeHub.Infrastructure.Outbox.Handlers;

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

            AddServices(services);

            AddStores(services);

            AddRepositories(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IHouseholdRepository, HouseholdRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ILowStockAlertRepository, LowStockAlertRepository>();
            services.AddScoped<IShoppingRepository, ShoppingRepository>();
        }

        private static void AddStores(IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IUserLookup, UserLookup>();

            //Outbox handlers
            services.AddScoped<IOutboxEventHandler, ShoppingItemAddedOutboxHandler>();
            services.AddScoped<IOutboxEventHandler, ShoppingItemUpdatedOutboxHandler>();
            services.AddScoped<IOutboxEventHandler, ShoppingItemDeletedOutboxHandler>();
            services.AddScoped<IOutboxEventHandler, ShoppingItemBoughtStateChangedOutboxHandler>();

            //Background services
            services.AddHostedService<OutboxProcessorHostedService>();
        }
    }
}
