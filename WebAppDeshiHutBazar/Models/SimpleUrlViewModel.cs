namespace WebDeshiHutBazar
{
    public class SimpleUrlViewModel 
    {
        public SimpleUrlViewModel()
        {          
        }
        public SimpleUrlViewModel(string url, long userId)  {
            UserID = userId;
            Url = url;
        }

        public string Url { get; set; }

        public long UserID { get; set; }

        public string Email { get; set; }

        public string ClientName { get; set; }
    }
}
