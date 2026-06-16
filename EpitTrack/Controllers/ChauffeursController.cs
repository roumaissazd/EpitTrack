using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpitTrack.Data;
using EpitTrack.Models;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using EpitTrack.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EpitTrack.Controllers
{
    public class ChauffeursController : Controller
    {
        private readonly AppDbContext _context;

        public ChauffeursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Rhs
        public async Task<IActionResult> Index()
        {
              return _context.Chauffeurs != null ? 
                          View(await _context.Chauffeurs.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Rhs'  is null.");
        }

        // GET: Rhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chauffeurs == null)
            {
                return NotFound();
            }

            var ch = await _context.Chauffeurs
                .FirstOrDefaultAsync(m => m.id_chauff == id);
            if (ch == null)
            {
                return NotFound();
            }

            return View(ch);
        }

        // GET: chauffeur/Create
        public IActionResult Create()
        {
            List<string> options = new List<string> { "BERLINE", "TPMR", "7PLACES", "9PLACES" };
            string selectedValue = "BERLINE";
            SelectList selectList = new SelectList(options, selectedValue);
            ViewBag.typesVoiture = selectList;
            return View();
        }

        // POST: chauffeur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_chauff,nom_chauff, prenom_chauff, adr_chauff,cp_chauff, ville_chauff, pays_chauff,nbh_chauff,hdispo_chauff,position_chauff,pos_chauff_long,pos_chauff_lat,type_voiture_chauff")] Chauffeur ch)
        {
            if (ModelState.IsValid)
            {

                _context.Chauffeurs.Add(ch);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ch);
        }
 
        //GET: Absence/add
        public async Task<IActionResult> Absences(int? id)
        {
            if (id == null || _context.Chauffeurs == null)
            {
                return NotFound();
            }
            var ch = await _context.Chauffeurs
                              .Include(c => c.Indisponibilites)
                              .Include(c => c.preplanifs)
                              .Include(c=> c.preplanifs).ThenInclude(p=>p.Lacourse)
                              .FirstOrDefaultAsync(c => c.id_chauff == id);

            if (ch == null)
            {
                return NotFound();
            }
            
            List<string> Motifs = new List<string> { "CP", "MALADIE", "AUTRE" };
            SelectList selectList = new SelectList(Motifs);
            ViewBag.Motifs = selectList;

            IndisponibiliteViewModel indisponibiliteVM = new IndisponibiliteViewModel();
            indisponibiliteVM.notre_chauffeur = ch;
            indisponibiliteVM.IndisponibilitesExistantes = ch.Indisponibilites;
            indisponibiliteVM.PreplanifExistantes = ch.preplanifs;
            Indisponibilite newIndisp = new Indisponibilite();
            preplanif newPreplanif = new preplanif();
            indisponibiliteVM.NouvelleIndisponibilite = newIndisp;
            

            return View(indisponibiliteVM);
        }

        // POST: Absence/add

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateAbsence([Bind("id_indispo,date_deb_indispo_chauff,date_fin_indispo_chauff,num_deb_indispo_chauff,num_fin_indispo_chauff,motif_indispo_chauff,id_chauff,nom_chauff,prenom_chauff,chauffeur")] Indisponibilite Indps)
        public async Task<IActionResult> CreateAbsence(DateTime date_deb_indispo_chauff, DateTime date_reprise, string num_deb_indispo_chauff, string num_heure_reprise, string motif_indispo_chauff, int id_chauff,string nom_chauff,string prenom_chauff,string jour_complet, Chauffeur Chauffeur)
        {
            
            string date_deb_string = date_deb_indispo_chauff.ToString().Split(' ')[0];
            DateOnly _date_deb_indispo_chauff = DateOnly.Parse(date_deb_string);
            string date_reprise_string = date_reprise.ToString().Split(' ')[0];
            DateOnly _date_reprise = DateOnly.Parse(date_reprise_string);

            
            
            int _num_deb_indispo_chauff_heure  = string.IsNullOrEmpty(num_deb_indispo_chauff) ? 0 : int.Parse(num_deb_indispo_chauff.Split(':')[0]);
            int _num_deb_indispo_chauff_minute = string.IsNullOrEmpty(num_deb_indispo_chauff) ? 0 : int.Parse(num_deb_indispo_chauff.Split(':')[1]);

            int _num_heure_reprise_heure = string.IsNullOrEmpty(num_heure_reprise) ? 0 : int.Parse(num_heure_reprise.Split(':')[0]);
            int _num_heure_reprise_minute = string.IsNullOrEmpty(num_heure_reprise) ? 0 : int.Parse(num_heure_reprise.Split(':')[1]);
            
            Indisponibilite Indps = new Indisponibilite();
            Indps.id_chauff = id_chauff;
            Indps.nom_chauff = nom_chauff;
            Indps.prenom_chauff = prenom_chauff;
            bool isJourComplet = !string.IsNullOrEmpty(jour_complet) && jour_complet.Equals("true", StringComparison.OrdinalIgnoreCase);

            Indps.jour_complet = isJourComplet;
            var ch = await _context.Chauffeurs.FindAsync(Indps.id_chauff);
            Indps.date_deb_indispo_chauff = _date_deb_indispo_chauff;
            Indps.str_heure_deb_indispo_chauff = num_deb_indispo_chauff;
            Indps.str_heure_reprise = num_heure_reprise;
            Indps.date_reprise = _date_reprise;
            Indps.num_deb_indispo_chauff = (_num_deb_indispo_chauff_heure * 60) + _num_deb_indispo_chauff_minute;
            Indps.num_heure_reprise = (_num_heure_reprise_heure * 60) + _num_heure_reprise_minute;
            Indps.motif_indispo_chauff = motif_indispo_chauff;
            ch.Indisponibilites.Add(Indps);
            _context.Chauffeurs.Update(ch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Absences), new { id = Indps.id_chauff });
        }
       
        public async Task<IActionResult> DeleteAbsence(int id_chauff, int id_indps)
        {
            try
            {

                var mon_chauff = await _context.Chauffeurs.Include(c => c.Indisponibilites)
                                                          .FirstOrDefaultAsync(c => c.id_chauff == id_chauff);
                var my_indisp = mon_chauff.Indisponibilites.Where(id => id.id_indispo == id_indps).FirstOrDefault();
                mon_chauff.Indisponibilites.Remove(my_indisp);
                _context.Chauffeurs.Update(mon_chauff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Absences), new { id = mon_chauff.id_chauff });
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> DeletePreplanif(int id_chauff, int id_preplan)
        {
            try
            {

                var mon_chauff = await _context.Chauffeurs.Include(c => c.Indisponibilites)
                                                          .Include(c => c.preplanifs)
                                                          .FirstOrDefaultAsync(c => c.id_chauff == id_chauff);
                var my_preplanif = mon_chauff.preplanifs.Where(id => id.id_preplanif == id_preplan).FirstOrDefault();
                mon_chauff.preplanifs.Remove(my_preplanif);
                _context.Chauffeurs.Update(mon_chauff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Absences), new { id = mon_chauff.id_chauff });
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> GetCourseToAffect(DateOnly la_date)
        {
            var _courses = _context.Courses.Where(c => c.date_course == la_date).ToList(); 
            
            return Json(_courses);
        }


        public async Task<IActionResult> AddIndispo(int id)
        {
            if (id == null || _context.Chauffeurs == null)
            {
                return NotFound();
            }
            var ch = await _context.Chauffeurs
                              .Include(c => c.preplanifs)
                              .FirstOrDefaultAsync(c => c.id_chauff == id);

            if (ch == null)
            {
                return NotFound();
            }
            string selectedValue = "AFFECTATION";
            List<string> Motifs = new List<string> { "AFFECTATION", "ABSENCES", "SEUIL"};
            SelectList selectList = new SelectList(Motifs, selectedValue);
            ViewBag.Motifs = selectList;
            //ViewBag.chaffeur = ch.nom_chauff + " " + ch.prenom_chauff;
            return PartialView(ch);
        }
        
        
        

        











        // GET: Rhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chauffeurs == null)
            {
                return NotFound();

            }

            var ch = await _context.Chauffeurs.FindAsync(id);
            string selectedValue = ch.type_voiture_chauff;
            List<string> options = new List<string> { "BERLINE", "TPMR", "7PLACES", "9PLACES" };
            SelectList selectList = new SelectList(options, selectedValue);
            ViewBag.typesVoiture = selectList;
            if (ch == null)
            {
                return NotFound();
            }
            return View(ch);
        }

        // POST: Rhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_chauff,nom_chauff, prenom_chauff, adr_chauff,cp_chauff, ville_chauff, pays_chauff,nbh_chauff,hdispo_chauff,position_chauff,pos_chauff_long,pos_chauff_lat,type_voiture_chauff")] Chauffeur ch)
        {
            if (id != ch.id_chauff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!chExists(ch.id_chauff))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ch);
        }

        // GET: Rhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chauffeurs == null)
            {
                return NotFound();
            }

            var ch = await _context.Chauffeurs
                .FirstOrDefaultAsync(m => m.id_chauff == id);
            if (ch == null)
            {
                return NotFound();
            }

            return View(ch);
        }

        // POST: Rhs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chauffeurs == null)
            {
                return Problem("Entity set 'AppDbContext.chauffeurs'  is null.");
            }
            var ch = await _context.Chauffeurs.FindAsync(id);
            if (ch != null)
            {
                _context.Chauffeurs.Remove(ch);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool chExists(int id)
        {
          return (_context.Chauffeurs?.Any(e => e.id_chauff == id)).GetValueOrDefault();
        }
    }
}
