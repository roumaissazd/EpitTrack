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
    public class RhsController : Controller
    {
        private readonly AppDbContext _context;

        public RhsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Rhs
        public async Task<IActionResult> Index()
        {
              return _context.Rhs != null ? 
                          View(await _context.Rhs.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Rhs'  is null.");
        }

        // GET: Rhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rhs == null)
            {
                return NotFound();
            }

            var rh = await _context.Rhs
                .FirstOrDefaultAsync(m => m.id_rh == id);
            if (rh == null)
            {
                return NotFound();
            }

            return View(rh);
        }

        // GET: Rhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_rh,prenom_rh,nom_rh,poste_rh,date_embauche_rh,salaire_net_rh")] rh rh)
        {
            if (ModelState.IsValid)
            {
                rh.date_embauche_rh = rh.date_embauche_rh.ToUniversalTime();
                _context.Add(rh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rh);
        }

        // GET: Rhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rhs == null)
            {
                return NotFound();
            }

            var rh = await _context.Rhs.FindAsync(id);
            if (rh == null)
            {
                return NotFound();
            }
            return View(rh);
        }

        // POST: Rhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_rh,prenom_rh,nom_rh,poste_rh,date_embauche_rh,salaire_net_rh")] rh rh)
        {
            if (id != rh.id_rh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rh.date_embauche_rh = rh.date_embauche_rh.ToUniversalTime();
                    _context.Update(rh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rhExists(rh.id_rh))
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
            return View(rh);
        }

        // GET: Rhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rhs == null)
            {
                return NotFound();
            }

            var rh = await _context.Rhs
                .FirstOrDefaultAsync(m => m.id_rh == id);
            if (rh == null)
            {
                return NotFound();
            }

            return View(rh);
        }

        // POST: Rhs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rhs == null)
            {
                return Problem("Entity set 'AppDbContext.Rhs'  is null.");
            }
            var rh = await _context.Rhs.FindAsync(id);
            if (rh != null)
            {
                _context.Rhs.Remove(rh);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool rhExists(int id)
        {
          return (_context.Rhs?.Any(e => e.id_rh == id)).GetValueOrDefault();
        }
    }
}
