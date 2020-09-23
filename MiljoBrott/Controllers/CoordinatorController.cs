using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MiljoBrott.Controllers
{
	public class CoordinatorController : Controller
	{
		public ViewResult CrimeCoordinator()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult ReportCrime()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult StartCoordinator()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult Thanks()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult Validate()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}
	}
}
