using System;
using Castle.Windsor;
using Catbert4.Controllers;
using Catbert4.Core.Domain;
using Catbert4.Services;
using Catbert4.Services.UserManagement;
using Catbert4.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    [TestClass]
    public partial class UserManagementControllerTests : ControllerTestBase<UserManagementController>
    {
        public IRepository<User> UserRepository;
        public IRepository<Application> ApplicationRepository;
        public IRepository<Unit> UnitRepository;
        public IRepository<Role> RoleRepository;
        public IRepository<Permission> PermissionRepository;
        public IRepository<UnitAssociation> UnitAssociationRepository;

        public IUserService UserService;
        public IUnitService UnitService;
        public IRoleService RoleService;
        public IDirectorySearchService DirectorySearchService;

        //public IQueryExtensionProvider QueryExtension;

        public readonly Type ControllerClass = typeof(UserManagementController);

        #region Init
        protected override void SetupController()
        {

            /*
		private readonly IRepository<User> _userRepository;
	    private readonly IRepository<Application> _applicationRepository;
	    private readonly IRepository<Unit> _unitRepository;
	    private readonly IRepository<Role> _roleRepository;
	    private readonly IRepository<Permission> _permissionRepository;
		private readonly IRepository<UnitAssociation> _unitAssociationRepository;
	    private readonly IUserService _userService;
		private readonly IUnitService _unitService;
		private readonly IRoleService _roleService;
		private readonly IDirectorySearchService _directorySearchService;

		public UserManagementController(IRepository<User> userRepository, 
            IRepository<Application> applicationRepository,
            IRepository<Unit> unitRepository,
            IRepository<Role> roleRepository,
			IRepository<Permission> permissionRepository,
			IRepository<UnitAssociation> unitAssociationRepository,
			IUserService userService, 
			IUnitService unitService, 
			IRoleService roleService, 
			IDirectorySearchService directorySearchService)
		{
			_userRepository = userRepository;
		    _applicationRepository = applicationRepository;
		    _unitRepository = unitRepository;
		    _roleRepository = roleRepository;
		    _permissionRepository = permissionRepository;
			_unitAssociationRepository = unitAssociationRepository;
		    _userService = userService;
			_unitService = unitService;
			_roleService = roleService;
			_directorySearchService = directorySearchService;
		}             
             */
            UserRepository = FakeRepository<User>();
            ApplicationRepository = FakeRepository<Application>();
            UnitRepository = FakeRepository<Unit>();
            RoleRepository = FakeRepository<Role>();
            PermissionRepository = FakeRepository<Permission>();
            UnitAssociationRepository = FakeRepository<UnitAssociation>();

            UserService = MockRepository.GenerateStub<IUserService>();
            UnitService = MockRepository.GenerateStub<IUnitService>();
            RoleService = MockRepository.GenerateStub<IRoleService>();
            DirectorySearchService = MockRepository.GenerateStub<IDirectorySearchService>();

            Controller = new TestControllerBuilder().CreateController<UserManagementController>(
                UserRepository,
                ApplicationRepository,
                UnitRepository,
                RoleRepository,
                PermissionRepository,
                UnitAssociationRepository,
                UserService,
                UnitService,
                RoleService,
                DirectorySearchService);
        }

        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }

        //protected override void RegisterAdditionalServices(IWindsorContainer container)
        //{
        //    TermCodeRepository = MockRepository.GenerateStub<IRepository<TermCode>>();
        //    container.Kernel.AddComponentInstance<IRepository<TermCode>>(TermCodeRepository);
        //    base.RegisterAdditionalServices(container);
        //}

        protected override void RegisterAdditionalServices(IWindsorContainer container)
        {
            
            container.AddComponent("queryExtensionProvider", typeof(IQueryExtensionProvider),
                                   typeof(QueryExtensionFakes));

            base.RegisterAdditionalServices(container);
        }
        

        #endregion Init
    }
}
