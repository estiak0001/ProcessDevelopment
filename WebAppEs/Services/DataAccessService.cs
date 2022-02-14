using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAppEs.Data;
using WebAppEs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppEs.ViewModel.PartsModel;
using WebAppEs.Entity;
using WebAppEs.ViewModel.FaultsEntry;
using WebAppEs.ViewModel.Home;
using WebAppEs.ViewModel.Register;
using WebAppEs.ViewModel.Report;
using WebAppEs.ViewModel.Supplier;

namespace WebAppEs.Services
{
    public class DataAccessService : IDataAccessService
	{
		private readonly IMemoryCache _cache;
		private readonly ApplicationDbContext _context;

		public DataAccessService(ApplicationDbContext context, IMemoryCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal)
		{
			var isAuthenticated = principal.Identity.IsAuthenticated;
			if (!isAuthenticated)
			{
				return new List<NavigationMenuViewModel>();
			}

			var roleIds = await GetUserRoleIds(principal);

			var permissions = await _cache.GetOrCreateAsync("Permissions",
				async x => await (from menu in _context.NavigationMenu select menu).ToListAsync());

			var rolePermissions = await _cache.GetOrCreateAsync("RolePermissions",
				async x => await (from menu in _context.RoleMenuPermission select menu).Include(x => x.NavigationMenu).ToListAsync());

			var data = (from menu in rolePermissions
						join p in permissions on menu.NavigationMenuId equals p.Id
						where roleIds.Contains(menu.RoleId)
						select p)
							  .Select(m => new NavigationMenuViewModel()
							  {
								  Id = m.Id,
								  Name = m.Name,
								  Area = m.Area,
								  Visible = m.Visible,
								  IsExternal = m.IsExternal,
								  ActionName = m.ActionName,
								  ExternalUrl = m.ExternalUrl,
								  DisplayOrder = m.DisplayOrder,
								  ParentMenuId = m.ParentMenuId,
								  ControllerName = m.ControllerName,
							  }).Distinct().OrderBy(x=> x.DisplayOrder).ToList();

			return data;
		}

		public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act)
		{
			var result = false;
			var roleIds = await GetUserRoleIds(ctx);
			var data = await (from menu in _context.RoleMenuPermission
							  where roleIds.Contains(menu.RoleId)
							  select menu)
							  .Select(m => m.NavigationMenu)
							  .Distinct()
							  .ToListAsync();

			foreach (var item in data)
			{
				result = (item.ControllerName == ctrl && item.ActionName == act);
				if (result)
				{
					break;
				}
			}

			return result;
		}

		public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string id)
		{
			var items = await (from m in _context.NavigationMenu
							   join rm in _context.RoleMenuPermission
								on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
								into rmp
							   from rm in rmp.DefaultIfEmpty()
							   select new NavigationMenuViewModel()
							   {
								   Id = m.Id,
								   Name = m.Name,
								   Area = m.Area,
								   ActionName = m.ActionName,
								   ControllerName = m.ControllerName,
								   IsExternal = m.IsExternal,
								   ExternalUrl = m.ExternalUrl,
								   DisplayOrder = m.DisplayOrder,
								   ParentMenuId = m.ParentMenuId,
								   Visible = m.Visible,
								   Permitted = rm.RoleId == id
							   })
							   .AsNoTracking()
							   .ToListAsync();

			return items;
		}

		public async Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<Guid> permissionIds)
		{
			var existing = await _context.RoleMenuPermission.Where(x => x.RoleId == id).ToListAsync();
			_context.RemoveRange(existing);

			foreach (var item in permissionIds)
			{
				await _context.RoleMenuPermission.AddAsync(new RoleMenuPermission()
				{
					RoleId = id,
					NavigationMenuId = item,
				});
			}

			var result = await _context.SaveChangesAsync();

			// Remove existing permissions to roles so it can re evaluate and take effect
			_cache.Remove("RolePermissions");

			return result > 0;
		}

		private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
		{
			var userId = GetUserId(ctx);
			var data = await (from role in _context.UserRoles
							  where role.UserId == userId
							  select role.RoleId).ToListAsync();
			return data;
		}

		private string GetUserId(ClaimsPrincipal user)
		{
			return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

        public async Task<bool> AddPartsModel(PartsModelViewModel2 viewModel)
        {
			var result = 0;
			var UpdateDataSet = await _context.MobileRNDPartsModels.Where(x => x.Id == viewModel.ID).FirstOrDefaultAsync();
			var existing = await _context.MobileRNDPartsModels.Where(x => x.ModelName == viewModel.Name).FirstOrDefaultAsync();

			if (UpdateDataSet == null)
			{
				if (existing != null)
				{
					return false;
				}
				else
				{
					_context.MobileRNDPartsModels.Add(new MobileRNDPartsModels()
					{
						ModelName = viewModel.Name,
						SupplierId = viewModel.SupplierId
					});

					result = await _context.SaveChangesAsync();
				}
			}
			else
			{
				UpdateDataSet.ModelName = viewModel.Name;
				UpdateDataSet.SupplierId = viewModel.SupplierId;

				_context.MobileRNDPartsModels.Update(UpdateDataSet);
				result = await _context.SaveChangesAsync();
			}

			return result > 0;
		}

        public  List<PartsModelViewModel> GetAllPartsModelList()
        {
			var items =  (from partModel in _context.MobileRNDPartsModels
						  join rm in _context.MobileRNDSupplier
						   on new { X1 = partModel.SupplierId } equals new { X1 = rm.Id }
						   into rmp
						  from rm in rmp.DefaultIfEmpty()
						  select new PartsModelViewModel()
							   {
								   ID = partModel.Id,
								   Name = partModel.ModelName,
								   SupplierId = partModel.SupplierId,
								   Supplier = rm.SupplierName
							   }).ToList();
			return items;
		}

        public  bool AddFaultsEntry(MobileRNDFaultsEntryViewModel viewModel)
        {
			var datetimetoday = DateTime.Today;

//			var existing = await _context.MobileRNDFaultsEntry.Where(x => x.Date == DateTime.Today).FirstOrDefaultAsync();


			if (viewModel == null)
            {
                return false;
            }
            else
            {
				_context.MobileRNDFaultsEntry.Add(new MobileRNDFaultsEntry()
				{
					Date = viewModel.Date,
					EmployeeID = viewModel.EmployeeID,
					LineNo = viewModel.LineNo,
					PartsModelID = viewModel.PartsModelID,
					LotNo = viewModel.LotNo,
					TotaCheckedQty = viewModel.TotalCheckedQty == null ? 0 : (int)viewModel.TotalCheckedQty,
					FuncMaterialFault = viewModel.FuncMaterialFault == null ? 0 : (int)viewModel.FuncMaterialFault,
					FuncProductionFault = viewModel.FuncProductionFault == null ? 0 : (int)viewModel.FuncProductionFault,
					FuncSoftwareFault = viewModel.FuncSoftwareFault == null ? 0 : (int)viewModel.FuncSoftwareFault,
					AesthMaterialFault = viewModel.AesthMaterialFault == null ? 0 : (int)viewModel.AesthMaterialFault,
					AesthProductionFault = viewModel.AesthProductionFault == null ? 0 : (int)viewModel.AesthProductionFault,
					TotalFunctionalFault = viewModel.TotalFunctionalFault,
					TotalAestheticFault = viewModel.TotalAestheticFault,
					UserID = viewModel.UserID
				});
            }
            var result =  _context.SaveChanges();
            return true;
		}

		public  bool UpdateFaultsEntry(MobileRNDFaultsEntryViewModel viewModel)
		{
			var datetimetoday = DateTime.Today;
			//var existing = null;
			//			var existing = await _context.MobileRNDFaultsEntry.Where(x => x.Date == DateTime.Today).FirstOrDefaultAsync();


			if (viewModel == null)
			{
				return false;
			}
			else
			{
				_context.MobileRNDFaultsEntry.Update(new MobileRNDFaultsEntry()
				{
					Id = (Guid)viewModel.Id,
					Date = viewModel.Date,
					EmployeeID = viewModel.EmployeeID,
					LineNo = viewModel.LineNo,
					PartsModelID = viewModel.PartsModelID,
					LotNo = viewModel.LotNo,
					TotaCheckedQty = viewModel.TotalCheckedQty == null ? 0 : (int)viewModel.TotalCheckedQty,
					FuncMaterialFault = (int)viewModel.FuncMaterialFault,
					FuncProductionFault = (int)viewModel.FuncProductionFault,
					FuncSoftwareFault = (int)viewModel.FuncSoftwareFault,
					AesthMaterialFault = (int)viewModel.AesthMaterialFault,
					AesthProductionFault = (int)viewModel.AesthProductionFault,
					TotalFunctionalFault = viewModel.TotalFunctionalFault,
					TotalAestheticFault = viewModel.TotalAestheticFault,
				});
			}
			var result =  _context.SaveChanges();
			return true;
		}

		public List<MobileRNDFaultsEntryViewModel> GetAllFaultsList(string EmployeeID)
        {
			if(EmployeeID == null)
            {
				EmployeeID = "";
            }
			var items = (from faults in _context.MobileRNDFaultsEntry
						 join model in _context.MobileRNDPartsModels
								on new { X1 = faults.PartsModelID } equals new { X1 = model.Id }
								into rmp
						 from rm in rmp.DefaultIfEmpty()
						 select new MobileRNDFaultsEntryViewModel()
						 {
							 Id = faults.Id,
							 Date = faults.Date,
							 EmployeeID = faults.EmployeeID,
							 //DateString =  faults.Date.ToString("MM/dd/yyyy"),
							 Line = "Line " + faults.LineNo,
							 ModelName = rm.ModelName,
							 ModelNameWithLot = rm.ModelName + "/" + faults.LotNo + " Lot",
							 TotalCheckedQty = faults.TotaCheckedQty,
							 FuncMaterialFault = faults.FuncMaterialFault,
							 FuncProductionFault = faults.FuncProductionFault,
							 FuncSoftwareFault = faults.FuncSoftwareFault,
							 TotalFunctionalFault = faults.TotalFunctionalFault,
							 AesthMaterialFault = faults.AesthMaterialFault,
							 AesthProductionFault = faults.AesthProductionFault,
							 TotalAestheticFault = faults.TotalAestheticFault,
							 CreateDate = faults.CreatedOn,
							 LineNo = faults.LineNo,
							 TotalFuncAes = (faults.TotalFunctionalFault + faults.TotalAestheticFault),
							 StatusIsToday = faults.Date == DateTime.Today ? true : false
						 }).Distinct().OrderBy(d => d.Date).ThenByDescending(x => x.TotalFuncAes).Where(p=> (EmployeeID == "" || p.EmployeeID == EmployeeID)).ToList();

			return items;
		}

		public List<MobileRNDFaultsEntryViewModel> SortableAllFaultsList(DateTime? startDate, DateTime? toDate, string lineNo, Guid ModelID, string lotNo, string EmployeeID, Guid SupplierId)
		{
			if (lineNo == null)
			{
				lineNo = "";
			}
			if (ModelID == null)
			{
				ModelID = Guid.Empty;
			}
			if (lotNo == null)
			{
				lotNo = "";
			}
			if (EmployeeID == null)
			{
				EmployeeID = "";
			}
			if(SupplierId == null)
            {
				SupplierId = Guid.Empty;
            }

			var items = (from faults in _context.MobileRNDFaultsEntry
						  join model in _context.MobileRNDPartsModels
								 on new { X1 = faults.PartsModelID } equals new { X1 = model.Id }
								 into rmp
						  from rm in rmp.DefaultIfEmpty()
						  select new MobileRNDFaultsEntryViewModel()
						  {
							  Id = faults.Id,
							  Date = faults.Date,
							  EmployeeID = faults.EmployeeID,
							  //DateString = faults.Date == null ? null : String.Format("{0:MM/dd/yyyy}", faults.Date),
							  Line = "Line " + faults.LineNo,
							  ModelName = rm.ModelName,
							  ModelNameWithLot = rm.ModelName + "/" + faults.LotNo + " Lot",
							  SupplierId = rm.SupplierId,
							  PartsModelID = faults.PartsModelID,
							  LotNo = faults.LotNo,
							  TotalCheckedQty = faults.TotaCheckedQty,
							  

							  FuncMaterialFault = faults.FuncMaterialFault,
							  FuncProductionFault = faults.FuncProductionFault,
							  FuncSoftwareFault = faults.FuncSoftwareFault,

							  FuncMaterialFaultd = faults.FuncMaterialFault == 0 || faults.TotaCheckedQty  == 0 ? 0 : ((double)((double)faults.FuncMaterialFault / (double)faults.TotaCheckedQty) * 100),
							  FuncProductionFaultd = faults.FuncProductionFault == 0 || faults.TotaCheckedQty == 0 ? 0 : ((double)(faults.FuncProductionFault / (double)faults.TotaCheckedQty) * 100),
							  FuncSoftwareFaultd = faults.FuncSoftwareFault == 0 || faults.TotaCheckedQty == 0 ? 0 : ((double)(faults.FuncSoftwareFault / (double)faults.TotaCheckedQty) * 100),

							  TotalFunctionalFault = faults.TotalFunctionalFault,
							  TotalFunctionalFaultd = faults.TotalFunctionalFault == 0 || faults.TotaCheckedQty == 0  ? 0 : ((double)((double)faults.TotalFunctionalFault / (double)faults.TotaCheckedQty) * 100),

							  AesthMaterialFault = faults.AesthMaterialFault,
							  AesthProductionFault = faults.AesthProductionFault,

							  AesthMaterialFaultd = faults.AesthMaterialFault == 0 || faults.TotaCheckedQty == 0 ? 0 : ((double)((double)faults.AesthMaterialFault / (double)faults.TotaCheckedQty) * 100),
							  AesthProductionFaultd = faults.AesthProductionFault == 0 || faults.TotaCheckedQty == 0 ? 0 : ((double)((double)faults.AesthProductionFault / (double)faults.TotaCheckedQty) * 100),

							  TotalAestheticFault = faults.TotalAestheticFault,
							  TotalAestheticFaultd = faults.TotalAestheticFault == 0 || faults.TotaCheckedQty == 0 ? 0 : ((double)((double)faults.TotalAestheticFault / (double)faults.TotaCheckedQty) * 100),

							  CreateDate = faults.CreatedOn,
							  LineNo = faults.LineNo,
							  TotalFuncAes = (faults.TotalFunctionalFault + faults.TotalAestheticFault),
							  StatusIsToday = faults.Date == DateTime.Today ? true : false
						  }).Distinct().OrderBy(d => d.Date).ThenByDescending(x => x.TotalFuncAes).Where(s => ((startDate == null && toDate == null) || (s.Date >= startDate && s.Date <= toDate)) && (lineNo == "" || s.LineNo == lineNo) && (ModelID == Guid.Empty || s.PartsModelID == ModelID) && (lotNo == "" || s.LotNo == lotNo) && (EmployeeID == "" || s.EmployeeID == EmployeeID) && (SupplierId == Guid.Empty || s.SupplierId == SupplierId)).ToList();

			return items;
		}

		public MobileRNDFaultsEntryViewModel GetFaults(Guid Id)
        {
			var items = (from faults in _context.MobileRNDFaultsEntry.Where(x => x.Id == Id) 
						 join model in _context.MobileRNDPartsModels
								on new { X1 = faults.PartsModelID } equals new { X1 = model.Id }
								into rmp
						 from rm in rmp.DefaultIfEmpty()
						 select new MobileRNDFaultsEntryViewModel()
						 {
							 Id = faults.Id,
							 EmployeeID = faults.EmployeeID,
							 Date = faults.Date,
							 DateString = String.Format("{0:MM/dd/yyyy}", faults.Date),
							 Line = "Line " + faults.LineNo,
							 LineNo = faults.LineNo,
							 LotNo = faults.LotNo,
							 ModelNameWithLot = rm.ModelName + "/" + faults.LotNo + " Lot",
							 TotalCheckedQty = faults.TotaCheckedQty,
							 FuncMaterialFault = faults.FuncMaterialFault,
							 FuncProductionFault = faults.FuncProductionFault,
							 FuncSoftwareFault = faults.FuncSoftwareFault,
							 TotalFunctionalFault = faults.TotalFunctionalFault,
							 AesthMaterialFault = faults.AesthMaterialFault,
							 AesthProductionFault = faults.AesthProductionFault,
							 TotalAestheticFault = faults.TotalAestheticFault,
							 PartsModelID = faults.PartsModelID,
							 Disabled = "disabled"
						 }).FirstOrDefault();
			return items;
		}

        public bool AddFaultsDetails(MobileRNDFaultDetailsViewModel viewModel)
        {
			//var datetimetoday = DateTime.Today;

			//			var existing = await _context.MobileRNDFaultsEntry.Where(x => x.Date == DateTime.Today).FirstOrDefaultAsync();

			if (viewModel == null)
			{
				return false;
			}
			else
			{
				_context.MobileRNDFaultDetails.Add(new MobileRNDFaultDetails()
				{
					Date = viewModel.Date,
					EmployeeID = viewModel.EmployeeID,
					FaultEntryID = viewModel.FaultEntryId,
					FaultType = viewModel.FaultType,
					FaultQty = viewModel.FaultQty,
					RootCause = viewModel.RootCause,
					Solution = viewModel.Solution,
					Remarks = viewModel.Remarks,
					ApplicationUserID = viewModel.UserID
				});
			}
			var result = _context.SaveChanges();

			return true;
		}

        public MobileRNDFaultsEntryViewModel GetSortedFaults(DateTime? sortdate, string lineNo, Guid ModelID, string lotNo)
        {
			var items = (from faults in _context.MobileRNDFaultsEntry.Where(x => x.Date == sortdate && x.LineNo == lineNo && x.PartsModelID == ModelID && x.LotNo == lotNo)
						 join model in _context.MobileRNDPartsModels
								on new { X1 = faults.PartsModelID } equals new { X1 = model.Id }
								into rmp
						 from rm in rmp.DefaultIfEmpty()
						 select new MobileRNDFaultsEntryViewModel()
						 {
							 Id = faults.Id,
							 EmployeeID = faults.EmployeeID,
							 Date = faults.Date,
							 DateString = String.Format("{0:MM/dd/yyyy}", faults.Date),
							 Line = "Line " + faults.LineNo,
							 LineNo = faults.LineNo,
							 LotNo = faults.LotNo,
							 ModelNameWithLot = rm.ModelName + "/" + faults.LotNo + " Lot",
							 TotalCheckedQty = faults.TotaCheckedQty,
							 FuncMaterialFault = faults.FuncMaterialFault,
							 FuncProductionFault = faults.FuncProductionFault,
							 FuncSoftwareFault = faults.FuncSoftwareFault,
							 TotalFunctionalFault = faults.TotalFunctionalFault,
							 AesthMaterialFault = faults.AesthMaterialFault,
							 AesthProductionFault = faults.AesthProductionFault,
							 TotalAestheticFault = faults.TotalAestheticFault,
							 PartsModelID = faults.PartsModelID,
							 Disabled = "disabled"
						 }).FirstOrDefault();
			return items;
		}

        //public MobileRNDFaultsEntryViewModel GetSortedFaults(DateTime sortdate, int lineNo, string ModelID, string lotNo)
        //{
        //    throw new NotImplementedException();
        //}

        public List<MobileRNDFaultDetails> GetSortedFaultsDetails(Guid Id)
        {
			//var items = (from fadt in _context.MobileRNDFaultDetails.Where(x => x.FaultEntryID == Id)
						 
			//			 select new MobileRNDFaultDetails()
			//			 {
			//				 FaultEntryID = fadt.FaultEntryID,
			//				 EmployeeID = fadt.EmployeeID,
			//				 Date = fadt.Date,
							 
			//			 }).ToList();
			var items = _context.MobileRNDFaultDetails.Where(x => x.FaultEntryID == Id).ToList();
			return items;
		}

        public  bool RemoveDetails(List<MobileRNDFaultDetails> Model)
        {
			_context.MobileRNDFaultDetails.RemoveRange(Model);
			var result =  _context.SaveChanges();

			if(result == 0)
            {
				return false;
			}
            else
            {
				return true;
			}
		}

        public List<MobileRNDFaultDetailsViewModel> GetFaultsDetails(Guid id)
        {
            var items = (from fadt in _context.MobileRNDFaultDetails.Where(x => x.FaultEntryID == id)

                         select new MobileRNDFaultDetailsViewModel()
                         {
                             FaultEntryId = fadt.FaultEntryID,
                             EmployeeID = fadt.EmployeeID,
                             Date = fadt.Date,
							 FaultType = fadt.FaultType,
							 FaultQty = fadt.FaultQty,
							 RootCause = fadt.RootCause,
							 Solution = fadt.Solution,
							 Remarks = fadt.Remarks

                         }).ToList();

			return items;
        }

        public PartsModelViewModel GetPartsModelList(Guid Id)
        {
			var items = (from partModel in _context.MobileRNDPartsModels.Where(x => x.Id == Id)

						 select new PartsModelViewModel()
						 {
							 ID = partModel.Id,
							 Name = partModel.ModelName,
							 SupplierId = partModel.SupplierId,
							 IsUpdate = "Update"
						 }).FirstOrDefault();

			return items;
		}

        public DashboasrViewModel GetDashboardData(DateTime? YesterdayDate, DateTime LastSevenDayStart, DateTime LastMonthDayStart)
        {
			var today = DateTime.Today;

			DashboasrViewModel data = new DashboasrViewModel();
			int LastDayTotalAes = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == YesterdayDate) select x.TotalAestheticFault).Sum();
			int LastDayTotalFunc = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == YesterdayDate) select x.TotalFunctionalFault).Sum();
			int TotalLastDay = LastDayTotalAes+ LastDayTotalFunc;

			int LastWeekTotalFunc = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastSevenDayStart && x.Date <= YesterdayDate) select x.TotalFunctionalFault).Sum();
			int LastWeekTotalAes = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastSevenDayStart && x.Date <= YesterdayDate) select x.TotalAestheticFault).Sum();
			int TotalLastWeek = LastWeekTotalFunc + LastWeekTotalAes;

			int LastMonthTotalFunc = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastMonthDayStart && x.Date <= YesterdayDate) select x.TotalFunctionalFault).Sum();
			int LastMonthTotalAes = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastMonthDayStart && x.Date <= YesterdayDate) select x.TotalAestheticFault).Sum();
			int TotalLastMonth = LastMonthTotalFunc + LastMonthTotalAes;

			int TodayTotalFunc = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == today) select x.TotalFunctionalFault).Sum();
			int TodayTotalAes = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == today) select x.TotalAestheticFault).Sum();
			int TotalToday = TodayTotalFunc + TodayTotalAes;

			data.LastDayTF = TotalLastDay;
			data.LastDayTFFunc = LastDayTotalFunc;
			data.LastDayTFAes = LastDayTotalAes;
			data.LastWeekTFFunc = LastWeekTotalFunc;
			data.LastWeekTFAes = LastWeekTotalAes;
			data.LastWeekTF = TotalLastWeek;
			data.LastMonthTFFunc = LastMonthTotalFunc;
			data.LastMonthTFAes = LastMonthTotalAes;
			data.LastMonthTF = TotalLastMonth;
			data.TodayTFFunc = TodayTotalFunc;
			data.TodayTFAes = TodayTotalAes;
			data.TodayTF = TotalToday;

			data.TodayTotalCheck = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == today) select x.TotaCheckedQty).Sum();
			data.LastDayTotalCheck = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date == YesterdayDate) select x.TotaCheckedQty).Sum();
			data.LastWeekTotalCheck = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastSevenDayStart && x.Date <= YesterdayDate) select x.TotaCheckedQty).Sum();
			data.LastMonthTotalCheck = (from x in _context.MobileRNDFaultsEntry.Where(x => x.Date >= LastMonthDayStart && x.Date <= YesterdayDate) select x.TotaCheckedQty).Sum();

			return data;
		}

        public DashboasrViewModel GetSingelDayData(DateTime? Date)
        {
			//if(Date == null)
   //         {
			//	Date = (DateTime)(from record in _context.MobileRNDFaultsEntry orderby record.Date select record.Date).Last();
			//}

			DashboasrViewModel data = new DashboasrViewModel();
			ChartLevelViewModel lavel = new ChartLevelViewModel();
			FunctionalFaultsPercentageViewModel FuncPercentage = new FunctionalFaultsPercentageViewModel();

			AestheticFaultsPercentageViewModel AesPercentage = new AestheticFaultsPercentageViewModel();

			lavel.LavelName = (from head in _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x=> x.LineNo)
							  join model in _context.MobileRNDPartsModels
										 on new { X1 = head.PartsModelID } equals new { X1 = model.Id }
										 into rmp
							  from rm in rmp.DefaultIfEmpty()
							  select ("L" + head.LineNo + ": " + rm.ModelName + "/" + head.LotNo)).ToArray();
			

			// Calculation for Bar Chart
			FuncPercentage.Material =  _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x => x.LineNo).Select(x => Math.Round((x.FuncMaterialFault == 0 || x.TotaCheckedQty == 0 ? 0 : ((double)((double)x.FuncMaterialFault / (double)x.TotaCheckedQty)*100)), 2)).ToArray();

			FuncPercentage.Production = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x => x.LineNo).Select(x => Math.Round((x.FuncProductionFault == 0 || x.TotaCheckedQty == 0 ? 0 : ((double)((double)x.FuncProductionFault / (double)x.TotaCheckedQty)*100)), 2)).ToArray();

			FuncPercentage.Software = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x => x.LineNo).Select(x => Math.Round((x.FuncSoftwareFault == 0 || x.TotaCheckedQty == 0 ? 0 : ((double)((double)x.FuncSoftwareFault / (double)x.TotaCheckedQty)*100)), 2)).ToArray();

			// Calculation for Pie Chart
			var TotalCheckedSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.TotaCheckedQty), 2)).Sum();

			var TotalFunctionalMaterialSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.FuncMaterialFault), 2)).Sum();
			var TotalFunctionalProductionSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.FuncProductionFault), 4)).Sum();
			var TotalFunctionalSoftwareSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.FuncSoftwareFault), 2)).Sum();


			// Calculation for Bar Chart 2
			AesPercentage.Material = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x => x.LineNo).Select(x => Math.Round((x.AesthMaterialFault == 0 || x.TotaCheckedQty == 0 ? 0 : ((double)((double)x.AesthMaterialFault / (double)x.TotaCheckedQty)*100)), 2)).ToArray();

			AesPercentage.Production = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).OrderBy(x => x.LineNo).Select(x => Math.Round((x.AesthProductionFault == 0 || x.TotaCheckedQty == 0 ? 0 : ((double)((double)x.AesthProductionFault / (double)x.TotaCheckedQty)*100)), 2)).ToArray();

			// Calculation for Pie Chart 2
			

			var TotalAesMaterialSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.AesthMaterialFault), 4)).Sum();
			var TotalAesProductionSum = _context.MobileRNDFaultsEntry.Where(x => x.Date == Date).Select(x => Math.Round(((double)x.AesthProductionFault), 4)).Sum();

			data.Lavel = lavel;
			data.FunctionalFaultsPercentageViewModel = FuncPercentage;
			data.AestheticFaultsPercentageViewModel = AesPercentage;

			//Functional
			data.FunctionalFaultsPercentageViewModel.TotalFuncMaterialFaultPercent = new double[] { 0};
			data.FunctionalFaultsPercentageViewModel.TotalFuncProductionFaultPercent = new double[] { 0};
			data.FunctionalFaultsPercentageViewModel.TotalFuncSoftwareFaultPercent = new double[] { 0};

			data.FunctionalFaultsPercentageViewModel.TotalFuncMaterialFaultPercent[0] = Math.Round((TotalFunctionalMaterialSum == 0 || TotalCheckedSum == 0 ? 0 :(double)((double)TotalFunctionalMaterialSum / (double)TotalCheckedSum)*100), 2);
			data.FunctionalFaultsPercentageViewModel.TotalFuncProductionFaultPercent[0] = Math.Round((TotalFunctionalProductionSum == 0 || TotalCheckedSum == 0 ? 0 : (double)((double)TotalFunctionalProductionSum / (double)TotalCheckedSum)*100), 2);
			data.FunctionalFaultsPercentageViewModel.TotalFuncSoftwareFaultPercent[0] = Math.Round((TotalFunctionalSoftwareSum == 0 || TotalCheckedSum == 0 ? 0 : (double)((double)TotalFunctionalSoftwareSum / (double)TotalCheckedSum)*100), 2);

			// Aesthetic
			data.AestheticFaultsPercentageViewModel.TotalAestheticProductionFaultPercent = new double[] { 0 };
			data.AestheticFaultsPercentageViewModel.TotalAestheticMaterialFaultPercent = new double[] { 0 };

			data.AestheticFaultsPercentageViewModel.TotalAestheticMaterialFaultPercent[0] = Math.Round((TotalFunctionalMaterialSum == 0 || TotalCheckedSum == 0 ? 0 : (double)((double)TotalAesMaterialSum / (double)TotalCheckedSum)*100), 2);
			data.AestheticFaultsPercentageViewModel.TotalAestheticProductionFaultPercent[0] = Math.Round((TotalFunctionalProductionSum == 0 || TotalCheckedSum == 0 ? 0 : (double)((double)TotalAesProductionSum / (double)TotalCheckedSum)*100), 2);

			return data;
		}

        public List<EmployeeListVM> GetAllEmployeeList()
        {
			var items = (from user in _context.Users

						 select new EmployeeListVM()
						 {
							 EmployeeID = user.EmployeeID,
							 EmployeeName = user.Name + " ("+ user.EmployeeID+")"
						 }).ToList();
			return items;
		}

        public List<DetailsReportViewModel> DetailsReportList(DateTime? Date)
        {
			var data = (from fadt in _context.MobileRNDFaultDetails.Where(x => x.Date == Date).DefaultIfEmpty()
						 join entry in _context.MobileRNDFaultsEntry on fadt.FaultEntryID equals entry.Id 
						 join model in _context.MobileRNDPartsModels on entry.PartsModelID equals model.Id
						 select new DetailsReportViewModel()
						 {
							 LineNo = entry.LineNo,
							 LineWithModel = "Assembly Line "+entry.LineNo+" Model: "+model.ModelName +"/"+entry.LotNo,
							 FaultType = fadt.FaultType,
							 FaultQty = fadt.FaultQty,
							 RootCause = fadt.RootCause,
							 Solution = fadt.Solution,
							 Remarks = fadt.Remarks

						 }).OrderBy(d => d.LineNo).ToList();
				return data;
		}

        public bool RemoveeNTRY(Guid Id)
        {
			var entry = _context.MobileRNDFaultsEntry.Where(x=> x.Id == Id).FirstOrDefault();
			_context.MobileRNDFaultsEntry.Remove(entry);
			var result =  _context.SaveChanges();
			if(result == 0)
            {
				return false;
            }
            else
            {
				return true;
			}
		}

        public DateTime LastDate()
        {
			return (DateTime)(from record in _context.MobileRNDFaultsEntry orderby record.Date select record.Date).Last();
		}

        public List<AutoCompleteViewModel> FaultsTypeAutoComplete(string Prefix, string type)
        {
			List<AutoCompleteViewModel> result = new List<AutoCompleteViewModel>();
			if (type == "FT")
            {
				result = (from fa in this._context.MobileRNDFaultDetails
						  where fa.FaultType.Contains(Prefix)
						  select new AutoCompleteViewModel
						  {
							  label = fa.FaultType,
							  val = fa.FaultType
						  }).Distinct().ToList();

				return result;
			}
			if (type == "RC")
			{
				result = (from fa in this._context.MobileRNDFaultDetails
						  where fa.RootCause.Contains(Prefix)
						  select new AutoCompleteViewModel
						  {
							  label = fa.RootCause,
							  val = fa.FaultType
						  }).Distinct().ToList();
				return result;
			}
			if (type == "SO")
			{
				result = (from fa in this._context.MobileRNDFaultDetails
						  where fa.Solution.Contains(Prefix)
						  select new AutoCompleteViewModel
						  {
							  label = fa.Solution,
							  val = fa.FaultType
						  }).Distinct().ToList();
				return result;
			}
			if (type == "RE")
			{
				result = (from fa in this._context.MobileRNDFaultDetails
						  where fa.Remarks.Contains(Prefix)
						  select new AutoCompleteViewModel
						  {
							  label = fa.Remarks,
							  val = fa.FaultType
						  }).Distinct().ToList();
				return result;
			}
			else
            {
				return result;
			}
		}

        public MobileRNDSupplier_VM GetSupplierList(Guid Id)
        {
			var items = (from sup in _context.MobileRNDSupplier.Where(x => x.Id == Id)

						 select new MobileRNDSupplier_VM()
						 {
							 Id = sup.Id,
							 SupplierName = sup.SupplierName,
							 IsUpdate = "Update"
						 }).FirstOrDefault();

			return items;
		}

        public async Task<bool> AddSupplier(MobileRNDSupplier_VM viewModel)
        {
			var result = 0;
			var UpdateDataSet = await _context.MobileRNDSupplier.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();
			var IsExist = await _context.MobileRNDSupplier.Where(x => x.SupplierName == viewModel.SupplierName).FirstOrDefaultAsync();
			if (UpdateDataSet == null)
			{
				if(IsExist == null)
                {
					_context.MobileRNDSupplier.Add(new MobileRNDSupplier()
					{
						SupplierName = viewModel.SupplierName,
					});
					result = await _context.SaveChangesAsync();
					return result > 0;
				}

                else
                {
					return false;
                }
				
			}
			else
			{
				if (IsExist == null)
				{
					UpdateDataSet.SupplierName = viewModel.SupplierName;

					_context.MobileRNDSupplier.Update(UpdateDataSet);
					result = await _context.SaveChangesAsync();
					return result > 0;
				}
                else
                {
					return false;
                }
			}
		}

        public List<MobileRNDSupplier_VM> GetAllSupplierList()
        {
			var items = (from sup in _context.MobileRNDSupplier
						 
						 select new MobileRNDSupplier_VM()
						 {
							 Id = sup.Id,
							 SupplierName = sup.SupplierName,
						 }).ToList();
			return items;
		}

        public PartsModelViewModel2 GetPartsModelList2(Guid Id)
        {
			var items = (from partModel in _context.MobileRNDPartsModels.Where(x => x.Id == Id)

						 select new PartsModelViewModel2()
						 {
							 ID = partModel.Id,
							 Name = partModel.ModelName,
							 SupplierId = partModel.SupplierId,
							 IsUpdate = "Update"
						 }).FirstOrDefault();

			return items;
		}
    }
}