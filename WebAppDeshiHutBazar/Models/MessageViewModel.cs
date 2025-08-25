using Common;
using System;
namespace WebDeshiHutBazar
{    
    public class MessageViewModel : BaseViewModel
    {
        public MessageViewModel()
        {
        }

        public long MessageID { get; set; }
      
        public long SenderUserID { get; set; }

        public long ReceiverUserID { get; set; }

        public string Message { get; set; }

        public string ClientName { get; set; }

        public string Email { get; set; }

        public long ParentMessageID { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }

        public string PosterEmail { get; set; }

        public DateTime DateMsgSent { get; set; }

        public string DateMsgSentString { get; set; }

        public string SenderEmail { get; set; }

        public string ReceiverEmail { get; set; }

        public string SenderClientName { get; set; }

        public string ReceiverClientName { get; set; }

        public bool IsNewMessage { get; set; }

        public long PostID { get; set; }

        public string Phone { get; set; }
    }
}
