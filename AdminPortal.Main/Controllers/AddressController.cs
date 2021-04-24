using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Responses;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.Area;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAreaQueries _areaQueries;
        private readonly IAppUserService _appUser;

        public AddressController(IAreaQueries areaQueries, IAppUserService appUser)
        {
            _areaQueries = areaQueries;
            _appUser = appUser;
        }

        [Authorize(Policy = AuthorizationPolicies.GlobalAdminRoles)]
        [Route("~/ajax/address/search/cities")]
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> Cities(string q, int? page, CancellationToken cancellationToken)
        {
            var cities = await _areaQueries.SearchCitiesAsync(q, page ?? 1, 10, cancellationToken);
            // if (cities == null) return null;
            return Json(new PagedJsonResponse<CityWithStateModel>
            {
                Data = cities,
                Page = cities.PageNumber,
                Size = cities.PageSize,
                Total = cities.TotalItemCount
            });
        }

        [Authorize(Policy = AuthorizationPolicies.GlobalAdminRoles)]
        [Route("~/ajax/address/search/pincodes")]
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> Pincodes(string q, int? page, CancellationToken cancellationToken)
        {
            var pincodes = await _areaQueries.SearchPincodesAsync(q, page ?? 1, 10, cancellationToken);
            return Json(new PagedJsonResponse<PincodeModel>
            {
                Data = pincodes,
                Page = pincodes.PageNumber,
                Size = pincodes.PageSize,
                Total = pincodes.TotalItemCount
            });

        }

        [Route("~/ajax/address/search/localities")]
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult?> Localities(string q, int? cityId, int? page, CancellationToken cancellationToken)
        {
            PagedList<LocalityModel> Localities;
            int? FranchiseId = _appUser.Current?.FranchiseId;
            if (FranchiseId != null)
                Localities = await _areaQueries.SearchLocalitiesOfFranchiseAsync(q, FranchiseId ?? 0, page ?? 1, 10, cancellationToken);
            else if (cityId != null)
                Localities = await _areaQueries.SearchLocalitiesOfCityAsync(q, cityId ?? 0, page ?? 1, 10, cancellationToken);
            else return null;
            return Json(new PagedJsonResponse<LocalityModel>
            {
                Data = Localities,
                Page = Localities.PageNumber,
                Size = Localities.PageSize,
                Total = Localities.TotalItemCount
            });
        }
    }
}