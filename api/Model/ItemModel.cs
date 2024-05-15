using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class ItemModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
    }
}
