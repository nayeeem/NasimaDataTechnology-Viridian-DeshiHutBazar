namespace Model
{
    using System.Data.Entity.Migrations;
    using Common;   

    internal sealed class Configuration : DbMigrationsConfiguration<Model.WebDeshiHutBazarEntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Model.WebDeshiHutBazarEntityContext";
        }

        private void UserSeed(WebDeshiHutBazarEntityContext context)
        {
            HashingCryptographyService _hashingService = new HashingCryptographyService();
            var passwordVM = _hashingService.GetMessageDigest("12348765");
            context.Users.Add(new User("naimul.prodhan@hotmail.com",
                passwordVM.Digest,
                "7898",
                true,
                "Technical Department",
                "Super Admin",
                passwordVM.Salt,
                EnumCountry.Bangladesh)
            {
                AdminPersonalEmail = "naimul.prodhan@hotmail.com",
                IsUserBlocked = false,
                Roles = "SuperAdmin",
                IsVerifiedAccount = true
            });
            //context.Users.Add(new User("seed@nasimatech.com",
            //    passwordVM.Digest,
            //    "7898",
            //    true,
            //    "Technical Department",
            //    "Admin",
            //    passwordVM.Salt,
            //    EnumCountry.Bangladesh)
            //{
            //    AdminPersonalEmail = "naimul.prodhan@gmail.com",
            //    IsUserBlocked = false,
            //    Roles = "Admin"
            //});
            //context.Users.Add(new User("hafsa@gmail.com",
            //    passwordVM.Digest,
            //    "7898",
            //    true,
            //    "Content Manager",
            //    "Content Admin",
            //    passwordVM.Salt,
            //    EnumCountry.Bangladesh)
            //{
            //    AdminPersonalEmail = "naimul.prodhan@gmail.com",
            //    IsUserBlocked = false,
            //    Roles = "Admin"
            //});

            //context.Users.Add(new User("nnaim@gmail.com",
            //    passwordVM.Digest, 
            //    "HSO", EnumUserAccountType.Company, passwordVM.Salt, 
            //    EnumCountry.Bangladesh)
            //{
            //    IsUserBlocked = false,
            //    Roles = "Company"
            //});

            context.SaveChanges();

            //var user = context.Users.FirstOrDefault(a => a.Email == "hafsa@gmail.com");

            //context.Companies.Add(new Company()
            //{
            //    EnumCountry = EnumCountry.Bangladesh,
            //    CompanyAddress = new CompanyAddress(EnumCountry.Bangladesh,1,"Dhaka", "1205")
            //    {
            //      Area = "Dhanmondi"                 
            //    },
            //    ShopAddress = new CompanyAddress(EnumCountry.Bangladesh, 1, "Dhaka", "1205")
            //    {
            //        Area = "Dhanmondi"
            //    },
            //    CompanyWebsite = "www.deshihutbazar.com",
            //    CompanyName = "NDT",
            //    CompanyPhone = "01765805853",
            //    ShopContactName = "Naim",
            //    ShopContactEmail = "naimul.prodhan@gmail.com",
            //    ShopContactPhoneNumber = "01765805853",
            //    UserID = user.UserID
            //});

            //context.SaveChanges();
        }

        protected override void Seed(WebDeshiHutBazarEntityContext context)
        {
            UserSeed(context);           
        }
    }
}
