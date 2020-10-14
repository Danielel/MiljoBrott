using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Infrastructure;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class CitizenController : Controller
	{
		private IEnvironmentalRepository repository;

		public CitizenController(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public ViewResult Contact()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Faq()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Services()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Validate(Errand errand)
		{
			ViewBag.Worker = "Citizen";
			HttpContext.Session.SetJson("ErrandCreation", errand);
			return View(errand);
		}

		public ViewResult Thanks()
		{
			ViewBag.Worker = "Citizen";
			var errand = HttpContext.Session.GetJson<Errand>("ErrandCreation");
			string refNumber = repository.SaveErrand(errand);
			ViewBag.Ref = refNumber;
			HttpContext.Session.Remove("ErrandCreation");
			if (errand == null)
				throw new Exception("Got to thanks page without errand object");
			else
				return View();
		}

		
	}
}
