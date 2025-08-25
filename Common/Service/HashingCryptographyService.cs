using System;
using System.Security.Cryptography;
using System.Text;
using Common;

namespace Common
{
    public class HashingCryptographyService
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public static byte[] HashMessage(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
            {
                return rfc2898.GetBytes(32);
            }
        }

        public MessageDigestModel GetMessageDigest(string msgToHash)
        {
            MessageDigestModel objDigestModel = new MessageDigestModel
            {
                Salt = HashingCryptographyService.GenerateSalt()
            };
            var hashedMsg = HashingCryptographyService.HashMessage(
                Encoding.UTF8.GetBytes(msgToHash),
                objDigestModel.Salt,
                10000);
            objDigestModel.Digest = Convert.ToBase64String(hashedMsg);
            return objDigestModel;
        }

        public MessageDigestModel GetMessageDigest(string msgToHash,byte[] salt)
        {
            MessageDigestModel objDigestModel = new MessageDigestModel();
            
            objDigestModel.Salt = salt;
            var hashedMsg = HashingCryptographyService.HashMessage(
                Encoding.UTF8.GetBytes(msgToHash),
                salt,
                10000);
            objDigestModel.Digest = Convert.ToBase64String(hashedMsg);
            return objDigestModel;
        }
    } 
}
