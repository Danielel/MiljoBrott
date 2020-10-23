using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiljoBrott.Models;

namespace MiljoBrott.Controllers
{

	[Authorize]
	public partial class AccountController : Controller
	{
		private UserManager<IdentityUser> userManager;
		private SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userM, SignInManager<IdentityUser> signM)
		{
			userManager = userM;
			signInManager = signM;
		}

		[AllowAnonymous]
		private IActionResult Login(string returnUrl)
		{
			return View(new LoginModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);
			if(ModelState.IsValid)
			{
				if (user != null)
				{
					await signInManager.SignOutAsync();

					var trySignInResult = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
					if (trySignInResult.Succeeded)
					{
						if(await userManager.IsInRoleAsync(user, "Coordinator"))
						{
							return Redirect("/Controller/Action"); //För Coordinator
						}
						if(await userManager.IsInRoleAsync(user, "Investigator"))
						{
							return Redirect("/Controller/Action"); //För investigator
						}
					}
				}
			}

			ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord");
			return View(loginModel);
		}

		public async Task<RedirectResult> Logout(string returnUrl = "/")
		{
			await signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

		[AllowAnonymous]
		public ViewResult AccessDenied()
		{
			return View();
		}
	}
}
