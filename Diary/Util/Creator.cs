using Diary.Models;
using System;
using System.Collections.Generic;

namespace Diary.Util
{

    public class Creator
    {

        public Func<Dictionary<string, object>, Memo> GetCreator(string type)
        {
            switch (type)
            {
                case "Памятка":
                    return MemoCreator;
                case "Дело":
                    return BuisnessCreator;
                case "Встреча":
                    return MeetingCreator;
                default:
                    return d => { return null; };
            }
        }

        private Memo MemoCreator(Dictionary<string, object> dict)
        {
            CheckNull(dict);
            CheckProps(dict, typeof(Memo));
            DateTime? tmp;
            bool isDone;
            int id;
            try
            {
                tmp = DateTime.Parse((string)dict["StartTime"]);
            }
            catch
            {
                tmp = null;
            }
            try
            {
                isDone = bool.Parse((string)dict["IsDone"]);
                id = int.Parse((string)dict["Id"]);
            }
            catch
            {
                isDone = false;
                id = 0;
            }
            return new Memo()
            {
                StartTime = tmp,
                IsDone = isDone,
                Theme = (string)dict["Theme"],
                Type = (string)dict["Type"],
                Id = id
            };
        }

        private Buisness BuisnessCreator(Dictionary<string, object> dict)
        {
            CheckNull(dict);
            CheckProps(dict, typeof(Buisness));
            DateTime? tmp1;
            DateTime? tmp2;
            bool isDone;
            int id;
            try
            {
                tmp1 = DateTime.Parse((string)dict["StartTime"]);
                tmp2 = DateTime.Parse((string)dict["EndTime"]);
            }
            catch
            {
                tmp1 = null;
                tmp2 = null;
            }
            try
            {
                id = int.Parse((string)dict["Id"]);
                isDone = bool.Parse((string)dict["IsDone"]);
            }
            catch
            {
                id = 0;
                isDone = false;
            }
            return new Buisness()
            {
                StartTime = tmp1,
                EndTime = tmp2,
                Theme = (string)dict["Theme"],
                IsDone = isDone,
                Type = (string)dict["Type"],
                Id = id
            };
        }

        private Meeting MeetingCreator(Dictionary<string, object> dict)
        {
            CheckNull(dict);
            CheckProps(dict, typeof(Meeting));
            DateTime? tmp1;
            DateTime? tmp2;
            bool isDone;
            int id;
            try
            {
                tmp1 = DateTime.Parse((string)dict["StartTime"]);
                tmp2 = DateTime.Parse((string)dict["EndTime"]);

            }
            catch
            {
                tmp1 = null;
                tmp2 = null;
                id = 0;
                isDone = false;
            }
            try
            {
                id = int.Parse((string)dict["Id"]);
                isDone = bool.Parse((string)dict["IsDone"]);
            }
            catch
            {
                id = 0;
                isDone = false;
            }
            return new Meeting()
            {
                StartTime = tmp1,
                EndTime = tmp2,
                Theme = (string)dict["Theme"],
                Address = (string)dict["Address"],
                IsDone = isDone,
                Type = (string)dict["Type"],
                Id = id
            };
        }


        private void CheckNull<T>(params T[] par)
        {
            foreach (var p in par)
                if (p == null)
                    throw new ArgumentException($"{ nameof(par)} was null");
        }
        private void CheckProps(Dictionary<string, object> dict, Type t)
        {
            var props = t.GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name == "Id")
                    continue;
                if (!dict.ContainsKey(prop.Name))
                    throw new Exception("Dict not contains prop");
            }
        }
    }
}