// ---- PHẦN KHAI BÁO USING CẦN THIẾT ----
using Microsoft.EntityFrameworkCore;
using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.Service.Executes.SurveyService;
using PhatTrienNhanSu.Service.Implementations;
using PhatTrienNhanSu.Service.Interfaces;
using PhatTrienNhanSu.Service.Executes.ProviderService;

// ---- BẮT ĐẦU CẤU HÌNH ----
var builder = WebApplication.CreateBuilder(args);


// === Cấu hình HttpClient để gọi API Nhân sự ===

// 1. Đọc chuỗi URL từ cấu hình
var employeeApiUrl = builder.Configuration["ServiceUrls:EmployeeAPI"];

// 2. Kiểm tra xem chuỗi URL có tồn tại và hợp lệ không
if (string.IsNullOrEmpty(employeeApiUrl))
{
    // Ném ra một exception rõ ràng để ứng dụng dừng lại và báo lỗi.
    // Điều này tốt hơn nhiều so với việc để ứng dụng chạy với một cấu hình sai.
    throw new InvalidOperationException("Service URL for 'EmployeeAPI' is not configured in appsettings.json.");
}

// 3. Đăng ký HttpClient với URL đã được xác thực
builder.Services.AddHttpClient("EmployeeAPI", client =>
{
    client.BaseAddress = new Uri(employeeApiUrl);
});


// === PHẦN HIỆN CÓ CỦA BẠN ===
// 1. Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Cấu hình DbContext, chỉ định dùng SQL Server và truyền chuỗi kết nối vào
builder.Services.AddDbContext<PhatTrienNhanSuDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Đăng ký các services của bạn (Dependency Injection)
builder.Services.AddScoped<ISurveyOne, SurveyOne>();
builder.Services.AddScoped<ISurveyMany, SurveyMany>();
builder.Services.AddScoped<ISurveyCommand, SurveyCommand>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

// Đăng ký các services của Provider
builder.Services.AddScoped<IProviderOne, ProviderOne>();
builder.Services.AddScoped<IProviderMany, ProviderMany>();
builder.Services.AddScoped<IProviderCommand, ProviderCommand>();

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
    pattern: "{controller=Survey}/{action=Index}/{id?}");

app.Run();