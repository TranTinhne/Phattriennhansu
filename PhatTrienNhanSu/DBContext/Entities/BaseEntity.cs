using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(1000)] // Giới hạn lại thay vì MAX để có thể tạo index
        public string? Keyword { get; set; }

        public int Status { get; set; }

        // Kiểu uniqueidentifier trong SQL Server tương ứng với Guid trong C#
        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        // UpdatedBy và UpdatedDate PHẢI cho phép NULL
        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}