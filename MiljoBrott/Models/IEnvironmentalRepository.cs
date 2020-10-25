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
		/// Provides the correct list of StartViewErrands for the given employee and its role,
		/// Coordinators will receive all errands. managers will receive all errands assigned to the department of the manager
		/// Investigators will receive all errands that have been assigned to him/her.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns>Errands in the form of StartViewErrands</returns>
		Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrands(string employeeId);

		/// <summary>
		/// Returns the CrimeContentErrandView version of an Errand object for proper model binding.
		/// </summary>
		/// <param name="errandId"></param>
		/// <returns></returns>
		Task<CrimeContentErrandView> GetCrimeContentErrandView(int errandId);

	}
}
