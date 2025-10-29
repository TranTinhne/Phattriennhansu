using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("TrainingPrograms")]
    public class TrainingProgram
    {
        [Key]
        public int ProgramID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProgramCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProgramName { get; set; }

        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Trạng thái chương trình: -1 = Archived, 0 = Draft, 1 = Active
        public sbyte Status { get; set; } // Dùng sbyte để cho phép số âm

        // Các cột kiểm soát
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Mối quan hệ điều hướng
        public virtual ICollection<TrainingCourse> TrainingCourses { get; set; } = new List<TrainingCourse>();
        public virtual ICollection<EmployeeTrainingRegistration> Registrations { get; set; } = new List<EmployeeTrainingRegistration>();
    }
}