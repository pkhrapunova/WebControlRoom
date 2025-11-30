using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Аудитория")]
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = null!; 
        public int? Capacity { get; set; } 

        public int? RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public int? BuildingId { get; set; } 
        public Building? Building { get; set; }

        public ICollection<RoomOccupancy> Occupancies { get; set; }
    }
}
