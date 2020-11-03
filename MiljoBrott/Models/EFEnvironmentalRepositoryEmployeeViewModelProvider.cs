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
	/// Part of the EFEnvironmentalRepository that provdies funtionality for accessing ViewModel versions of Employees.
	/// </summary>
	public partial class EFEnvironmentalRepository : IEnvironmentalRepository
	{
		public async Task<CompleteEmployeeView> GetCompleteViewOfEmployee(string employeeId)
		{
			Employee emp = await GetEmployee(employeeId);
			CompleteEmployeeView cEV = new CompleteEmployeeView
			{
				EmployeeId = emp.EmployeeId,
				EmployeeName = emp.EmployeeName,
				RoleTitle = emp.RoleTitle,
				DepartmentId = emp.DepartmentId,
				DepartmentName = GetDepartmentName(emp.DepartmentId)
			};
			return cEV;
		}


		private IQueryable<CompleteEmployeeView> GetCompleteEmployeesView(IQueryable<Employee> employeeCollection)
		{
			var employeeList = from emp in employeeCollection
							 join dep in Departments on emp.DepartmentId equals dep.DepartmentId
							 into departmentErrand
							 from deptE in departmentErrand.DefaultIfEmpty()

							 orderby emp.EmployeeId ascending

							 select new CompleteEmployeeView
							 {
								 DepartmentId = emp.DepartmentId,
								 DepartmentName = (emp.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
								 EmployeeName = emp.EmployeeName,
								 EmployeeId = emp.EmployeeId,
								 RoleTitle = emp.RoleTitle
							 };
			return employeeList;
		}

		public async Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployees()
		{
			return await Task.Run(() =>
			{
				return GetCompleteEmployeesView(Employees);
			});
		}

		private IQueryable<CompleteEmployeeView> GetEmployeesMatchingRole(IQueryable<CompleteEmployeeView> employeeCollection, string role)
		{
			if (role == null)
				return employeeCollection;
			else
			{
				var errandsToMatch = from matchingEmps in employeeCollection
									 where matchingEmps.RoleTitle.Equals(role)
									 select matchingEmps;
				return errandsToMatch;
			}
		}

		public async Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployeesFiltered(string role, string departmentId)
		{

			var employeesOfCorrectRole = GetEmployeesMatchingRole(await GetStartViewEmployees(), role); //coordinator wants all errands
			if (departmentId != null)
			{
				IQueryable<CompleteEmployeeView> employeesToMatch = from matchingEmps in employeesOfCorrectRole
																	where departmentId.Equals(matchingEmps.DepartmentId)
																	select matchingEmps;
				return employeesToMatch;
			}
			return employeesOfCorrectRole;
		}

		public async Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployeeNameSearched(string caseNumber)
		{
			IQueryable<CompleteEmployeeView> employees = await GetStartViewEmployees();

			var matchingErrands = from emp in employees
								  where (emp.EmployeeId.Contains(caseNumber) || emp.EmployeeName.Contains(caseNumber))
								  select emp;
			return matchingErrands;
		}


	}
}
