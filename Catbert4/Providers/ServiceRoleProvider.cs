using System.ServiceModel;
using Catbert4.Services.Wcf;

namespace Catbert4.Providers
{
    public static class ServiceFactoryExtensions
    {
        public static RoleServiceClient GetClient(this ChannelFactory<IRoleService> factory)
        {
            return new RoleServiceClient(factory);
        }
    }
}