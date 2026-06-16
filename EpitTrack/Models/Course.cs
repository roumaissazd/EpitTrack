using EpitTrack.Data;
using EpitTrack.Services; 
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

using Syncfusion.EJ2.Spreadsheet;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EpitTrack.Models
{

    [Table("db_courses", Schema = "public")]
    
        public class Course
        {
             [Key]
            [Required]
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
        public string? adr_dep_lat { get;set; }
        public string? adr_arr_long { get;set; }
        public string? adr_arr_lat { get; set; } 
        public decimal ca_ht_chauff { get; set; }
        public decimal id_chauff_p { get; set; }
        public Course()
        { }
        public bool ImportCourses(IFormFile file, AppDbContext _context)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();

            file.CopyTo(stream);
            ExcelPackage package = new ExcelPackage(stream);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); //

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var laCourse = new Course();
                DateOnly ladate = System.DateOnly.Parse(worksheet.Cells[row, 1].Text.Substring(0, 10));
                laCourse.date_course = ladate;
                laCourse.nom_passagers = fctFormatString(worksheet.Cells[row, 2].Text);
                laCourse.heur_deb_course = Convert_heur_to_int(worksheet.Cells[row, 3].Text);
                laCourse.heur_fin_course = Convert_heur_to_int(worksheet.Cells[row, 4].Text); 
                laCourse.adr_depart = fctFormatString(worksheet.Cells[row, 5].Text);
                laCourse.adr_arrivee = fctFormatString(worksheet.Cells[row, 6].Text);
                laCourse.prix_vente_ht = decimal.Parse(worksheet.Cells[row, 7].Text.Replace(".", ","));
                laCourse.mt_tva = decimal.Parse(worksheet.Cells[row, 8].Text.Replace(".", ","));
                laCourse.prix_vente_ttc = decimal.Parse(worksheet.Cells[row, 9].Text.Replace(".", ","));
                laCourse.no_dossier = worksheet.Cells[row, 10].Text;
                laCourse.statut_mission = worksheet.Cells[row, 11].Text;
                if(string.IsNullOrEmpty(worksheet.Cells[row, 12].Text.Replace(".", ",")))
                {
                    laCourse.km_inclus = 0;
                }
                else
                {
                    laCourse.km_inclus = decimal.Parse(worksheet.Cells[row, 12].Text.Replace(".", ","));
                }
                
                laCourse.prix_achat_ht = decimal.Parse(worksheet.Cells[row, 13].Text.Replace(".", ","));
                laCourse.prenom_chauff = worksheet.Cells[row, 14].Text;
                laCourse.nom_chauff = worksheet.Cells[row, 15].Text;
                laCourse.tel_chauff = worksheet.Cells[row, 16].Text;
                laCourse.nom_client = worksheet.Cells[row, 17].Text;
                laCourse.immat = worksheet.Cells[row, 18].Text;
                laCourse.partenaire = worksheet.Cells[row, 19].Text;
                laCourse.model_vehic = worksheet.Cells[row, 20].Text;
                laCourse.no_mission = int.Parse(worksheet.Cells[row, 21].Text);
                laCourse.km_deb = (worksheet.Cells[row, 22].Text == "") ? 0 : decimal.Parse(worksheet.Cells[row, 22].Text.Replace(".", ","));
                laCourse.km_fin = (worksheet.Cells[row, 23].Text == "") ? 0 : decimal.Parse(worksheet.Cells[row, 23].Text.Replace(".", ","));
                laCourse.ref_client_dossier = worksheet.Cells[row, 24].Text;
                laCourse.type_service = worksheet.Cells[row, 25].Text;
                laCourse.h_deb_service = Convert_heur_to_int(worksheet.Cells[row, 26].Text);
                laCourse.h_fin_service = Convert_heur_to_int(worksheet.Cells[row, 27].Text);
                laCourse.type_vehicule = worksheet.Cells[row, 28].Text;
                laCourse.id_chauff = 0;
                _context.Courses.Add(laCourse);
            };
            _context.SaveChanges();
            /* Calcul longitudes et latitudes */
           // bool majlonglat = await maj_longlat(_context, ladate);
            return true;
        }
        /*
        public async Task<bool> maj_longlat(AppDbContext _context, string ladate_courses)
        {
            DateOnly La_date_courses = DateOnly.ParseExact(ladate_courses, "yyyy-MM-dd");
            var lescourses = await _context.Courses.Where(course => course.date_course == La_date_courses).OrderBy(course => course.heur_deb_course).ToListAsync();
            
            ;
            
            foreach (var lacourse in lescourses)
            {
                try
                {
                    var coordonneesAdrDep = await _geocodageService.ObtenirCoordonnees(lacourse.adr_depart);
                    lacourse.adr_dep_lat = coordonneesAdrDep.Item1;
                    lacourse.adr_dep_long = coordonneesAdrDep.Item2;
                    var coordonneesAdrArr = await _geocodageService.ObtenirCoordonnees(lacourse.adr_arrivee);
                    lacourse.adr_arr_lat = coordonneesAdrArr.Item1;
                    lacourse.adr_arr_long = coordonneesAdrArr.Item2;
                    _context.Courses.Update(lacourse);
                    await _context.SaveChangesAsync();

                }
                catch
                {

                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        */











        /*
        public int prochaine_course(string position_encours, int heure_circuit, AppDbContext _context)
        {
            var plus_proche_Course = new Course();

                foreach (Course course in _context.Courses.Where(course=>course.id_next_course==0).ToList())
                {
                  plus_proche_Course = _context.Courses.Where(course => course.heur_deb_course+duree_approche(position_encours,course.adr_depart) >= heure_circuit)
                                                       .OrderBy(course => course.heur_deb_course - heure_circuit)
                                                       .FirstOrDefault();

                course.id_next_course = plus_proche_Course.id_course;
                _context.Update(plus_proche_Course);
                _context.SaveChangesAsync();
            }
            return plus_proche_Course.id_course;
        }
        */
        public int Convert_heur_to_int(string heure_str)
        {
            if (string.IsNullOrEmpty(heure_str))
            {
                return 0;
            }
            else
            { 
             int num_heures = string.IsNullOrEmpty(heure_str.Split(':')[0]) ? 0 : int.Parse(heure_str.Split(':')[0]);
             int num_minutes = string.IsNullOrEmpty(heure_str.Split(':')[1]) ? 0 : int.Parse(heure_str.Split(':')[1]);
        
            return (num_heures * 60) + num_minutes;
            }

        }

        public int duree_approche(string adr_dep, string adr_arr)
        {
            return 30;
        }

        public static string fctFormatString(string text)
        {
            // Récupération de la chaine de caractère originale
            StringBuilder newText = new StringBuilder(text);

            // Gestion des "accents"
            // -> déclaration de variables de conversion "accents"
            string specialCaracter = "Ã©;Ã¨;Ã¿;Ãª;Ã‰";
            string normalCaracter = "é;è;ÿ;ê;É";
            // -> conversion des chaines en tableaux de caractères
            string[] tabAccent = specialCaracter.Split(';');
            string[] tabSansAccent = normalCaracter.Split(';');
            
            // -> pour chaque accent, remplacement
            for (int i = 0; i < tabAccent.Length; i++)
            {
                newText.Replace(tabAccent[i].ToString(), tabSansAccent[i].ToString());
            }

            return newText.ToString();
        }


    }
}
