namespace Common
{
    public class MessageDigestModel
    {
        public string Digest { get; set; }

        public byte[] DigestByte { get; set; }

        public byte[] Salt { get; set; }

        public byte[] Key { get; set; }

        public byte[] IV { get; set; }
    }
}
