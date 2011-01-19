using System.ServiceModel;
using System.ServiceModel.Web;

namespace Catbert4.Services.Wcf
{
    [ServiceContract(Namespace="https://secure.caes.ucdavis.edu/Catbert4")]
    public interface IMessageService
    {
        [OperationContract]
        [WebGet] //Allow this to be called via Ajax
        ServiceMessage[] GetMessages(string appName);
    }
}
