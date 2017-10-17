using StackExchange.Redis;
using System;

namespace RedisTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;

            RedisKey alphaKey = "alphaKey";
            RedisKey betaKey = "betaKey";
            RedisKey gammaKey = "gammaKey";

            redis.KeyDelete(alphaKey, CommandFlags.FireAndForget);
            redis.KeyDelete(betaKey, CommandFlags.FireAndForget);
            redis.KeyDelete(gammaKey, CommandFlags.FireAndForget);

            var trans = redis.CreateTransaction();

            var incr = trans.StringSetAsync(alphaKey, "abc");
            var exec = trans.ExecuteAsync();

            var result = redis.Wait(exec);
            var alphaValue = redis.StringGet(alphaKey);

            Console.WriteLine($"Alpha key is {alphaValue} and result is {result}");
            
            //using conditions to watch keys
            var condition = trans.AddCondition(Condition.KeyNotExists(gammaKey));
            var keyIncrement = trans.StringIncrementAsync(gammaKey);
            exec = trans.ExecuteAsync();
            result = redis.Wait(exec);

            var gammaValue = redis.StringGet(gammaKey);

            Console.WriteLine($"Gamma key is {gammaValue} and result is {result}");

            //fail condition
            condition = trans.AddCondition(Condition.KeyNotExists(gammaKey));
            keyIncrement = trans.StringIncrementAsync(gammaKey);
            exec = trans.ExecuteAsync();

            //resultis false 
            result = redis.Wait(exec);

            gammaValue = redis.StringGet(gammaKey);

            //value is still 1 and result is false
            Console.WriteLine($"Gamma key is {gammaValue} and result is {result}");

            Console.ReadKey();
        }
    }
}
