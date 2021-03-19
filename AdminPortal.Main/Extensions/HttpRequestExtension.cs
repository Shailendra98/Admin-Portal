﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Extensions
{
    public static class HttpRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        public static bool IsMobileDevice(this HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString();
            return userAgent.Contains("Mobi");
        }
    }
}
