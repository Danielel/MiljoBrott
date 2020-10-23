using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Fyll i användarnamn")]
		[Display(Name = "Användarnamn:")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Fyll i löserord")]
		[Display(Name = "Lösenord:")]
		[UIHint("password")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
