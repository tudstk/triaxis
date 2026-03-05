using FluentValidation;

namespace Triaxis.Application.Studies.Commands.CreateStudy;

public class CreateStudyCommandValidator : AbstractValidator<CreateStudyCommand>
{
    public CreateStudyCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Client ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Study name is required.")
            .MaximumLength(300).WithMessage("Study name must not exceed 300 characters.");

        RuleFor(x => x.ProtocolNumber)
            .NotEmpty().WithMessage("Protocol number is required.")
            .MaximumLength(100).WithMessage("Protocol number must not exceed 100 characters.");

        RuleFor(x => x.Phase)
            .MaximumLength(50).WithMessage("Phase must not exceed 50 characters.")
            .When(x => x.Phase is not null);
    }
}
