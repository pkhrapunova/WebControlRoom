using System.ComponentModel.DataAnnotations.Schema;

namespace WebControlRoom.Models
{
    [Table("Факультет")]
    public class Faculty
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;

        public int? BuildingId { get; set; } 
        public Building? Building { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
