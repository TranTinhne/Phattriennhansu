// ---- PHẦN KHAI BÁO USING CẦN THIẾT ----
using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.Service.TrainingService;

// ---- BẮT ĐẦU CẤU HÌNH ----
var builder = WebApplication.CreateBuilder(args);


// --- BỔ SUNG QUAN TRỌNG: Cấu hình HttpClient để gọi API Nhân sự ---
// Đăng ký HttpClient để có thể inject vào các service sau này.
// Việc này rất cần thiết cho các service của bạn để lấy thông tin nhân viên.
builder.Services.AddHttpClient("EmployeeAPI", client =>
{
    // Lấy địa chỉ của API Nhân sự từ file cấu hình
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:EmployeeAPI"]);
});


// === PHẦN HIỆN CÓ CỦA BẠN (ĐÃ TỐT) ===
// 1. Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Cấu hình DbContext, chỉ định dùng SQL Server và truyền chuỗi kết nối vào
builder.Services.AddDbContext<PhatTrienNhanSuDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Đăng ký các services của bạn (Dependency Injection)
builder.Services.AddScoped<ITrainingOne, TrainingOne>();
builder.Services.AddScoped<ITrainingMany, TrainingMany>();
builder.Services.AddScoped<ITrainingCommand, TrainingCommand>();
// (Trong tương lai, bạn sẽ đăng ký các service khác ở đây, ví dụ IEmployeeService)


// 4. Đăng ký các dịch vụ MVC
builder.Services.AddControllersWithViews();


// ---- BẮT ĐẦU CẤU HÌNH PIPELINE ----
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// TẠM THỜI CHƯA CÓ XÁC THỰC, NHƯNG VẪN CẦN PHÂN QUYỀN (AUTHORIZATION)
// Middleware này vẫn cần thiết để các attribute như [AllowAnonymous] hoạt động.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();