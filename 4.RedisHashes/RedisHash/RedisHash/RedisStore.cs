using System;
using System.Configuration;
using StackExchange.Redis;

namespace RedisHash
{
    public class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { ConfigurationManager.AppSettings["redis.connection"] }
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();
    }
}