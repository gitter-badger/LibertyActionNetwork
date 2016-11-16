using Li.Lan.Common.Data;
using Li.Lan.Common.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Li.Lan.Data
{
    public class LoggingRepository : ILoggingRepository
    {
        public LoggingRepository(IConnectionStringProvider connectionStringProvider)
        {
            this.ConnectionStringProvider = connectionStringProvider;
        }

        private IConnectionStringProvider ConnectionStringProvider { get; set; }

        public Log StoreLog(Log log)
        {
            log.StoredOnUtc = DateTime.UtcNow;

            var connectionString = this.GetConnectionString();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("dbo.InsertLog", sqlConnection) { CommandType = CommandType.StoredProcedure };

                // Set up parameters
                cmd.Parameters.Add("@LogGuid", SqlDbType.UniqueIdentifier).Value = log.LogGuid;
                cmd.Parameters.Add("@CreatedOnUtc", SqlDbType.DateTime).Value = log.CreatedOnUtc;
                cmd.Parameters.Add("@StoredOnUtc", SqlDbType.DateTime).Value = log.StoredOnUtc;
                cmd.Parameters.Add("@ApplicationVersion", SqlDbType.Char, 14).Value = log.ApplicationVersion;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = log.UserId;
                cmd.Parameters.Add("@SessionId", SqlDbType.UniqueIdentifier).Value = log.SessionId;
                cmd.Parameters.Add("@ActionId", SqlDbType.UniqueIdentifier).Value = log.ActionId;
                cmd.Parameters.Add("@Tag", SqlDbType.NVarChar, 200).Value = log.Tag;
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 200).Value = log.Category;
                cmd.Parameters.Add("@SubCategory", SqlDbType.NVarChar, 200).Value = log.SubCategory;
                cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = log.Message;
                cmd.Parameters.Add("@LogType", SqlDbType.TinyInt).Value = log.LogType;

                cmd.Parameters.Add("@UserHostAddress", SqlDbType.VarChar, 50).Value = log.UserHostAddress;

                // Stored Procedure return value
                var returnParameter = cmd.Parameters.Add("@LogId", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                sqlConnection.Open();

                cmd.ExecuteNonQuery();

                log.LogId = (int)returnParameter.Value;
            }

            return log;
        }

        private string GetConnectionString()
        {
            return this.ConnectionStringProvider.GetConnectionString();
        }
    }
}