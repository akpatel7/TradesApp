using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODataService
{
    public class DataAccess : IDisposable
    {

        string _connectionString;
        SqlConnection _connection;
        SqlTransaction _transaction;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccess()
        {
            _connectionString = Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["BCATradeEntities"]);
            _connection = new SqlConnection();
            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            CloseConnection();
        }

        /// <summary>
        /// Close Connection
        /// </summary>
        private void CloseConnection()
        {
            if (_transaction != null)
                _transaction.Dispose();

            _connection.Close();
            _connection.Dispose();
        }

        /// <summary>
        /// Execute Data Reader
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = _connection;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            return sqlDataReader;
        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void ExecuteNonQuery(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = _connection;
            if (_transaction != null)
                sqlCommand.Transaction = _transaction;

            sqlCommand.ExecuteNonQuery();

        }

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="sqlCommand"></param>
        public int ExecuteScalar(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = _connection;
            if (_transaction != null)
                sqlCommand.Transaction = _transaction;

            int returnValue = int.Parse(sqlCommand.ExecuteScalar().ToString());

            return returnValue;

        }

        /// <summary>
        /// Begin Transaction
        /// </summary>
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public void CommitTransaction()
        {
            _transaction.Commit();
        }

    }
}
