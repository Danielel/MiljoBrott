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
		public async Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrands()
		{
			var errandList = from err in Errands
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

		public async Task<IQueryable<StartViewErrand>> GetStartViewManagerErrands(string departmentId)
		{
			var errandList = from err in await GetErrandsOfDepartment(departmentId)
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

		public async Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrands(string employeeId)
		{
			var errandList = from err in await GetErrandsOfEmployee(employeeId)
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
