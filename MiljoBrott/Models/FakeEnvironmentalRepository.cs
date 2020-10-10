using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class FakeEnvironmentalRepository : IEnvironmentalRepository
	{
		public IQueryable<Errand> Errands => new List<Errand>
		{
			new Errand{ ErrandID = 2020-45-0001, Place = "Skogslunden vid Jensens gård", TypeOfCrime="Sopor", DateOfObservation = new DateTime(2020,04,24), 
				Observation ="Anmälaren var på promeand i skogslunden när hon upptäckte soporna", 
				InvestigatorInfo = "Undersökning har gjorts och bland soporna hittades bl.a ett brev till Gösta Olsson", 
				InvestigatorAction = "Brev har skickats till Gösta Olsson om soporna och anmälan har gjorts till polisen 2018-05-01", 
				InformerName = "Ada Bengtsson", InformerPhone = "0432-5545522", StatusId="Klar", DepartmentId="Renhållning och avfall", 
				EmployeeId ="Susanne Fred"},
			new Errand{ ErrandID = 2020-45-0002, Place = "Småstadsjön", TypeOfCrime="Oljeutsläpp", DateOfObservation = new DateTime(2020,04,29), 
				Observation ="Jag såg en oljefläck på vattnet när jag var där för att fiska", 
				InvestigatorInfo = "Undersökning har gjorts på plats, ingen fläck har hittas", 
				InvestigatorAction = "", 
				InformerName = "Bengt Svensson", InformerPhone = "0432-5152255", StatusId="Ingen åtgärd", DepartmentId="Natur och Skogsvård", 
				EmployeeId ="Oskar Ivarsson"},
			new Errand{ ErrandID = 2020-45-0003, Place = "Ödehuset", TypeOfCrime="Skrot", DateOfObservation = new DateTime(2020,05,02), 
				Observation ="Anmälaren körde förbi ödehuset och upptäcker ett antal bilar och annat skrot", 
				InvestigatorInfo = "Undersökning har gjorts och bilder har tagits", 
				InvestigatorAction = "", 
				InformerName = "Olle Pettersson", InformerPhone = "0432-5255522", StatusId="Påbörjad", DepartmentId="Miljö och Hälsoskydd", 
				EmployeeId ="Lena Larsson"},
			new Errand{ ErrandID = 2020-45-0004, Place = "Restaurang Krögaren", TypeOfCrime="Buller", DateOfObservation = new DateTime(2020,06,04), 
				Observation ="Restaurangen hade för högt ljud på så man inte kunde sova", 
				InvestigatorInfo = "Bullermätning har gjorts. Man håller sig inom riktvärden", 
				InvestigatorAction = "Meddelat restaurangen att tänka på ljudet i fortsättning", 
				InformerName = "Roland Jönsson", InformerPhone = "0432-5322255", StatusId="Klar", DepartmentId="Miljö och Hälsokydd", 
				EmployeeId ="Martin Kvist"},
			new Errand{ ErrandID = 2020-45-0005, Place = "Torget", TypeOfCrime="Klotter", DateOfObservation = new DateTime(2020,07,10), 
				Observation ="Samtliga skräpkorgar och bänkar är nedklottrade", 
				InvestigatorInfo = "", 
				InvestigatorAction = "", 
				InformerName = "Peter Svensson", InformerPhone = "0432-5322555", StatusId="Inrapporterad", DepartmentId="Ej tillsatt", 
				EmployeeId ="Ej tillsatt"}
		}.AsQueryable<Errand>();

		public IQueryable<ErrandStatus> ErrandStatuses => new List<ErrandStatus>
		{
			new ErrandStatus { StatusId = "S_A", StatusName = "Rapporterad"},
			new ErrandStatus { StatusId = "S_B", StatusName = "Ingen åtgärd"},
			new ErrandStatus { StatusId = "S_C", StatusName = "Startad"},
			new ErrandStatus { StatusId = "S_D", StatusName = "Färdig"}
		}.AsQueryable();

		public IQueryable<Department> Departments => new List<Department>
		{
			new Department { DepartmentId = "D00", DepartmentName = "Småstads Kommun"},
			new Department { DepartmentId = "D01", DepartmentName = "IT-avdelningen"},
			new Department { DepartmentId = "D02", DepartmentName = "Lek och Skoj"},
			new Department { DepartmentId = "D03", DepartmentName = "Miljöskydd"}
		}.AsQueryable();

		public IQueryable<Employee> Employees => new List<Employee>
		{
			new Employee { EmployeeId = "E302", EmployeeName = "Martin Kvist", RoleTitle = "investigator", DepartmentId = "D01"},
			new Employee { EmployeeId = "E301", EmployeeName = "Lena Larsson", RoleTitle = "investigator", DepartmentId = "D01"},
			new Employee { EmployeeId = "E401", EmployeeName = "Oskar Ivarsson", RoleTitle = "investigator", DepartmentId = "D02"},
			new Employee { EmployeeId = "E501", EmployeeName = "Susanne Fred", RoleTitle = "investigator", DepartmentId = "D03"}
		}.AsQueryable();

		public IQueryable<Employee> GetEmployeesOfRole(string roleTitle)
		{
			var employees = from ee in Employees
							where ee.RoleTitle.Equals(roleTitle)
							select ee;
			return employees;
		}
		

		public Task<Errand> GetErrand(string errandId)
		{
			return Task.Run(() =>
			{
				var errand = from err in Errands
							 where String.Equals(err.ErrandID, errandId)
							 select err;
				var result = errand.FirstOrDefault();
				return result;
			});
		}

		/*
		//Apperantly not needed StatusId in Errand contains the string not an id Oo
		public string GetErrandStatus(string id)
		{
			var status = from es in ErrandStatuses
						 where String.Equals(es.StatusId, id)
						 select es;
			var item = status.FirstOrDefault();
			if (item != null)
				return item.StatusName;
			return null;
		}

		public static string FormatPhoneNumber(string number)
		{
			int length = number.Length;
			if (length >= 10)
			{
				number = number.Insert(length - 2, " ");
				number = number.Insert(length - 4, " ");
			}
			return number;
		}*/
		
	}
}
