using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.DTOs;
using FluentValidation;


namespace TodoList.Application.Validators
{
    public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
    {
        public CreateTodoDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title can't be empty")
                .MinimumLength(3).WithMessage("Title must have at least 3 characters")
                .MaximumLength(100).WithMessage("Title is too long");
        }
    }
}
