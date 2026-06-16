using System.ComponentModel.DataAnnotations;
namespace EpitTrack.Models
{
    public class ChauffeurPossible
    {
        [Key]
        [Required]
        public long? chu_id { get; set; }
        public int h_arrivee { get; set; }
        public float distance { get; set; }
    }
}
