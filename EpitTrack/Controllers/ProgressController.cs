using Microsoft.AspNetCore.Mvc;

namespace EpitTrack.Controllers
{
    public class ProgressController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> StartProcessing()
        {
            // Simuler un traitement long
            for (int i = 0; i <= 100; i += 10)
            {
                // Stocker la progression dans une session ou une source de données
                HttpContext.Session.SetInt32("Progress", i);
                await Task.Delay(1000); // Simuler le traitement
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetProgress()
        {
            // Obtenir la progression stockée
            int progress = HttpContext.Session.GetInt32("Progress") ?? 0;
            return Ok(progress);
        }
    }

}
