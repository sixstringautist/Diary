using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Interfaces
{
    interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(Func<T, bool> predicate);
        IEnumerable<T> Filter(Func<T, bool> predicate);
        void Add(T item);
        void Delete(T item);
    }
}
