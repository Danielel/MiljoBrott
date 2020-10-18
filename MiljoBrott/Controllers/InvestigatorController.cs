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
				dbErrand.InvestigatorInfo = dbErrand.InvestigatorInfo + errand.InvestigatorInfo + "\n";
			if(actionExists)
				dbErrand.InvestigatorAction = dbErrand.InvestigatorAction + errand.InvestigatorAction + "\n";
			if(statusExists)
				dbErrand.StatusId = errand.StatusId;

			repository.UpdateErrand(dbErrand);
			return RedirectToAction("CrimeInvestigator", new { id = errandId });
		}
		
		public ViewResult StartInvestigator()
		{
			ViewBag.Worker = "Investigator";
			return View(repository);
		}
	}
}
