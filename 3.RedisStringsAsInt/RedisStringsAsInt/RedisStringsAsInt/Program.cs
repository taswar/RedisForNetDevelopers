using System;

namespace RedisStringsAsInt
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;
            var number = 101;
            var intKey = "intKey";

            if (redis.StringSet(intKey, number))
            {
                //redis incr command
                var result = redis.StringIncrement(intKey); //after operation Our int number is now 102
                Console.WriteLine(result);

                //incrby command
                var newNumber = redis.StringIncrement(intKey, 100); // we now have incremented by 100, thus the new number is 202
                Console.WriteLine(newNumber);               

                redis.KeyDelete("zeroValueKey");
                //by default redis stores a value of zero if no value is provided           
                var zeroValue = (int)redis.StringGet("zeroValueKey");
                Console.WriteLine(zeroValue);

                var someValue = (int)redis.StringIncrement("zeroValueKey"); //someValue is now 1 since it was incremented
                Console.WriteLine(someValue);
                
                //decr command
                redis.StringDecrement("zeroValueKey");
                someValue = (int)redis.StringGet("zeroValueKey"); //now someValue is back to 0   
                Console.WriteLine(someValue);

                //decrby command
                someValue = (int)redis.StringDecrement("zeroValueKey", 99); // now someValue is -99   
                Console.WriteLine(someValue);
                
                //append command
                redis.StringAppend("zeroValueKey", 1);
                someValue = (int)redis.StringGet("zeroValueKey"); //"Our zeroValueKey number is now -991   
                Console.WriteLine(someValue);

                redis.StringSet("floatValue", 1.1);
                var floatValue = (float)redis.StringIncrement("floatValue", 0.1); //fload value is now 1.2   
                Console.WriteLine(floatValue);
            }

            Console.ReadKey();
        }
    }
}
