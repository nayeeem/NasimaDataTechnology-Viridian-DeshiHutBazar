using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Common;


namespace WebDeshiHutBazar
{
    public class EmailService : IEmailNotificationService
    {
        public string DEFAULT_SENDER_EMAIL = ConfigurationManager.AppSettings["DefaultSenderEmail"];
        public string DEFAULT_COMPANY_WEBSITE = ConfigurationManager.AppSettings["DefaultCompanyWebsite"];
        public string DEFAULT_COMPANY_PHONE = ConfigurationManager.AppSettings["DefaultCompanyPhone"];
        public string DEFAULT_COMPANY_NAME = ConfigurationManager.AppSettings["DefaultCompanyName"];
        public string DEFAULT_PARENT_COMPANY_NAME = ConfigurationManager.AppSettings["DefaultParentCompanyName"];
        public string DEFAULT_COMPANY_ADDRESS = ConfigurationManager.AppSettings["DefaultCompanyAddress"];
        public string DEFAULT_COMPANY_HO_ADDRESS = ConfigurationManager.AppSettings["DefaultCompanyHOAddress"];
        public string SERVER_URL = ConfigurationManager.AppSettings["ServerUrl"];
        public const string SENDER_EMAIL_PASSWORD = "MsiNjiDjt@11n";

        public EmailService()
        {
        }

        public void SendContactUsEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendAccountVerifyEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendPasswordResetEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendExportEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendImportEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendRequestEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendProviderContactEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendCommentEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendLikeEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendPostViewedEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendContactRequestEmail(EmailViewModel emailViewModel)
        {
            SendAutomaticEmail(emailViewModel);
        }

        public void SendAutomaticEmail(EmailViewModel objEmailViewModel)
        {
            var senderEmailAddress = new MailAddress(objEmailViewModel.SenderCompanyEmail, "Bangladesh BAZAAR");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(objEmailViewModel.SenderCompanyEmail, SENDER_EMAIL_PASSWORD)
            };

            if (!string.IsNullOrEmpty(objEmailViewModel.ReceiverEmail))
            {
                try
                {
                    var toReceiverEmailAddress = new MailAddress(objEmailViewModel.ReceiverEmail, "Bangladesh BAZAAR");
                    using (var message = new MailMessage(senderEmailAddress, toReceiverEmailAddress)
                    {
                        Subject = objEmailViewModel.SubjectText,
                        Body = objEmailViewModel.MessageBodyHTMLText,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(message);
                    }
                }
                catch { }
            }
            var toAdminEmailAddress = new MailAddress("naimul.prodhan@gmail.com", "Bangladesh BAZAAR");
            using (var message = new MailMessage(senderEmailAddress, toAdminEmailAddress)
            {
                Subject = objEmailViewModel.SubjectText,
                Body = objEmailViewModel.MessageBodyHTMLText,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        public EmailViewModel GetContactUsViewModel(ContactViewModel objContact)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(DEFAULT_SENDER_EMAIL,
                    SENDER_EMAIL_PASSWORD,
                    DEFAULT_SENDER_EMAIL,
                    "Admin",
                    DEFAULT_COMPANY_ADDRESS,
                    DEFAULT_COMPANY_HO_ADDRESS,
                    DEFAULT_COMPANY_PHONE,
                    DEFAULT_COMPANY_WEBSITE,
                    DEFAULT_COMPANY_NAME
                    )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Message to admin from online visitor",
                Email = objContact.Email,
                Message = objContact.Message,
                FullName = objContact.FullName,
                MessageCategory = objContact.MessageCategory.ToString(),
                Subject = objContact.Subject
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetVerifyEmailViewModel(string codeValue)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(DEFAULT_SENDER_EMAIL,
                    SENDER_EMAIL_PASSWORD,
                    DEFAULT_SENDER_EMAIL,
                    "Admin",
                    DEFAULT_COMPANY_ADDRESS,
                    DEFAULT_COMPANY_HO_ADDRESS,
                    DEFAULT_COMPANY_PHONE,
                    DEFAULT_COMPANY_WEBSITE,
                    DEFAULT_COMPANY_NAME
                    )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Please Verify Email",
                EmailVerifyUrl = SERVER_URL + "/Account/VerifyAccount?code=" + codeValue
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetResetPassViewModel(string newPassword, string advertiserEmail)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(DEFAULT_SENDER_EMAIL,
                   SENDER_EMAIL_PASSWORD,
                   DEFAULT_SENDER_EMAIL,
                   "Admin",
                   DEFAULT_COMPANY_ADDRESS,
                   DEFAULT_COMPANY_HO_ADDRESS,
                   DEFAULT_COMPANY_PHONE,
                   DEFAULT_COMPANY_WEBSITE,
                   DEFAULT_COMPANY_NAME
                   )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Bangladesh BAZAAR - Re-set Password",
                NewPassword = newPassword,
                EmailAdvertiser = advertiserEmail
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetExportViewModel(string message, string phone)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                DEFAULT_SENDER_EMAIL,
                                "Admin User",
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Product/Service Export",
                Message = message,
                PhoneNofication = phone
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetImportViewModel(string message, string phone)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                DEFAULT_SENDER_EMAIL,
                                "Admin User",
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Product/Service Import",
                Message = message,
                PhoneNofication = phone
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetRequestViewModel(string message, string phone)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                DEFAULT_SENDER_EMAIL,
                                "Admin User",
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                SubjectText = "Product/Service Request",
                Message = message,
                PhoneNofication = phone
            };
            return objEmailViewModel;
        }

        public EmailViewModel GetProviderContactViewModel(FabiaProviderViewModel providerObject, long providerID)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                providerObject.Email,
                                providerObject.ProviderName,
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                EmailVerifyUrl = SERVER_URL + "/FabiaService/ViewItemDetail?postId=" + providerID,
                SubjectText = "Visitor collected your Phone Number from Fabia Service"
            };
            if (providerObject != null)
            {
                objEmailViewModel.PostTitle = providerObject.ServiceTitle;
            }
            return objEmailViewModel;
        }

        public EmailViewModel GetCommentViewModel(PostViewModel postVM, long postID)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(DEFAULT_SENDER_EMAIL,
                   SENDER_EMAIL_PASSWORD,
                   postVM.Email,
                   postVM.ClientName,
                   DEFAULT_COMPANY_ADDRESS,
                   DEFAULT_COMPANY_HO_ADDRESS,
                   DEFAULT_COMPANY_PHONE,
                   DEFAULT_COMPANY_WEBSITE,
                   DEFAULT_COMPANY_NAME
                   )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                EmailVerifyUrl = SERVER_URL + "/AllItemMarket/ViewItemDetail?postId=" + postID,
                SubjectText = "There is a Commented on your Ad"
            };
            if (postVM != null)
            {
                objEmailViewModel.PostTitle = postVM.Title;
            }
            return objEmailViewModel;
        }

        public EmailViewModel GetLikeViewModel(PostViewModel postVM, long postID)
        {           
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                postVM.Email,
                                postVM.PosterName,
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                EmailVerifyUrl = SERVER_URL + "/AllItemMarket/ViewItemDetail?postId=" + postID,
                SubjectText = "Someone liked your product/service"
            };
            if (postVM != null)
            {
                objEmailViewModel.PostTitle = postVM.Title;
            }
            return objEmailViewModel;
        }

        public EmailViewModel GetPostViewedViewModel(PostDisplayViewModel postVM, long postID)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                postVM.Email,
                                postVM.PosterName,
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                EmailVerifyUrl = SERVER_URL + "/AllItemMarket/ViewItemDetail?postId=" + postID,
                SubjectText = "Someone viewed your product/service"
            };
            if (postVM != null)
            {
                objEmailViewModel.PostTitle = postVM.Title;
            }
            return objEmailViewModel;
        }

        public EmailViewModel GetContactRequestViewModel(PostDisplayViewModel postVM, long postID, string email)
        {
            EmailViewModel objEmailViewModel = new EmailViewModel(
                DEFAULT_SENDER_EMAIL,
                                SENDER_EMAIL_PASSWORD,
                                postVM.Email,
                                postVM.PosterName,
                                DEFAULT_COMPANY_ADDRESS,
                                DEFAULT_COMPANY_HO_ADDRESS,
                                DEFAULT_COMPANY_PHONE,
                                DEFAULT_COMPANY_WEBSITE,
                                DEFAULT_COMPANY_NAME
                                )
            {
                SendDate = DateRelatedService.GetBangladeshCurrentDateTime(),
                EmailVerifyUrl = SERVER_URL + "/AllItemMarket/ViewItemDetail?postId=" + postID,
                SubjectText = "Someone collected your contact number",
                EmailNotification = email
            };
            if (postVM != null)
            {
                objEmailViewModel.PostTitle = postVM.Title;
            }
            return objEmailViewModel;
        }
    }
}