using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EpitTrack.Models
{

	[Table("db_banque_ca", Schema = "public")]
	public class BanqueCa
	{
		[Key]
		[Required]
		public int id_banque { get; set; }

		[DisplayName("date banque")]
		public DateOnly date_banque { get; set; }

		[DisplayName("date valeur")]
		public DateOnly date_valeur { get; set; }

		[DisplayName("Opération")]
		public string lib_operation { get; set; }

		[DisplayName("débit")]

		public decimal mt_debit { get; set; }

		[DisplayName("crédit")]
		public decimal mt_credit { get; set; }

        [DisplayName("ID class opération")]
        public int id_class_op { get; set; }

        [DisplayName("Lib class opération")]
        public string lib_class_op { get; set; } = " "; 

        [DisplayName("ID Sous class opération")]
        public int id_sous_class_op { get; set; }

        [DisplayName("Lib sous class opération")]
        public string lib_sous_class_op { get; set; } = " ";

    }
}
