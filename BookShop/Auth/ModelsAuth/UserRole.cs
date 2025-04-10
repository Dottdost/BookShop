namespace BookShop.Auth.ModelsAuth
{
    public class UserRole
    {
        public int UserRoleId { get; set; }  // Идентификатор роли пользователя

        public int UserId { get; set; }  // Идентификатор пользователя
        public int RoleId { get; set; }  // Идентификатор роли

        public Role Role { get; set; }  // Навигационное свойство для связи с ролью
        public User User { get; set; }  // Навигационное свойство для связи с пользователем
    }
}
