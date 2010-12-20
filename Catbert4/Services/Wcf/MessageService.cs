using System.Linq;
using Catbert4.Core.Domain;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Core.PersistanceSupport;
using System;

namespace Catbert4.Services.Wcf
{
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Get the current message for a given application.  
        /// An active system message gets priority over individual application messages.
        /// </summary>
        /// <example> Calling code example:
        /// From Javascript:
        /// $(function () {
        /// $.get('[url]/Message.svc/json/GetMessage',
        ///{ appName: "AD419" },
        ///function (result) { console.log(result.d); },
        ///'json');
        ///});
        /// From C#:
        /// new ChannelFactory<IMessageService/>(new BasicHttpBinding(), "[url]/Message.svc"))
        /// </example>
        /// <returns>Returns the system message, or string.empty if no message</returns>
        public string GetMessage(string appName)
        {
            return GetMessageText(appName);
        }
        
        private static string GetMessageText(string appName)
        {
            //First check for any global messages active now
            var globalMessage = (from m in RepositoryFactory.MessageRepository.Queryable
                                 where m.Application == null
                                       && m.Active
                                       && m.BeginDisplayDate < DateTime.Now
                                       && m.EndDisplayDate > DateTime.Now
                                 select m).FirstOrDefault();

            if (globalMessage != null)
            {
                return globalMessage.Text;
            }

            var applicationMessage = (from m in RepositoryFactory.MessageRepository.Queryable
                                      where m.Application.Name == appName
                                            && m.Active
                                            && m.BeginDisplayDate < DateTime.Now
                                            && m.EndDisplayDate > DateTime.Now
                                      select m).FirstOrDefault();

            if (applicationMessage != null)
            {
                return applicationMessage.Text;
            }

            //Neither an application nor a global message was found
            return string.Empty;
        }

        public class RepositoryFactory
        {

            // Private constructor prevents instantiation from other classes
            private RepositoryFactory() { }

            /**
            * SingletonHolder is loaded on the first execution of RepositoryFactory.[Property] 
            * or the first access to SingletonHolder.Instance, not before.
            */
            private static class SingletonHolder
            {
                public static readonly IRepository<Message> Instance = ServiceLocator.Current.GetInstance<IRepository<Message>>();
            }

            public static IRepository<Message> MessageRepository
            {
                get
                {
                    return SingletonHolder.Instance;
                }
            }

        }
    }
}