using System.Collections.Generic;
using System.Threading.Tasks;
using Model;


namespace Data
{
    public interface IUserMessageRepository
    {
        Task<bool> AddMessage(UserMessage message);

        Task<List<UserMessage>> GetInboxMsgListByUserID(long userId);

        Task<List<UserMessage>> GetSentMsgListByUserID(long userId);

        Task<UserMessage> GetSingleMessageByMsgId(long msgId);

        Task<bool> UpdateMessage(UserMessage message);
    }
}
