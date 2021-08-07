using System;

namespace API.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge (this DateTime dob){

            /* Checking the current yeat
               subtracting the no of years 
               checking if this year birthday has come or not if not then subtracting by 1 
            */
            var today = DateTime.Today ;
            var  age = today.Year - dob.Year;
            if(dob.Date > today.AddYears(-age)) age--; 
            return age;
        }
        
    }
}