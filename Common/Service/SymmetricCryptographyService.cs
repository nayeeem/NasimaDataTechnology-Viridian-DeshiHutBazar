using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Common;

namespace Common
{
    public class SymmetricCryptographyService
    {
        public byte[] GenerateRandomNumber(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length]; 
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        private byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC; 
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key; 
                aes.IV = iv;
                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length); 
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)     
        {                                         
            using (var aes = new AesCryptoServiceProvider())         
            {             
                aes.Mode = CipherMode.CBC;             
                aes.Padding = PaddingMode.PKCS7;  
                aes.Key = key;             
                aes.IV = iv;  
                using (var memoryStream = new MemoryStream())             
                {                                        
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);  
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);                 
                    cryptoStream.FlushFinalBlock();  
                    var decryptBytes = memoryStream.ToArray(); 
                    return decryptBytes;             
                }         
            }                                 
        }

        public string GetDecryptMessage(string messageToDecrypt,byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(messageToDecrypt), key, iv));
        }

        public MessageDigestModel GetEncryptMessage(string messageToEncrypt)
        {
            MessageDigestModel objMsgModel = new MessageDigestModel();
            var msgbytes = Encoding.UTF8.GetBytes(messageToEncrypt);
            var key = GenerateRandomNumber(32);
            var iv = GenerateRandomNumber(16);
            objMsgModel.Digest = Convert.ToBase64String(Encrypt(msgbytes, key, iv));
            objMsgModel.Key = key;
            objMsgModel.IV = iv;
            return objMsgModel;
        }
    }
}
