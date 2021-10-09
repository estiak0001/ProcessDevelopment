using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppEs.Entity;
using WebAppEs.Models;
using WebAppEs.Services;
using WebAppEs.ViewModel.FaultsEntry;

namespace WebAppEs.Controllers
{
    public class AddFaultsInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDataAccessService _dataAccessService;
        private readonly ILogger<AdminController> _logger;

        public AddFaultsInfoController(
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
        public IActionResult Index()
        {
            var employeeID = HttpContext.Session.GetString("EmployeeID");
            if (employeeID == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            ClaimsPrincipal currentUser = this.User;
            var ID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isSuperAdmin = currentUser.IsInRole("SuperAdmin");
            bool isAdmin = currentUser.IsInRole("Admin");
           
            if (isSuperAdmin == true || isAdmin == true)
            {
                MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();
                var data = _dataAccessService.GetAllFaultsList("");
                viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
                viewmodel.MobileRNDFaultsEntryViewModelList = data;
                return View(viewmodel);
            }
            else
            {
                MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();
                var data = _dataAccessService.GetAllFaultsList(employeeID);
                viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
                viewmodel.MobileRNDFaultsEntryViewModelList = data;
                return View(viewmodel);
            }
        }

        [Authorize("Authorization")]
        public IActionResult CreateFaultsInfo(Guid Id)
        {
            var employeeID = HttpContext.Session.GetString("EmployeeID");
            if (employeeID == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();

            if (Id == Guid.Empty)
            {
                viewmodel.Date = DateTime.Today;
                
                viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
                viewmodel.ButtonText = "Submit";
                return View(viewmodel);
            }
            else
            {
                viewmodel = _dataAccessService.GetFaults(Id);
                viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
                viewmodel.MobileRNDFaultDetailsViewModel = _dataAccessService.GetFaultsDetails(Id);
                viewmodel.ButtonText = "Update";
                return View(viewmodel);
            }
        }

        [Authorize("Authorization")]
        [HttpPost]
        public  IActionResult CreateFaultsInfoAsync(MobileRNDFaultsEntryViewModel viewmodel)
        {
            var employeeID = HttpContext.Session.GetString("EmployeeID");
            
            var IsSubmit = false;
            var ExistData = _dataAccessService.GetFaults(viewmodel.Id);
            
            if (employeeID == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            ClaimsPrincipal currentUser = this.User;
            var ID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid newGuid = Guid.Parse(ID);
            bool isSuperAdmin = currentUser.IsInRole("SuperAdmin");
            bool isAdmin = currentUser.IsInRole("Admin");


            viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
            //var employeeID = HttpContext.Session.GetString("EmployeeID");
            if (ModelState.IsValid)
            {
                if(ExistData == null)
                {
                    var head = _dataAccessService.GetSortedFaults(viewmodel.Date, viewmodel.LineNo, viewmodel.PartsModelID, viewmodel.LotNo);
                    if(head != null)
                    {
                        ModelState.AddModelError("Date", "This Line and Model Data Already Exist! Please Update from List.");
                    }
                    else
                    {
                        viewmodel.TotalFunctionalFault = (int)((viewmodel.FuncMaterialFault == null ? 0 : viewmodel.FuncMaterialFault) + (viewmodel.FuncProductionFault == null ? 0 : viewmodel.FuncProductionFault) + (viewmodel.FuncSoftwareFault == null ? 0 : viewmodel.FuncSoftwareFault));
                        viewmodel.TotalAestheticFault = (int)((viewmodel.AesthMaterialFault == null ? 0 : viewmodel.AesthMaterialFault) + (viewmodel.AesthProductionFault == null ? 0 : viewmodel.AesthProductionFault));
                        viewmodel.EmployeeID = employeeID;
                        viewmodel.UserID = newGuid;
                        IsSubmit =  _dataAccessService.AddFaultsEntry(viewmodel);
                    }
                }
                else
                {
                    viewmodel.TotalFunctionalFault = (int)(viewmodel.FuncMaterialFault + viewmodel.FuncProductionFault + viewmodel.FuncSoftwareFault);
                    viewmodel.TotalAestheticFault = (int)(viewmodel.AesthMaterialFault + viewmodel.AesthProductionFault);
                    viewmodel.EmployeeID = ExistData.EmployeeID;
                    viewmodel.Date = ExistData.Date;
                    IsSubmit =  _dataAccessService.UpdateFaultsEntry(viewmodel);
                }

                if (IsSubmit)
                {
                    return RedirectToAction("Index", "AddFaultsInfo", null);
                }
                else
                {
                    ModelState.AddModelError("Date", "This Line and Model Data Already Exist! Please Update from List.");
                    return View(viewmodel);
                }
            }
            ModelState.AddModelError("Date", "Please Select Line Model and Lot to Submit!");
            return View(viewmodel);

            //MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();
            //viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();

            //return View(viewmodel);
        }

        public IActionResult FaultsDetails()
        {
            var employeeID = HttpContext.Session.GetString("EmployeeID");
            if (employeeID == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();
            viewmodel.Date = DateTime.Today;
            viewmodel.PartsModelViewModel = _dataAccessService.GetAllPartsModelList();
            return View(viewmodel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        public JsonResult FaultsDetailsAdd([FromBody]  MobileRNDFaultsEntryViewModel model)
        {
            ClaimsPrincipal currentUser = this.User;
            var ID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid newGuid = Guid.Parse(ID);
            bool isSuperAdmin = currentUser.IsInRole("SuperAdmin");
            bool isAdmin = currentUser.IsInRole("Admin");

            var HedSubmit = false;
            var DetailsSubmit = false;
            //bool status = false;
            StatusModel status = new StatusModel();
            status.success = false;
            var employeeID = HttpContext.Session.GetString("EmployeeID");

            if (ModelState.IsValid)
            {
                var head = _dataAccessService.GetSortedFaults(model.Date, model.LineNo, model.PartsModelID, model.LotNo);
                if (head == null)
                {
                    model.EmployeeID = employeeID;
                    HedSubmit =  _dataAccessService.AddFaultsEntry(model);
                    if(HedSubmit)
                    {
                        var Savedhead = _dataAccessService.GetSortedFaults(model.Date, model.LineNo, model.PartsModelID, model.LotNo);
                        
                        foreach (var it in model.MobileRNDFaultDetailsViewModel)
                        {
                            it.Date = Savedhead.Date;
                            it.EmployeeID = Savedhead.EmployeeID;
                            it.FaultEntryId = Savedhead.Id;
                            it.UserID = newGuid;
                            var IsSubmit =  _dataAccessService.AddFaultsDetails(it);
                        }
                    }
                }
                else
                {
                    var details = _dataAccessService.GetSortedFaultsDetails(head.Id);
                    if(details != null)
                    {
                        var isDelete = _dataAccessService.RemoveDetails(details);
                    }
                    
                    foreach (var it in model.MobileRNDFaultDetailsViewModel)
                    {
                        it.Date = head.Date;
                        it.EmployeeID = head.EmployeeID;
                        it.FaultEntryId = head.Id;
                        it.UserID = newGuid;
                        DetailsSubmit =  _dataAccessService.AddFaultsDetails(it);
                    }
                }
            }
            else
            {
                status.success = false;
            }

            if(DetailsSubmit)
            {
                status.success = true;
            }

            return Json(status);
        }

        public IActionResult FullPreview(Guid Id)
        {
            var employeeID = HttpContext.Session.GetString("EmployeeID");
            if (employeeID == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            MobileRNDFaultsEntryViewModel viewmodel = new MobileRNDFaultsEntryViewModel();
            var Head = _dataAccessService.GetFaults(Id);
            var Details = _dataAccessService.GetFaultsDetails(Id);
            viewmodel = Head;
            viewmodel.MobileRNDFaultDetailsViewModel = Details;
            
            return View(viewmodel);
        }


        [HttpPost]
        public JsonResult LoadFaultsDetails([FromBody] MobileRNDFaultsEntryViewModel model)
        {
            var head = _dataAccessService.GetSortedFaults(model.Date, model.LineNo, model.PartsModelID, model.LotNo);
            var details = _dataAccessService.GetSortedFaultsDetails(head.Id);

            return Json(details);
        }



        public JsonResult LoadFullSetData(DateTime Date, string LineNo, Guid PartsModelID, string LotNo)
        {
            MobileRNDFaultsEntryViewModel data = new MobileRNDFaultsEntryViewModel();
            var head = _dataAccessService.GetSortedFaults(Date, LineNo, PartsModelID, LotNo);
            if(head != null)
            {
                var details = _dataAccessService.GetFaultsDetails(head.Id);
                head.MobileRNDFaultDetailsViewModel = details;
            }
            
            if(head!=null)
            {
                data = head;
            }
            else
            {
                data = null;
            }

            return Json(data);
        }

        [Authorize("Authorization")]
        public IActionResult Delete(Guid id)
        {
            var result = _dataAccessService.RemoveeNTRY(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult FaultsTypeAutoComplete(string prefix, string type)
        {
            var result = _dataAccessService.FaultsTypeAutoComplete(prefix, type);

            return Json(result);
        }
    }
}
