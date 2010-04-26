using System.Web.Services;
using CAESDO.Catbert.BLL.Service;
using CAESDO.Catbert.Core.ServiceObjects;

/// <summary>
/// Summary description for UserInformationService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserInformationService : WebService
{
    [WebMethod]
    public UserInformation GetUserInfo(string loginId)
    {
        return UserInformationServiceBLL.GetInformationByLoginId(loginId);
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
}

