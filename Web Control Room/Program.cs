using Microsoft.EntityFrameworkCore;
using WebControlRoom.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем строку подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Добавляем DbContext
builder.Services.AddDbContext<DispatcherContext>(options =>
    options.UseSqlServer(connectionString));

// Добавляем MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка пайплайна
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Роутинг по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
