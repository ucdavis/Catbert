using System;
using System.ServiceModel;
using Catbert4.Services.Wcf;

namespace Catbert4.Providers
{
    public class RoleServiceClient : IDisposable
    {
        public IRoleService Service { get; set; }
        private readonly ChannelFactory<IRoleService> _factory;

        public RoleServiceClient(ChannelFactory<IRoleService> factory)
        {
            _factory = factory;
            Service = _factory.CreateChannel();
        }

        public void Dispose()
        {
            _factory.Close();
        }
    }
}