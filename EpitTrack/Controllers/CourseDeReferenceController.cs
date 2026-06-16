using EpitTrack.Data;
using EpitTrack.Models;
using EpitTrack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EpitTrack.Controllers
{
    public class CourseDeReferenceController : Controller
    {
        private readonly AppDbContext _context;

        public CourseDeReferenceController(AppDbContext context)
        {
            _context = context;
        }
        // GET: CourseDeReferenceController
        public ActionResult Index()
        {
            PreferencesViewModel preferencesViewModel = new PreferencesViewModel();
            preferencesViewModel.lesChauffeurs = _context.Chauffeurs.ToList();
            preferencesViewModel.LesCoursesDeReferences = _context.CoursesDeReferences.ToList().OrderBy(c=>c.id_course_ref);
            return View(preferencesViewModel);
        }

        public ActionResult Actu_references()
        {
            
            
            
            _context.Database.ExecuteSqlRawAsync("SELECT prc_maj_references()");


            return RedirectToAction(nameof(Index));
        }
        public async Task<bool> Affect_Preferences(int order_chauff, int id_course, int id_chauff)
        {

           
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("SELECT prc_affect_preferences({0},{1},{2})", order_chauff, id_course, id_chauff);
                    _context.SaveChanges();
                }
                catch
                {

                }
           
            return true;
        }
        public async Task<bool> Annuler_affect(int order_chauff, int id_course)
        {

            
                try
                {
                    await _context.Database.ExecuteSqlRawAsync("SELECT prc_annul_affect({0},{1})", order_chauff, id_course);
                    _context.SaveChanges();
                }
                catch
                {

                }
            
            return true;
        }

               
        




        public async Task Actualiser_ref()
        {

            var mes_courses = _context.Courses.Where(c => c.adr_dep_lat != null && c.adr_dep_long != null && c.adr_arr_lat != null && c.adr_arr_long != null).ToList();

            var lesCoursesCalMoy = _context.Courses.Where(c => c.adr_dep_lat != null && c.adr_dep_long != null && c.adr_arr_lat != null && c.adr_arr_long != null)
                                              .GroupBy(c => new { c.adr_dep_lat, c.adr_dep_long, c.adr_arr_lat, c.adr_arr_long })
                                              .Select(g => new
                                              {
                                                  adr_dep_lat = g.Key.adr_dep_lat,
                                                  adr_dep_long = g.Key.adr_dep_long,
                                                  adr_arr_lat = g.Key.adr_arr_lat,
                                                  adr_arr_long = g.Key.adr_arr_long,
                                                  moy_heure_fin = g.Average(c => c.h_fin_service),
                                                  moy_km_course = g.Average(c => (c.km_fin - c.km_deb)),
                                              }); ;

            var les_coursesrDejaReferences = _context.CoursesDeReferences.ToList();

            foreach (var ma_course in mes_courses)
            {
                var course_avec_moy = lesCoursesCalMoy.Where(c => c.adr_arr_lat == ma_course.adr_arr_lat && c.adr_dep_lat == ma_course.adr_dep_lat && c.adr_arr_lat == ma_course.adr_arr_lat && c.adr_arr_long == ma_course.adr_arr_long).FirstOrDefault();

                var c_ref = les_coursesrDejaReferences.Where(cf => cf.adr_dep_long_ref == course_avec_moy.adr_dep_long && cf.adr_dep_lat_ref == course_avec_moy.adr_dep_lat && cf.adr_arr_long_ref == course_avec_moy.adr_arr_long && cf.adr_arr_long_ref == course_avec_moy.adr_arr_long).FirstOrDefault();
                {
                    if (c_ref == null)
                    {
                        CourseDeReference courseDeRef = new CourseDeReference();
                        courseDeRef.nom_passagers_ref = ma_course.nom_passagers;
                        courseDeRef.heur_deb_course_ref = ma_course.heur_deb_course;
                        courseDeRef.heur_fin_course_ref = ma_course.heur_fin_course;
                        courseDeRef.moy_h_fin_service_ref = (int)course_avec_moy.moy_heure_fin;
                        courseDeRef.adr_depart_ref = ma_course.adr_depart;
                        courseDeRef.adr_arrivee_ref = ma_course.adr_arrivee;
                        courseDeRef.adr_dep_lat_ref = ma_course.adr_dep_lat;
                        courseDeRef.adr_dep_long_ref = ma_course.adr_dep_long;
                        courseDeRef.adr_arr_lat_ref = ma_course.adr_arr_lat;
                        courseDeRef.adr_arr_long_ref = ma_course.adr_arr_long;
                        courseDeRef.moy_km_course_ref = (int)course_avec_moy.moy_km_course;
                        courseDeRef.nom_chauff_1_ref = "";
                        courseDeRef.prenom_chauff_1_ref = "";
                        courseDeRef.nom_chauff_2_ref = "";
                        courseDeRef.prenom_chauff_2_ref = "";
                        courseDeRef.nom_chauff_3_ref = "";
                        courseDeRef.prenom_chauff_3_ref = "";
                        _context.CoursesDeReferences.Add(courseDeRef);
                    }
                    else
                    {
                        c_ref.moy_h_fin_service_ref = (int)course_avec_moy.moy_heure_fin;
                        c_ref.moy_km_course_ref = (int)course_avec_moy.moy_km_course;
                        _context.CoursesDeReferences.Update(c_ref);
                    }
                    await _context.SaveChangesAsync();

                }

                RedirectToAction("Index");
            }
        }


        // GET: CourseDeReferenceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CourseDeReferenceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseDeReferenceController/Create
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

        // GET: CourseDeReferenceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CourseDeReferenceController/Edit/5
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

        // GET: CourseDeReferenceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CourseDeReferenceController/Delete/5
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
    }
}
