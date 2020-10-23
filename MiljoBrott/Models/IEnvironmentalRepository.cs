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

		IQueryable<Employee> GetEmployeesOfRole(string roleTitle);

		Task<IQueryable<Employee>> GetEmployeesOfDepartment(string departmentId);

		Task<IQueryable<Employee>> GetEmployeesOfDepartmentAndRole(string departmentId, string role);

		IQueryable<Department> GetDepartmentsExcluding(string departmentId);

		Task<Employee> GetEmployee(string employeeId);

		Task<Errand> GetErrand(int errandId);

		Task<CrimeContentViewErrand> GetCrimeContentErrandView(int errandId);

		Picture AddNewPicture(int errandId, string fileName);

		Sample AddNewSample(int errandId, string fileName);

		Picture GetPicture(int pictureId);
		Sample GetSample(int sampleId);
		string SaveErrand(Errand errand);

		bool UpdateErrand(Errand errand);

		int GetSequenceNumber();

		IQueryable<ErrandStatus> GetInvestigatorErrandStatuses();

		Task<IQueryable<StartViewErrand>> GetStartViewCoordinatorErrands();

		Task<IQueryable<StartViewErrand>> GetStartViewManagerErrands(string departmentId);

		Task<IQueryable<StartViewErrand>> GetStartViewInvestigatorErrands(string employeeId);

		/**
		 * Provides the correct list of StartViewErrands for the given employee
		 * 
		 */
		Task<IQueryable<StartViewErrand>> GetStartViewEmployeeErrands(string employeeId);

		//string GetErrandStatus(string id);

	}
}
