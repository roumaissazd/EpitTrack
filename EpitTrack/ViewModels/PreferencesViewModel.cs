using EpitTrack.Models;
namespace EpitTrack.ViewModels
{
    public class PreferencesViewModel
    {
        public IEnumerable<CourseDeReference> LesCoursesDeReferences { get; set; }
        public IEnumerable<Chauffeur> lesChauffeurs { get; set; }
    }
}
