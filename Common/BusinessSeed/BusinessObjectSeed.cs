using System.Linq;
using System.Collections.Generic;
using Common;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using Common.Language;

namespace Common
{
    public class BusinessObjectSeed
    {
        public static List<AValueModel> GetAllCategoryList() {
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            return cateSubCategorySeedDataList
                             .ToList()
                             .Where(a => a.Variable == EnumAllowedVariable.Category)
                             .ToList<AValueModel>();
        }

        public static List<AValueModel> GetAllSubCategoryList()
        {
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            return cateSubCategorySeedDataList
                                .ToList()
                                .Where(a => a.Variable == EnumAllowedVariable.SubCategory)
                                .ToList<AValueModel>();
        }

        public static List<AValueModel> GetAllSubCategoryByCategoryID(long parentId)
        {
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            return cateSubCategorySeedDataList.ToList().Where(a => 
                                                            a.Variable == EnumAllowedVariable.SubCategory && 
                                                            a.ParentValueID == parentId)
                                                             .ToList<AValueModel>();
        }

        public static string GetCateSubCategoryItemText(long? id)
        {
            if (!id.HasValue)
                return string.Empty;
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            var cateSubCategoryAValueObj = cateSubCategorySeedDataList.FirstOrDefault(a => a.ValueID == id);
            if (cateSubCategoryAValueObj == null)
                return string.Empty;
            return cateSubCategoryAValueObj.Text;
        }

        public static string GetCategoryCSS(long? id)
        {
            if (!id.HasValue)
                return string.Empty;
            if (id.HasValue && id.Value == 0)
                return string.Empty;
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            var cateSubCategoryAValueObj = cateSubCategorySeedDataList.FirstOrDefault(a => a.ValueID == id);
            if (cateSubCategoryAValueObj == null)
                return string.Empty;
            return cateSubCategoryAValueObj.IconLink;
        }

        public static List<AValueModel> GetCateSubCategoryAValueEnglishSeed()
        {
            List<AValueModel> ListEnglishAValues = new List<AValueModel>
            {
                //category
                new AValueModel() { ValueID = (long) EnumMarket.Electronics, ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.Electronics, IconLink= "fas fa-tablet-alt" },
                new AValueModel() { ValueID = (long) EnumMarket.TVHomeAppliances , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.TVHomeAppliances, IconLink= "fas fa-video" },
                new AValueModel() { ValueID = (long) EnumMarket.HomeAndLiving , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.HomeAndLivings, IconLink= "fas fa-store-alt" },
                new AValueModel() { ValueID = (long) EnumMarket.Beauty , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.Beauty, IconLink= "fas fa-user-injured" },
                new AValueModel() { ValueID = (long) EnumMarket.Health , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.Health, IconLink= "fas fa-briefcase-medical" },
                new AValueModel() { ValueID = (long) EnumMarket.WomanFashion , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.WomanFashion , IconLink= "fas fa-venus-double" },
                new AValueModel() { ValueID = (long) EnumMarket.MenFashion , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.MenFashion , IconLink= "fas fa-user" },
                new AValueModel() { ValueID = (long) EnumMarket.ToysKidsAndBabies , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text = LanguageCategoryList.ToysKidsAndBabies , IconLink= "fas fa-child" },
                new AValueModel() { ValueID = (long) EnumMarket.FitnessAndLifeStyles , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.FitnessAndLifeStyles , IconLink= "fa fa-baseball-ball" },
                new AValueModel() { ValueID = (long) EnumMarket.TrainingHireTravels , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.TrainingHireTravels , IconLink= "fa fa-university" },
                new AValueModel() { ValueID = (long) EnumMarket.ExportImports , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.ExportImport  , IconLink= "fas fa-warehouse" },
                new AValueModel() { ValueID = (long) EnumMarket.Motor , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.Motor  , IconLink= "fas fa-motorcycle" },
                new AValueModel() { ValueID = (long) EnumMarket.GroceriesAndPets , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.Groceries  , IconLink= "fas fa-cookie-bite" },
                new AValueModel() { ValueID = (long) EnumMarket.Others , ParentValueID = 0, Variable = EnumAllowedVariable.Category, Text =LanguageCategoryList.Other  , IconLink= "fa fa-random" }
            };

            //category ids for parent of subcategory
            var electronics = (long )EnumMarket.Electronics;
            var tvhomeappliances = (long)EnumMarket.TVHomeAppliances;
            var homeliving = (long)EnumMarket.HomeAndLiving;
            var beauty = (long)EnumMarket.Beauty;
            var health = (long)EnumMarket.Health;
            var womenfashion = (long)EnumMarket.WomanFashion;
            var menfashion = (long)EnumMarket.MenFashion;
            var toyskidsandbabies = (long)EnumMarket.ToysKidsAndBabies;
            var fitnessandlifestyle = (long)EnumMarket.FitnessAndLifeStyles;
            var exportimports = (long)EnumMarket.ExportImports;
            var traininghiretravels = (long)EnumMarket.TrainingHireTravels;
            var motor = (long)EnumMarket.Motor;
            var groceries = (long)EnumMarket.GroceriesAndPets;
            var other = (long)EnumMarket.Others;

            //Electronics = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001001
                ValueID = (long) EnumSpecialMarket.AudioAndSoundSystems,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.AudioAndSoundSystems,
                IconLink = "fa fa-car-alt"
            });
            //Vehicle = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001002
                ValueID = (long)EnumSpecialMarket.LaptopAndPCs,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.LaptopAndComputer,
                IconLink = "fa fa-car-alt"
            });
            //Vehicle = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001003
                ValueID = (long)EnumSpecialMarket.CameraAndAccesories,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.CameraAndAccessories,
                IconLink = "fa fa-car-alt"
            });
            //Vehicle = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001004
                ValueID = (long)EnumSpecialMarket.MobilePhones,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MobilePhone,
                IconLink = "fa fa-car-alt"
            });
            //Vehicle = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001005
                ValueID = (long)EnumSpecialMarket.TabletAndGadgets,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.TabletAndGadget,
                IconLink = "fa fa-car-alt"
            });
            //Vehicle = 1001,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1001006
                ValueID = (long)EnumSpecialMarket.PhoneAccessories,
                ParentValueID = electronics,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.PhoneAccessorie,
                IconLink = "fa fa-car-alt"
            });   

            //TV & Home Appliances = 1002,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1002001
                ValueID = (long)EnumSpecialMarket.KitchenAppliances,
                ParentValueID = tvhomeappliances,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.KitchenAppliances,
                IconLink = "fa fa fa-home"
            });

            //TV & Home Appliances = 1002,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID =  1002002
                ValueID = (long)EnumSpecialMarket.TVAndHomeAudio,
                ParentValueID = tvhomeappliances,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.TVAndHomeAudio,
                IconLink = "fa fa fa-home"
            });

            //TV & Home Appliances = 1002,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID =  1002003
                ValueID = (long)EnumSpecialMarket.VacuumAndFloorCare,
                ParentValueID = tvhomeappliances,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.VacuumAndFloorCare,
                IconLink = "fa fa fa-home"
            });

            //TV & Home Appliances = 1002,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID =  1002004
                ValueID = (long)EnumSpecialMarket.CoolingAndHeating,
                ParentValueID = tvhomeappliances,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.CoolingAndHeating,
                IconLink = "fa fa fa-home"
            });

            //TV & Home Appliances = 1002,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID =  1002005
                ValueID = (long)EnumSpecialMarket.LargeAppliances,
                ParentValueID = tvhomeappliances,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.LargeAppliances,
                IconLink = "fa fa fa-home"
            });

            //Home & Living = 1003,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1003001
                ValueID = (long)EnumSpecialMarket.DiyAndCleaning,
                ParentValueID = homeliving,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.DiyAndCleaning,
                IconLink = "fa fa fa-home"
            });

            //Home & Living = 1003,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1003002
                ValueID = (long)EnumSpecialMarket.KitchenAndDining,
                ParentValueID = homeliving,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.KitchenAndDining,
                IconLink = "fa fa fa-home"
            });


            //Home & Living = 1003,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1003003
                ValueID = (long)EnumSpecialMarket.Furniture,
                ParentValueID = homeliving,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Furniture,
                IconLink = "fa fa fa-home"
            });


            //Home & Living = 1003,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1003003
                ValueID = (long)EnumSpecialMarket.OfficeFurniture,
                ParentValueID = homeliving,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.OfficeFurniture,
                IconLink = "fa fa fa-home"
            });


            //Home & Living = 1003,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1003004
                ValueID = (long)EnumSpecialMarket.BathBedAndDecor,
                ParentValueID = homeliving,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BathBedAndDecor,
                IconLink = "fa fa fa-home"
            });

            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //Beauty = 1004,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1004001
                ValueID = (long)EnumSpecialMarket.Makeup,
                ParentValueID = beauty,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Makeup,
                IconLink = "fa fa fa-home"
            });

            //Beauty = 1004,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1004002
                ValueID = (long)EnumSpecialMarket.SkinCare,
                ParentValueID = beauty,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.SkinCare,
                IconLink = "fa fa fa-home"
            });

            //Beauty = 1004,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1004003
                ValueID = (long)EnumSpecialMarket.HairBathBody,
                ParentValueID = beauty,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.HairBathBody,
                IconLink = "fa fa fa-home"
            });

            //Beauty = 1004,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1004004
                ValueID = (long)EnumSpecialMarket.BeautyTools,
                ParentValueID = beauty,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BeautyTools,
                IconLink = "fa fa fa-home"
            });

            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            ListEnglishAValues.Add(new AValueModel()
            {
                ValueID = (long)EnumSpecialMarket.Wellbeings,
                ParentValueID = health,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Wellbeings,
                IconLink = "fa fa fa-laptop"
            });

            ListEnglishAValues.Add(new AValueModel()
            {
                ValueID = (long)EnumSpecialMarket.PharmacyProducts,
                ParentValueID = health,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Pharmacy,
                IconLink = "fa fa fa-laptop"
            });


            ListEnglishAValues.Add(new AValueModel()
            {
                ValueID = (long)EnumSpecialMarket.MedicalSupplies,
                ParentValueID = health,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MedicalSupplies,
                IconLink = "fa fa fa-laptop"
            });

            ListEnglishAValues.Add(new AValueModel()
            {
                ValueID = (long)EnumSpecialMarket.BeautySupplements,
                ParentValueID = health,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BeautySupplements,
                IconLink = "fa fa fa-laptop"
            });


            ListEnglishAValues.Add(new AValueModel()
            {
                ValueID = (long)EnumSpecialMarket.PersonalCare,
                ParentValueID = health,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.PersonalCare,
                IconLink = "fa fa fa-laptop"
            });

            
            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006001
                ValueID = (long)EnumSpecialMarket.WomenBags,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenBags,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006002
                ValueID = (long)EnumSpecialMarket.WomenGolds,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenGolds,
                IconLink = "fa fa fa-laptop"
            });


            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006003
                ValueID = (long)EnumSpecialMarket.WomenDresses,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenDresses,
                IconLink = "fa fa fa-laptop"
            });


            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006004
                ValueID = (long)EnumSpecialMarket.WomenShoes,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenShoes,
                IconLink = "fa fa fa-laptop"
            });


            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006005
                ValueID = (long)EnumSpecialMarket.WomenSpecticals,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Specticals,
                IconLink = "fa fa fa-laptop"
            });


            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006006
                ValueID = (long)EnumSpecialMarket.WomenWatches,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenWatches,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAppliances = 1006,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006006
                ValueID = (long)EnumSpecialMarket.WomenFashionAccessories,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenFashionAccessories,
                IconLink = "fa fa fa-laptop"
            });

            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1006006
                ValueID = (long)EnumSpecialMarket.WomenBoishakDress,
                ParentValueID = womenfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomenFashionWomenBoishakDress,
                IconLink = "fa fa fa-laptop"
            });


            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007001
                ValueID = (long)EnumSpecialMarket.MenWaletBags,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenWaletBags,
                IconLink = "fa fa fa-laptop"
            });

            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007002
                ValueID = (long)EnumSpecialMarket.MenWatches,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenWatches,
                IconLink = "fa fa fa-laptop"
            });

            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007003
                ValueID = (long)EnumSpecialMarket.MenDresses,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenDresses,
                IconLink = "fa fa fa-laptop"
            });

            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007004
                ValueID = (long)EnumSpecialMarket.MenShoes,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenShoes,
                IconLink = "fa fa fa-laptop"
            });

            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007005
                ValueID = (long)EnumSpecialMarket.Specticals,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Specticals,
                IconLink = "fa fa fa-laptop"
            });

            //WomanDress = 1007,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1007005
                ValueID = (long)EnumSpecialMarket.MenFashionAccessories,
                ParentValueID = menfashion,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MenFashionAccessories,
                IconLink = "fa fa fa-laptop"
            });

            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //MenFashion = 1008,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1008001
                ValueID = (long)EnumSpecialMarket.BabyMaternity,
                ParentValueID = toyskidsandbabies,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BabyMaternity,
                IconLink = "fa fa fa-laptop"
            });

            //MenFashion = 1008,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1008002
                ValueID = (long)EnumSpecialMarket.BabyGears,
                ParentValueID = toyskidsandbabies,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BabyGears,
                IconLink = "fa fa fa-laptop"
            });

            //MenFashion = 1008,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1008003
                ValueID = (long)EnumSpecialMarket.BabyDiapers,
                ParentValueID = toyskidsandbabies,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.BabyDiapers,
                IconLink = "fa fa fa-laptop"
            });

            //MenFashion = 1008,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1008004
                ValueID = (long)EnumSpecialMarket.ToysGames,
                ParentValueID = toyskidsandbabies,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.ToysGames,
                IconLink = "fa fa fa-laptop"
            });
            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //ChildrenAndEssentials = 1009,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1009001
                ValueID = (long)EnumSpecialMarket.CarOilFluids,
                ParentValueID = motor,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.CarOilFluids,
                IconLink = "fa fa fa-laptop"
            });

            //ChildrenAndEssentials = 1009,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1009002
                ValueID = (long)EnumSpecialMarket.CarEssentials,
                ParentValueID = motor,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.CarEssentials,
                IconLink = "fa fa fa-laptop"
            });

            //ChildrenAndEssentials = 1009,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1009003
                ValueID = (long)EnumSpecialMarket.MotorCycleEssentials,
                ParentValueID = motor,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MotorCycleEssentials,
                IconLink = "fa fa fa-laptop"
            });

            //ChildrenAndEssentials = 1009,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1009004
                ValueID = (long)EnumSpecialMarket.CarServicesInstallations,
                ParentValueID = motor,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.CarServicesInstallations,
                IconLink = "fa fa fa-laptop"
            });

            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //HealthAndBeauty = 1010,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1010001
                ValueID = (long)EnumSpecialMarket.MensSportswear,
                ParentValueID = fitnessandlifestyle,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MensSportswear,
                IconLink = "fa fa fa-laptop"
            });

            //HealthAndBeauty = 1010,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1010002
                ValueID = (long)EnumSpecialMarket.WomensSportswear,
                ParentValueID = fitnessandlifestyle,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.WomensSportswear,
                IconLink = "fa fa fa-laptop"
            });

            //HealthAndBeauty = 1010,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1010003
                ValueID = (long)EnumSpecialMarket.FitnessEquipments,
                ParentValueID = fitnessandlifestyle,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.FitnessEquipments,
                IconLink = "fa fa fa-laptop"
            });

            //HealthAndBeauty = 1010,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1010004
                ValueID = (long)EnumSpecialMarket.GlobalFittness,
                ParentValueID = fitnessandlifestyle,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.GlobalFittness,
                IconLink = "fa fa fa-laptop"
            });

            //HealthAndBeauty = 1010,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1010005
                ValueID = (long)EnumSpecialMarket.MusicBooksGames,
                ParentValueID = fitnessandlifestyle,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.MusicBooksGames,
                IconLink = "fa fa fa-laptop"
            });
            


            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011001
                ValueID = (long)EnumSpecialMarket.SnacksCookies,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.SnacksCookies,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011002
                ValueID = (long)EnumSpecialMarket.Nuts,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Nuts,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011003
                ValueID = (long)EnumSpecialMarket.Beverages,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Beverages,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011004
                ValueID = (long)EnumSpecialMarket.GroceryEssentials,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.GroceryEssentials,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011005
                ValueID = (long)EnumSpecialMarket.Laundry,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Laundry,
                IconLink = "fa fa fa-laptop"
            });

            //HomeAndLiving = 1011,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1011005
                ValueID = (long)EnumSpecialMarket.Pets,
                ParentValueID = groceries,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.Pets,
                IconLink = "fa fa fa-laptop"
            });

            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //ForOffice = 1012,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1012001
                ValueID = (long)EnumSpecialMarket.TrainingPackages,
                ParentValueID = traininghiretravels,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.TrainingPackages,
                IconLink = "fa fa fa-laptop"
            });

            //ForOffice = 1012,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1012002
                ValueID = (long)EnumSpecialMarket.TravelPackages,
                ParentValueID = traininghiretravels,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.TravelPackages,
                IconLink = "fa fa fa-laptop"
            });

            //ForOffice = 1012,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1012003
                ValueID = (long)EnumSpecialMarket.HireMe,
                ParentValueID = traininghiretravels,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.HireMe,
                IconLink = "fa fa fa-laptop"
            });


            //////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

            //SportsAndHobbies = 1013,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1013001
                ValueID = (long)EnumSpecialMarket.ExportProducts,
                ParentValueID = exportimports,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.ExportProducts,
                IconLink = "fa fa fa-laptop"
            });

            //SportsAndHobbies = 1013,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1013002
                ValueID = (long)EnumSpecialMarket.ImportProducts,
                ParentValueID = exportimports,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.ImportProducts,
                IconLink = "fa fa fa-laptop"
            });

            ////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////
            ///


            //SportsAndHobbies = 1014,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1014002
                ValueID = (long)EnumSpecialMarket.OtherProductItems,
                ParentValueID = other,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.OtherProductItems,
                IconLink = "fa fa fa-laptop"
            });

            //SportsAndHobbies = 1014,
            ListEnglishAValues.Add(new AValueModel()
            {
                //ValueID = 1014002
                ValueID = (long)EnumSpecialMarket.OtherServiceItems,
                ParentValueID = other,
                Variable = EnumAllowedVariable.SubCategory,
                Text = LanguageCategoryList.OtherServiceItems,
                IconLink = "fa fa fa-laptop"
            });


            return ListEnglishAValues;
        }       

        public static List<AValueModel> GetCateSubCategoryListByVariableAndParent(EnumAllowedVariable variable, long parentId)
        {
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            return cateSubCategorySeedDataList.Where(a => a.Variable == variable && 
                                                          a.ParentValueID == parentId)
                                                           .OrderBy(order => order.ValueID)
                                                           .ToList();
        }

        public static List<AValueModel> GetCateSubCategoryListByVariable(EnumAllowedVariable variable)
        {
            var cateSubCategorySeedDataList = GetCateSubCategoryAValueEnglishSeed();
            return cateSubCategorySeedDataList.Where(a => a.Variable == variable).OrderBy(order => order.ValueID).ToList();
        }

        public static string GetCatSubCategoryItemTextForId(long valueId)
        {
            if (valueId == 0)
                return "";
            var listCateListAValues = GetCateSubCategoryAValueEnglishSeed();
            var cateObjAValue = listCateListAValues.SingleOrDefault(a => a.ValueID == valueId); 
            return cateObjAValue != null ? cateObjAValue.Text : "";
        }

        public static long GetCategoryIDForSubCategoryID(long valueId)
        {
            if (valueId == 0)
                return 0;
            var listCateListAValues = GetCateSubCategoryAValueEnglishSeed();
            var cateObjAValue = listCateListAValues.SingleOrDefault(a => a.ValueID == valueId);
            return cateObjAValue != null ? cateObjAValue.ParentValueID : 0;
        }
    }
}
