using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gymmi.Data;
using Gymmi.Models;

namespace Gymmi.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AdminController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Helper method to get current user ID from session
        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // Helper method to check if user is authenticated and is an admin
        private bool IsAuthenticatedAdmin()
        {
            var userId = GetCurrentUserId();
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return userId.HasValue && (roleId == 1 || roleId == 2 || roleId == 4); // Admin, Staff, or Trainer
        }

        // Helper method to redirect to login if not authenticated
        private IActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (!IsAuthenticatedAdmin())
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản quản trị để truy cập trang này.";
                return RedirectToAction("Login", "Home");
            }
            return null;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var totalMembers = await _context.HoiViens.CountAsync();
                var totalUsers = await _context.Users.CountAsync();
                var totalRooms = await _context.PhongTaps.CountAsync();
                var totalEquipment = await _context.ThietBis.CountAsync();
                
                // Handle potential null values
                var totalRevenue = _context.HoaDon_ThanhToans.Any() 
                    ? await _context.HoaDon_ThanhToans.SumAsync(h => h.SoTien)
                    : 0;

                ViewBag.TotalMembers = totalMembers;
                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalRooms = totalRooms;
                ViewBag.TotalEquipment = totalEquipment;
                ViewBag.TotalRevenue = totalRevenue;

                return View();
            }
            catch (Exception ex)
            {
                // Log the error and return a safe view
                ViewBag.Error = "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message;
                ViewBag.TotalMembers = 0;
                ViewBag.TotalUsers = 0;
                ViewBag.TotalRooms = 0;
                ViewBag.TotalEquipment = 0;
                ViewBag.TotalRevenue = 0;
                return View();
            }
        }

        // User Management
        public async Task<IActionResult> Users()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .ToListAsync();
                return View(users ?? new List<User>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách người dùng: " + ex.Message;
                return View(new List<User>());
            }
        }

        public async Task<IActionResult> CreateUser()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Roles = new List<Role>();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo người dùng thành công!";
                    return RedirectToAction(nameof(Users));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo người dùng: " + ex.Message);
            }
            
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) 
                {
                    TempData["Error"] = "Không tìm thấy người dùng";
                    return RedirectToAction(nameof(Users));
                }
                
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Users));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    user.UpdatedAt = DateTime.Now;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật người dùng thành công!";
                    return RedirectToAction(nameof(Users));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật: " + ex.Message);
            }
            
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var user = await _context.Users
                    .Include(u => u.HoiViens)
                    .Include(u => u.PhanCongs)
                    .Include(u => u.TheHoiViens)
                    .FirstOrDefaultAsync(u => u.ID_User == id);

                if (user != null)
                {
                    // Check if user has active relationships
                    if (user.HoiViens.Any())
                    {
                        TempData["Error"] = "Không thể xóa user này vì có liên kết với hội viên.";
                        return RedirectToAction(nameof(Users));
                    }

                    if (user.PhanCongs.Any())
                    {
                        TempData["Error"] = "Không thể xóa user này vì có phân công công việc.";
                        return RedirectToAction(nameof(Users));
                    }

                    if (user.TheHoiViens.Any())
                    {
                        TempData["Error"] = "Không thể xóa user này vì có thẻ hội viên liên quan.";
                        return RedirectToAction(nameof(Users));
                    }

                    // Don't allow deleting the last admin
                    if (user.Role.TenRole == "Admin")
                    {
                        var adminCount = await _context.Users.CountAsync(u => u.Role.TenRole == "Admin");
                        if (adminCount <= 1)
                        {
                            TempData["Error"] = "Không thể xóa admin cuối cùng trong hệ thống.";
                            return RedirectToAction(nameof(Users));
                        }
                    }

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa user thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy user để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa user: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Users));
        }

        // Member Management
        public async Task<IActionResult> Members()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var members = await _context.HoiViens
                    .Include(h => h.User)
                    .ToListAsync();
                return View(members ?? new List<HoiVien>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách hội viên: " + ex.Message;
                return View(new List<HoiVien>());
            }
        }

        public async Task<IActionResult> CreateMember()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Hội viên").ToListAsync();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Users = new List<User>();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(HoiVien hoiVien)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    hoiVien.NgayThamGia = DateTime.Now;
                    _context.HoiViens.Add(hoiVien);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo hội viên thành công!";
                    return RedirectToAction(nameof(Members));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo hội viên: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Hội viên").ToListAsync();
            return View(hoiVien);
        }

        public async Task<IActionResult> EditMember(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var member = await _context.HoiViens.FindAsync(id);
                if (member == null)
                {
                    TempData["Error"] = "Không tìm thấy hội viên để chỉnh sửa.";
                    return RedirectToAction(nameof(Members));
                }

                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                return View(member);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Members));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(HoiVien hoiVien)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.HoiViens.Update(hoiVien);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật hội viên thành công!";
                    return RedirectToAction(nameof(Members));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật hội viên: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            return View(hoiVien);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var member = await _context.HoiViens
                    .Include(h => h.TheHoiViens)
                    .FirstOrDefaultAsync(h => h.ID_HoiVien == id);

                if (member != null)
                {
                    // Check if member has active bookings through User
                    var activeBookings = await _context.LichTaps.Where(l => l.ID_User == member.ID_User && l.NgayTap >= DateTime.Now).CountAsync();
                    if (activeBookings > 0)
                    {
                        TempData["Error"] = $"Không thể xóa hội viên này vì có {activeBookings} lịch tập đang hoạt động. Vui lòng hủy các lịch tập trước khi xóa hội viên.";
                        return RedirectToAction(nameof(Members));
                    }

                    // Check if member has active membership cards
                    var activeMemberships = member.TheHoiViens.Where(t => t.NgayHetHan >= DateTime.Now).Count();
                    if (activeMemberships > 0)
                    {
                        TempData["Error"] = $"Không thể xóa hội viên này vì có {activeMemberships} thẻ hội viên đang hoạt động.";
                        return RedirectToAction(nameof(Members));
                    }

                    _context.HoiViens.Remove(member);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa hội viên thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy hội viên để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa hội viên: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Members));
        }

        // Room Management
        public async Task<IActionResult> Rooms()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var rooms = await _context.PhongTaps
                    .Include(p => p.User)
                    .Include(p => p.TrainerPhuTrach)
                    .Include(p => p.ThietBis)
                    .ToListAsync();
                return View(rooms ?? new List<PhongTap>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách phòng tập: " + ex.Message;
                return View(new List<PhongTap>());
            }
        }

        public async Task<IActionResult> CreateRoom()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                ViewBag.Trainers = await _context.Users.Where(u => u.Role.TenRole == "Huấn luyện viên").ToListAsync();
                
                // Load available equipment (not assigned to any room or in good condition)
                ViewBag.AvailableEquipment = await _context.ThietBis
                    .Where(t => t.ID_Phong == 0 || t.ID_Phong == null || t.TinhTrang == "Khả dụng" || t.TinhTrang == "Hoạt động")
                    .ToListAsync();
                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Users = new List<User>();
                ViewBag.Trainers = new List<User>();
                ViewBag.AvailableEquipment = new List<ThietBi>();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(PhongTap phongTap, List<int> selectedEquipment)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                // Validate that at least one equipment is selected
                if (selectedEquipment == null || !selectedEquipment.Any())
                {
                    ModelState.AddModelError("", "Bạn phải chọn ít nhất một thiết bị cho phòng tập.");
                }

                if (ModelState.IsValid)
                {
                    // Set default slot values if not provided
                    if (phongTap.SlotToiDa <= 0)
                    {
                        phongTap.SlotToiDa = 20; // Default max slots
                    }
                    phongTap.SlotDaDangKy = 0; // Start with no bookings
                    phongTap.TinhTrang = "Hoạt động"; // Set default status

                    _context.PhongTaps.Add(phongTap);
                    await _context.SaveChangesAsync();

                    // Assign selected equipment to the room
                    if (selectedEquipment != null && selectedEquipment.Any())
                    {
                        var equipmentToUpdate = await _context.ThietBis
                            .Where(t => selectedEquipment.Contains(t.ID_ThietBi))
                            .ToListAsync();

                        foreach (var equipment in equipmentToUpdate)
                        {
                            equipment.ID_Phong = phongTap.ID_PhongTap;
                            equipment.TinhTrang = "Đang sử dụng";
                        }

                        await _context.SaveChangesAsync();
                    }

                    TempData["Success"] = "Tạo phòng tập thành công!";
                    return RedirectToAction(nameof(Rooms));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phòng tập: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            ViewBag.Trainers = await _context.Users.Where(u => u.Role.TenRole == "Huấn luyện viên").ToListAsync();
            ViewBag.AvailableEquipment = await _context.ThietBis
                .Where(t => t.ID_Phong == 0 || t.ID_Phong == null || t.TinhTrang == "Khả dụng" || t.TinhTrang == "Hoạt động")
                .ToListAsync();
            
            return View(phongTap);
        }

        public async Task<IActionResult> EditRoom(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var room = await _context.PhongTaps
                    .Include(p => p.ThietBis)
                    .FirstOrDefaultAsync(p => p.ID_PhongTap == id);
                    
                if (room == null)
                {
                    TempData["Error"] = "Không tìm thấy phòng tập để chỉnh sửa.";
                    return RedirectToAction(nameof(Rooms));
                }

                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                ViewBag.Trainers = await _context.Users.Where(u => u.Role.TenRole == "Huấn luyện viên").ToListAsync();
                ViewBag.AvailableEquipment = await _context.ThietBis
                    .Where(t => t.ID_Phong == 0 || t.ID_Phong == null || t.ID_Phong == id)
                    .ToListAsync();
                ViewBag.SelectedEquipment = room.ThietBis.Select(t => t.ID_ThietBi).ToList();
                
                return View(room);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Rooms));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRoom(PhongTap phongTap, List<int> selectedEquipment)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                // Validate that at least one equipment is selected
                if (selectedEquipment == null || !selectedEquipment.Any())
                {
                    ModelState.AddModelError("", "Bạn phải chọn ít nhất một thiết bị cho phòng tập.");
                }

                if (ModelState.IsValid)
                {
                    var existingRoom = await _context.PhongTaps
                        .Include(p => p.ThietBis)
                        .FirstOrDefaultAsync(p => p.ID_PhongTap == phongTap.ID_PhongTap);

                    if (existingRoom != null)
                    {
                        // Update room properties
                        existingRoom.TenPhong = phongTap.TenPhong;
                        existingRoom.DiaChi = phongTap.DiaChi;
                        existingRoom.SucChua = phongTap.SucChua;
                        existingRoom.SlotToiDa = phongTap.SlotToiDa;
                        existingRoom.ID_TrainerPhuTrach = phongTap.ID_TrainerPhuTrach;
                        existingRoom.ID_User = phongTap.ID_User;
                        existingRoom.TinhTrang = phongTap.TinhTrang;
                        existingRoom.ViTri = phongTap.ViTri;

                        // Release old equipment
                        foreach (var oldEquipment in existingRoom.ThietBis)
                        {
                            oldEquipment.ID_Phong = 0;
                            oldEquipment.TinhTrang = "Khả dụng";
                        }

                        // Assign new equipment
                        if (selectedEquipment != null && selectedEquipment.Any())
                        {
                            var equipmentToUpdate = await _context.ThietBis
                                .Where(t => selectedEquipment.Contains(t.ID_ThietBi))
                                .ToListAsync();

                            foreach (var equipment in equipmentToUpdate)
                            {
                                equipment.ID_Phong = existingRoom.ID_PhongTap;
                                equipment.TinhTrang = "Đang sử dụng";
                            }
                        }

                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Cập nhật phòng tập thành công!";
                        return RedirectToAction(nameof(Rooms));
                    }
                    else
                    {
                        TempData["Error"] = "Không tìm thấy phòng tập để cập nhật.";
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật phòng tập: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            ViewBag.Trainers = await _context.Users.Where(u => u.Role.TenRole == "Huấn luyện viên").ToListAsync();
            ViewBag.AvailableEquipment = await _context.ThietBis
                .Where(t => t.ID_Phong == 0 || t.ID_Phong == null || t.ID_Phong == phongTap.ID_PhongTap)
                .ToListAsync();
            
            return View(phongTap);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var room = await _context.PhongTaps
                    .Include(p => p.ThietBis)
                    .FirstOrDefaultAsync(p => p.ID_PhongTap == id);

                if (room != null)
                {
                    // Release equipment back to available status
                    foreach (var equipment in room.ThietBis)
                    {
                        equipment.ID_Phong = 0;
                        equipment.TinhTrang = "Khả dụng";
                    }

                    // Remove room
                    _context.PhongTaps.Remove(room);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Xóa phòng tập thành công! Các thiết bị đã được chuyển về trạng thái khả dụng.";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy phòng tập để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa phòng tập: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Rooms));
        }

        // Package Management
        public async Task<IActionResult> Packages()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var packages = await _context.GoiTaps.ToListAsync();
                return View(packages ?? new List<GoiTap>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách gói tập: " + ex.Message;
                return View(new List<GoiTap>());
            }
        }

        public IActionResult CreatePackage()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage(GoiTap goiTap)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.GoiTaps.Add(goiTap);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo gói tập thành công!";
                    return RedirectToAction(nameof(Packages));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo gói tập: " + ex.Message);
            }
            
            return View(goiTap);
        }

        public async Task<IActionResult> EditPackage(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var package = await _context.GoiTaps.FindAsync(id);
                if (package == null)
                {
                    TempData["Error"] = "Không tìm thấy gói tập để chỉnh sửa.";
                    return RedirectToAction(nameof(Packages));
                }

                return View(package);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Packages));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditPackage(GoiTap goiTap)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.GoiTaps.Update(goiTap);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật gói tập thành công!";
                    return RedirectToAction(nameof(Packages));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật gói tập: " + ex.Message);
            }
            
            return View(goiTap);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var package = await _context.GoiTaps.FindAsync(id);
                if (package != null)
                {
                    _context.GoiTaps.Remove(package);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa gói tập thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy gói tập để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa gói tập: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Packages));
        }

        // Work Assignment
        public async Task<IActionResult> WorkAssignments()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var assignments = await _context.PhanCongs
                    .Include(p => p.User)
                    .Include(p => p.PhongTap)
                    .Include(p => p.CaLamViec)
                    .Include(p => p.CreatedByAdmin)
                    .ToListAsync();
                return View(assignments ?? new List<PhanCong>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách phân công: " + ex.Message;
                return View(new List<PhanCong>());
            }
        }

        public async Task<IActionResult> CreateAssignment()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                ViewBag.Users = await _context.Users
                    .Where(u => u.Role.TenRole == "Nhân viên" || u.Role.TenRole == "Huấn luyện viên")
                    .ToListAsync();
                ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
                ViewBag.WorkShifts = await _context.CaLamViecs.ToListAsync();
                ViewBag.Admins = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Users = new List<User>();
                ViewBag.Rooms = new List<PhongTap>();
                ViewBag.WorkShifts = new List<CaLamViec>();
                ViewBag.Admins = new List<User>();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(PhanCong phanCong)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    phanCong.NgayPhanCong = DateTime.Now;
                    phanCong.CreatedByAdminID = GetCurrentUserId() ?? 0;
                    
                    _context.PhanCongs.Add(phanCong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo phân công thành công!";
                    return RedirectToAction(nameof(WorkAssignments));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phân công: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users
                .Where(u => u.Role.TenRole == "Nhân viên" || u.Role.TenRole == "Huấn luyện viên")
                .ToListAsync();
            ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
            ViewBag.WorkShifts = await _context.CaLamViecs.ToListAsync();
            ViewBag.Admins = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            
            return View(phanCong);
        }

        public async Task<IActionResult> EditAssignment(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var assignment = await _context.PhanCongs.FindAsync(id);
                if (assignment == null)
                {
                    TempData["Error"] = "Không tìm thấy phân công để chỉnh sửa.";
                    return RedirectToAction(nameof(WorkAssignments));
                }

                ViewBag.Users = await _context.Users
                    .Where(u => u.Role.TenRole == "Nhân viên" || u.Role.TenRole == "Huấn luyện viên")
                    .ToListAsync();
                ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
                ViewBag.WorkShifts = await _context.CaLamViecs.ToListAsync();
                ViewBag.Admins = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                
                return View(assignment);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(WorkAssignments));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAssignment(PhanCong phanCong)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.PhanCongs.Update(phanCong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phân công thành công!";
                    return RedirectToAction(nameof(WorkAssignments));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật phân công: " + ex.Message);
            }
            
            ViewBag.Users = await _context.Users
                .Where(u => u.Role.TenRole == "Nhân viên" || u.Role.TenRole == "Huấn luyện viên")
                .ToListAsync();
            ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
            ViewBag.WorkShifts = await _context.CaLamViecs.ToListAsync();
            ViewBag.Admins = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            
            return View(phanCong);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var assignment = await _context.PhanCongs.FindAsync(id);
                if (assignment != null)
                {
                    _context.PhanCongs.Remove(assignment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa phân công thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy phân công để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa phân công: " + ex.Message;
            }
            
            return RedirectToAction(nameof(WorkAssignments));
        }

        // Reports
        public async Task<IActionResult> Reports()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var reports = await _context.BaoCaos
                    .Include(b => b.User)
                    .ToListAsync();
                return View(reports ?? new List<BaoCao>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách báo cáo: " + ex.Message;
                return View(new List<BaoCao>());
            }
        }

        // Invoices
        public async Task<IActionResult> PaymentInvoices()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var invoices = await _context.HoaDon_ThanhToans
                    .Include(h => h.TheHoiVien)
                    .Include(h => h.GoiTap)
                    .Include(h => h.User)
                    .ToListAsync();
                return View(invoices ?? new List<HoaDon_ThanhToan>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách hóa đơn thanh toán: " + ex.Message;
                return View(new List<HoaDon_ThanhToan>());
            }
        }

        public async Task<IActionResult> DisposalInvoices()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var invoices = await _context.HoaDon_ThanhLys
                    .Include(h => h.ThietBi)
                    .Include(h => h.User)
                    .ToListAsync();
                return View(invoices ?? new List<HoaDon_ThanhLy>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách hóa đơn thanh lý: " + ex.Message;
                return View(new List<HoaDon_ThanhLy>());
            }
        }

        // Equipment Management
        public async Task<IActionResult> Equipment()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var equipment = await _context.ThietBis
                    .Include(t => t.PhongTap)
                    .Include(t => t.User)
                    .ToListAsync();
                return View(equipment ?? new List<ThietBi>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách thiết bị: " + ex.Message;
                return View(new List<ThietBi>());
            }
        }

        public async Task<IActionResult> CreateEquipment()
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                ViewBag.Rooms = new List<PhongTap>();
                ViewBag.Users = new List<User>();
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipment(ThietBi thietBi)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.ThietBis.Add(thietBi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo thiết bị thành công!";
                    return RedirectToAction(nameof(Equipment));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo thiết bị: " + ex.Message);
            }
            
            ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            return View(thietBi);
        }

        public async Task<IActionResult> EditEquipment(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var equipment = await _context.ThietBis.FindAsync(id);
                if (equipment == null)
                {
                    TempData["Error"] = "Không tìm thấy thiết bị để chỉnh sửa.";
                    return RedirectToAction(nameof(Equipment));
                }

                ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
                ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
                return View(equipment);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Equipment));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEquipment(ThietBi thietBi)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.ThietBis.Update(thietBi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật thiết bị thành công!";
                    return RedirectToAction(nameof(Equipment));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thiết bị: " + ex.Message);
            }
            
            ViewBag.Rooms = await _context.PhongTaps.ToListAsync();
            ViewBag.Users = await _context.Users.Where(u => u.Role.TenRole == "Admin").ToListAsync();
            return View(thietBi);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthenticated();
            if (authCheck != null) return authCheck;

            try
            {
                var equipment = await _context.ThietBis.FindAsync(id);
                if (equipment != null)
                {
                    _context.ThietBis.Remove(equipment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa thiết bị thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy thiết bị để xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa thiết bị: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Equipment));
        }
    }
} 