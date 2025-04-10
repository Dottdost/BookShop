using System.Threading.Tasks;
namespace BookShop.Auth.ServicesAuth.Interfaces

{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
