using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MiljoBrott.Controllers
{
	public class HomeController : Controller
	{
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
