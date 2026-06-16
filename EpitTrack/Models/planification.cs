using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_planification", Schema = "public")]
    public class planification
    {
      
        [Key]
        [Required]
        
        public int id_planification { get; set; }
        public int id_planif { get; set; }
        public int id_course { get; set; }
        [DisplayName("date_course")]
        public DateOnly date_course { get; set; }
        public string nom_passagers { get; set; }
        public int heur_deb_course { get; set; }
        public int heur_fin_course { get; set; }

        public int heur_moy_fin_course { get; set; }
        public string adr_depart { get; set; }
        public string adr_arrivee { get; set; }
        public decimal prix_vente_ht { get; set; }
        public decimal mt_tva { get; set; }
        public decimal prix_vente_ttc { get; set; }
        public string no_dossier { get; set; }
        public string statut_mission { get; set; }
        public decimal km_inclus { get; set; }
        public decimal km_moy_course { get; set; }
        public decimal prix_achat_ht { get; set; }
        public string prenom_chauff { get; set; }
        public string nom_chauff { get; set; }
        public string tel_chauff { get; set; }
        public string nom_client { get; set; }
        public string immat { get; set; }
        public string partenaire { get; set; }
        public string model_vehic { get; set; }
        public decimal no_mission { get; set; }
        public decimal km_deb { get; set; }
        public decimal km_fin { get; set; }
        public string ref_client_dossier { get; set; }
        public string type_service { get; set; }
        public int h_deb_service { get; set; }
        public int h_fin_service { get; set; }
        public string type_vehicule { get; set; }
        public decimal id_next_course { get; set; }
        public decimal id_chauff { get; set; }
        public decimal id_chauff1 { get; set; }
        public decimal id_chauff2 { get; set; }
        public decimal id_chauff3 { get; set; }
        public string? adr_dep_long { get; set; }
        public string? adr_dep_lat { get; set; }
        public string? adr_arr_long { get; set; }
        public string? adr_arr_lat { get; set; }
        public decimal ca_ht_chauff { get; set; }
        public decimal id_chauff_p { get; set; }
    }
}
