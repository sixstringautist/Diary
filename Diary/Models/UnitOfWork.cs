using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
namespace Diary.Models
{
    public class UnitOfWork
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
                    throw new ArgumentNullException($"{nameof(el)} was null.");
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
            Add(DbContext.Memos,item);
        }

        public void Add(Buisness item)
        {
            Add(DbContext.Memos, item);
        }

        public void Add(Meeting item)
        {
            Add(DbContext.Memos, item);
        }
        #endregion

        #region Delete
        private void Delete<T>(DbSet<T> set, T item)
            where T : Memo
        {
            Apply<T, T>(set, x =>
            {
                x.Attach(item);
                x.Remove(item);
                return null;
            });
        }

        public void Delete(Memo item)
        {
            Delete(DbContext.Memos, item);
        }

        public void Delete(Buisness item)
        {
            Delete(DbContext.Buisnesses, item);
        }

        public void Delete(Meeting item)
        {
            Delete(DbContext.Meetings, item);
        }
        #endregion

        #region Filter
        private IEnumerable<T> Filter<T>(DbSet<T> set, Func<T, bool> predicate)
            where T : Memo
        {
            return Apply(set, x => x.Where(predicate));
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
        public ReturnType Get<ReturnType>(Func<ReturnType, bool> predicate)
            where ReturnType:Memo
        {
            return Get<ReturnType>(DbContext.Set<ReturnType>(),predicate);
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
            return GetAll(DbContext.Set<ReturnType>());
        }
        #endregion


        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
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