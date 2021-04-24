using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TKW.AdminPortal.Extensions;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Franchise;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Utils
{
    public class SessionData
    {
        private IAppUserService _appUser;

        private readonly IUserQueries _userQueries;

        private ISession _session;

        private IFranchiseQueries _franchiseQueries;

        public SessionData(IAppUserService appUser, IUserQueries userQueries, IHttpContextAccessor httpContextAccessor, IFranchiseQueries franchiseQueries)
        {
            _appUser = appUser;
            _userQueries = userQueries;
            _session = httpContextAccessor.HttpContext.Session;
            _franchiseQueries = franchiseQueries;
        }

        private UserModel? _user;

        private List<FranchiseModel> _franchises;

        public async Task<UserModel?> GetUser()
        {
            if (_appUser.Current == null) return null;
            if (_user != null) return _user;
            _user = _session.GetUser();
            if (_user != null) return _user;
            _user = await _userQueries.UserByIdAsync(_appUser.Current.Id);
            _session.SetUser(_user);
            return _user;
        }

        public async Task<List<FranchiseModel>> GetFranchises()
        {
            if (_franchises != null) return _franchises;
            _franchises = _session.GetFranchises();
            if (_franchises != null) return _franchises;
            _franchises = await _franchiseQueries.AllFranchisesAsync();
            _session.SetFranchises(_franchises);
            return _franchises;
        }
    }
}
