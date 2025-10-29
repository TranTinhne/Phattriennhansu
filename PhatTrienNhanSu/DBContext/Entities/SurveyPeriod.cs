using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("SurveyPeriods")]
    public class SurveyPeriod : BaseEntity // Kế thừa từ BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Các cột Id, Status, CreatedBy, CreatedDate... đã có trong BaseEntity

        public virtual ICollection<EmployeeSurveyResponse> SurveyResponses { get; set; } = new List<EmployeeSurveyResponse>();
    }
}