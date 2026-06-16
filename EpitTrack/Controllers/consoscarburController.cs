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
    public class consoscarburController : Controller
    {
        private readonly AppDbContext _context;

        public consoscarburController(AppDbContext context)
        {
            _context = context;
        }

        // GET: consoscarbur
        public async Task<IActionResult> Index()
        {
              return _context.ConsosCarbur != null ? 
                          View(await _context.ConsosCarbur.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.ConsosCarbur'  is null.");
        }

        // GET: consoscarbur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ConsosCarbur == null)
            {
                return NotFound();
            }

            var consocarbur = await _context.ConsosCarbur
                .FirstOrDefaultAsync(m => m.id_consocarbur == id);
            if (consocarbur == null)
            {
                return NotFound();
            }

            return View(consocarbur);
        }

        // GET: consoscarbur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: consoscarbur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_consocarbur,date_deb_consocarbur,date_fin_consocarbur,mt_ht_consocarbur,mt_ttc_consocarbur")] consocarbur consocarbur)
        {
            if (ModelState.IsValid)
            {
                consocarbur.date_deb_consocarbur = consocarbur.date_deb_consocarbur.ToUniversalTime();
                consocarbur.date_fin_consocarbur = consocarbur.date_fin_consocarbur.ToUniversalTime();
                _context.Add(consocarbur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consocarbur);
        }

        // GET: consoscarbur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ConsosCarbur == null)
            {
                return NotFound();
            }

            var consocarbur = await _context.ConsosCarbur.FindAsync(id);
            if (consocarbur == null)
            {
                return NotFound();
            }
            return View(consocarbur);
        }

        // POST: consoscarbur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_consocarbur,date_deb_consocarbur,date_deb_fincarbur,mt_ht_consocarbur,mt_ttc_consocarbur")] consocarbur consocarbur)
        {
            if (id != consocarbur.id_consocarbur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    consocarbur.date_deb_consocarbur = consocarbur.date_deb_consocarbur.ToUniversalTime();
                    consocarbur.date_fin_consocarbur = consocarbur.date_fin_consocarbur.ToUniversalTime();
                    _context.Update(consocarbur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!consocarburExists(consocarbur.id_consocarbur))
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
            return View(consocarbur);
        }

        // GET: consoscarbur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ConsosCarbur == null)
            {
                return NotFound();
            }

            var consocarbur = await _context.ConsosCarbur
                .FirstOrDefaultAsync(m => m.id_consocarbur == id);
            if (consocarbur == null)
            {
                return NotFound();
            }

            return View(consocarbur);
        }

        // POST: consoscarbur/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ConsosCarbur == null)
            {
                return Problem("Entity set 'AppDbContext.ConsosCarbur'  is null.");
            }
            var consocarbur = await _context.ConsosCarbur.FindAsync(id);
            if (consocarbur != null)
            {
                _context.ConsosCarbur.Remove(consocarbur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool consocarburExists(int id)
        {
          return (_context.ConsosCarbur?.Any(e => e.id_consocarbur == id)).GetValueOrDefault();
        }
    }
}
