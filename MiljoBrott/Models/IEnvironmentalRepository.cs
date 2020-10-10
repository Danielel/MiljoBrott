﻿using System;
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

		IQueryable<Employee> GetEmployeesOfRole(string roleTitle);

		Task<Errand> GetErrand(int errandId);

		//string GetErrandStatus(string id);

	}
}
