using System.Linq;
using System.Collections.Generic;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class BikashBillTransactionRepository : WebBusinessEntityContext, IBikashBillTransactionRepository
    {
        public async Task<List<BikashBillTransacton>> GetAllBikashBillTransaction()
        {
            var listBikashBills = await _Context.BikashBillTransactons.Where(a => a.IsActive).ToListAsync();
            return listBikashBills.ToList();
        }

        public async Task<List<BikashBillTransacton>> GetAllPendingBikashBillTransaction()
        {
            var listBikashBills = await _Context.BikashBillTransactons.Where(a => 
                                    a.IsActive && 
                                    a.AdminApprovalStatus == Common.EnumTransactionStatus.AdminCheckPending).ToListAsync();
            return listBikashBills.ToList();
        }

        public async Task<bool> AddBikashBillTransaction(BikashBillTransacton objBill)
        {
            if (objBill == null)
                return false;
            _Context.BikashBillTransactons.Add(objBill);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDonateBikashBillTransaction(DonateBikashBillTransacton objBill)
        {
            if (objBill == null)
                return false;
            _Context.DonateBikashBillTransactons.Add(objBill);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
