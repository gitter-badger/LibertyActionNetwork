namespace Li.Lan.Common.Data
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionStringProvider(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private string ConnectionString { get; set; }

        public string GetConnectionString()
        {
            return this.ConnectionString;
        }
    }
}