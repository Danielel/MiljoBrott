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

		Task<Errand> GetErrand(int errandId);

		string SaveErrand(Errand errand);

		bool UpdateErrand(Errand errand);

		int GetSequenceNumber();

		//string GetErrandStatus(string id);

	}
}
