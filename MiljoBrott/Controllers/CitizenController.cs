using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class CitizenController : Controller
	{
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

		public ViewResult Thanks()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Validate(Errand errand)
		{
			ViewBag.Worker = "Citizen";
			return View(errand);
		}
	}
}
