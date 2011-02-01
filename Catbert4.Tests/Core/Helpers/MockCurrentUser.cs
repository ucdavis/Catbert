using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Catbert4.Tests.Core.Helpers
{
    #region mocks
    /// <summary>
    /// Mock the Identity. Used for getting the current user name
    /// </summary>
    public class MockIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                return "MockAuthentication";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "UserName";
            }
        }
    }


    /// <summary>
    /// Mock the Principal. Used for getting the current user name
    /// </summary>
    public class MockPrincipal : IPrincipal
    {
        IIdentity _identity;
        public bool RoleReturnValue { get; set; }
        public string[] UserRoles { get; set; }

        public MockPrincipal(string[] userRoles)
        {
            UserRoles = userRoles;
        }

        public IIdentity Identity
        {
            get { return _identity ?? (_identity = new MockIdentity()); }
        }

        public bool IsInRole(string role)
        {
            if (UserRoles.Contains(role))
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Mock the HTTPContext. Used for getting the current user name
    /// </summary>
    public class MockHttpContext : HttpContextBase
    {
        private IPrincipal _user;
        private readonly int _count;
        public string[] UserRoles { get; set; }
        public MockHttpContext(int count, string[] userRoles)
        {
            _count = count;
            UserRoles = userRoles;
        }

        public override IPrincipal User
        {
            get { return _user ?? (_user = new MockPrincipal(UserRoles)); }
            set
            {
                _user = value;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return new MockHttpRequest(_count);
            }
        }
    }

    public class MockHttpRequest : HttpRequestBase
    {
        MockHttpFileCollectionBase Mocked { get; set; }

        public MockHttpRequest(int count)
        {
            Mocked = new MockHttpFileCollectionBase(count);
        }
        public override HttpFileCollectionBase Files
        {
            get
            {
                return Mocked;
            }
        }
    }

    public class MockHttpFileCollectionBase : HttpFileCollectionBase
    {
        public int Counter { get; set; }

        public MockHttpFileCollectionBase(int count)
        {
            Counter = count;
            for (int i = 0; i < count; i++)
            {
                BaseAdd("Test" + (i + 1), new byte[] { 4, 5, 6, 7, 8 });
            }

        }

        public override int Count
        {
            get
            {
                return Counter;
            }
        }
        public override HttpPostedFileBase Get(string name)
        {
            return new MockHttpPostedFileBase();
        }
        public override HttpPostedFileBase this[string name]
        {
            get
            {
                return new MockHttpPostedFileBase();
            }
        }
        public override HttpPostedFileBase this[int index]
        {
            get
            {
                return new MockHttpPostedFileBase();
            }
        }
    }

    public class MockHttpPostedFileBase : HttpPostedFileBase
    {
        public override int ContentLength
        {
            get
            {
                return 5;
            }
        }
        public override string FileName
        {
            get
            {
                return "Mocked File Name";
            }
        }
        public override Stream InputStream
        {
            get
            {
                var memStream = new MemoryStream(new byte[] { 4, 5, 6, 7, 8 });
                return memStream;
            }
        }
    }

    #endregion
}
