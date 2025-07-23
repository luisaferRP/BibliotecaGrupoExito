using BibliotecaGrupoExito.Domain.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PrestamosBiblioteca.Tests.Unit.Domain.Extensions
{
    [TestClass]
    public class NumberExtensionsTests
    {
        [TestMethod]
        [DataRow("121", true)]
        [DataRow("12321", true)]
        [DataRow("7", true)]
        [DataRow("0", false)]     
        [DataRow("-121", false)]   
        [DataRow("123", false)]
        [DataRow("1234", false)]
        [DataRow("123321", true)]

        //to test string cases (with non-numeric, null, empty characters)
        [DataRow("12-321", true)]   
        [DataRow("abc", false)]    
        [DataRow(null, false)]     
        [DataRow("", false)]       
        [DataRow("   ", false)]    

        public void IsPalindromo_ShouldReturnCorrectResultForNumbers(string isbnString, bool expected)
        {
            // Act
            bool actual = isbnString.IsPalindrome();

            // Assert
            Assert.AreEqual(expected, actual, $"Para el ISBN '{isbnString}', se esperaba {expected} pero se obtuvo {actual}.");
        }

        [TestMethod]
        [DataRow("123", 6)]        // 1+2+3 = 6
        [DataRow("987", 24)]       // 9+8+7 = 24
        [DataRow("1", 1)]
        [DataRow("0", 0)]
        [DataRow("-543", 12)]
        [DataRow("123456789", 45)]
        // to test chain cases
        [DataRow("ABC123XYZ", 6)]
        [DataRow("99-9", 27)]      
        [DataRow(null, 0)]         
        [DataRow("", 0)]          
        [DataRow("   ", 0)]     
        public void SumarDigitos_ShouldReturnCorrectSum(String isbnString, int expectedSum)
        {
            // Act
            int actualSum = isbnString.SumDigits();

            // Assert
            Assert.AreEqual(expectedSum, actualSum, $"Para el ISBN '{isbnString}', la suma esperada era {expectedSum} pero se obtuvo {actualSum}.");
        }
    }
}