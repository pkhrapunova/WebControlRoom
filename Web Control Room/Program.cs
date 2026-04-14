using Microsoft.EntityFrameworkCore;
using WebControlRoom.Models;

var builder = WebApplication.CreateBuilder(args);

// Строка подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext
builder.Services.AddDbContext<DispatcherContext>(options =>
    options.UseSqlServer(connectionString));

// MVC
builder.Services.AddControllersWithViews();

// Сессии (для хранения залогиненного пользователя)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Пайплайн
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ВАЖНО: сначала сессии, потом авторизация
app.UseSession();
app.UseAuthorization();

// Роутинг по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();