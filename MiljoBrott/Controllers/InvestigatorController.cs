using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MiljoBrott.Controllers
{
	public class InvestigatorController : Controller
	{
		public ViewResult CrimeInvestigator()
		{
			return View();
		}
		
		public ViewResult StartInvestigator()
		{
			return View();
		}
	}
}
