using System.Collections.Generic;
using System.Linq;
using Model;
using Data;
using Common;
using System.Threading.Tasks;
using System;

namespace WebDeshiHutBazar
{
    public class BikashBillTransactionService : IBikashBillTransactionService
    {
        public readonly IBikashBillTransactionRepository _BikashBillRepository;
        public BikashBillTransactionService(IBikashBillTransactionRepository bikashBillRepository) {
            _BikashBillRepository = bikashBillRepository;
        }

        public async Task<bool> AddNewBill(BikashBillTransacton objBill)
        {
            await _BikashBillRepository.AddBikashBillTransaction(objBill);            
            return true;
        }

        public async Task<bool> AddNewDonationBill(DonateBikashBillTransacton objBill)
        {
            await _BikashBillRepository.AddDonateBikashBillTransaction(objBill);
            return true;
        }

        public async Task<List<BikashBillTransactonViewModel>> GetAllBikashBillList(EnumCurrency currency)
        {
            var listTranEntities = await _BikashBillRepository.GetAllBikashBillTransaction();
            BikashBillTransactonViewModel objBikashTranViewModel;
            List<BikashBillTransactonViewModel> objBikashTranViewModelList = new List<BikashBillTransactonViewModel>();
            foreach (var bikashTranEntity in GetOrderedList(listTranEntities))
            {
                objBikashTranViewModel = GetNewBikashTransactionViewModel(bikashTranEntity, currency);
                SetBikashTransactionViewModel(objBikashTranViewModel, bikashTranEntity, currency);                
                objBikashTranViewModelList.Add(objBikashTranViewModel);
            }
            return objBikashTranViewModelList;
        }

        private List<BikashBillTransacton> GetOrderedList(List<BikashBillTransacton> listTranEntities)
        {
            return listTranEntities.OrderBy(a => (int)a.AdminApprovalStatus).ThenByDescending(a => a.EntryDateTime).ToList();
        }

        private void SetBikashTransactionViewModel(
            BikashBillTransactonViewModel objBikashTranViewModel, 
            BikashBillTransacton bikashTranEntity,
            EnumCurrency currency)
        {            
            var userOrderEntity = bikashTranEntity.UserOrder;
            UserOrderViewModel objUserOrderViewModel = null;
            UserOrderDetailViewModel objUserOrderDetailViewModel;
            List<UserOrderDetailViewModel> objListUserOrderDetailViewModel = new List<UserOrderDetailViewModel>();
            if (userOrderEntity != null)
            {
                objUserOrderViewModel = GetNewUserOrderViewModel(userOrderEntity, currency);
                var listDetails = userOrderEntity.ListOrderDetails;
                if (listDetails != null)
                {
                    foreach (var itemDet in listDetails)
                    {
                        objUserOrderDetailViewModel = GetNewUserOrderDetailsViewModel(itemDet, currency);
                        objListUserOrderDetailViewModel.Add(objUserOrderDetailViewModel);
                    }
                }
                objUserOrderViewModel.ListOrderDetails = objListUserOrderDetailViewModel;
            }
            objBikashTranViewModel.UserOrderModel = objUserOrderViewModel ?? null;
        }

        private UserOrderDetailViewModel GetNewUserOrderDetailsViewModel(
            UserOrderDetail itemDet, 
            EnumCurrency currency)
        {
            var objUserOrderDetailModel = new UserOrderDetailViewModel(currency)
            {
                PackageName = itemDet.Package != null ? itemDet.Package.PackageName : "",
                ItemBillAomunt = itemDet.ItemBillAomunt,
                TotalFreePost = itemDet.TotalFreePost,
                TotalPremiumPost = itemDet.TotalPremiumPost,
                PackagePrice = itemDet.PackagePrice,
                Discount = itemDet.PackageDiscount,
                SubscriptionPeriod = itemDet.SubscriptionPeriod,
                PackageType = itemDet.PackageType                
            };
            return objUserOrderDetailModel;
        }

        private UserOrderViewModel GetNewUserOrderViewModel(
            UserOrder userOrderEntity, 
            EnumCurrency currency)
        {
            var objUserOrderModel = new UserOrderViewModel(currency)
            {
                UserOrderID = userOrderEntity.UserOrderID,
                OrderDate = userOrderEntity.OrderDate,
                OrderStatus = userOrderEntity.OrderStatus,
                TotalBill = userOrderEntity.TotalBill
            };
            return objUserOrderModel;
        }

        private BikashBillTransactonViewModel GetNewBikashTransactionViewModel(
            BikashBillTransacton bikashTranEntity, 
            EnumCurrency currency)
        {
            var objBikashTranModel = new BikashBillTransactonViewModel(currency);
            objBikashTranModel.EntryDateTime = bikashTranEntity.EntryDateTime;
            objBikashTranModel.CreatedDate = bikashTranEntity.CreatedDate;
            objBikashTranModel.Status = bikashTranEntity.Status;
            objBikashTranModel.TransactionNumber = bikashTranEntity.TransactionNumber;
            objBikashTranModel.AgentNumber = bikashTranEntity.AgentNumber;
            objBikashTranModel.PaidAmount = bikashTranEntity.PaidAmount;
            objBikashTranModel.FormattedPaidAmountValue = objBikashTranModel.GetFormatedPriceValue(objBikashTranModel.PaidAmount.HasValue ? objBikashTranModel.PaidAmount.ToString() : "0 " + currency.ToString());
            objBikashTranModel.Currency = currency;
            objBikashTranModel.UserOrderID = bikashTranEntity.UserOrderID ?? 0;
            objBikashTranModel.BikashBillId = bikashTranEntity.BikashBillId;
            objBikashTranModel.AdminApprovalStatus = bikashTranEntity.AdminApprovalStatus;
            return objBikashTranModel;
        }

        public BikashBillTransacton CreateNewBikashTranEntityObject(
            BikashBillTransactonViewModel objModelBikashBillTranVM, 
            UserCreditOrder objUserCreditOrderEntity, 
            EnumCountry enumCountry)
        {
            BikashBillTransacton objBillEntityObject = new BikashBillTransacton(
                    objUserCreditOrderEntity,
                    objModelBikashBillTranVM.UserId,
                    objModelBikashBillTranVM.TransactionNumber,
                    objModelBikashBillTranVM.AgentNumber,
                    objModelBikashBillTranVM.PaidAmount,
                    enumCountry)
            {
                EnumCountry = enumCountry
            };
            return objBillEntityObject;
        }

        public DonateBikashBillTransacton CreateNewDonateBikashTranEntityObject(
            BikashBillTransactonViewModel objModelBikashBillTranVM,
            EnumCountry enumCountry)
        {
            DonateBikashBillTransacton objBillEntityObject = new DonateBikashBillTransacton(
                    objModelBikashBillTranVM.TransactionNumber,
                    objModelBikashBillTranVM.AgentNumber,
                    objModelBikashBillTranVM.PaidAmount, enumCountry)
            {
                EnumCountry = enumCountry
            };
            return objBillEntityObject;
        }
    }
}
