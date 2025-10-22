using FluentValidation;
using TriInkom.Entities;

namespace TriInkom.DTO
{
    public class ApplicationDto
    {
        public string Number { get; set; }
        public EApplicationStatus Status { get; set; }
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }

    public class CreateApplicationDto
    {
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public int TermValue { get; set; }
        public decimal InterestValue { get; set; }
    }

    public class ApplicationFilterDto
    {
        public EApplicationStatus? Status { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int? MinTerm { get; set; }
        public int? MaxTerm { get; set; }
    }
    public class CreateApplicationDtoValidator : AbstractValidator<CreateApplicationDto>
    {
        public CreateApplicationDtoValidator()
        {
            RuleFor(x => x.Number).NotEmpty().WithMessage("Номер заявки должен быть указан");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Сумма займа должна превышать 0");
            RuleFor(x => x.TermValue).GreaterThan(0).WithMessage("Срок займа должнен превышать 0");
            RuleFor(x => x.InterestValue).GreaterThan(0).WithMessage("Процентная ставка займа должна превышать 0");
        }
    }
}
