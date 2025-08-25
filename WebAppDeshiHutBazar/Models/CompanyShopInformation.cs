namespace WebDeshiHutBazar
{
    public class CompanyShopInformation
    {
        public CompanyShopInformation()
        {
            ShopAddress = new CompanyAddressViewModel();
        }

        public string ShopContactPerson { get; set; }

        public string ShopContactEmail { get; set; }

        public string ShopContactPhone { get; set; }

        public CompanyAddressViewModel ShopAddress { get; set; }
    }
}
