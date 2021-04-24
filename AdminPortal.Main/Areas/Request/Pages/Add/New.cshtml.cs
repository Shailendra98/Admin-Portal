using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Constants;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Request.Pages.Add
{
    public class NewModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IAppUserService _appUser;
        private readonly IUserQueries _userQueries;
        private readonly IRequestQueries _requestQueries;
        private readonly IUserService _userService;
        private readonly IAreaQueries _areaQueries;
        private readonly IMaterialQueries _materialQueries;
        public NewModel(IRequestService requestService, IMaterialQueries materialQueries, IAppUserService appUser, IUserQueries userQueries, IRequestQueries requestQueries, IUserService userService, IAreaQueries areaQueries)
        {
            _requestService = requestService;
            _materialQueries = materialQueries;
            _appUser = appUser;
            _userQueries = userQueries;
            _requestQueries = requestQueries;
            _userService = userService;
            _areaQueries = areaQueries;
        }

        [BindProperty]
        [Required(ErrorMessage = "Request details is missing.")]
        public ViewModels.NewRequestModel NewRequestModel { get; set; }

        [BindProperty]
        public List<MaterialRateQuantityInputModel>? Materials { get; set; }

        [BindProperty]
        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Schedule is required.")]
        public ScheduleModel ScheduleModel { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public List<RequestModel> PendingRequests { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string mobileNo, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(mobileNo) || !Regex.IsMatch(mobileNo, RegexPatterns.MobileNo)) return RedirectToPage("Index");
            NewRequestModel = new ViewModels.NewRequestModel
            {
                MobileNo = mobileNo
            };
            var user = await _userQueries.UserByMobileNumberAsync(mobileNo, cancellationToken);
            if (user != null)
            {
                NewRequestModel.IsNewUser = false;
                NewRequestModel.MobileNo = user.MobileNo;
                NewRequestModel.Name = user.Name;
                NewRequestModel.AddressId = user.DefaultAddressId;
                NewRequestModel.IsNewAddress = (!NewRequestModel.AddressId.HasValue || NewRequestModel.AddressId == 0);
                PendingRequests = await _requestQueries.PendingRequestsOfSellerMinAsync(user.Id, cancellationToken);
            }
            else
            {
                NewRequestModel.IsNewUser = true;
            }
            if (NewRequestModel.IsNewAddress) { NewRequestModel.Address = new AdminPortal.ViewModels.AddressModel { IncludeNameMobileNo = false, OnlyLocalities = _appUser.Current.FranchiseId.HasValue }; }
            ScheduleModel = new ScheduleModel();
            var availableSchedule = await _requestQueries.GetAvailableScheduleForManagerAsync(cancellationToken);
            ScheduleModel.AvailableSchedule = new AdminPortal.ViewModels.AvailableScheduleModel
            {
                Slots = availableSchedule.Slots.ToDictionary(m => m.Id, m => m.Text),
                Dates = availableSchedule.Dates.Select(m => new AvailableDateAndSlotModel { Date = m.Date, SlotIds = m.SlotIds }).ToList()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var user = await _userQueries.UserByMobileNumberAsync(NewRequestModel.MobileNo, cancellationToken);
            int userId = 0;
            if (ModelState.IsValid)
            {
                var isValid = true;
                if (user == null)
                {
                    var random = new Random();
                    var r = await _userService.CreateUserAsync(
                        NewRequestModel.Name,
                        NewRequestModel.MobileNo,
                        UserRole.Seller,
                        NewRequestModel.Address.AddressLine,
                        NewRequestModel.Address.LocalityId.Value,
                        Enumeration.TryFromValue<AddressType>(NewRequestModel.Address.AddressTypeId.Value) ?? AddressType.Home,
                        NewRequestModel.Address.Latitude,
                        NewRequestModel.Address.Longitude,
                        null,
                        cancellationToken);
                    if (r.IsFailure)
                    {
                        ErrorMessage = r.Error.Message;
                        isValid = false;
                    }
                    else
                    {
                        userId = r.Value.user.Id;
                        NewRequestModel.AddressId = r.Value.address.Id;
                        NewRequestModel.IsNewUser = false;
                        NewRequestModel.IsNewAddress = false;
                    }
                }
                else
                {
                    userId = user.Id;
                    if (!NewRequestModel.AddressId.HasValue)
                    {
                        var addressResult = await _userService.AddUserAddressAsync(user.Id, NewRequestModel.Address.AddressLine, NewRequestModel.Address.LocalityId.Value, NewRequestModel.Address.AddressTypeId.Value, NewRequestModel.Address.Name, NewRequestModel.Address.MobileNo, NewRequestModel.Address.Latitude, NewRequestModel.Address.Longitude, cancellationToken);
                        if (addressResult.IsSuccess)
                        {
                            NewRequestModel.AddressId = addressResult.Value.Id;
                            NewRequestModel.IsNewAddress = false;
                        }
                        else
                        {
                            ErrorMessage = addressResult.Error.Message;
                            isValid = false;
                        }
                    }
                }

                if (isValid)
                {

                    var requestResult = await _requestService.AddRequestAsync(userId, NewRequestModel.AddressId.Value,
                                                                              SourceApp.AdminPortal,
                                                                              ScheduleModel.Date.Value,
                                                                              ScheduleModel.TimeSlotId.Value,
                                                                              Materials == null ? null : Materials.Where(m => m.Id.HasValue).Select(m => (m.Id!.Value, m.Rate))
                                                                              , null,
                                                                              NewRequestModel.Comment,
                                                                              cancellationToken: cancellationToken);

                    if (requestResult.IsSuccess) return RedirectToPage("../Details", new { Id = requestResult.Value.Id });

                    if (requestResult.Error is RequestAlreadySubmittedError e) return RedirectToPage("../Details", new { Id = e.RequestId });
                    else ErrorMessage = requestResult.Error.Message;
                }
            }
            if (user != null)
            {
                PendingRequests = await _requestQueries.PendingRequestsOfSellerMinAsync(user.Id, cancellationToken);
                NewRequestModel.Name = user.Name;
                NewRequestModel.MobileNo = user.MobileNo;
            }
            var availableSchedule = await _requestQueries.GetAvailableScheduleForManagerAsync(cancellationToken);
            ScheduleModel.AvailableSchedule = new AdminPortal.ViewModels.AvailableScheduleModel
            {
                Slots = availableSchedule.Slots.ToDictionary(m => m.Id, m => m.Text),
                Dates = availableSchedule.Dates.Select(m => new AvailableDateAndSlotModel { Date = m.Date, SlotIds = m.SlotIds }).ToList()
            };
            if (NewRequestModel.IsNewAddress)
            {
                if (NewRequestModel.Address != null)
                {
                    NewRequestModel.Address.OnlyLocalities = _appUser.Current.FranchiseId.HasValue;
                    NewRequestModel.Address.IncludeNameMobileNo = false;
                }
                else
                {
                    NewRequestModel.Address = new AdminPortal.ViewModels.AddressModel
                    {
                        OnlyLocalities = _appUser.Current.FranchiseId.HasValue,
                        IncludeNameMobileNo = false
                    };
                }
            }
            if (Materials != null && Materials.Count > 0)
            {
                var materials = await _materialQueries.ActivePurchaseMaterialsAsync(Materials.Where(m => m.Id.HasValue).Select(m => m.Id.Value), cancellationToken);
                foreach (var m in Materials)
                {
                    m.Name = materials.FirstOrDefault(a => a.Id == m.Id)?.Name ?? string.Empty;
                }
            }
            return Page();
        }
    }
}