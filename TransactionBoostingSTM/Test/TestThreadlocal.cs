using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionBoosting.Test
{
    public class LocalVal
    {
        public ThreadLocal<List<string>> threadString;

        public LocalVal()
        {
            threadString = new ThreadLocal<List<string>>(() =>
            {
                List<string> list = new List<string>();
                list.Add(Thread.CurrentThread.ManagedThreadId.ToString());
                return list;
            });
        }

        public void print()
        {
            Console.WriteLine("This is Thread {0}, and my list size is {1}, val is: {2}", Thread.CurrentThread.ManagedThreadId, threadString.Value.Count, threadString.Value[0]);
        }
    }
    
    public class TestThreadlocal
    {
        public void test()
        {
            LocalVal local = new LocalVal();
            Action action = () =>
            {
                local.print();
            };
            
            Parallel.Invoke(action, action, action, action, action, action, action, action);
            
            local.threadString.Dispose();
        }
    }
}