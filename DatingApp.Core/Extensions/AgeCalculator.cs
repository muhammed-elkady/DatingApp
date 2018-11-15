using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Extensions
{
    public static class AgeCalculator
    {
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            // in case of a leap year
            if (dateOfBirth.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
