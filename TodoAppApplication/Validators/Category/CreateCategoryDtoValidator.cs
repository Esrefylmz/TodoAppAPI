using FluentValidation;
using TodoAppApplication.DTOs.Category;

namespace TodoAppApplication.Validators.Category
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("Title is required.")
                .MaximumLength(50)
                    .WithMessage("Title cannot exceed 50 characters.");
        }



    }
}
