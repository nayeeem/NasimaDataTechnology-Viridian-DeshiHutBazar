using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    /// <summary>
    /// Entity Name: Post
    /// </summary>
    public class PostProcess : BaseEntity
    {        
        public PostProcess()
        {
        }
        
        public PostProcess(EnumCountry country,
            EnumStepNumber stepNumber,
            string name, 
            Post post, 
            decimal price, 
            EnumPaidBy paidBy,
            double durationHour,
            string reason,
            string description)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            StepName = name;
            StepNo = stepNumber;
            PaidBy = paidBy;
            Description = description;
            Price = price;
            PostID = post.PostID;
            AvailabilityDurationHour = durationHour;
            ReasonPayment = reason;       
        }        
        
        [Key]
        public long PostProcessID { get; set; }

        public EnumStepNumber StepNo { get; set; }
        
        public string StepName { get; set; }
        
        [MaxLength(4000)]
        public string Description { get; set; }
                
        public byte[] StepImage { get; set; }        
        
        public long? PostID { get; set; }        
        
        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }
        
        public decimal Price { get; set; }

        public double AvailabilityDurationHour { get; set; }

        public EnumPaidBy PaidBy { get; set; }

        public string ReasonPayment { get; set; }

        public void SetModifiedDate(EnumCountry country)
        {

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            ModifiedDate = BaTime;
        }

        public void SetProcessImage(byte[] img)
        {
            StepImage = img;
        }
    }
}
