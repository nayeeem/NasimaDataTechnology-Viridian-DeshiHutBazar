using System;
using Common;

namespace WebDeshiHutBazar
{
    public class LogPostVisitViewModel 
    {
        public LogPostVisitViewModel() {
            
        }        

        public long PostVisitLogID { get; set; }

        public string VisitorEmail { get; set; }

        public string VisitorPhoneNumber { get; set; }

        public long PostID { get; set; }

        public string PostTitle { get; set; }

        public string CategoryName { get; set; }        

        public string SubCategoryName { get; set; }

        public decimal PostItemPrice { get; set; }

        public string StateName { get; set; }

        public string Area { get; set; }

        public string PosterName { get; set; }

        public string PosterPhoneNumber { get; set; }

        public DateTime LogDateTime { get; set; }

        public long AdvertiserUserID { get; set; }

        public string AdvertiserAccountEmail { get; set; }

        public string AdvertiserAccountClientName { get; set; }

        public byte[] PostImageFile1 { get; set; }

        public byte[] PostImageFile2 { get; set; }

        public byte[] PostImageFile3 { get; set; }

        public byte[] PostImageFile4 { get; set; }

        public EnumPostVisitAction PostVisitAction { get; set; }

    }
}
