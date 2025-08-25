using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Common
{
    public class LocationRelatedSeed
    {        
        private static readonly Dictionary<EnumCountry, EnumCurrency> objCountryCurrencyList = new Dictionary<EnumCountry, EnumCurrency>()
        {
            { EnumCountry.Australia, EnumCurrency.AUD},
            { EnumCountry.Bangladesh, EnumCurrency.BDT},
            { EnumCountry.Canada, EnumCurrency.CAD},
            { EnumCountry.Malaysia, EnumCurrency.MYR},
            { EnumCountry.Tanzania, EnumCurrency.TZS},
            { EnumCountry.USA, EnumCurrency.USD},
            { EnumCountry.Nigeria, EnumCurrency.NGN}
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static EnumCurrency GetCountryCurrency(EnumCountry countryName)
        {
            var item = objCountryCurrencyList[countryName];
            return item;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string GetCountryCurrencyDescription(EnumCountry countryName)
        {
            var currency = GetCountryCurrency(countryName);
            return GetCurrencyDescription(currency);
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly List<KeyValuePair< EnumCountry, EnumState>> objCountryStateList = new List<KeyValuePair<EnumCountry, EnumState>>()
        {
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Dhaka),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.Bangladesh, EnumState.Chittagong),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Rajshahi),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Khulna),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Barishal),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Sylhet),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Maimenshing),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Rangpur),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.USA, EnumState.NewYork),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.USA, EnumState.Virginia),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.USA, EnumState.Washington),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.USA, EnumState.IOWA),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Umuahia),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.Bangladesh, EnumState. Yola),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Uyo),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Awka),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Bauchi),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Yenagoa),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Makurdi),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Maiduguri),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Calabar),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.Bangladesh, EnumState. Asaba),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Abakaliki),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.BeninCity),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.AdoEkiti),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Enugu),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Abuja),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Gombe),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Owerri),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.Bangladesh, EnumState. Dutse),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Kaduna),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Kano),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Katsina),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.BirninKebbi),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Lokoja),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Ilorin),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Ikeja),
            new KeyValuePair<EnumCountry, EnumState>(EnumCountry.Bangladesh, EnumState. Lafia),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Minna),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Abeokuta),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Akure),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Oshogbo),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Ibadan),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Jos),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.PortHarcourt),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Sokoto),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Jalingo),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Damaturu),
            new KeyValuePair<EnumCountry, EnumState>( EnumCountry.Bangladesh, EnumState.Gusau)
       };


        /// <summary>                                                            
        ///                                                                          
        /// </summary>                                                               
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static List<AValueModel> GetCountryStates(EnumCountry countryName, bool IsAllCountry)
        {
            List<KeyValuePair < EnumCountry, EnumState >> listStates;
            if (IsAllCountry)
            {
                 listStates = objCountryStateList.ToList();
            }
            else
            {
                 listStates = objCountryStateList.Where(a => a.Key == countryName).ToList();
            }
            List<AValueModel> objStateList = new List<AValueModel>();
            Type enumType = typeof(EnumState);
            foreach (var item in listStates)
            {
                var value = item.Value;

                MemberInfo memberInfo = 
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objStateList.Add(objPair);
            }
            return objStateList;
        }

        public static string GetCurrencyDescription(EnumCurrency currencyEnum)
        {
            Type enumType = typeof(EnumCurrency);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumCurrency value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == currencyEnum)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetPackageTypeDescription(EnumPackageType packageTypeEnum)
        {
            Type enumType = typeof(EnumPackageType);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageType value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == packageTypeEnum)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetSubscriptionPeriodDescription(EnumPackageSubscriptionPeriod packageTypeEnum)
        {
            Type enumType = typeof(EnumPackageSubscriptionPeriod);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageSubscriptionPeriod value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == packageTypeEnum)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetPackageStatusDescription(EnumPackageStatus packageStatusEnum)
        {
            Type enumType = typeof(EnumPackageStatus);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageStatus value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == packageStatusEnum)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetCountryDescription(EnumCountry countryEnum)
        {
            Type enumType = typeof(EnumCountry);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumCountry value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == countryEnum)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static string GetStateDescription(EnumState? stateEnum)
        {
            if (!stateEnum.HasValue)
                return "";

            Type enumType = typeof(EnumState);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumState value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (value == stateEnum.Value)
                {
                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public static List<AValueModel> GetPackageTypeList()
        {
            List<AValueModel> objPackageTypeList = new List<AValueModel>();

            Type enumType = typeof(EnumPackageType);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageType value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objPackageTypeList.Add(objPair);
            }
            return objPackageTypeList;
        }

        public static List<AValueModel> GetSubscriptionPeriodList()
        {
            List<AValueModel> objPackageTypeList = new List<AValueModel>();

            Type enumType = typeof(EnumPackageSubscriptionPeriod);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageSubscriptionPeriod value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel();
                objPair.ValueID = (long)value;
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objPackageTypeList.Add(objPair);
            }
            return objPackageTypeList;
        }

        public static List<AValueModel> GetPackageStatusList()
        {
            List<AValueModel> objPackageStatusList = new List<AValueModel>();

            Type enumType = typeof(EnumPackageStatus);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPackageStatus value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objPackageStatusList.Add(objPair);
            }
            return objPackageStatusList;
        }

        public static List<AValueModel> GetCountryList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumCountry);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumCountry value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetCurrencyList()
        {
            List<AValueModel> objCurrencyList = new List<AValueModel>();

            Type enumType = typeof(EnumCurrency);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumCurrency value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }

                objCurrencyList.Add(objPair);

            }
            return objCurrencyList;
        }

        public static List<AValueModel> GetDeviceTypeList()
        {
            List<AValueModel> objPackageTypeList = new List<AValueModel>();

            Type enumType = typeof(EnumDeviceType);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumDeviceType value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objPackageTypeList.Add(objPair);
            }
            return objPackageTypeList;
        }

        public static List<AValueModel> GetShowHideList()
        {
            List<AValueModel> objPackageTypeList = new List<AValueModel>();

            Type enumType = typeof(EnumShowOrHide);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumShowOrHide value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel();
                objPair.ValueID = (long)value;
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objPackageTypeList.Add(objPair);
            }
            return objPackageTypeList;
        }

        public static List<AValueModel> GetColumnList()
        {
            List<AValueModel> objColumnList = new List<AValueModel>();

            Type enumType = typeof(EnumColumn);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumColumn value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objColumnList.Add(objPair);
            }
            return objColumnList;
        }
        
        public static List<AValueModel> GetPanelDisplayPositionList()
        {
            List<AValueModel> objColumnList = new List<AValueModel>();

            Type enumType = typeof(EnumPanelDisplayStyle);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPanelDisplayStyle value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objColumnList.Add(objPair);
            }
            return objColumnList;
        }

        public static List<AValueModel> GetImageCategories()
        {
            List<AValueModel> objColumnList = new List<AValueModel>();

            Type enumType = typeof(EnumImageCategory);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumImageCategory value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objColumnList.Add(objPair);
            }
            return objColumnList;
        }

        public static List<AValueModel> GetPublicPages()
        {
            List<AValueModel> objColumnList = new List<AValueModel>();

            Type enumType = typeof(EnumPublicPage);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPublicPage value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                objColumnList.Add(objPair);
            }
            return objColumnList;
        }

        public static List<AValueModel> GetOfferTypeList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumOfferType);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumOfferType value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetPostTypeList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumPostType);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPostType value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetPaidByList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumPaidBy);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPaidBy value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetStepNumberList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumStepNumber);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumStepNumber value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel();
                objPair.ValueID = (long)value;
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetReportLengthList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumReportLength);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumReportLength value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetPartsPurchaseByList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumPartsPurchaseBy);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumPartsPurchaseBy value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel
                {
                    ValueID = (long)value
                };
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

        public static List<AValueModel> GetHomeServiceAvailableList()
        {
            List<AValueModel> objCountryList = new List<AValueModel>();

            Type enumType = typeof(EnumHomeServiceAvailable);
            var enumValues = enumType.GetEnumValues();

            foreach (EnumHomeServiceAvailable value in enumValues)
            {
                MemberInfo memberInfo =
                    enumType.GetMember(value.ToString()).First();
                var descriptionAttribute =
                    memberInfo.GetCustomAttribute<DescriptionAttribute>();

                AValueModel objPair = new AValueModel();
                objPair.ValueID = (long)value;
                if (descriptionAttribute != null)
                {
                    objPair.Text = descriptionAttribute.Description;
                }
                else
                {
                    objPair.Text = value.ToString();
                }
                if (objPair.ValueID != 0)
                {
                    objCountryList.Add(objPair);
                }
            }
            return objCountryList;
        }

    }
}
