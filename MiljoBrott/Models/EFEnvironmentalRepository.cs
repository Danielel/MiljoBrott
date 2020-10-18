using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class EFEnvironmentalRepository : IEnvironmentalRepository
	{

		private ApplicationDbContext context;

		public EFEnvironmentalRepository(ApplicationDbContext cont)
		{
			context = cont;
		}

		//public IQueryable<Errand> Errands => context.Errands;
		public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e=>e.Pictures);

		public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;

		public IQueryable<Department> Departments => context.Departments;

		public IQueryable<Employee> Employees => context.Employees;

		public IQueryable<Picture> Pictures => context.Pictures;

		public IQueryable<Sample> Samples => context.Samples;
		public IQueryable<Sequence> Sequences => context.Sequences;

		public IQueryable<Employee> GetEmployeesOfRole(string roleTitle)
		{
			var employees = from ee in Employees
							where ee.RoleTitle.Equals(roleTitle)
							select ee;
			//Console.WriteLine("Employee named " + employees.First().EmployeeName + " has role " + employees.First().RoleTitle);
			return employees;
		}

		public IQueryable<Department> GetDepartmentsExcluding(string departmentId)
		{
			return from dep in Departments
				   where !dep.DepartmentId.Equals(departmentId)
				   select dep;
		}


		public Task<Errand> GetErrand(int errandId)
		{
			return Task.Run(() =>
			{
				var errand = from err in Errands
							 where err.ErrandID == errandId
							 select err;
				var result = errand.FirstOrDefault();
				return result;
			});
		}

		public int GetSequenceNumber()
		{
			Sequence sequence = context.Sequences.FirstOrDefault(sq => sq.Id == 1);
			if (sequence == null)
				throw new Exception("No Sequence object in database");
			int currentNumber = sequence.CurrentValue;
			sequence.CurrentValue++;
			context.SaveChanges();

			return currentNumber;
		}

		public string CreateErrandRefNumber()
		{
			int sequenceNumber = GetSequenceNumber();
			string refNumber = DateTime.Now.Year + "-45-" + sequenceNumber;
			return refNumber;
		}

		//Returns the ref-number of the errand
		public string SaveErrand(Errand errand)
		{
			if(errand.ErrandID == 0)
			{
				errand.RefNumber = CreateErrandRefNumber();
				errand.StatusId = "S_A";
				context.Errands.Add(errand);
				context.SaveChanges();
			} else
			{
				UpdateErrand(errand);
			}
			return errand.RefNumber;
		}
		
		public Sample AddNewSample(int errandId, string fileName)
		{
			Sample sample = new Sample { ErrandId = errandId, SampleName = fileName, SampleId = 0 };
			
			context.Samples.Add(sample);
			context.SaveChanges();
			return sample;
		}
		
		public Picture AddNewPicture(int errandId, string fileName)
		{
			Picture pic = new Picture { ErrandId = errandId, PictureName = fileName, PictureId = 0 };
			
			context.Pictures.Add(pic);
			context.SaveChanges();
			return pic;
		}

		public Picture GetPicture(int pictureId)
		{
			var picture = from pic in Pictures
							where pic.PictureId.Equals(pictureId)
							select pic;
			var result = picture.FirstOrDefault();
			return result;
		}
		public Sample GetSample(int sampleId)
		{
			var sample = from sa in Samples
							where sa.SampleId.Equals(sampleId)
							select sa;
			var result = sample.FirstOrDefault();
			return result;
		}

		//Returns true if there was an errand of matching id that was updated.
		public bool UpdateErrand(Errand errand)
		{
			if(errand.ErrandID != 0)
			{
				Errand dbErrand = context.Errands.FirstOrDefault(err => errand.ErrandID == err.ErrandID);
				if(dbErrand != null)
				{
					dbErrand.DateOfObservation = errand.DateOfObservation;
					dbErrand.DepartmentId = errand.DepartmentId;
					dbErrand.EmployeeId = errand.EmployeeId;
					dbErrand.InformerName = errand.InformerName;
					dbErrand.InformerPhone = errand.InformerPhone;
					dbErrand.InvestigatorAction = errand.InvestigatorAction;
					dbErrand.InvestigatorInfo = errand.InvestigatorInfo;
					dbErrand.Observation = errand.Observation;
					dbErrand.Place = errand.Place;
					dbErrand.RefNumber = errand.RefNumber;
					dbErrand.StatusId = errand.StatusId;
					dbErrand.TypeOfCrime = errand.TypeOfCrime;
					context.SaveChanges();
				}
				return true;
			}
			else
				return false;
		}

		public IQueryable<ErrandStatus> GetInvestigatorErrandStatuses()
		{
			return from es in ErrandStatuses
				   where !es.StatusId.Equals("S_A") && !es.StatusId.Equals("S_B")
				   select es;
		}


	}
}
