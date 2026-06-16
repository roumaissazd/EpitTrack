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
    public class CoutRhsController : Controller
    {
        private readonly AppDbContext _context;

        public CoutRhsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CoutRhs
        public async Task<IActionResult> Index()
        {
            return _context.CoutRhs != null ?
                        View(await _context.CoutRhs.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.CoutRhs'  is null.");
        }

        // GET: CoutRhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CoutRhs == null)
            {
                return NotFound();
            }

            var coutrh = await _context.CoutRhs
                .FirstOrDefaultAsync(m => m.id_coutrh == id);
            if (coutrh == null)
            {
                return NotFound();
            }

            return View(coutrh);
        }

        // GET: CoutRhs/Create
        public IActionResult Create()
        {
            var les_rh = _context.Rhs.ToList();


            var selectList = new SelectList(les_rh.Select(x => new
            {
                Value = x.id_rh,
                Text = $"{x.prenom_rh} {x.nom_rh}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
            }), "Value", "Text");

            ViewBag.LesRh = selectList;
            return View();
        }

        // POST: CoutRhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_coutrh, id_rh, prenom_rh, nom_rh, date_deb_coutrh, date_fin_coutrh, mt_cout_paie, mt_frais")] coutrh coutrh)
        {
            if (ModelState.IsValid)
            {
                coutrh.date_deb_coutrh = coutrh.date_deb_coutrh.ToUniversalTime();
                coutrh.date_fin_coutrh = coutrh.date_fin_coutrh.ToUniversalTime();
                
                var _rh = await _context.Rhs.FindAsync(coutrh.id_rh);
                coutrh.nom_rh = _rh.nom_rh;
                coutrh.prenom_rh = _rh.prenom_rh;
                _context.Add(coutrh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coutrh);
        }

        // GET: CoutRhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CoutRhs == null)
            {
                return NotFound();
            }

//            var coutrh = await _context.CoutRhs.FindAsync(id);

            var coutrh = await _context.CoutRhs.Where(e => e.id_coutrh == id).FirstOrDefaultAsync();




            if (coutrh == null)
            {
                return NotFound();
            }
            return View(coutrh);
        }

        // POST: CoutRhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_coutrh, id_rh, prenom_rh, nom_rh, date_deb_coutrh, date_fin_coutrh, mt_cout_paie, mt_frais")] coutrh coutrh)
        {
            if (id != coutrh.id_coutrh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    coutrh.date_deb_coutrh = coutrh.date_deb_coutrh.ToUniversalTime();
                    coutrh.date_fin_coutrh = coutrh.date_fin_coutrh.ToUniversalTime();
                    _context.Update(coutrh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!coutrhExists(coutrh.id_coutrh))
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
            return View(coutrh);
        }

        // GET: CoutRhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CoutRhs == null)
            {
                return NotFound();
            }

            var coutrh = await _context.CoutRhs
                .FirstOrDefaultAsync(m => m.id_coutrh == id);
            if (coutrh == null)
            {
                return NotFound();
            }

            return View(coutrh);
        }

        // POST: CoutRhs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CoutRhs == null)
            {
                return Problem("Entity set 'AppDbContext.CoutRhs'  is null.");
            }
            var coutrh = await _context.CoutRhs.FindAsync(id);
            if (coutrh != null)
            {
                _context.CoutRhs.Remove(coutrh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool coutrhExists(int id)
        {
            return (_context.CoutRhs?.Any(e => e.id_coutrh == id)).GetValueOrDefault();
        }
    }
}
