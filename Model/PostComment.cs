using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
namespace Model
{
    public class PostComment : BaseEntity
    {
        public PostComment() { }

        public PostComment(string comment, Post post, EnumCountry country) {
            Comment = comment;
            EnumCountry = country;
            if (post != null)
            {
                PostID = post.PostID;
                Post = post;
            }

            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetCountryTimeZoneName(country));
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            CreatedDate = BaTime;
            ModifiedDate = BaTime;
            EnumCountry = country;
        }

        [Key]
        public long CommentID { get; set; }

        public string Comment { get; set; }

        public long? PostID { get; set; }

        [ForeignKey("PostID")]
        public Post Post { get; set; }

        public long Like { get; set; }
    }
}
