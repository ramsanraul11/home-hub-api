namespace HomeHub.Api.Security
{
    public sealed class HouseholdAdminOrOwnerHandler : AuthorizationHandler<HouseholdAdminOrOwnerRequirement>
    {
        private readonly IHouseholdRepository _repo;
        public HouseholdAdminOrOwnerHandler(IHouseholdRepository repo) => _repo = repo;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HouseholdAdminOrOwnerRequirement requirement)
        {
            Guid userId;
            try { userId = CurrentUser.GetUserId(context.User); }
            catch { return; }

            var httpContext = context.Resource as HttpContext;
            if (httpContext is null && context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext afc)
                httpContext = afc.HttpContext;
            if (httpContext is null) return;

            if (!httpContext.Request.RouteValues.TryGetValue("householdId", out var raw) ||
                !Guid.TryParse(raw?.ToString(), out var householdId))
                return;

            var role = await _repo.GetRoleForUserAsync(householdId, userId, httpContext.RequestAborted);
            if (role is HouseholdRole.Owner or HouseholdRole.Admin)
                context.Succeed(requirement);
        }
    }
}
