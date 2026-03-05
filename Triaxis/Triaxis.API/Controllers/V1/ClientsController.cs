using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triaxis.Application.Clients.Commands.CreateClient;
using Triaxis.Application.Clients.Commands.DeactivateClient;
using Triaxis.Application.Clients.Commands.UpdateClient;
using Triaxis.Application.Clients.Queries.GetClientById;
using Triaxis.Application.Clients.Queries.GetClients;

namespace Triaxis.API.Controllers.V1;

[ApiController]
[Route("api/v1/clients")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly ISender _sender;

    public ClientsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Policy = "RequireSuperAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new ProblemDetails { Detail = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireSuperAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new UpdateClientCommand(id, request.Name), ct);
        if (!result.IsSuccess)
            return result.Error!.Contains("not found") ? NotFound(new ProblemDetails { Detail = result.Error }) : BadRequest(new ProblemDetails { Detail = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Policy = "RequireSuperAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new DeactivateClientCommand(id), ct);
        if (!result.IsSuccess)
            return NotFound(new ProblemDetails { Detail = result.Error });

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetClientsQuery(search, isActive, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetClientByIdQuery(id), ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}

public record UpdateClientRequest(string Name);
