namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromServices] RegisterHandler handler,
            [FromBody] RegisterCommand cmd,
            CancellationToken ct)
        {
            var res = await handler.Handle(cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : Problem(res.Error!.Message, statusCode: 400);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromServices] LoginHandler handler,
            [FromBody] LoginCommand cmd,
            CancellationToken ct)
        {
            var res = await handler.Handle(cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : Unauthorized(res.Error!.Message);
        }
    }
}
