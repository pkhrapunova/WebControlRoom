using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Корпус")]
    public class Building
    {
        public int Id { get; set; } 
        public int Number { get; set; } 

        public ICollection<Faculty> Faculties { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}