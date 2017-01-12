using StackExchange.Redis;
using System;

namespace RedisHyperLogLog
{
    class Program
    {
        static void Main(string[] args)
        {

            var redis = RedisStore.RedisCache;

            RedisKey key = "setKey";
            RedisKey alphaKey = "alphaKey";            
            RedisKey destinationKey = "destKey";

            redis.KeyDelete(key, CommandFlags.FireAndForget);
            redis.KeyDelete(alphaKey, CommandFlags.FireAndForget);
            redis.KeyDelete(destinationKey, CommandFlags.FireAndForget);

            redis.HyperLogLogAdd(key, "a");
            redis.HyperLogLogAdd(key, "b");
            redis.HyperLogLogAdd(key, "c");

            Console.WriteLine(redis.HyperLogLogLength(key)); //output 3


            redis.HyperLogLogAdd(alphaKey, "1");
            redis.HyperLogLogAdd(alphaKey, "2");
            redis.HyperLogLogAdd(alphaKey, "3");


            Console.WriteLine(redis.HyperLogLogLength(alphaKey)); //output 3


            redis.HyperLogLogAdd(alphaKey, "a");

            Console.WriteLine(redis.HyperLogLogLength(alphaKey)); //output 4


            redis.HyperLogLogMerge(destinationKey, key, alphaKey);


            Console.WriteLine(redis.HyperLogLogLength(destinationKey)); //output 6 which is (a, b, c, 1, 2, 3)

            Console.ReadKey();
        }
    }
}
