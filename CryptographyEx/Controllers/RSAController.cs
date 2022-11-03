using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CryptographyEx.Controllers
{
    public class RSAController : Controller
    {
        public IActionResult GenerateKeys()
        {
            RSA rsa = RSA.Create();
            byte[] rsaPrivate = rsa.ExportRSAPrivateKey();
            byte[] pkcs8Private = rsa.ExportPkcs8PrivateKey();
            byte[] rsaPublic = rsa.ExportRSAPublicKey();

            string rsaKeys = $"RSA Private: \n{Convert.ToBase64String(rsaPrivate)}"
                           + $"\n\nPKCS#8 Private: \n{Convert.ToBase64String(pkcs8Private)}"
                           + $"\n\nRSA Public: \n{Convert.ToBase64String(rsaPublic)}";

            return Content(rsaKeys);
        }

        public IActionResult EncryptDecryptSessionKey()
        {
            // Client generates the shared symmetric AES Key for session (would validate server identity/certificate first)
            Aes aes = Aes.Create();

            RSA rsa = RSA.Create();  // public and private keys are automatically created (previously)

            // Client encrypts the shared symmetric session AES key using the server’s public RSA key and sends to server
            byte[] encryptedAESKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);

            // Server decrypts using its private RSA key to recover the shared symmetric AES session key
            byte[] decryptedAESKey = rsa.Decrypt(encryptedAESKey, RSAEncryptionPadding.Pkcs1);

            string keyInfo = $"AES Session Key: \n{Convert.ToBase64String(aes.Key)}"
                              + $"\n\nEncrypted with the following RSA public Key: " + $"\n{Convert.ToBase64String(rsa.ExportRSAPublicKey())}"
                              + $"\n\nEncrypted AES Session Key: \n{Convert.ToBase64String(encryptedAESKey)}"
                              + $"\n\nDecrypted with the following RSA private Key: \n{Convert.ToBase64String(rsa.ExportRSAPrivateKey())}"
                              + $"\n\nDecrypted AES Session Key: \n{Convert.ToBase64String(decryptedAESKey)}";

            // Session communications encrypted henceforth using the shared symmetric AES session key
            return Content(keyInfo);
        }


    }
}
