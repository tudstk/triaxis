using FluentValidation;

namespace Triaxis.Application.Clients.Commands.CreateClient;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .MaximumLength(200).WithMessage("Client name must not exceed 200 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Client code is required.")
            .MaximumLength(50).WithMessage("Client code must not exceed 50 characters.")
            .Matches("^[A-Za-z0-9_-]+$").WithMessage("Client code can only contain letters, numbers, hyphens, and underscores.");
    }
}
