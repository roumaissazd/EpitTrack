using EpitTrack.Data;
using EpitTrack.Models;
using EpitTrack.ViewModels;
using Google.OrTools.ConstraintSolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Globalization;

namespace EpitTrack.Controllers
{
    public class RentabiliteController : Controller
    {

        private readonly AppDbContext _context;

        public RentabiliteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RentabiliteController
        public async Task<ActionResult> Index()
        {
            bool calculer = await Calcul_Rentab();
            bool alimenter = await Get_Rentab();

            List<Rentabilite> listData = await _context.Rentabilites.ToListAsync();

            /*
            ViewData["listRentabByAnnee"] = listData.OrderByDescending(r => r.annee_courses).ToList();

            List<Rentabilite> listData2024 = Rentabilite.GetRentabilite(2024, 0, "", "", "BY-MONTH");
            Highcharts columnChartRes = InitGraphic(listData2024);
            */
            return View(listData);

        }

        public async Task<IActionResult> AffichRentabByMonths(int p_annee)
        {
           
            bool alimenter = await Get_Rentab_By_Month();
            List<Rentabilite> listData = await _context.Rentabilites.Where(r=>r.annee_courses== p_annee).ToListAsync();
            return PartialView("AffichRentabByMonths", listData);
        }



        // GET: RentabiliteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RentabiliteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RentabiliteController/Create
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

        // GET: RentabiliteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RentabiliteController/Edit/5
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

        // GET: RentabiliteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RentabiliteController/Delete/5
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

        public  async Task<Boolean> Calcul_Rentab()
        {

            var result = await _context.Database.ExecuteSqlRawAsync("SELECT prc_calcul_rentab()");
            return true;

        }
        public async Task<Boolean> Get_Rentab()
        {

            var result = await _context.Database.ExecuteSqlRawAsync("SELECT prc_get_rentab_annuelle()");
            return true;
        }
        public async Task<Boolean> Get_Rentab_By_Month()
        {

            var result = await _context.Database.ExecuteSqlRawAsync("SELECT prc_get_rentab_by_month()");
            return true;
        }
       

    }

}
