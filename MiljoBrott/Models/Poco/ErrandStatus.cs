using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class ErrandStatus
	{
		[Key]
		public string StatusId { get; set; }
		public string StatusName { get; set; }
	}
}
