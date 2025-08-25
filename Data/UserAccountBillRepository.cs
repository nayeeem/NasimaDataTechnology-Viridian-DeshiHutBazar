using System.Linq;
using System.Collections.Generic;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class UserAccountBillRepository : WebBusinessEntityContext, IAccountBillRepository
    {
        public async Task<List<UserAccountBillTransaction>> GetAllSystemBills()
        {
            return await _Context.UserAccountBillTransactions.Where(a => a.IsActive && a.TransactionApprovalStatus == Common.EnumTransactionStatus.SystemApproved).ToListAsync();
        }

        public async Task<bool> AddNewBill(UserAccountBillTransaction objBill, long currentUserID)
        {
            if (objBill == null)
                return false;
            objBill.CreatedBy = currentUserID;
            _Context.UserAccountBillTransactions.Add(objBill);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
