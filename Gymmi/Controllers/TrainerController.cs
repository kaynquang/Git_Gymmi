using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gymmi.Data;
using Gymmi.Models;

namespace Gymmi.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TrainerController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Helper method to get current user ID from session
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // Helper method to check if user is authenticated and is a trainer
        private bool IsAuthenticatedTrainer()
        {
            var userId = GetCurrentUserId();
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return userId.HasValue && roleId == 4; // Role ID 4 = Huấn luyện viên
        }

        // Helper method to redirect to login if not authenticated
        private IActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (!IsAuthenticatedTrainer())
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản huấn luyện viên để truy cập trang này.";
                return RedirectToAction("Login", "Home");
            }
            return null;
        }

        // Main Dashboard for Trainers
        public async Task<IActionResult> Dashboard()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                // Get trainer info
                var trainer = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.ID_User == userId);

                if (trainer == null)
                {
                    ViewBag.Error = "Không tìm thấy thông tin huấn luyện viên";
                    return View();
                }

                // Get trainer's work assignments
                var assignments = await _context.PhanCongs
                    .Include(p => p.PhongTap)
                    .Include(p => p.CaLamViec)
                    .Include(p => p.CreatedByAdmin)
                    .Where(p => p.ID_User == userId)
                    .OrderBy(p => p.NgayPhanCong)
                    .ToListAsync();

                // Get today's assignments
                var todayAssignments = assignments
                    .Where(a => a.NgayPhanCong.Date == DateTime.Today)
                    .ToList();

                // Get upcoming assignments (next 7 days)
                var upcomingAssignments = assignments
                    .Where(a => a.NgayPhanCong.Date > DateTime.Today && a.NgayPhanCong.Date <= DateTime.Today.AddDays(7))
                    .ToList();

                // Get statistics
                var totalAssignments = assignments.Count;
                var activeAssignments = assignments.Count(a => a.TrangThai == "Đang hoạt động");
                var thisWeekAssignments = assignments
                    .Count(a => a.NgayPhanCong >= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek) &&
                               a.NgayPhanCong < DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek));

                ViewBag.Trainer = trainer;
                ViewBag.TotalAssignments = totalAssignments;
                ViewBag.ActiveAssignments = activeAssignments;
                ViewBag.ThisWeekAssignments = thisWeekAssignments;
                ViewBag.TodayAssignments = todayAssignments;
                ViewBag.UpcomingAssignments = upcomingAssignments;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        // Work Schedule for Trainers
        public async Task<IActionResult> Schedule()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                // Get trainer's work schedule
                var assignments = await _context.PhanCongs
                    .Include(p => p.PhongTap)
                    .Include(p => p.CaLamViec)
                    .Include(p => p.CreatedByAdmin)
                    .Where(p => p.ID_User == userId)
                    .OrderBy(p => p.NgayPhanCong)
                    .ThenBy(p => p.CaLamViec.TenCa)
                    .ToListAsync();

                return View(assignments);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<PhanCong>());
            }
        }

        // Profile Management for Trainers
        public async Task<IActionResult> Profile()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var trainer = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.ID_User == userId);

                if (trainer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin huấn luyện viên";
                    return RedirectToAction("Dashboard");
                }

                return View(trainer);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User trainer)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    var existingTrainer = await _context.Users.FindAsync(trainer.ID_User);
                    if (existingTrainer != null)
                    {
                        existingTrainer.HoTen = trainer.HoTen;
                        existingTrainer.Sdt = trainer.Sdt;
                        existingTrainer.Email = trainer.Email;
                        existingTrainer.UpdatedAt = DateTime.Now;

                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Cập nhật thông tin thành công!";
                        return RedirectToAction("Profile");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
            }

            return View(trainer);
        }

        // View assigned members/clients
        public async Task<IActionResult> Clients()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                // Get all members for now
                var members = await _context.HoiViens
                    .Include(h => h.User)
                    .Include(h => h.TheHoiViens)
                        .ThenInclude(t => t.GoiTap)
                    .ToListAsync();

                return View(members);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<HoiVien>());
            }
        }
    }
}
