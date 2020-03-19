using System;
using System.Data.Entity;
namespace Diary.Models
{
    public class DiaryDbContext : DbContext
    {
        public DbSet<Memo> Memos { get; set; }

        public DbSet<Buisness> Buisnesses { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        class Initializer : DropCreateDatabaseAlways<DiaryDbContext>
        {
            protected override void Seed(DiaryDbContext context)
            {
                var tmp = new Memo() { StartTime = DateTime.Now + TimeSpan.FromMinutes(50), Theme = "Тест", Type = "Памятка" };
                context.Memos.Add(tmp);
                context.SaveChanges();
            }
        }

        public DiaryDbContext(string connectionString) : base(connectionString)
        {
            //TODO: Не забудь изменить на CreateIfNotExist
            Database.SetInitializer(new Initializer());
            Database.Initialize(false);
        }
    }
}