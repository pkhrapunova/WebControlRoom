using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Вид")]
    public class RoomType
    {
        public int Id { get; set; } 
        public string TypeName { get; set; } = null!; 

        public ICollection<Room> Rooms { get; set; }
    }
}
