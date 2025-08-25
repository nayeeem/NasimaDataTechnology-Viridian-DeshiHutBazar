using Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebDeshiHutBazar
{
    public class CompanyAddressViewModel
    {
        public CompanyAddressViewModel()
        {
        }

        public long CompanyAddressID { get; set; }

        public int StateID { get; set; }

        public string DisplayState { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string ZipCode { get; set; }

        public string HouseNo { get; set; }

        public string RoadNo { get; set; }

        public string Block { get; set; }

        public string ApartmentNo { get; set; }

        public string AddressDetails { get; set; }

        public string LandMark { get; set; }

        public IEnumerable<SelectListItem> AV_State { get; set; }
    }
}
