using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class AValue : BaseEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public AValue()
        {
        }

        /// <summary>
        /// For EF
        /// </summary>
        public AValue(EnumCountry country) {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="variable">EnumAllowedVariable object</param>
        public AValue(EnumCountry country, string text, EnumAllowedVariable variable)
        {
            if(string.IsNullOrEmpty(text))
                throw new ArgumentException("Text not provided.");
            Text = text;
            Variable = variable;
            EnumCountry = country;
        }

        /// <summary>
        /// PK
        /// </summary>
        [Key]
        public long ValueID { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Variable 
        /// </summary>
        [Required]
        public EnumAllowedVariable Variable { get; set; }

        /// <summary>
        /// Value ID Ex: If no parent then set to 0
        /// </summary>
        public long ParentValueId { get; set; }        
    }
}
