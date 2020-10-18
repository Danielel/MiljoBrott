using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class ShowFileController : Controller
	{
		private IEnvironmentalRepository repository;

		private IWebHostEnvironment environment;

		public ShowFileController(IEnvironmentalRepository repo, IWebHostEnvironment envi)
		{
			repository = repo;

			environment = envi;
		}
		public IActionResult ShowImage(int id)
		{
			int pictureId = id;

			return View(repository.GetPicture(pictureId));
		}
		
		public FileResult DownloadSample(int id)
		{
			int sampleId = id;
			Sample sample = repository.GetSample(sampleId);
			var path = System.IO.Path.Combine(environment.WebRootPath, "samples", sample.SampleName);
			path = "samples/" + sample.SampleName;
			return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, System.IO.Path.GetFileName(path));

			//return View(repository.GetSample(sampleId));
		}
	}
}
