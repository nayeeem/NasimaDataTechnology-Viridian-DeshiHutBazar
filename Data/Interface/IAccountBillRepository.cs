using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IAccountBillRepository
    {
        Task<List<UserAccountBillTransaction>> GetAllSystemBills();

        Task<bool> AddNewBill(UserAccountBillTransaction objBill, long currentUserID);
    }
}