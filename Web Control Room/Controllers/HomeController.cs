using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebControlRoom.Models;

namespace WebControlRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly DispatcherContext _context;

        public HomeController(DispatcherContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int? buildingId,
            int? roomTypeId,
            int? classTimeId,
            int? minCapacity,
            bool? hasMultimedia,
            DateTime? date)
        {
            ViewBag.Buildings = await _context.Buildings.ToListAsync();
            ViewBag.RoomTypes = await _context.RoomTypes.ToListAsync();
            ViewBag.ClassTimes = await _context.ClassTimes.ToListAsync();
            ViewBag.SelectedMultimedia = hasMultimedia;

            if (!date.HasValue)
            {
                date = DateTime.Today;
            }
            ViewBag.SelectedDate = date.Value.ToString("yyyy-MM-dd");

            var now = DateTime.Now.TimeOfDay;

            var currentClassTimeId = await _context.ClassTimes
                .Where(p => p.StartTime <= now && p.EndTime >= now)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            if (currentClassTimeId == 0)
            {
                currentClassTimeId = await _context.ClassTimes
                    .Where(p => p.StartTime > now)
                    .OrderBy(p => p.StartTime)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();
            }

            if (currentClassTimeId == 0)
            {
                currentClassTimeId = await _context.ClassTimes
                    .OrderByDescending(p => p.StartTime)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();
            }

            if (!classTimeId.HasValue)
            {
                classTimeId = currentClassTimeId;
            }

            ViewBag.SelectedClassTimeId = classTimeId;

            var dayOfWeek = (int)date.Value.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;

            var occupiedRooms = _context.RoomOccupancies
                .Where(o => o.ClassTimeId == classTimeId
                            && date >= o.StartDate
                            && date <= o.EndDate
                            && o.WeekDayId == dayOfWeek)
                .Select(o => o.RoomId);

            var freeRoomsQuery = _context.Rooms
                .Where(r => !occupiedRooms.Contains(r.Id))
                .Where(r => !buildingId.HasValue || r.BuildingId == buildingId)
                .Where(r => !roomTypeId.HasValue || r.RoomTypeId == roomTypeId)
                .Where(r => !minCapacity.HasValue || r.Capacity >= minCapacity)
                .Where(r => !hasMultimedia.HasValue || r.Multimedia == hasMultimedia)
                .Include(r => r.Building)
                .Include(r => r.RoomType);

            var freeRooms = await freeRoomsQuery.ToListAsync();

            return View(freeRooms);
        }
    }
}
