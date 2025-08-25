using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Model
{
    public class File : BaseEntity
    {
        public File() { }
        
        public File(string fileName, Byte[] data, Post post, EnumCountry country)
        {
            this.Image = data ?? throw new ArgumentException("Data not provided.");
            this.Post = post ?? throw new ArgumentException("Post not provided.");
            this.PostID = post.PostID;

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }   

        [Key]
        public long FileID { get; set; }

        public string FileName { get; set; }
        
        [Required]
        public byte[] Image { get; set; }

        public long? PostID { get; set; }

        [ForeignKey("PostID")]
        public Post Post { get; set; }       
        
        public string FileURL { get; set; }

        public EnumPhoto EnumPhoto { get; set; }
    }
}
