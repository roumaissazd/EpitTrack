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
    public class ClassOperationsController : Controller
    {
        private readonly AppDbContext _context;

        public ClassOperationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ClassOperations
        public async Task<IActionResult> Index()
        {
              return _context.ClassOperations != null ? 
                          View(await _context.ClassOperations.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.ClassOperations'  is null.");
        }

        // GET: ClassOperations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassOperations == null)
            {
                return NotFound();
            }

            var classOperation = await _context.ClassOperations
                .FirstOrDefaultAsync(m => m.id_class_op == id);
            if (classOperation == null)
            {
                return NotFound();
            }

            return View(classOperation);
        }

        // GET: ClassOperations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassOperations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_class_op,lib_class_op")] ClassOperation classOperation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classOperation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classOperation);
        }

        // GET: ClassOperations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassOperations == null)
            {
                return NotFound();
            }

            var classOperation = await _context.ClassOperations.FindAsync(id);
            if (classOperation == null)
            {
                return NotFound();
            }
            return View(classOperation);
        }

        // POST: ClassOperations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_class_op,lib_class_op")] ClassOperation classOperation)
        {
            if (id != classOperation.id_class_op)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classOperation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassOperationExists(classOperation.id_class_op))
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
            return View(classOperation);
        }

        // GET: ClassOperations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassOperations == null)
            {
                return NotFound();
            }

            var classOperation = await _context.ClassOperations
                .FirstOrDefaultAsync(m => m.id_class_op == id);
            if (classOperation == null)
            {
                return NotFound();
            }

            return View(classOperation);
        }

        // POST: ClassOperations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassOperations == null)
            {
                return Problem("Entity set 'AppDbContext.ClassOperation'  is null.");
            }
            var classOperation = await _context.ClassOperations.FindAsync(id);
            if (classOperation != null)
            {
                _context.ClassOperations.Remove(classOperation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassOperationExists(int id)
        {
          return (_context.ClassOperations?.Any(e => e.id_class_op == id)).GetValueOrDefault();
        }
    }
}
