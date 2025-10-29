using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("CourseCatalog")]
    public class CourseCatalog
    {
        [Key]
        public int CatalogID { get; set; }

        [Required(ErrorMessage = "Mã khóa học là bắt buộc.")]
        [MaxLength(50)]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Tên khóa học là bắt buộc.")]
        [MaxLength(255)]
        public string CourseName { get; set; }

        [MaxLength(255)]
        public string? Area { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(4, 1)")]
        public decimal? DurationHours { get; set; }

        [MaxLength(255)]
        public string? Provider { get; set; }

        // Trạng thái của mục trong danh mục: 1 = Active, 0 = Inactive
        public byte Status { get; set; }

        // Mối quan hệ điều hướng: Một mục trong catalog có thể được chọn trong nhiều phiếu khảo sát
        public virtual ICollection<EmployeeSurveyResponse> SurveyResponses { get; set; } = new List<EmployeeSurveyResponse>();
    }
}