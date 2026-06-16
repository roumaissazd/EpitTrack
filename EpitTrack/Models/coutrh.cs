using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_coutrh", Schema = "public")]
    public class coutrh
    {
        [Key]
        [Required]
        public int id_coutrh { get; set; }

        [DisplayName("Id Rh")]
        public int id_rh { get; set; }

        [DisplayName("Prénom")]
        public string prenom_rh { get; set; }

        [DisplayName("Nom")]
        public string nom_rh { get; set; }

        [DisplayName("Début période")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_deb_coutrh { get; set; }

        [DisplayName("Fin Période")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_fin_coutrh { get; set; }


        [DisplayName("Coût globale paie")]
        public decimal mt_cout_paie { get; set; }

        [DisplayName("MT des Frais")]
        public decimal mt_frais { get; set; }
    }
}
