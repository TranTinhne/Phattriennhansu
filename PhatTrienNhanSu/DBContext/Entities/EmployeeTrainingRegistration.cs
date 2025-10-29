using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("EmployeeTrainingRegistrations")]
    public class EmployeeTrainingRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        public int EmployeeID { get; set; }
        public int CourseID { get; set; }
        public int ProgramID { get; set; }
        public DateTime RegisterDate { get; set; }

        // Thông tin quy trình phê duyệt
        public int? ApproverId { get; set; }
        public DateTime? ApprovalDate { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        // Trạng thái đăng ký: -1 = Cancelled, 0 = Pending, 1 = Approved, 2 = Rejected, 3 = Completed
        public sbyte Status { get; set; } // Dùng sbyte để cho phép số âm

        // Kết quả sau khóa học
        public float? Score { get; set; }
        public bool Certificate { get; set; }

        // Các cột kiểm soát
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Mối quan hệ điều hướng
        [ForeignKey("CourseID")]
        public virtual TrainingCourse TrainingCourse { get; set; }

        [ForeignKey("ProgramID")]
        public virtual TrainingProgram TrainingProgram { get; set; }
    }
}