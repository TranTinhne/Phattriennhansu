using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("SurveyPeriods")]
    public class SurveyPeriod
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên kỳ khảo sát là bắt buộc.")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Trạng thái kỳ khảo sát: 0 = Draft, 1 = Open, 2 = Closed
        public byte Status { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Mối quan hệ điều hướng: Một kỳ khảo sát có nhiều phản hồi
        public virtual ICollection<EmployeeSurveyResponse> SurveyResponses { get; set; } = new List<EmployeeSurveyResponse>();
    }
}