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

		public struct StartViewErrandInputData
		{
			public bool caseNumberSearched;
			public bool filterUsed;
			public string employeeId;
			public string caseNumber;
			public string statusId;
			public string departmentId;
			public string investigatorId;
		}

		public StartViewErrandContent(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		private async Task<IQueryable<StartViewErrand>> GetFilteredErrands(string role, StartViewErrandInputData inputData)
		{
			IQueryable<StartViewErrand> filteredListOfErrands;
			string statusID = !inputData.statusId.Equals("Välj alla") ? inputData.statusId : null; //null matches all 
			string departmentID;
			string investigatorID;
			if (inputData.departmentId != null)
				departmentID = !inputData.departmentId.Equals("Välj alla") ? inputData.departmentId : null; //null matches all
			else
				departmentID = null;
			if (inputData.investigatorId != null)
				investigatorID = !inputData.investigatorId.Equals("Välj alla") ? inputData.investigatorId : null; //null matches all 
			else
				investigatorID = null; 


			if (role.Equals("Investigator"))
				filteredListOfErrands = await repository.GetStartViewInvestigatorErrandsFiltered(inputData.employeeId, statusID);
			else if (role.Equals("Manager"))
				filteredListOfErrands = await repository.GetStartViewManagerErrandsFiltered((await repository.GetEmployee(inputData.employeeId)).DepartmentId, statusID, investigatorID);
			else if (role.Equals("Coordinator"))
				filteredListOfErrands = await repository.GetStartViewCoordinatorErrandsFiltered(statusID, departmentID);
			else
				throw new Exception("No matching role");
			return filteredListOfErrands;
		}

		public async Task<IViewComponentResult> InvokeAsync(StartViewErrandInputData inputData)
		{
			//id = employeeId
			Employee currentEmployee = await repository.GetEmployee(inputData.employeeId);
			ViewBag.Worker = currentEmployee.RoleTitle;
			IQueryable<StartViewErrand> errands;
			ViewBag.FilteredOrSearched = true;
			if (inputData.caseNumberSearched && inputData.caseNumber != null) //a caseNumber was entered for search
				errands = await repository.GetStartViewEmployeeErrandsCaseNumberSearched(inputData.employeeId, inputData.caseNumber);
			else if (inputData.filterUsed) //the filter function was used to filter errands
				errands = await GetFilteredErrands(currentEmployee.RoleTitle, inputData);
			else
			{
				ViewBag.FilteredOrSearched = false; //No search or filtering was made
				errands = await repository.GetStartViewEmployeeErrands(inputData.employeeId);
			}
				

			return View(errands);
		}
	}
}
