using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Verifica si un número entero es un palíndromo.
        /// Un número es un palíndromo si se lee igual de izquierda a derecha y de derecha a izquierda.
        /// Se considera que un número negativo o cero no es un palíndromo para este contexto,
        /// a menos que se especifique lo contrario en la regla de negocio.
        /// </summary>
        /// <param name="number">El número a verificar.</param>
        /// <returns>Verdadero si el número es un palíndromo, falso en caso contrario.</returns>
        public static bool IsPalindromo(this long number)
        {
            // Según la regla, el ISBN debe ser un número entero positivo.
            // Los números negativos y el cero no son palíndromos en este contexto.
            if (number <= 0)
            {
                return false;
            }

            long originalNumber = number;
            long reversedNumber = 0;

            // Invertir el número
            while (number > 0)
            {
                long digit = number % 10;        // Obtener el último dígito
                reversedNumber = reversedNumber * 10 + digit; // Añadir el dígito al número invertido
                number /= 10;                   // Eliminar el último dígito del número original
            }

            // Comparar el número original con el número invertido
            return originalNumber == reversedNumber;
        }

        /// <summary>
        /// Calcula la suma de los dígitos de un número.
        /// Necesario para la regla de negocio de préstamos de libros.
        /// </summary>
        /// <param name="number">El número del cual sumar los dígitos.</param>
        /// <returns>La suma de los dígitos del número.</returns>
        public static int SumarDigitos(this long number)
        {
            if (number < 0)
            {
                // Si el número puede ser negativo y la suma de dígitos debe aplicarse,
                // se podría tomar el valor absoluto. Para ISBN positivo, esto no debería ocurrir.
                number = Math.Abs(number);
            }

            int sum = 0;
            string s = number.ToString(); // Convertir a string para iterar por dígitos
            foreach (char c in s)
            {
                if (char.IsDigit(c))
                {
                    sum += (int)char.GetNumericValue(c); // Convertir char a int
                }
            }
            return sum;
        }
    }
}
