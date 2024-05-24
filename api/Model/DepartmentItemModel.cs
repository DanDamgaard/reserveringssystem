using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class DepartmentItemModel
    {
        [Required]
        public int Id { get; set; }
        [Required] 
        public int ItemId { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
        public string CustomerName {  get; set; } = string.Empty;
        public string CustomerPhone {  get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
    }
}
