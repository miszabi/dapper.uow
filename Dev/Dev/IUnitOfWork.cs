using System.Data;

namespace Dev
{
    public interface IUnitOfWork: IDisposable, IAsyncDisposable
    {
        public void Start(Action<ConnectionContext, IsolationLevel> action);
        public Task StartAsync(Action<ConnectionContext, IsolationLevel> action);
        public void Commit();
        public void Rollback();
        public Task CommitAsync();
        public Task RollbackAsync();
        public Task DisposeAsync();
    }
}
