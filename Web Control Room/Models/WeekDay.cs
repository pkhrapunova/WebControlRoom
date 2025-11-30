using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("[День Недели]")]
    public class WeekDay
    {
        public int Id { get; set; } // Код
        public string Name { get; set; } = null!; // Название

        public ICollection<RoomOccupancy> Occupancies { get; set; }
    }
}
