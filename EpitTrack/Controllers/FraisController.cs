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
    public class FraisController : Controller
    {
        private readonly AppDbContext _context;

        public FraisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: LesFrais
        public async Task<IActionResult> Index()
        {
            return _context.LesFrais != null ?
                        View(await _context.LesFrais.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.LesFrais'  is null.");
        }

        // GET: LesFrais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LesFrais == null)
            {
                return NotFound();
            }

            var frais = await _context.LesFrais
                .FirstOrDefaultAsync(m => m.id_frais == id);
            if (frais == null)
            {
                return NotFound();
            }

            return View(frais);
        }

        // GET: LesFrais/Create
        public IActionResult Create()
        {
            var _typesFrais = _context.TypesFrais.ToList();


            var selectList = new SelectList(_typesFrais.Select(x => new
            {
                Value = x.id_typefrais,
                Text = $"{x.type_frais}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
            }), "Value", "Text");

            ViewBag.typesFrais = selectList;
            return View();
        }

        // POST: LesFrais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_frais,id_typefrais,type_frais,date_deb_frais,date_fin_frais,mt_frais_ht,mt_frais_ttc,comment_frais")] frais frais)
        {
            if (ModelState.IsValid)
            {
                frais.date_deb_frais = frais.date_deb_frais.ToUniversalTime();
                frais.date_fin_frais = frais.date_fin_frais.ToUniversalTime();


                var _typefrais = await _context.TypesFrais.FindAsync(frais.id_typefrais);
                frais.type_frais = _typefrais.type_frais;
                _context.Add(frais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(frais);
        }

        // GET: LesFrais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LesFrais == null)
            {
                return NotFound();
            }

            //            var frais = await _context.LesFrais.FindAsync(id);

            var frais = await _context.LesFrais.Where(e => e.id_frais == id).FirstOrDefaultAsync();




            if (frais == null)
            {
                return NotFound();
            }
            return View(frais);
        }

        // POST: LesFrais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("id_frais,id_typefrais,type_frais,date_deb_frais,date_fin_frais,mt_frais_ht,mt_frais_ttc,comment_frais")] frais frais)
        {
            if (id != frais.id_frais)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    frais.date_deb_frais = frais.date_deb_frais.ToUniversalTime();
                    frais.date_fin_frais = frais.date_fin_frais.ToUniversalTime();
                    _context.Update(frais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!fraisExists(frais.id_frais))
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
            return View(frais);
        }

        // GET: LesFrais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LesFrais == null)
            {
                return NotFound();
            }

            var frais = await _context.LesFrais
                .FirstOrDefaultAsync(m => m.id_frais == id);
            if (frais == null)
            {
                return NotFound();
            }

            return View(frais);
        }

        // POST: LesFrais/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LesFrais == null)
            {
                return Problem("Entity set 'AppDbContext.LesFrais'  is null.");
            }
            var frais = await _context.LesFrais.FindAsync(id);
            if (frais != null)
            {
                _context.LesFrais.Remove(frais);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool fraisExists(int id)
        {
            return (_context.LesFrais?.Any(e => e.id_frais == id)).GetValueOrDefault();
        }
    }
}

