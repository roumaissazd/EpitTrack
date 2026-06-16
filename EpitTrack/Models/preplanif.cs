using EpitTrack.Data;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

using Syncfusion.EJ2.Spreadsheet;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace EpitTrack.Models
{
    [Table("db_preplanif", Schema = "public")]
    public class preplanif
    {
        [Key]
        [Required]
        [DisplayName("Id chauff")]
        public int id_preplanif { get; set; }

        [DisplayName("Id course")]
        public int id_course { get; set; }

        [DisplayName("date_course")]
        public DateOnly date_course { get; set; }

        [DisplayName("heure deb course")]
        public int hnum_deb_course { get; set; }

        [DisplayName("heure fin course")]
        public int hnum_fin_course { get; set; }
        public string? adr_dep_long { get; set; }
        public string? adr_dep_lat { get; set; }
        public string? adr_arr_long { get; set; }
        public string? adr_arr_lat { get; set; }

        [DisplayName("Id chauffeur")]
        public int id_chauff { get; set; }
        public virtual Chauffeur Chauffeur { get; set; }
        public virtual Course Lacourse { get; set; }
    }
}
