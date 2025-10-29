using Microsoft.EntityFrameworkCore;

namespace PhatTrienNhanSu.Service.Executes.ProviderService.Models
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public string ProviderCode { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public int Status { get; set; }
    }
}