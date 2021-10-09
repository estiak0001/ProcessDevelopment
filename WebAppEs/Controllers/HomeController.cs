using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppEs.Models;
using System.Diagnostics;
using WebAppEs.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebAppEs.ViewModel.Home;
using System;
using WebAppEs.Views.NewFolder;

namespace WebAppEs.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDataAccessService _dataAccessService;

		public HomeController(UserManager<ApplicationUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IDataAccessService dataAccessService, ILogger<HomeController> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dataAccessService = dataAccessService;
			_logger = logger;
		}

		//[Authorize("Authorization")]
		public IActionResult Index()
		{
			
			var yesterday = DateTime.Today.AddDays(-1);
			var lastSevenDayStart =  DateTime.Today.AddDays(-7);
			var lastMonthDayStart = DateTime.Today.AddDays(-30);

			var LastEntryDate = _dataAccessService.LastDate();

			var chartdata = _dataAccessService.GetSingelDayData(LastEntryDate);

			DashboasrViewModel dashboard = new DashboasrViewModel();
			

			var employeeID = HttpContext.Session.GetString("EmployeeID");
			if(employeeID == null)
            {
				return RedirectToAction("Logout", "Account");
			}

			dashboard = _dataAccessService.GetDashboardData(yesterday, lastSevenDayStart, lastMonthDayStart);
			if (chartdata.Lavel != null)
			{
				dashboard.Lavel = chartdata.Lavel;
				dashboard.FunctionalFaultsPercentageViewModel = chartdata.FunctionalFaultsPercentageViewModel;
				dashboard.AestheticFaultsPercentageViewModel = chartdata.AestheticFaultsPercentageViewModel;
			}
			dashboard.Date = LastEntryDate;
			return View(dashboard);
		}
		//[Authorize("Authorization")]
		public IActionResult Privacy()
		{
			return View();
		}
		//[Authorize("Authorization")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		
		public JsonResult LoadChartDataBayFilter(DateTime Date)
		{
			DashboasrViewModel dashboard = new DashboasrViewModel();
			var chartdata = _dataAccessService.GetSingelDayData(Date);
			if (chartdata.Lavel != null)
			{
				dashboard.Lavel = chartdata.Lavel;
				dashboard.FunctionalFaultsPercentageViewModel = chartdata.FunctionalFaultsPercentageViewModel;
				dashboard.AestheticFaultsPercentageViewModel = chartdata.AestheticFaultsPercentageViewModel;
			}

			return Json(dashboard);
		}


	}
}