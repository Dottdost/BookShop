namespace BookShop.Auth.SharedAuth;

public static class RegexPattern
{
    public const string Username = @"^[a-zA-Z0-9_\-]{6,}$"; // Логин: минимум 6 символов, только буквы, цифры, подчеркивания и дефисы
    public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_*$#@!%]).{8,}$"; // Пароль: минимум 8 символов, хотя бы одна строчная и заглавная буква, цифра и спецсимвол
}