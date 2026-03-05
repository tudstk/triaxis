using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Triaxis.Application.Auth.Commands.Login;
using Triaxis.Application.Auth.Commands.Register;
using Triaxis.Application.Auth.Commands.RefreshToken;

namespace Triaxis.API.Controllers.V1;

[ApiController]
[Route("api/v1/auth")]
[EnableRateLimiting("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new ProblemDetails { Detail = result.Error });

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return Unauthorized(new ProblemDetails { Detail = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return Unauthorized(new ProblemDetails { Detail = result.Error });

        return Ok(result.Value);
    }
}
