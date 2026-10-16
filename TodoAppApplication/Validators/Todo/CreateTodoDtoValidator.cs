using FluentValidation;
using TodoAppApplication.DTOs.Todo;

namespace TodoAppApplication.Validators.Todo
{
    public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
    {
        public CreateTodoDtoValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("Title is required.")
                .MaximumLength(50)
                    .WithMessage("Title cannot exceed 50 characters.");
        }
    }
}
