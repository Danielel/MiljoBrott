using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public interface IEnvironmentalRepository
	{
		IQueryable<Errand> Errands { get; }
		IQueryable<ErrandStatus> ErrandStatuses { get; }
		IQueryable<Department> Departments { get; }
		IQueryable<Employee> Employees { get; }
		IQueryable<Picture> Pictures { get; }
		IQueryable<Sample> Samples { get; }
		IQueryable<Sequence> Sequences { get; }

		/// <summary>
		/// Get an Employee object from the database
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		Task<Employee> GetEmployee(string employeeId);

		/// <summary>
		/// Get all employees of a certain role
		/// </summary>
		/// <param name="roleTitle"></param>
		/// <returns></returns>
		IQueryable<Employee> GetEmployeesOfRole(string roleTitle);

		/// <summary>
		/// Get all employees of a certain department.
		/// </summary>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		Task<IQueryable<Employee>> GetEmployeesOfDepartment(string departmentId);

		/// <summary>
		/// Get employees of a certain role under a certain department
		/// </summary>
		/// <param name="departmentId"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		Task<IQueryable<Employee>> GetEmployeesOfDepartmentAndRole(string departmentId, string role);

		/// <summary>
		/// Returns all departments excluding the department with matching id
		/// </summary>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		IQueryable<Department> GetDepartmentsExcluding(string departmentId);

		/// <summary>
		/// Provides the name of the department
		/// </summary>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		string GetDepartmentName(string departmentId);



		/// <summary>
		/// Add a Picture object entry into the database for a given errandId and fileName
		/// </summary>
		/// <param name="errandId"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Picture AddNewPicture(int errandId, string fileName);

		/// <summary>
		/// Add a Sample object entry into the database for a given errandId and fileName
		/// </summary>
		/// <param name="errandId"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Sample AddNewSample(int errandId, string fileName);

		/// <summary>
		/// Get Picture object of matching pictureId from database
		/// </summary>
		/// <param name="pictureId"></param>
		/// <returns></returns>
		Picture GetPicture(int pictureId);
		/// <summary>
		/// Get Sample object of matching sampleId from database
		/// </summary>
		/// <param name="sampleId"></param>
		/// <returns></returns>
		Sample GetSample(int sampleId);

		/// <summary>
		/// Get errand of matching errandId from database
		/// </summary>
		/// <param name="errandId"></param>
		/// <returns></returns>
		Task<Errand> GetErrand(int errandId);

		/// <summary>
		/// Save an errand to database
		/// </summary>
		/// <param name="errandId"></param>
		/// <returns>the RefNumber of the errand</returns>
		string SaveErrand(Errand errand);
		/// <summary>
		/// Update and save to the database an existing errand with the data on the errand parameter.
		/// </summary>
		/// <param name="errand"></param>
		/// <returns>true if an errand of the same id was in the database</returns>
		bool UpdateErrand(Errand errand);
		
		/// <summary>
		/// Updates and saves to the database an existing employee with the employee data from the parameter.
		/// </summary>
		/// <param name="employee"></param>
		/// <returns>true if employee found in database</returns>
		bool UpdateEmployee(Employee employee);

		/// <summary>
		/// Create an employe with employeeId
		/// If an employee already exists with that id, returns false
		/// otherwhise true
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		Task<bool> AddEmployee(string employeeId);

		/// <summary>
		/// Deletes the employee of matching employeeId
		/// </summary>
		/// <param name="employee"></param>
		/// <returns>returns true if an employee of the employeeId existed otherwise false</returns>
		bool DeleteEmployee(string employeeId);

		/// <summary>
		/// Provides the ErrandStatus objects that an Investigator can assign
		/// </summary>
		/// <returns></returns>
		IQueryable<ErrandStatus> GetInvestigatorErrandStatuses();

		/// <summary>
		/// Provides all errands in the form of StartViewErrands
		/// </summary>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrands();
		/// <summary>
		/// Provides the correct list of StartViewErrands assigned to some specific department
		/// </summary>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewManagerErrands(string departmentId);
		/// <summary>
		/// Provides the correct list of StartViewErrands assigned to some specific employee
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrands(string employeeId);

		/// <summary>
		/// Provides all errands of status statusId and department departmentId, if any of the statusId or departmentId equal null then
		/// errands of all statuses/departments are provided
		/// </summary>
		/// <param name="statusId"></param>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrandsFiltered(string statusId, string departmentId);

		/// <summary>
		/// Provides the correct list of StartViewErrands assigned to some specific department
		/// where statusId matches the errand status and investigatorId matches the assigned investigator
		/// if any of statusId or investigatorId equals null then errands of all statuses/departments are provided
		/// </summary>
		/// <param name="departmentId"></param>
		/// <param name="statusId"></param>
		/// <param name="investigatorId"></param>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewManagerErrandsFiltered(string departmentId, string statusId, string investigatorId);

		/// <summary>
		/// Provides the correct list of StartViewErrands assigned to some specific employee
		/// where statusId matches the errand status. If statusId equals null then all errands of all statuses are provided.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="statusId"></param>
		/// <returns></returns>
		Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrandsFiltered(string employeeId, string statusId);


		/// <summary>
		/// Provides the correct list of StartViewErrands for the given employee and its role,
		/// Coordinators will receive all errands. managers will receive all errands assigned to the department of the manager
		/// Investigators will receive all errands that have been assigned to him/her.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns>Errands in the form of StartViewErrands</returns>
		Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrands(string employeeId);

		/// <summary>
		/// Provides the correct list of StartViewErrands for the given employee and its role,
		/// where the errand refNumber contains the caseNumber string 
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="caseNumber"></param>
		/// <returns>Errands in the form of StartViewErrands</returns>
		Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrandsCaseNumberSearched(string employeeId, string caseNumber);

		/// <summary>
		/// Returns the CrimeContentErrandView version of an Errand object for proper model binding.
		/// </summary>
		/// <param name="errandId"></param>
		/// <returns></returns>
		Task<CrimeContentErrandView> GetCrimeContentErrandView(int errandId);

		/// <summary>
		/// Provides the CompleteEmployeeView version of an employee
		/// </summary>
		/// <returns></returns>
		Task<CompleteEmployeeView> GetCompleteViewOfEmployee(string employeeId);

		/// <summary>
		/// Provides a list of all employees including their linked deparmentNames in the form of CompleteEmployeeViews
		/// </summary>
		/// <returns></returns>
		Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployees();

		/// <summary>
		/// Provides all employees of RoleTitle role and department departmentId, if any of the role or departmentId parameters equal null then
		/// employees of all roles/departments are provided 
		/// </summary>
		/// <param name="role"></param>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployeesFiltered(string role, string departmentId);

		/// <summary>
		/// Provides all employees which have the casenumber string in either the EmployeeId or EmployeeName
		/// </summary>
		/// <param name="caseNumber"></param>
		/// <returns></returns>
		Task<IQueryable<CompleteEmployeeView>> GetStartViewEmployeeNameSearched(string caseNumber);

	}
}
