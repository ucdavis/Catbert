using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;
using Catbert4.Core.Service;
using UCDArch.Core.Utils;
using Microsoft.Practices.ServiceLocation;
using Catbert4.Services;

namespace Catbert4.Public
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CatbertService : DataService<CatbertServiceDataContext>
    {
        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            var headers = args.OperationContext.RequestHeaders;
            var appName = headers["AppName"];
            var token = headers["Token"];

            Check.Require(!string.IsNullOrWhiteSpace(appName), "Application Name is required to be set. Please ensure you have set an AppSetting called AppName");
            Check.Require(!string.IsNullOrWhiteSpace(token), "Token is required to be set. Please ensure you have set an AppSetting called Token");

            //base.CurrentDataSource.ApplicationName = appName ?? "HelpRequest"; //TODO: remove after testing

            base.OnStartProcessingRequest(args);
        }

        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.UseVerboseErrors = true;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }

    public class CatbertServiceDataContext
    {
        public string ApplicationName { get; set; }

        public IQueryable<ServiceUser> Users
        {
            get { return QueryableOf<ServiceUser>(); }
        }

        public IQueryable<ServiceUnitAssociation> UnitAssociations
        {
            get { return QueryableOf<ServiceUnitAssociation>().Where(x=>x.Application.Name == ApplicationName); }
        }

        public IQueryable<ServiceUnit> Units
        {
            get { return QueryableOf<ServiceUnit>(); }
        }

        public IQueryable<ServiceApplication> Applications
        {
            get { return QueryableOf<ServiceApplication>().Where(x=>x.Name == ApplicationName); }
        }

        public IQueryable<ServiceSchool> Schools
        {
            get { return QueryableOf<ServiceSchool>(); }
        }

        private static IQueryable<T> QueryableOf<T>()
        {
            return ServiceLocator.Current.GetInstance<IQueryService>().GetQueryable<T>();
        }
    }
}
