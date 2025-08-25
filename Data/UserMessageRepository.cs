using System.Linq;
using System.Collections.Generic;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class UserMessageRepository : WebBusinessEntityContext, IUserMessageRepository
    {
        //private readonly WebBusinessPlatformEntityContext _context = new WebBusinessPlatformEntityContext();
        public async Task<bool> AddMessage(UserMessage message)
        {
            _Context.Messages.Add(message);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserMessage>> GetInboxMsgListByUserID(long userId)
        {
            return await _Context.Messages.Where(a => a.ReceiverUserID == userId && a.IsActive).ToListAsync();
        }

        public async Task<List<UserMessage>> GetSentMsgListByUserID(long userId)
        {
            return await _Context.Messages.Where(a => a.SenderUserID == userId && a.IsActive).ToListAsync();
        }

        public async Task<UserMessage> GetSingleMessageByMsgId(long msgId)
        {
            return await _Context.Messages.SingleOrDefaultAsync(a => a.MessageID == msgId);
        }

        public async Task<bool> UpdateMessage(UserMessage message)
        {
            _Context.Entry<UserMessage>(message).State = System.Data.Entity.EntityState.Modified;
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
