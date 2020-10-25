using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Components
{
	public class CrimeContent : ViewComponent
	{
		private IEnvironmentalRepository repository;

		public CrimeContent(IEnvironmentalRepository repo)
		{
			repository = repo;
		}

		public async Task<IViewComponentResult> InvokeAsync(int id)
		{
			CrimeContentErrandView crimeContentErrandView = await repository.GetCrimeContentErrandView(id);
			return View(crimeContentErrandView);
		}
	}
}
