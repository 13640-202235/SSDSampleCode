using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyEx.Controllers
{
    public class RSASigningController : Controller
    {
        public IActionResult SignAndVerify()
        {
            string message = "This message will be signed using my private RSA key so that " +
                             "the recipient can validate whether the message has been " +
                             "tampered with.";

            byte[] msg = Encoding.UTF8.GetBytes(message);

            //generate the RSA keys
            RSA rsa = RSA.Create();
            RSAParameters publicParameters = rsa.ExportParameters(false);

            //compute the hash of teh data and sign with RSA private key
            byte[] signature = rsa.SignData(msg, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            string b64Signature = Convert.ToBase64String(signature);


            //recipient , create RSA and use the sender public key
            RSA rRSA = RSA.Create();
            rRSA.ImportParameters(publicParameters);

            //verify the signature
            bool result = rRSA.VerifyData(msg, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            string summary = "Signature is invalid";

            if (result)
            {
                message = Encoding.UTF8.GetString(msg, 0, msg.Length);
                summary = $"Message: \n{message}\nSignature: \n{b64Signature}";
            }


            return Content(summary);
        }
    }
}
