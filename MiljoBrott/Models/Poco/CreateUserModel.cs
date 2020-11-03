using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class CreateUserModel
	{
		[Required(ErrorMessage = "Fyll i användarnamn")]
		[Display(Name = "Användarnamn:")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Fyll i löserord")]
		[Display(Name = "Lösenord:")]
		[UIHint("password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Fyll i löserord")]
		[Display(Name = "Repetera Lösenord:")]
		[UIHint("password")]
		public string RepeatPassword { get; set; }

	}
}
