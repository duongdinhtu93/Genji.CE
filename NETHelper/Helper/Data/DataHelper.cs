using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects.DataClasses;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationCore.Helper
{
    public static class DataHelper
    {
        public static byte[] ToBinary(this IList<Guid> values)
        {
            if (values == null)
                return (byte[])null;
            return values.SelectMany<Guid, byte>((Func<Guid, IEnumerable<byte>>)(d => (IEnumerable<byte>)d.ToByteArray())).ToArray<byte>();
        }

        public static List<Guid> ToGuids(this byte[] values)
        {
            List<Guid> guidList = new List<Guid>();
            int length = Guid.Empty.ToByteArray().Length;
            if (values == null)
                return guidList;
            int count = 0;
            while (count < values.Length)
            {
                guidList.Add(new Guid(((IEnumerable<byte>)values).Skip<byte>(count).Take<byte>(length).ToArray<byte>()));
                count += length;
            }
            return guidList;
        }

        public static byte[] ToBinary(this IList<int> values)
        {
            if (values == null)
                return (byte[])null;
            if (values.Count<int>() == 0)
                return new byte[0];
            byte[] numArray = new byte[values.Max() / 8 + 1];
            foreach (int num1 in (IEnumerable<int>)values)
            {
                int index = num1 / 8;
                int num2 = num1 % 8;
                numArray[index] = (byte)((uint)numArray[index] | (uint)(1 << num2));
            }
            return numArray;
        }

        public static List<int> ToNumbers(this byte[] values)
        {
            List<int> intList = new List<int>();
            if (values == null)
                return intList;
            for (int index = 0; index < values.Length; ++index)
            {
                byte num1 = values[index];
                int num2 = 0;
                while ((int)num1 > 0)
                {
                    if ((int)num1 % 2 == 1)
                        intList.Add(num2 + index * 8);
                    num1 >>= 1;
                    ++num2;
                }
            }
            return intList;
        }

        public static bool IsContains(this byte[] numbers, int value)
        {
            if (value < 0 || numbers == null)
                return false;
            int index = value / 8;
            int num1 = value % 8;
            if (index >= numbers.Length)
                return false;
            byte num2 = (byte)(1 << num1);
            return ((int)numbers[index] & (int)num2) > 0;
        }

        public static bool Contains(this int sumValue, int value)
        {
            return (sumValue & value) == value;
        }

        public static byte[] SerializeData(this object data)
        {
            MemoryStream memoryStream = new MemoryStream();
            new BinaryFormatter().Serialize((Stream)memoryStream, data);
            return memoryStream.ToArray();
        }

        public static object DeserializeData(this byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            memoryStream.Position = 0L;
            return binaryFormatter.Deserialize((Stream)memoryStream);
        }

        public static string ToAscii(this string value)
        {
            value = value.GetString().Replace("đ", "d").Replace("Đ", "D");
            return new string(((IEnumerable<char>)value.Normalize(NormalizationForm.FormD).ToCharArray()).Where<char>((Func<char, bool>)(c => (int)c <= (int)sbyte.MaxValue)).ToArray<char>());
        }

        public static string Format(this object value, string formatString)
        {
            if (!formatString.Trim().StartsWith("{"))
                formatString = "{0:" + formatString + "}";
            return string.Format(formatString, value);
        }

        public static string ToUpperFirstOnly(this string value)
        {
            string str = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
                str = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
            return str;
        }

        public static string TrimAll(this string value)
        {
            string str = string.Empty;
            if (value != null)
            {
                str = value;
                while (str.Contains(" "))
                    str = str.Replace(" ", "");
            }
            return str;
        }

        public static string ReverseString(this string value)
        {
            string empty = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                for (int index = value.Length - 1; index >= 0; --index)
                    empty += value[index].ToString();
            }
            return empty;
        }

        public static string RemoveDoubleSpace(this string value)
        {
            string str = string.Empty;
            if (value != null)
            {
                str = value;
                while (str.Contains("  "))
                    str = str.Replace("  ", " ");
            }
            return str;
        }

        public static string RemoveEndsWith(this string value, string endValue)
        {
            string str = value;
            if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(endValue) && value.EndsWith(endValue))
                str = value.Substring(0, value.Length - endValue.Length);
            return str;
        }

        public static string RemoveStartsWith(this string value, string startValue)
        {
            string str = value;
            if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(startValue) && value.StartsWith(startValue))
                str = value.Substring(startValue.Length);
            return str;
        }

        public static string AddEndsWith(this string value, string endValue)
        {
            return value.GetString() + endValue.GetString();
        }

        public static string AddStartsWith(this string value, string startValue)
        {
            return startValue.GetString() + value.GetString();
        }

        public static string ToSplitString(this string value)
        {
            string str1 = string.Empty;
            char ch;
            for (int index = 0; index < value.Length; ++index)
            {
                if (index == 0)
                {
                    string str2 = str1;
                    ch = value[index];
                    string str3 = ch.ToString();
                    str1 = str2 + str3;
                }
                else if ((int)value[index] >= 65 && (int)value[index] <= 90)
                {
                    if ((int)value[index - 1] < 65 || (int)value[index - 1] > 90)
                    {
                        string str2 = str1;
                        string str3 = " ";
                        ch = value[index];
                        string str4 = ch.ToString();
                        str1 = str2 + str3 + str4;
                    }
                    else
                    {
                        string str2 = str1;
                        ch = value[index];
                        string str3 = ch.ToString();
                        str1 = str2 + str3;
                    }
                }
                else
                {
                    string str2 = str1;
                    ch = value[index];
                    string str3 = ch.ToString();
                    str1 = str2 + str3;
                }
            }
            return str1.Replace("( ", " (").Replace(" )", ") ").Replace("{ ", " {").Replace(" }", "} ").Replace("[ ", " [").Replace(" ]", "] ").Replace(" :", ": ").Replace(" ;", "; ").Replace(" .", ". ").Replace(" ,", ", ").Replace(" ?", "? ").Replace("  ", " ");
        }

        public static string ToSplitString(this IList listObject)
        {
            return listObject.ToSplitString(',');
        }

        public static string ToSplitString(this IList listObject, char splitChar)
        {
            return listObject.ToSplitString(string.Empty, string.Empty, splitChar);
        }

        public static string ToSplitString(this IList listObject, string coverString, string removeString)
        {
            return listObject.ToSplitString(coverString, removeString, ',');
        }

        public static string ToSplitString(this IList listObject, string coverString, string removeString, char splitChar)
        {
            string str1 = string.Empty;
            if (string.IsNullOrWhiteSpace(splitChar.GetString()))
                splitChar = ',';
            if (listObject != null)
            {
                foreach (object obj in (IEnumerable)listObject)
                {
                    if (!string.IsNullOrEmpty(str1))
                        str1 += splitChar.ToString();
                    string str2 = obj.GetString();
                    coverString = coverString.GetString();
                    if (!string.IsNullOrWhiteSpace(removeString))
                        str2 = str2.Replace(removeString, string.Empty);
                    str1 = str1 + coverString + str2 + coverString;
                }
            }
            return str1;
        }

        public static List<T> FromSplitString<T>(this string splitString, params char[] splitChars)
        {
            List<T> objList = new List<T>();
            if (!string.IsNullOrWhiteSpace(splitString))
            {
                if (splitChars == null || ((IEnumerable<char>)splitChars).Count<char>() == 0)
                    splitChars = new char[1] { ',' };
                foreach (string str in splitString.Split(splitChars))
                    objList.Add(str.TryGetValue<T>());
            }
            return objList;
        }

        public static string GetString(this object value)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        public static bool GetBoolean(this bool? value)
        {
            return value.HasValue && value.Value;
        }

        public static int GetInteger(this int? value)
        {
            return value.HasValue ? value.Value : 0;
        }

        public static short GetShort(this short? value)
        {
            return value.HasValue ? value.Value : (short)0;
        }

        public static long GetLong(this long? value)
        {
            return value.HasValue ? value.Value : 0L;
        }

        public static Decimal GetDecimal(this Decimal? value)
        {
            return value.HasValue ? value.Value : Decimal.Zero;
        }

        public static double GetDouble(this double? value)
        {
            return value.HasValue ? value.Value : 0.0;
        }

        public static float GetFloat(this float? value)
        {
            return value.HasValue ? value.Value : 0.0f;
        }

        public static Guid GetGuid(this Guid? value)
        {
            return value.HasValue ? value.Value : Guid.Empty;
        }

        public static Guid ReverseGuid(this Guid guidValue)
        {
            return new Guid(BitConverter.ToString(guidValue.ToByteArray()).Replace("-", string.Empty));
        }

        public static Guid ReverseGuid(this string guidString)
        {
            return guidString.TryGetValue<Guid>().ReverseGuid();
        }

        public static TimeSpan GetTimeSpan(this TimeSpan? value)
        {
            return value.HasValue ? value.Value : TimeSpan.Zero;
        }

        public static DateTime GetDateTime(this DateTime? value)
        {
            return value.HasValue ? value.Value : DataHelper.GetEmptyDate();
        }

        public static DateTime GetEmptyDate()
        {
            return new DateTime(0L);
        }

        public static object RandomValue(this Type propertyType)
        {
            return !propertyType.IsInteger() && !propertyType.IsShort() && (!propertyType.IsLong() && !propertyType.IsFloat()) && !propertyType.IsDouble() && !propertyType.IsDecimal() ? (!propertyType.IsGuid() ? (!propertyType.IsDateTime() ? ((object)null).TryGetValue(propertyType) : (object)DateTime.Now) : (object)Guid.NewGuid()) : (object)new Random().Next(0, int.MaxValue);
        }

        public static object TryGetValueOrMin(this object value, Type propertyType)
        {
            object obj = value.TryGetValue(propertyType);
            if (value == null)
            {
                if (propertyType.IsDateTime())
                    obj = (object)SqlDateTime.MinValue.Value;
                else if (propertyType.IsInteger())
                    obj = (object)int.MinValue;
                else if (propertyType.IsDecimal())
                    obj = (object)new Decimal(-1, -1, -1, true, (byte)0);
                else if (propertyType.IsFloat())
                    obj = (object)float.MinValue;
                else if (propertyType.IsDouble())
                    obj = (object)double.MinValue;
                else if (propertyType.IsShort())
                    obj = (object)short.MinValue;
                else if (propertyType.IsLong())
                    obj = (object)long.MinValue;
            }
            return obj;
        }

        public static object TryGetValueOrMax(this object value, Type propertyType)
        {
            object obj = value.TryGetValue(propertyType);
            if (value == null)
            {
                if (propertyType.IsDateTime())
                    obj = (object)SqlDateTime.MaxValue.Value;
                else if (propertyType.IsInteger())
                    obj = (object)int.MaxValue;
                else if (propertyType.IsDecimal())
                    obj = (object)new Decimal(-1, -1, -1, false, (byte)0);
                else if (propertyType.IsFloat())
                    obj = (object)float.MaxValue;
                else if (propertyType.IsDouble())
                    obj = (object)double.MaxValue;
                else if (propertyType.IsShort())
                    obj = (object)short.MaxValue;
                else if (propertyType.IsLong())
                    obj = (object)long.MaxValue;
            }
            return obj;
        }

        public static object TryGetValueOrDefault(this object value, Type propertyType)
        {
            object now = value.TryGetValue(propertyType);
            if (value == null && propertyType.IsDateTime())
                now = (object)DateTime.Now;
            return now;
        }

        public static T TryGetValueOrMin<T>(this object value)
        {
            return (T)value.TryGetValueOrMin(typeof(T));
        }

        public static T TryGetValueOrMax<T>(this object value)
        {
            return (T)value.TryGetValueOrMax(typeof(T));
        }

        public static T TryGetValueOrDefault<T>(this object value)
        {
            return (T)value.TryGetValueOrDefault(typeof(T));
        }

        public static T TryGetValue<T>(this object value)
        {
            object obj = value.TryGetValue(typeof(T));
            return obj != null ? (T)obj : default(T);
        }

        public static T TryGetValue<T>(this object value, out bool invalidFormat)
        {
            object obj = value.TryGetValue(typeof(T), out invalidFormat);
            return obj != null ? (T)obj : default(T);
        }

        public static object TryGetValue(this object value, Type propertyType)
        {
            bool invalidFormat = false;
            return value.TryGetValue(propertyType, out invalidFormat);
        }

        public static object TryGetValue(this object value, Type propertyType, out bool invalidFormat)
        {
            object obj = (object)null;
            invalidFormat = false;
            if (value.GetString().ToLower().Equals("null"))
                obj = (object)null;
            else if (propertyType != (Type)null)
            {
                if (value != null && value.GetType().GetRealPropertyType() == propertyType.GetRealPropertyType())
                    obj = value;
                else if (propertyType.IsEnum)
                    obj = Enum.Parse(propertyType, value.GetString());
                else if (propertyType.IsBoolean())
                {
                    if (value.GetString().TrimAll() == "1")
                        value = (object)"true";
                    else if (value.GetString().TrimAll() == "0")
                        value = (object)"false";
                    bool result = false;
                    if (bool.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(bool))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsInteger())
                {
                    int result = 0;
                    if (int.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(int))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsShort())
                {
                    short result = 0;
                    if (short.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(short))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsLong())
                {
                    long result = 0;
                    if (long.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(long))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDecimal())
                {
                    Decimal result = new Decimal();
                    if (Decimal.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(Decimal))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsFloat())
                {
                    float result = 0.0f;
                    if (float.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(float))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDouble())
                {
                    double result = 0.0;
                    if (double.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(double))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsGuid())
                {
                    Guid result = Guid.Empty;
                    if (Guid.TryParse(value.GetString().TrimAll(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(Guid))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsDateTime())
                {
                    DateTime result = DateTime.MinValue;
                    if (DateTime.TryParse(value.GetString(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(DateTime))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType.IsTimeSpan())
                {
                    TimeSpan result = DateTime.Now.TimeOfDay;
                    if (TimeSpan.TryParse(value.GetString(), out result))
                    {
                        obj = (object)result;
                    }
                    else
                    {
                        if (propertyType == typeof(TimeSpan))
                            obj = (object)result;
                        invalidFormat = true;
                    }
                }
                else if (propertyType == typeof(string))
                {
                    if (value != null)
                        obj = (object)value.GetString();
                }
                else
                    obj = value;
            }
            else
                obj = value;
            return obj;
        }

        public static DateTime TryGetValue(this object value, string dateFormat)
        {
            bool invalidFormat = false;
            return value.TryGetValue(dateFormat, out invalidFormat);
        }

        public static DateTime TryGetValue(this object value, string dateFormat, out bool invalidFormat)
        {
            DateTime result = DateTime.Now;
            if (string.IsNullOrWhiteSpace(dateFormat))
                result = value.TryGetValue<DateTime>(out invalidFormat);
            else
                invalidFormat = !DateTime.TryParseExact(value.GetString(), dateFormat, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return result;
        }

        public static bool HasChanged(this object originalValue, object currentValue)
        {
            return originalValue.HasChanged(currentValue, true);
        }

        public static bool HasChanged(this object originalValue, object currentValue, bool nullToZero)
        {
            bool flag = false;
            if (originalValue == null && (currentValue == null || !nullToZero && currentValue.TryGetValue<Decimal>() == Decimal.Zero))
                flag = false;
            else if (originalValue == null && currentValue != null)
                flag = !(currentValue.GetType() == typeof(string)) || !string.IsNullOrWhiteSpace(currentValue.GetString());
            else if (originalValue != null && currentValue == null)
                flag = !(originalValue.GetType() == typeof(string)) || !string.IsNullOrWhiteSpace(originalValue.GetString());
            else if (!originalValue.Equals(currentValue) && originalValue.GetHashCode() != currentValue.GetHashCode())
                flag = true;
            return flag;
        }

        public static bool IsEquals(this Array array1, Array array2)
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null || array2 == null || array1.GetType() != array2.GetType() || array1.Length != array2.Length)
                return false;
            for (int index = 0; index < array1.Length; ++index)
            {
                if (array1.GetValue(index).HasChanged(array2.GetValue(index)))
                    return false;
            }
            return true;
        }

        public static bool IsOverlap(DateTime? dateFrom1, DateTime? dateTo1, DateTime? dateFrom2, DateTime? dateTo2)
        {
            DateTime? overlapFrom = new DateTime?();
            DateTime? overlapTo = new DateTime?();
            return DataHelper.IsOverlap(dateFrom1, dateTo1, dateFrom2, dateTo2, out overlapFrom, out overlapTo);
        }

        public static bool IsOverlap(DateTime? dateFrom1, DateTime? dateTo1, DateTime? dateFrom2, DateTime? dateTo2, out DateTime? overlapFrom, out DateTime? overlapTo)
        {
            dateFrom1 = dateFrom1.HasValue ? dateFrom1 : new DateTime?(SqlDateTime.MinValue.Value);
            dateFrom2 = dateFrom2.HasValue ? dateFrom2 : new DateTime?(SqlDateTime.MinValue.Value);
            dateTo1 = dateTo1.HasValue ? dateTo1 : new DateTime?(SqlDateTime.MaxValue.Value);
            dateTo2 = dateTo2.HasValue ? dateTo2 : new DateTime?(SqlDateTime.MaxValue.Value);
            if (dateFrom1.Value <= dateTo2.Value && dateTo1.Value >= dateFrom2.Value)
            {
                overlapFrom = new DateTime?(dateFrom1.Value > dateFrom2.Value ? dateFrom1.Value : dateFrom2.Value);
                overlapTo = new DateTime?(dateTo1.Value < dateTo2.Value ? dateTo1.Value : dateTo2.Value);
                return true;
            }
            overlapFrom = new DateTime?();
            overlapTo = new DateTime?();
            return false;
        }

        public static bool IsOverlap(DateTime? dateFrom1, DateTime? dateTo1, DateTime? dateFrom2, DateTime? dateTo2, out TimeSpan overlap)
        {
            DateTime? overlapFrom = new DateTime?();
            DateTime? overlapTo = new DateTime?();
            overlap = new TimeSpan();
            if (!DataHelper.IsOverlap(dateFrom1, dateTo1, dateFrom2, dateTo2, out overlapFrom, out overlapTo))
                return false;
            overlap = overlapTo.Value.Subtract(overlapFrom.Value);
            return true;
        }

        public static int Compare(this object value1, object value2)
        {
            if (value1 == value2)
                return 0;
            if (value1 == null && value2 != null)
                return -1;
            if (value1 != null && value2 == null)
                return 1;
            if (value1 != null && value2 != null)
            {
                Type type = value1.GetType();
                if (type == typeof(string) || type == typeof(char))
                    return value1.TryGetValue<string>().CompareTo(value2);
                if (type == typeof(int) || type == typeof(int?))
                    return value1.TryGetValue<int>().CompareTo(value2);
                if (type == typeof(Decimal) || type == typeof(Decimal?))
                    return value1.TryGetValue<Decimal>().CompareTo(value2);
                if (type == typeof(double) || type == typeof(double?))
                    return value1.TryGetValue<double>().CompareTo(value2);
                if (type == typeof(float) || type == typeof(float?))
                    return value1.TryGetValue<float>().CompareTo(value2);
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                    return value1.TryGetValue<DateTime>().CompareTo(value2);
                if (type == typeof(bool) || type == typeof(bool?))
                    return value1.TryGetValue<bool>().CompareTo(value2);
                if (type == typeof(Guid) || type == typeof(Guid?))
                    return value1.TryGetValue<Guid>().CompareTo(value2);
                if (type == typeof(byte) || type == typeof(byte?))
                    return Convert.ToByte(value1.ToString()).CompareTo(value2);
                if (type == typeof(sbyte) || type == typeof(sbyte?))
                    return Convert.ToSByte(value1.ToString()).CompareTo(value2);
            }
            return 0;
        }

        public static int GetPositiveInteger(this int value)
        {
            return value < 0 ? -1 * value : value;
        }

        public static long GetPositiveLong(this long value)
        {
            return value < 0L ? -1L * value : value;
        }

        public static Decimal GetPositiveDecimal(this Decimal value)
        {
            return value < Decimal.Zero ? Decimal.MinusOne * value : value;
        }

        public static double GetPositiveDouble(this double value)
        {
            return value < 0.0 ? -1.0 * value : value;
        }

        public static float GetPositiveFloat(this float value)
        {
            return (double)value < 0.0 ? -1f * value : value;
        }

        public static bool IsNull(this object value)
        {
            return value == null || value == DBNull.Value;
        }

        public static bool IsNullOrEmpty(this object value)
        {
            return value == null || string.IsNullOrWhiteSpace(value.ToString());
        }

        public static bool IsBoolean(this Type value)
        {
            return value != (Type)null && (value == typeof(bool) || value == typeof(bool?));
        }

        public static bool IsInteger(this Type value)
        {
            return value != (Type)null && (value == typeof(int) || value == typeof(int?));
        }

        public static bool IsShort(this Type value)
        {
            return value != (Type)null && (value == typeof(short) || value == typeof(short?) || value == typeof(short) || value == typeof(short?));
        }

        public static bool IsLong(this Type value)
        {
            return value != (Type)null && (value == typeof(long) || value == typeof(long?) || value == typeof(long) || value == typeof(long?));
        }

        public static bool IsDecimal(this Type value)
        {
            return value != (Type)null && (value == typeof(Decimal) || value == typeof(Decimal?));
        }

        public static bool IsDouble(this Type value)
        {
            return value != (Type)null && (value == typeof(double) || value == typeof(double?));
        }

        public static bool IsFloat(this Type value)
        {
            return value != (Type)null && (value == typeof(float) || value == typeof(float?));
        }

        public static bool IsGuid(this Type value)
        {
            return value != (Type)null && (value == typeof(Guid) || value == typeof(Guid?));
        }

        public static bool IsDateTime(this Type value)
        {
            return value != (Type)null && (value == typeof(DateTime) || value == typeof(DateTime?));
        }

        public static bool IsTimeSpan(this Type value)
        {
            return value != (Type)null && (value == typeof(TimeSpan) || value == typeof(TimeSpan?));
        }

        public static bool IsNullable(this Type value)
        {
            try
            {
                return value.IsGenericType && value.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool HasProperty(this Type entityType, string propertyName, out Type propertyType)
        {
            bool flag = false;
            propertyType = (Type)null;
            if (entityType != (Type)null && !string.IsNullOrWhiteSpace(propertyName))
            {
                string str = propertyName;
                char[] chArray = new char[1] { '.' };
                foreach (string name in str.Split(chArray))
                {
                    PropertyInfo property = entityType.GetProperty(name);
                    propertyType = property != (PropertyInfo)null ? property.PropertyType : (Type)null;
                    entityType = property != (PropertyInfo)null ? property.PropertyType : (Type)null;
                    flag = property != (PropertyInfo)null;
                    if (!flag)
                        break;
                }
            }
            return flag;
        }

        public static bool HasProperty(this Type entityType, string propertyName)
        {
            Type propertyType = (Type)null;
            return entityType.HasProperty(propertyName, out propertyType);
        }

        public static bool HasProperty(this object entity, string propertyName)
        {
            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
                return DataHelper.HasProperty(entity.GetType(), propertyName);
            return false;
        }

        public static Type GetPropertyType(this Type entityType, string propertyName)
        {
            Type propertyType = (Type)null;
            entityType.HasProperty(propertyName, out propertyType);
            return propertyType;
        }

        public static Type GetPropertyType(this object entity, string propertyName)
        {
            if (entity == null || string.IsNullOrWhiteSpace(propertyName))
                return (Type)null;
            Type propertyType = (Type)null;
            entity.GetType().HasProperty(propertyName, out propertyType);
            return propertyType;
        }

        public static Type GetRealPropertyType(this object entity, string propertyName)
        {
            return entity.GetPropertyType(propertyName).GetRealPropertyType();
        }

        public static Type GetRealPropertyType(this Type propertyType)
        {
            if (propertyType.IsGuid())
                return typeof(Guid);
            if (propertyType.IsInteger())
                return typeof(int);
            if (propertyType.IsShort())
                return typeof(short);
            if (propertyType.IsLong())
                return typeof(long);
            if (propertyType.IsDouble())
                return typeof(double);
            if (propertyType.IsDecimal())
                return typeof(Decimal);
            if (propertyType.IsFloat())
                return typeof(float);
            if (propertyType.IsDateTime())
                return typeof(DateTime);
            if (propertyType.IsBoolean())
                return typeof(bool);
            return propertyType;
        }

        public static Type GetRealPropertyType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetRealPropertyType();
        }

        public static Type GetNullablePropertyType(this object entity, string propertyName)
        {
            return entity.GetPropertyType(propertyName).GetNullablePropertyType();
        }

        public static Type GetNullablePropertyType(this Type propertyType)
        {
            if (propertyType.IsGuid())
                return typeof(Guid?);
            if (propertyType.IsInteger())
                return typeof(int?);
            if (propertyType.IsShort())
                return typeof(short?);
            if (propertyType.IsLong())
                return typeof(long?);
            if (propertyType.IsDouble())
                return typeof(double?);
            if (propertyType.IsDecimal())
                return typeof(Decimal?);
            if (propertyType.IsFloat())
                return typeof(float?);
            if (propertyType.IsDateTime())
                return typeof(DateTime?);
            if (propertyType.IsBoolean())
                return typeof(bool?);
            return propertyType;
        }

        public static object GetFieldValue(this object entity, string fieldName)
        {
            return entity.GetFieldValue(fieldName, BindingFlags.Default);
        }

        public static object GetFieldValue(this object entity, string fieldName, BindingFlags bindingAttr)
        {
            object obj = (object)null;
            if (entity != null && !string.IsNullOrWhiteSpace(fieldName))
            {
                FieldInfo fieldInfo = bindingAttr != BindingFlags.Default ? entity.GetType().GetField(fieldName, bindingAttr) : entity.GetType().GetField(fieldName);
                if (fieldInfo != (FieldInfo)null)
                    obj = fieldInfo.GetValue(entity);
            }
            return obj;
        }

        public static object GetPropertyValue(this object entity, string propertyName)
        {
            return entity.GetPropertyValue(propertyName, BindingFlags.Default);
        }

        public static object GetPropertyValue(this object entity, string propertyName, BindingFlags bindingAttr)
        {
            object obj = (object)null;
            if (entity != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                string str = propertyName;
                char[] chArray = new char[1] { '.' };
                foreach (string name in str.Split(chArray))
                {
                    PropertyInfo propertyInfo = bindingAttr != BindingFlags.Default ? entity.GetType().GetProperty(name, bindingAttr) : entity.GetType().GetProperty(name);
                    if (propertyInfo != (PropertyInfo)null && propertyInfo.CanRead)
                    {
                        obj = entity = propertyInfo.GetValue(entity, (object[])null);
                        if (entity == null)
                            break;
                    }
                    else
                    {
                        obj = (object)null;
                        break;
                    }
                }
            }
            return obj;
        }

        public static void SetPropertyValue(this object entity, string propertyName, object value)
        {
            entity.SetPropertyValue(propertyName, value, BindingFlags.Default);
        }

        public static void SetPropertyValue(this object entity, string propertyName, object value, BindingFlags bindingAttr)
        {
            if (entity == null || string.IsNullOrWhiteSpace(propertyName))
                return;
            PropertyInfo propertyInfo = bindingAttr != BindingFlags.Default ? entity.GetType().GetProperty(propertyName, bindingAttr) : entity.GetType().GetProperty(propertyName);
            entity.SetPropertyValue(propertyInfo, value);
        }

        public static void SetPropertyValue(this object entity, PropertyInfo propertyInfo, object value)
        {
            if (entity == null || !(propertyInfo != (PropertyInfo)null) || !propertyInfo.CanWrite)
                return;
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && propertyInfo.PropertyType == typeof(XElement))
                value = (object)XElement.Parse(value.ToString());
            if (propertyInfo.PropertyType.IsEnum)
                value = Enum.ToObject(propertyInfo.PropertyType, value);
            value = value == DBNull.Value ? (object)null : value;
            if (value != null && propertyInfo != (PropertyInfo)null)
                value = value.TryGetValue(propertyInfo.PropertyType);
            propertyInfo.SetValue(entity, value, (object[])null);
        }

        public static void SetFieldValue(this object entity, string fieldName, object value)
        {
            entity.SetFieldValue(fieldName, value, BindingFlags.Default);
        }

        public static void SetFieldValue(this object entity, string fieldName, object value, BindingFlags bindingAttr)
        {
            if (entity == null || string.IsNullOrWhiteSpace(fieldName))
                return;
            FieldInfo fieldInfo = bindingAttr != BindingFlags.Default ? entity.GetType().GetField(fieldName, bindingAttr) : entity.GetType().GetField(fieldName);
            entity.SetFieldValue(fieldInfo, value);
        }

        public static void SetFieldValue(this object entity, FieldInfo fieldInfo, object value)
        {
            if (entity == null || !(fieldInfo != (FieldInfo)null))
                return;
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && fieldInfo.FieldType == typeof(XElement))
                value = (object)XElement.Parse(value.ToString());
            if (fieldInfo.FieldType.IsEnum)
                value = Enum.ToObject(fieldInfo.FieldType, value);
            value = value == DBNull.Value ? (object)null : value;
            if (value != null && fieldInfo != (FieldInfo)null)
                value = value.TryGetValue(fieldInfo.FieldType);
            fieldInfo.SetValue(entity, value);
        }

        public static object InvokeMethod(this object entity, string methodName, params object[] parameters)
        {
            return entity.InvokeMethod(methodName, BindingFlags.Default, parameters);
        }

        public static object InvokeMethod(this object entity, string methodName, BindingFlags bindingAttr, params object[] parameters)
        {
            object obj = (object)null;
            if (entity != null && !string.IsNullOrWhiteSpace(methodName))
                obj = (bindingAttr != BindingFlags.Default ? (MethodBase)entity.GetType().GetMethod(methodName, bindingAttr) : (MethodBase)entity.GetType().GetMethod(methodName)).Invoke(entity, parameters);
            return obj;
        }

        public static bool HasMethod(this Type entityType, string methodName)
        {
            bool flag = false;
            if (entityType != (Type)null && !string.IsNullOrWhiteSpace(methodName))
                flag = entityType.GetMethod(methodName) != (MethodInfo)null;
            return flag;
        }

        public static IList CreateList(this Type elementType)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
        }

        public static object CreateInstance(this Type elementType)
        {
            return Activator.CreateInstance(elementType);
        }

        public static object CreateInstance(this Type elementType, params object[] args)
        {
            return Activator.CreateInstance(elementType, args);
        }

        public static bool IsMemberOf(this Type elementType, Type parentType)
        {
            return elementType.GetInterface(parentType.Name) != (Type)null || elementType.IsSubclassOf(parentType);
        }

        public static bool IsTypeOf(this Type elementType, Type objectType)
        {
            return elementType == objectType || elementType.IsMemberOf(objectType);
        }

        public static bool IsTypeOf(this object value, Type objectType)
        {
            return value != null && DataHelper.IsTypeOf(value.GetType(), objectType);
        }

        public static Type GetElementType(this IEnumerable list)
        {
            Type type = (Type)null;
            if (list != null)
            {
                foreach (object obj in list)
                {
                    if (obj != null)
                    {
                        type = obj.GetType();
                        break;
                    }
                }
                if (type == (Type)null)
                    type = list.AsQueryable().ElementType;
            }
            return type;
        }

        public static Type GetRealEntityType(this Type entityType)
        {
            if (entityType != (Type)null && entityType.Namespace.EndsWith("DynamicProxies"))
                entityType = entityType.BaseType;
            return entityType;
        }
        
        public static object CopyData(this object objectSource, params string[] excludedFields)
        {
            object objectResult = (object)null;
            if (objectSource != null)
            {
                objectResult = objectSource.GetType().CreateInstance();
                objectSource.CopyData(objectResult, excludedFields);
            }
            return objectResult;
        }

        public static TEntity CopyData<TEntity>(this object objectSource, params string[] excludedFields)
        {
            return objectSource.CopyData<TEntity>((Dictionary<string, string>)null, excludedFields);
        }

        public static TEntity CopyData<TEntity>(this object objectSource, Dictionary<string, string> mappingFields, params string[] excludedFields)
        {
            TEntity instance = Activator.CreateInstance<TEntity>();
            objectSource.CopyData((object)instance, mappingFields, excludedFields);
            return instance;
        }

        public static void CopyData(this object objectSource, object objectResult, params string[] excludedFields)
        {
            objectSource.CopyData(objectResult, (Dictionary<string, string>)null, excludedFields);
        }

        public static void CopyData(this object objectSource, object objectResult, Dictionary<string, string> mappingFields, params string[] excludedFields)
        {
            if (objectSource == null || objectResult == null)
                return;
            Type type1 = objectSource.GetType();
            Type type2 = objectResult.GetType();
            PropertyInfo[] properties1 = type1.GetProperties();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (PropertyInfo propertyInfo1 in properties1)
            {
                PropertyInfo sourceProperty = propertyInfo1;
                if (sourceProperty != (PropertyInfo)null && sourceProperty.CanRead)
                {
                    string propertyName = sourceProperty.Name;
                    if (mappingFields != null && mappingFields.ContainsKey(propertyName))
                        propertyName = mappingFields[propertyName].TrimAll();
                    PropertyInfo propertyInfo2 = ((IEnumerable<PropertyInfo>)properties2).Where<PropertyInfo>((Func<PropertyInfo, bool>)(d =>
                    {
                        if (d.Name == propertyName && d.CanWrite)
                            return d.GetRealPropertyType() == sourceProperty.GetRealPropertyType();
                        return false;
                    })).FirstOrDefault<PropertyInfo>();
                    if (propertyInfo2 != (PropertyInfo)null && (!DataHelper.IsTypeOf(propertyInfo2.PropertyType, typeof(EntityObject)) && !DataHelper.IsTypeOf(propertyInfo2.PropertyType, typeof(RelatedEnd)) && !DataHelper.IsTypeOf(propertyInfo2.PropertyType, typeof(EntityState)) && !DataHelper.IsTypeOf(propertyInfo2.PropertyType, typeof(EntityKey))))
                    {
                        if (excludedFields != null && ((IEnumerable<string>)excludedFields).Count<string>() > 0 && ((IEnumerable<string>)excludedFields).Contains<string>(propertyInfo2.Name))
                            propertyInfo2 = (PropertyInfo)null;
                        if (propertyInfo2 != (PropertyInfo)null && propertyInfo2.CanWrite)
                        {
                            object propertyValue1 = objectSource.GetPropertyValue(sourceProperty.Name);
                            object propertyValue2 = objectResult.GetPropertyValue(propertyInfo2.Name);
                            if (propertyValue1.HasChanged(propertyValue2))
                                objectResult.SetPropertyValue(propertyInfo2, propertyValue1);
                        }
                    }
                }
            }
        }

        public static string CreateDateString(int? year, int? month, int? day)
        {
            return DataHelper.CreateDateString(year, month, day, string.Empty);
        }

        public static string CreateDateString(int? year, int? month, int? day, string format)
        {
            string str = string.Empty;
            try
            {
                if (year.GetInteger() > 0 && month.GetInteger() > 0 && day.GetInteger() > 0)
                {
                    format = string.IsNullOrWhiteSpace(format) ? "dd/MM/yyyy" : format;
                    DateTime dateTime = new DateTime(year.GetInteger(), month.GetInteger(), day.GetInteger());
                    str = (SqlDateTime)dateTime > SqlDateTime.MinValue ? dateTime.ToString(format) : string.Empty;
                }
                else if (year.GetInteger() > 0 && month.GetInteger() > 0)
                {
                    DateTime dateTime = new DateTime(year.GetInteger(), month.GetInteger(), 1);
                    str = (SqlDateTime)dateTime > SqlDateTime.MinValue ? dateTime.ToString("MM/yyyy") : string.Empty;
                }
                else if (month.GetInteger() > 0 && day.GetInteger() > 0)
                {
                    format = string.IsNullOrWhiteSpace(format) ? "dd/MM" : format.Replace("/yyyy", "");
                    DateTime dateTime = new DateTime(2014, month.GetInteger(), day.GetInteger());
                    str = (SqlDateTime)dateTime > SqlDateTime.MinValue ? dateTime.ToString(format) : string.Empty;
                }
                else if (year.GetInteger() > 0)
                    str = year.GetInteger().ToString();
            }
            catch (Exception ex)
            {
                str = string.Empty;
            }
            return str;
        }

        public static string GetStringFields(params object[] valueFields)
        {
            return DataHelper.GetStringFields(-1, valueFields);
        }

        public static string GetStringFields(int maxLength, params object[] valueFields)
        {
            string str = string.Empty;
            foreach (object valueField in valueFields)
            {
                if (!string.IsNullOrWhiteSpace(valueField.GetString()))
                {
                    if (!string.IsNullOrWhiteSpace(str))
                        str += " - ";
                    str += valueField.GetString().TrimAll();
                }
            }
            if (maxLength > 0 && str.Length > maxLength)
                str = str.Substring(0, maxLength).TrimAll() + "...";
            return str;
        }

        public static void RemoveRelated(this object entity)
        {
            if (entity == null || !DataHelper.IsTypeOf(entity.GetType(), typeof(EntityObject)))
                return;
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (property.CanWrite)
                {
                    if (DataHelper.IsTypeOf(property.PropertyType, typeof(Guid)))
                        entity.SetPropertyValue(property, (object)Guid.Empty);
                    else if (DataHelper.IsTypeOf(property.PropertyType, typeof(EntityObject)) || DataHelper.IsTypeOf(property.PropertyType, typeof(Guid?)))
                        entity.SetPropertyValue(property, (object)null);
                }
            }
        }

        public static DataTable ToPivotTable(this Dictionary<string, object> values)
        {
            DataTable dataTable = (DataTable)null;
            if (values != null && values.Count > 0)
            {
                dataTable = new DataTable("PivotTable");
                foreach (KeyValuePair<string, object> keyValuePair in values)
                {
                    Type type = typeof(string);
                    if (keyValuePair.Value != null)
                        type = keyValuePair.Value.GetType().GetRealPropertyType();
                    dataTable.Columns.Add(keyValuePair.Key, type);
                }
                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);
                foreach (KeyValuePair<string, object> keyValuePair in values)
                    row[keyValuePair.Key] = keyValuePair.Value;
            }
            return dataTable;
        }

        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            DateTime dateTime = date;
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
                dateTime = dateTime.AddDays(-1.0);
            return dateTime;
        }

        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            DateTime dateTime = date;
            while ((uint)dateTime.DayOfWeek > 0U)
                dateTime = dateTime.AddDays(1.0);
            return dateTime;
        }

        public static DateTime GetNextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime dateTime = date;
            while (dateTime == date || dateTime.DayOfWeek != dayOfWeek)
                dateTime = dateTime.AddDays(1.0);
            return dateTime;
        }

        public static DateTime GetPreviousDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime dateTime = date;
            while (dateTime == date || dateTime.DayOfWeek != dayOfWeek)
                dateTime = dateTime.AddDays(-1.0);
            return dateTime;
        }

        public static void Difference(this DateTime fromDate, DateTime toDate, out int years, out int months, out int days)
        {
            DateTime dateTime = new DateTime(1, 1, 1);
            TimeSpan timeSpan = toDate.Subtract(fromDate);
            years = dateTime.Add(timeSpan).Year - 1;
            months = dateTime.Add(timeSpan).Month - 1;
            days = dateTime.Add(timeSpan).Day;
        }

        public static DataTable GetFields(this Type entityType)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Description");
            if (entityType != (Type)null)
            {
                foreach (PropertyInfo property in entityType.GetProperties())
                    dataTable.Rows.Add((object)property.Name, (object)property.Name);
            }
            return dataTable;
        }

        public static DataTable GetFields(this DataTable dataTable)
        {
            DataTable dataTable1 = new DataTable();
            dataTable1.Columns.Add("Name");
            dataTable1.Columns.Add("Description");
            if (dataTable != null)
            {
                foreach (DataColumn dataColumn in dataTable.Columns.OfType<DataColumn>())
                {
                    string str = dataColumn.Caption ?? dataColumn.ColumnName;
                    dataTable1.Rows.Add((object)dataColumn.ColumnName, (object)str);
                }
            }
            return dataTable1;
        }
    }
}
