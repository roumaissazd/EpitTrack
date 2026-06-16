using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpitTrack.Models;
using EpitTrack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace EpitTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        /*
        public async Task<IActionResult> OLDIndex()
        {
                // cout RH

           List<coutrh> lesCoutRhs = await _context.CoutRhs.ToListAsync();

            var CoutRhParMois = lesCoutRhs
           .GroupBy(t => new { Mois = t.date_deb_coutrh.Month, Annee = t.date_deb_coutrh.Year })
           .Select(g => new
            {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_cout_paie + t.mt_frais)
            })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList(); 
            
         
            // Cout Leasing

            List<contratloa> lescontratloa = await _context.ContratsLoa.ToListAsync();

            var CoutLeasingMois = lescontratloa.GroupBy(t => new { Mois = t.date_deb_contratloa.Month, Annee = t.date_deb_contratloa.Year })
           .Select(g => new
            {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_mois_ttc_contratloa)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();

            // Cout carburant

            List<consocarbur> lesconsocarbur = await _context.ConsosCarbur.ToListAsync();
            
            var CoutCarburantMois = lesconsocarbur.GroupBy(t => new { Mois = t.date_deb_consocarbur.Month, Annee = t.date_deb_consocarbur.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_ttc_consocarbur)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();


            // Les frais
            List<frais> lesfrais = await _context.LesFrais.ToListAsync();

            var LesFraisMois = lesfrais.GroupBy(t => new { Mois = t.date_deb_frais.Month, Annee = t.date_deb_frais.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_frais_ttc)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();

            List<dashboard> lesdashbords = new List<dashboard>();
            var annee_encours = DateTime.Today.Year;
            for (int i = 1; i < 13; i++)
            {
                dashboard thedashbord = new();

                thedashbord.mois_cout= new DateTime(annee_encours, i,1).ToString("MMMM", CultureInfo.CurrentCulture);
                

                if  ( LesFraisMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                    
                {
                   
                    thedashbord.mt_frais = LesFraisMois.Where(cout => cout.Mois == i && cout.Annee== annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.mt_frais = 0;
                }

                if (CoutRhParMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_rh = CoutRhParMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_rh = 0;
                }

                if (CoutCarburantMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_carburant = CoutCarburantMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_carburant = 0;
                }

                if (CoutLeasingMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_leasing = CoutLeasingMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours ).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_leasing = 0;
                }

                lesdashbords.Add(thedashbord);
             }

            var LesCoutsAnnuels = lesdashbords.GroupBy(t => new { Annee = t.annee_cout })
               .Select(g => new
               {
                   Annee = g.Key.Annee,
                   SumFrais = g.Sum(t => t.mt_frais),
                   SumRh = g.Sum(t => t.cout_rh),
                   SumLease = g.Sum(t => t.cout_leasing),
                   SumCarbur = g.Sum(t => t.cout_carburant)
               })
               .OrderBy(g => g.Annee)
               .ToList();

            for (int i = 0; i < LesCoutsAnnuels.Count(); i++)
            {
                dashboard ledashbord = new dashboard();
                ledashbord.mois_cout = "TOTAL";
                ledashbord.annee_cout= LesCoutsAnnuels[i].Annee;
                ledashbord.mt_frais  = LesCoutsAnnuels[i].SumFrais;
                ledashbord.cout_rh = LesCoutsAnnuels[i].SumRh;
                ledashbord.cout_leasing = LesCoutsAnnuels[i].SumLease;
                ledashbord.cout_carburant = LesCoutsAnnuels[i].SumCarbur;
                lesdashbords.Add(ledashbord);
            }


            return View(lesdashbords);
        }
        */
        public async Task<IActionResult> Index()
        {
            // cout RH

            List<BanqueCa> lesCoutRhs = await _context.CaBanques.Where(b => b.lib_class_op == "RH").ToListAsync();
              

            var CoutRhParMois = lesCoutRhs
           .GroupBy(t => new { Mois = t.date_banque.Month, Annee = t.date_banque.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_debit)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();


            // Cout Leasing

            //List<contratloa> lescontratloa = await _context.ContratsLoa.ToListAsync();

            List<BanqueCa> lescontratloa = await _context.CaBanques.Where(b => b.lib_class_op == "LEASING").ToListAsync();

            var CoutLeasingMois = lescontratloa.GroupBy(t => new { Mois = t.date_banque.Month, Annee = t.date_banque.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_debit)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();

            // Cout carburant

           // List<consocarbur> lesconsocarbur = await _context.ConsosCarbur.ToListAsync();

            List<BanqueCa> lesconsocarbur = await _context.CaBanques.Where(b => b.lib_class_op == "CARBURANT").ToListAsync();
            var CoutCarburantMois = lesconsocarbur.GroupBy(t => new { Mois = t.date_banque.Month, Annee = t.date_banque.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_debit)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();


            // Les frais
            List<frais> lesfrais = await _context.LesFrais.ToListAsync();

            var LesFraisMois = lesfrais.GroupBy(t => new { Mois = t.date_deb_frais.Month, Annee = t.date_deb_frais.Year })
           .Select(g => new
           {
               Mois = g.Key.Mois,
               Annee = g.Key.Annee,
               Somme = g.Sum(t => t.mt_frais_ttc)
           })
           .OrderBy(g => g.Annee)
           .ThenBy(g => g.Mois)
           .ToList();

            List<dashboard> lesdashbords = new List<dashboard>();

            //var annee_encours = DateTime.Today.Year;
            var annee_encours = 2023;



            for (int i = 1; i < 13; i++)
            {
                dashboard thedashbord = new();

                thedashbord.mois_cout = new DateTime(annee_encours, i, 1).ToString("MMMM", CultureInfo.CurrentCulture);


                if (LesFraisMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())

                {

                    thedashbord.mt_frais = LesFraisMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.mt_frais = 0;
                }

                if (CoutRhParMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_rh = CoutRhParMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_rh = 0;
                }

                if (CoutCarburantMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_carburant = CoutCarburantMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_carburant = 0;
                }

                if (CoutLeasingMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().Any())
                {
                    thedashbord.cout_leasing = CoutLeasingMois.Where(cout => cout.Mois == i && cout.Annee == annee_encours).ToList().FirstOrDefault().Somme;
                }
                else
                {
                    thedashbord.cout_leasing = 0;
                }

                lesdashbords.Add(thedashbord);
            }

            var LesCoutsAnnuels = lesdashbords.GroupBy(t => new { Annee = t.annee_cout })
               .Select(g => new
               {
                   Annee = g.Key.Annee,
                   SumFrais = g.Sum(t => t.mt_frais),
                   SumRh = g.Sum(t => t.cout_rh),
                   SumLease = g.Sum(t => t.cout_leasing),
                   SumCarbur = g.Sum(t => t.cout_carburant)
               })
               .OrderBy(g => g.Annee)
               .ToList();

            for (int i = 0; i < LesCoutsAnnuels.Count(); i++)
            {
                dashboard ledashbord = new dashboard();
                ledashbord.mois_cout = "TOTAL";
                ledashbord.annee_cout = LesCoutsAnnuels[i].Annee;
                ledashbord.mt_frais = LesCoutsAnnuels[i].SumFrais;
                ledashbord.cout_rh = LesCoutsAnnuels[i].SumRh;
                ledashbord.cout_leasing = LesCoutsAnnuels[i].SumLease;
                ledashbord.cout_carburant = LesCoutsAnnuels[i].SumCarbur;
                lesdashbords.Add(ledashbord);
            }


            return View(lesdashbords);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}