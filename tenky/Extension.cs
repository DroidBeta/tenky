using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Capitalize the first letter of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Capitalize(this string str) => str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();

    }

    public static class IntExtension
    {
        /// <summary>
        /// Convert int? to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this int? input) => input.GetValueOrDefault();
    }
}
