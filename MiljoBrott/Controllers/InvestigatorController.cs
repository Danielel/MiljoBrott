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

namespace MiljoBrott.Controllers
{
	public class InvestigatorController : Controller
	{
		private IEnvironmentalRepository repository;
		private IWebHostEnvironment environment;

		public InvestigatorController(IEnvironmentalRepository repo, IWebHostEnvironment envi)
		{
			repository = repo;

			environment = envi;
		}

		public ViewResult CrimeInvestigator(int id)
		{
			ViewBag.Worker = "Investigator";
			ViewBag.Statuses = repository.ErrandStatuses; //ugly
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

		private (bool info, bool action, bool status) ErrandSaveDataExists(Errand errand)
		{
			return (!(errand.InvestigatorInfo is null), !(errand.InvestigatorAction is null), !(errand.StatusId.Equals("Välj")));
		}

		public async Task<IActionResult> InvestigatorDataUpload(IFormFile loadSample, IFormFile loadImage, Errand errand, int id)
		{
			int	errandId = id;

			(bool infoExists, bool actionExists, bool statusExists) = ErrandSaveDataExists(errand);
			bool errandExists = infoExists || actionExists || statusExists;
			bool sampleExists = !(loadSample is null) && loadSample.Length > 0;
			bool imageExists = !(loadImage is null) && loadImage.Length > 0;

			if (!(sampleExists || imageExists || errandExists))
				return RedirectToAction("CrimeInvestigator", new { id = errandId });

			//string sampleFileName;

			Errand dbErrand = await repository.GetErrand(errandId);
			if (sampleExists)
			{
				string sampleFileName = await CreateAndSaveFile(loadSample, "samples");
			}
			if(imageExists)
			{
				string imageFileName = await CreateAndSaveFile(loadImage, "inv_images");
			}
			if(infoExists)
				dbErrand.InvestigatorInfo = dbErrand.InvestigatorInfo + errand.InvestigatorInfo + "\n";
			if(actionExists)
				dbErrand.InvestigatorAction = dbErrand.InvestigatorAction + errand.InvestigatorAction + "\n";
			if(statusExists)
				dbErrand.StatusId = errand.StatusId;

			repository.UpdateErrand(dbErrand);
			return RedirectToAction("CrimeInvestigator", new { id = errandId });
		}
		/*
		private async Task<IActionResult> SSSTakeFileD(IFormFile loadSample, IFormFile loadImage, Errand errand)
		{
			int errandId;
			if (TempData.ContainsKey("I_ID"))
				errandId = int.Parse(TempData["I_ID"].ToString()); //Unsafe from multiple tabs etcetera
			else
			{
				Task<RedirectToActionResult> taskA = Task<RedirectToActionResult>.Run(() => { return RedirectToAction("StartInvestigator"); });
				return await taskA; //await so we can return
				return RedirectToAction("StartInvestigator"); //error message
			}

			bool sampleExists = !(loadSample is null) && loadSample.Length > 0;
			bool imageExists = !(loadImage is null) && loadImage.Length > 0;
			bool errandExists = ErrandSaveDataExists(errand);

			if (!(sampleExists || imageExists || errandExists))
				return RedirectToAction("CrimeInvestigator", new { id = 1 });

			if (!(loadSample is null) && loadSample.Length > 0)
				await CreateAndSaveFile(loadSample, "samples");
			if (!(loadImage is null) && loadImage.Length > 0)
				await CreateAndSaveFile(loadImage, "images");
			var tempPath = Path.GetTempFileName();
			var path = Path.Combine(environment.WebRootPath, "samples", loadSample.FileName);
			if (loadSample.Length > 0)
			{
				if (loadImage.Length > 0)
				{
					//Party
				}
				using (var stream = new FileStream(tempPath, FileMode.Create))
				{
					await loadSample.CopyToAsync(stream);
				}
			}
			System.IO.File.Move(tempPath, path);
			return RedirectToAction("CrimeInvestigator", new { id = 0 });
		}

		public IActionResult InvestigateErrand(IFormFile loadSample, IFormFile loadImage, Errand errand)
		{
			int errandId;
			if (TempData.ContainsKey("I_ID"))
				errandId = int.Parse(TempData["I_ID"].ToString()); //Unsafe from multiple tabs etcetera
			else
				return RedirectToAction("StartInvestigator"); //error message

			Task<Errand> taskOfErrand = repository.GetErrand(errandId);
			Errand errandFromDb = taskOfErrand.Result;
			if (errand.StatusId.Equals("true"))
			{
				errandFromDb.StatusId = "S_B"; //Perhaps get from method instead
				errandFromDb.InvestigatorInfo = errand.InvestigatorInfo;

				repository.UpdateErrand(errandFromDb);
			}
			else
			{
				errandFromDb.EmployeeId = errand.EmployeeId;
				repository.UpdateErrand(errandFromDb);
			}

			return RedirectToAction("CrimeInvestigator", new { id = errandFromDb.ErrandID });
		}
		*/
		public ViewResult StartInvestigator()
		{
			ViewBag.Worker = "Investigator";
			return View(repository);
		}
	}
}
