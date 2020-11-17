using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	[Authorize(Roles = "SiteAdmin")]
	public class SiteAdminController : Controller
	{
		private IEnvironmentalRepository repository;
		private IHttpContextAccessor contextAcc;
		private RoleManager<IdentityRole> roleManager;
		private UserManager<IdentityUser> userManager;

		public SiteAdminController(IEnvironmentalRepository repo, IHttpContextAccessor cont, IServiceProvider services)
		{
			repository = repo;

			contextAcc = cont;

			roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
			userManager = services.GetRequiredService<UserManager<IdentityUser>>();
		}

		private IQueryable<string> GetRoles()
		{
			var roles = roleManager.Roles;
			var roleNames = from role in roles
							select role.Name;
			return roleNames;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUser(CreateUserModel newUserModel)
		{
			IdentityUser user = await userManager.FindByNameAsync(newUserModel.UserName);
			Employee employee = await repository.GetEmployee(newUserModel.UserName);
			if (user != null || employee != null)
			{
				ModelState.AddModelError("", "Användaren finns redan.");
				return View(newUserModel);
			}
			if(!newUserModel.RepeatPassword.Equals(newUserModel.Password))
			{
				ModelState.AddModelError("", "Lösenordet matchar inte det repeterade lösenordet.");
				return View(newUserModel);
			}
			if (ModelState.IsValid)
			{
				IdentityUser newUser = new IdentityUser(newUserModel.UserName);
				IdentityResult identityResult = await userManager.CreateAsync(newUser, newUserModel.Password);
				if(identityResult.Succeeded)
				{
					await repository.AddEmployee(newUserModel.UserName);
					return Redirect("/SiteAdmin/EmployeeModification/" + newUserModel.UserName);
				}
				else
				{
					ModelState.AddModelError("", "Lösenordet måste innehålla minst 6 tecken inklusive stora bokstäver, små bokstäver, siffror och icke alfanumeriska tecken.");
					return View(newUserModel);
				}
					
			}
			return View(newUserModel);
		}

		public IActionResult CreateUser()
		{
			return View();
		}

		public async Task<IActionResult> DeleteUser(string id) //id = employeeId
		{
			string employeeId = id;
			repository.DeleteEmployee(employeeId);

			IdentityUser userToBeDeleted = await userManager.FindByNameAsync(employeeId);
			var deleted = await userManager.DeleteAsync(userToBeDeleted);

			return RedirectToAction("StartSiteAdmin", new { id = employeeId });
		}

		public ViewResult Validate(string id) //id = employeeId for delete
		{
			string employeeId = id;
			ViewBag.EmployeeId = employeeId;
			return View();
		}

		public async Task<IActionResult> UpdateUser(CompleteEmployeeView cEmployee, string id) //id = employeeId
		{
			string employeeId = id;
			Employee employeeToSave = new Employee
			{
				DepartmentId = cEmployee.DepartmentId,
				RoleTitle = cEmployee.RoleTitle,
				EmployeeId = employeeId,
				EmployeeName = cEmployee.EmployeeName
			};

			string dbEmployeeRoleString = (await repository.GetEmployee(employeeId)).RoleTitle;
			if (!cEmployee.RoleTitle.Equals(dbEmployeeRoleString)) //changed role
			{
				IdentityUser updatedUser = await userManager.FindByNameAsync(employeeId);
				await userManager.AddToRoleAsync(updatedUser, cEmployee.RoleTitle);
				if (dbEmployeeRoleString != null)
				{
					if (await userManager.IsInRoleAsync(updatedUser, dbEmployeeRoleString))
						await userManager.RemoveFromRoleAsync(updatedUser, dbEmployeeRoleString); //Remove from previous role if emp was in one.
				}
			}

			repository.UpdateEmployee(employeeToSave);
			return RedirectToAction("EmployeeModification", new { id = employeeId });
		}

		/// <summary>
		/// Opens the view where you can enter data to change role, department, and name of an employee
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ViewResult> EmployeeModification(string id) //id = employeeId
		{
			CompleteEmployeeView employee = await repository.GetCompleteViewOfEmployee(id);
			ViewBag.EmployeeID = id;
			ViewBag.Roles = GetRoles();
			ViewBag.Departments = repository.Departments;

			return View(employee);
		}

		/// <summary>
		/// The start View for SiteAdmins
		/// All employees are listed so they can be modified
		/// </summary>
		/// <returns></returns>
		public async Task<ViewResult> StartSiteAdmin()
		{
			ViewBag.Departments = repository.Departments;
			ViewBag.Roles = GetRoles();
			ViewBag.FilteredOrSearched = false;

			IQueryable<CompleteEmployeeView> listOfEmployees = await repository.GetStartViewEmployees();
			return View(listOfEmployees);
		}

		private async Task<IQueryable<CompleteEmployeeView>> GetFilteredEmployees(string role, string departmentId)
		{
			IQueryable<CompleteEmployeeView> filteredListOfEmployees;
			role = !role.Equals("Välj alla") ? role : null; //null matches all 
			departmentId = !departmentId.Equals("Välj alla") ? departmentId : null; //null matches all 

			filteredListOfEmployees = await repository.GetStartViewEmployeesFiltered(role, departmentId);
			return filteredListOfEmployees;
		}

		/// <summary>
		/// The start View for SiteAdmins
		/// Shows employees so they can be modified.
		/// Employees are filtered by role and or departmendId or they are shown by the search term isCasenumber
		/// isCasenumber matches to employeeId and or employeeName
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ViewResult> StartSiteAdmin(string role, string departmentId, string casenumber, bool isCasenumber)
		{
			ViewBag.Departments = repository.Departments;
			ViewBag.Roles = GetRoles();
			ViewBag.FilteredOrSearched = true;

			IQueryable<CompleteEmployeeView> listOfEmployees;

			if (isCasenumber && casenumber != null) //a caseNumber was entered for search
				listOfEmployees = await repository.GetStartViewEmployeeNameSearched(casenumber);
			else if (role != null || departmentId != null) //the filter function was used to filter employees
				listOfEmployees = await GetFilteredEmployees(role, departmentId);
			else
			{
				ViewBag.FilteredOrSearched = false; //No search or filtering was made
				listOfEmployees = await repository.GetStartViewEmployees();
			}

			return View(listOfEmployees);
		}
	}
}
