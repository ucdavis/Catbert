using System;
using System.Web;
using CAESDO.Catbert.Data;

namespace CAESDO.Catbert.Web
{
    /// <summary>
    /// Implements the Open-Session-In-View pattern using <see cref="NHibernateSessionManager" />.
    /// Assumes that each HTTP request is given a single transaction for the entire page-lifecycle.
    /// Inspiration for this class came from Ed Courtenay at 
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class NHibernateSessionModule : IHttpModule
    {
        public void Init(HttpApplication context) {
            //context.BeginRequest += new EventHandler(BeginTransaction);
            //context.EndRequest += new EventHandler(CommitAndCloseSession);

            context.BeginRequest += new EventHandler(OpenSession);
            context.EndRequest += new EventHandler(CloseSession);
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private void BeginTransaction(object sender, EventArgs e) {
            NHibernateSessionManager.Instance.BeginTransaction();
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="NHibernateSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e) {
            try {
                NHibernateSessionManager.Instance.CommitTransaction();
            }
            finally {
                NHibernateSessionManager.Instance.CloseSession();
            }
        }

        /// <summary>
        /// Makes sure that there is an open session by either grabbing and discarding an existing session, or
        /// creating a new one if there isn't one already
        /// </summary>
        private void OpenSession(object sender, EventArgs e)
        {
            NHibernateSessionManager.Instance.GetSession();
        }

        /// <summary>
        /// Closes a session without commiting the transaction
        /// </summary>
        private void CloseSession(object sender, EventArgs e)
        {
            NHibernateSessionManager.Instance.CloseSession();
        }


        public void Dispose() { }
    }
}
