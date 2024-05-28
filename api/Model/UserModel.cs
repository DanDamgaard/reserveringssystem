using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required]
        public int Department { get; set; }
        [Required]
        public string UserRole { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

    }
}
