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
            DateTime? date)
        {
            // Загружаем данные для фильтров
            ViewBag.Buildings = await _context.Buildings.ToListAsync();
            ViewBag.RoomTypes = await _context.RoomTypes.ToListAsync();
            ViewBag.ClassTimes = await _context.ClassTimes.ToListAsync();

            // Если дата или пара не выбраны — возвращаем пустой список
            if (!date.HasValue || !classTimeId.HasValue)
            {
                return View(new List<Room>());
            }

            var dayOfWeek = (int)date.Value.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek; // Преобразуем: Sunday=7

            // Находим занятые аудитории на выбранную дату и день недели
            var occupiedRooms = _context.RoomOccupancies
                .Where(o => o.ClassTimeId == classTimeId
                            && date >= o.StartDate
                            && date <= o.EndDate
                            && o.WeekDayId == dayOfWeek)
                .Select(o => o.RoomId);

            // Находим свободные аудитории
            var freeRooms = await _context.Rooms
                .Where(r => !occupiedRooms.Contains(r.Id))
                .Where(r => !buildingId.HasValue || r.BuildingId == buildingId)
                .Where(r => !roomTypeId.HasValue || r.RoomTypeId == roomTypeId)
                .Where(r => !minCapacity.HasValue || r.Capacity >= minCapacity)
                .Include(r => r.Building)
                .Include(r => r.RoomType)
                .ToListAsync();

            return View(freeRooms);
        }
    }
}
