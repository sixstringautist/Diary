using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diary.Models;

namespace Diary.Util
{
    public class MemoModelBinder : IModelBinder
    {

        Creator creator = new Creator();


        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var provider = bindingContext.ValueProvider;

            var Id = provider.GetValue("Id");


            var EndTime = provider.GetValue("EndTime");
            var Address = provider.GetValue("Address");
            var IsDone = provider.GetValue("IsDone");


            var Type = provider.GetValue("Type").ConvertTo(typeof(string));
            var Theme = provider.GetValue("Theme").ConvertTo(typeof(string));
            var StartTime = provider.GetValue("StartTime").ConvertTo(typeof(string));

            var dict = new Dictionary<string, object>();

            if (Id == null)
            {
                dict.Add("Id", "0");
            }
            else
            {
                dict.Add("Id", Id.ConvertTo(typeof(string)));
            }

            if (EndTime != null)
                dict.Add("EndTime", EndTime.ConvertTo(typeof(string)));
            if (Address != null)
                dict.Add("Address", Address.ConvertTo(typeof(string)));
            if (IsDone == null)
            {
                dict.Add("IsDone", "false");
            }
            else
            {
                dict.Add("IsDone", IsDone.ConvertTo(typeof(string)));
            }

            dict.Add("Type", Type);
            dict.Add("Theme", Theme);
            dict.Add("StartTime", StartTime);

            return creator.GetCreator((string)Type)(dict);

        }

    }
}