using System;
using Xunit;
using FluentAssertions;
using System.Reflection;
using System.IO;
using System.Text;

namespace DbConnection.tests
{
    public class UnitTest1
    {
        [Fact]
        public void DbConnection_is_abstract_class()
        {
            typeof(DbConnection).Should().BeAbstract();
        }

        [Fact]
        public void DbConnection_has_properties_connectionString_and_timeout()
        {
            var result = typeof(DbConnection).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            result.Should().Contain(x => x.Name == "ConnectionString");
            result.Should().Contain(x => x.Name == "Timeout");
        }

        [Fact]
        public void DbConnection_has_abstract_methods_Open_and_Close()
        { 
            var result = typeof(DbConnection).GetMethods();

            result.Should().Contain(x => x.Name == "Open").Which.ReflectedType.Should().BeAbstract();
            result.Should().Contain(x => x.Name == "Close").Which.ReflectedType.Should().BeAbstract();
        }

        [Fact]
        public void SqlConnection_Open_passes_connectionString_to_Console()
        {
            var connection = new SqlConnection("connect123");

            var expected = GetOutputFor(connection.Open);

            expected.Should().Contain("SqlConnection opened");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void SqlConnection_Open_throws_exception_if_connectionString_is_NullOrEmpty()
        {
            Exception result = new Exception();
            try
            {
                var connection = new SqlConnection(null);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            result.Should().BeOfType<NullReferenceException>();
            result.Message.Should().Contain("ConnectionString cannot be null or empty");
        }
        
        [Fact]
        public void SqlConnection_Close_passes_connectionString_to_Console()
        {
            var connection = new SqlConnection("connect123");

            var expected = GetOutputFor(connection.Close);

            expected.Should().Contain("SqlConnection closed");
            expected.Should().Contain("connect123");
        }

        [Fact]
        public void SqlConnection_Open_throws_exception_if_exceeds_timeout()
        {
            var connection = new SqlConnection("connect123", 0.0000001);
            var connection2 = new SqlConnection("connect456", 100);

            connection.Invoking(x => x.Open()).Should().Throw<TimeoutException>().WithMessage("Timed out");
            connection2.Invoking(x => x.Open()).Should().NotThrow<TimeoutException>();
        }

        [Fact]
        public void OracleConnection_Open_passes_connectionString_to_Console()
        {
            var connection = new OracleConnection("connect123", 100);

            var expected = GetOutputFor(connection.Open);

            expected.Should().Contain("OracleConnection opened");
            expected.Should().Contain("connect123");
        }

        [Fact]
        public void OracleConnection_Open_throws_exception_if_connectionString_is_NullOrEmpty()
        {
            Exception result = new Exception();
            try
            {
                var connection = new OracleConnection(null);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            result.Should().BeOfType<NullReferenceException>();
            result.Message.Should().Contain("ConnectionString cannot be null or empty");
        }

        [Fact]
        public void OracleConnection_Close_passes_connectionString_to_Console()
        {
            var connection = new OracleConnection("connect123");

            var expected = GetOutputFor(connection.Close);

            expected.Should().Contain("OracleConnection closed");
            expected.Should().Contain("connect123");
        }

        [Fact]
        public void OracleConnection_Open_throws_exception_if_exceeds_timeout()
        {
            var connection = new OracleConnection("connect123", 0.0000001);
            var connection2 = new OracleConnection("connect456", 0.1);

            connection.Invoking(x => x.Open()).Should().Throw<TimeoutException>().WithMessage("Timed out");
            connection2.Invoking(x => x.Open()).Should().NotThrow<TimeoutException>();
        }

        [Fact]
        public void DbCommand_throws_exception_if_DbConnection_is_NullOrEmpty()
        {
            Exception result = new Exception();
            try
            {
                var command = new DbCommand(null);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            result.Should().BeOfType<NullReferenceException>();
            result.Message.Should().Contain("DbConnection cannot be null");
        }

        [Fact]
        public void DbCommand_Execute_Opens_OracleConnection()
        {
            var connection = new OracleConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("OracleConnection opened");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void DbCommand_Execute_Opens_SqlConnection()
        {
            var connection = new SqlConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("SqlConnection opened");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void DbCommand_Execute_Closes_OracleConnection()
        {
            var connection = new OracleConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("OracleConnection closed");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void DbCommand_Execute_Closes_SqlConnection()
        {
            var connection = new SqlConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("SqlConnection closed");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void DbCommand_Execute_sends_command_to_SqlDb()
        {
            var connection = new SqlConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("SqlConnection opened");
            expected.Should().Contain("command find sent to DbConnection.SqlConnection");
            expected.Should().Contain("SqlConnection closed");
            expected.Should().Contain("connect123");
        }
        
        [Fact]
        public void DbCommand_Execute_sends_command_to_OracleDb()
        {
            var connection = new OracleConnection("connect123");
            var command = new DbCommand(connection);

            var expected = GetOutputFor(command.Execute, "find");

            expected.Should().Contain("OracleConnection opened");
            expected.Should().Contain("command find sent to DbConnection.OracleConnection");
            expected.Should().Contain("OracleConnection closed");
            expected.Should().Contain("connect123");
        }

        private static string GetOutputFor(Action<string> method, string input)
        {
            string expected;
            using (var stream = new MemoryStream())
            {
                using (var logger = new StreamWriter(stream))
                {
                    Console.SetOut(logger);
                    method.Invoke(input);
                }
                expected = Encoding.UTF8.GetString(stream.ToArray());
            }
            return expected;
        }
        
        private static string GetOutputFor(Action method)
        {
            string expected;
            using (var stream = new MemoryStream())
            {
                using (var logger = new StreamWriter(stream))
                {
                    Console.SetOut(logger);
                    method.Invoke();
                }
                expected = Encoding.UTF8.GetString(stream.ToArray());
            }
            return expected;
        }
        
        private static string GetOutputFor(Func<bool> method)
        {
            string expected;
            using (var stream = new MemoryStream())
            {
                using (var logger = new StreamWriter(stream))
                {
                    Console.SetOut(logger);
                    method.Invoke();
                }
                expected = Encoding.UTF8.GetString(stream.ToArray());
            }
            return expected;
        }

    }
}
