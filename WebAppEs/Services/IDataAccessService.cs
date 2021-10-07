using WebAppEs.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppEs.ViewModel.PartsModel;
using WebAppEs.ViewModel.FaultsEntry;
using WebAppEs.Entity;
using WebAppEs.ViewModel.Home;
using WebAppEs.ViewModel.Register;
using WebAppEs.ViewModel.Report;

namespace WebAppEs.Services
{
    public interface IDataAccessService
	{
		Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act);
		Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal);
		Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string id);
		Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<Guid> permissionIds);
		List<EmployeeListVM> GetAllEmployeeList();

		//PartsModel
		Task<bool> AddPartsModel(PartsModelViewModel viewModel);
		List<PartsModelViewModel> GetAllPartsModelList();

		PartsModelViewModel GetPartsModelList(Guid Id);

		// Add Faults
		bool AddFaultsEntry(MobileRNDFaultsEntryViewModel viewModel);
		bool UpdateFaultsEntry(MobileRNDFaultsEntryViewModel viewModel);
		List<MobileRNDFaultsEntryViewModel> GetAllFaultsList(string EmployeeID);
		List<MobileRNDFaultsEntryViewModel> SortableAllFaultsList(DateTime? startDate, DateTime? toDate, string lineNo, Guid ModelID, string lotNo, string EmployeeID);
		MobileRNDFaultsEntryViewModel GetFaults(Guid Id);
		MobileRNDFaultsEntryViewModel GetSortedFaults(DateTime? sortdate, string lineNo, Guid ModelID, string lotNo);
		// Add Faults Details

		bool AddFaultsDetails(MobileRNDFaultDetailsViewModel viewModel);
		List<MobileRNDFaultDetails> GetSortedFaultsDetails(Guid id);

		List<MobileRNDFaultDetailsViewModel> GetFaultsDetails(Guid id);

		bool RemoveDetails(List<MobileRNDFaultDetails> Model);

		//Dashboard
		//DashboasrViewModel GetDashboardData(DateTime YesterdayDate, DateTime LastWeekStart, DateTime LastWeekEnd, DateTime LastMonthStart, DateTime LastMonthEnd);
		DashboasrViewModel GetDashboardData(DateTime? YesterdayDate, DateTime LastSevenDayStart, DateTime LastMonthDayStart);
		DashboasrViewModel GetSingelDayData(DateTime? Date);
		//FunctionalFaultsPercentageViewModel FunctionalFaultsPercentageViewModel(DateTime Date);
		//AestheticFaultsPercentageViewModel AestheticFaultsPercentageViewModel(DateTime Date);

		//Report
		List<DetailsReportViewModel> DetailsReportList(DateTime? Date);
		bool RemoveeNTRY(Guid Id);

		DateTime LastDate();

		List<AutoCompleteViewModel> FaultsTypeAutoComplete(string Prefix, string type);

	}
}