using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	/// <summary>
	/// Part of the EFEnvironmentalRepository that provdies funtionality for accessing ViewModel versions of Errands.
	/// </summary>
	public partial class EFEnvironmentalRepository : IEnvironmentalRepository
	{

		private IQueryable<StartViewErrand> GetStartViewErrands(IQueryable<Errand> errandCollection)
		{
			var errandList = from err in errandCollection
							 join stat in ErrandStatuses on err.StatusId equals stat.StatusId
							 join dep in Departments on err.DepartmentId equals dep.DepartmentId
							 into departmentErrand
							 from deptE in departmentErrand.DefaultIfEmpty()

							 join ee in Employees on err.EmployeeId equals ee.EmployeeId
							 into employeeErrand
							 from empE in employeeErrand.DefaultIfEmpty()

							 orderby err.RefNumber ascending

							 select new StartViewErrand
							 {
								 DateOfObservation = err.DateOfObservation,
								 ErrandId = err.ErrandID,
								 RefNumber = err.RefNumber,
								 TypeOfCrime = err.TypeOfCrime,
								 StatusName = stat.StatusName,
								 DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
								 EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName)
							 };
			return errandList;
		}
		public async Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrands()
		{
			return await Task<IQueryable<StartViewErrand>>.Run(() =>
			{
				return GetStartViewErrands(Errands);
			});
			
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewManagerErrands(string departmentId)
		{
			return GetStartViewErrands(await GetErrandsOfDepartment(departmentId));
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrands(string employeeId)
		{
			return GetStartViewErrands(await GetErrandsOfEmployee(employeeId));
		}

		private IQueryable<Errand> GetErrandsMatchingStatus(IQueryable<Errand> errandCollection, string statusId)
		{
			if (statusId == null)
				return errandCollection;
			else
			{
				var errandsToMatch = from matchingErrs in errandCollection
									 where matchingErrs.StatusId.Equals(statusId)
									 select matchingErrs;
				return errandsToMatch;
			}
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrandsFiltered(string employeeId, string statusId)
		{
			var errandsOfCorrectStatus = GetErrandsMatchingStatus(await GetErrandsOfEmployee(employeeId), statusId);
			return GetStartViewErrands(errandsOfCorrectStatus);
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewManagerErrandsFiltered(string departmentId, string statusId, string investigatorId)
		{
			
			var errandsOfCorrectStatus = GetErrandsMatchingStatus(await GetErrandsOfDepartment(departmentId), statusId); //manager only wants errands from one department
			if(investigatorId != null)
			{
				IQueryable<Errand> errandsToMatch = from matchingErrs in errandsOfCorrectStatus
								 where matchingErrs.EmployeeId.Equals(investigatorId)
									select matchingErrs;
				return GetStartViewErrands(errandsToMatch);
			}
			return GetStartViewErrands(errandsOfCorrectStatus);
			
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrandsFiltered(string statusId, string departmentId)
		{
			return await Task.Run(() =>
			{
				var errandsOfCorrectStatus = GetErrandsMatchingStatus(Errands, statusId); //coordinator wants all errands
				if (departmentId != null)
				{
					IQueryable<Errand> errandsToMatch = from matchingErrs in errandsOfCorrectStatus
														where matchingErrs.DepartmentId.Equals(departmentId)
														select matchingErrs;
					return GetStartViewErrands(errandsToMatch);
				}
				return GetStartViewErrands(errandsOfCorrectStatus);
			});
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrands(string employeeId)
		{
			Employee employee = await GetEmployee(employeeId);
			if (employee.RoleTitle.Equals("Investigator"))
			{
				return await GetStartViewInvestigatorErrands(employeeId);
			}
			else if (employee.RoleTitle.Equals("Manager"))
			{
				return await GetStartViewManagerErrands(employee.DepartmentId);
			}
			else if (employee.RoleTitle.Equals("Coordinator"))
			{
				return await GetStartViewCoordinatorErrands();
			}
			else
				throw new Exception("employeeId has no role");
		}

		public async Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrandsCaseNumberSearched(string employeeId, string caseNumber)
		{
			Employee employee = await GetEmployee(employeeId);
			IQueryable<StartViewErrand> errands = await GetStartViewEmployeeErrands(employeeId);

			var matchingErrands = from err in errands
								  where err.RefNumber.Contains(caseNumber)
								  select err;
			return matchingErrands;
		}


		public async Task<CrimeContentErrandView> GetCrimeContentErrandView(int errandId)
		{
			Errand viewErrand = await GetErrand(errandId);

			var errandList = from err in Errands
							 join stat in ErrandStatuses on err.StatusId equals stat.StatusId
							 join dep in Departments on err.DepartmentId equals dep.DepartmentId
							 into departmentErrand
							 from deptE in departmentErrand.DefaultIfEmpty()

							 join ee in Employees on err.EmployeeId equals ee.EmployeeId
							 into employeeErrand
							 from empE in employeeErrand.DefaultIfEmpty()

							 orderby err.RefNumber ascending
							 select new CrimeContentErrandView
							 {
								 DateOfObservation = err.DateOfObservation,
									ErrandId = err.ErrandID,
									RefNumber = err.RefNumber,
									TypeOfCrime = err.TypeOfCrime,
									StatusName = stat.StatusName,
									DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
									EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName),
									Place = err.Place,
									Observation = err.Observation,
									InvestigatorInfo = err.InvestigatorInfo,
									InvestigatorAction = err.InvestigatorAction,
									InformerName = err.InformerName,
									InformerPhone = err.InformerPhone,
									Samples = err.Samples,
									Pictures = err.Pictures
							 };
			var actualErrand = from crimeContentErrandView in errandList
							   where crimeContentErrandView.ErrandId.Equals(viewErrand.ErrandID)
							   select crimeContentErrandView;
			return actualErrand.FirstOrDefault();
		}


	}
}
