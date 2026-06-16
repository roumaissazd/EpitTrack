
using EpitTrack.Data;
using EpitTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EpitTrack.Controllers
{
    public class UbersController : Controller
    {
        private readonly AppDbContext _context;

        public UbersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CoutRhs


        public async Task<IActionResult> Index()
        {
            
            List<uber> lesUbers = await _context.ubers.ToListAsync();
            return View(lesUbers);
        }

        [HttpPost]
        public IActionResult Importer(IFormFile file)
        {
            uber _uber = new uber();
            if (file.Length > 0)
                {
                    if (_uber.ImportUber(file, _context))
                    {

                    };

                }
            return View();
        }
    }


}

    

