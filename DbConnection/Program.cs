using System;

namespace DbConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlConnection = new SqlConnection("connect123", 100);
            var oracleConnection = new OracleConnection("connect456", 100);
            var sqlCommand = new DbCommand(sqlConnection);
            var oracleCommand = new DbCommand(oracleConnection);

            sqlCommand.Execute("find");
            oracleCommand.Execute("find");
        }
    }
}
