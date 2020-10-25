using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Infrastructure;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{
	public class HomeController : Controller
	{
		private IEnvironmentalRepository repository;

		private UserManager<IdentityUser> userManager;
		private SignInManager<IdentityUser> signInManager;

		public HomeController(IEnvironmentalRepository repo, UserManager<IdentityUser> userM, SignInManager<IdentityUser> signM)
		{
			repository = repo;

			userManager = userM;
			signInManager = signM;
		}

		public ViewResult Index()
		{
			ViewBag.Worker = "Citizen";
			return View();
		}

		public ViewResult Login(string returnUrl)
		{
			ViewBag.Worker = "Citizen";
			return View(new LoginModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);
			if (ModelState.IsValid)
			{
				if (user != null)
				{
					await signInManager.SignOutAsync();

					var trySignInResult = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
					if (trySignInResult.Succeeded)
					{
						//May want to redirect to returnUrl but unsure if that is wanted
						if (await userManager.IsInRoleAsync(user, "Coordinator"))
						{
							return Redirect("/Coordinator/StartCoordinator"); //För Coordinator
						}
						if (await userManager.IsInRoleAsync(user, "Investigator"))
						{
							return Redirect("/Investigator/StartInvestigator"); //För Investigator
						}
						if (await userManager.IsInRoleAsync(user, "Manager"))
						{
							return Redirect("/Manager/StartManager"); //För Manager
						}
						if (await userManager.IsInRoleAsync(user, "SiteAdmin"))
						{
							//Unimplemented
							//return Redirect("/SiteAdmin/StartSiteAdmin");
						}
					}
				}
			}

			ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord");
			return View(loginModel);
		}

		[Authorize]
		public async Task<RedirectResult> Logout(string returnUrl = "/")
		{
			await signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

		[AllowAnonymous]
		public ViewResult AccessDenied(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
	}
}
