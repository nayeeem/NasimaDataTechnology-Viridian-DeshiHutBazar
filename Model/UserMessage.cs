using System;
using System.ComponentModel.DataAnnotations;
using Common;
namespace Model
{    
    public class UserMessage : BaseEntity
    {
        public UserMessage() { }

        public UserMessage(EnumCountry country, User sender, User receiver, string message, byte[] key, byte[] iv) {
            SenderUserID = sender.UserID;
            ReceiverUserID = receiver.UserID;
            Msg = message;
            IsNewMessage = true;
            Key = key;
            IV = iv;
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
        }
        
        [Key]
        public long MessageID { get; set; }
        
        public long SenderUserID { get; set; }
        
        public long ReceiverUserID { get; set; }

        public string Msg { get; set; }
        
        public long ParentMessageID { get; set; }
        
        public bool IsNewMessage { get; private set; }

        public void SetIsNewMessage(bool isNew)
        {
            IsNewMessage = isNew;
        }

        public byte[] Key { get; set; }

        public byte[] IV { get; set; }
    }
}
