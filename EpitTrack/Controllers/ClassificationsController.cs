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
    public class ClassificationsController : Controller
    {
        private readonly AppDbContext _context;

        public ClassificationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Classifications
        public async Task<IActionResult> Index()
        {
            var classifications = await _context.Classifications
        .Include(c => c.ClassOp)
        .Include(c => c.SousClassOp)
        .ToListAsync();
            return _context.Classifications != null ? 
                          View(await _context.Classifications.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Classifications'  is null.");
        }

        // GET: Classifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classifications == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications
                .FirstOrDefaultAsync(m => m.id_classification == id);
            if (classification == null)
            {
                return NotFound();
            }

            return View(classification);
        }

        // GET: Classifications/Create
        public IActionResult Create()
        {
            var _ClassOp = _context.ClassOperations.ToList();


            var selectList = new SelectList(_ClassOp.Select(x => new
            {
                Value = x.id_class_op,
                Text = $"{x.lib_class_op}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
            }), "Value", "Text");

            ViewBag.ClassOp = selectList;

            var _SousClassOp = _context.SousClassOps.ToList();


            var sous_selectList = new SelectList(_SousClassOp.Select(x => new
            {
                Value = x.id_sous_class_op,
                Text = $"{x.lib_sous_class_op}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
            }), "Value", "Text");

            ViewBag.SousClassOp = sous_selectList;

            return View();
        }

        // POST: Classifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_classification,lib_to_class,id_class_op,lib_class_op,id_sous_class_op,lib_sous_class_op")] Classification classification)
        {
            var class_op = await _context.ClassOperations.FindAsync(classification.id_class_op);
            var sous_class_op = await _context.SousClassOps.FindAsync(classification.id_sous_class_op);
            classification.ClassOp = class_op;
            classification.SousClassOp = sous_class_op;
            classification.lib_class_op = class_op.lib_class_op;
            classification.lib_sous_class_op = sous_class_op.lib_sous_class_op;
           _context.Add(classification);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index)); 
        }


        public IActionResult GetSousClasses(int classOpId)
        {
            var sousClasses = _context.SousClassOps
                .Where(sc => sc.id_class_op == classOpId)
                .Select(sc => new
                {
                    value = sc.id_sous_class_op,
                    text = sc.lib_sous_class_op
                })
                .ToList();

            return Json(sousClasses);
        }


        // GET: Classifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classifications == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications.FindAsync(id);
            if (classification == null)
            {
                return NotFound();
            }
            return View(classification);
        }

        // POST: Classifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_classification,lib_to_class,id_class_op,lib_class_op,id_sous_class_op,lib_sous_class_op")] Classification classification)
        {
            if (id != classification.id_classification)
            {
                return NotFound();
            }
            var class_op = await _context.ClassOperations.FindAsync(classification.id_class_op);
            var sous_class_op = await _context.SousClassOps.FindAsync(classification.id_sous_class_op);
            classification.ClassOp = class_op;
            classification.SousClassOp = sous_class_op;
            classification.lib_class_op = class_op.lib_class_op;
            classification.lib_sous_class_op = sous_class_op.lib_sous_class_op;

            
                try
                {
                    _context.Update(classification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassificationExists(classification.id_classification))
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

        // GET: Classifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classifications == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications
                .FirstOrDefaultAsync(m => m.id_classification == id);
            if (classification == null)
            {
                return NotFound();
            }

            return View(classification);
        }

        // POST: Classifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classifications == null)
            {
                return Problem("Entity set 'AppDbContext.Classifications'  is null.");
            }
            var classification = await _context.Classifications.FindAsync(id);
            if (classification != null)
            {
                _context.Classifications.Remove(classification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassificationExists(int id)
        {
          return (_context.Classifications?.Any(e => e.id_classification == id)).GetValueOrDefault();
        }
    }
}
