using BookShop.Auth.Models;

namespace BookShop.Data.Models
{
    public class UserRole
    {
        public Guid UserRoleId { get; set; } = new Guid();  // Идентификатор роли пользователя

        public Guid UserId { get; set; }  // Идентификатор пользователя
        public Guid RoleId { get; set; }  // Идентификатор роли

        public Role Role { get; set; }  // Навигационное свойство для связи с ролью
        public User User { get; set; }  // Навигационное свойство для связи с пользователем
    }
}