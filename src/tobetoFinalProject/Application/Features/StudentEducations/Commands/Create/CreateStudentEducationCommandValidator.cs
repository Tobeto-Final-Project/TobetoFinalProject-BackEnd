using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.StudentEducations.Commands.Create;

public class CreateStudentEducationCommandValidator : AbstractValidator<CreateStudentEducationCommand>
{
    public CreateStudentEducationCommandValidator()
    {
        RuleFor(c => c.EducationStatus).NotEmpty();
        RuleFor(c => c.SchoolName).NotEmpty().WithMessage("Okul alan� bo� b�rak�lamaz.").MinimumLength(2).WithMessage("Okul alan� en az 2 karakter olmal�d�r.");
        RuleFor(c => c.Branch).NotEmpty().WithMessage("B�l�m alan� bo� b�rak�lamaz.").Length(2, 50).WithMessage("B�l�m alan� en az 2, en fazla 50 karakter olmal�d�r.");
        ;

        RuleFor(c => c.StartDate).NotEmpty();

        RuleFor(x => x.GraduationDate)
            .GreaterThan(x => x.StartDate).WithMessage("Mezuniyet Tarihi alan� ba�lang�� tarihinden sonra olmal�d�r.");

        RuleFor(c => c.GraduationDate)
            .NotEmpty()
            .When(c => c.IsContinued == false)
            .WithMessage("E�er devam edilmiyorsa mezuniyet tarihi bo� olmamal�d�r.")
            .Must(c => c == null)
            .When(c => c.IsContinued == true)
            .WithMessage("E�er devam ediyorsa mezuniyet tarihi girilemez.");

        RuleFor(c => c.IsContinued)
            .Must(isContinued => isContinued).When(c => c.GraduationDate == null)
            .WithMessage("E�er mezuniyet tarihi belirtilmediyse, devam edip etmedi�i belirtilmelidir.");
    }
}