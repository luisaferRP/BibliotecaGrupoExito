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
            //filter only numbers
            var original = new string(isbn.Where(char.IsDigit).ToArray());

            if(string.IsNullOrEmpty(original))
            {
                return false;
            }
            
            var reversed = new string(original.Reverse().ToArray());

            return original.Equals(reversed);
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
