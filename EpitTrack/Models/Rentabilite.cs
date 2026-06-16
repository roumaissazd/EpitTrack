using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EpitTrack.Models
{
    [Table("db_rentab", Schema = "public")]
    public class Rentabilite
        {
            [Key, Column(Order = 0)]
            public int annee_courses { get; set; }
            
            [Key, Column(Order = 1)]
            public int mois_courses { get; set; }
             
            [Key, Column(Order = 2)]
            public int no_dossier { get; set; }
            
            [Key, Column(Order = 3)]
            public int no_mission { get; set; }
            public string lib_mois_courses { get; set; }
            public DateTime date_course { get; set; }
            public int nb_courses { get; set; }
            
            public string nom_client { get; set; }
            public string nom_passager { get; set; }
            public float nb_km_inclus { get; set; }
            public float nb_km_reels { get; set; }
            public float prix_vente_ht { get; set; }
            public float prix_vente_km { get; set; }
            public float prix_vente_ttc { get; set; }
            public float cout_fixe { get; set; }
            public float cout_chauffeur { get; set; }
            public float cout_carburant { get; set; }
            public float marge_brute { get; set; }
        }
}
