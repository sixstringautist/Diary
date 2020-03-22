using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Diary.Util
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DatetimeFormatValidationWithHoursAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            DateTime result;
            if (value == default) return true;
            if (DateTime.TryParseExact(((DateTime)value).ToString("g"), "dd.MM.yyyy HH:mm", new CultureInfo("ru-RU"), DateTimeStyles.None, out result))
            {
                return true;
            }
            this.ErrorMessage = "Неверный формат";
            return false;

        }

    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeFormatValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime result;
            if (value == null) return true;
            if (DateTime.TryParseExact(((DateTime)value).ToString("d"), "dd.MM.yyyy", new CultureInfo("ru-RU"), DateTimeStyles.None, out result))
            {
                return true;
            }
            ErrorMessage = "Неверный формат";
            return false;
        }
    }
}