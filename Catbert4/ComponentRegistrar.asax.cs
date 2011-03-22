using Castle.MicroKernel.Registration;
using Castle.Windsor;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.NHibernateValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using System.Security.Principal;
using NHibernate;
using Catbert4.Services;
using Catbert4.Services.UserManagement;

namespace Catbert4
{
    public static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            //Add your components here
            container.Register(Component.For<IValidator>().ImplementedBy<Validator>().Named("validator"));
            container.Register(Component.For<IDbContext>().ImplementedBy<DbContext>().Named("dbContext"));
            container.Register(Component.For<IQueryExtensionProvider>().ImplementedBy<NHibernateQueryExtensionProvider>().Named("queryExtensionProvider"));
            container.Register(Component.For<IQueryService>().ImplementedBy<QueryService>().Named("queryService"));
            
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>().Named("userService"));
            container.Register(Component.For<IUnitService>().ImplementedBy<UnitService>().Named("unitService"));
            container.Register(Component.For<IRoleService>().ImplementedBy<RoleService>().Named("roleService"));
            
            /**/
            container.Register(Component.For<IInterceptor>().ImplementedBy<AuditInterceptor>().Named("interceptor"));

            container.Register(Component.For<IPrincipal>().ImplementedBy<WebPrincipal>().Named("principal"));
            container.Register(Component.For<IDirectorySearchService>().ImplementedBy<DirectoryServices>().Named("directorySearchService"));
            
            AddRepositoriesTo(container);
        }

        private static void AddRepositoriesTo(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(RepositoryWithTypedId<,>)).Named("repositoryWithTypedId"));
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).Named("repositoryType"));
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>().Named("repository"));
        }
    }
}