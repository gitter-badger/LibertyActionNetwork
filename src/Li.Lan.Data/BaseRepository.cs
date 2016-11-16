using Li.Lan.Common.Data;
using System.Data;
using System.Data.SqlClient;

namespace Li.Lan.Data
{
    public class BaseRepository
    {
        public BaseRepository(IConnectionStringProvider connectionStringProvider)
        {
            this.ConnectionStringProvider = connectionStringProvider;
        }

        protected IConnectionStringProvider ConnectionStringProvider { get; set; }

        protected string EnsureWildcard(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                return null;

            return string.Format("%{0}%", parameter);
        }

        protected SqlCommand CreateSqlCommandStoredProcedure(string storedProcedureName, SqlConnection sqlConnection)
        {
            var cmd = new SqlCommand(storedProcedureName, sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        protected string GetConnectionString()
        {
            return this.ConnectionStringProvider.GetConnectionString();
        }

        protected SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(this.GetConnectionString());
        }

        protected LanContext CreateContext()
        {
            return new LanContext(this.GetConnectionString());
        }
    }
}