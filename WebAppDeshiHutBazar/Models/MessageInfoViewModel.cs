using Common;
using System.Collections.Generic;
namespace WebDeshiHutBazar
{    
    public class MessageInfoViewModel : BaseViewModel
    {
        public MessageInfoViewModel()
        {
            ListMsgInbox = new List<MessageViewModel>();
            ListMsgSent = new List<MessageViewModel>();
        }

        public List<MessageViewModel> ListMsgInbox { get; set; }

        public List<MessageViewModel> ListMsgSent { get; set; }

        public long UserId { get; set; }
    }
}
