using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int Department { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
