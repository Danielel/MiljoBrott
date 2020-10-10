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
			ViewBag.ID = id;
			return View(repository.GetEmployeesOfRole("investigator"));
		}
		
		public ViewResult StartManager()
		{
			ViewBag.Worker = "Manager";
			return View(repository);
		}
	}
}
