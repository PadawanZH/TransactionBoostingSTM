using System.Collections.Generic;
using System.Threading;
using NUnit.Framework.Internal;
using TransactionBoosting;
using Logger = TransactionBoosting.Util.Logger;

namespace TransactionBoostingSTM
{
    
    public abstract class BaseContainer
    {
        public string name;
        public delegate bool LogItemDelegete();
        
        protected ThreadLocal<List<LogItemDelegete>> undoLog;
        protected ILock locks;
        
        BaseContainer(string name)
        {
            this.name = name;
            undoLog = new ThreadLocal<List<LogItemDelegete>>(() => new List<LogItemDelegete>());
        }

        public void TxStart()
        {
            undoLog.Value.Clear();
        }

        public void TxAbort()
        {
            undoLog.Value.Reverse();
            foreach (var op in undoLog.Value)
            {
                if (!op.Invoke())
                {
                    Logger.log(Logger.LogLevel.Error, "Failed abortion during execution");
                }
            }
            undoLog.Dispose();
            locks.ReleaseLocksOfThread();   
        }

        public void TxCommit()
        {
            undoLog.Dispose();
            locks.ReleaseLocksOfThread();
        }

    }
}