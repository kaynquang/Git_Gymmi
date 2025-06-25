using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gymmi.Models;
using Gymmi.Data;

namespace Gymmi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Find user by username and password
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.HoiViens)
                .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            // Store user info in session
            HttpContext.Session.SetInt32("UserId", user.ID_User);
            HttpContext.Session.SetString("UserName", user.HoTen);
            HttpContext.Session.SetInt32("RoleId", user.ID_Role);
            HttpContext.Session.SetString("RoleName", user.Role.TenRole);

            // Redirect based on role
            if (user.ID_Role == 1) // Admin
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.ID_Role == 3) // Hội viên (Member)
            {
                return RedirectToAction("Dashboard", "Member");
            }
            else if (user.ID_Role == 2) // Nhân viên
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.ID_Role == 4) // Huấn luyện viên
            {
                return RedirectToAction("Dashboard", "Trainer");
            }
            else
            {
                ModelState.AddModelError("", "Loại tài khoản không hợp lệ.");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.");
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
