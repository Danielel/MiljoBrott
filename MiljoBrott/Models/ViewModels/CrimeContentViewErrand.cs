using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class CrimeContentViewErrand
	{
		public DateTime DateOfObservation { get; set; }
		public int ErrandId { get; set; }
		public string RefNumber { get; set; }
		public string TypeOfCrime { get; set; }
		public string StatusName { get; set; }
		public string DepartmentName { get; set; }
		public string EmployeeName { get; set; }
		public string Place { get; set; }
		public string Observation { get; set; }
		public string InvestigatorInfo { get; set; }
		public string InvestigatorAction { get; set; }
		public string InformerName { get; set; }
		public string InformerPhone { get; set; }
		public ICollection<Sample> Samples { get; set; }
		public ICollection<Picture> Pictures { get; set; }
	}
}
