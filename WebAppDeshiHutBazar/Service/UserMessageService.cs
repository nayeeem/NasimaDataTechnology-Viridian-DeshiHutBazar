using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;

using System.Threading.Tasks;
using System;

namespace WebDeshiHutBazar
{
    public class UserMessageService : IUserMessageService
    {
        private readonly IUserRepository _UserRepository;
        
        private readonly IUserMessageRepository _MessageRepository;
        private readonly HashingCryptographyService _HashCryptographyService;
        private readonly SymmetricCryptographyService _SymmetricCryptographyService;

        public UserMessageService(
            IUserRepository userRepository,
            IUserMessageRepository messageRepository
            )
        {
            _UserRepository = userRepository;
            _MessageRepository = messageRepository;
            _HashCryptographyService = new HashingCryptographyService();
            _SymmetricCryptographyService = new SymmetricCryptographyService();
        }

        private bool IsValidPasswordSetForNewUser(string pass, string repass)
        {
            return !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(repass) && pass == repass;
        }

        private bool IsNewUser(User user)
        {
            return user == null;
        }

        private bool IsValidReceiver(User receiveruser)
        {
            return receiveruser != null;
        }

        private bool IsValidExistingUserButWrongCredential(User senderuser, string pass)
        {

            return senderuser != null && senderuser.Password != _HashCryptographyService.GetMessageDigest(pass, senderuser.Salt).Digest;
        }

        public async Task<string> SaveContactRequestMessage(MessageViewModel objMessage, EnumCountry country, PackageConfig package)
        {
            var senderuser = await _UserRepository.GetSingleUser(objMessage.Email);
            var receiveruser = await _UserRepository.GetSingleUser(objMessage.PosterEmail);

            if (IsNewUser(senderuser) && IsValidPasswordSetForNewUser(objMessage.Password,objMessage.RePassword))
            {
                senderuser = await CreateNewUser(objMessage.Email, objMessage.Password, objMessage.ClientName, country, package);
                if (IsValidReceiver(receiveruser))
                {
                    var res  = await CreateMessage(senderuser,receiveruser,objMessage.Message, country);
                    return "Success";
                }
                return "ReceiverNotFound";
            }
            else if (IsValidExistingUserButWrongCredential(senderuser, objMessage.Password))
            {
                return "IncorrectPassword";
            }
            else
            {
                var res1 = await CreateMessage(senderuser, receiveruser, objMessage.Message, country);
                return "Success";
            }
        }

        public async Task<bool> CreateMessage(User senderuser, User receiveruser, string msg, EnumCountry country)
        {
            var encreptMsg = _SymmetricCryptographyService.GetEncryptMessage(msg);
            var message = new UserMessage(country, senderuser, receiveruser, encreptMsg.Digest, encreptMsg.Key, encreptMsg.IV)
            {
                ParentMessageID = 0,
                IsActive = true
            };
            var res = await _MessageRepository.AddMessage(message);
            return true;
        }

        private async Task<User> CreateNewUser(string email, string pass, string name, EnumCountry country, PackageConfig package)
        {
            var passVM = _HashCryptographyService.GetMessageDigest(pass);
            var senderuser = new User(email, passVM.Digest, name, EnumUserAccountType.IndividualAdvertiser, passVM.Salt, country);           
            var res = await _UserRepository.AddUser(senderuser);
            var res1 = await _UserRepository.UpdateUser(senderuser);
            return senderuser;
        }

        public async Task<List<MessageViewModel>> GetAllInboxMsgsByUserId(long userId)
        {
            var listMsgs = await _MessageRepository.GetInboxMsgListByUserID(userId);

            MessageViewModel messageVm;
            List<MessageViewModel> listMsgVm = new List<MessageViewModel>();

            foreach(var message in listMsgs)
            {
                messageVm = new MessageViewModel();
                messageVm = await MapModel(message, messageVm);
                listMsgVm.Add(messageVm);
            }
            
            return listMsgVm.OrderByDescending(a=>a.DateMsgSent).ToList();
        }

        private async Task<MessageViewModel> MapModel(UserMessage message, MessageViewModel messageVm)
        {
            return await MapMessageEntityToMessageViewModel(message, messageVm);
        }

        public async Task<List<MessageViewModel>> GetAllSentMsgsByUserId(long userId)
        {
            var listMsgs = await _MessageRepository.GetSentMsgListByUserID(userId);

            MessageViewModel messageVm;
            List<MessageViewModel> listMsgVm = new List<MessageViewModel>();

            foreach (var message in listMsgs)
            {
                messageVm = new MessageViewModel();
                messageVm = await MapModel(message, messageVm);
                listMsgVm.Add(messageVm);
            }            

            return listMsgVm.OrderByDescending(a => a.DateMsgSent).ToList();
        }

        public async Task<MessageViewModel> GetSingleMessageByMsgId(long messageId)
        {
            var message = await _MessageRepository.GetSingleMessageByMsgId(messageId);
            if (message != null)
            {
                var messageVm = new MessageViewModel();
                messageVm = await MapModel(message, messageVm);
                return messageVm;
            }
            return new MessageViewModel();
        }

        private async Task<bool> CreateReplyMessage(EnumCountry country, User senderuser, User receiveruser, string msg, long parentMessageId)
        {
            var encreptMsg = _SymmetricCryptographyService.GetEncryptMessage(msg);
            var message = new UserMessage(country, senderuser, receiveruser, encreptMsg.Digest, encreptMsg.Key, encreptMsg.IV)
            {
                ParentMessageID = parentMessageId,
                IsActive = true
            };
            var res = await _MessageRepository.AddMessage(message);
            return true;
        }

        public async Task<string> SaveReplyMessage(MessageViewModel objMessage, EnumCountry country)
        {
            var senderuser = await _UserRepository.GetSingleUser(objMessage.SenderUserID);
            var receiveruser = await _UserRepository.GetSingleUser(objMessage.ReceiverUserID);

            if (senderuser != null && receiveruser != null)
            {
                var res= await CreateReplyMessage(country, senderuser, receiveruser, objMessage.Message, objMessage.MessageID);
                return "Success";
            }
            return string.Empty;
        }

        public async Task<bool> UpdateMessageStatus(long messageId)
        {
            var message = await _MessageRepository.GetSingleMessageByMsgId(messageId);
            if (message != null)
            {
                message.SetIsNewMessage(false);
                var res  = await _MessageRepository.UpdateMessage(message);
            }
            return true;
        }

        public async Task<bool> IsThereAnyNewMessageForUser(long userId)
        {
            if (userId==-1)
                return false;
            var list = await _MessageRepository.GetInboxMsgListByUserID(userId);
            return list.Any(a => a.IsNewMessage);
        }

        public async Task<MessageViewModel> MapMessageEntityToMessageViewModel(UserMessage message, MessageViewModel messageVm)
        {

            messageVm.MessageID = message.MessageID;
            messageVm.ParentMessageID = message.ParentMessageID;

            messageVm.Message = message.Msg;

            messageVm.DateMsgSent = message.CreatedDate;
            messageVm.DateMsgSentString = DateRelatedService.GetDateString(message.CreatedDate);

            var sender = await _UserRepository.GetSingleUser(message.SenderUserID);
            var receiver = await _UserRepository.GetSingleUser(message.ReceiverUserID);

            messageVm.SenderEmail = sender != null ? sender.Email : string.Empty;
            messageVm.ReceiverEmail = receiver != null ? receiver.Email : string.Empty;

            messageVm.SenderClientName = sender != null ? sender.ClientName : string.Empty;
            messageVm.ReceiverClientName = receiver != null ? receiver.ClientName : string.Empty;

            messageVm.SenderUserID = message.SenderUserID;
            messageVm.ReceiverUserID = message.ReceiverUserID;

            messageVm.IsNewMessage = message.IsNewMessage;

            messageVm.Message = _SymmetricCryptographyService.GetDecryptMessage(message.Msg, message.Key, message.IV);

            return messageVm;
        }
    }
}
