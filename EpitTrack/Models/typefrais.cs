using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpitTrack.Models
{
    [Table("db_typefrais", Schema = "public")]
    public class typefrais
    {
        [Key]
        [Required]
        public int id_typefrais { get; set; }

        [DisplayName("Type frais")]
        public string type_frais { get; set; }

        [DisplayName("commentaire")]
        public string comment_typefrais { get; set; }
        
    }
}

