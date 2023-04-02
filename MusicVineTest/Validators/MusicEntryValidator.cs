using FluentValidation;
using MusicVineTest.Models;

namespace MusicVineTest.Validators
{
    public class MusicEntryValidator: AbstractValidator<MusicEntry>
    {
        public MusicEntryValidator() 
        {
            RuleFor(entry => entry.Name).NotEmpty()
                .MaximumLength(255).WithMessage("Must provide a name");
            RuleFor(entry => entry.DurationSeconds)
                .GreaterThan(0).WithMessage("Duration must be longer than 0 seconds");
        }

    }
}
