using System;
using System.Collections.Generic;
using System.Text;

namespace CAESDO.Catbert.Data
{
    public class TransactionScope : IDisposable
    {
        private bool _transactionCommitted = false;

        public TransactionScope()
        {
            NHibernateSessionManager.Instance.GetSession();

            NHibernateSessionManager.Instance.BeginTransaction();
        }

        public void RollBackTransaction()
        {
            NHibernateSessionManager.Instance.RollbackTransaction();
        }

        public void CommittTransaction()
        {
            NHibernateSessionManager.Instance.CommitTransaction();

            _transactionCommitted = true;
        }

        public bool HasOpenTransaction
        {
            get { return NHibernateSessionManager.Instance.HasOpenTransaction(); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_transactionCommitted == false) //rollback the transaction if it hasn't been committed
            {
                try
                {
                    NHibernateSessionManager.Instance.RollbackTransaction();
                }
                finally
                {
                    //NHibernateSessionManager.Instance.CloseSession();
                }
            }
        }

        #endregion
    }
}
