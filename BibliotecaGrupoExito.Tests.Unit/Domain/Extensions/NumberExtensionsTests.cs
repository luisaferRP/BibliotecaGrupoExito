using BibliotecaGrupoExito.Domain.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PrestamosBiblioteca.Tests.Unit.Domain.Extensions
{
    [TestClass]
    public class NumberExtensionsTests
    {
        [TestMethod]
        [DataRow(121, true)]
        [DataRow(12321, true)]
        [DataRow(7, true)]
        [DataRow(0, false)] // Regla de negocio: positivo
        [DataRow(-121, false)] // Regla de negocio: positivo
        [DataRow(123, false)]
        [DataRow(1234, false)]
        [DataRow(123321, true)]
        public void IsPalindromo_ShouldReturnCorrectResultForNumbers(long number, bool expected)
        {
            // Act
            bool actual = number.IsPalindromo();

            // Assert
            Assert.AreEqual(expected, actual, $"Para el número {number}, se esperaba {expected} pero se obtuvo {actual}.");
        }

        [TestMethod]
        [DataRow(123, 6)]   // 1+2+3 = 6
        [DataRow(987, 24)]  // 9+8+7 = 24
        [DataRow(1, 1)]
        [DataRow(0, 0)]
        [DataRow(-543, 12)] // Math.Abs(-543) = 543 -> 5+4+3 = 12
        [DataRow(123456789, 45)]
        public void SumarDigitos_ShouldReturnCorrectSum(long number, int expectedSum)
        {
            // Act
            int actualSum = number.SumarDigitos();

            // Assert
            Assert.AreEqual(expectedSum, actualSum, $"Para el número {number}, la suma esperada era {expectedSum} pero se obtuvo {actualSum}.");
        }
    }
}