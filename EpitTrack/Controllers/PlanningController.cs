using EpitTrack.Data;
using EpitTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using Microsoft.CodeAnalysis;
using Google.OrTools.Sat;
using Google.OrTools.ModelBuilder;
using Syncfusion.EJ2.Linq;
using EpitTrack.Services;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Linq.Expressions;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Syncfusion.EJ2.Schedule;
using Syncfusion.EJ2.PivotView;
using Syncfusion.Blazor.Popups;
using EpitTrack.ViewModels;
using Microsoft.Build.Framework;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Google.Protobuf;

using Microsoft.Extensions.Logging;

namespace EpitTrack.Controllers
{
    public class PlanningController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IGeocodageService _geocodageService;
        private readonly ICalculTempsTrajetService _calculTempsTrajetService;
        private readonly IHubContext<ProgressHub> _hubContext;
        private readonly ILogger<PlanningController> _logger;


        // GET: PlanningController
        public PlanningController(AppDbContext context, IGeocodageService geocodageService, ICalculTempsTrajetService calculTempsTrajetService, IHubContext<ProgressHub> hubContext, ILogger<PlanningController> logger)
        {
            _context = context;
            _geocodageService = geocodageService;
            _calculTempsTrajetService = calculTempsTrajetService;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string date_courses, int id_planif)
        {
             var newPlanifViewModel = new PlanificationViewModel();
             var newPlanif = new planif();
             var newPlanification = new List<planification>();
             DateOnly DateDesCourses;

            if (id_planif > 0)
            {
                newPlanif = _context.planifs.Where(p => p.id_planif == id_planif).FirstOrDefault();
                newPlanification = _context.planifications.Where(p => p.id_planif == id_planif).ToList();
                ViewBag.idplanif = newPlanif.id_planif;
                var ExistPlanning = _context.planifs.Where(p => p.date_courses == newPlanif.date_courses).ToList();
                {
                    var selectListPlanning = new SelectList(ExistPlanning.Select(x => new
                    {
                        Value = x.id_planif,
                        Text = $"{x.lib_planif}" // Utilisez ici les propriétés que vous souhaitez afficher dans le texte du SelectList
                    }), "Value", "Text");


                    ViewBag.listplanning = selectListPlanning;
                }

            }
            else
            {
              if (date_courses is null)
                {
                    DateDesCourses = DateOnly.FromDateTime(DateTime.Now);
                }
                else
                {
                    DateDesCourses = DateOnly.ParseExact(date_courses, "yyyy-MM-dd");
                    
                }
                newPlanif.date_courses = DateDesCourses;
                newPlanif.date_planif = DateOnly.FromDateTime(DateTime.Now);
            }
                newPlanifViewModel.leplanif = newPlanif;
                newPlanifViewModel.lesplanifications = newPlanification.OrderBy(p=>p.heur_deb_course).ToList();
               // ViewBag.datetoplanif = newPlanif.date_courses;
                ViewBag.selectedplanifid = newPlanif.id_planif;
            return View(newPlanifViewModel);
        }

        public async Task<IActionResult> ValiderDate(DateTime date_courses)
        {

            var _date_courses = DateOnly.FromDateTime(date_courses);

            var _planif = _context.planifs.Where(p => p.date_courses == _date_courses).OrderByDescending(p => p.id_planif).FirstOrDefault();

            int _id_planif = _planif?.id_planif ?? 0;
            
            return RedirectToAction("Index", "Planning", new {date_courses = date_courses.ToString("yyyy-MM-dd"), id_planif = _id_planif});
            
        }

        // Traiter les données du formulaire
       
        public async Task<IActionResult> NouvellePlanif(DateTime date_courses)
        {
            var _date_courses = DateOnly.FromDateTime(date_courses);
            var _lib_planif = "planif " + DateTime.Now.TimeOfDay.ToString();
            var result = _context.Database.ExecuteSqlRaw("SELECT * FROM prc_create_planif({0}, {1})", _date_courses, _lib_planif);

            var _planif = _context.planifs.Where(p => p.date_courses == _date_courses).OrderByDescending(p => p.id_planif).FirstOrDefault();

            int _id_planif = _planif?.id_planif ?? 0;

            return RedirectToAction("Index", "Planning", new { date_courses = date_courses.ToString("yyyy-MM-dd"), id_planif = _id_planif });

        }


        public async Task<IActionResult> Initialiser_Planif(int id_planif)
        {
            ViewBag.idplanif = id_planif;
            
            bool ResultInit = await Init_Planification(_context, id_planif);
            var maplanif = _context.planifs.Where(p => p.id_planif == id_planif).FirstOrDefault();
            maplanif.val_compt = 0;
            maplanif.pourcent_compt = 100;
            var maplanification = _context.planifications.Where(p => p.id_planif == id_planif).ToList();
            maplanif.max_compt = maplanification.Count();
            _context.SaveChanges();
            var newPlanifViewModel = new PlanificationViewModel();
            newPlanifViewModel.leplanif = maplanif;
            newPlanifViewModel.lesplanifications = maplanification;
            return RedirectToAction("Index", "Planning", new { date_courses = maplanif.date_courses, id_planif = maplanif.id_planif });
        }




        public async Task<bool> Init_Planification(AppDbContext _context, int mon_id_planif)
        {
            var ma_planif = _context.planifs.Where(p => p.id_planif == mon_id_planif).FirstOrDefault();
            var leschauffeurs = await _context.Chauffeurs.ToListAsync();
            var lescourses = await _context.planifications.Where(course => course.id_planif == mon_id_planif).OrderBy(course => course.heur_deb_course).ToListAsync();
            ma_planif.max_compt = lescourses.Count();
            foreach (var chauffeur in leschauffeurs)
            {
                try
                {
                    chauffeur.hdispo_chauff = 0;
                    chauffeur.position_chauff = chauffeur.adr_chauff + " " + chauffeur.cp_chauff + " " + chauffeur.ville_chauff + " " + chauffeur.pays_chauff;
                    chauffeur.etat_chauff = "DOMICILE";
                    var coordonneesPosChauff = await _geocodageService.ObtenirCoordonnees(chauffeur.position_chauff);
                    chauffeur.pos_chauff_lat = coordonneesPosChauff.Item1;
                    chauffeur.pos_chauff_long = coordonneesPosChauff.Item2;
                    _context.Chauffeurs.Update(chauffeur);
                }
                catch
                {

                }
            }

            
            foreach (var lacourse in lescourses)
            {
                try
                {
                    ma_planif.val_compt = ma_planif.val_compt+1;
                    ma_planif.nb_courses_planifiees = 0;
                    _context.planifs.Update(ma_planif);
                    await _context.SaveChangesAsync();

                    lacourse.id_chauff = 0;
                    lacourse.nom_chauff = "";
                    lacourse.prenom_chauff = "";
                    lacourse.ca_ht_chauff = 0;
                    lacourse.tel_chauff = "";
                    lacourse.date_course = lacourse.date_course;

                    var coordonneesAdrDep = await _geocodageService.ObtenirCoordonnees(lacourse.adr_depart);
                    lacourse.adr_dep_lat = coordonneesAdrDep.Item1;
                    lacourse.adr_dep_long = coordonneesAdrDep.Item2;
                    var coordonneesAdrArr = await _geocodageService.ObtenirCoordonnees(lacourse.adr_arrivee);
                    lacourse.adr_arr_lat = coordonneesAdrArr.Item1;
                    lacourse.adr_arr_long = coordonneesAdrArr.Item2;
                    planification courseSynchro = await SynchroWithReference(lacourse);
                    _context.planifications.Update(courseSynchro);
                    await _context.SaveChangesAsync();
                }
                catch
                {

                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        
        
        

        public async Task<bool> Init_Chauffeurs (AppDbContext _context, int last_time_matinee)
        {
            var leschauffeurs = await _context.Chauffeurs.ToListAsync();

            foreach (var chauffeur in leschauffeurs)
            {
                try
                {
                    chauffeur.hdispo_chauff = last_time_matinee;
                    chauffeur.position_chauff = chauffeur.adr_chauff + " " + chauffeur.cp_chauff + " " + chauffeur.ville_chauff + " " + chauffeur.pays_chauff;
                    chauffeur.etat_chauff = "DOMICILE";
                    var coordonneesPosChauff = await _geocodageService.ObtenirCoordonnees(chauffeur.position_chauff);
                    chauffeur.pos_chauff_lat = coordonneesPosChauff.Item1;
                    chauffeur.pos_chauff_long = coordonneesPosChauff.Item2;
                    _context.Chauffeurs.Update(chauffeur);
                }
                catch
                {

                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InitChauffeur(AppDbContext _context, int idChauffeur)
        {
            var NotreChauffeur = await _context.Chauffeurs.Where(ch => ch.id_chauff == idChauffeur).FirstOrDefaultAsync();

               try
                {
                    NotreChauffeur.position_chauff = NotreChauffeur.adr_chauff + " " + NotreChauffeur.cp_chauff + " " + NotreChauffeur.ville_chauff + " " + NotreChauffeur.pays_chauff;
                    
                    var coordonneesPosChauff = await _geocodageService.ObtenirCoordonnees(NotreChauffeur.position_chauff);
                    /*
                    NotreChauffeur.pos_chauff_lat = coordonneesPosChauff.Item1;
                    NotreChauffeur.pos_chauff_long = coordonneesPosChauff.Item2;
                    */
                    NotreChauffeur.hdispo_chauff = NotreChauffeur.hdispo_chauff + await Temps_Trajet(NotreChauffeur.pos_chauff_lat, NotreChauffeur.pos_chauff_long, coordonneesPosChauff.Item1, coordonneesPosChauff.Item2);
                    NotreChauffeur.etat_chauff = "DOMICILE";   
                    _context.Chauffeurs.Update(NotreChauffeur);
                }
                catch
                {

                }
                await _context.SaveChangesAsync();
            return true;
         }


        public async Task<IActionResult> ChargerPlanning(int SelectedPlanifId)
        {
            DateOnly date_courses = _context.planifs.Where(p => p.id_planif == SelectedPlanifId).FirstOrDefault().date_courses;
            
            return RedirectToAction("Index", new { datetoplanif = date_courses.ToString(), id_planif = SelectedPlanifId });
        }


       
        

        public async Task<IActionResult> Planifier(int SelectedPlanifId, int param_tps_attente_chauff, string mode)
        {
            var myplanif = _context.planifs.Where(p => p.id_planif == SelectedPlanifId).FirstOrDefault();
            //myplanif.val_compt = 0;
            
            myplanif.pourcent_compt = 100;
            _context.SaveChanges();
            int cpt = await PlanifierCourses(_context, SelectedPlanifId); // Appel à la méthode de planification asynchrone
            DateOnly date_courses = myplanif.date_courses;
            //myplanif.nb_courses_planifiees = myplanif.nb_courses_planifiees+cpt;
            _context.planifs.Update(myplanif); //
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { datetoplanif = date_courses.ToString(), id_planif = SelectedPlanifId });
            
        }

        
        public async Task<int> PlanifierCourses(AppDbContext _context, int idplanif)
        {

            int nb_courses_planifies = 0;
            int param_tps_attente_chauff = 0;
            var lescourses = new List<planification>();
            bool init_chauff = false;
            bool continuer = true;
            int last_time_matinee = 0;
            int tour = 0;
            var my_planif = await _context.planifs.Where(p => p.id_planif == idplanif).FirstOrDefaultAsync();
            
            param_tps_attente_chauff = my_planif.tps_attente;
            do
            {
               int nb_courses_pl = await planifier_courses(_context, idplanif, param_tps_attente_chauff);
               if (nb_courses_pl ==0 && param_tps_attente_chauff > 180)
                   {
                 
                        continuer = false;
                   }
                   init_chauff = await Init_Chauffeurs(_context, last_time_matinee);
                   param_tps_attente_chauff = 0;
                
                param_tps_attente_chauff = param_tps_attente_chauff + 30;
                my_planif.val_compt  = nb_courses_pl;
                my_planif.pourcent_compt = my_planif.val_compt * 100 / my_planif.max_compt;
                _context.planifs.Update(my_planif);
                await _context.SaveChangesAsync();
                nb_courses_planifies = nb_courses_planifies + nb_courses_pl;
                lescourses = await _context.planifications.Where(course => course.id_chauff == 0 && course.id_planif == idplanif).OrderBy(course => course.heur_deb_course).ToListAsync();
                if (lescourses.Count()==0)
                {
                    continuer = false;
                }
            }
            

            while (continuer == true );

            return nb_courses_planifies;
        }

        public async Task<int> planifier_courses(AppDbContext _context, int idplanif, int param_tps_attente_chauff)
           {
            int nb_courses_pl = 0;
            var my_planif = await _context.planifs.Where(p => p.id_planif == idplanif).FirstOrDefaultAsync();

            var lescourses = await _context.planifications.Where(course => course.id_chauff == 0 && course.id_planif == idplanif).OrderBy(course => course.heur_deb_course).ToListAsync();

            var leschauffeurs = await _context.Chauffeurs.Include(c => c.preplanifs)
                                                         .Include(c => c.Indisponibilites)
                                                         .ToListAsync();


            my_planif.val_compt = my_planif.nb_courses_planifiees;

            for (int i = 0; i < lescourses.Count; i++)
                {
                var lacourse = lescourses[i];
                int meilleurTemps = 1440;
                int heureDebutCourse = lacourse.heur_deb_course;
                int id_Meilleur_Chauff = 0;
                int tps_attente_chauff = 0;
                my_planif.val_compt = my_planif.val_compt + 1;
                my_planif.tps_attente = param_tps_attente_chauff;
                await _context.SaveChangesAsync();

                if (lacourse.id_chauff == 0)
                {
                    /* Si la course est preplanifiée, on enregistre le chauffeur preplanifié et on passe à la suivante  */
                    if (lacourse.id_chauff_p > 0)
                    {
                        var chauff_preplan = await _context.Chauffeurs.FindAsync(lacourse.id_chauff_p);
                        lacourse.id_chauff = lacourse.id_chauff_p;
                        lacourse.nom_chauff = chauff_preplan.nom_chauff;
                        lacourse.prenom_chauff = chauff_preplan.prenom_chauff;
                        chauff_preplan.hdispo_chauff = lacourse.heur_fin_course;
                        chauff_preplan.ca_ht_chauff = chauff_preplan.ca_ht_chauff + lacourse.prix_vente_ht;
                        chauff_preplan.position_chauff = lacourse.adr_arrivee;
                        chauff_preplan.pos_chauff_lat = lacourse.adr_arr_lat;
                        chauff_preplan.pos_chauff_long = lacourse.adr_arr_long;
                        _context.Chauffeurs.Update(chauff_preplan);

                        continue;
                    }
                    else
                    {


                        foreach (var chauffeur in leschauffeurs)
                        {


                            
                            int tempsTrajetChauffeur = 0;
                            int h_arrivee_chauff = 0;
                            bool initchauff = false;
                            // tempsTrajetChauffeur = int.Parse(IntegrationCourse(lacourse, chauffeur).ToString());

                            
                            if (chauffeur.hdispo_chauff > 0 && (lacourse.heur_deb_course - chauffeur.hdispo_chauff > 240))
                            {
                               initchauff = await InitChauffeur(_context, chauffeur.id_chauff);
                            }
                            
                            tempsTrajetChauffeur = await IntegrationCourse(lacourse, chauffeur);

                           

                            if (tempsTrajetChauffeur > 0)
                            {
                                h_arrivee_chauff = int.Parse(chauffeur.hdispo_chauff.ToString()) + int.Parse(tempsTrajetChauffeur.ToString());
                                if (h_arrivee_chauff < heureDebutCourse)
                                {
                                    tps_attente_chauff = heureDebutCourse - h_arrivee_chauff;
                                    //if ((chauffeur.hdispo_chauff == 0) || ((tps_attente_chauff < param_tps_attente_chauff) && (chauffeur.hdispo_chauff > 0)))
                                    
                                    if ((chauffeur.etat_chauff=="DOMICILE") || ((tps_attente_chauff < param_tps_attente_chauff) && (chauffeur.etat_chauff=="COURSE")))
                                        {
                                        if (h_arrivee_chauff < meilleurTemps)
                                        {
                                            meilleurTemps = h_arrivee_chauff;
                                            id_Meilleur_Chauff = chauffeur.id_chauff;
                                        }
                                    }
                                }
                            }
                        }


                        // mise à jour du chauffeur
                        if (id_Meilleur_Chauff > 0)
                        {
                            var Meilleur_chauffeur = await _context.Chauffeurs.FindAsync(id_Meilleur_Chauff);
                            Meilleur_chauffeur.hdispo_chauff = lacourse.heur_fin_course;
                            Meilleur_chauffeur.etat_chauff = "COURSE";
                            Meilleur_chauffeur.ca_ht_chauff = Meilleur_chauffeur.ca_ht_chauff + lacourse.prix_vente_ht;
                            Meilleur_chauffeur.position_chauff = lacourse.adr_arrivee;
                            Meilleur_chauffeur.pos_chauff_lat = lacourse.adr_arr_lat;
                            Meilleur_chauffeur.pos_chauff_long = lacourse.adr_arr_long;
                            _context.Chauffeurs.Update(Meilleur_chauffeur);
                            // mise à jour de la course 
                            lacourse.id_chauff = decimal.Parse(Meilleur_chauffeur.id_chauff.ToString());
                            lacourse.nom_chauff = Meilleur_chauffeur.nom_chauff;
                            lacourse.ca_ht_chauff = Meilleur_chauffeur.ca_ht_chauff;
                            lacourse.prenom_chauff = Meilleur_chauffeur.prenom_chauff;
                            nb_courses_pl = my_planif.nb_courses_planifiees + 1;
                            _context.planifications.Update(lacourse); //
                            my_planif.nb_courses_planifiees = nb_courses_pl;
                            my_planif.tps_attente = param_tps_attente_chauff;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                
            }
            return nb_courses_pl;
        }


        public async Task<planification> SynchroWithReference(planification courseToSynchro)
        {
            CourseDeReference notre_course = await _context.CoursesDeReferences.Where(c => (c.adr_dep_lat_ref == courseToSynchro.adr_dep_lat)
                                                                    && (c.adr_dep_long_ref == courseToSynchro.adr_dep_long)
                                                                    && (c.adr_arr_lat_ref == courseToSynchro.adr_arr_lat)
                                                                    && (c.adr_arr_long_ref == courseToSynchro.adr_arr_long)).FirstAsync();
            if (notre_course != null)
            {
                courseToSynchro.heur_moy_fin_course = notre_course.moy_h_fin_service_ref;
                courseToSynchro.km_moy_course = notre_course.moy_km_course_ref;
                courseToSynchro.id_chauff1 = notre_course.id_chauff_1_ref;
                courseToSynchro.id_chauff2 = notre_course.id_chauff_2_ref;
                courseToSynchro.id_chauff3 = notre_course.id_chauff_3_ref;
            }
            return courseToSynchro;

        }


       

       

        public DateTime? DefaultDate(DateTime? date_a_tester)
        {
            if (date_a_tester == null)
            {
                return new DateTime().ToUniversalTime();
            }
            else return DateTime.Parse(date_a_tester.ToString()).ToUniversalTime();
        }
        public async Task<int> TempsTrajet(string adresseDepart, string adresseArrivee)
        {
            var coordonneesDepart = await _geocodageService.ObtenirCoordonnees(adresseDepart);
            var coordonneesArrivee = await _geocodageService.ObtenirCoordonnees(adresseArrivee);

            string departLatitude = coordonneesDepart.Item1;
            string departLongitude = coordonneesDepart.Item2;
            string arriveeLatitude = coordonneesArrivee.Item1;
            string arriveeLongitude = coordonneesArrivee.Item2;

            int tempsTrajet = await _calculTempsTrajetService.ObtenirTempsTrajetEntreDeuxPoints(departLatitude, departLongitude, arriveeLatitude, arriveeLongitude);
            tempsTrajet = (int)(tempsTrajet / 60 + 1);
            return tempsTrajet;
        }

        public async Task<int> Temps_Trajet(string departLatitude, string departLongitude, string arriveeLatitude, string arriveeLongitude)
        {

            int tempsTrajet = await _calculTempsTrajetService.ObtenirTempsTrajetEntreDeuxPoints(departLatitude, departLongitude, arriveeLatitude, arriveeLongitude);
            tempsTrajet = (int)(tempsTrajet / 60 + 1);
            return tempsTrajet;
        }

        public async Task<int> IntegrationCourse(planification notre_course, Chauffeur notre_chauffeur)
        {
            int tps_integration = 0;
            int tempsTrajetChauffeur = 0;
            bool c_possible = true;
           

           

            tps_integration = await Temps_Trajet(notre_chauffeur.pos_chauff_lat, notre_chauffeur.pos_chauff_long, notre_course.adr_dep_lat, notre_course.adr_dep_long);

            if (notre_chauffeur.Indisponibilites.Count() == 0 && notre_chauffeur.preplanifs.Count() == 0)
            {
                return tps_integration;
            }
            else
            {
                if (notre_chauffeur.Indisponibilites.Count() > 0)
                {
                    // la date de la course à preplanifier est dans la période d'absence de chauffeur 
                    var indispos_blocants = notre_chauffeur.Indisponibilites.Where(indisp => (indisp.date_deb_indispo_chauff < notre_course.date_course && notre_course.date_course < indisp.date_reprise)
                                                                                          || (indisp.date_deb_indispo_chauff == notre_course.date_course && indisp.jour_complet == true)
                                                                                          || (notre_course.date_course==indisp.date_reprise && notre_course.heur_deb_course<indisp.num_heure_reprise)
                                                                                          || (indisp.date_deb_indispo_chauff == notre_course.date_course && indisp.num_deb_indispo_chauff  < notre_course.heur_fin_course)
                                                                                             ).ToList();
                    if (indispos_blocants.Count() == 0)
                    {
                        if (notre_chauffeur.preplanifs.Count() > 0)
                        {
                            // Il y'a des courses preplanifiés à la même date et aux mêmes horaire que la course à preplanifer
                            var preplan_blocants = notre_chauffeur.preplanifs.Where(preplan => (preplan.date_course == notre_course.date_course)
                                                                                   && (
                                                                                        ((notre_course.heur_deb_course >= preplan.hnum_deb_course) && (notre_course.heur_deb_course <= preplan.hnum_fin_course))
                                                                                     || ((notre_course.heur_fin_course >= preplan.hnum_deb_course) && (notre_course.heur_fin_course <= preplan.hnum_fin_course))
                                                                                     )).ToList();

                            if (preplan_blocants.Count() == 0)
                            {
                                var preplan_chauff = notre_chauffeur.preplanifs.Where(preplan => (preplan.date_course == notre_course.date_course))
                                                                                                .OrderBy(preplan => preplan.hnum_deb_course)
                                                                                                .ToList();
                                foreach (var preplan in preplan_chauff)
                                {
                                    if (notre_course.heur_fin_course > preplan.hnum_deb_course)  // la course à planifier est avant la course préplanifié
                                    {

                                        int temps_trajet_vers_preplanfie = await Temps_Trajet(notre_course.adr_arr_lat, notre_course.adr_arr_long, preplan.adr_dep_lat, preplan.adr_dep_long);

                                        if (notre_course.heur_fin_course + temps_trajet_vers_preplanfie > preplan.hnum_deb_course)
                                        {
                                            c_possible = false;
                                            break;  // le chauffeur arrive trop tard à la course preplanifié
                                        }
                                    }
                                    else
                                    {
                                        //la course à planifier est aprés la course préplanifié
                                        tempsTrajetChauffeur = await Temps_Trajet(preplan.adr_arr_lat, preplan.adr_arr_long, notre_course.adr_dep_lat, notre_course.adr_dep_long);

                                        if (preplan.hnum_fin_course + tempsTrajetChauffeur > notre_course.heur_deb_course)
                                        {
                                            c_possible = false;
                                            break;  // le chauffeur arrive trop tard à la course à planifier
                                        }
                                    }

                                }
                                if (c_possible = true)
                                {
                                    tps_integration = tempsTrajetChauffeur;
                                }
                            }
                        }
                    }
                    else
                    {
                        c_possible = false;
                    }
                }
                if (c_possible == false)
                {
                    tps_integration = 0;
                }
                return tps_integration;
            }
        }

        /*
        public async Task<IActionResult> ValiderDate(DateTime datetoplanif)
        {


            return RedirectToAction("Index", "Planning", new { datetoplanif = datetoplanif.ToString("yyyy-MM-dd") });
        }
        */
        


        public async Task<IActionResult> Actualiser()
        {

            //return RedirectToAction("Index", _context.Courses.ToList());
            return RedirectToAction("Index", "Planning", new { datetoplanif = DateTime.Now.ToString() });
        }


        [HttpPost]
        public async Task<IActionResult> Reshresh(int id)
        {
            // Logique pour mettre à jour l'élément
            var mescourses = await _context.Courses.ToListAsync();
            Course course_updated = mescourses.FirstOrDefault(course => course.id_course == id);

            // Renvoyer une réponse, par exemple, l'élément mis à jour ou un statut
            return Json(new { success = true, updatedItem = course_updated });
        }

        [HttpPost]
        public async Task<IActionResult> Affecter(int id)
        {
            // Logique pour mettre à jour l'élément

            var leschauffeurs = await _context.Chauffeurs.ToListAsync();
            return Json(new { success = true, leschauffeurs });
        }


        [HttpPost]
        public async Task<IActionResult> Affect_chauff(int idch, int idc)
        {

            try
            {
                var macourse = await _context.planifications.FindAsync(idc);
                var mon_chauff = await _context.Chauffeurs.Include(c => c.preplanifs)
                                                          .Include(c => c.Indisponibilites)
                                                          .FirstOrDefaultAsync(c => c.id_chauff == idch);
                int tpstrajet = await IntegrationCourse(macourse, mon_chauff);

                if (tpstrajet > 0)
                {
                    macourse.id_chauff = idch;
                    macourse.nom_chauff = mon_chauff.nom_chauff;
                    macourse.prenom_chauff = mon_chauff.prenom_chauff;
                    _context.planifications.Update(macourse); //
                    macourse.date_course = macourse.date_course;

                    /* supprimer l'ancienne preplanification */
                    var existpreplanif = _context.preplanifs.FirstOrDefault(p => p.id_course == macourse.id_course);
                    if (existpreplanif != null)
                    {
                        _context.preplanifs.Remove(existpreplanif);
                    }
                    /* ajout preplanif */
                    preplanif preplanification = new preplanif();
                    preplanification.id_chauff = mon_chauff.id_chauff;
                    preplanification.id_course = macourse.id_course;
                    preplanification.date_course = macourse.date_course;
                    preplanification.hnum_deb_course = macourse.heur_deb_course;
                    preplanification.hnum_fin_course = macourse.heur_fin_course;
                    preplanification.adr_dep_lat = macourse.adr_dep_lat;
                    preplanification.adr_dep_long = macourse.adr_dep_long;
                    preplanification.adr_arr_lat = macourse.adr_arr_lat;
                    preplanification.adr_arr_long = macourse.adr_arr_long;
                    mon_chauff.preplanifs.Add(preplanification);
                    _context.Chauffeurs.Update(mon_chauff); //
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Les modifications ont été enregistrées avec succès");
                    return Json(new { success = true, updatedItem = macourse });
                }
                return Json(new { success = false, message = mon_chauff.prenom_chauff + " " + mon_chauff.nom_chauff + " n'est pas disponible" });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enregistrement des modifications");
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Supp_Affect(int idch, int idc)
        {

            try
            {
                var mon_chauff = await _context.Chauffeurs.Include(c => c.preplanifs)
                                                          .Include(c => c.Indisponibilites)
                                                          .FirstOrDefaultAsync(c => c.id_chauff == idch);

                var existpreplanif = _context.preplanifs.FirstOrDefault(p => p.id_course == idc && p.id_chauff == idch);
                if (existpreplanif != null)
                {
                    _context.preplanifs.Remove(existpreplanif);
                }
                _context.Chauffeurs.Update(mon_chauff); //
                await _context.SaveChangesAsync();
                var leschauffeurs = await _context.Chauffeurs.ToListAsync();
                return Json(new { success = true, leschauffeurs });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enregistrement des modifications");
                return Json(new { success = false, message = ex.Message });
            }

        }

        public async Task<IActionResult> visualiser_planning(int id_planif)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("SELECT prc_add_planning({0})", id_planif);
            var plannings = await _context.Plannings.Where(pg => pg.id_planif == id_planif).ToListAsync();

            return View(plannings);
        }

        public IActionResult ChartData(int id_planif)
        {
            var list_courses = _context.Plannings.Where(pg=>pg.id_planif==id_planif && pg.id_chauff>0).OrderBy(pg=>pg.id_chauff).ToList();
            var datecourse = _context.planifs.Where(p => p.id_planif == id_planif).FirstOrDefault().date_courses;
            DateTime dateTimeAvecHeure = new DateTime(datecourse.Year, datecourse.Month, datecourse.Day, 0, 0, 0);
            var list_chauff = _context.Chauffeurs.ToList();
            var chauffeurs = new List<string>();
            var courses = new List<object>();
            int i = 0;
            decimal _id_chauff = 0;
           
            foreach (var course in list_courses)
            {
                if (course.id_chauff != _id_chauff)
                {
                    _id_chauff = course.id_chauff;
                    chauffeurs.Add(course.nom_chauff + " " + course.prenom_chauff + "(" + course.total_ca_ht + ")");
                    i = i + 1;
                }
                int heure_dep = course.heur_deb_course / 60;
                heure_dep = heure_dep + 1; //heure de paris
                int duration_minutes = (course.heur_moy_fin_course == 0 ? course.heur_fin_course : course.heur_moy_fin_course) - course.heur_deb_course;
                

                var obj = new { Name = course.nom_passagers +"("+course.id_course+")", StartHour = heure_dep, DurationMinutes = duration_minutes, ChauffeurIndex = i-1 };
                courses.Add(obj);
               
            }

            var chartData = new { StartDate = dateTimeAvecHeure, Chauffeurs = chauffeurs, Courses = courses };
            
            return Json(chartData);
        }

        
        public async Task<IActionResult> RafraichirPlanification(int id_planif)
        {
            ViewBag.idplanif = id_planif;
            var _PlanificationViewModel = new PlanificationViewModel();
            var _planif = new planif();
                _planif.date_planif = DateOnly.FromDateTime(DateTime.Now);
            var _planifications = new List<planification>();

            _planif.pourcent_compt = _planif.val_compt * 100 / _planif.max_compt;
            if (id_planif>0)
            {
             ViewBag.idplanif = id_planif;
            _planif = await _context.planifs.FirstOrDefaultAsync(p => p.id_planif == id_planif);
            _planifications = await _context.planifications.Where(p => p.id_planif == id_planif).ToListAsync();
            }
            _PlanificationViewModel.leplanif = _planif;
            _PlanificationViewModel.lesplanifications = _planifications;
            return PartialView("Planification", _PlanificationViewModel);
        }
        

        public async Task<IActionResult> RefreshPlanification(int id_planif)
        {
            var _PlanificationViewModel = new PlanificationViewModel();
            var _planif = await _context.planifs.FirstOrDefaultAsync(p => p.id_planif == id_planif);
            var _planifications = await _context.planifications.Where(p => p.id_planif == id_planif).ToListAsync();
            if(_planif is not null)
            { 
            _planif.pourcent_compt = _planif.val_compt * 100 / _planif.max_compt;
            _PlanificationViewModel.leplanif = _planif;
            _PlanificationViewModel.lesplanifications = _planifications;
            }
            return PartialView("Planification", _PlanificationViewModel);
        }





        [HttpPost]
        public IActionResult Importer(IFormFile file)
        {
            /*
            Planning _planning = new Planning();
            if (file.Length > 0)
            {
                if (_planning.ImportPlanning(file, _context))
                {

                };

            }
            
            */
            Course _course = new Course();

            if (file is not null && file.Length > 0)
            {
                if (_course.ImportCourses(file, _context))
                {
                    TempData["Message"] = "Le fichier "+ file.FileName + " a été importé avec succés";
                }
            }
             
            
            return RedirectToAction("Index", "Planning");
            
        }

        // GET: PlanningController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanningController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanningController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlanningController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanningController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlanningController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanningController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public class ProcResult
        {
            public int insert_id { get; set; } // Assurez-vous que le nom de la propriété correspond à celui retourné par la procédure
        }
        public class IntReturn
        {
            public int Value { get; set; }
        }
    }
}
