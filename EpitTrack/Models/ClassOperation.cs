using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EpitTrack.Models
{
    [Table("db_class_op", Schema = "public")]
    public class ClassOperation
    {
            [Key]
            [Required]
            public int id_class_op { get; set; }

            [DisplayName("Lib class opération")]
            public string lib_class_op { get; set; }

        }
}
