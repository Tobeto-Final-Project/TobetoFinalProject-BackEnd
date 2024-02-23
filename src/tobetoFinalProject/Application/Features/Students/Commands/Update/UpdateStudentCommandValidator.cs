using FluentValidation;

namespace Application.Features.Students.Commands.Update;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {


        RuleFor(c => c.Phone)
            .Matches(@"^\d{11}$").WithMessage("Telefon numaras� tam olarak 11 rakamdan olu�mal�d�r.");

        RuleFor(c => c.NationalIdentity)
            .Matches(@"^\d{11}$").WithMessage("TC Kimlik Numaras� 11 rakamdan olu�mal�d�r.");

        RuleFor(c => c.BirthDate)
            .InclusiveBetween(DateTime.Today.AddYears(-35), DateTime.Today.AddYears(-18))
            .WithMessage("Ya� 18 ile 35 aras�nda olmal�d�r.");

        RuleFor(c => c.Country)
            .Matches(@"^[a-zA-Z�����������]+$").WithMessage("Sadece harflerden olu�mal�d�r.")
            .MinimumLength(2).WithMessage("�lke 2 karakterden az olamaz.")
            .MaximumLength(30).WithMessage("�lke 30 karakterden �ok olamaz.");

        RuleFor(c => c.Description).MaximumLength(300).WithMessage("Hakk�mda En fazla 300 karakter olmal�");

        RuleFor(c => c.AddressDetail).MaximumLength(200).WithMessage("Adres Detaylar� En fazla 200 karakter olmal�");
    }
}