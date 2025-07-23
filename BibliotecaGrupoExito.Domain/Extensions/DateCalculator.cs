using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Extensions
{
    public class DateCalculator
    {
        public static DateTime CalculaterReturnDate(DateTime startDate, int businessDays)
        {
            DateTime currentDate = startDate.Date;

            if (currentDate.DayOfWeek == DayOfWeek.Saturday)
            {
                currentDate = currentDate.AddDays(2); 
            }
            else if (currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                currentDate = currentDate.AddDays(1);
            }

            while (businessDays > 0)
            {
                currentDate = currentDate.AddDays(1);
                      
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays--;
                }
            }
            return currentDate;
        }

        //I adjust the date if it falls on a Sunday, moving it to the next business day.
        public static DateTime AdjustIfSunday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return date.AddDays(2);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(1);
            }
            return date;
        }
    }
}