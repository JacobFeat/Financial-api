using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FinancialApi.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            return _dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            return _dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            return _dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            return _dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, DynamicParameters parameters)
        {
            return _dbConnection.Execute(sql, parameters) > 0;
        }

        public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters)
        {
            return _dbConnection.Query<T>(sql, parameters);
        }

        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
        {
            return _dbConnection.QuerySingle<T>(sql, parameters);
        }
    }
}