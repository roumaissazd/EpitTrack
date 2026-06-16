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

namespace EpitTrack.Models
{
    [Table("db_indisponibilite", Schema = "public")]
    public class Indisponibilite
    {
        [Key]
        [Required]
        public int id_indispo { get; set; }

        [DisplayName("Date Deb Absence")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly date_deb_indispo_chauff { get; set; }
        
        [DisplayName("Date Reprise")]
        [DataType(DataType.Date)]
        
        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public DateOnly date_reprise { get; set; }
        
        [DisplayName("Heure Deb Absence")]
        public string str_heure_deb_indispo_chauff { get; set; }

        [DisplayName("Heure Deb Absence")]
        public int num_deb_indispo_chauff { get; set; }

        [DisplayName("Heure Reprise")]

        public string str_heure_reprise { get; set; }
        [DisplayName("Heure Reprise")]
        public int num_heure_reprise { get; set; }

        [DisplayName("Motif Absence")]
        public string motif_indispo_chauff { get; set; }
        public int id_chauff { get; set; }
        [DisplayName("Nom chauff")]
        public string nom_chauff { get; set; }

        [DisplayName("Prénom chauff")]
        public string prenom_chauff { get; set; }

        [DisplayName("Jour compet")]
        public bool jour_complet { get; set; }
        public virtual Chauffeur Chauffeur { get; set; }
    }
}
