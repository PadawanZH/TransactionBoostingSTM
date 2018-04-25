using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;
using TransactionBoosting.Util;
using TransactionBoostingSTM;

namespace TransactionBoosting
{
    public class LockKey : ILock
    {
        private ConcurrentDictionary<long, Mutex> lockDictionary;        //dictionary of <value, lockOfValue>
        private ConcurrentDictionary<int, HashSet<Mutex>> lockOfThread;    //directory of <threadID, lockHoldByThread>

        public LockKey()
        {
            lockDictionary = new ConcurrentDictionary<long, Mutex>();
            lockOfThread = new ConcurrentDictionary<int, HashSet<Mutex>>();
        }

        public void Lock(long key)
        {
            var valueLock = lockDictionary.GetOrAdd(key, new Mutex());
            var threadid = Thread.CurrentThread.ManagedThreadId;

            var lockSet = lockOfThread.GetOrAdd(threadid, (k) => new HashSet<Mutex>());

            if (lockSet.Add(valueLock)) //lockSet is thread-local, just access it unprotected
            {
                if (valueLock.WaitOne(1))
                {
                    Logger.log(Logger.LogLevel.Info, "locked  on the value: {0}", key);
                }
                else
                {
                    Logger.log(Logger.LogLevel.Warn, "failed to lock value: {0}, tx abort", key);
                    lockSet.Remove(valueLock);
                    throw new TxAbortException();
                }
            }
            
        }

        public void Unlock(long key)
        {
            Mutex valueLock;
            if (lockDictionary.TryGetValue(key, out valueLock))
            {
                valueLock.ReleaseMutex();
            }
            else
            {
                Logger.log(Logger.LogLevel.Error, "Try to unlock a non-locked value: {0}", key);
            }
        }

        /// <summary>
        /// Release the locks held by the calling thread
        /// </summary>
        public void ReleaseLocksOfThread()
        {
            if (lockOfThread.TryRemove(Thread.CurrentThread.ManagedThreadId, out var lockSet))
            {
                try
                {
                    foreach (var valueLock in lockSet)
                    {
                        valueLock.ReleaseMutex();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                lockSet.Clear();
            }
            else
            {
                Logger.log(Logger.LogLevel.Error, "No lockset record when try to release the locks");
            }
        }
    }
}