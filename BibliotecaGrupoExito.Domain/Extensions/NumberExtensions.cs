using System;
using System.Collections.Generic;

namespace BibliotecaGrupoExito.Domain.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsPalindrome(this string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return false;
            }
            string trimmedIsbn = isbn.Trim();
            if (trimmedIsbn.StartsWith("-"))
            {
                return false;
            }
            //filter only numbers
            var cleanedDigits = new string(isbn.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(cleanedDigits))
            {
                return false;
            }
            else if(cleanedDigits == "0")
            {
                return false;
            }

          

            var reversed = new string(cleanedDigits.Reverse().ToArray());

            return cleanedDigits.Equals(reversed, StringComparison.Ordinal);
        }

        public static int SumDigits(this string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return 0;
            }

            int sum = 0;
            foreach (char c in isbn)
            {
                //valid if it is a number and I convert it into a numeric value to add it
                if (char.IsDigit(c))
                {
                    sum += (int)char.GetNumericValue(c);
                }
            }
            return sum;
        }
    }
}
