using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_frais", Schema = "public")]
    public class frais
    {
        [Key]
        [Required]

        [DisplayName("Identifiant")]
        public int id_frais { get; set; }

        public int id_typefrais { get; set; }

        [DisplayName("Type de frais")]
        public string type_frais { get; set; }

        [DisplayName("Deb Période")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_deb_frais { get; set; }

        [DisplayName("Fin Période")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_fin_frais { get; set; }

        [DisplayName("Mt Frais HT")]
        public decimal mt_frais_ht { get; set; }

        [DisplayName("Mt Frais TTC")]
        public decimal mt_frais_ttc { get; set; }

        [DisplayName("Commentaire")]
        public string? comment_frais { get; set; }
    }
}

