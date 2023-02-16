using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Itb.DalCore.UnitOfWork;
using NHibernate;
using NHibernate.Context;

namespace Itb.DalCore.NHibernate.UnitOfWork
{
    /// <summary> NHibernate unit of work
    /// </summary>
    internal class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly bool isTransactionOwner;
        private ITransaction transaction;

        /// <summary> Creates new Unit of Work with data source exception translator
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="isolationLevel">transaction isolation level</param>
        public NHibernateUnitOfWork(ISession session, IsolationLevel isolationLevel)
        {
            if (null == session)
            {
                throw new ArgumentNullException("session");
            }
            CurrentSessionContext.Bind(session);

            this.session = session;

            this.isTransactionOwner = !session.GetSessionImplementation().TransactionInProgress;
            if (this.isTransactionOwner)
            {
                this.transaction = this.session.BeginTransaction(isolationLevel);
            }
            else
            {
                this.transaction = session.Transaction;
            }
        }

        private int disposedFlag = 0;

        private void CheckIfDisposed()
        {
            if (this.disposedFlag != 0)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

#pragma warning disable 1591

        private bool commitRequested = false;

        public void Commit()
        {
            CheckIfDisposed();

            this.commitRequested = true;

            if (!this.isTransactionOwner)
            {
                return;
            }

            try
            {
                transaction.Commit();
            }
            catch (StaleObjectStateException e)
            {
                throw new Itb.DalCore.Exceptions.StaleObjectException(e.EntityName, e.Identifier, e.Message, e);
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CheckIfDisposed();

            this.commitRequested = true;

            if (!this.isTransactionOwner)
            {
                return;
            }

            try
            {
                await transaction.CommitAsync(cancellationToken);
            }
            catch (StaleObjectStateException e)
            {
                throw new Itb.DalCore.Exceptions.StaleObjectException(e.EntityName, e.Identifier, e.Message, e);
            }
        }

        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.disposedFlag, 1, 0) != 0)
            {
                return;
            }

            if ((this.isTransactionOwner && !transaction.WasCommitted && !transaction.WasRolledBack)
                || (!this.isTransactionOwner && !this.commitRequested))
            {
                try
                {
                    this.transaction.Rollback();
                }
                catch (TransactionException)
                {
                    // HACK: transaction may be rolled back on DBMS side, so do nothing
                }
                catch (ObjectDisposedException)
                {
                    // HACK: transaction can be already disposed, so do nothing
                }
            }

            if (this.isTransactionOwner)
            {
                this.transaction.Dispose();
            }

            this.transaction = null;

            CurrentSessionContext.Unbind(session.SessionFactory);

            var uowSession = session as SessionProvider.SessionAdapter;
            if (uowSession != null)
            {
                uowSession.AllowDisposeOriginalSession(true);
            }

            session.Dispose();
        }

#pragma warning restore 1591
    }
}