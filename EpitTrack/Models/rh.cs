using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_rh", Schema = "public")]
    public class rh
    {
        [Key]
        [Required]
        public int id_rh { get; set; }

        [DisplayName("Prénom")]
        public string prenom_rh { get; set; }

        [DisplayName("Nom")]
        public string nom_rh { get; set; }

        [DisplayName("Poste")]
        public string poste_rh { get; set; }

        [DisplayName("Date Embauche")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_embauche_rh { get; set; }

        [DisplayName("Rémunération Net")]
        public decimal salaire_net_rh { get; set; }
        
    }
}

