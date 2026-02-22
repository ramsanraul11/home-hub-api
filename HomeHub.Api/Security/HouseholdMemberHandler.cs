namespace HomeHub.Api.Security
{
    public sealed class HouseholdMemberHandler : AuthorizationHandler<HouseholdMemberRequirement>
    {
        private readonly IHouseholdRepository _repo;

        public HouseholdMemberHandler(IHouseholdRepository repo) => _repo = repo;

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HouseholdMemberRequirement requirement)
        {
            // 1) UserId
            Guid userId;
            try { userId = CurrentUser.GetUserId(context.User); }
            catch { return; }

            // 2) HttpContext (válido en controllers y también en muchos casos de endpoint routing)
            var httpContext = context.Resource as HttpContext;

            // fallback: MVC filter context
            if (httpContext is null && context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext afc)
                httpContext = afc.HttpContext;

            if (httpContext is null)
                return;

            // 3) Obtener householdId del route
            if (!TryGetHouseholdId(httpContext, out var householdId))
                return;

            var isMember = await _repo.IsMemberAsync(householdId, userId, httpContext.RequestAborted);
            if (isMember)
                context.Succeed(requirement);
        }

        private static bool TryGetHouseholdId(HttpContext httpContext, out Guid householdId)
        {
            householdId = default;

            // Prioriza householdId
            if (httpContext.Request.RouteValues.TryGetValue("householdId", out var raw) &&
                Guid.TryParse(raw?.ToString(), out householdId))
                return true;

            // fallback por si tu ruta usa {id}
            if (httpContext.Request.RouteValues.TryGetValue("id", out raw) &&
                Guid.TryParse(raw?.ToString(), out householdId))
                return true;

            return false;
        }
    }
}
