using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyEx.Controllers
{
    public class HMACController : Controller
    {
        public IActionResult HMACVerify()
        {
            // Create message to sign
            string message = "This message will have a HMAC calculated and included using a shared "
                + " random key so that the recipient can validate whether the message has "
                + " been tampered with";
            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Create random shared key
            byte[] sharedKey = new byte[64];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(sharedKey);

            // sign by computing SHA512 hash and prepending to message
            HMACSHA512 hmac = new HMACSHA512(sharedKey);
            byte[] sendersHash = hmac.ComputeHash(msg);

            // send hash followed by message
            MemoryStream ms = new MemoryStream(sendersHash.Length + msg.Length);
            ms.Write(sendersHash, 0, sendersHash.Length);
            ms.Write(msg, 0, msg.Length);

            // initialize hash function for recipient
            HMACSHA512 rHmac = new HMACSHA512(sharedKey);
            byte[] recipientsHash = new byte[rHmac.HashSize / 8];
            // read hash 
            ms.Position = 0;
            ms.Read(recipientsHash, 0, recipientsHash.Length);

            // read message & calculate hash of message
            int msgLength = (int)ms.Length - recipientsHash.Length;
            byte[] rMsg = new byte[msgLength];
            ms.Read(rMsg, 0, rMsg.Length);
            recipientsHash = hmac.ComputeHash(rMsg);

            // Verify the HMAC by comparing the hash of the message to the sender's hash 
            bool result = recipientsHash.SequenceEqual(sendersHash);
            // hash has been validated, restore and display the message
            string summary = "Signature invalid";
            if (result)
            {
                string rMessage = Encoding.UTF8.GetString(rMsg);
                string b64Hash = Convert.ToBase64String(recipientsHash);
                summary = $"Message: \n{rMessage} \nHash: \n{b64Hash}";
            }
            return Content(summary);
        }

    }
}
