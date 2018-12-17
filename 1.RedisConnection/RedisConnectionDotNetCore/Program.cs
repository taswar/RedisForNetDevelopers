using System;

namespace RedisConnectionDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
           var redis = RedisStore.RedisCache;

            if (redis.StringSet("testKey", "testValue"))
            {
                var val = redis.StringGet("testKey");

                Console.WriteLine(val);
            }

            Console.ReadKey();
        }
    }
}
