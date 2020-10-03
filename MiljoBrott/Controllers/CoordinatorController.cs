using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

		public ViewResult CrimeCoordinator(string id)
		{
			ViewBag.Worker = "Coordinator";
			ViewBag.ID = id;
			return View(repository.Departments);
		}

		public ViewResult ReportCrime()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult StartCoordinator()
		{
			ViewBag.Worker = "Coordinator";
			return View(repository);
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
