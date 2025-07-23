using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Infrastructure.Common
{
    public static class DateCalculator
    {
        /// <summary>
        /// Calcula una fecha futura sumando un número de días hábiles (excluyendo domingos).
        /// </summary>
        /// <param name="startDate">La fecha de inicio.</param>
        /// <param name="businessDays">El número de días hábiles a sumar.</param>
        /// <returns>La fecha resultante después de sumar los días hábiles.</returns>
        public static DateTime AddBusinessDays(DateTime startDate, int businessDays)
        {
            DateTime currentDate = startDate;
            while (businessDays > 0)
            {
                currentDate = currentDate.AddDays(1);
                // Si la fecha actual no es domingo, decrementamos los días hábiles restantes
                if (currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays--;
                }
            }
            return currentDate;
        }

        /// <summary>
        /// Ajusta una fecha si cae en domingo, moviéndola al siguiente día hábil.
        /// </summary>
        /// <param name="date">La fecha a ajustar.</param>
        /// <returns>La fecha ajustada si era domingo, de lo contrario la fecha original.</returns>
        public static DateTime AdjustIfSunday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(1); // Mover al lunes
            }
            return date;
        }
    }
}