using EpitTrack.Data;
using EpitTrack.Models;
using EpitTrack.Services;
using EpitTrack.ViewModels;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Syncfusion.EJ2.Spreadsheet;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EpitTrack.Controllers
{
    public class BanqueController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IGeocodageService _geocodageService;
        public BanqueController(AppDbContext context, IGeocodageService geocodageService)
        {
            _context = context;
            _geocodageService = geocodageService;

        }




        // GET: CoursesController
        public async Task<ActionResult> Index(string p_date_deb, string p_date_fin)
        {
            var BanqueViewModel = new BanquesViewModel();

            if (p_date_deb is null)
            {
                BanqueViewModel.date_deb = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                BanqueViewModel.date_deb = DateOnly.Parse(p_date_deb.Substring(0, 10));
            }

            if (p_date_fin is null)
            {
                BanqueViewModel.date_fin = DateOnly.FromDateTime(DateTime.Now);
            }

            else
            {
                BanqueViewModel.date_fin = DateOnly.Parse(p_date_fin.Substring(0, 10));
            }

            var banques = await _context.CaBanques.Where(op => op.date_banque >= BanqueViewModel.date_deb && op.date_banque <= BanqueViewModel.date_fin).OrderBy(op => op.date_banque).ToListAsync();
            var classes = await _context.ClassOperations.OrderBy(c => c.id_class_op).ToListAsync();
            var sousclasses = await _context.SousClassOps.OrderBy(c => c.id_sous_class_op).ToListAsync();
         
            BanqueViewModel.OperationsBanqueCA = banques;
            BanqueViewModel.lesClassesOperations = classes;
            BanqueViewModel.lesSousClassesOperations = sousclasses;

            return View(BanqueViewModel);
        }

        public async Task<ActionResult> ValidDatesPeriode(DateTime date_deb, DateTime date_fin)
        {
            var _date_deb = DateOnly.FromDateTime(date_deb);
            var _date_fin = DateOnly.FromDateTime(date_fin);

            

            return RedirectToAction("Index", "Banque", new { p_date_deb = _date_deb.ToString(), p_date_fin = _date_fin.ToString() });
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
            bool import = await ImportBanque(file, _context);
            return RedirectToAction("Index", "Banque");
        }

        public async Task<bool> ImportBanque(IFormFile file, AppDbContext _context)
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();

            file.CopyTo(stream);
            ExcelPackage package = new ExcelPackage(stream);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); //

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                if (row ==460)
                {
                    var test = 0;
                }
                var labanque = new BanqueCa();
                DateOnly dateBanque = DateOnly.Parse(worksheet.Cells[row, 1].Text.Substring(0, 10));
                DateOnly dateValeur = DateOnly.Parse(worksheet.Cells[row, 2].Text.Substring(0, 10));
                labanque.date_banque = dateBanque;
                labanque.date_valeur = dateValeur;
                labanque.lib_operation = fctFormatString(worksheet.Cells[row, 3].Text);
                if (string.IsNullOrEmpty(worksheet.Cells[row, 4].Text.Replace(".", ",")))
                {
                    labanque.mt_debit = 0;
                }
                else
                {

                    labanque.mt_debit = decimal.Parse(worksheet.Cells[row, 4].Text.Replace(".", ","));
                }
                if (string.IsNullOrEmpty(worksheet.Cells[row, 5].Text.Replace(".", ",")))
                {
                    labanque.mt_credit = 0;
                }
                else
                {
                    labanque.mt_credit = decimal.Parse(worksheet.Cells[row, 5].Text.Replace(".", ","));
                }
                _context.CaBanques.Add(labanque);
               
            };

            _context.SaveChanges();
            return true;
        }

        public async Task<ActionResult> Classifier_operation(string date_deb, string date_fin)
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT prc_classifier_op({0},{1})", DateOnly.Parse(date_deb), DateOnly.Parse(date_fin));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Banque", new { p_date_deb = date_deb, p_date_fin = date_fin });

        }



        public async Task<bool> Affecter_class(int p_idbanque, int p_idclass, int p_id_sous_class)
        {

            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT prc_affecter_class({0},{1},{2})", p_idbanque, p_idclass, p_id_sous_class);
                _context.SaveChanges();
            }
            catch
            {

            }

            return true;
        }

        public async Task<bool> Affecter_sous_class(int p_idbanque, int p_id_sous_class)
        {

            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT prc_affecter_sous_class({0},{1})", p_idbanque, p_id_sous_class);
                _context.SaveChanges();
            }
            catch
            {

            }

            return true;
        }

        public async Task<bool> Annuler_Affect_C(int p_idbanque)
        {

            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT prc_annul_affect_class({0})", p_idbanque);
                _context.SaveChanges();
            }
            catch
            {

            }

            return true;
        }



        [HttpPost]
        public async Task<IActionResult> ChargerSc(int p_idclass)
        {
            var selectList = new SelectList(_context.SousClassOps.Where(sc => sc.id_class_op == p_idclass)
                                                             .Select(sc => new
                                                             {
                                                                 Value = sc.id_sous_class_op,
                                                                 Text = sc.lib_sous_class_op
                                                             }), "Value", "Text");

            var items = selectList.Select(item =>
                new Dictionary<string, object>
                {
            { "Value", item.Value },
            { "Text", item.Text }
                }
            ).ToList();

            return Json(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetSousClasses(int idClassOp)
        {
            var sousClasses = await _context.SousClassOps
                                            .Where(s => s.id_class_op == idClassOp)
                                            .ToListAsync();
            return Json(sousClasses.Select(s => new { id = s.id_sous_class_op, lib = s.lib_sous_class_op}));
        }


       




        public async Task<IActionResult> visualiser_repartition()
        {
            return View();
        }




        /*public IActionResult ChartData()
        {
            var lesSorties = _context.CaBanques
                .Where(b => b.mt_debit > 0 && b.id_class_op > 0 && b.id_sous_class_op > 0)
                .GroupBy(b => new
                    {
                        Mois = b.date_valeur.Month,
                        b.id_class_op,
                        b.id_sous_class_op,
                        b.lib_class_op,
                        b.lib_sous_class_op
                    })
                .Select(g => new
                    {
                      Mois = g.Key.Mois,
                      LibClassOp = g.Key.lib_class_op,
                      LibSousClassOp = g.Key.lib_sous_class_op,
                      SumMtDebit = g.Sum(b => b.mt_debit)
                 })
               .ToList();

            var lesEntrees = _context.CaBanques
                .Where(b => b.mt_credit > 0 && b.id_class_op > 0 && b.id_sous_class_op > 0)
                .GroupBy(b => new
                {
                    Mois = b.date_valeur.Month,
                    b.id_class_op,
                    b.id_sous_class_op,
                    b.lib_class_op,
                    b.lib_sous_class_op
                })
                .Select(g => new
                {
                    Mois = g.Key.Mois,
                    LibClassOp = g.Key.lib_class_op,
                    LibSousClassOp = g.Key.lib_sous_class_op,
                    SumMtCredit = g.Sum(b => b.mt_credit)
                })
               .ToList();

            var chartData = new { Entrees = lesEntrees, Sorties = lesSorties };

            return Json(chartData);
        }*/


        /*
        public IActionResult ChartData(string month, string type)
        {


            //  int mois = GetMonthNumberFromName(month);
            int mois = ConvertirMoisToInt(month);

            if (mois > 0 && !string.IsNullOrEmpty(type))
            {
                var details = _context.CaBanques
                    .Where(b => (type == "Entrées" ? b.mt_credit > 0 : b.mt_debit > 0) && b.date_valeur.Month == mois)
                    .GroupBy(b => b.lib_class_op)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Total = g.Sum(b => type == "Entrées" ? b.mt_credit : b.mt_debit)
                    })
                    .ToList();

                return Json(details);
            }
            else
            {
                var lesSorties = _context.CaBanques
                    .Where(b => b.mt_debit > 0)
                    .GroupBy(b => b.date_valeur.Month)
                    .Select(g => new
                    {
                        Mois = g.Key,
                        SumMtDebit = g.Sum(b => b.mt_debit)
                    })
                    .ToList();

                var lesEntrees = _context.CaBanques
                    .Where(b => b.mt_credit > 0)
                    .GroupBy(b => b.date_valeur.Month)
                    .Select(g => new
                    {
                        Mois = g.Key,
                        SumMtCredit = g.Sum(b => b.mt_credit)
                    })
                    .ToList();

                return Json(new { Entrees = lesEntrees, Sorties = lesSorties });
            }

        }
        */

       

public decimal[][] ChartData()
    {
        // Définir la culture à "InvariantCulture" pour utiliser le point comme séparateur décimal
        CultureInfo ci = CultureInfo.InvariantCulture;

            


            var entrees = _context.CaBanques
            .Where(b => b.mt_credit > 0)
            .GroupBy(b => b.date_valeur.Month)
            .Select(g => new {
                Mois = g.Key,
                SumMtCredit = g.Sum(b => b.mt_credit)  // Conversion en chaîne avec format décimal
            })
            .OrderBy(x => x.Mois)
            .ToList();

        var sorties = _context.CaBanques
            .Where(b => b.mt_debit > 0)
            .GroupBy(b => b.date_valeur.Month)
            .Select(g => new {
                Mois = g.Key,
                SumMtDebit = g.Sum(b => b.mt_debit)  // Conversion en chaîne avec format décimal
            })
            .OrderBy(x => x.Mois)
            .ToList();

        var result = new
        {
            Entrees = entrees,
            Sorties = sorties
        };

            var LesEntrees = new decimal[12];
            var LesSorties = new decimal[12];
            decimal [][] LesEntreesSorties = new decimal[2][];


            for (int i = 0; i < 12; i++)
            {
                int currentMonth = i + 1;  // Les mois dans votre source de données sont probablement de 1 à 12

                // Trouver l'entrée pour le mois actuel, sinon utiliser 0 si aucune entrée n'est trouvée
                var entree = entrees.FirstOrDefault(e => e.Mois == currentMonth);
                LesEntrees[i] = entree != null ? entree.SumMtCredit : 0;

                // De même pour les sorties
                var sortie = sorties.FirstOrDefault(s => s.Mois == currentMonth);
                LesSorties[i] = sortie != null ? sortie.SumMtDebit : 0;
            }

            LesEntreesSorties[0] = LesEntrees;
            LesEntreesSorties[1] = LesSorties;

            return LesEntreesSorties;
    }

        public IActionResult ChartDetailData(string month, string type)
        {
            int mois = ConvertirMoisToInt(month);
            if (mois > 0 && !string.IsNullOrEmpty(type))
            {
                var details = _context.CaBanques
                    .Where(b => (type == "Entrées" ? b.mt_credit > 0 : b.mt_debit > 0) && b.date_valeur.Month == mois)
                    .GroupBy(b => b.lib_class_op)
                    .Select(g => new {
                        Name = g.Key,
                        Total = g.Sum(b => type == "Entrées" ? b.mt_credit : b.mt_debit)
                    })
                    .ToList();

                var formattedData = details.Select(d => new {
                    name = d.Name,
                    y = d.Total
                }).ToList();

                return Json(formattedData);
            }
            else
            {
                // Vérifiez que cette partie n'est pas toujours exécutée par erreur
                return Json(new { Error = "Invalid month or type" });
            }


            /*
            // Définir la culture à "InvariantCulture" pour utiliser le point comme séparateur décimal
            CultureInfo ci = CultureInfo.InvariantCulture;




            var entrees = _context.CaBanques
            .Where(b => b.mt_credit > 0)
            .GroupBy(b => b.date_valeur.Month)
            .Select(g => new {
                Mois = g.Key,
                SumMtCredit = g.Sum(b => b.mt_credit)  // Conversion en chaîne avec format décimal
            })
            .OrderBy(x => x.Mois)
            .ToList();

            var sorties = _context.CaBanques
                .Where(b => b.mt_debit > 0)
                .GroupBy(b => b.date_valeur.Month)
                .Select(g => new {
                    Mois = g.Key,
                    SumMtDebit = g.Sum(b => b.mt_debit)  // Conversion en chaîne avec format décimal
                })
                .OrderBy(x => x.Mois)
                .ToList();

            var result = new
            {
                Entrees = entrees,
                Sorties = sorties
            };

            var LesEntrees = new decimal[12];
            var LesSorties = new decimal[12];
            decimal[][] LesEntreesSorties = new decimal[2][];


            for (int i = 0; i < 12; i++)
            {
                int currentMonth = i + 1;  // Les mois dans votre source de données sont probablement de 1 à 12

                // Trouver l'entrée pour le mois actuel, sinon utiliser 0 si aucune entrée n'est trouvée
                var entree = entrees.FirstOrDefault(e => e.Mois == currentMonth);
                LesEntrees[i] = entree != null ? entree.SumMtCredit : 0;

                // De même pour les sorties
                var sortie = sorties.FirstOrDefault(s => s.Mois == currentMonth);
                LesSorties[i] = sortie != null ? sortie.SumMtDebit : 0;
            }

            LesEntreesSorties[0] = LesEntrees;
            LesEntreesSorties[1] = LesSorties;

            return LesEntreesSorties;
            */
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

        public static int GetMonthNumberFromName(string monthName)
        {
            try
            {
                // Création d'une instance de DateTimeFormatInfo associée à la culture française
                DateTimeFormatInfo dtfi = new CultureInfo("fr-FR").DateTimeFormat;

                // Récupération du numéro du mois à partir du nom du mois
                int monthNumber = Array.IndexOf(dtfi.MonthNames, monthName) + 1;
                return monthNumber;
            }
            catch
            {
                // Gérer l'exception si le nom du mois est invalide
                return 0; // Retourner 0 ou lancer une exception selon le besoin
            }
        }

        public int ConvertirMoisToInt(string mois)
        {
            switch (mois)
            {
                case "Janvier":
                    return 1;

                case "Février":
                    return 2;

                case "Mars":
                    return 3;

                case "Avril":
                    return 4;

                case "Mai":
                    return 5;

                case "Juin":
                    return 6;

                case "Juillet":
                    return 7;

                case "Août":
                    return 8;

                case "Septembre":
                    return 9;

                case "Octobre":
                    return 10;

                case "Novembre":
                    return 11;

                case "Décembre":
                    return 12;

                default:
                    return 0;
            }
        }
    }
}
       
