using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("CourseCatalog")]
    public class CourseCatalog : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string CourseName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Area { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(4, 1)")]
        public decimal? DurationHours { get; set; }

        [MaxLength(255)]
        public int? ProviderID { get; set; } // Khóa ngoại

        [ForeignKey("ProviderID")]
        public virtual TrainingProvider? Provider { get; set; } // Thuộc tính điều hướng

        public CourseCatalog()
        {
            Status = 1;
        }

        public virtual ICollection<EmployeeSurveyResponse> SurveyResponses { get; set; } = new List<EmployeeSurveyResponse>();
    }
}