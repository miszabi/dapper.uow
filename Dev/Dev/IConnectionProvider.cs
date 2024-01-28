
using System.Data;
using System.Data.SqlClient;

namespace Dev;

public interface IConnectionProvider
{
    IDbConnection CreateConnection();   
}

public class SQLConnectionProvider : IConnectionProvider
{
    private readonly AppConfig _appConfig;
    public SQLConnectionProvider(AppConfig config)
    {
        _appConfig = config;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_appConfig.ConnectionString);
    }
}
