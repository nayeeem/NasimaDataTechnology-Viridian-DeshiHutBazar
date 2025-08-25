using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Data
{
    public interface IBikashBillTransactionRepository
    {
        Task<List<BikashBillTransacton>> GetAllBikashBillTransaction();

        Task<List<BikashBillTransacton>> GetAllPendingBikashBillTransaction();

        Task<bool> AddBikashBillTransaction(BikashBillTransacton objBill);

        Task<bool> AddDonateBikashBillTransaction(DonateBikashBillTransacton objBill);
    }
}