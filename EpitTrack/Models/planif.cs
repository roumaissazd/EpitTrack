using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_planif", Schema = "public")]
    public class planif
    {
        [Key]
        [Required]
        [DisplayName("Id planif")]
        public int id_planif { get; set; }

        [DisplayName("Date Planif")]
        public DateOnly date_planif { get; set; }

        [DisplayName("Date Courses")]
        public DateOnly date_courses { get; set; }

        [DisplayName("CA HT Planif")]
        public decimal ca_ht_planif { get; set; }

        [DisplayName("Nb Chauffeurs")]
        public int nb_chauff { get; set; }

        [DisplayName("Nb Courses")]
        public int nb_courses { get; set; }

        [DisplayName("Nb Courses planifiées")]
        public int nb_courses_planifiees { get; set; }

        [DisplayName("Nb KM Courses")]
        public int nb_km_courses { get; set; }

        [DisplayName("Nb Heures Courses")]
        public int nb_h_courses { get; set; }

        [DisplayName("Val Compteur")]
        public int val_compt { get; set; }


        [DisplayName("Max Compteur")]
        public int max_compt { get; set; }

        [DisplayName("Poucentage Compteur")]
        public decimal pourcent_compt { get; set; }

        [DisplayName("Lib Planif")]
        public string lib_planif { get; set; }

        [DisplayName("Tps Attente Chauff")]
        public int tps_attente { get; set; }

    }
}
