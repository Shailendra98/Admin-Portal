using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Constants;
using TKW.AdminPortal.DataAnnotations;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class NewRequestModel
    {
        //private string _Username;
        //[Required(ErrorMessage = "Username is required.")]
        //public string Username
        //{
        //    get
        //    {
        //        return _Username;
        //    }
        //    set
        //    {
        //        _Username = value;
        //        if (string.IsNullOrEmpty(_MobileNo) && Regex.IsMatch(_Username, @"(\d{10})")) _MobileNo = value;
        //    }
        //}

        //private string? _MobileNo;

        [RegularExpression(RegexPatterns.MobileNo, ErrorMessage = "Mobile number is invalid.")]
        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Mobile number is required.")]
        public string? MobileNo { get; set; }
        //public string? MobileNo { get { return _MobileNo; } set { _MobileNo = value; if (string.IsNullOrEmpty(_Username)) _Username = value; } }

        [RequiredIf("IsNewUser", true, ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [RequiredIf("IsNewAddress", false, ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public long? AddressId { get; set; }

        [RequiredIf("IsNewAddress", true, ErrorMessage = "Address is required.")]
        public AddressModel? Address { get; set; }

        //[Required(ErrorMessage = "Schedule is required.")]
        //public ScheduleModel Schedule { get; set; }

        private bool _IsNewUser;

        public bool IsNewUser
        {
            get
            {
                return _IsNewUser;
            }
            set
            {
                _IsNewUser = value;
                if (_IsNewUser) IsNewAddress = true;
            }
        }

        public bool IsNewAddress { get; set; }

        public string? Comment { get; set; }
    }
}
