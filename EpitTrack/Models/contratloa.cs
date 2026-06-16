using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_contratloa", Schema ="public")]
    public class contratloa
    {
        [Key]
        [Required]
        public int id_contratloa { get; set; }
        
        [DisplayName("No Contrat")]
        public string num_contratloa { get; set; }

        [DisplayName("Immat Vehic.")]
        public string immat_contratloa { get; set; }
        
        [DisplayName("Date Deb")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_deb_contratloa { get; set; }

        [DisplayName("Date Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime date_fin_contratloa { get; set; }

        [DisplayName("Nbre Mois")]
        public int nbre_mois_contratloa { get; set; }

        [DisplayName("Loyer HT")]
        public decimal mt_mois_ht_contratloa { get; set; }

        [DisplayName("Loyer TTC")]
        public decimal mt_mois_ttc_contratloa { get; set; }

        [DisplayName("Prem.Loyer HT")]
        public decimal mt_1_mois_ht_contratloa { get; set; }

        [DisplayName("Prem.Loyer TTC")]
        public decimal mt_1_mois_ttc_contratloa { get; set; }

        [DisplayName("VR HT")]
        public decimal mt_vr_ht_contratloa { get; set; }

        [DisplayName("VR TTC")]
        public decimal mt_vr_ttc_contratloa { get; set; }
    }
}
