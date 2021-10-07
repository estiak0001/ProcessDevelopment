using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAppEs.Models;
using WebAppEs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.ViewModel.Register;
using Microsoft.AspNetCore.Http;

namespace WebAppEs.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDataAccessService _dataAccessService;
		private readonly ILogger<AdminController> _logger;

		public AdminController(
				UserManager<ApplicationUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IDataAccessService dataAccessService,
				ILogger<AdminController> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dataAccessService = dataAccessService;
			_logger = logger;
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> Roles()
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var roleViewModel = new List<RoleViewModel>();

			try
			{
				var roles = await _roleManager.Roles.ToListAsync();
				foreach (var item in roles)
				{
					roleViewModel.Add(new RoleViewModel()
					{
						Id = item.Id,
						Name = item.Name,
					});
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(roleViewModel);
		}

		[Authorize("Roles")]
		public IActionResult CreateRole()
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			return View(new RoleViewModel());
		}

		[HttpPost]
		[Authorize("Roles")]
		public async Task<IActionResult> CreateRole(RoleViewModel viewModel)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			if (ModelState.IsValid)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole() { Name = viewModel.Name });
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Roles));
				}
				else
				{
					ModelState.AddModelError("Name", string.Join(",", result.Errors));
				}
			}

			return View(viewModel);
		}

		[AllowAnonymous]
		public async Task<IActionResult> AddUser()
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var viewModel = new RegisterViewModel();
			var allRoles = await _roleManager.Roles.ToListAsync();
			viewModel.Roles = allRoles.Select(x => new RoleViewModel()
			{
				Id = x.Id,
				Name = x.Name,
				Selected = false
			}).ToArray();

			return View(viewModel);
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> AddUser(RegisterViewModel model)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var viewModel = new RegisterViewModel();
			var allRoles = await _roleManager.Roles.ToListAsync();
			viewModel.Roles = allRoles.Select(x => new RoleViewModel()
			{
				Id = x.Id,
				Name = x.Name,
				Selected = false
			}).ToArray();

			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					EmployeeID = model.EmployeeID,
					Name = model.Name,
					UserName = model.EmployeeID,
					Email = model.Email,
					Department = model.Department,
				};
				var users = await _userManager.FindByNameAsync(user.UserName);

				if(users == null)
                {
					if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 4)
					{
						ModelState.AddModelError("Password", "The Password must be at least 4 characters long.");
						return View(viewModel);
					}
                    else
                    {
						await _userManager.CreateAsync(user, model.Password);
						var userforassign = await _userManager.FindByNameAsync(user.UserName);
						await _userManager.AddToRolesAsync(userforassign, model.Roles.Where(x => x.Selected).Select(x => x.Name));
						return RedirectToAction(nameof(Users));
					}
				}
                else
                {
					ModelState.AddModelError("EmployeeID", "This Employee ID Already Exist!");
				}

			}
			return View(viewModel);
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> Users()
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var userViewModel = new List<UserViewModel>();

			try
			{
				var users = await _userManager.Users.ToListAsync();
				foreach (var item in users)
				{
					userViewModel.Add(new UserViewModel()
					{
						Id = item.Id,
						Email = item.Email,
						UserName = item.UserName,
						Department = item.Department,
						EmployeeID = item.EmployeeID,
						Name =item.Name
						
					});
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(userViewModel);
		}

		[Authorize("Users")]
		public async Task<IActionResult> EditUser(string id)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var viewModel = new UserViewModel();
			if (!string.IsNullOrWhiteSpace(id))
			{
				var user = await _userManager.FindByIdAsync(id);
				var userRoles = await _userManager.GetRolesAsync(user);

				viewModel.Email = user?.Email;
				viewModel.Name = user?.Name;
				viewModel.EmployeeID = user?.EmployeeID;
				viewModel.Department = user?.Department;

				var allRoles = await _roleManager.Roles.ToListAsync();
				viewModel.Roles = allRoles.Select(x => new RoleViewModel()
				{
					Id = x.Id,
					Name = x.Name,
					Selected = userRoles.Contains(x.Name)
				}).ToArray();
			}

			return View(viewModel);
		}

		[HttpPost]
		[Authorize("Users")]
		public async Task<IActionResult> EditUser(UserViewModel viewModel)
		{

			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(viewModel.Id);
				var userRoles = await _userManager.GetRolesAsync(user);

				user.Email = viewModel.Email;
				user.Name = viewModel.Name;
				user.Department = viewModel.Department;

				await _userManager.RemoveFromRolesAsync(user, userRoles);
				await _userManager.AddToRolesAsync(user, viewModel.Roles.Where(x => x.Selected).Select(x => x.Name));

				return RedirectToAction(nameof(Users));
			}

			return View(viewModel);
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var permissions = new List<NavigationMenuViewModel>();
			if (!string.IsNullOrWhiteSpace(id))
			{
				permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(id);
			}

			return View(permissions);
		}

		[HttpPost]
		[Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id, List<NavigationMenuViewModel> viewModel)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			if (ModelState.IsValid)
			{
				var permissionIds = viewModel.Where(x => x.Permitted).Select(x => x.Id);

				await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissionIds);
				return RedirectToAction(nameof(Roles));
			}

			return View(viewModel);
		}
	}
}