using System;

namespace CAESDO.Catbert.Test.Core
{
    public class ContinuousIntegrationDeploymentHack
    {
        public ContinuousIntegrationDeploymentHack()
        {
            new NHibernate.ProxyGenerators.CastleDynamicProxy.ProxyFactoryFactory();
            new System.Data.SQLite.SQLiteException();

            throw new Exception("This class should never be called or instantiated");
        }

    }
}