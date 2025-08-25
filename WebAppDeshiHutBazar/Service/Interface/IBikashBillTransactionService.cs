using Model;
using Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebDeshiHutBazar
{
    public interface IBikashBillTransactionService
    {
        Task<bool> AddNewBill(BikashBillTransacton objBill);

        Task<List<BikashBillTransactonViewModel>> GetAllBikashBillList(EnumCurrency currency);

        BikashBillTransacton CreateNewBikashTranEntityObject(BikashBillTransactonViewModel objModelBikashBillTranVM, UserCreditOrder objUserCreditOrderEntity, EnumCountry enumCountry);

        DonateBikashBillTransacton CreateNewDonateBikashTranEntityObject(
            BikashBillTransactonViewModel objModelBikashBillTranVM, 
            EnumCountry enumCountry);

        Task<bool> AddNewDonationBill(DonateBikashBillTransacton objBill);
    }
}
