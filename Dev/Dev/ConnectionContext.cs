using System.Data;

namespace Dev
{
    public class ConnectionContext
    {
        public IDbConnection? Connection { get; set; }

        public IDbTransaction? Transaction { get; set; }
    }
}
