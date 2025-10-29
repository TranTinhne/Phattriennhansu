using System.ComponentModel.DataAnnotations;

namespace PhatTrienNhanSu.Service.Executes.SurveyService.Models
{
   /// ViewModel chính để hiển thị và submit form khảo sát
    public class SurveyFormViewModel
    {
        [Required]
        public int SurveyPeriodId { get; set; }
        public string SurveyPeriodName { get; set; } = string.Empty;

        // Dùng để hiển thị cấu trúc nhóm trên View
        public List<SurveyAreaGroup> AreaGroups { get; set; } = new();

        // Dùng để binding dữ liệu khi người dùng submit form
        public List<SurveySelection> Selections { get; set; } = new();
    }

   /// Đại diện cho một nhóm lĩnh vực (vd: "Kiến thức về công ty")
    public class SurveyAreaGroup
    {
        public string AreaName { get; set; } = string.Empty;
        public List<CatalogItem> CatalogItems { get; set; } = new();
    }

    /// DTO chứa thông tin tóm tắt của một mục trong CourseCatalog
    public class CatalogItem
    {
        public int Id { get; set; }
        public int CatalogID { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal? DurationHours { get; set; }
        public string? Provider { get; set; }
    }

    /// Đại diện cho một dòng lựa chọn của người dùng trên form
    public class SurveySelection
    {
        public int CatalogID { get; set; }

        // Các checkbox tương ứng 5 cột trong hình
        public bool LevelUpdateKnowledge { get; set; }
        public bool LevelEnhanceKnowledge { get; set; }
        public bool LevelNecessary { get; set; }
        public bool LevelVeryNecessary { get; set; }
        public bool IsPriority { get; set; }

        public bool IsSelected => LevelUpdateKnowledge || LevelEnhanceKnowledge || LevelNecessary || LevelVeryNecessary || IsPriority;
    }
}