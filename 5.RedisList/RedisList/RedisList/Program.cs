using StackExchange.Redis;
using System;
using System.Linq;

namespace RedisList
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;

            var listKey = "listKey";

            redis.KeyDelete(listKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "a");

            var len = redis.ListLength(listKey);
            Console.WriteLine(len); //output  is 1

            redis.ListRightPush(listKey, "b");

            Console.WriteLine(redis.ListLength(listKey)); //putput is 2

            //lets clear it out
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);
          
            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue) x.ToString()).ToArray());

            Console.WriteLine(redis.ListLength(listKey)); //output is 26

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is abcdefghijklmnopqrstuvwxyz

            var lastFive = redis.ListRange(listKey, -5);

            Console.WriteLine(string.Concat(lastFive)); //output vwxyz

            var firstFive = redis.ListRange(listKey, 0, 4);

            Console.WriteLine(string.Concat(firstFive)); //output abcde

            redis.ListTrim(listKey, 0, 1);

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output ab

            //lets clear it out
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue)x.ToString()).ToArray());

            var firstElement = redis.ListLeftPop(listKey);

            Console.WriteLine(firstElement); //output a, list is now bcdefghijklmnopqrstuvwxyz

            var lastElement = redis.ListRightPop(listKey);

            Console.WriteLine(lastElement); //output z, list is now bcdefghijklmnopqrstuvwxy

            redis.ListRemove(listKey, "c");                               

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is bdefghijklmnopqrstuvwxy   
            
            redis.ListSetByIndex(listKey, 1, "c");

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is bcefghijklmnopqrstuvwxy   

            var thirdItem = redis.ListGetByIndex(listKey, 3);

            Console.WriteLine(thirdItem); //output f  

            //lets clear it out
            var destinationKey = "destinationList";
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);
            redis.KeyDelete(destinationKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue)x.ToString()).ToArray());

            var listLength = redis.ListLength(listKey);

            for (var i = 0; i < listLength ; i++)
            {
                var val = redis.ListRightPopLeftPush(listKey, destinationKey);

                Console.Write(val);    //output zyxwvutsrqponmlkjihgfedcba
            }
           
            Console.ReadKey();
        }
    }
}
