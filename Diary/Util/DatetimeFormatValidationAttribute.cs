using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Diary.Util
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DatetimeFormatValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            
            DateTime result;
            if (value == null || value.ToString() == "")
            { 
                this.ErrorMessage = "Это поле обязательно к заполнение";
                return false;
            }
            
            if (DateTime.TryParseExact(((DateTime)value).ToString("g"), "dd.MM.yyyy HH:mm", new CultureInfo("ru-RU"), DateTimeStyles.None, out result))
            {
                return true;
            }
            this.ErrorMessage = "Неверный формат";
            return false;
            
        }

    }
}