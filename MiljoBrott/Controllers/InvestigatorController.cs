using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MiljoBrott.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace MiljoBrott.Controllers
{
	[Authorize(Roles = "Investigator")]
	public class InvestigatorController : Controller
	{
		private IEnvironmentalRepository repository;
		private IWebHostEnvironment environment;
		private IHttpContextAccessor contextAcc;

		public InvestigatorController(IEnvironmentalRepository repo, IWebHostEnvironment envi, IHttpContextAccessor cont)
		{
			repository = repo;

			contextAcc = cont;
			environment = envi;
		}

		private async Task<Employee> GetEmployeeData() //Could be inherited from a super version of Controller but whatever
		{
			var userName = contextAcc.HttpContext.User.Identity.Name; 
			Employee employee = await repository.GetEmployee(userName);
			ViewBag.Worker = employee.RoleTitle;
			return employee;
		}

		public ViewResult CrimeInvestigator(int id)
		{
			ViewBag.Worker = "Investigator";
			ViewBag.Statuses = repository.GetInvestigatorErrandStatuses();
			ViewBag.ID = id;
			return View();
		}

		private async Task<string> CreateAndSaveFile(IFormFile fileU, string folderName)
		{
			var tempPath = Path.GetTempFileName();
			string fileName = Guid.NewGuid().ToString() + "_" + fileU.FileName;

			var path = Path.Combine(environment.WebRootPath, folderName, fileName);

			using (var stream = new FileStream(tempPath, FileMode.Create))
			{
				await fileU.CopyToAsync(stream);
			}
			System.IO.File.Move(tempPath, path);
			return fileName;
		}

		/// <summary>
		/// Checks if a model errand from the CrimeInvestigator view has any relevant data within and returns bools accordingly
		/// </summary>
		/// <param name="errand"></param>
		/// <returns></returns>
		private (bool info, bool action, bool status) ErrandSaveDataExists(Errand errand)
		{
			return (!(errand.InvestigatorInfo is null), 
				!(errand.InvestigatorAction is null), 
				!(errand.StatusId.Equals("Välj")));
		}

		/// <summary>
		/// Appends InvestigatorInfo/InvestigatorAction text to the the already existing InvestigatorInfo/InvestigatorAction
		/// </summary>
		/// <param name="errandString"></param>
		/// <param name="dbErrandString"></param>
		/// <returns>new InvestigatorInfo/InvestigatorAction strings for storage in database</returns>
		private static string GetUpdatedInvestigatorData(string errandString, string dbErrandString)
		{
			if (dbErrandString is null)
				dbErrandString = dbErrandString + errandString;
			else
				dbErrandString = dbErrandString + " " + errandString;
			return dbErrandString;
		}


		/// <summary>
		/// If an investigator has submitted any data it should be updated in the database and potential files should be saved.
		/// </summary>
		/// <param name="loadSample"></param>
		/// <param name="loadImage"></param>
		/// <param name="errand"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IActionResult> InvestigatorDataUpload(IFormFile loadSample, IFormFile loadImage, Errand errand, int id)
		{
			int	errandId = id;

			(bool infoExists, bool actionExists, bool statusExists) = ErrandSaveDataExists(errand);
			bool errandExists = infoExists || actionExists || statusExists;
			bool sampleExists = !(loadSample is null) && loadSample.Length > 0;
			bool imageExists = !(loadImage is null) && loadImage.Length > 0;

			if (!(sampleExists || imageExists || errandExists))
				return RedirectToAction("CrimeInvestigator", new { id = errandId });


			Errand dbErrand = await repository.GetErrand(errandId);
			if (sampleExists)
			{
				string sampleFileName = await CreateAndSaveFile(loadSample, "samples");
				repository.AddNewSample(errandId, sampleFileName);
			}
			if(imageExists)
			{
				string imageFileName = await CreateAndSaveFile(loadImage, "inv_images");
				repository.AddNewPicture(errandId, imageFileName);
			}
			if(infoExists)
				dbErrand.InvestigatorInfo = GetUpdatedInvestigatorData(errand.InvestigatorInfo, dbErrand.InvestigatorInfo);
			if (actionExists)
				dbErrand.InvestigatorAction = GetUpdatedInvestigatorData(errand.InvestigatorAction, dbErrand.InvestigatorAction);
			if(statusExists)
				dbErrand.StatusId = errand.StatusId;

			repository.UpdateErrand(dbErrand);
			return RedirectToAction("CrimeInvestigator", new { id = errandId });
		}

		

		public async Task<ViewResult> StartInvestigator()
		{
			Employee currentEmployee = await GetEmployeeData();
			ViewBag.employeeID = currentEmployee.EmployeeId;

			return View(repository.ErrandStatuses);
		}
	}
}
