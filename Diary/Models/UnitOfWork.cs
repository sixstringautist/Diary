using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diary.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace Diary.Models
{
    public class UnitOfWork : IRepository<Memo>, IRepository<Buisness>, IRepository<Meeting>,IDisposable
    {
        DiaryDbContext DbContext;

        public UnitOfWork(string connectionString)
        {
            DbContext = new DiaryDbContext(connectionString);
        }
        private void CheckNull<T>(params T[] p)
        {
            foreach (var el in p)
            {
                if (p == null)
                    throw new ArgumentNullException("");
            }
        }
        public ReturnType Apply<EntityType, ReturnType>(DbSet<EntityType> set, Func<DbSet<EntityType>, ReturnType> func)
            where EntityType : class
        {
            CheckNull(set);
            CheckNull(func);
            return func(set);
        }

        #region Add
        private void Add<T>(DbSet<T> set, T item)
            where T : Memo
        {
            CheckNull(item);
            Apply(set, s => s.Add(item));
        }

        public void Add(Memo item)
        {
            Add(item);
        }

        public void Add(Buisness item)
        {
            Add(item);
        }

        public void Add(Meeting item)
        {
            Add(item);
        }
        #endregion

        #region Delete
        private void Delete<T>(DbSet<T> set,T item)
            where T: Memo
        {
            Apply<T,T>(set, x=> {
                x.Attach(item);
                x.Remove(item);
                return null;
            });
        }

        public void Delete(Memo item)
        {
            Delete(DbContext.Memos,item);
        }

        public void Delete(Buisness item)
        {
            Delete(DbContext.Buisnesses,item);
        }

        public void Delete(Meeting item)
        {
            Delete(DbContext.Meetings, item);
        }
        #endregion

        #region Filter
        private IEnumerable<T> Filter<T>(DbSet<T> set,Func<T, bool> predicate)
            where T: Memo
        {
            return Apply(set, x=> x.Where(predicate));
        }
        public IEnumerable<Memo> Filter(Func<Memo, bool> predicate)
        {
            return Filter(DbContext.Memos, predicate);
        }

        public IEnumerable<Buisness> Filter(Func<Buisness, bool> predicate)
        {
            return Filter(DbContext.Buisnesses, predicate);
        }

        public IEnumerable<Meeting> Filter(Func<Meeting, bool> predicate)
        {
            return Filter(DbContext.Meetings, predicate);
        }
        #endregion

        #region Get
        private ReturnType Get<ReturnType>(DbSet<ReturnType> set, Func<ReturnType, bool> predicate)
            where ReturnType : Memo
        {
            return Apply(set, s => s.FirstOrDefault(predicate));
        }
        public Memo Get(Func<Memo, bool> predicate)
        {
            return Get(DbContext.Memos, predicate);
        }

        public Buisness Get(Func<Buisness, bool> predicate)
        {
            return Get(DbContext.Buisnesses, predicate);
        }

        public Meeting Get(Func<Meeting, bool> predicate)
        {
            return Get(DbContext.Meetings, predicate);
        }
        #endregion

        #region GetAll
        private IEnumerable<ReturnType> GetAll<ReturnType>(DbSet<ReturnType> set)
            where ReturnType : Memo
        {
            return Apply(set, s => s);
        }
        public IEnumerable<ReturnType> GetAll<ReturnType>()
            where ReturnType : Memo
        {
            return (this as IRepository<ReturnType>).GetAll();
        }
        IEnumerable<Memo> IRepository<Memo>.GetAll() 
        {
            return GetAll(DbContext.Memos);
        }

        IEnumerable<Buisness> IRepository<Buisness>.GetAll()
        {
            return GetAll(DbContext.Buisnesses);
        }

        IEnumerable<Meeting> IRepository<Meeting>.GetAll()
        {
            return GetAll(DbContext.Meetings);
        }
        #endregion
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}