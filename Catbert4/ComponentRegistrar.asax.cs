﻿using System;
using Castle.Windsor;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.NHibernateValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data;
using UCDArch.Data.NHibernate;
using System.Security.Principal;
using System.Web;
using NHibernate;
using Catbert4.Services;
using System.Web.Security;
using Catbert4.Services.UserManagement;

namespace Catbert4
{
    public static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            //Add your components here
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            container.AddComponent("dbContext", typeof(IDbContext), typeof(DbContext));
            container.AddComponent("queryExtensionProvider", typeof (IQueryExtensionProvider),
                                   typeof(NHibernateQueryExtensionProvider));
            container.AddComponent("queryService", typeof(IQueryService),
                                   typeof(QueryService));

            container.AddComponent("userService", typeof (IUserService), typeof (UserService));
            container.AddComponent("unitService", typeof (IUnitService), typeof (UnitService));
            container.AddComponent("roleService", typeof (IRoleService), typeof (RoleService));

            /**/
            container.AddComponent("interceptor", typeof (IInterceptor), typeof (AuditInterceptor));

            container.AddComponent("principal", typeof (IPrincipal), typeof (WebPrincipal));
            container.AddComponent("directorySearchService", typeof (IDirectorySearchService),
                                   typeof (DirectoryServices));
            
            AddRepositoriesTo(container);
        }

        private static void AddRepositoriesTo(IWindsorContainer container)
        {
            container.AddComponent("repository", typeof(IRepository), typeof(Repository));
            container.AddComponent("genericRepository", typeof(IRepository<>), typeof(Repository<>));
            container.AddComponent("typedRepository", typeof(IRepositoryWithTypedId<,>),
                                   typeof(RepositoryWithTypedId<,>));
        }
    }
}