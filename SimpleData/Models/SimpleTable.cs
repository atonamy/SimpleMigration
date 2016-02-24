using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleData.Models
{
	[Table("public.simple_table")]
	public class SimpleTable
	{ 
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DataId { get; set; } 
		[MaxLength(200)] 
		public string Content { get; set; } 
	}
}

