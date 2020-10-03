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

		public ViewResult CrimeInvestigator(string id)
		{
			ViewBag.Worker = "Investigator";
			ViewBag.ID = id;
			return View(repository.ErrandStatuses);
		}
		
		public ViewResult StartInvestigator()
		{
			ViewBag.Worker = "Investigator";
			return View(repository);
		}
	}
}
