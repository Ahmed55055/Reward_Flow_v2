using FluentValidation;
using static Reward_Flow_v2.Employees.CreateEmployee.CreateEmployee;

namespace Reward_Flow_v2.Employees.CreateEmployee;

public class CreateEmployeeRequestValidator : AbstractValidator<Request>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Salary).GreaterThanOrEqualTo(0).When(x => x.Salary.HasValue);
    }
}