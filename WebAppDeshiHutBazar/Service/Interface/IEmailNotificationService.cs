

namespace WebDeshiHutBazar
{
    public interface IEmailNotificationService
    {       
        void SendContactUsEmail(EmailViewModel emailViewModel);

        void SendAccountVerifyEmail(EmailViewModel emailViewModel);

        void SendPasswordResetEmail(EmailViewModel emailViewModel);

        void SendExportEmail(EmailViewModel emailViewModel);

        void SendImportEmail(EmailViewModel emailViewModel);

        void SendRequestEmail(EmailViewModel emailViewModel);

        void SendProviderContactEmail(EmailViewModel emailViewModel);

        void SendCommentEmail(EmailViewModel emailViewModel);

        void SendLikeEmail(EmailViewModel emailViewModel);

        void SendPostViewedEmail(EmailViewModel emailViewModel);

        void SendContactRequestEmail(EmailViewModel emailViewModel);

        EmailViewModel GetContactUsViewModel(ContactViewModel objContact);

        EmailViewModel GetVerifyEmailViewModel(string codeValue);

        EmailViewModel GetResetPassViewModel(string newPassword, string advertiserEmail);

        EmailViewModel GetExportViewModel(string message, string phone);

        EmailViewModel GetImportViewModel(string message, string phone);

        EmailViewModel GetRequestViewModel(string message, string phone);

        EmailViewModel GetProviderContactViewModel(FabiaProviderViewModel providerObject, long providerID);

        EmailViewModel GetCommentViewModel(PostViewModel postVM, long postID);

        EmailViewModel GetLikeViewModel(PostViewModel postVM, long postID);

        EmailViewModel GetPostViewedViewModel(PostDisplayViewModel postVM, long postID);

        EmailViewModel GetContactRequestViewModel(PostDisplayViewModel postVM, long postID, string email);
    }
}
