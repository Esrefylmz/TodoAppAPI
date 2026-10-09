using FluentValidation;
using TodoAppAPI.DTOs.Category;

namespace TodoAppAPI.Validators.Category
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {

        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("Title is required.")
                .MaximumLength(50)
                    .WithMessage("Title cannot exceed 50 characters.");
        }

    }
}
