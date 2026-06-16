using EpitTrack.Models;

namespace EpitTrack.ViewModels
{
    public class IndisponibiliteViewModel
    {
        public Chauffeur notre_chauffeur { get; set; }
        public IEnumerable<Indisponibilite> IndisponibilitesExistantes { get; set; }
        public IEnumerable<preplanif> PreplanifExistantes { get; set; }
        public Indisponibilite NouvelleIndisponibilite { get; set; }

        public preplanif NouvellePreplanif { get; set; }
        // Ajoutez d'autres propriétés si nécessaire, comme une liste de motifs
    }
}
