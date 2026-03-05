using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triaxis.Application.Studies.Commands.ActivateStudy;
using Triaxis.Application.Studies.Commands.CreateStudy;
using Triaxis.Application.Studies.Commands.UpdateStudy;
using Triaxis.Application.Studies.Commands.UpdateStudySettings;
using Triaxis.Application.Studies.Queries.GetStudiesByClient;
using Triaxis.Application.Studies.Queries.GetStudyById;
using Triaxis.Domain.Entities;

namespace Triaxis.API.Controllers.V1;

[ApiController]
[Route("api/v1/clients/{clientId:guid}/studies")]
[Authorize]
public class StudiesController : ControllerBase
{
    private readonly ISender _sender;

    public StudiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Policy = "RequireClientAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid clientId, [FromBody] CreateStudyRequest request, CancellationToken ct)
    {
        var command = new CreateStudyCommand(clientId, request.Name, request.ProtocolNumber, request.Phase);
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new ProblemDetails { Detail = result.Error });

        return CreatedAtAction(nameof(GetById), new { clientId, id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireClientAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid clientId, Guid id, [FromBody] UpdateStudyRequest request, CancellationToken ct)
    {
        var command = new UpdateStudyCommand(id, request.Name, request.ProtocolNumber, request.Phase);
        var result = await _sender.Send(command, ct);
        if (!result.IsSuccess)
            return result.Error!.Contains("not found") ? NotFound(new ProblemDetails { Detail = result.Error }) : BadRequest(new ProblemDetails { Detail = result.Error });

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/settings")]
    [Authorize(Policy = "RequireClientAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSettings(Guid clientId, Guid id, [FromBody] StudySettingsData settings, CancellationToken ct)
    {
        var result = await _sender.Send(new UpdateStudySettingsCommand(id, settings), ct);
        if (!result.IsSuccess)
        {
            if (result.Error!.Contains("not found"))
                return NotFound(new ProblemDetails { Detail = result.Error });
            return Conflict(new ProblemDetails { Detail = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Policy = "RequireClientAdmin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Activate(Guid clientId, Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new ActivateStudyCommand(id), ct);
        if (!result.IsSuccess)
        {
            if (result.Error!.Contains("not found"))
                return NotFound(new ProblemDetails { Detail = result.Error });
            return Conflict(new ProblemDetails { Detail = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClient(
        Guid clientId,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new GetStudiesByClientQuery(clientId, search, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid clientId, Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetStudyByIdQuery(id), ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}

public record CreateStudyRequest(string Name, string ProtocolNumber, string? Phase = null);
public record UpdateStudyRequest(string Name, string ProtocolNumber, string? Phase = null);
