namespace PhatTrienNhanSu.Service.Interfaces
{
    public interface IUserAccessor
    {
        // Guid GetCurrentUserId(); // Dùng Guid nếu user id là uniqueidentifier
        int GetCurrentUserId(); // Dùng int nếu user id là int
    }
}