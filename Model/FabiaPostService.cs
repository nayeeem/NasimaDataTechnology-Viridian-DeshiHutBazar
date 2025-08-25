using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    /// <summary>
    /// Entity Name: PostService
    /// This class is used to create a list for Fabia Base services, 
    /// but providers has their own services. They can select their 
    /// services from this list of entities.
    /// </summary>
    public class PostService : BaseEntity
    {
        public PostService()
        {
        }

        public PostService(EnumCountry country,
            string title,
            Post post,
            decimal price,
            EnumPaidBy paidBy,
            double discount,
            string reason,
            string description,
            string policy,
            string transportPolicy)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;

            Title = title;
            ServicePolicy = policy;
            PaidBy = paidBy;
            Description = description;
            ServicePrice = price;
            PostID = post.PostID;
            Discount = discount;
            ReasonPayment = reason;
            TransportPolicy = transportPolicy;
        }

        [Key]
        public long PostServiceID { get; set; }

        public string Title { get; set; }

        [MaxLength(4000)]
        public string ServicePolicy { get; set; }

        [MaxLength(4000)]
        public string TransportPolicy { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public byte[] ServiceImage { get; set; }

        public long? PostID { get; set; }

        [ForeignKey("PostID")]
        public virtual Post Post { get; set; }

        public decimal ServicePrice { get; set; }

        public double Discount { get; set; }

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
            ServiceImage = img;
        }
    }
}
