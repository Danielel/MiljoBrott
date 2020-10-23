using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Infrastructure;
using MiljoBrott.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Components
{
	public class ReportCrime : ViewComponent
	{
		private IEnvironmentalRepository repository;

		public ReportCrime(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public IViewComponentResult Invoke()
		{
			Errand errand = HttpContext.Session.GetJson<Errand>("ErrandCreation");
			if (errand == null)
				return View();
			else
				return View(errand);

			//return View(await repository.GetErrand(id));
		}
	}
}
