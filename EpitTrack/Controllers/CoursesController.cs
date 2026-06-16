using EpitTrack.Data;
using EpitTrack.Models;
using EpitTrack.Services;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Syncfusion.EJ2.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EpitTrack.Controllers
{
    public class CoursesController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IGeocodageService _geocodageService;
        public CoursesController(AppDbContext context, IGeocodageService geocodageService)
        { 
            _context = context;
            _geocodageService = geocodageService;
            
        }




        // GET: CoursesController
        public async Task<ActionResult> Index(string date_courses)
        {
            var Les_courses = new List<Course>();
            if (date_courses is null)
            {
                Les_courses = _context.Courses.ToList();
            }
            else
            {
                var _date_courses = DateOnly.Parse(date_courses.Substring(0, 10));
                Les_courses =await _context.Courses.Where(course => course.date_course == _date_courses).OrderBy(course => course.heur_deb_course).ToListAsync();
            }
            
            return View(Les_courses);
        }

        // GET: CoursesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoursesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoursesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoursesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CoursesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Importer(IFormFile file)

        {
            

            if (file is not null && file.Length > 0)
            {
                string DateOfCourses = "";
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                MemoryStream stream = new MemoryStream();

                file.CopyTo(stream);
                ExcelPackage package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); //
                DateOfCourses = worksheet.Cells[2, 1].Text.Substring(0, 10);
                DateOnly ladate = DateOnly.Parse(DateOfCourses);
                var Exist_Courses = await _context.Courses.Where(c => c.date_course == ladate).ToListAsync();
                TempData["Message"] = "";

                if (Exist_Courses.Count()>0)
                { 

                    TempData["Message"] = "Les courses de "+ DateOfCourses + " ont déjà importé";
                }
                else
                {
                    TempData["Message"] = "Importation des courses du  " + DateOfCourses;

                    bool import = await ImportCourses(file, _context, ladate);
                    if (import)
                    {
                        TempData["Message"] = "Le fichier " + file.FileName + " a été importé avec succés";
                        ViewBag.date_courses = ladate;
                    }
                }
            }
            return RedirectToAction("Index", "Courses");

        }

        public async Task<bool> ImportCourses(IFormFile file, AppDbContext _context, DateOnly the_dateofcourse)
        {
            
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();

            file.CopyTo(stream);
            ExcelPackage package = new ExcelPackage(stream);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); //

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var laCourse = new Course();
                DateOnly ladate = DateOnly.Parse(worksheet.Cells[row, 1].Text.Substring(0, 10));
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
                if (string.IsNullOrEmpty(worksheet.Cells[row, 12].Text.Replace(".", ",")))
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
            bool majlonglat = await maj_longlat();
            return true;
        }
        public async Task<bool> maj_longlat()
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_adresses()");
            var lescourses = await _context.Courses.Where(course => course.adr_dep_lat == null || course.adr_arr_lat ==null).OrderBy(course => course.heur_deb_course).ToListAsync();

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
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_moy()");
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_preferences()");
          return true;
        }
        public async Task<ActionResult> ValidMajLongLat(DateTime date_courses)
        {
            var _date_courses = DateOnly.FromDateTime(date_courses);
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_adresses()");

            var lescourses = await _context.Courses.Where(course => course.date_course == _date_courses).OrderBy(course => course.heur_deb_course).ToListAsync();

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
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_moy()");
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_preferences()");
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Courses", new { date_courses = date_courses.ToString() });
        }



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
