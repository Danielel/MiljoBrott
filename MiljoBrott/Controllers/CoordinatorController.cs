using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Infrastructure;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	[Authorize(Roles = "Coordinator")]
	public class CoordinatorController : Controller
	{
		private IEnvironmentalRepository repository;
		private IHttpContextAccessor contextAcc;
		public CoordinatorController(IEnvironmentalRepository repo, IHttpContextAccessor cont)
		{
			repository = repo;
			contextAcc = cont;
		}

		private async Task<Employee> GetEmployeeData() //Could be inherited from a super version of Controller but whatever
		{
			var userName = contextAcc.HttpContext.User.Identity.Name;
			Employee employee = await repository.GetEmployee(userName);
			ViewBag.Worker = employee.RoleTitle;
			return employee;
		}

		public ViewResult CrimeCoordinator(int id)
		{
			ViewBag.Worker = "Coordinator";
			ViewBag.ID = id;
			ViewBag.Departments = repository.GetDepartmentsExcluding("D00"); //Coordinators should not be able to assign errands to the "Småstad Kommun" department.
			return View();
		}

		public IActionResult DepartmentChange(Department dep, int id)
		{
			int errandId = id;
			if (!(dep.DepartmentId.Equals("Välj"))) //if department == "Välj, no department was chosen in the dropdown list 
			{
				Task<Errand> taskOfErrand = repository.GetErrand(errandId);
				Errand errand = taskOfErrand.Result;
				errand.DepartmentId = dep.DepartmentId;

				repository.UpdateErrand(errand);

			}
			return RedirectToAction("CrimeCoordinator",  new { id = errandId });
		}

		public ViewResult ReportCrime()
		{
			ViewBag.Worker = "Coordinator";
			return View();
		}

		public ViewResult StartCoordinator()
		{
			ViewBag.Worker = "Coordinator";

			var username = contextAcc.HttpContext.User.Identity.Name;
			ViewBag.employeeID = username; //username = employeeId
			return View(repository);
		}

		public ViewResult Validate(Errand errand)
		{
			ViewBag.Worker = "Coordinator";
			HttpContext.Session.SetJson("ErrandCreation", errand);
			return View(errand);
		}

		public ViewResult Thanks()
		{
			ViewBag.Worker = "Coordinator";
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
