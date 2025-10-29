using System.ComponentModel.DataAnnotations;

namespace PhatTrienNhanSu.Service.TrainingService.Models
{
    // Dùng cho danh sách chương trình
    public class ProgramSummary
    {
        public int ProgramID { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public DateTime? StartDate { get; set; }
    }

    // Dùng cho trang chi tiết chương trình
    public class ProgramDetail
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string? Description { get; set; }
        public List<CourseDetail> AvailableCourses { get; set; } = new();
    }

    // Dùng bên trong ProgramDetail
    public class CourseDetail
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string? Instructor { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
    }

    // Dùng để nhận request từ form đăng ký trên View
    public class RegistrationRequest
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int ProgramId { get; set; }
    }

    // Dùng cho trang lịch sử đăng ký của nhân viên
    public class RegistrationDetail
    {
        public string ProgramName { get; set; }
        public string CourseName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Status { get; set; }
        public float? Score { get; set; }
        public bool Certificate { get; set; }
    }
}