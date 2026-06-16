using EpitTrack.Models;

namespace EpitTrack.ViewModels
{
    public class PlanificationViewModel
    {
        public planif leplanif { get; set; }
        public IEnumerable<planification> lesplanifications { get; set; }
        
    }
}
