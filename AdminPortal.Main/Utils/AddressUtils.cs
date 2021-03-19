using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Utils
{
    public static class AddressUtils
    {
        public static string GenerateHTML(string addressLine,string type , string locality, string city,string state,string pincode,  string name=null,string mobileNo=null)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(name))
            {
                address = name;
                if (!string.IsNullOrWhiteSpace(mobileNo))
                    address += " (" + mobileNo + ")";
                if (!string.IsNullOrWhiteSpace(type))
                    address += " - " + type;
                address += "<br/>";
            }
            else if (!string.IsNullOrWhiteSpace(mobileNo))
            {
                address = "Contact: " + mobileNo;
                if (!string.IsNullOrWhiteSpace(type))
                    address += " - " + type;
                address += "<br/>";
            }
            else if (!string.IsNullOrWhiteSpace(type))
                address += type + "<br/>";
            address += addressLine + ", " ;
            address += locality;
            
            if (!string.IsNullOrWhiteSpace(city))
            {
                address += "<br/>" + city;
                if (!string.IsNullOrWhiteSpace(state))
                    address += ", " + state;
            }
            else if (!string.IsNullOrWhiteSpace(state))
                address += "<br/>" + state;
            if (!string.IsNullOrWhiteSpace(pincode))
                address += " - " + pincode;
            return address;
        }
    }
}
