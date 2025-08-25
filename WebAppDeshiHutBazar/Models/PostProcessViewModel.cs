using System.Collections.Generic;
using System.Web.Mvc;
using Common;
namespace WebDeshiHutBazar
{    
    public class PostProcessViewModel : BaseViewModel
    {
        public PostProcessViewModel()
        {
        }

        public PostProcessViewModel(EnumCurrency currency) : base(currency)
        {
        }                  
        
        public long PostProcessID { get; set; }

        public EnumStepNumber StepNo { get; set; }

        public string StepName { get; set; }
        
        public string Description { get; set; }
                
        public byte[] StepImage { get; set; }        
        
        public long PostID { get; set; }

        public string PostTitle { get; set; }

        public decimal Price { get; set; }

        public double AvailabilityDurationHour { get; set; }

        public EnumPaidBy PaidBy { get; set; }

        public string ReasonPayment { get; set; }

        public IEnumerable<SelectListItem> AV_StepNo { get; set; }

        public IEnumerable<SelectListItem> AV_PaidBy { get; set; }
    }
}
