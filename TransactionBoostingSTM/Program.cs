using System;
using TransactionBoosting.Test;

namespace TransactionBoostingSTM
{
    class Program
    {
        static void Main(string[] args)
        {
            TestThreadlocal t = new TestThreadlocal();
            t.test();
        }
    }
}