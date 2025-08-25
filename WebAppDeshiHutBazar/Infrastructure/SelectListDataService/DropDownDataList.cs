using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model;
using Common.Language;

namespace WebDeshiHutBazar
{
    public class DropDownDataList
    {
        public DropDownDataList() { }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetHomeServiceAvailableList()
        {
            var listCountries = LocationRelatedSeed.GetHomeServiceAvailableList().OrderBy(a => a.ValueID).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            SelectListItem objItem = new SelectListItem();
            objItem.Text = "";
            objItem.Value = null;
            objOfferTypeListItems.Add(objItem);
            foreach (var item in listCountries)
            {
                objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPartsPurchaseByList()
        {
            var listCountries = LocationRelatedSeed.GetPartsPurchaseByList().OrderBy(a => a.ValueID).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            SelectListItem objItem = new SelectListItem();
            objItem.Text = "";
            objItem.Value = null;
            objOfferTypeListItems.Add(objItem);
            foreach (var item in listCountries)
            {
                objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetReportLengthList()
        {
            var listCountries = LocationRelatedSeed.GetReportLengthList().OrderBy(a => a.ValueID).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            SelectListItem objItem = new SelectListItem();
            objItem.Text = "";
            objItem.Value = null;
            objOfferTypeListItems.Add(objItem);
            foreach (var item in listCountries)
            {
                objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetStepsList()
        {
            var listCountries = LocationRelatedSeed.GetStepNumberList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPaidByList()
        {
            var listCountries = LocationRelatedSeed.GetPaidByList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPostTypeList()
        {
            var listCountries = LocationRelatedSeed.GetPostTypeList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetOfferTypeList()
        {
            var listCountries = LocationRelatedSeed.GetOfferTypeList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objOfferTypeListItems = new List<SelectListItem>();
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objOfferTypeListItems.Add(objItem);
            }
            return objOfferTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPaymentTimeList()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            var cultureNmae = currentCulture.Name;

            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            objCurrencyListItems.Add(new SelectListItem() { Value = null, Text = "" });

            SelectListItem objItem = new SelectListItem();
            objItem.Text =  LanguageDefault.DDL_Item_MonthlyPayment;
            objItem.Value = "1";
            objCurrencyListItems.Add(objItem);

            objItem = new SelectListItem();
            objItem.Text = LanguageDefault.DDL_Item_YearlyPayment;
            objItem.Value = "2";
            objCurrencyListItems.Add(objItem);
            
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPageList()
        {
            var listCountries = LocationRelatedSeed.GetPublicPages().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objCoutryListItems = new List<SelectListItem>();
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();                
                objCoutryListItems.Add(objItem);
            }
            return objCoutryListItems.AsEnumerable();
        }
        
        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPanelDisplayPositionList()
        {
            var listColumns = LocationRelatedSeed.GetPanelDisplayPositionList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            foreach (var item in listColumns)
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = item.ValueID.ToString()
                };
                objCurrencyListItems.Add(objItem);
            }
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetImageCategoryList()
        {
            var listColumns = LocationRelatedSeed.GetImageCategories().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            foreach (var item in listColumns)
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = item.ValueID.ToString()
                };
                objCurrencyListItems.Add(objItem);
            }
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetColumnList()
        {
            var listColumns = LocationRelatedSeed.GetColumnList().OrderBy(a => a.ValueID).ToList();
            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            foreach (var item in listColumns)
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = item.ValueID.ToString()
                };
                objCurrencyListItems.Add(objItem);
            }
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetColumnListMobile()
        {
            var listColumns = LocationRelatedSeed.GetColumnList().OrderBy(a => a.ValueID).ToList();
            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            foreach (var item in listColumns.Where(a => a.ValueID != 3 && a.ValueID != 4).ToList())
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = item.ValueID.ToString()
                };
                objCurrencyListItems.Add(objItem);
            }
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetCurrencyList()
        {
            var listCurrency = LocationRelatedSeed.GetCurrencyList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objCurrencyListItems = new List<SelectListItem>();
            objCurrencyListItems.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in listCurrency)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objCurrencyListItems.Add(objItem);
            }
            return objCurrencyListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetCountryList()
        {
            var listCountries = LocationRelatedSeed.GetCountryList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objCoutryListItems = new List<SelectListItem>();
            objCoutryListItems.Add(new SelectListItem() { Value = "1", Text = "" });
            foreach (var item in listCountries)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objCoutryListItems.Add(objItem);
            }
            return objCoutryListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPackageTypeList()
        {
            var listTypes = LocationRelatedSeed.GetPackageTypeList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objTypeListItems = new List<SelectListItem>();
            objTypeListItems.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in listTypes)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objTypeListItems.Add(objItem);
            }
            return objTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetSubscriptionPeriodList()
        {
            var listTypes = LocationRelatedSeed.GetSubscriptionPeriodList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objTypeListItems = new List<SelectListItem>();
            objTypeListItems.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in listTypes)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objTypeListItems.Add(objItem);
            }
            return objTypeListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetPackageStatusList()
        {
            var listStatuses = LocationRelatedSeed.GetPackageStatusList().OrderBy(a => a.Text).ToList();
            List<SelectListItem> objStatusListItems = new List<SelectListItem>();
            objStatusListItems.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in listStatuses)
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = item.Text;
                objItem.Value = item.ValueID.ToString();
                objStatusListItems.Add(objItem);
            }
            return objStatusListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetCategoryList()
        {
            return GetAValueSelectList(BusinessObjectSeed.GetCateSubCategoryListByVariable(EnumAllowedVariable.Category), "").ToList().AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetSubCategoryList()
        {
            var list = BusinessObjectSeed.GetCateSubCategoryListByVariable(EnumAllowedVariable.SubCategory);            
            return GetAValueSelectList(list, "").ToList().AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache30Mins")]
        public static IEnumerable<SelectListItem> GetCountryStateList(EnumCountry country, bool isAllCountry)
        {
            return GetAValueSelectList(LocationRelatedSeed.GetCountryStates(country, isAllCountry), "");
        }

        [OutputCache(CacheProfile = "Cache30Mins")]
        public static IEnumerable<SelectListItem> GetSubCategoryList(Int64 categoryId)
        {
            var listSubCat = BusinessObjectSeed.GetCateSubCategoryListByVariableAndParent(EnumAllowedVariable.SubCategory, categoryId);
            return GetAValueSelectList(listSubCat, "");
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetDeviceTypeList()
        {
            var listDeviceTypes = LocationRelatedSeed.GetDeviceTypeList();
            List<SelectListItem> objListItems = new List<SelectListItem>();
            listDeviceTypes.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = a.Text.Trim(),
                    Value = a.ValueID.ToString().Trim()
                };
                objListItems.Add(objItem);
            });
            return objListItems.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static IEnumerable<SelectListItem> GetShowHideList()
        {
            var listShowHideList = LocationRelatedSeed.GetShowHideList();
            List<SelectListItem> objListItems = new List<SelectListItem>();
            listShowHideList.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = a.Text.Trim(),
                    Value = a.ValueID.ToString().Trim()
                };
                objListItems.Add(objItem);
            });
            return objListItems.AsEnumerable();
        }

        private static IEnumerable<SelectListItem> GetAValueSelectList(List<AValueModel> listAValue, string selectText)
        {
            List<SelectListItem> objList = new List<SelectListItem>() { new SelectListItem() { Text = selectText, Value = null, Selected = true } };
            listAValue.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem
                {
                    Text = a.Text.Trim(),
                    Value = a.ValueID.ToString().Trim()
                };
                objList.Add(objItem);
            });
            return objList.AsEnumerable();
        }

        public static IEnumerable<SelectListItem> GetAValueSelectList(List<AValue> listAValue)
        {
            List<SelectListItem> objList = new List<SelectListItem> { new SelectListItem() { Text = "", Value = "" } };
            listAValue.ForEach(a => {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = a.Text.Trim();
                objItem.Value = a.ValueID.ToString().Trim();
                objList.Add(objItem);
            });
            return objList.AsEnumerable();
        }

        public static IEnumerable<SelectListItem> GetAValueSelectList(List<AValueModel> listAValue)
        {
            List<SelectListItem> objList = new List<SelectListItem> { new SelectListItem() { Text = "", Value = "" } };
            listAValue.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = a.Text.Trim();
                objItem.Value = a.ValueID.ToString().Trim();
                objList.Add(objItem);
            });
            return objList.AsEnumerable();
        }

        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static List<SelectListItem> GetAllContactMessageType()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            var cultureNmae = currentCulture.Name;
            List<SelectListItem> objList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "", Value = "", Selected = true },
                new SelectListItem() { Text = LanguageDefault.MessageGeneral, Value = "1" },
                new SelectListItem() { Text = LanguageDefault.MessageTechnical, Value = "2" },
                new SelectListItem() { Text = LanguageDefault.MessageAffiliateProgram, Value = "3" },
                new SelectListItem() { Text = LanguageDefault.MessageFeaturedAds, Value = "4" },
                new SelectListItem() { Text = LanguageDefault.MessageBilling, Value = "5" },
                new SelectListItem() { Text = LanguageDefault.MessageBuyAdSpace, Value = "6" }
            };
            return objList;                
        }
        
        [OutputCache(CacheProfile = "Cache1dayServerNBrowser")]
        public static List<SelectListItem> GetAllStateList()
        {
            var listStates = Common.LocationRelatedSeed.GetCountryStates(EnumCountry.Bangladesh, false);
            List<SelectListItem> objList = new List<SelectListItem> { new SelectListItem() { Text = "", Value = "" } };
            listStates.ForEach(a =>
            {
                SelectListItem objItem = new SelectListItem();
                objItem.Text = a.Text.Trim();
                objItem.Value = a.ValueID.ToString().Trim();
                objList.Add(objItem);
            });
            return objList;
        }
    }
}