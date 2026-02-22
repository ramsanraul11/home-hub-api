namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController : ControllerBase
    {
        private string? UserAgent => Request.Headers.UserAgent.ToString();
        private string? Ip => HttpContext.Connection.RemoteIpAddress?.ToString();

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromServices] RegisterHandler handler,
            [FromBody] RegisterCommand cmd,
            CancellationToken ct)
        {
            var res = await handler.Handle(cmd, UserAgent, Ip, ct);
            return res.IsSuccess ? Ok(res.Value) : Problem(res.Error!.Message, statusCode: 400);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromServices] LoginHandler handler,
            [FromBody] LoginCommand cmd,
            CancellationToken ct)
        {
            var res = await handler.Handle(cmd, UserAgent, Ip, ct);
            return res.IsSuccess ? Ok(res.Value) : Unauthorized(res.Error!.Message);
        }

        public sealed record RefreshRequest(string RefreshToken);

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromServices] RefreshHandler handler,
            [FromBody] RefreshRequest req,
            CancellationToken ct)
        {
            var res = await handler.Handle(new RefreshCommand(req.RefreshToken), UserAgent, Ip, ct);
            return res.IsSuccess ? Ok(res.Value) : Unauthorized(res.Error!.Message);
        }

        public sealed record LogoutRequest(string RefreshToken);

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromServices] LogoutHandler handler,
            [FromBody] LogoutRequest req,
            CancellationToken ct)
        {
            var res = await handler.Handle(new LogoutCommand(req.RefreshToken), Ip, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error!.Message);
        }
    }
}
