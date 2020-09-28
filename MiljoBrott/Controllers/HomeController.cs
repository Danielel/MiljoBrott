using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class HomeController : Controller
	{
		private IEnvironmentalRepository repository;

		public HomeController(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public ViewResult Index()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Login()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}
	}
}
