using EpitTrack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EpitTrack.ViewModels
{
    public class BanquesViewModel
    {
        [DisplayName("Date Deb")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly date_deb { get; set; }

        [DisplayName("Date Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly date_fin { get; set; }
        public IEnumerable<BanqueCa> OperationsBanqueCA { get; set; }
        public IEnumerable<ClassOperation> lesClassesOperations { get; set; }
        public IEnumerable<SousClassOp> lesSousClassesOperations { get; set; }
    }
}