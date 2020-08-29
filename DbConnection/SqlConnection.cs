using System;
using System.Threading.Tasks;

namespace DbConnection
{
    public class SqlConnection : DbConnection
    {
        public SqlConnection(string connectionString)
            : base(connectionString)
        {
        }

        public SqlConnection(string connectionString, double timeoutInSeconds)
            : base(connectionString, timeoutInSeconds)
        {

        }

        public override bool Open()
        {
            if (Timeout == TimeSpan.Zero)
            {
                Console.WriteLine($"SqlConnection opened with connectionString: {ConnectionString}");
                return true;
            }
            var task = Task.Run(() => Console.WriteLine($"SqlConnection opened with connectionString: {ConnectionString}"));
            if (task.Wait(Timeout))
                return true;
            else
                throw new TimeoutException("Timed out");            
        }

        public override void Close()
        {
            Console.WriteLine($"SqlConnection closed with connectionString: {ConnectionString}");
        }

    }


}
