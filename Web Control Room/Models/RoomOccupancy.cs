using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("[Занятость Аудитории]")]
    public class RoomOccupancy
    {
        public int Id { get; set; } // Код

        public int? RoomId { get; set; } // КодАудитории
        public Room? Room { get; set; }

        public int? GroupId { get; set; } // КодГруппы
        public Group? Group { get; set; }

        public int? WeekDayId { get; set; } // КодДня
        public WeekDay? WeekDay { get; set; }

        public int? ClassTimeId { get; set; } // КодПары
        public ClassTime? ClassTime { get; set; }

        public DateTime StartDate { get; set; } // ДатаНачала
        public DateTime EndDate { get; set; } // ДатаОкончания
    }
}
