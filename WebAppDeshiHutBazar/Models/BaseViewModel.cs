
using System;
using System.ComponentModel.DataAnnotations;
using Common;
using Common.Language;

namespace WebDeshiHutBazar
{
    public class BaseViewModel
    {
        public BaseViewModel() { }

        public BaseViewModel(EnumCurrency currency) {
            PageingModelAll = new PagingModel();            
            MenuObjectModel = new MenuObjectModel();            
            CategorySearchInfoModel = new CategorySearchInfoModel();
            Currency = currency;
        }

        public string PageName { get; set; }

        [Display(Name = "Currency")]
        public EnumCurrency? Currency { get; set; }
       
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; }

        public long CreatedBy { get; set; }

        [Display(Name = "Date")]
        public DateTime CreatedDate { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }                
        
        public MenuObjectModel MenuObjectModel { get; set; }

        public CategorySearchInfoModel CategorySearchInfoModel { get; set; }

        public PagingModel PageingModelAll { get; set; }

        [Display(Name = "Date", ResourceType = typeof(LanguageDefault))]
        public string CreatedDateTimeString
        {
            get
            {
                return CreatedDate.ToShortDateString();
            }
        }

        public string GetFormatedPriceValue(string price)
        {
            string leftStr1 = "";
            string leftStr2 = "";
            string rightStr1 = "";
            string rightStr2 = "";
            var spaceIndex = price.Trim().IndexOf(' ');
            if (spaceIndex != -1)
            {
                leftStr1 = price.Substring(0, spaceIndex);
                rightStr1 = price.Substring(spaceIndex);
            }
            else
            {
                leftStr1 = price;
            }

            var dotIndex = leftStr1.IndexOf('.');
            if (dotIndex != -1)
            {
                leftStr2 = leftStr1.Substring(0, dotIndex);
                rightStr2 = leftStr1.Substring(dotIndex);
            }
            else
            {
                leftStr2 = leftStr1;
            }

            var originalstr = leftStr2;
            var remainder = originalstr.Length % 3;
            var devident = originalstr.Length / 3;
            var finalstring = "";
            var j = remainder == 0 ? 1 : 5;
            var counter = 1;

            for (var i = 0; i < originalstr.Length; i++)
            {
                if (counter == remainder + 1 && remainder != 0)
                {
                    finalstring += ",";
                    finalstring += originalstr[i];
                    j = 1;
                }
                else
                {
                    if (j == 4)
                    {
                        finalstring += ",";
                        finalstring += originalstr[i];
                        j = 1;
                    }
                    else
                    {
                        finalstring += originalstr[i];
                    }
                }
                counter++;
                j++;
            }

            if (!string.IsNullOrEmpty(rightStr2))
                finalstring = finalstring + rightStr2 + " " + GetCurrency();
            else
                finalstring = finalstring + " " + GetCurrency();
            return finalstring;
        }

        private string GetCurrency()
        {
            if (Currency.HasValue)
                return Currency.Value.ToString();

            return "BDT";
        }

        public string DisplayCurrencyShort
        {
            get
            {
                if (Currency.HasValue)
                {
                    return Currency.ToString();
                }
                else
                {
                    return "BDT";
                }
            }
        }
    }
}