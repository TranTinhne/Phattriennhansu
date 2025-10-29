# Hệ thống Phát triển Nguồn nhân lực

Đây là một ứng dụng web được xây dựng trên nền tảng **ASP.NET Core MVC** với mục tiêu chính là triển khai chức năng **Khảo sát Nhu cầu Đào tạo** trong doanh nghiệp. Ứng dụng cho phép quản trị viên tạo và quản lý một danh mục khóa học, sau đó triển khai các kỳ khảo sát để nhân viên bày tỏ nguyện vọng đào tạo của mình.



## Mục lục
- [Tính năng chính](#tính-năng-chính)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Kiến trúc Dự án](#kiến-trúc-dự-án)
- [Hướng dẫn Cài đặt & Chạy dự án](#hướng-dẫn-cài-đặt--chạy-dự-án)
  - [Yêu cầu](#yêu-cầu)
  - [Các bước cài đặt](#các-bước-cài-đặt)
- [Cơ sở dữ liệu](#cơ-sở-dữ-liệu)
  - [Mô hình quan hệ](#mô-hình-quan-hệ)
  - [Quản lý bằng Migrations](#quản-lý-bằng-migrations)

---

## Tính năng chính

### Chức năng dành cho Nhân viên
- **Form Khảo sát Năng động**: Xem danh sách các khóa học tiềm năng được nhóm theo từng lĩnh vực cụ thể.
- **Bày tỏ Nhu cầu**: Đánh dấu vào các khóa học mong muốn theo 5 cấp độ:
  1. Cập nhật kiến thức
  2. Nâng cao kiến thức
  3. Cần thiết
  4. Rất cần thiết
  5. Ưu tiên
- **Kiểm soát Phiên làm việc**: Hệ thống tự động kiểm tra kỳ khảo sát đang hoạt động và ngăn người dùng nộp lại nếu đã hoàn thành.

### Chức năng dành cho Quản trị viên
- **CRUD Danh mục Khóa học**: Quản lý toàn bộ danh mục các khóa học có thể được khảo sát (Thêm, Sửa, Xem danh sách, Xóa mềm).
- **CRUD Nhà cung cấp**: Quản lý danh sách các đơn vị đào tạo nội bộ và đối tác bên ngoài.
- **Dropdown Động**: Dữ liệu Nhà cung cấp được sử dụng để tạo danh sách chọn (dropdown) khi thêm/sửa một khóa học, giúp đảm bảo tính nhất quán của dữ liệu.

---

## Công nghệ sử dụng

- **Backend**:
  - .NET 8
  - ASP.NET Core MVC
  - Entity Framework Core 8
- **Frontend**:
  - Razor Pages
  - HTML5 & CSS3
  - Bootstrap 5
- **Database**:
  - Microsoft SQL Server

---

## Kiến trúc Dự án

Dự án được xây dựng theo kiến trúc 3 lớp (3-tier), được phân tách thành 3 project riêng biệt để đảm bảo tính module hóa, dễ bảo trì và mở rộng.

1.  **`PhatTrienNhanSu` (Lớp Presentation - Giao diện)**:
    -   Là một project ASP.NET Core MVC, chịu trách nhiệm xử lý request từ người dùng và trả về giao diện.
    -   Chứa `Controllers` và `Views`.
    -   *Phụ thuộc vào:* `Service`.

2.  **`Service` (Lớp Business Logic - Nghiệp vụ)**:
    -   Là một project Class Library, chứa toàn bộ logic nghiệp vụ của ứng dụng.
    -   Được tổ chức theo chức năng. Mỗi chức năng được chia nhỏ theo các file `One`, `Many`, `Command`, và `Model` để phân tách rõ ràng trách nhiệm.
    -   *Phụ thuộc vào:* `DbContexts`.

3.  **`DbContexts` (Lớp Data Access - Truy cập dữ liệu)**:
    -   Là một project Class Library, chứa các lớp `Entity` (ánh xạ bảng trong CSDL) và lớp `DbContext`.
    -   Chịu trách nhiệm giao tiếp với database thông qua EF Core.
    -   *Không phụ thuộc* vào các project khác.

**Luồng phụ thuộc:** `PhatTrienNhanSu` → `Service` → `DbContexts`

---

## Hướng dẫn Cài đặt & Chạy dự án

### Yêu cầu

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) hoặc cao hơn.
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) với workload "ASP.NET and web development".
- [Microsoft SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (phiên bản Developer hoặc Express là đủ).

### Các bước cài đặt

1.  **Clone Repository:**
    ```bash
    git clone https://github.com/your-username/Phattriennhansu.git
    cd Phattriennhansu
    ```

2.  **Mở dự án:** Mở file `PhatTrienNhanSu.sln` bằng Visual Studio 2022.

3.  **Cấu hình Chuỗi kết nối:**
    -   Mở file `appsettings.json` trong project `PhatTrienNhanSu`.
    -   Tìm đến mục `ConnectionStrings`.
    -   Thay đổi giá trị `Server` thành tên SQL Server trên máy của bạn (ví dụ: `.` hoặc `(localdb)\MSSQLLocalDB`).
      ```json
      "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PhatTrienNhanSuDB;Trusted_Connection=True;TrustServerCertificate=True;"
      }
      ```

4.  **Tạo Cơ sở dữ liệu:**
    -   Mở **Package Manager Console** (View -> Other Windows -> Package Manager Console).
    -   Chạy lệnh sau để tạo database và các bảng từ code:
      ```powershell
      Update-Database -StartupProject PhatTrienNhanSu
      ```

5.  **Chèn dữ liệu mẫu (Tùy chọn):**
    -   Để có dữ liệu ban đầu để kiểm thử, hãy chạy file script SQL `seed-data.sql` (nếu có) trên database `PhatTrienNhanSuDB` bằng SQL Server Management Studio (SSMS).

6.  **Chạy ứng dụng:**
    -   Đảm bảo `PhatTrienNhanSu` được chọn là project khởi động (Startup Project).
    -   Nhấn **F5** hoặc nút Run (▶) để bắt đầu.
    -   Trình duyệt sẽ tự động mở ra và điều hướng đến trang khảo sát.

---

## Cơ sở dữ liệu

### Mô hình quan hệ

-   **`SurveyPeriods`**: Quản lý các kỳ khảo sát (ví dụ: "Khảo sát Q1/2025").
-   **`TrainingProviders`**: Quản lý danh sách các nhà cung cấp/đơn vị đào tạo.
-   **`CourseCatalog`**: Danh mục gốc của tất cả các khóa học. Bảng này có khóa ngoại đến `TrainingProviders`.
-   **`EmployeeSurveyResponses`**: Lưu trữ lựa chọn của từng nhân viên. Mỗi bản ghi liên kết đến một nhân viên, một khóa học trong `CourseCatalog` và một kỳ trong `SurveyPeriods`.

### Quản lý bằng Migrations

Dự án sử dụng **EF Core Code-First Migrations** để quản lý sự thay đổi của CSDL, đảm bảo tất cả các thành viên trong nhóm luôn có cấu trúc CSDL đồng nhất.

-   **Để tạo một migration mới sau khi thay đổi `Entities`:**
    ```powershell
    Add-Migration <TenMigrationMoi> -Project DbContexts -StartupProject PhatTrienNhanSu
    ```
-   **Để áp dụng migration vào CSDL:**
    ```powershell
    Update-Database -StartupProject PhatTrienNhanSu
    ```
