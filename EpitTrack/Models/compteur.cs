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
    [Table("db_compteur", Schema = "public")]
    public class compteur
    {

        [Key]
        [Required]
        public int id_compt { get; set; }
        public int id_process_encours { get; set; }
        public int val_compt { get; set; }
        public int nb_courses_planifiees { get; set; }
        public int max_compt { get; set; }
        public decimal pourcent_compt { get; set; }
    }
}
