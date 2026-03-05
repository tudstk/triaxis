using FluentValidation;

namespace Triaxis.Application.Clients.Commands.UpdateClient;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Client ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .MaximumLength(200).WithMessage("Client name must not exceed 200 characters.");
    }
}
