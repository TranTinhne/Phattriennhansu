using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhatTrienNhanSu.DbContexts.Entities
{
    [Table("TrainingProviders")]
    public class TrainingProvider : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string ProviderCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ProviderName { get; set; } = string.Empty;

        public bool IsInternal { get; set; }

        public virtual ICollection<CourseCatalog> CourseCatalogs { get; set; } = new List<CourseCatalog>();
    }
}