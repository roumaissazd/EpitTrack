using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EpitTrack.Data;
using EpitTrack.Models;
using EpitTrack.ViewModels;
using Microsoft.EntityFrameworkCore;

public class SelectClassViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public SelectClassViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int idBanque)
    {
        //string _etat = "";
        /*
        var _banque_op = await _context.CaBanques.Where(b => b.id_banque == idBanque).FirstAsync();
            if (_banque_op.id_class_op > 0 )
            {
                  _etat = "CLASSE";
            }
        */
        var _SelectClassViewModel = new SelectClassViewModel
        {
          //  Etat = _etat,
            banque_op = await _context.CaBanques.Where(b => b.id_banque == idBanque).FirstAsync(),
            lesClassesOperations = await _context.ClassOperations.ToListAsync(),
            lesSousClassOperations = await _context.SousClassOps.ToListAsync()
        };
        return View(_SelectClassViewModel);
    }
}

