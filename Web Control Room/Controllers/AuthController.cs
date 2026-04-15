using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using WebControlRoom.Models;

public class AuthController : Controller
{
    private readonly DispatcherContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(DispatcherContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendCode(string email)
    {
        if (!email.EndsWith("@gsu.by", System.StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Error = "Можно использовать только почту @gsu.by";
            return View("Login");
        }

        var code = new Random().Next(100000, 999999).ToString();

        _context.EmailConfirmCodes.Add(new EmailConfirmCode
        {
            Email = email,
            Code = code,
            ExpireAt = DateTime.Now.AddMinutes(5)
        });
        await _context.SaveChangesAsync();

        var host = _configuration["EmailSettings:Host"];
        var port = int.Parse(_configuration["EmailSettings:Port"]);
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];
        var fromEmail = _configuration["EmailSettings:FromEmail"];

        using var smtp = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var message = new MailMessage(fromEmail, email,
            "Код для входа", $"Ваш код для входа: {code}");

        await smtp.SendMailAsync(message);

        ViewBag.Email = email;
        return View("ConfirmCode"); 
    }

    [HttpPost]
    public IActionResult ConfirmCode(string email, string code)
    {
        var record = _context.EmailConfirmCodes
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x => x.Email == email && x.Code == code && x.ExpireAt > DateTime.Now);

        if (record == null)
        {
            ViewBag.Error = "Неверный или просроченный код";
            ViewBag.Email = email;
            return View("ConfirmCode");
        }

        HttpContext.Session.SetString("userEmail", email);

        return RedirectToAction("Index", "Home");
    }
}