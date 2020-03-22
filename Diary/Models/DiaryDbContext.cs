using System;
using System.Collections.Generic;
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
                var r = new Random();
                List<Memo> tmp = new List<Memo>();
                for (int i = 0; i < 50; i++)
                {
                    switch (r.Next(0, 3))
                    {
                        case 0:
                            tmp.Add(new Memo { StartTime = DateTime.Now + TimeSpan.FromHours(r.Next(5, 20)), Theme = "Тест" });
                            break;
                        case 1:
                            tmp.Add(new Buisness() { StartTime = DateTime.Now + TimeSpan.FromHours(r.Next(5,20)), Theme="Тест", EndTime=DateTime.Now + TimeSpan.FromHours(r.Next(22,36))});
                            break;
                        case 2:
                            tmp.Add(new Meeting() { StartTime = DateTime.Now + TimeSpan.FromHours(r.Next(5, 20)), 
                                Theme = "Тест",
                                EndTime = DateTime.Now + TimeSpan.FromMinutes(r.Next(21, 36)),
                            Address=$"ул. Пушкина, д.{r.Next(1,100)}"});
                            break;
                    }
                }
                context.Memos.AddRange(tmp);
                context.SaveChanges();
            }
        }

        public DiaryDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new Initializer());
            Database.Initialize(false);
        }
    }
}