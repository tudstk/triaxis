using FluentValidation;

namespace Triaxis.Application.Studies.Commands.UpdateStudy;

public class UpdateStudyCommandValidator : AbstractValidator<UpdateStudyCommand>
{
    public UpdateStudyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Study ID is required.");

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
