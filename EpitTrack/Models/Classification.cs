using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_classification", Schema = "public")]
    public class Classification
    {
        [Key]
        [Required]
        public int id_classification { get; set; }

        [DisplayName("Lib to class")]
        public string lib_to_class { get; set; } = " ";

        [DisplayName("ID class opération")] 
        public int id_class_op { get; set; }

        [DisplayName("Lib class opération")]
        public string lib_class_op { get; set; } = " ";

        [DisplayName("ID Sous class opération")]
        public int id_sous_class_op { get; set; }

        [DisplayName("Lib sous class opération")]
        public string lib_sous_class_op { get; set; } = " ";
        public ClassOperation ClassOp { get; set; }
        public SousClassOp SousClassOp { get; set; }
    }
}
