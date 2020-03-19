using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Json;
using System.IO;
using Diary.Models;

namespace Diary.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork u;


        public HomeController(UnitOfWork u)
        {
            this.u = u;
        }
        public ActionResult Index()
        {
            ViewBag.MemoCollection = u.GetAll<Memo>().ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Delete()
        {
            string ids;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                ids = reader.ReadToEnd();
            }
            var identificators = JsonConvert.DeserializeObject<List<int>>(ids);
            var tmp = u.Filter(x => identificators.Contains(x.Id)).ToList();
            if (tmp == null)
                return HttpNotFound();
            tmp.ForEach(x => u.Delete(x));
            u.SaveChanges();
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult MakeComplete()
        {
            string ids;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                ids = reader.ReadToEnd();
            }
            var identificators = JsonConvert.DeserializeObject<List<int>>(ids);
            var tmp = u.Filter(x => identificators.Contains(x.Id)).ToList();
            if (tmp == null)
                return HttpNotFound();
            tmp.ForEach(x => x.IsDone = true);
            u.SaveChanges();
            return new EmptyResult();
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var tmp = u.Get<Memo>(x => x.Id == id);
                switch (tmp.Type)
                {
                    case "Памятка":
                        return PartialView("EditMemo",tmp);
                    case "Встреча":
                        return PartialView("EditMeeting", tmp);
                    case "Дело":
                        return PartialView("EditBuisness", tmp);
                }
            }
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult Edit(Memo m)
        {
            switch (m.Type)
            {
                case "Памятка":
                    if (TryValidateModel(m))
                    {
                        var tmp = u.Get<Memo>(x => x.Id == m.Id);
                        if (tmp == null)
                            ModelState.AddModelError("notfound","Запись не найдена!");
                        tmp.StartTime = m.StartTime;
                        tmp.Theme = m.Theme;
                        u.SaveChanges();
                        return JavaScript("location.reload(true)");
                    }
                    else return PartialView("AddMemo",m);
                case "Встреча":
                    var met = m as Meeting;
                    if (TryValidateModel(met))
                    {
                        var tmp1 = u.Get<Meeting>(x => x.Id == m.Id);
                        tmp1.Theme = m.Theme;
                        tmp1.StartTime = met.StartTime;
                        tmp1.EndTime = met.EndTime;
                        tmp1.Address = met.Address;
                        u.SaveChanges();
                        return JavaScript("location.reload(true)");
                    }
                    else return PartialView("AddMeeting", met);
                case "Дело":
                    var b = m as Buisness;
                    if (TryValidateModel(b))
                    {
                        var tmp2 = u.Get<Buisness>(x => x.Id == m.Id);
                        tmp2.Theme = b.Theme;
                        tmp2.StartTime = b.StartTime;
                        tmp2.EndTime = b.EndTime;
                        return JavaScript("location.reload(true)");
                    }
                    else return PartialView("AddBuisness", b);
                default: 
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Add(Memo m)
        {
            switch (m.Type)
            {
                case "Памятка":
                    if (TryValidateModel(m))
                    {
                        u.Add(m);
                        u.SaveChanges();
                        return JavaScript("location.reload(true)");
                    }
                    else return PartialView("AddMemo", m);
                case "Дело":
                    var tmp = m as Buisness;
                    if (TryValidateModel(tmp))
                    {
                        if (u.Get<Buisness>(x => Buisness.Compare(x, tmp) == 0) == null)
                        {
                            u.Add(m);
                            u.SaveChanges();
                            return JavaScript("location.reload(true)");
                        }
                        else
                        {
                            ModelState.AddModelError("intersect", "Эта запись пересекается с другой по времени!");
                            return PartialView("AddBuisness", tmp);
                        }
                    }
                    else return PartialView("AddBuisness", tmp);
                case "Встреча":
                    var tmp1 = m as Meeting;
                    if (TryValidateModel(m as Meeting))
                    {
                        if (u.Get<Buisness>(x => Buisness.Compare(x, tmp1) == 0) == null)
                        {
                            u.Add(m);
                            u.SaveChanges();
                            return JavaScript("location.reload(true)");
                        }
                        else
                        {
                            ModelState.AddModelError("intersect", "Эта запись пересекается с другой по времени!");
                            return PartialView("AddMeeting", tmp1);
                        }
                    }
                    else return PartialView("AddMeeting", m as Meeting);
                default: return RedirectToAction("Index");
            }
        }
    }
}