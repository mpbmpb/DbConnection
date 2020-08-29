using System;

namespace DbConnection
{
    public class DbCommand
    {
        private DbConnection _connection;

        public DbCommand(DbConnection connection)
        {
            if (connection == null) throw new NullReferenceException("DbConnection cannot be null");
            _connection = connection;
        }

        public void Execute(string command)
        {
            _connection.Open();
            Console.WriteLine();
            Console.WriteLine($"command {command} sent to {_connection.GetType()}");
            Console.WriteLine();
            _connection.Close();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
