
using Ninject.Modules;
using Ninject.Web.Common;
using Diary.Models;
using System.Configuration;

namespace Diary.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<UnitOfWork>().ToSelf().InRequestScope().WithConstructorArgument("connectionString",
                ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString);
        }
    }
}