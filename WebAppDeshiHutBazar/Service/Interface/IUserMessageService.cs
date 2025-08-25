using System.Collections.Generic;
using Model;
using Common;

using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public interface IUserMessageService
    {
        Task<bool> CreateMessage(User senderuser, User receiveruser, string msg, EnumCountry country);

        Task<string> SaveContactRequestMessage(MessageViewModel objMessage, EnumCountry country, PackageConfig package);

        Task<string> SaveReplyMessage(MessageViewModel objMessage, EnumCountry country);

        Task<List<MessageViewModel>> GetAllInboxMsgsByUserId(long userId);

        Task<List<MessageViewModel>> GetAllSentMsgsByUserId(long userId);

        Task<MessageViewModel> GetSingleMessageByMsgId(long messageId);

        Task<bool> UpdateMessageStatus(long messageId);

        Task<bool> IsThereAnyNewMessageForUser(long userId);

        Task<MessageViewModel> MapMessageEntityToMessageViewModel(UserMessage message, MessageViewModel messageVm);
    }
}
