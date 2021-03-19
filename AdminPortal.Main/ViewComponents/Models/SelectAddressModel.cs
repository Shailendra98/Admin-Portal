using System;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;

namespace TKW.AdminPortal.ViewComponents.Models
{
    public class SelectAddressModel
    {
        public long Id { get; set; }

        public string AddressLine { get; set; }

        public string AddressType { get; set; }

        public string Locality { get; set; }

        public string City { get; set; }

        public int Pincode { get; set; }

        public string State { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string MobileNo { get; set; }

        public string InputName { get; set; }

        public string ModalId { get; set; }

        public void SetAddressFields(UserAddressModel Address)
        {
            if (Address == null) return;

            Id = Address.Id;
            MobileNo = Address.MobileNo;
            Name = Address.Name;
            AddressLine = Address.AddressLine;
            AddressType = Address.Type.Name ;
            Locality = Address.LocalityName;
            City = Address.CityName;
            Pincode = Address.Pincode;
            State = Address.StateName;
            UserId = Address.UserId;
        }

        public SelectAddressModel() { }

        public SelectAddressModel(UserAddressModel Address, String InputName, String ModalId)
        {
            this.InputName = InputName;
            this.ModalId = ModalId;
            SetAddressFields(Address);
        }
    }
}
