using EpitTrack.Data;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

using Syncfusion.EJ2.Spreadsheet;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


namespace EpitTrack.Models
{
    [Table("db_chauffeur", Schema = "public")]
    public class Chauffeur
    {
        [Key]
        [Required]
        [DisplayName("Id chauff")]
        public int id_chauff { get; set; }

        [DisplayName("Nom chauff")]
        public string nom_chauff { get; set; }

        [DisplayName("Prénom chauff")]
        public string prenom_chauff { get; set; }

        [DisplayName("Adresse chauff")]
        public string adr_chauff { get; set; }

        [DisplayName("CP chauff")]
        public int cp_chauff { get; set; }

        [DisplayName("Ville chauff")]
        public string ville_chauff { get; set; }

        [DisplayName("Pays chauff")]
        public string pays_chauff { get; set; }

        [DisplayName("Nombre Heure travail chauff")]
        public int nbh_chauff { get; set; }

        [DisplayName("Heure Dispo chauff")]
        public int hdispo_chauff { get; set; }

        [DisplayName("Position chauff")]
        public string position_chauff { get; set; }

        [DisplayName("Longitude Position chauff")]
        public string pos_chauff_long { get; set; }

        [DisplayName("Latitude Position chauff")]
        public string pos_chauff_lat { get; set; }

        [DisplayName("Type de voiture")]
        public string type_voiture_chauff { get; set; }

        [DisplayName("CA HT chauffeur")]
        public decimal ca_ht_chauff { get; set; }

        public string etat_chauff { get; set; }
        public virtual ICollection<preplanif> preplanifs { get; set; }
        public virtual ICollection<Indisponibilite> Indisponibilites { get; set; }
        public Chauffeur()
        {
            preplanifs = new HashSet<preplanif>();
            Indisponibilites = new HashSet<Indisponibilite>();
        }

    }
}




