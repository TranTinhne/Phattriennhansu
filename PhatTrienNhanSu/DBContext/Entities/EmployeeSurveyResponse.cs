using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("EmployeeSurveyResponses")]
    public class EmployeeSurveyResponse // Bảng này có thể không cần kế thừa nếu bạn muốn nó khác biệt
    {
        [Key]
        public int ResponseID { get; set; }

        public int EmployeeID { get; set; } // Giữ lại EmployeeID riêng
        public int CatalogID { get; set; }
        public int SurveyPeriodId { get; set; }

        public bool LevelUpdateKnowledge { get; set; }
        public bool LevelEnhanceKnowledge { get; set; }
        public bool LevelNecessary { get; set; }
        public bool LevelVeryNecessary { get; set; }
        public bool IsPriority { get; set; }

        public DateTime SubmissionDate { get; set; }

        [ForeignKey("CatalogID")]
        public virtual CourseCatalog CourseCatalog { get; set; } = null!;

        [ForeignKey("SurveyPeriodId")]
        public virtual SurveyPeriod SurveyPeriod { get; set; } = null!;
    }
}