﻿using System.Text.RegularExpressions;
using System.Data;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace DroidBeta.Tenky.Extension
{
    public static class CharExtension
    {
        /// <summary>
        /// Convert char[] to string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToNewString(this char[] input) => new string(input);
    }

    public static class StringExtension
    {

        /// <summary>
        /// Replace with dual value.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        public static string Replace(this string input, string[] oldValue, string newValue)
        {
            foreach(string oldValueLoop in oldValue)
            {
                input = input.Replace(oldValueLoop, newValue);
            }
            return input;
        }

        /// <summary>
        /// Determine whether the string is int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInt(this string input)
        {
            try
            {
                Int32.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string[] Split(this string input, string separator) => input.Split(separator, StringSplitOptions.None);

        public static string[] Split(this string input, string separator, StringSplitOptions options) => input.Split(new string[] {separator}, options);

        public static int ToInt(this string input) => int.Parse(input);

        /// <summary>
        /// Capitalize the first letter of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Capitalize(this string str) => str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();

        public static string GetIpFamily(this string ip)
        {
            if (IPAddress.TryParse(ip, out IPAddress address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        return "ipv4";
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        return "ipv6";
                    default:
                        throw new ArgumentException(ip + "is not a valid IP address!");
                }
            }
            else
            {
                throw new ArgumentException(ip + "is not a valid IP address!");
            }
        }

        public static bool IsIPv4(string ip) => (GetIpFamily(ip) == "ipv4");

        public static bool IsIPv6(string ip) => (GetIpFamily(ip) == "ipv6");

        public static bool IsIP(this string ip)
        {
            try
            {
                ip.GetIpFamily();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

    }

    public static class IntExtension
    {
        /// <summary>
        /// Convert int? to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this int? input) => input.GetValueOrDefault();

        public static bool IsImei(this int imei)
        {
            if (imei.GetLength() != 15)
                return false;
            else
                return imei == imei.SubInt(0, 14).ToImei15();
        }

        public static int ToImei15(this int imei14)
        {
            
            if(imei14.GetLength() != 14)
            {
                throw new ArgumentOutOfRangeException(imei14 + " is not 14 bit!");
            }

            int sum = 0;
            int[] ME = new int[14];
            //split string into a char array
            for (int i = 0; i < 14; i++)
            {
                ME[i] = Convert.ToInt32(imei14.SubInt(i, 1));
            }
            int[] ME_Even = new int[7];
            for (int i = 0; i < 7; i++)
                ME_Even[i] = ME[i * 2 + 1] * 2;
            int[] ME_Odd = new int[7];
            for (int i = 0; i < 7; i++)
                ME_Odd[i] = ME[i * 2];
            for (int i = 0; i < 7; i++)
                sum += ME_Odd[i] + ME_Even[i] / 10 + ME_Even[i] % 10;
            if (sum % 10 == 0)
                sum = 0;
            else
                sum = 10 - sum % 10;

            return imei14.Append(sum);
        }

        public static int GetLength(this int interger) => (int)Math.Log10(interger);

        public static int SubInt(this int num, int startIndex) => num.SubInt(startIndex, num.GetLength() - 1);
        public static int SubInt(this int num, int startIndex, int length) => int.Parse(num.ToString().Substring(startIndex, length));

        public static int Append(this int firstNum, params int[] restNums)
        {
            string tmp = firstNum.ToString();
            foreach(int restNum in restNums)
            {
                tmp += restNum.ToString();
            }
            return tmp.ToInt();
        }

        public static bool StartsWith(this int input, int value) => input.ToString().StartsWith(value.ToString());

        public static bool EndsWith(this int input, int value) => input.ToString().EndsWith(value.ToString());
        //EndsWith(int, int) has another way to write
        //return (input % 10) == value

    }

    public static class WebResponseExtension
    {
        public static string GetStreamString(this WebResponse webResponse) => GetStreamString(webResponse, Encoding.UTF8);
        public static string GetStreamString(this WebResponse webResponse, Encoding encoding)
        {
            Stream stream = webResponse.GetResponseStream();
            string responseStr = "";
            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream, encoding);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            webResponse.Close();
            return responseStr;
        }
    }

}
