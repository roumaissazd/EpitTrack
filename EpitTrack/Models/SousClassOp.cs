using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EpitTrack.Models
{
    [Table("db_sous_class_op", Schema = "public")]
    public class SousClassOp
    {
            [Key]
            [Required]
            public int id_sous_class_op { get; set; }

             [DisplayName("Id class opération")]
             public int id_class_op { get; set; }

             [DisplayName("Lib class opération")]
             public string lib_class_op { get; set; }

            [DisplayName("Lib sous class opération")]
            public string lib_sous_class_op { get; set; }
  }

}
