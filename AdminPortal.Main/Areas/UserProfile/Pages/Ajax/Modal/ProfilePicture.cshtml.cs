using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Identity;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class ProfilePictureModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private IUserService _userService;
        private IUserQueries _userQueries;

        public ProfilePictureModel(IUserService userService, IUserQueries userQueries)
        {
            _userQueries = userQueries;
            _userService = userService;
        }
        public string ErrorMessage { get; set; }

        public bool IsDone { get; set; }

        [BindProperty(SupportsGet = true)]
        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(Id, cancellationToken);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var User = await _userQueries.UserByIdAsync(Id, cancellationToken);

                using var ms = new MemoryStream();
                Photo.CopyTo(ms);
                var result = await _userService.UploadProfilePictureAsync(Id, ms.ToArray(), cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                }
                else
                    ErrorMessage = result.Error.Message;
            }
            return Page();
        }
    }
}
