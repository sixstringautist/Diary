using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.ComponentModel.DataAnnotations;
using Diary.Util;

namespace Diary.Models
{
    public class MainViewModel : IValidatableObject
    {
        [DateTimeFormatValidation]
        public DateTime? DateStartFilter { get; set; }
        [DateTimeFormatValidation]
        public DateTime? DateEndFilter { get; set; }
        [RegularExpression("Все|Памятка|Дело|Встреча")]
        public string TypeFilter { get; set; } = "Все";
        public (int pageNumber, int pageSize, int pageCount) PageInfo => (Memos.PageNumber, Memos.PageSize,Memos.PageCount);

        public (int pageNumber,int pageSize) ChangePagedListProp 
        {
            set
            {
                if (value.Item1 <= 0 || value.Item2 < 0 || value.Item2 > 50)
                    throw new ArgumentOutOfRangeException("Page number or page size smaller than zero or page size bigger than 50");
                if (Memos != null)
                {
                    Memos = Memos.ToPagedList(value.Item1, value.Item2);
                }
                else throw new Exception("Paged list is null");
            }
        }
        public IPagedList<Memo> Memos { get; private set; } = new List<Memo>().ToPagedList(1, 10);

        public void SetPagedList(UnitOfWork u, (int pageNumber,int pageSize) pageInfo)
        {
            if (TypeFilter == "Все")
            {
                if (DateStartFilter !=null && DateEndFilter != null)
                    Memos = u.Filter(x => x.StartTime?.Date >= DateStartFilter?.Date && x.StartTime?.Date <= DateEndFilter?.Date)
                        .ToList()
                        .ToPagedList(pageInfo.pageNumber, pageInfo.pageSize);
                else 
                {
                    Memos = u.GetAll<Memo>().ToList().ToPagedList(pageInfo.pageNumber, pageInfo.pageSize);
                }
            }
            else 
            {
                if (DateStartFilter != null && DateEndFilter != null)
                    Memos = u.Filter(x => x.StartTime?.Date >= DateStartFilter?.Date && x.StartTime?.Date <= DateEndFilter?.Date && x.Type == TypeFilter)
                        .ToList()
                        .ToPagedList(pageInfo.pageNumber, pageInfo.pageSize);
                else 
                {
                    Memos = u.Filter(x => x.Type == TypeFilter).ToList().ToPagedList(pageInfo.pageNumber, pageInfo.pageSize);
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var tmp = new List<ValidationResult>();
            if (DateEndFilter < DateStartFilter)
                tmp.Add(new ValidationResult("Время окончания должно быть больше времени начала!", new[] { nameof(DateEndFilter), nameof(DateStartFilter) }));
            return tmp;
        }
    }
}