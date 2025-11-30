using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Группа")]
    public class Group
    {
        public int Id { get; set; } // Код
        public string Name { get; set; } = null!; // Название
        public string Specialty { get; set; } = null!; // Специальность
        public int Course { get; set; } // Курс
        public DateTime? SemesterStart { get; set; } // ДатаНачалаСеместра
        public DateTime? SemesterEnd { get; set; } // ДатаОкончанияСеместра
        public int SemesterNumber { get; set; } // НомерСеместра
        public int StudentCount { get; set; } // Численность

        public int? FacultyId { get; set; } // КодФакультета
        public Faculty? Faculty { get; set; }

        public ICollection<RoomOccupancy> Occupancies { get; set; }
    }
}
