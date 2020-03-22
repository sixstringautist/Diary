using Diary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Diary.Models
{
    public class Memo : IValidatableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string type;
        [Required]
        [RegularExpression("Памятка|Дело|Встреча")]
        public string Type { get => type; protected set => type = value; }
        [Required(ErrorMessage = "Тема обязательна для заполнения!")]
        [RegularExpression("[А-я]{3,100}")]
        public string Theme { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DatetimeFormatValidationWithHours]
        public DateTime? StartTime { get; set; }

        public Memo()
        {
            Type = "Памятка";
        }

        public bool IsDone { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (StartTime < DateTime.Now)
                yield return new ValidationResult("Дата начала должна быть больше текущего времени", new[] { nameof(StartTime) });
        }

    }
    public class Buisness : Memo, IValidatableObject
    {

        [Required]
        [DataType(DataType.DateTime)]
        [DatetimeFormatValidationWithHours]
        public DateTime? EndTime { get; set; }


        public Buisness()
        {
            Type = "Дело";
        }
        public static int Compare(Buisness e1, Buisness e2)
        {
            return e1.StartTime <= e2.EndTime && e1.EndTime >= e2.StartTime ? 0
                : e1.EndTime < e2.StartTime ? 1
                : -1;
        }
        new public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var tmp = new List<ValidationResult>();
            tmp.AddRange(base.Validate(validationContext));
            if (EndTime < StartTime)
                tmp.Add(new ValidationResult("Время окончания должно быть больше времени начала!", new[] { nameof(EndTime), nameof(StartTime) }));
            return tmp;
        }
    }

    public class Meeting : Buisness

    {
        public Meeting()
        {
            Type = "Встреча";
        }
        [Required]
        public string Address { get; set; }

    }
}