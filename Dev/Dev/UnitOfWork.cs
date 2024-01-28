using System.Data;

namespace Dev;

public class UnitOfWork : IUnitOfWork
{
    private readonly ConnectionContext _connectionContext;
    private readonly IConnectionProvider _connectionProvider;
    private bool _transactionWasComitted;
    private bool _transactionWasRolledBack;

    private IsolationLevel _isolationLevel;
    public UnitOfWork(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        _connectionContext = new ConnectionContext();
    }

    public void Start(Action<ConnectionContext, IsolationLevel> action) 
    {
        action(_connectionContext, _isolationLevel);
        Open();
    }

    public async Task StartAsync(Action<ConnectionContext, IsolationLevel> action)
    {
        action(_connectionContext, _isolationLevel);
        Open();

        await Task.CompletedTask;
    }

    public void Commit() 
    {
        _connectionContext.Transaction!.Commit();
        _transactionWasComitted = true;
    }
    public void Rollback() 
    {
        _connectionContext.Transaction!.Rollback();
    
           _transactionWasRolledBack = true;
    }
    public void Dispose() 
    {
        if(!_transactionWasComitted && !_transactionWasRolledBack)
        {
            _connectionContext.Transaction?.Commit();
        }            

        _connectionContext.Transaction?.Dispose();
        
        if (_connectionContext.Connection != null && _connectionContext.Connection.State == ConnectionState.Open)
        {
            _connectionContext.Connection.Close();
        }

        _connectionContext.Connection?.Dispose();
    }

    public async Task CommitAsync() { await Task.FromResult(true); }
    public async Task RollbackAsync() { await Task.FromResult(true); }
    public async Task DisposeAsync() { await Task.FromResult(true); }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await Task.FromResult(false);    
    }

    void Open() 
    {
        _connectionContext.Connection = _connectionProvider.CreateConnection();
        _connectionContext.Connection.Open();

        _connectionContext.Transaction = _connectionContext.Connection.BeginTransaction(_isolationLevel);
    }        
}
