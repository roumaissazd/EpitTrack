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
    public class TypeFraisController : Controller
    {
        private readonly AppDbContext _context;

        public TypeFraisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TypesFrais
        public async Task<IActionResult> Index()
        {
            return _context.TypesFrais != null ?
                        View(await _context.TypesFrais.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.TypesFrais'  is null.");
        }

        // GET: TypesFrais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypesFrais == null)
            {
                return NotFound();
            }

            var typefrais = await _context.TypesFrais.FirstOrDefaultAsync(m => m.id_typefrais == id);
            if (typefrais == null)
            {
                return NotFound();
            }

            return View(typefrais);
        }

        // GET: TypesFrais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypesFrais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_typefrais,type_frais,comment_typefrais")] typefrais typefrais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typefrais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typefrais);
        }

        // GET: TypesFrais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypesFrais == null)
            {
                return NotFound();
            }

            //            var typefrais = await _context.TypesFrais.FindAsync(id);

            var typefrais = await _context.TypesFrais.Where(e => e.id_typefrais == id).FirstOrDefaultAsync();




            if (typefrais == null)
            {
                return NotFound();
            }
            return View(typefrais);
        }

        // POST: TypesFrais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_typefrais,type_frais,comment_typefrais")] typefrais typefrais)
        {
            if (id != typefrais.id_typefrais)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typefrais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!typefraisExists(typefrais.id_typefrais))
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
            return View(typefrais);
        }

        // GET: TypesFrais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypesFrais == null)
            {
                return NotFound();
            }

            var typefrais = await _context.TypesFrais
                .FirstOrDefaultAsync(m => m.id_typefrais == id);
            if (typefrais == null)
            {
                return NotFound();
            }

            return View(typefrais);
        }

        // POST: TypesFrais/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypesFrais == null)
            {
                return Problem("Entity set 'AppDbContext.TypesFrais'  is null.");
            }
            var typefrais = await _context.TypesFrais.FindAsync(id);
            if (typefrais != null)
            {
                _context.TypesFrais.Remove(typefrais);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool typefraisExists(int id)
        {
            return (_context.TypesFrais?.Any(e => e.id_typefrais == id)).GetValueOrDefault();
        }
    }
}

