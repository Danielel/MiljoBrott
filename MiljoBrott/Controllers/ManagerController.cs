using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;
using static MiljoBrott.Components.StartViewErrandContent;

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

		private async Task<Employee> GetEmployeeData() //Could be inherited from a super version of Controller but whatever
		{
			var userName = contextAcc.HttpContext.User.Identity.Name;
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
					errandFromDb.EmployeeId = errand.EmployeeId;
					repository.UpdateErrand(errandFromDb);
				}
			}

			return RedirectToAction("CrimeManager", new { id = errandFromDb.ErrandID });
		}

		public async Task<ViewResult> StartManager()
		{
			Employee managerEmployee = await GetEmployeeData();
			ViewBag.departmentId = managerEmployee.DepartmentId;

			StartViewErrandInputData startViewErrandInputData = new StartViewErrandInputData
			{
				employeeId = managerEmployee.EmployeeId
			};
			ViewBag.startViewErrandInputData = startViewErrandInputData;


			return View(repository);
		}
		
		[HttpPost]
		public async Task<ViewResult> StartManager(string statusId, string investigatorId, string casenumber, bool isCasenumber)
		{
			Employee managerEmployee = await GetEmployeeData();
			ViewBag.departmentId = managerEmployee.DepartmentId;

			StartViewErrandInputData startViewErrandInputData = new StartViewErrandInputData
			{
				filterUsed = !isCasenumber,
				caseNumberSearched = isCasenumber,
				employeeId = managerEmployee.EmployeeId,
				caseNumber = isCasenumber ? casenumber : null,
				statusId = !isCasenumber ? statusId : null,
				investigatorId = !isCasenumber ? investigatorId : null
				 
			};
			ViewBag.startViewErrandInputData = startViewErrandInputData;


			return View(repository);
		}
	}
}
