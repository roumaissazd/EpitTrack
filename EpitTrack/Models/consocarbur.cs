using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_consocarbur", Schema = "public")]
    public class consocarbur
    {

        [Key]
        [Required]
        public int id_consocarbur { get; set; }

        [DisplayName("Date Début Conso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_deb_consocarbur { get; set; }

        [DisplayName("Date Fin Conso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_fin_consocarbur { get; set; }

        [DisplayName("Montant Conso HT ")]
        public decimal mt_ht_consocarbur { get; set; }

        [DisplayName("Montant Conso TTC ")]
        public decimal mt_ttc_consocarbur { get; set; }


    }
}
