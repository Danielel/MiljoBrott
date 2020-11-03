using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class CompleteEmployeeView
	{
		public string EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public string RoleTitle { get; set; }
		public string DepartmentId { get; set; }
		public string DepartmentName { get; set; }
	}
}
