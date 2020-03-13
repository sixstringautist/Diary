using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Diary.Models
{

    public class Memo
    {
        public int Id { get; set; }
        [Required]
        public string Theme { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public bool IsDone { get; set; }
    }
    public class Buisness : Memo
    {   
        [Required]
        public DateTime EndTime { get; set; }
    }

    public class Meeting : Buisness
    {
        [RegularExpression(@"([A-я0-9\s\.]),*")]
        public string Address { get; set; }
    }
}