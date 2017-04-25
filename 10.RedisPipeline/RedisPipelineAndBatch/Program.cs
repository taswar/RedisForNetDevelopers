using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisPipelineAndBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;
            
            RedisKey alphaKey = "alphaKey";
            RedisKey betaKey = "betaKey";

            redis.KeyDelete(alphaKey, CommandFlags.FireAndForget);
            redis.KeyDelete(betaKey, CommandFlags.FireAndForget);

            var writeTask = redis.StringSetAsync(alphaKey, "abc");
            var writeBetaTask = redis.StringSetAsync(betaKey, "beta");

            var readTask = redis.StringGetAsync(alphaKey);

            redis.Wait(writeTask);

            var readValue = redis.Wait(readTask);

            Console.WriteLine($"Redis Task wait and read {readValue}");

            writeBetaTask.Wait();

            readValue = redis.StringGet(betaKey);

            Console.WriteLine($"Task wait and read {readValue}");


            //Batching
            var list = new List<Task<bool>>();
            var keys = new List<RedisKey> {alphaKey, betaKey};
            IBatch batch = redis.CreateBatch();

            //add the delete into batch
            batch.KeyDeleteAsync(alphaKey);

            foreach (var key in keys)
            {
                var task = batch.StringSetAsync(key, "123");
                list.Add(task);
            }

            batch.Execute();

            Task.WhenAll(list.ToArray());

            readTask = redis.StringGetAsync(alphaKey);
            readValue = redis.Wait(readTask);
            Console.WriteLine($"Alpha read value {readValue}");


            readTask = redis.StringGetAsync(betaKey);
            readValue = redis.Wait(readTask);
            Console.WriteLine($"Beta read value {readValue}");


            Console.ReadKey();
        }
    }
}
