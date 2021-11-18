using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class EditProfileModel : PageModel
    {
        private IUserService _userService;
        private IUserQueries _userQueries;

        public EditProfileModel(IUserService userService, IUserQueries userQueries)
        {
            _userQueries = userQueries;
            _userService = userService;

        }
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [BindProperty]
        [Display(Name = "Mobile Number")]
        public string? MobileNo { get; set; }

        [BindProperty]
        [Display(Name = "Picture")]
        public string? Picture { get; set; }

        [BindProperty]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public byte[] PictureBytes { get; private set; }

        public async Task OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(Id, cancellationToken);

            IsDone = false;
            Picture = User!.PictureName;
            Name = User!.Name;
            MobileNo = User!.MobileNo;
            Email = User.Email;
            Id = User.Id;

        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result1 = await _userService.UpdateNameAsync(Id, Name, cancellationToken);

                if (result1.IsSuccess)
                {
                    var result2 = await _userService.UpdateMobileNumberAsync(Id, MobileNo, cancellationToken);

                    if (result2.IsSuccess)
                    {
                        var result3 = await _userService.UpdateEmailAsync(Id, Email, cancellationToken);
                        if (result3.IsSuccess)
                        {
                            var result4 = await _userService.UploadProfilePictureAsync(Id, PictureBytes, cancellationToken);

                            if (result4.IsSuccess)
                            {

                                IsDone = true;
                                return Page();
                            }
                        }
                    }
                }
                ErrorMessage = result1.Error.Message;
            }
            return Page();
        }
    }
}
