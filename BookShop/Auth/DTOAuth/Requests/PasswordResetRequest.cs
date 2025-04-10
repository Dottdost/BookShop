namespace BookShop.Auth.DTOAuth.Requests;

public class ResetPasswordRequest
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
}

