using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gymmi.Data;
using Gymmi.Models;

namespace Gymmi.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDBContext _context;

        public MemberController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Helper method to get current user ID from session
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // Helper method to check if user is authenticated and is a member
        private bool IsAuthenticatedMember()
        {
            var userId = GetCurrentUserId();
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return userId.HasValue && roleId == 3; // Role ID 3 = Hội viên
        }

        // Helper method to redirect to login if not authenticated
        private IActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (!IsAuthenticatedMember())
            {
                TempData["Error"] = "Vui lòng đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Home");
            }
            return null;
        }

        // Main Dashboard for Members
        public async Task<IActionResult> Dashboard()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                // Get member info with their active membership
                var member = await _context.HoiViens
                    .Include(h => h.User)
                    .Include(h => h.TheHoiViens)
                        .ThenInclude(t => t.GoiTap)
                    .FirstOrDefaultAsync(h => h.ID_User == userId);

                if (member == null)
                {
                    ViewBag.Error = "Không tìm thấy thông tin hội viên";
                    return View();
                }

                // Get active membership card
                var activeMembership = member.TheHoiViens
                    .Where(t => t.TrangThai == "Đang hoạt động" && t.NgayHetHan > DateTime.Now)
                    .OrderByDescending(t => t.NgayBatDau)
                    .FirstOrDefault();

                // Get recent workout sessions
                var recentWorkouts = await _context.LichTaps
                    .Include(l => l.PhongTap)
                    .Where(l => l.ID_User == userId)
                    .OrderByDescending(l => l.NgayTap)
                    .Take(5)
                    .ToListAsync();

                // Get payment history
                var recentPayments = await _context.HoaDon_ThanhToans
                    .Include(h => h.GoiTap)
                    .Where(h => h.ID_User == userId)
                    .OrderByDescending(h => h.NgayThanhToan)
                    .Take(5)
                    .ToListAsync();

                ViewBag.Member = member;
                ViewBag.ActiveMembership = activeMembership;
                ViewBag.RecentWorkouts = recentWorkouts;
                ViewBag.RecentPayments = recentPayments;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        // Member Profile Management
        public async Task<IActionResult> Profile()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var member = await _context.HoiViens
                    .Include(h => h.User)
                    .FirstOrDefaultAsync(h => h.ID_User == userId);

                if (member == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin hội viên";
                    return RedirectToAction("Dashboard");
                }

                return View(member);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(HoiVien member)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    var existingMember = await _context.HoiViens.FindAsync(member.ID_HoiVien);
                    if (existingMember != null)
                    {
                        existingMember.HoTen = member.HoTen;
                        existingMember.NgaySinh = member.NgaySinh;
                        existingMember.Sdt = member.Sdt;
                        existingMember.Email = member.Email;
                        existingMember.DiaChi = member.DiaChi;
                        existingMember.GioiTinh = member.GioiTinh;

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

            return View(member);
        }

        // Membership Information
        public async Task<IActionResult> Membership()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var member = await _context.HoiViens
                    .Include(h => h.TheHoiViens)
                        .ThenInclude(t => t.GoiTap)
                    .FirstOrDefaultAsync(h => h.ID_User == userId);

                if (member == null)
                {
                    ViewBag.Error = "Không tìm thấy thông tin hội viên";
                    return View(new List<TheHoiVien>());
                }

                return View(member.TheHoiViens.OrderByDescending(t => t.NgayBatDau).ToList());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<TheHoiVien>());
            }
        }

        // Available Packages for Purchase
        public async Task<IActionResult> Packages()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var packages = await _context.GoiTaps
                    .Where(g => g.NgayKetThuc > DateTime.Now) // Only active packages
                    .OrderBy(g => g.GiaTien)
                    .ToListAsync();

                return View(packages);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<GoiTap>());
            }
        }

        // Workout Schedule Management
        public async Task<IActionResult> Schedule()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var workouts = await _context.LichTaps
                    .Include(l => l.PhongTap)
                    .Include(l => l.TheHoiVien)
                        .ThenInclude(t => t.GoiTap)
                    .Where(l => l.ID_User == userId)
                    .OrderBy(l => l.NgayTap)
                    .ToListAsync();

                ViewBag.UserId = userId;
                return View(workouts);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<LichTap>());
            }
        }

        // Book New Workout Session
        public async Task<IActionResult> BookWorkout()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                // Get member's active membership
                var activeMembership = await _context.TheHoiViens
                    .Include(t => t.GoiTap)
                    .Where(t => t.ID_User == userId && t.TrangThai == "Đang hoạt động" && t.NgayHetHan > DateTime.Now)
                    .FirstOrDefaultAsync();

                if (activeMembership == null)
                {
                    TempData["Error"] = "Bạn không có gói tập hoạt động. Vui lòng mua gói tập trước khi đặt lịch.";
                    return RedirectToAction("Packages");
                }

                // Get available rooms
                var rooms = await _context.PhongTaps
                    .Where(p => p.TinhTrang == "Hoạt động")
                    .ToListAsync();

                ViewBag.ActiveMembership = activeMembership;
                ViewBag.Rooms = rooms;
                ViewBag.UserId = userId;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookWorkout(int roomId, DateTime workoutDate, TimeSpan workoutTime, string description)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                // Validate booking time (must be at least 2 hours in advance)
                var bookingDateTime = workoutDate.Date + workoutTime;
                if (bookingDateTime <= DateTime.Now.AddHours(2))
                {
                    TempData["Error"] = "Bạn phải đặt lịch trước ít nhất 2 giờ.";
                    return RedirectToAction("BookWorkout");
                }

                // Check if member has active membership
                var activeMembership = await _context.TheHoiViens
                    .Where(t => t.ID_User == userId && t.TrangThai == "Đang hoạt động" && t.NgayHetHan > DateTime.Now)
                    .FirstOrDefaultAsync();

                if (activeMembership == null)
                {
                    TempData["Error"] = "Bạn không có gói tập hoạt động.";
                    return RedirectToAction("Packages");
                }

                // Get room and check slot availability
                var room = await _context.PhongTaps
                    .Where(p => p.ID_PhongTap == roomId)
                    .FirstOrDefaultAsync();

                if (room == null)
                {
                    TempData["Error"] = "Phòng tập không tồn tại.";
                    return RedirectToAction("BookWorkout");
                }

                // Check current bookings for this specific time slot
                var currentBookings = await _context.LichTaps
                    .Where(l => l.ID_PhongTap == roomId && 
                               l.NgayTap.Date == workoutDate.Date && 
                               l.ThoiGianTap == workoutTime)
                    .CountAsync();

                if (currentBookings >= room.SlotToiDa)
                {
                    TempData["Error"] = "Phòng tập đã hết slot cho khung giờ này. Vui lòng chọn thời gian khác.";
                    return RedirectToAction("BookWorkout");
                }

                // Check if user already has booking for this time slot
                var userExistingBooking = await _context.LichTaps
                    .Where(l => l.ID_User == userId && 
                               l.NgayTap.Date == workoutDate.Date && 
                               l.ThoiGianTap == workoutTime)
                    .FirstOrDefaultAsync();

                if (userExistingBooking != null)
                {
                    TempData["Error"] = "Bạn đã có lịch tập trong khung giờ này.";
                    return RedirectToAction("BookWorkout");
                }

                // Create new workout booking
                var newWorkout = new LichTap
                {
                    ID_User = userId,
                    ID_PhongTap = roomId,
                    ID_TheHoiVien = activeMembership.ID_TheHoiVien,
                    NgayTap = workoutDate,
                    ThoiGianTap = workoutTime,
                    MoTa = description ?? ""
                };

                _context.LichTaps.Add(newWorkout);

                // Update room's registered slots count
                room.SlotDaDangKy = currentBookings + 1;
                _context.PhongTaps.Update(room);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Đặt lịch tập thành công!";
                return RedirectToAction("Schedule");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("BookWorkout");
            }
        }

        // View Gym Facilities
        public async Task<IActionResult> Facilities()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var rooms = await _context.PhongTaps
                    .Include(p => p.ThietBis)
                    .Where(p => p.TinhTrang == "Hoạt động")
                    .OrderBy(p => p.ID_PhongTap)
                    .ToListAsync();

                return View(rooms);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<PhongTap>());
            }
        }

        // Payment History
        public async Task<IActionResult> PaymentHistory()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var payments = await _context.HoaDon_ThanhToans
                    .Include(h => h.GoiTap)
                    .Include(h => h.TheHoiVien)
                    .Where(h => h.ID_User == userId)
                    .OrderByDescending(h => h.NgayThanhToan)
                    .ToListAsync();

                return View(payments);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<HoaDon_ThanhToan>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelWorkout(int workoutId)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            var userId = GetCurrentUserId().Value;

            try
            {
                var workout = await _context.LichTaps
                    .Include(l => l.PhongTap)
                    .Where(l => l.ID_LichTap == workoutId && l.ID_User == userId)
                    .FirstOrDefaultAsync();

                if (workout == null)
                {
                    TempData["Error"] = "Không tìm thấy lịch tập này.";
                    return RedirectToAction("Schedule");
                }

                // Check if cancellation is at least 2 hours before workout
                var workoutDateTime = workout.NgayTap.Date + workout.ThoiGianTap;
                if (workoutDateTime <= DateTime.Now.AddHours(2))
                {
                    TempData["Error"] = "Bạn chỉ có thể hủy lịch trước ít nhất 2 giờ.";
                    return RedirectToAction("Schedule");
                }

                // Get current bookings for this time slot to update slot count
                var currentBookings = await _context.LichTaps
                    .Where(l => l.ID_PhongTap == workout.ID_PhongTap && 
                               l.NgayTap.Date == workout.NgayTap.Date && 
                               l.ThoiGianTap == workout.ThoiGianTap)
                    .CountAsync();

                // Update room's registered slots count
                if (workout.PhongTap != null && currentBookings > 0)
                {
                    workout.PhongTap.SlotDaDangKy = currentBookings - 1;
                    _context.PhongTaps.Update(workout.PhongTap);
                }

                _context.LichTaps.Remove(workout);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hủy lịch tập thành công!";
                return RedirectToAction("Schedule");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Schedule");
            }
        }
    }
} 