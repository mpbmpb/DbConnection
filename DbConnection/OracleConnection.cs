using System;
using System.Threading.Tasks;

namespace DbConnection
{
    public class OracleConnection : DbConnection
    {
        public OracleConnection(string connectionString)
            : base(connectionString)
        {
        }

        public OracleConnection(string connectionString, double timeoutInSeconds)
            : base(connectionString, timeoutInSeconds)
        {

        }

        public override bool Open()
        {
            if (Timeout == TimeSpan.Zero)
            {
                Console.WriteLine($"OracleConnection opened with connectionString: {ConnectionString}");
                return true;
            }
            var task = Task.Run(() => Console.WriteLine($"OracleConnection opened with connectionString: {ConnectionString}"));
            if (task.Wait(Timeout))
                return true;
            else
                throw new TimeoutException("Timed out");
        }

        public override void Close()
        {
            Console.WriteLine($"OracleConnection closed with connectionString: {ConnectionString}");
        }

    }


}
