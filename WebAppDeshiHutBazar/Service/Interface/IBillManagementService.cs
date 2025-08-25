using Model;
using Common;
using System.Threading.Tasks;
using System;

namespace WebDeshiHutBazar
{
    public interface IBillManagementService
    {
        Task<bool> MarkPostFree(long postId, long currentUserID, EnumCountry country);

        Task<bool> MarkPostPayable(long postId, long currentUserID, EnumCountry country);

        Task<bool> MarkPostCreditPaid(long postId, long currentUserID, EnumCountry country);

        Task<bool> MarkPostPremiumCreditPaid(long postId, long currentUserID, EnumCountry country);

        Task<bool> MarkPostSubscriptionPaid(long postId, int? userPackId, long currentUserID, EnumCountry country);

        Task<bool> MarkPostPremiumSubscriptionPaid(long postId, int? userPackId, long currentUserID, EnumCountry country);

        Task<bool> UpdatePostStatus(long postId, EnumPostStatus postStatus, int? userPackId, long currentUserID, EnumCountry country);

        Task<bool> DebitBalance(long userId, double amount, long currentUserID, EnumCountry country);

        Task<bool> UpdateAccountBillTransactionLog(long postId, double amount, long currentUserID, EnumCountry country);

        Task<bool> AddNewBill(UserAccountBillTransaction objBill, long currentUserID);
    }
}
