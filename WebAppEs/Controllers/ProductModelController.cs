using Microsoft.AspNetCore.Authorization;
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
using WebAppEs.ViewModel.PartsModel;

namespace WebAppEs.Controllers
{
    public class ProductModelController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDataAccessService _dataAccessService;
		private readonly ILogger<AdminController> _logger;

		public ProductModelController(
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

			var DataModel = _dataAccessService.GetAllPartsModelList();
			return View(DataModel);
        }

        public IActionResult CreateModel(Guid Id)
        {
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			var ModelData = _dataAccessService.GetPartsModelList(Id);
			PartsModelViewModel viewModel = new PartsModelViewModel();
			if (ModelData != null)
            {
				viewModel = ModelData;
			}
			return View(viewModel);
        }

		[HttpPost]
		//[Authorize("Roles")]
		public async Task<IActionResult> CreateModel(PartsModelViewModel viewModel)
		{
			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if (employeeID == null)
			{
				return RedirectToAction("Logout", "Account");
			}

			if (ModelState.IsValid)
			{
				var IsSubmit = await _dataAccessService.AddPartsModel(viewModel);
				if(IsSubmit)
                {
					return RedirectToAction("Index", "ProductModel", null);
				}
                else
                {
					ModelState.AddModelError("Name", "This Model Already Exist!");
				}
			}
		return View(viewModel);
		}
	}
}
