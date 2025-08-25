using System.Web.Mvc;
using System.Linq;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Mail;
using System.Net;

namespace WebDeshiHutBazar
{
    public class MessageController : BaseController
    {
        private readonly IUserMessageService _UserMessageService;
        public readonly IPackageConfigurationService _PackageConfigurationService;
        private readonly IPostVisitService _PostVisitService;
        private readonly IPostMangementService _PostManagementService;

        public MessageController() { }

        public MessageController(IUserMessageService userMessageService, IPackageConfigurationService packageConfigurationService,
            IPostVisitService postVisitService,
            IPostMangementService postManagementService)
        {
            _UserMessageService = userMessageService;
            _PackageConfigurationService = packageConfigurationService;
            _PostVisitService = postVisitService;
            _PostManagementService = postManagementService;
        }

        [Authorize(Roles = "Company, Advertiser")]
        public async Task<ViewResult> Message()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                CheckLogoutRequirements();

            MessageInfoViewModel objMessageInfoVm = new MessageInfoViewModel()
            {
                ListMsgInbox = await _UserMessageService.GetAllInboxMsgsByUserId(userId),
                ListMsgSent = await _UserMessageService.GetAllSentMsgsByUserId(userId),
                UserId = userId,
                PageName = "Message Page"
            };
            return View(@"../../Areas/Advertizer/Views/Message/Message", objMessageInfoVm);
        }


        [HttpPost]        
        public async Task<JsonResult> SaveContactRequestMessage(MessageViewModel objMessage)
        {
            var isValid = ValidationService.IsValidEmail(objMessage.Email);
            if (!isValid)
                return Json("EmailInvalid");
            var resultAddComments = await _PostVisitService.SavePostVisit(objMessage.PostID, objMessage.Email, objMessage.Phone, EnumPostVisitAction.PostContactQueried);
            var postVM = await _PostManagementService.GetPostByPostIDForEdit(objMessage.PostID, CURRENCY_CODE);
            try
            {











                var msg = "<div>Following visitor (email/phone) is interested on your product/s. He/she collected your contact details from Bangladesh BAZAAR:</div>" +
                           "</br>" +
                           "<div>Interested Buyer Information: <br/>Email: " + objMessage.Email + "<br/>" +
                           "Phone: " + objMessage.Phone + "</div>";
                var fromAddress = new MailAddress("deshihutbazar.info@gmail.com", "Bangladesh BAZAAR");
                const string fromPassword = "MsiNjiDjt@11n";
                string subject = "Interested Buyer Information: Bangladesh BAZAAR";
                string body = msg;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                var toAddress2 = new MailAddress(postVM.Email, postVM.ClientName);
                using (var message = new MailMessage(fromAddress, toAddress2)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                return Json(msg);
            }
            catch
            {
                return Json("SendFailed");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> SaveReplyMessage(MessageViewModel objMessage)
        {
            var msg = await _UserMessageService.SaveReplyMessage(objMessage, COUNTRY_CODE);
            return Json(msg);
        }

        
        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> UpdateMessaageStatus(long messageId)
        {
            var res = await _UserMessageService.UpdateMessageStatus(messageId);
            return Json("Success");
        }

        [HttpPost]
        [Authorize(Roles = "Company, Advertiser")]
        public async Task<JsonResult> IsThereAnyNewMessageForUser()
        {
            var userId = GetSessionUserId();
            if (userId == -1)
                return Json(false);
            return Json(await _UserMessageService.IsThereAnyNewMessageForUser(userId));
        }
    }
}