                                                                                                                                                                                                                                                                                                                                                                                                     using EpitTrack.Data;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

using Syncfusion.EJ2.Spreadsheet;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;
namespace EpitTrack.Models
{
    [Table("db_courses_references", Schema = "public")]
    public class CourseDeReference
    {
        [Key]
        [Required]
        public int id_course_ref { get; set; }

        [DisplayName("Nom Passagers")]
        public string nom_passagers_ref { get; set; }
        
        [DisplayName("Heure deb")]
        public int heur_deb_course_ref { get; set; }

        [DisplayName("Heure fin Estimée")]
        public int heur_fin_course_ref { get; set; }

        [DisplayName("adresse départ")]
        public string adr_depart_ref { get; set; }

        [DisplayName("adresse d'arrivée")]
        public string adr_arrivee_ref { get; set; }

        [DisplayName("distance")]
        public decimal moy_km_course_ref { get; set; }
        [DisplayName("prix vente ht")]
        public decimal prix_vente_ht_ref { get; set; }
        public string? adr_dep_long_ref { get; set; }
        public string? adr_dep_lat_ref { get; set; }
        public string? adr_arr_long_ref { get; set; }
        public string? adr_arr_lat_ref { get; set; }
        public int moy_h_deb_service_ref { get; set; }

        [DisplayName("Heure fin Moyenne")]
        public int moy_h_fin_service_ref { get; set; }

        [DisplayName("chauffeur favoris 1 ")]
        public int id_chauff_1_ref { get; set; }
        public string nom_chauff_1_ref { get; set; }
        public string prenom_chauff_1_ref { get; set; }

        [DisplayName("chauffeur favoris 2 ")]
        public int id_chauff_2_ref { get; set; }
        public string nom_chauff_2_ref { get; set; }
        public string prenom_chauff_2_ref { get; set; }

        [DisplayName("chauffeur favoris 3 ")]
        public int id_chauff_3_ref { get; set; }
        public string nom_chauff_3_ref { get; set; }
        public string prenom_chauff_3_ref { get; set; }
    }
}
