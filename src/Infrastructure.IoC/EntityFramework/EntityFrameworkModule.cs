using Core.Application.Services;
using Core.Domain.Model;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Repositories;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Configuration;

namespace Infrastructure.IoC.EntityFramework
{
    public class EntityFrameworkModule : NinjectModule
    {
        public override void Load()
        {
            // Bind the generic repo interface to it's implementation
            Bind(typeof (IRepository<>)).To(typeof (EfRepository<>));

            // Bind the auditable generic repo interface to it's implemntation
            Bind(typeof (IAuditableRepository<>)).To(typeof (EfAuditableRepository<>));

            // Bind the custom DbContext interface to our custom DbContext
            var connectionStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            Bind<IEntityContext>()
                .To<EntityContext>()
                .InRequestScope()
                .WithConstructorArgument("connectionString", connectionStr);
        }
    }
}
