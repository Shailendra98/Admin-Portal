using Microsoft.Extensions.Options;
using TKW.AdminPortal.Constants;
using TKW.ApplicationCore.Settings;

namespace TKW.AdminPortal.Utils
{
    public class UrlGenerator
    {
        private PhotoSetting _options;

        public UrlGenerator(IOptions<PhotoSetting> options)
        {
            _options = options.Value;
        }

        public string MaterialPicUrlPrefix { get => _options.GetMaterialUrlPrefix; }

        public string RequestPicUrlPrefix { get => _options.GetRequestUrlPrefix; }

        public string ProfilePicUrlPrefix { get => _options.GetProfileUrlPrefix; }

        public string ExpensePicUrlPrefix { get => _options.GetExpenseUrlPrefix; }

        public string SellBillPicUrlPrefix { get => _options.GetSellUrlPrefix; }

        public string OdometerPicUrlPrefix { get => _options.GetVehicleOdometerPrefix; }

        public string MaterialPic(string PictureName, MatPicSize size)
        {
            if (string.IsNullOrWhiteSpace(PictureName))
            {
                switch (size)
                {
                    case MatPicSize.x64: return "/images/default/materialx64.png";
                    case MatPicSize.x128: return "/images/default/materialx128.png";
                    default: return "/images/default/materialx192.png";
                }
            }
            else
                switch (size)
                {
                    case MatPicSize.x64: return _options.GetMaterialUrlPrefix + "x64/" + PictureName;
                    case MatPicSize.x128: return _options.GetMaterialUrlPrefix + "x128/" + PictureName;
                    default: return _options.GetMaterialUrlPrefix + "x192/" + PictureName;
                }
        }

        public string ProfilePic(string? pictureName, UserPicSize size)
        {
            if (string.IsNullOrWhiteSpace(pictureName))
            {
                switch (size)
                {
                    case UserPicSize.x80:
                        return "/images/default/profilex80.png";
                    case UserPicSize.x124:
                        return "/images/default/profilex124.png";
                    default:
                        return "/images/default/profilex240.png";
                }
            }
            else
            {
                switch (size)
                {
                    case UserPicSize.x80:
                        return _options.GetProfileUrlPrefix + "x80/" + pictureName;
                    case UserPicSize.x124:
                        return _options.GetProfileUrlPrefix + "x124/" + pictureName;
                    default:
                        return _options.GetProfileUrlPrefix + pictureName;
                }
            }
        }

        public string RequestPic(string PictureName)
        {
            if (string.IsNullOrEmpty(PictureName)) return null;
            return _options.GetRequestUrlPrefix + PictureName;
        }
        public string ExpensePic(string PictureName)
        {
            if (string.IsNullOrEmpty(PictureName)) return null;
            return _options.GetExpenseUrlPrefix + PictureName;
        }
        public string SellBillPic(string PictureName)
        {
            if (string.IsNullOrEmpty(PictureName)) return null;
            return _options.GetSellUrlPrefix + PictureName;
        }
        public string OdometerPic(string PictureName)
        {
            if (string.IsNullOrEmpty(PictureName)) return null;
            return _options.GetVehicleOdometerPrefix + PictureName;
        }
    }
}
