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
    public class SousClassOpsController : Controller
    {
        private readonly AppDbContext _context;

        public SousClassOpsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SousClassOps
        public async Task<IActionResult> Index()
        {
              return _context.SousClassOps != null ? 
                          View(await _context.SousClassOps.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.SousClassOp'  is null.");
        }

        // GET: SousClassOps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SousClassOps == null)
            {
                return NotFound();
            }

            var sousClassOp = await _context.SousClassOps
                .FirstOrDefaultAsync(m => m.id_sous_class_op == id);
            if (sousClassOp == null)
            {
                return NotFound();
            }

            return View(sousClassOp);
        }

        // GET: SousClassOps/Create
        public IActionResult Create()
        {
			var _ClassOp = _context.ClassOperations.ToList();


			var selectList = new SelectList(_ClassOp.Select(x => new
			{
				Value = x.id_class_op,
				Text = $"{x.lib_class_op}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
			}), "Value", "Text");

			ViewBag.ClassOp = selectList;
			return View();
        }

        // POST: SousClassOps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_sous_class_op,id_class_op,lib_sous_class_op")] SousClassOp sousClassOp)
        {
            

                var _ClassOp = await _context.ClassOperations.FindAsync(sousClassOp.id_class_op);

                sousClassOp.lib_class_op = _ClassOp.lib_class_op;

                _context.Add(sousClassOp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: SousClassOps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SousClassOps == null)
            {
                return NotFound();
            }

            var sousClassOp = await _context.SousClassOps.FindAsync(id);
            if (sousClassOp == null)
            {
                return NotFound();
            }
            return View(sousClassOp);
        }

        // POST: SousClassOps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_sous_class_op,id_class_op,lib_sous_class_op")] SousClassOp sousClassOp)
        {
            if (id != sousClassOp.id_sous_class_op)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sousClassOp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SousClassOpExists(sousClassOp.id_sous_class_op))
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
            return View(sousClassOp);
        }

        // GET: SousClassOps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SousClassOps == null)
            {
                return NotFound();
            }

            var sousClassOp = await _context.SousClassOps
                .FirstOrDefaultAsync(m => m.id_sous_class_op == id);
            if (sousClassOp == null)
            {
                return NotFound();
            }

            return View(sousClassOp);
        }

        // POST: SousClassOps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SousClassOps == null)
            {
                return Problem("Entity set 'AppDbContext.SousClassOp'  is null.");
            }
            var sousClassOp = await _context.SousClassOps.FindAsync(id);
            if (sousClassOp != null)
            {
                _context.SousClassOps.Remove(sousClassOp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SousClassOpExists(int id)
        {
          return (_context.SousClassOps?.Any(e => e.id_sous_class_op == id)).GetValueOrDefault();
        }
    }
}
