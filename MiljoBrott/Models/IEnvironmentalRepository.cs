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

		IQueryable<Department> GetDepartmentsExcluding(string departmentId);

		Task<Errand> GetErrand(int errandId);

		Picture AddNewPicture(int errandId, string fileName);

		Sample AddNewSample(int errandId, string fileName);

		Picture GetPicture(int pictureId);
		Sample GetSample(int sampleId);
		string SaveErrand(Errand errand);

		bool UpdateErrand(Errand errand);

		int GetSequenceNumber();

		IQueryable<ErrandStatus> GetInvestigatorErrandStatuses();

		//string GetErrandStatus(string id);

	}
}
