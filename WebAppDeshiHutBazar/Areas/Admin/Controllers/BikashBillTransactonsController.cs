using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Model;
using Data;

namespace WebDeshiHutBazar
{
    public class BikashBillTransactonsController : BaseController
    {
        private readonly WebDeshiHutBazarEntityContext db;
        public readonly IBikashBillTransactionService _BikashBillTransactionService;

        public BikashBillTransactonsController()
            { }

        public BikashBillTransactonsController(IBikashBillTransactionService bikashBillTransactionService)
        {
            db = new WebDeshiHutBazarEntityContext();
            _BikashBillTransactionService = bikashBillTransactionService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Index()
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();

            var listBikashTranVM = await _BikashBillTransactionService.GetAllBikashBillList(CURRENCY_CODE);

            PostPriceConfigInformationViewModel objModel = new PostPriceConfigInformationViewModel(CURRENCY_CODE)
            {
                ListBikasdhBill = listBikashTranVM
            };
            return View(@"../../Areas/Admin/Views/BikashBillTransactons/Index", objModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(long? id)
        {
            if (GetSessionUserId() == -1)
                CheckLogoutRequirements();

            using (var context = new WebDeshiHutBazarEntityContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        BikashBillTransacton bikashBillTransacton = context.BikashBillTransactons.FirstOrDefault(a => a.BikashBillId == id.Value);

                        if (bikashBillTransacton.AdminApprovalStatus == EnumTransactionStatus.AdminApproved)
                        {
                            transaction.Commit();
                            return RedirectToAction("Index", "BikashBillTransactons");
                        }
                        
                        if (bikashBillTransacton != null)
                        {
                            bikashBillTransacton.AdminApprovalStatus = EnumTransactionStatus.AdminApproved;
                            context.SaveChanges();

                            var creditAmount = bikashBillTransacton.PaidAmount;

                            long? userOrderID = null;
                            var userOrderEntityObject = bikashBillTransacton.UserOrder;
                            if (userOrderEntityObject != null)
                                userOrderID = userOrderEntityObject.UserOrderID;

                            long? userCreditOrderID = null;
                            var userCreditOrderEntityObject = bikashBillTransacton.UserCreditOrder;
                            if (userCreditOrderEntityObject != null)
                                userCreditOrderID = userCreditOrderEntityObject.UserCreditOrderID;


                            if (userOrderID.HasValue && bikashBillTransacton.AdminApprovalStatus == EnumTransactionStatus.AdminApproved)
                            {
                                var userOrderObjectEntity = context.UserOrders.FirstOrDefault(a => 
                                                                                            a.UserOrderID == userOrderID &&
                                                                                            a.IsActive);
                                if (userOrderObjectEntity == null)
                                {
                                    transaction.Rollback();
                                    return RedirectToAction("Index", "BikashBillTransactons");
                                }

                                userOrderObjectEntity.OrderStatus = EnumPackageOrderStatus.Approved;
                                context.SaveChanges();


                                var userObjEntity = userOrderObjectEntity.User;
                                if (userObjEntity == null)
                                {
                                    transaction.Rollback();
                                    return RedirectToAction("Index", "BikashBillTransactons");
                                }

                                var listOrderDetailsEntities = userOrderObjectEntity.ListOrderDetails.Where(a => a.IsActive).ToList();
                                foreach (var orderItemEntity in listOrderDetailsEntities.ToList())
                                {
                                    var packageObjEntity = orderItemEntity.Package;
                                    if (packageObjEntity == null)
                                    {
                                        transaction.Rollback();
                                        return RedirectToAction("Index", "BikashBillTransactons");
                                    }

                                    UserPackage objUserPackageObjEntity = new UserPackage(
                                                                userObjEntity,
                                                                packageObjEntity,COUNTRY_CODE)
                                    {
                                        EnumCountry = EnumCountry.Bangladesh,
                                        Discount = packageObjEntity.Discount,
                                        TotalPremiumPost = orderItemEntity.TotalPremiumPost,
                                        TotalFreePost = orderItemEntity.TotalFreePost,
                                        PackagePrice = orderItemEntity.PackagePrice,
                                        SubscriptionPeriod = orderItemEntity.SubscriptionPeriod,
                                        IsActive = true
                                    };
                                    objUserPackageObjEntity.UpdateExpireDate(orderItemEntity.SubscriptionPeriod,COUNTRY_CODE);
                                    context.UserPackages.Add(objUserPackageObjEntity);
                                    context.Entry(objUserPackageObjEntity).State = EntityState.Added;
                                    context.SaveChanges();
                                }
                            }
                            if (userCreditOrderID.HasValue && bikashBillTransacton.AdminApprovalStatus == EnumTransactionStatus.AdminApproved)
                            {
                                var objUserCreditOrderEntity = context.UserCreditOrders.FirstOrDefault(a => 
                                                                                    a.UserCreditOrderID == userCreditOrderID.Value && 
                                                                                    a.IsActive);
                                if (objUserCreditOrderEntity == null)
                                {
                                    transaction.Rollback();
                                    return RedirectToAction("Index", "BikashBillTransactons");
                                }

                                objUserCreditOrderEntity.OrderStatus = EnumPackageOrderStatus.Approved;
                                context.SaveChanges();

                                var userEntity = objUserCreditOrderEntity.User;
                                var curBalance = userEntity.AccountBalance;
                                var newBaalance = curBalance + creditAmount;
                                userEntity.AccountBalance = newBaalance;

                                context.Entry(userEntity).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                            transaction.Commit();
                            return RedirectToAction("Index", "BikashBillTransactons");
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return RedirectToAction("Index", "BikashBillTransactons");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
