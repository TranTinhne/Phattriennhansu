namespace PhatTrienNhanSu.Service.Results
{
    /// <summary>
    /// Lớp cơ sở để trả về kết quả từ các service, cho biết thao tác thành công hay thất bại.
    /// </summary>
    public class ServiceResult
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        /// <summary>
        /// Tạo một kết quả thành công.
        /// </summary>
        public static ServiceResult Success() => new ServiceResult { IsSuccess = true };

        /// <summary>
        /// Tạo một kết quả thất bại với thông báo lỗi.
        /// </summary>
        public static ServiceResult Fail(string message) => new ServiceResult { IsSuccess = false, ErrorMessage = message };
    }

    /// <summary>
    /// Lớp kế thừa từ ServiceResult, cho phép trả về dữ liệu (Data) khi thành công.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của Data trả về.</typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; private set; }

        /// <summary>
        /// Tạo một kết quả thành công kèm theo dữ liệu.
        /// </summary>
        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { IsSuccess = true, Data = data };

        /// <summary>
        /// Tạo một kết quả thất bại với thông báo lỗi (Ghi đè phương thức Fail).
        /// </summary>
        public new static ServiceResult<T> Fail(string message) => new ServiceResult<T> { IsSuccess = false, ErrorMessage = message };
    }
}