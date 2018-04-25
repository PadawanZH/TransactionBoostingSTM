namespace TransactionBoosting
{
    public interface ILock
    {
        void ReleaseLocksOfThread();
    }
}