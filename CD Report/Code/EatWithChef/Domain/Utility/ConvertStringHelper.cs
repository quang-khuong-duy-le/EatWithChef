using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utility
{
    public class ConvertStringHelper
    {
        //Generate authentication code by email.
        // define characters allowed in passcode.  set length so divisible into 256
        private static char[] ValidChars = {'2','3','4','5','6','7','8','9',
                   'A','B','C','D','E','F','G','H',
                   'J','K','L','M','N','P','Q',
                   'R','S','T','U','V','W','X','Y','Z'}; // len=32

        private const string hashkey = "password"; //key for HMAC function -- change!
        private const int codelength = 6; // lenth of passcode

        public static string GetCodeForEmail(string address)
        {
            byte[] hash;
            using (HMACSHA1 sha1 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(hashkey)))
                hash = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(address));
            int startpos = hash[hash.Length - 1] % (hash.Length - codelength);
            StringBuilder passbuilder = new StringBuilder();
            for (int i = startpos; i < startpos + codelength; i++)
                passbuilder.Append(ValidChars[hash[i] % ValidChars.Length]);
            return passbuilder.ToString();
        }

        public static String ConvertShortName(String strVietNamese)
        {
            //Loại bỏ dấu ':'
            char[] delimiter = { ':', '?', '"', '/', '!', ',', '-', '=', '%', '$', '&', '*' };
            String[] subString = strVietNamese.Split(delimiter);
            strVietNamese = "";
            for (int i = 0; i < subString.Length; i++)
            {
                strVietNamese += subString[i];
            }
            //Loại bỏ tiếng việt
            const string textToFind = " áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string textToReplace = "-aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            while ((index = strVietNamese.IndexOfAny(textToFind.ToCharArray())) != -1)
            {
                int index2 = textToFind.IndexOf(strVietNamese[index]);
                strVietNamese = strVietNamese.Replace(strVietNamese[index], textToReplace[index2]);
            }

            return strVietNamese.ToLower();
        }
    }
}
