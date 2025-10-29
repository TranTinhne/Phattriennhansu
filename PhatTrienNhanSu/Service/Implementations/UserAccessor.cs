using System.Security.Claims;
using PhatTrienNhanSu.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PhatTrienNhanSu.Service.Implementations
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            // Trả về một int rỗng hoặc ném lỗi nếu không có user (tùy logic của bạn)
            return -1;
        }
    }
}