using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.SharedAuth;
using FluentValidation;

namespace BookShop.Auth.DataAuth.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        // Валидация имени пользователя
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required") // Проверка на пустое значение
            .Matches(RegexPattern.Username)
            .WithMessage("Username must be at least 6 characters long and contain only letters, numbers, underscores, and hyphens");

        // Валидация пароля
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Matches(RegexPattern.Password)
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one number");
    }
}