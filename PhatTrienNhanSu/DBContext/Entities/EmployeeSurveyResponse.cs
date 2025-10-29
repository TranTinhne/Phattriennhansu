using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("EmployeeSurveyResponses")]
    public class EmployeeSurveyResponse
    {
        [Key]
        public int ResponseID { get; set; }

        public int EmployeeID { get; set; }
        public int CatalogID { get; set; }
        public int SurveyPeriodId { get; set; }

        // Các lựa chọn của nhân viên
        public bool LevelUpdateKnowledge { get; set; }
        public bool LevelEnhanceKnowledge { get; set; }
        public bool LevelNecessary { get; set; }
        public bool LevelVeryNecessary { get; set; }
        public bool IsPriority { get; set; }

        public DateTime SubmissionDate { get; set; }

        // Mối quan hệ điều hướng
        [ForeignKey("CatalogID")]
        public virtual CourseCatalog CourseCatalog { get; set; }

        [ForeignKey("SurveyPeriodId")]
        public virtual SurveyPeriod SurveyPeriod { get; set; }
    }
}