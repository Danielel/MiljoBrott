using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class ManagerController : Controller
	{
		private IEnvironmentalRepository repository;

		public ManagerController(IEnvironmentalRepository repo)
		{
			repository = repo;
		}
		public ViewResult CrimeManager(int id)
		{
			ViewBag.Worker = "Manager";
			ViewBag.Employees = repository.GetEmployeesOfRole("investigator");
			ViewBag.ID = id;
			return View();
		}

		public IActionResult AssignInvestigator(Errand errand, int id, bool noAction)
		{
			int errandId = id;

			Task<Errand> taskOfErrand = repository.GetErrand(errandId);
			Errand errandFromDb = taskOfErrand.Result;
			if (noAction)
			{
				errandFromDb.StatusId = "S_B"; //Perhaps get from method instead
				errandFromDb.InvestigatorInfo = errand.InvestigatorInfo;

				repository.UpdateErrand(errandFromDb);
			}
			else
			{
				errandFromDb.EmployeeId = errand.EmployeeId; //Change errand status?
				repository.UpdateErrand(errandFromDb);
			}

			return RedirectToAction("CrimeManager", new { id = errandFromDb.ErrandID });
		}

		public ViewResult StartManager()
		{
			ViewBag.Worker = "Manager";
			return View(repository);
		}
	}
}
