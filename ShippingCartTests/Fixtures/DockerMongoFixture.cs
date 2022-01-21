using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.Mapping;

namespace ShippingCartTests.Fixtures
{
    public class DockerMongoFixture : IDisposable
    {
        private const string ImageName = "mongo_test";
        private const string MongoInPort = "27017";
        public const string MongoOutPort = "1111";
        public static readonly string ConnectionString = $"mongodb://localhost:{MongoOutPort}";
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);
        private Process _process;
        public IMapper Mapper { get; }

        public ShoppingCartDatabaseSettings GetDatabaseSettings() => new()
        {
            DatabaseName = "ShoppingCartDb",
            ConnectionString = ConnectionString,
            CollectionName = "ShoppingCart",
        };

        public DockerMongoFixture()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            Mapper = config.CreateMapper();

            _process = Process.Start("docker", $"run --name {ImageName} -p {MongoOutPort}:{MongoInPort} mongo");
            var started = WaitForMongoDbConnection(ConnectionString, "admin");
            if (!started)
                throw new Exception(
                    $"Startup failed, could not get MongoDB connection after trying for '{TestTimeout}'");
        }

        public void Dispose()
        {
            Console.Out.WriteLine("Dispose called");
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }

            var processStop = Process.Start("docker", $"stop {ImageName}");
            processStop?.WaitForExit();
            var processRm = Process.Start("docker", $"rm {ImageName}");
            processRm?.WaitForExit();
        }

        private static bool WaitForMongoDbConnection(string connectionString, string dbName)
        {
            Console.Out.WriteLine("Waiting for Mongo to respond");
            var probeTask = Task.Run(() =>
            {
                var isAlive = false;
                var client = new MongoClient(connectionString);

                for (var i = 0; i < 3000; i++)
                {
                    client.GetDatabase(dbName);
                    var server = client.Cluster.Description.Servers.FirstOrDefault();
                    isAlive = server != null &&
                              server.HeartbeatException == null &&
                              server.State == MongoDB.Driver.Core.Servers.ServerState.Connected;

                    if (isAlive)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                return isAlive;
            });
            probeTask.Wait();
            return probeTask.Result;
        }
    }
}
