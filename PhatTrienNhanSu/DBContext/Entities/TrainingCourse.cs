using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("TrainingCourses")]
    public class TrainingCourse
    {
        [Key]
        public int CourseID { get; set; }

        public int ProgramID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CourseName { get; set; }

        [MaxLength(100)]
        public string? Instructor { get; set; }

        [Column(TypeName = "decimal(5, 1)")]
        public decimal? Duration { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxAttendees { get; set; }
        public bool RequiresApproval { get; set; }

        // Trạng thái khóa học: -1 = Archived, 0 = Draft, 1 = Scheduled, 2 = InProgress, 3 = Completed
        public sbyte Status { get; set; } // Dùng sbyte để cho phép số âm

        // Các cột kiểm soát
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Mối quan hệ điều hướng
        [ForeignKey("ProgramID")]
        public virtual TrainingProgram TrainingProgram { get; set; }
        public virtual ICollection<EmployeeTrainingRegistration> Registrations { get; set; } = new List<EmployeeTrainingRegistration>();
    }
}