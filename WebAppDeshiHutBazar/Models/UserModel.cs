namespace WebDeshiHutBazar
{
    public class UserModel
    {
        public UserModel() {
        }

        public long UserID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsAdminUser { get; set; }

        public string ClientName { get; set; }       

        public bool IsVerifiedUser { get; set; }
    }
}
