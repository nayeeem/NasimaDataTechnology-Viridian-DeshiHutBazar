using Common;

namespace WebDeshiHutBazar
{
    public class FileViewModel
    {
        public FileViewModel() { }

        public bool IsNewItem { get; set; }
        
        public long FileID { get; set; }

        public string FileName { get; set; }

        public byte[] Image { get; set; }

        public long? PostID { get; set; }

        public string FileType { get; set; }

        public EnumPhoto EnumPhoto { get; set; }
    }
}
