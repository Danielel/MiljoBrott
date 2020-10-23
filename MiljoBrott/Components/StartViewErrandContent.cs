using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Components
{
	public class StartViewErrandContent : ViewComponent
	{
		private IEnvironmentalRepository repository;

		public StartViewErrandContent(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public async Task<IViewComponentResult> InvokeAsync(string id)
		{
			//id = employeeId
			Employee currentEmployee = await repository.GetEmployee(id);
			ViewBag.Worker = currentEmployee.RoleTitle;
			IQueryable<StartViewErrand> errands = await repository.GetStartViewEmployeeErrands(id);
			return View(errands);
		}
	}
}
