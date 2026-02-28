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
            services.AddScoped<AddMemberHandler>();
            services.AddScoped<ListMembersHandler>();
            services.AddScoped<RemoveMemberHandler>();
            services.AddScoped<CreateTaskHandler>();
            services.AddScoped<ListTasksHandler>();
            services.AddScoped<AssignTaskHandler>();
            services.AddScoped<CompleteTaskHandler>();
            services.AddScoped<CreateNoticeHandler>();
            services.AddScoped<ListNoticesHandler>();
            services.AddScoped<ArchiveNoticeHandler>();
            services.AddScoped<GetNoticeHandler>();
            services.AddScoped<UpdateNoticeHandler>();
            services.AddScoped<CreateCategoryHandler>();
            services.AddScoped<CreateItemHandler>();
            services.AddScoped<ListCategoriesHandler>();
            services.AddScoped<ListItemsHandler>();
            services.AddScoped<UpdateItemQuantityHandler>();
            services.AddScoped<LowStockAlertProjector>();
            services.AddScoped<ListLowStockAlertsHandler>();
            services.AddScoped<ResolveLowStockAlertHandler>();
            return services;
        }
    }
}
