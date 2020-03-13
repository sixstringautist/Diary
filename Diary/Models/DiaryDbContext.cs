using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Diary.Models
{
    public class DiaryDbContext : DbContext
    {
       public DbSet<Memo> Memos { get; set; }

        public DbSet<Buisness> Buisnesses { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DiaryDbContext(string connectionString):base(connectionString) 
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DiaryDbContext>());
            Database.Initialize(true);
        }
    }
}