using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class InvestigatorController : Controller
	{
		private IEnvironmentalRepository repository;

		public InvestigatorController(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public ViewResult CrimeInvestigator()
		{
			ViewBag.Worker = "Investigator";
			return View();
		}
		
		public ViewResult StartInvestigator()
		{
			ViewBag.Worker = "Investigator";
			return View(repository);
		}
	}
}
