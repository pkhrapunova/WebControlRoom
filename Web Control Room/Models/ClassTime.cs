using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Пара")]
    public class ClassTime
    {
        public int Id { get; set; } // Код
        public int Number { get; set; } // Номер
        public TimeSpan StartTime { get; set; } // ВремяНачала
        public TimeSpan EndTime { get; set; } // ВремяОкончания

        public ICollection<RoomOccupancy> Occupancies { get; set; }
    }
}
