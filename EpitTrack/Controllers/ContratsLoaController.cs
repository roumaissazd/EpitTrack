using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EpitTrack.Data;
using EpitTrack.Models;

namespace EpitTrack.Controllers
{
    public class ContratsLoaController : Controller
    {
        private readonly AppDbContext _context;

        public ContratsLoaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ContratsLoa
        public async Task<IActionResult> Index()
        {
              return _context.ContratsLoa != null ? 
                          View(await _context.ContratsLoa.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.ContratsLoa'  is null.");
        }

        // GET: ContratsLoa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContratsLoa == null)
            {
                return NotFound();
            }

            var contratloa = await _context.ContratsLoa
                .FirstOrDefaultAsync(m => m.id_contratloa == id);
            if (contratloa == null)
            {
                return NotFound();
            }

            return View(contratloa);
        }

        // GET: ContratsLoa/Create
        public IActionResult Create()
        {
            return View(new contratloa());
        }

        // POST: ContratsLoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_contratloa,num_contratloa,immat_contratloa,date_deb_contratloa,date_fin_contratloa,nbre_mois_contratloa,mt_mois_ht_contratloa,mt_mois_ttc_contratloa,mt_1_mois_ht_contratloa,mt_1_mois_ttc_contratloa,mt_vr_ht_contratloa,mt_vr_ttc_contratloa")] contratloa contratloa)
        {
            if (ModelState.IsValid)
            {

                contratloa.date_deb_contratloa = contratloa.date_deb_contratloa.ToUniversalTime();
                contratloa.date_fin_contratloa = contratloa.date_fin_contratloa.ToUniversalTime();

                _context.Add(contratloa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contratloa);
        }

        // GET: ContratsLoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContratsLoa == null)
            {
                return NotFound();
            }

            var contratloa = await _context.ContratsLoa.FindAsync(id);
            if (contratloa == null)
            {
                return NotFound();
            }
            return View(contratloa);
        }

        // POST: ContratsLoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost, ActionName("EditConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_contratloa,num_contratloa,immat_contratloa,date_deb_contratloa,date_fin_contratloa,nbre_mois_contratloa,mt_mois_ht_contratloa,mt_mois_ttc_contratloa,mt_1_mois_ht_contratloa,mt_1_mois_ttc_contratloa,mt_vr_ht_contratloa,mt_vr_ttc_contratloa")] contratloa contratloa)
        {
            if (id != contratloa.id_contratloa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contratloa.date_deb_contratloa = contratloa.date_deb_contratloa.ToUniversalTime();
                    contratloa.date_fin_contratloa = contratloa.date_fin_contratloa.ToUniversalTime();

                    _context.Update(contratloa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!contratloaExists(contratloa.id_contratloa))
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
            return View(contratloa);
        }

        // GET: ContratsLoa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContratsLoa == null)
            {
                return NotFound();
            }

            var contratloa = await _context.ContratsLoa
                .FirstOrDefaultAsync(m => m.id_contratloa == id);
            if (contratloa == null)
            {
                return NotFound();
            }

            return View(contratloa);
        }

        // POST: ContratsLoa/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContratsLoa == null)
            {
                return Problem("Entity set 'AppDbContext.ContratsLoa'  is null.");
            }
            var contratloa = await _context.ContratsLoa.FindAsync(id);
            if (contratloa != null)
            {
                _context.ContratsLoa.Remove(contratloa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool contratloaExists(int id)
        {
          return (_context.ContratsLoa?.Any(e => e.id_contratloa == id)).GetValueOrDefault();
        }
    }
}
