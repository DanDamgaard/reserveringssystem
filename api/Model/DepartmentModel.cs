using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class DepartmentModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
