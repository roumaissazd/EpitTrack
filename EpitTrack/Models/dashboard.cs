using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_dashboard", Schema = "public")]
    public class dashboard
    {
        [Key]
        [Required]
        public int id_dashboard { get; set; }

        [DisplayName("Date Deb")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_deb_periode { get; set; }

        [DisplayName("Date Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_fin_periode { get; set; }

        [DisplayName("Mois")]

        public string mois_cout { get; set; }

        [DisplayName("Annee")]
        public int annee_cout { get; set; }

        [DisplayName("Leasing")]
        public decimal cout_leasing { get; set; }

        [DisplayName("Carburant")]
        public decimal cout_carburant { get; set; }

        [DisplayName("Ressources Humaines")]
        public decimal cout_rh { get; set; }

        [DisplayName("Les Frais")]
        public decimal mt_frais { get; set; }
    }
}

