using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;

namespace CAESDO.Catbert.Data
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {
        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance {
            get {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private NHibernateSessionManager() {
            InitSessionFactory();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager = 
                new NHibernateSessionManager();
        }

        #endregion

        private void InitSessionFactory() {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor) {
            ISession session = ThreadSession;

            if (session != null && session.IsOpen) {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSession(interceptor);
        }

        public ISession GetSession() {
            return GetSession(null);
            //return GetSession(new TrackingInterceptor());
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor) {
            ISession session = ThreadSession;

            if (session == null) {
                if (interceptor != null) {
                    session = sessionFactory.OpenSession(interceptor);
                }
                else {
                    session = sessionFactory.OpenSession();
                }

                ThreadSession = session;
            }

            // Here's a design-by-contract clause used in the enterprise sample
            //Check.Ensure(session != null, "session was null");

            return session;
        }

        public void CloseSession() {
            ISession session = ThreadSession;
            ThreadSession = null;

            if (session != null && session.IsOpen) {
                session.Close();
            }
        }

        public void BeginTransaction() {
            ITransaction transaction = ThreadTransaction;

            if (transaction == null) {
                transaction = GetSession().BeginTransaction();
                ThreadTransaction = transaction;
            }
        }

        public void CommitTransaction() {
            ITransaction transaction = ThreadTransaction;

            try {
                if (HasOpenTransaction()) {
                    transaction.Commit();
                    ThreadTransaction = null;
                }
            }
            catch (HibernateException) {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction() {
            ITransaction transaction = ThreadTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransaction() {
            ITransaction transaction = ThreadTransaction;

            try {
                ThreadTransaction = null;

                if (HasOpenTransaction()) {
                    transaction.Rollback();
                }
            }
            finally {
                CloseSession();
            }
        }

        /// <summary>
        /// Make sure the object given is connected to the current session
        /// </summary>
        /// <param name="o">Mapped Object</param>
        public void EnsureFreshness(object o)
        {
            if (GetSession().Contains(o) == false) //check is the current session contains the desired object
                GetSession().Refresh(o);
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ITransaction ThreadTransaction {
            get {
                if (IsInWebContext()) {
                    return (ITransaction)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                else {
                    return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
                }
            }
            set {
                if (IsInWebContext()) {
                    HttpContext.Current.Items[TRANSACTION_KEY] = value;
                }
                else {
                    CallContext.SetData(TRANSACTION_KEY, value);
                }
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ISession ThreadSession {
            get {
                if (IsInWebContext()) {
                    return (ISession)HttpContext.Current.Items[SESSION_KEY];
                }
                else {
                    return (ISession)CallContext.GetData(SESSION_KEY); 
                }
            }
            set {
                if (IsInWebContext()) {
                    HttpContext.Current.Items[SESSION_KEY] = value;
                }
                else {
                    CallContext.SetData(SESSION_KEY, value);
                }
            }
        }

        private bool IsInWebContext() {
            return HttpContext.Current != null;
        }

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private ISessionFactory sessionFactory;
    }
}
