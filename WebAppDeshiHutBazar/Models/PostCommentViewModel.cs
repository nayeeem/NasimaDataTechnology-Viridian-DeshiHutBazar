using Common;

namespace WebDeshiHutBazar
{
    public class PostCommentViewModel :BaseViewModel
    {
        public PostCommentViewModel()
        {
        }   

        public long CommentID { get; set; }

        public string Comment { get; set; }
        
        public long? PostID { get; set; }

        public string CommentDate { get; set; }

        public long Like { get; set; }
    }
}
