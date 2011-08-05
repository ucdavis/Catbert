using System.Runtime.Serialization;

namespace Catbert4.Services.Wcf
{
    [DataContract]
    public class ServiceMessage
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool Critical { get; set; }
        [DataMember]
        public bool Global { get; set; }
    }
}