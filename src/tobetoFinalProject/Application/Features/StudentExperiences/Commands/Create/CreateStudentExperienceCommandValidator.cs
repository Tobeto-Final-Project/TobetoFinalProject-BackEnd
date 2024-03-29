using FluentValidation;

namespace Application.Features.StudentExperiences.Commands.Create;

public class CreateStudentExperienceCommandValidator : AbstractValidator<CreateStudentExperienceCommand>
{
    public CreateStudentExperienceCommandValidator()
    {
        RuleFor(c => c.CompanyName).NotEmpty().WithMessage("Kurum alan� bo� b�rak�lamaz.").Length(5, 50).WithMessage("CompanyName en az 5, en fazla 50 karakter olmal�d�r.");
        RuleFor(c => c.Sector).NotEmpty();
        RuleFor(c => c.Position).NotEmpty().WithMessage("Pozisyon alan� bo� b�rak�lamaz.")
            .Length(5, 50).WithMessage("Position en az 5, en fazla 50 karakter olmal�d�r.");
        ;
        RuleFor(c => c.StartDate).NotEmpty();
        RuleFor(c => c.CityId).NotEmpty();

        RuleFor(x => x.StartDate)
             .LessThan(x => x.EndDate)
             .WithMessage("Ba�lang�� tarihi biti� tarihinden �nce olmal�d�r.")
             .When(x => x.EndDate != null);

    }
}