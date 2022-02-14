using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.Models;
using WebAppEs.Services;
using WebAppEs.ViewModel.Supplier;

namespace WebAppEs.Controllers
{
    public class AddSupplierController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDataAccessService _dataAccessService;
		private readonly ILogger<AdminController> _logger;

		public AddSupplierController(
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

		public IActionResult Index()
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var DataModel = _dataAccessService.GetAllSupplierList();
			return View(DataModel);
		}

		public IActionResult CreateSupplier(Guid Id)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var ModelData = _dataAccessService.GetSupplierList(Id);
			MobileRNDSupplier_VM viewModel = new MobileRNDSupplier_VM();
			if (ModelData != null)
			{
				viewModel = ModelData;
			}
			return View(viewModel);
		}

		[HttpPost]
		//[Authorize("Roles")]
		public async Task<IActionResult> CreateSupplier(MobileRNDSupplier_VM viewModel)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			if (ModelState.IsValid)
			{
				var IsSubmit = await _dataAccessService.AddSupplier(viewModel);
				if (IsSubmit)
				{
					return RedirectToAction("Index", "AddSupplier", null);
				}
				else
				{
					ModelState.AddModelError("SupplierName", "This Supplier Already Exist!");
				}
			}
			return View(viewModel);
		}
	}
}

