using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Helper.Utils
{
    public static class MoneyToWords
    {
        private static string[] strNumber = new string[10]
        {
      "không",
      "một",
      "hai",
      "ba",
      "bốn",
      "năm",
      "sáu",
      "bảy",
      "tám",
      "chín"
        };
        private static string[] strMinUnit = new string[6]
        {
      "lẻ",
      "lăm",
      "mười",
      "mươi",
      "mốt",
      "trăm"
        };
        private static string[] strMaxUnit = new string[4]
        {
      "",
      "ngàn",
      "triệu",
      "tỷ"
        };
        private static string[] strMainGroup;
        private static string[] strSubGroup;

        private static string Len1(string strA)
        {
            return MoneyToWords.strNumber[int.Parse(strA)];
        }

        private static string Len2(string strA)
        {
            if (strA.Substring(0, 1) == "0")
                return MoneyToWords.strMinUnit[0] + " " + MoneyToWords.Len1(strA.Substring(1, 1));
            if (strA.Substring(0, 1) == "1")
            {
                if (strA.Substring(1, 1) == "5")
                    return MoneyToWords.strMinUnit[2] + " " + MoneyToWords.strMinUnit[1];
                if (strA.Substring(1, 1) == "0")
                    return MoneyToWords.strMinUnit[2];
                return MoneyToWords.strMinUnit[2] + " " + MoneyToWords.Len1(strA.Substring(1, 1));
            }
            if (strA.Substring(1, 1) == "5")
                return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[3] + " " + MoneyToWords.strMinUnit[1];
            if (strA.Substring(1, 1) == "0")
                return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[3];
            if (strA.Substring(1, 1) == "1")
                return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[3] + " " + MoneyToWords.strMinUnit[4];
            return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[3] + " " + MoneyToWords.Len1(strA.Substring(1, 1));
        }

        private static string Len3(string strA)
        {
            if (strA.Substring(0, 3) == "000")
                return (string)null;
            if (strA.Substring(1, 2) == "00")
                return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[5];
            return MoneyToWords.Len1(strA.Substring(0, 1)) + " " + MoneyToWords.strMinUnit[5] + " " + MoneyToWords.Len2(strA.Substring(1, strA.Length - 1));
        }

        private static string FullLen(string strSend)
        {
            bool flag = false;
            string str1 = "";
            string str2 = strSend.Trim();
            int startIndex1 = str2.Length - 9;
            int length = 0;
            if (strSend.Trim() == "")
                return MoneyToWords.Len1("0");
            for (int startIndex2 = 0; startIndex2 < str2.Length && !(str2.Substring(startIndex2, 1) != "0"); ++startIndex2)
            {
                if (startIndex2 == str2.Length - 1)
                    return MoneyToWords.strNumber[0];
            }
            int num = 0;
            while (strSend.Trim().Substring(num++, 1) == "0")
                str2 = str2.Remove(0, 1);
            if (str2.Length < 9)
                length = str2.Length;
            MoneyToWords.strMainGroup = (uint)(str2.Length % 9) <= 0U ? new string[str2.Length / 9] : new string[str2.Length / 9 + 1];
            for (int index = MoneyToWords.strMainGroup.Length - 1; index >= 0; --index)
            {
                if (startIndex1 >= 0)
                {
                    length = startIndex1;
                    MoneyToWords.strMainGroup[index] = str2.Substring(startIndex1, 9);
                    startIndex1 -= 9;
                }
                else
                    MoneyToWords.strMainGroup[index] = str2.Substring(0, length);
            }
            for (int index1 = 0; index1 < MoneyToWords.strMainGroup.Length; ++index1)
            {
                int startIndex2 = MoneyToWords.strMainGroup[index1].Length - 3;
                if (MoneyToWords.strMainGroup[index1].Length < 3)
                    length = MoneyToWords.strMainGroup[index1].Length;
                MoneyToWords.strSubGroup = (uint)(MoneyToWords.strMainGroup[index1].Length % 3) <= 0U ? new string[MoneyToWords.strMainGroup[index1].Length / 3] : new string[MoneyToWords.strMainGroup[index1].Length / 3 + 1];
                for (int index2 = MoneyToWords.strSubGroup.Length - 1; index2 >= 0; --index2)
                {
                    if (startIndex2 >= 0)
                    {
                        length = startIndex2;
                        MoneyToWords.strSubGroup[index2] = MoneyToWords.strMainGroup[index1].Substring(startIndex2, 3);
                        startIndex2 -= 3;
                    }
                    else
                        MoneyToWords.strSubGroup[index2] = MoneyToWords.strMainGroup[index1].Substring(0, length);
                }
                for (int index2 = 0; index2 < MoneyToWords.strSubGroup.Length; ++index2)
                {
                    flag = false;
                    if (index1 == MoneyToWords.strMainGroup.Length - 1 && index2 == MoneyToWords.strSubGroup.Length - 1)
                        str1 = MoneyToWords.strSubGroup[index2].Length >= 3 ? str1 + MoneyToWords.Len3(MoneyToWords.strSubGroup[index2]) : (MoneyToWords.strSubGroup[index2].Length != 1 ? str1 + MoneyToWords.Len2(MoneyToWords.strSubGroup[index2]) : str1 + MoneyToWords.Len1(MoneyToWords.strSubGroup[index2]));
                    else if (MoneyToWords.strSubGroup[index2].Length < 3)
                        str1 = MoneyToWords.strSubGroup[index2].Length != 1 ? str1 + MoneyToWords.Len2(MoneyToWords.strSubGroup[index2]) + " " : str1 + MoneyToWords.Len1(MoneyToWords.strSubGroup[index2]) + " ";
                    else if (MoneyToWords.Len3(MoneyToWords.strSubGroup[index2]) == null)
                        flag = true;
                    else
                        str1 = str1 + MoneyToWords.Len3(MoneyToWords.strSubGroup[index2]) + " ";
                    if (!flag)
                        str1 = (uint)(MoneyToWords.strSubGroup.Length - 1 - index2) <= 0U ? str1 + MoneyToWords.strMaxUnit[MoneyToWords.strSubGroup.Length - 1 - index2] + " " : str1 + MoneyToWords.strMaxUnit[MoneyToWords.strSubGroup.Length - 1 - index2] + " ";
                }
                if (index1 != MoneyToWords.strMainGroup.Length - 1)
                    str1 = flag ? str1.Substring(0, str1.Length - 1) + " " + MoneyToWords.strMaxUnit[3] + " " : str1.Substring(0, str1.Length - 1) + MoneyToWords.strMaxUnit[3] + " ";
            }
            string str3 = str1.Trim();
            if (str3.Substring(str3.Length - 1, 1) == ".")
                str3 = str3.Remove(str3.Length - 1, 1);
            return str3;
        }

        public static string Convert(string strSend)
        {
            strSend = strSend.Replace(',', '.');
            string str = MoneyToWords.Convert(strSend, '.');
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
                str = str[0].ToString().ToUpper() + str.Substring(1);
            return str;
        }

        public static string Convert(string strSend, char charInSeparator)
        {
            if (strSend == "")
                return MoneyToWords.Len1("0") + " đồng";
            string str1 = " phẩy";
            string[] strArray = new string[2];
            try
            {
                strArray = strSend.Split(charInSeparator);
                string str2 = strArray[1];
                for (int startIndex = str2.Length - 1; startIndex >= 0 && str2.Substring(startIndex, 1) == "0"; --startIndex)
                    str2 = str2.Remove(startIndex, 1);
                if (!(str2 != ""))
                    return MoneyToWords.FullLen(strArray[0]) + " đồng chẵn";
                string str3 = "";
                for (int startIndex = 0; startIndex < str2.Length; ++startIndex)
                    str3 = str3 + MoneyToWords.Len1(str2.Substring(startIndex, 1)) + " ";
                return MoneyToWords.FullLen(strArray[0]) + " " + str1 + " " + str3.TrimEnd() + " đồng";
            }
            catch
            {
                return MoneyToWords.FullLen(strArray[0]) + " đồng chẵn";
            }
        }
    }
}
