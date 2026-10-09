using FluentValidation;
using TodoAppCore.Queries;

namespace TodoAppAPI.Validators.Todo
{
    public class TodoQueryParametersValidator: AbstractValidator<TodoQueryParameters>
    {
        public TodoQueryParametersValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");
        }
    }
}