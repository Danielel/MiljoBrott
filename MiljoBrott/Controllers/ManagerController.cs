using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	[Authorize(Roles = "Manager")]
	public class ManagerController : Controller
	{
		private IEnvironmentalRepository repository;
		private IHttpContextAccessor contextAcc;

		public ManagerController(IEnvironmentalRepository repo, IHttpContextAccessor cont)
		{
			repository = repo;

			contextAcc = cont;
		}

		private async Task<Employee> GetEmployeeData()
		{
			var userName = contextAcc.HttpContext.User.Identity.Name; //Gna be needed
			Employee employee = await repository.GetEmployee(userName);
			ViewBag.Worker = employee.RoleTitle;
			return employee;
		}

		public async Task<ViewResult> CrimeManager(int id)
		{
			int errandId = id;
			Employee managerEmployee = await GetEmployeeData();
			ViewBag.Employees = await repository.GetEmployeesOfDepartmentAndRole(managerEmployee.DepartmentId, "Investigator");
			ViewBag.ID = errandId;
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
				if (!errand.EmployeeId.Equals("Välj"))
				{
					errandFromDb.StatusId = "S_A"; //Perhaps get from method instead
					errandFromDb.EmployeeId = errand.EmployeeId; //Change errand status?
					repository.UpdateErrand(errandFromDb);
				}
			}

			return RedirectToAction("CrimeManager", new { id = errandFromDb.ErrandID });
		}

		public async Task<ViewResult> StartManager()
		{
			Employee managerEmployee = await GetEmployeeData();
			ViewBag.employeeID = managerEmployee.EmployeeId;
			ViewBag.departmentId = managerEmployee.DepartmentId;
			return View(repository);
		}
	}
}
