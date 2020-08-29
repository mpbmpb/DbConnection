using System;
namespace DbConnection
{
    public abstract class DbConnection
    {
        protected readonly string ConnectionString;
        protected readonly TimeSpan Timeout = new TimeSpan();

        public DbConnection(string connectionString)
        {
            if (connectionString == null || connectionString == "")
                throw new NullReferenceException("ConnectionString cannot be null or empty");
            ConnectionString = connectionString;
        }

        public DbConnection(string connectionString, double timeout)
            : this(connectionString)
        {
            int ticksPerSecond = 10000000;
            Timeout = TimeSpan.FromTicks((long)(timeout * ticksPerSecond));
        }
        public abstract bool Open();
        public abstract void Close();
    }
}
