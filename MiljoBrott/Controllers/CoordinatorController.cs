using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Infrastructure;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class CoordinatorController : Controller
	{
		private IEnvironmentalRepository repository;

		public CoordinatorController(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public ViewResult CrimeCoordinator(int id)
		{
			ViewBag.Worker = "Coordinator";
			ViewBag.ID = id;
			return View(repository.Departments);
		}

		public ViewResult ReportCrime()
		{
			ViewBag.Worker = "Coordinator";
			var errand = HttpContext.Session.GetJson<Errand>("ErrandCreation");
			if (errand == null)
				return View();
			else
				return View(errand);
		}

		public ViewResult StartCoordinator()
		{
			ViewBag.Worker = "Coordinator";
			return View(repository);
		}

		public ViewResult Thanks()
		{
			ViewBag.Worker = "Coordinator";
			HttpContext.Session.Remove("ErrandCreation");
			return View();
		}

		public ViewResult Validate(Errand errand)
		{
			ViewBag.Worker = "Coordinator";
			HttpContext.Session.SetJson("ErrandCreation", errand);
			return View(errand);
		}
	}
}
