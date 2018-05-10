using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace MailDemo
{
    public class EncodeUtil
    {
        public static string Encoder(string unicodeStr)
        {
            ASCIIEncoding myAscii = new ASCIIEncoding();
            //下面的代码将对unicodeStr字符串的内容进行编码。
            Byte[] encodeBytes = myAscii.GetBytes(unicodeStr);
            //下面的语句将对encodeBytes字节数组的内容进行解码
            string decodeStr = myAscii.GetString(encodeBytes);
            return decodeStr;
        }

        public static string GenSHA1(string strInput)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] bytesInput = System.Text.Encoding.Default.GetBytes(strInput);
            byte[] byteresult = sha.ComputeHash(bytesInput);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteresult)
            {
                sb.Append(b+" ");
            }
            return sb.ToString();
        }
    }
}
