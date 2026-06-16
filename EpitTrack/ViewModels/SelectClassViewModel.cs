using EpitTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EpitTrack.ViewModels
{
    public class SelectClassViewModel
    {
        //public string Etat { get; set; }
        public BanqueCa banque_op { get; set; }
        public IEnumerable<ClassOperation> lesClassesOperations { get; set; }

        public IEnumerable<SousClassOp> lesSousClassOperations { get; set; }
    }
}
